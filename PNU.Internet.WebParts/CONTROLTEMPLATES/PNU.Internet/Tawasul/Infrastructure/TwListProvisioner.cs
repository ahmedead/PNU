using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Idempotent C# provisioner for the Tawasul (تواصل) lists.
    ///
    /// Creating a list is one transaction: create -> add every column ->
    /// INSERT THE DEFAULT DATA, so a freshly provisioned list is never empty.
    /// Existing lists are never re-seeded.
    ///
    /// Every page load calls <see cref="EnsureListExists"/>; anything missing is
    /// re-created and re-seeded on that request. All content work targets the web
    /// resolved by TwTargetWeb (/ar/Tawasul); AdminUsers is provisioned on
    /// /ar/ContentAdmin. No PowerShell.
    /// </summary>
    public static class TwListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        // =====================================================================
        // page-load entry points
        // =====================================================================
        public static void EnsureListExists(string listName)
        {
            if (TwListSchema.Get(listName) == null) return;
            if (DoneThisRequest("TwList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = TwTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("TwList:" + listName);
            }
            catch (Exception ex)
            {
                TwLog.Write("TwListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        /// <summary>
        /// Per-request check for every list. Opens the target web ONCE and walks all
        /// content lists, then provisions AdminUsers on its own web.
        /// </summary>
        public static void EnsureAllListsExist()
        {
            EnsureAdminUsersList();

            if (DoneThisRequest("TwAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = TwTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in TwListNames.AllLists)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("TwList:" + listName);
                    }
                }

                MarkDoneThisRequest("TwAllLists");
            }
            catch (Exception ex)
            {
                TwLog.Write("TwListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            TwListDef def = TwListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("TwProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["TwProv:" + key] = true;
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
            if (DoneThisRequest("TwAdminUsers")) return;

            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = TwTargetWeb.OpenAdminWeb(site))
                    {
                        if (adminWeb == null) return;

                        TwListDef def = TwListSchema.Get(TwListNames.AdminUsers);
                        if (def == null) return;

                        if (adminWeb.Lists.TryGetList(def.Name) == null)
                            Invalidate(adminWeb.ID, def.Name);

                        EnsureListOnWeb(site.ID, adminWeb.ID, def);
                    }
                });

                MarkDoneThisRequest("TwAdminUsers");
            }
            catch (Exception ex)
            {
                TwLog.Write("TwListProvisioner.EnsureAdminUsersList", ex);
            }
        }

        // =====================================================================
        // full provisioning
        // =====================================================================
        public static void EnsureAllLists()
        {
            foreach (string listName in TwListNames.AllLists)
                EnsureList(listName);
        }

        public static void EnsureList(string listName)
        {
            TwListDef def = TwListSchema.Get(listName);
            if (def == null) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = TwTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureListOnWeb(site.ID, web.ID, def);
                }
            }
            catch (Exception ex)
            {
                TwLog.Write("TwListProvisioner.EnsureList:" + listName, ex);
                return;
            }
        }

        /// <summary>Context-free provisioning of one list on a specific web.</summary>
        public static void EnsureListOnWeb(Guid siteId, Guid webId, TwListDef def)
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
                    TwLog.Write("TwListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in TwListNames.AllLists)
                EnsureListOnWeb(siteId, webId, TwListSchema.Get(listName));
        }

        // =====================================================================
        // maintenance helpers
        // =====================================================================
        public static bool SeedIfEmpty(string listName)
        {
            TwListDef def = TwListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool seeded = false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = TwTargetWeb.Open(site))
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
                TwLog.Write("TwListProvisioner.SeedIfEmpty:" + listName, ex);
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
        private static void EnsureFields(SPList list, TwListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = TwHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                TwLog.Write("TwListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (TwFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = TwHelper.Pick(f.DisplayAr, f.DisplayEn);

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
                    TwLog.Write("TwListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void EnsureDefaultView(SPList list, TwListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (TwFieldDef f in def.Fields)
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
                TwLog.Write("TwListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        private static void SeedItems(SPList list, TwListDef def)
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
                TwLog.Write("TwListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
