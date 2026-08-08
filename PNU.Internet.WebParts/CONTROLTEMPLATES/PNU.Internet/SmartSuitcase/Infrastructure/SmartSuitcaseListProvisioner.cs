using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Idempotent C# provisioner for the Smart Suitcase lists.
    /// Creates lists on /ar/smart-suitcase and seeds initial data.
    /// </summary>
    public static class SmartSuitcaseListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        public static void EnsureListExists(string listName)
        {
            if (SscListSchema.Get(listName) == null) return;
            if (DoneThisRequest("SscList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = SscTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("SscList:" + listName);
            }
            catch (Exception ex)
            {
                SscLog.Write("SmartSuitcaseListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        public static void EnsureAllListsExist()
        {
            EnsureAdminUsersList();

            if (DoneThisRequest("SscAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = SscTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in SscListNames.AllLists)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("SscList:" + listName);
                    }
                }

                MarkDoneThisRequest("SscAllLists");
            }
            catch (Exception ex)
            {
                SscLog.Write("SmartSuitcaseListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            SscListDef def = SscListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("SscProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["SscProv:" + key] = true;
            }
            catch { /* non-web context */ }
        }

        public static void EnsureAdminUsersList()
        {
            if (DoneThisRequest("SscAdminUsers")) return;

            try
            {
                if (SPContext.Current == null) return;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb adminWeb = SscTargetWeb.OpenAdminWeb(site))
                    {
                        if (adminWeb == null) return;

                        SscListDef def = SscListSchema.Get(SscListNames.AdminUsers);
                        if (def == null) return;

                        if (adminWeb.Lists.TryGetList(def.Name) == null)
                            Invalidate(adminWeb.ID, def.Name);

                        EnsureListOnWeb(site.ID, adminWeb.ID, def);
                    }
                });

                MarkDoneThisRequest("SscAdminUsers");
            }
            catch (Exception ex)
            {
                SscLog.Write("SmartSuitcaseListProvisioner.EnsureAdminUsersList", ex);
            }
        }

        public static void EnsureListOnWeb(Guid siteId, Guid webId, SscListDef def)
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
                    SscLog.Write("SmartSuitcaseListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in SscListNames.AllLists)
                EnsureListOnWeb(siteId, webId, SscListSchema.Get(listName));
        }

        public static void Invalidate(Guid webId, string listName)
        {
            lock (_lock) { _provisioned.Remove(webId + "|" + listName); }
        }

        public static void ResetCache()
        {
            lock (_lock) { _provisioned.Clear(); }
        }

        private static void EnsureFields(SPList list, SscListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = SscHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                SscLog.Write("SmartSuitcaseListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (SscFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = SscHelper.Pick(f.DisplayAr, f.DisplayEn);

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

                    field.Update();
                    dirty = true;
                }
                catch (Exception ex)
                {
                    SscLog.Write("SmartSuitcaseListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void EnsureDefaultView(SPList list, SscListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (SscFieldDef f in def.Fields)
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
                SscLog.Write("SmartSuitcaseListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        private static void SeedItems(SPList list, SscListDef def)
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
                SscLog.Write("SmartSuitcaseListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
