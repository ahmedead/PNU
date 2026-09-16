using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Idempotent C# provisioner for the Postgraduate Academic Kit lists on
    /// /ar/Deanship/postgraduate. Creates missing lists, adds missing columns and seeds
    /// the default bilingual data on first creation or when requested from the admin control.
    /// </summary>
    public static class PgListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        public static void EnsureListExists(string listName)
        {
            if (PgListSchema.Get(listName) == null) return;
            if (DoneThisRequest("PgList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PgTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("PgList:" + listName);
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("PgAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in PgListNames.All)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("PgList:" + listName);
                    }
                }

                MarkDoneThisRequest("PgAllLists");
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            PgListDef def = PgListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("PgProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["PgProv:" + key] = true;
            }
            catch { }
        }

        public static void Invalidate(Guid webId, string listName)
        {
            lock (_lock)
            {
                _provisioned.Remove(CacheKey(webId, listName));
            }
        }

        public static void ResetCache()
        {
            lock (_lock)
            {
                _provisioned.Clear();
            }
        }

        private static string CacheKey(Guid webId, string listName)
        {
            return webId.ToString("N") + ":" + listName.ToLowerInvariant();
        }

        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in PgListNames.All)
            {
                PgListDef def = PgListSchema.Get(listName);
                if (def != null) EnsureListOnWeb(siteId, webId, def);
            }
        }

        public static void EnsureListOnWeb(Guid siteId, Guid webId, PgListDef def)
        {
            if (def == null) return;
            string key = CacheKey(webId, def.Name);

            lock (_lock)
            {
                if (_provisioned.Contains(key)) return;
            }

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
                                Guid newId = web.Lists.Add(def.Name, def.Description, SPListTemplateType.GenericList);
                                list = web.Lists[newId];
                                list.Title = def.Name;
                                list.Update();
                                created = true;
                            }

                            EnsureColumns(list, def);

                            if (created || list.ItemCount == 0)
                                SeedDefaultData(list, def);
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = prevUnsafe;
                        }
                    }
                });

                lock (_lock)
                {
                    _provisioned.Add(key);
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.EnsureListOnWeb:" + def.Name, ex);
            }
        }

        /// <summary>
        /// Replaces the content of one list with the default seed data. Existing items are
        /// deleted first so repeated seeding never produces duplicates.
        /// </summary>
        public static bool ForceSeedDefaultData(string listName)
        {
            PgListDef def = PgListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool success = false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = PgTargetWeb.Open(site))
                    {
                        if (web == null) return;
                        bool prevUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPList list = web.Lists.TryGetList(def.Name);
                            if (list == null)
                            {
                                Guid newId = web.Lists.Add(def.Name, def.Description, SPListTemplateType.GenericList);
                                list = web.Lists[newId];
                                list.Title = def.Name;
                                list.Update();
                            }

                            EnsureColumns(list, def);
                            DeleteAllItems(list);
                            SeedDefaultData(list, def);
                            success = true;
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
                PgLog.Write("PgListProvisioner.ForceSeedDefaultData:" + listName, ex);
                success = false;
            }

            return success;
        }

        /// <summary>Re-seeds every kit list in one go.</summary>
        public static bool ForceSeedAll()
        {
            bool allOk = true;

            foreach (string listName in PgListNames.All)
            {
                if (!ForceSeedDefaultData(listName)) allOk = false;
            }

            return allOk;
        }

        private static void DeleteAllItems(SPList list)
        {
            try
            {
                // The collection is fetched once: re-reading list.Items after every delete
                // would shrink it under the loop and leave items behind.
                SPListItemCollection items = list.Items;
                for (int i = items.Count - 1; i >= 0; i--)
                    items[i].Delete();
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.DeleteAllItems:" + list.Title, ex);
            }
        }

        private static void EnsureColumns(SPList list, PgListDef def)
        {
            bool changed = false;

            foreach (PgFieldDef f in def.Fields)
            {
                if (list.Fields.ContainsField(f.InternalName)) continue;

                string displayName = PgHelper.Pick(f.DisplayAr, f.DisplayEn);
                string added = list.Fields.Add(f.InternalName, f.Type, false);
                SPField field = list.Fields.GetFieldByInternalName(added);
                field.Title = displayName;

                SPFieldChoice choiceField = field as SPFieldChoice;
                if (choiceField != null && f.Choices != null && f.Choices.Length > 0)
                {
                    choiceField.Choices.Clear();
                    foreach (string choice in f.Choices) choiceField.Choices.Add(choice);
                    choiceField.FillInChoice = false;
                }

                SPFieldMultiLineText noteField = field as SPFieldMultiLineText;
                if (noteField != null)
                {
                    noteField.RichText = false;
                    noteField.NumberOfLines = f.Rows > 0 ? f.Rows : 6;
                }

                field.Update();
                changed = true;
            }

            if (changed) list.Update();

            EnsureDefaultView(list, def);
        }

        public static void EnsureDefaultView(SPList list, PgListDef def)
        {
            if (list == null || def == null || def.Fields == null || def.Fields.Count == 0) return;

            try
            {
                SPView defaultView = list.DefaultView;
                if (defaultView == null && list.Views != null && list.Views.Count > 0)
                {
                    defaultView = list.Views[0];
                }
                if (defaultView == null) return;

                bool viewChanged = false;
                foreach (PgFieldDef f in def.Fields)
                {
                    if (list.Fields.ContainsField(f.InternalName) && !defaultView.ViewFields.Exists(f.InternalName))
                    {
                        defaultView.ViewFields.Add(f.InternalName);
                        viewChanged = true;
                    }
                }

                if (viewChanged)
                {
                    defaultView.Update();
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.EnsureDefaultView:" + list.Title, ex);
            }
        }

        private static void SeedDefaultData(SPList list, PgListDef def)
        {
            if (def.Seed == null || def.Seed.Count == 0) return;

            try
            {
                foreach (Dictionary<string, string> row in def.Seed)
                {
                    SPListItem item = list.AddItem();

                    foreach (KeyValuePair<string, string> kv in row)
                    {
                        if (string.Equals(kv.Key, "Title", StringComparison.OrdinalIgnoreCase))
                        {
                            item["Title"] = kv.Value;
                            continue;
                        }

                        if (!list.Fields.ContainsField(kv.Key)) continue;

                        SPField field = list.Fields.GetFieldByInternalName(kv.Key);

                        if (field.Type == SPFieldType.Number)
                        {
                            double val;
                            if (double.TryParse(kv.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                                item[field.InternalName] = val;
                        }
                        else if (field.Type == SPFieldType.URL)
                        {
                            string url = kv.Value;
                            string desc = string.Empty;
                            int comma = url.IndexOf(',');
                            if (comma > -1)
                            {
                                desc = url.Substring(comma + 1).Trim();
                                url = url.Substring(0, comma).Trim();
                            }
                            item[field.InternalName] = new SPFieldUrlValue { Url = url, Description = desc };
                        }
                        else if (field.Type == SPFieldType.DateTime)
                        {
                            DateTime dt;
                            if (DateTime.TryParse(kv.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                item[field.InternalName] = dt;
                        }
                        else
                        {
                            item[field.InternalName] = kv.Value;
                        }
                    }

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("PgListProvisioner.SeedDefaultData:" + def.Name, ex);
            }
        }
    }
}
