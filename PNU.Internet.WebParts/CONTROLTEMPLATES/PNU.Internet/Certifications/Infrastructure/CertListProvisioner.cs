using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    /// <summary>
    /// Idempotent C# provisioner for the certifications list.
    ///
    /// Creating the list is one transaction: create -> add every column ->
    /// INSERT THE DEFAULT DATA (the 44 certificates of the guide), so a freshly
    /// provisioned list is never empty. An existing list is NEVER re-seeded, so the
    /// editors' changes are safe.
    ///
    /// Every page load calls <see cref="EnsureAllListsExist"/> (a cheap TryGetList on the
    /// target web); anything missing is re-created and re-seeded on that request.
    ///
    /// All work targets the web resolved by CertTargetWeb (/ar/Agencies/AcademicAffairs), so the
    /// Arabic and English pages share one list. No PowerShell.
    /// </summary>
    public static class CertListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        // =====================================================================
        // page-load entry points
        // =====================================================================
        /// <summary>
        /// Per-request check for one list on the target web. A missing list is created,
        /// filled with columns and seeded before the control binds.
        /// </summary>
        public static void EnsureListExists(string listName)
        {
            if (CertListSchema.Get(listName) == null) return;
            if (DoneThisRequest("CertList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = CertTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("CertList:" + listName);
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        /// <summary>
        /// Per-request check for every content list. Opens the target web ONCE - this is
        /// what the page control calls.
        /// </summary>
        public static void EnsureAllListsExist()
        {
            EnsureAdminUsersList();

            if (DoneThisRequest("CertAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = CertTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in CertListNames.AllLists)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("CertList:" + listName);
                    }
                }

                MarkDoneThisRequest("CertAllLists");
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureAllListsExist", ex);
            }
        }

        /// <summary>
        /// Checks one list against an already-open target web. A missing list drops the
        /// cached flag so the full provisioning path recreates and reseeds it.
        /// </summary>
        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            CertListDef def = CertListSchema.Get(listName);
            if (def == null) return;

            if (web.Lists.TryGetList(def.Name) == null)
                Invalidate(web.ID, def.Name);

            EnsureListOnWeb(site.ID, web.ID, def);
        }

        private static bool DoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                return ctx != null && ctx.Items.Contains("CertProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["CertProv:" + key] = true;
            }
            catch { /* non-web context */ }
        }

        /// <summary>
        /// Provisions the shared AdminUsers list on /ar/ContentAdmin.
        /// Runs elevated end to end: editors are not expected to have rights on that web,
        /// and the control still has to be able to read the list to authorise them.
        /// </summary>
        public static void EnsureAdminUsersList()
        {
            if (DoneThisRequest("CertAdminUsers")) return;

            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = CertTargetWeb.OpenAdminWeb(site))
                    {
                        if (adminWeb == null) return;

                        CertListDef def = CertListSchema.Get(CertListNames.AdminUsers);
                        if (def == null) return;

                        if (adminWeb.Lists.TryGetList(def.Name) == null)
                            Invalidate(adminWeb.ID, def.Name);

                        EnsureListOnWeb(site.ID, adminWeb.ID, def);
                    }
                });

                MarkDoneThisRequest("CertAdminUsers");
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureAdminUsersList", ex);
            }
        }

        // =====================================================================
        // full provisioning
        // =====================================================================
        public static void EnsureAllLists()
        {
            foreach (string listName in CertListNames.AllLists)
                EnsureList(listName);
        }

        public static void EnsureList(string listName)
        {
            CertListDef def = CertListSchema.Get(listName);
            if (def == null) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = CertTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureListOnWeb(site.ID, web.ID, def);
                }
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureList:" + listName, ex);
            }
        }

        /// <summary>Context-free provisioning of one list on a specific web.</summary>
        public static void EnsureListOnWeb(Guid siteId, Guid webId, CertListDef def)
        {
            if (def == null) return;

            string key = webId + "|" + def.Name;

            lock (_lock)
            {
                if (_provisioned.Contains(key)) return;

                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = site.OpenWeb(webId))
                        {
                            bool prevUnsafe = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                bool created = false;
                                SPList list = web.Lists.TryGetList(def.Name);

                                if (list == null)
                                {
                                    Guid id = web.Lists.Add(def.Name, def.Description, SPListTemplateType.GenericList);
                                    list = web.Lists[id];
                                    list.OnQuickLaunch = false;
                                    list.Update();
                                    created = true;
                                }

                                EnsureFields(list, def);
                                EnsureDefaultView(list, def);

                                if (created) SeedItems(list, def);
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = prevUnsafe;
                            }
                        }
                    });

                    _provisioned.Add(key);
                }
                catch (Exception ex)
                {
                    CertLog.Write("CertListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in CertListNames.AllLists)
                EnsureListOnWeb(siteId, webId, CertListSchema.Get(listName));
        }

        // =====================================================================
        // maintenance helpers
        // =====================================================================

        /// <summary>
        /// Re-inserts the default rows into an existing list that is empty - the way back
        /// to the delivered content after an editor deleted everything, without dropping
        /// the list and its columns.
        /// </summary>
        public static bool SeedIfEmpty(string listName)
        {
            CertListDef def = CertListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool seeded = false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = CertTargetWeb.Open(site))
                    {
                        if (web == null) return;

                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list == null || list.ItemCount > 0) return;

                            SeedItems(list, def);
                            seeded = true;
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.SeedIfEmpty:" + listName, ex);
            }

            return seeded;
        }

        public static void Invalidate(Guid webId, string listName)
        {
            lock (_lock) { _provisioned.Remove(webId + "|" + listName); }
        }

        public static void ResetCache()
        {
            lock (_lock) { _provisioned.Clear(); }
        }

        // =====================================================================
        // internals
        // =====================================================================
        private static void EnsureFields(SPList list, CertListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = CertHelper.Pick("اسم الشهادة (عربي)", "Certificate name (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (CertFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = CertHelper.Pick(f.DisplayAr, f.DisplayEn);

                    SPFieldMultiLineText note = field as SPFieldMultiLineText;
                    if (note != null)
                    {
                        note.RichText = false;
                        note.NumberOfLines = f.Rows > 0 ? f.Rows : 4;
                    }

                    SPFieldNumber num = field as SPFieldNumber;
                    if (num != null) num.DisplayFormat = SPNumberFormatTypes.NoDecimal;

                    SPFieldBoolean flag = field as SPFieldBoolean;
                    if (flag != null) flag.DefaultValue = "1";

                    SPFieldUser person = field as SPFieldUser;
                    if (person != null)
                    {
                        person.AllowMultipleValues = false;
                        person.SelectionMode = SPFieldUserSelectionMode.PeopleOnly;
                        person.Presence = false;
                    }

                    SPFieldDateTime date = field as SPFieldDateTime;
                    if (date != null) date.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;

                    SPFieldChoice choice = field as SPFieldChoice;
                    if (choice != null && f.Choices != null && f.Choices.Length > 0)
                    {
                        choice.Choices.Clear();
                        foreach (string option in f.Choices) choice.Choices.Add(option);
                        choice.DefaultValue = f.Choices[0];
                        choice.EditFormat = SPChoiceFormatType.Dropdown;
                    }

                    field.Update();
                    dirty = true;
                }
                catch (Exception ex)
                {
                    CertLog.Write("CertListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        /// <summary>
        /// Adds the summary columns to the default view. Only the InGrid columns: the
        /// list has 27 of them and a view carrying every one is unusable.
        /// </summary>
        private static void EnsureDefaultView(SPList list, CertListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (CertFieldDef f in def.Fields)
                {
                    if (!f.InGrid) continue;
                    if (!list.Fields.ContainsField(f.InternalName)) continue;
                    if (view.ViewFields.Exists(f.InternalName)) continue;
                    view.ViewFields.Add(f.InternalName);
                    dirty = true;
                }

                if (dirty) view.Update();
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        /// <summary>Inserts the default rows defined in the schema.</summary>
        private static void SeedItems(SPList list, CertListDef def)
        {
            if (def.Seed == null || def.Seed.Count == 0) return;

            try
            {
                foreach (Dictionary<string, string> row in def.Seed)
                {
                    SPListItem item = list.AddItem();

                    foreach (KeyValuePair<string, string> kv in row)
                    {
                        if (!list.Fields.ContainsField(kv.Key)) continue;

                        SPField field = list.Fields.GetFieldByInternalName(kv.Key);

                        if (field.Type == SPFieldType.URL)
                        {
                            string url = kv.Value;
                            string desc = string.Empty;
                            int comma = url.IndexOf(',');
                            if (comma > -1)
                            {
                                desc = url.Substring(comma + 1).Trim();
                                url = url.Substring(0, comma).Trim();
                            }
                            item[kv.Key] = new SPFieldUrlValue { Url = url, Description = desc };
                        }
                        else if (field.Type == SPFieldType.Number)
                        {
                            double d;
                            if (double.TryParse(kv.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                                item[kv.Key] = d;
                        }
                        else if (field.Type == SPFieldType.DateTime)
                        {
                            DateTime dt;
                            if (DateTime.TryParse(kv.Value, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.None, out dt))
                                item[kv.Key] = dt;
                        }
                        else
                        {
                            item[kv.Key] = kv.Value;
                        }
                    }

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                CertLog.Write("CertListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
