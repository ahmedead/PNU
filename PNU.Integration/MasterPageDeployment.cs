using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Integration
{
    /// <summary>
    /// Deploys Master Pages and Page Layouts between SharePoint farms
    /// using a shared folder as the transfer medium.
    ///
    /// STEP 1 — Run on UAT server:
    ///   PNU.Integration.exe export-masterpages
    ///   PNU.Integration.exe export-pagelayouts
    ///   PNU.Integration.exe export-all-ui
    ///
    /// STEP 2 — Run on Production server (same shared folder):
    ///   PNU.Integration.exe import-masterpages
    ///   PNU.Integration.exe import-pagelayouts
    ///   PNU.Integration.exe import-all-ui
    ///
    /// App.config keys required:
    ///   <add key="RootSiteUrl"       value="http://..." />   <!-- already exists -->
    ///   <add key="ExportFolder"      value="\\server\share\MasterPageExport" />
    ///
    /// Optional filter (comma-separated file names; empty = ALL):
    ///   <add key="MasterPage_Filter" value="" />
    ///   <add key="PageLayout_Filter" value="" />
    /// </summary>
    public static class MasterPageDeployment
    {
        // ── configuration ────────────────────────────────────────────────────────

        private static string LocalSiteUrl =>
            System.Configuration.ConfigurationManager.AppSettings["RootSiteUrl"];

        private static string ExportFolder =>
            System.Configuration.ConfigurationManager.AppSettings["ExportFolder"]
            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterPageExport");

        // ── public entry points ──────────────────────────────────────────────────

        public static void ExportMasterPages() => Export(".master", "MasterPage_Filter");
        public static void ExportPageLayouts() => Export(".aspx", "PageLayout_Filter");
        public static void ExportAllUiArtifacts() { ExportMasterPages(); ExportPageLayouts(); }

        public static void ImportMasterPages() => Import(".master", "MasterPage_Filter");
        public static void ImportPageLayouts() => Import(".aspx", "PageLayout_Filter");
        public static void ImportAllUiArtifacts() { ImportMasterPages(); ImportPageLayouts(); }

        // ── EXPORT (run on UAT) ──────────────────────────────────────────────────

        private static void Export(string ext, string filterKey)
        {
            HashSet<string> filter = GetFilter(filterKey);
            string destDir = Path.Combine(ExportFolder, ext.TrimStart('.').ToUpper());

            OracleDBContext.LogToFile($"=== EXPORT '{ext}' → {destDir} ===");
            Directory.CreateDirectory(destDir);

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(LocalSiteUrl))
                using (SPWeb web = site.RootWeb)
                {
                    SPList gallery = web.GetCatalog(SPListTemplateType.MasterPageCatalog);
                    SPFolder rootFolder = gallery.RootFolder;

                    ExportFolder_Recursive(rootFolder, ext, filter, destDir);
                }
            });

            OracleDBContext.LogToFile($"=== EXPORT '{ext}' DONE ===");
        }

        private static void ExportFolder_Recursive(
            SPFolder folder,
            string ext,
            HashSet<string> filter,
            string destDir)
        {
            foreach (SPFile file in folder.Files)
            {
                try
                {
                    if (!file.Name.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (filter.Count > 0 && !filter.Contains(file.Name))
                        continue;

                    byte[] bytes = file.OpenBinary();
                    string filePath = Path.Combine(destDir, file.Name);
                    File.WriteAllBytes(filePath, bytes);

                    OracleDBContext.LogToFile($"  [EXPORTED] {file.Name}  ({bytes.Length} bytes)");
                }
                catch (Exception ex)
                {
                    OracleDBContext.LogToFile($"  [EXPORT FAIL] {file.Name}: {ex.Message}");
                }
            }

            foreach (SPFolder sub in folder.SubFolders)
            {
                if (sub.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ExportFolder_Recursive(sub, ext, filter, destDir);
            }
        }

        // ── IMPORT (run on Production) ───────────────────────────────────────────

        private static void Import(string ext, string filterKey)
        {
            HashSet<string> filter = GetFilter(filterKey);
            string sourceDir = Path.Combine(ExportFolder, ext.TrimStart('.').ToUpper());

            OracleDBContext.LogToFile($"=== IMPORT '{ext}' ← {sourceDir} ===");

            if (!Directory.Exists(sourceDir))
            {
                OracleDBContext.LogToFile($"  [ERROR] Export folder not found: {sourceDir}");
                return;
            }

            string[] files = Directory.GetFiles(sourceDir, "*" + ext, SearchOption.TopDirectoryOnly);
            OracleDBContext.LogToFile($"  Found {files.Length} file(s) to import.");

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(LocalSiteUrl))
                using (SPWeb web = site.RootWeb)
                {
                    web.AllowUnsafeUpdates = true;

                    SPList gallery = web.GetCatalog(SPListTemplateType.MasterPageCatalog);
                    SPFolder destFolder = gallery.RootFolder;

                    foreach (string filePath in files)
                    {
                        string fileName = Path.GetFileName(filePath);

                        if (filter.Count > 0 && !filter.Contains(fileName))
                            continue;

                        try
                        {
                            byte[] content = File.ReadAllBytes(filePath);
                            UploadFile(destFolder, fileName, content, web, gallery);
                            OracleDBContext.LogToFile($"  [IMPORTED] {fileName}");
                        }
                        catch (Exception ex)
                        {
                            OracleDBContext.LogToFile($"  [IMPORT FAIL] {fileName}: {ex.Message}");
                        }
                    }

                    web.AllowUnsafeUpdates = false;
                }
            });

            OracleDBContext.LogToFile($"=== IMPORT '{ext}' DONE ===");
        }

        private static void UploadFile(
            SPFolder destFolder,
            string fileName,
            byte[] content,
            SPWeb web,
            SPList gallery)
        {
            string destUrl = destFolder.ServerRelativeUrl.TrimEnd('/') + "/" + fileName;

            // Undo any stale checkout on existing file
            try
            {
                SPFile existing = web.GetFile(destUrl);
                if (existing.Exists && existing.CheckOutType != SPFile.SPCheckOutType.None)
                    existing.UndoCheckOut();
            }
            catch { /* file does not exist yet — that is fine */ }

            // Upload / overwrite
            SPFile uploaded = destFolder.Files.Add(destUrl, content, overwrite: true);
            uploaded.Item.Update();

            // Check in
            uploaded.CheckIn(
                $"Deployed from UAT by PNU.Integration on {DateTime.Now:yyyy-MM-dd HH:mm}",
                SPCheckinType.MajorCheckIn);

            // Publish if the library uses minor versioning
            if (gallery.EnableMinorVersions)
                uploaded.Publish("Published by PNU.Integration deployment.");

            // Approve if content approval is required
            if (gallery.EnableModeration)
                uploaded.Approve("Auto-approved by PNU.Integration deployment.");
        }

        // ── helpers ──────────────────────────────────────────────────────────────

        private static HashSet<string> GetFilter(string appSettingKey)
        {
            string raw = System.Configuration.ConfigurationManager.AppSettings[appSettingKey] ?? "";
            return new HashSet<string>(
                raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(s => s.Trim()),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
