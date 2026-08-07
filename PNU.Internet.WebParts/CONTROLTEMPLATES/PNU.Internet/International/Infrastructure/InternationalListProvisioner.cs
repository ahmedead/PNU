using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Idempotent C# provisioner for the International Students lists.
    ///
    /// Provisioning is one transaction: create the list -> add every column ->
    /// INSERT THE DEFAULT DATA. Seeding happens at creation time, so a freshly
    /// provisioned list is never empty.
    ///
    /// Every page load calls <see cref="EnsureListExists"/> (a cheap TryGetList on the
    /// context web). If the list was deleted or the site was copied to another
    /// environment, it is re-created and re-seeded on that request.
    ///
    /// All work targets the web resolved by IntlTargetWeb (/ar/International), so the
    /// Arabic and English pages share one set of lists. No PowerShell.
    /// </summary>
    public static class InternationalListProvisioner
    {
        private static readonly object _lock = new object();

        /// <summary>Lists whose columns/seed have already been verified this app-pool lifetime.</summary>
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        // =====================================================================
        // page-load entry points
        // =====================================================================

        /// <summary>
        /// Cheap per-request existence check for one list on the current web.
        /// Runs on EVERY page load: if the list is missing it is created, its columns
        /// added and its default data inserted before the control binds.
        /// </summary>
        /// <summary>
        /// Per-request check for one list on the target web. Missing lists are created,
        /// filled with columns and seeded before the control binds.
        /// </summary>
        public static void EnsureListExists(string listName)
        {
            if (IntlListSchema.Get(listName) == null) return;
            if (DoneThisRequest("IntlList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = IntlTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("IntlList:" + listName);
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        /// <summary>
        /// Per-request check for every list. Opens the target web ONCE and walks all
        /// ten lists - this is what the page container calls.
        /// </summary>
        public static void EnsureAllListsExist()
        {
            EnsureAdminUsersList();

            if (DoneThisRequest("IntlAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = IntlTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in IntlListNames.AllLists)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("IntlList:" + listName);
                    }
                }

                MarkDoneThisRequest("IntlAllLists");
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureAllListsExist", ex);
            }
        }

        /// <summary>
        /// Checks one list against an already-open target web. A missing list drops the
        /// cached flag so the full provisioning path recreates and reseeds it.
        /// </summary>
        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            IntlListDef def = IntlListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("IntlProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["IntlProv:" + key] = true;
            }
            catch { /* non-web context */ }
        }

        /// <summary>
        /// Provisions the shared AdminUsers list on /ar/ContentAdmin.
        /// Runs elevated end to end: editors are not expected to have rights on that
        /// web, and the control still has to be able to read the list to authorise them.
        /// </summary>
        public static void EnsureAdminUsersList()
        {
            if (DoneThisRequest("IntlAdminUsers")) return;

            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = IntlTargetWeb.OpenAdminWeb(site))
                    {
                        if (adminWeb == null) return;

                        IntlListDef def = IntlListSchema.Get(IntlListNames.AdminUsers);
                        if (def == null) return;

                        if (adminWeb.Lists.TryGetList(def.Name) == null)
                            Invalidate(adminWeb.ID, def.Name);

                        EnsureListOnWeb(site.ID, adminWeb.ID, def);
                    }
                });

                MarkDoneThisRequest("IntlAdminUsers");
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureAdminUsersList", ex);
            }
        }

        // =====================================================================
        // full provisioning
        // =====================================================================

        /// <summary>Provisions every International list on the current web.</summary>
        public static void EnsureAllLists()
        {
            foreach (string listName in IntlListNames.AllLists)
                EnsureList(listName);
        }

        /// <summary>
        /// Provisions a single International list on the current web:
        /// creates it if missing, adds any missing columns, and inserts the default
        /// data as part of the creation step.
        /// </summary>
        public static void EnsureList(string listName)
        {
            IntlListDef def = IntlListSchema.Get(listName);
            if (def == null) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = IntlTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureListOnWeb(site.ID, web.ID, def);
                }
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureList:" + listName, ex);
            }
        }

        /// <summary>
        /// Context-free provisioning of one list on a specific web.
        /// Used by the page-load path and by the feature receiver.
        /// </summary>
        public static void EnsureListOnWeb(Guid siteId, Guid webId, IntlListDef def)
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

                                // Default data is part of provisioning: a newly created
                                // list is populated immediately, in the same request.
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
                    IntlLog.Write("InternationalListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        /// <summary>Provisions every list on a specific web (feature receiver path).</summary>
        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in IntlListNames.AllLists)
                EnsureListOnWeb(siteId, webId, IntlListSchema.Get(listName));
        }

        // =====================================================================
        // maintenance helpers
        // =====================================================================

        /// <summary>
        /// Re-inserts the default rows into an EXISTING list that is currently empty.
        /// Useful for lists provisioned before seeding existed, or after a full purge.
        /// Never touches a list that already has items.
        /// </summary>
        public static bool SeedIfEmpty(string listName)
        {
            IntlListDef def = IntlListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool seeded = false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = IntlTargetWeb.Open(site))
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
                IntlLog.Write("InternationalListProvisioner.SeedIfEmpty:" + listName, ex);
            }

            return seeded;
        }

        /// <summary>Forgets the cached "already provisioned" flag for one list on one web.</summary>
        public static void Invalidate(Guid webId, string listName)
        {
            lock (_lock) { _provisioned.Remove(webId + "|" + listName); }
        }

        /// <summary>Drops the whole in-memory provisioning cache.</summary>
        public static void ResetCache()
        {
            lock (_lock) { _provisioned.Clear(); }
        }

        // =====================================================================
        // internals
        // =====================================================================
        private static void EnsureFields(SPList list, IntlListDef def)
        {
            bool dirty = false;

            // Re-label the built-in Title column so admins see a meaningful name.
            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = IntlHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (IntlFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = IntlHelper.Pick(f.DisplayAr, f.DisplayEn);

                    SPFieldMultiLineText note = field as SPFieldMultiLineText;
                    if (note != null)
                    {
                        note.RichText = false;          // keep bullets as plain lines
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

                    field.Update();
                    dirty = true;
                }
                catch (Exception ex)
                {
                    IntlLog.Write(
                        "InternationalListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void EnsureDefaultView(SPList list, IntlListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (IntlFieldDef f in def.Fields)
                {
                    if (!list.Fields.ContainsField(f.InternalName)) continue;
                    if (view.ViewFields.Exists(f.InternalName)) continue;
                    view.ViewFields.Add(f.InternalName);
                    dirty = true;
                }

                if (dirty) view.Update();
            }
            catch (Exception ex)
            {
                IntlLog.Write("InternationalListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        /// <summary>Inserts the default rows defined in the schema.</summary>
        private static void SeedItems(SPList list, IntlListDef def)
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
                IntlLog.Write("InternationalListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
