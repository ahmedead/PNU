using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    /// <summary>
    /// Business layer for the Page Creator tool.
    /// Reads PageTemplates / PageLayoutsCatalog, gates access via PageCreatorAdmins,
    /// and creates publishing pages (optionally on both /ar/ and /en/ webs).
    /// Page creation follows the SAME sequence as SideMenuListProvisioner.EnsurePage
    /// (PageGenerator.CreatePublishingPage), using the existing ControlLoaderWebPart.
    /// </summary>
    public static class busclsPageCreator
    {
        // Default page layout when a template/layout supplies none
        private const string PAGE_LAYOUT_URL = "/_catalogs/masterpage/NewSideMenu.aspx";

        // ControlLoaderWebPart - same type/assembly used by PageGenerator.
        // Loaded via reflection so this file needs no compile-time reference.
        private const string WP_TYPE_NAME = "PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart";
        private const string WP_ASSEMBLY_NAME = "PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3";

        /// <summary>Web part zone on the DGA layouts - adjust if the layout differs.</summary>
        private const string WP_ZONE_ID = "TopZone";

        #region Security

        public static bool IsPageCreatorAdmin()
        {
            try
            {
                if (SPContext.Current.Web.CurrentUser == null) return false;
                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin) return true;

                string currentLogin = NormalizeLogin(SPContext.Current.Web.CurrentUser.LoginName);
                bool isAdmin = false;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    Guid webId = SPContext.Current.Web.ID;
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList admins = web.Lists.TryGetList(PageCreatorLists.PageCreatorAdmins);
                        if (admins == null) return;

                        foreach (SPListItem item in admins.Items)
                        {
                            string login = NormalizeLogin(SafeString(item, "UserLogin"));
                            if (!string.IsNullOrEmpty(login) &&
                                login.Equals(currentLogin, StringComparison.OrdinalIgnoreCase))
                            {
                                isAdmin = true;
                                break;
                            }
                        }
                    }
                });

                return isAdmin;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "busclsPageCreator.IsPageCreatorAdmin", ex.Message);
                return false;
            }
        }

        /// <summary>Strips claims prefix (i:0#.w|domain\user → domain\user).</summary>
        private static string NormalizeLogin(string login)
        {
            if (string.IsNullOrEmpty(login)) return string.Empty;
            int idx = login.LastIndexOf('|');
            return idx >= 0 ? login.Substring(idx + 1).Trim() : login.Trim();
        }

        #endregion

        #region Lookups

        public static List<clsPageTemplate> GetTemplates()
        {
            List<clsPageTemplate> result = new List<clsPageTemplate>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    Guid webId = SPContext.Current.Web.ID;
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList list = web.Lists.TryGetList(PageCreatorLists.PageTemplates);
                        if (list == null) return;

                        SPQuery query = new SPQuery
                        {
                            Query = @"<Where><Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq></Where>
                                      <OrderBy><FieldRef Name='Title' Ascending='TRUE'/></OrderBy>"
                        };

                        foreach (SPListItem item in list.GetItems(query))
                        {
                            result.Add(new clsPageTemplate
                            {
                                ID = item.ID,
                                TemplateName = SafeString(item, "Title"),
                                PageLayoutURL = SafeString(item, "PageLayoutURL"),
                                UserControlPath = SafeString(item, "UserControlPath"),
                                DefaultProperties = SafeString(item, "DefaultProperties"),
                                IsActive = SafeBool(item, "IsActive")
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "busclsPageCreator.GetTemplates", ex.Message);
            }
            return result;
        }

        public static List<clsPageLayout> GetLayouts()
        {
            List<clsPageLayout> result = new List<clsPageLayout>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    Guid webId = SPContext.Current.Web.ID;
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList list = web.Lists.TryGetList(PageCreatorLists.PageLayouts);
                        if (list == null) return;

                        SPQuery query = new SPQuery
                        {
                            Query = @"<Where><Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq></Where>
                                      <OrderBy><FieldRef Name='Title' Ascending='TRUE'/></OrderBy>"
                        };

                        foreach (SPListItem item in list.GetItems(query))
                        {
                            result.Add(new clsPageLayout
                            {
                                ID = item.ID,
                                Name = SafeString(item, "Title"),
                                PageLayoutURL = SafeString(item, "PageLayoutURL"),
                                IsActive = SafeBool(item, "IsActive")
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "busclsPageCreator.GetLayouts", ex.Message);
            }
            return result;
        }

        #endregion

        #region Page creation (wrapper around EnsurePage)

        /// <summary>
        /// Creates the page on the target web, and on the paired /ar/-/en/ web when requested.
        /// Returns a human-readable log (one line per action) for the results panel.
        /// </summary>
        public static List<string> CreatePages(clsPageCreateRequest req)
        {
            List<string> log = new List<string>();

            string pageName = NormalizePageName(req.PageName);
            if (string.IsNullOrEmpty(pageName))
            {
                log.Add("ERROR|Invalid page name.");
                return log;
            }

            string firstUrl = NormalizeWebUrl(req.WebSiteUrl);
            if (string.IsNullOrEmpty(firstUrl))
            {
                log.Add("ERROR|Invalid web site URL.");
                return log;
            }

            List<string> targets = new List<string> { firstUrl };

            if (req.AddToBothSites)
            {
                string paired = GetPairedWebUrl(firstUrl);
                if (!string.IsNullOrEmpty(paired) && !paired.Equals(firstUrl, StringComparison.OrdinalIgnoreCase))
                    targets.Add(paired);
                else
                    log.Add("WARN|Could not derive the paired /ar/-/en/ URL from '" + firstUrl + "'. Only one page was created.");
            }

            foreach (string target in targets)
                CreateOnWeb(target, pageName, req, log);

            return log;
        }

        private static void CreateOnWeb(string webServerRelUrl, string pageName,
            clsPageCreateRequest req, List<string> log)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string siteUrl = SPContext.Current.Site.Url;

                    using (SPSite site = new SPSite(siteUrl))
                    using (SPWeb web = site.OpenWeb(webServerRelUrl))
                    {
                        if (!web.Exists)
                        {
                            log.Add("ERROR|" + webServerRelUrl + " — web does not exist.");
                            return;
                        }

                        if (!PublishingWeb.IsPublishingWeb(web))
                        {
                            log.Add("ERROR|" + webServerRelUrl + " — not a publishing web.");
                            return;
                        }

                        if (CheckPageExists(web, pageName))
                        {
                            log.Add("WARN|" + webServerRelUrl + "/Pages/" + pageName + " — already exists, skipped.");
                            return;
                        }

                        EnsurePage(web, pageName, req.PageTitleAr, req.PageTitleEn,
                            req.UserControlPath, req.UserControlProperties, req.PageLayoutURL);

                        if (CheckPageExists(web, pageName))
                            log.Add("OK|" + webServerRelUrl + "/Pages/" + pageName + " — created.");
                        else
                            log.Add("ERROR|" + webServerRelUrl + "/Pages/" + pageName +
                                    " — creation failed, check the portal log.");
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "busclsPageCreator.CreateOnWeb [" + webServerRelUrl + "/" + pageName + "]", ex.Message);
                log.Add("ERROR|" + webServerRelUrl + "/Pages/" + pageName + " — " + ex.Message);
            }
        }

        #endregion

        #region EnsurePage (same pattern as SideMenuListProvisioner / PageGenerator)

        public static bool CheckPageExists(SPWeb web, string pageName)
        {
            try
            {
                if (web == null || string.IsNullOrEmpty(pageName)) return false;
                if (!PublishingWeb.IsPublishingWeb(web)) return false;

                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                if (pubWeb == null) return false;

                string pageServerRelativeUrl = web.ServerRelativeUrl.TrimEnd('/') + "/" + pubWeb.PagesListName + "/" + pageName.TrimStart('/');
                SPFile existingFile = web.GetFile(pageServerRelativeUrl);
                return existingFile != null && existingFile.Exists;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a publishing page from the supplied layout and drops the
        /// user control into its web part zone via ControlLoaderWebPart.
        /// No-op if the page already exists. Follows the same sequence as
        /// PageGenerator.CreatePublishingPage / SideMenuListProvisioner.EnsurePage.
        /// </summary>
        public static void EnsurePage(SPWeb web, string pageName, string titleAr,
                                       string titleEn, string userControlPath, string UserControlProperties = "", string pageLayoutUrl = PAGE_LAYOUT_URL)
        {
            SideMenuListProvisioner.EnsurePage(web, pageName, titleAr, titleEn, userControlPath, UserControlProperties, pageLayoutUrl);
        }

        #endregion

        #region URL / name helpers

        /// <summary>Sanitizes the page name and guarantees a single .aspx extension.</summary>
        public static string NormalizePageName(string rawName)
        {
            if (string.IsNullOrEmpty(rawName)) return string.Empty;

            string name = rawName.Trim();
            if (name.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                name = name.Substring(0, name.Length - 5);

            name = Regex.Replace(name, @"[^A-Za-z0-9\-_]", "-");
            name = Regex.Replace(name, @"-{2,}", "-").Trim('-', '_');

            return string.IsNullOrEmpty(name) ? string.Empty : name + ".aspx";
        }

        /// <summary>Accepts absolute or server-relative URL; returns clean server-relative web URL.</summary>
        public static string NormalizeWebUrl(string rawUrl)
        {
            if (string.IsNullOrEmpty(rawUrl)) return string.Empty;

            string url = rawUrl.Trim();
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                try { url = new Uri(url).AbsolutePath; }
                catch { return string.Empty; }
            }

            if (!url.StartsWith("/")) url = "/" + url;
            url = url.TrimEnd('/');

            // Strip trailing /Pages or /Pages/xxx.aspx if pasted by mistake
            int pagesIdx = url.IndexOf("/Pages", StringComparison.OrdinalIgnoreCase);
            if (pagesIdx > 0) url = url.Substring(0, pagesIdx);

            return url;
        }

        /// <summary>Swaps the /ar/ - /en/ language segment to get the paired web URL.</summary>
        public static string GetPairedWebUrl(string serverRelUrl)
        {
            if (string.IsNullOrEmpty(serverRelUrl)) return string.Empty;

            string url = serverRelUrl.EndsWith("/") ? serverRelUrl : serverRelUrl + "/";

            if (url.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
                return ("/en/" + url.Substring(4)).TrimEnd('/');
            if (url.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                return ("/ar/" + url.Substring(4)).TrimEnd('/');

            return string.Empty;
        }

        #endregion

        #region Safe field mapping

        private static string SafeString(SPListItem item, string fieldName)
        {
            try
            {
                if (item == null || !item.Fields.ContainsField(fieldName)) return string.Empty;
                object val = item[fieldName];
                return val == null ? string.Empty : val.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        private static bool SafeBool(SPListItem item, string fieldName)
        {
            try
            {
                if (item == null || !item.Fields.ContainsField(fieldName)) return false;
                object val = item[fieldName];
                if (val == null) return false;
                bool parsed;
                if (bool.TryParse(val.ToString(), out parsed)) return parsed;
                return val.ToString() == "1";
            }
            catch { return false; }
        }

        #endregion
    }
}
