using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    /// <summary>
    /// Idempotent C# provisioner for About PNU SharePoint lists on /ar/AboutUniversity.
    /// Creates missing lists, adds columns, and seeds default data on initial creation
    /// or when manually requested via the admin control.
    /// </summary>
    public static class AboutPnuListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        public static void EnsureListExists(string listName)
        {
            if (AboutPnuListSchema.Get(listName) == null) return;
            if (DoneThisRequest("AboutPnuList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AboutPnuTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("AboutPnuList:" + listName);
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("AboutPnuListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("AboutPnuAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AboutPnuTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in AboutPnuListNames.All)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("AboutPnuList:" + listName);
                    }
                }

                MarkDoneThisRequest("AboutPnuAllLists");
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("AboutPnuListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            AboutPnuListDef def = AboutPnuListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("AboutPnuProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["AboutPnuProv:" + key] = true;
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
            foreach (string listName in AboutPnuListNames.All)
            {
                AboutPnuListDef def = AboutPnuListSchema.Get(listName);
                if (def != null) EnsureListOnWeb(siteId, webId, def);
            }
        }

        public static void EnsureListOnWeb(Guid siteId, Guid webId, AboutPnuListDef def)
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
                                list.OnQuickLaunch = false;
                                list.Update();
                                created = true;
                            }

                            EnsureColumns(list, def);

                            if (created || list.ItemCount == 0)
                            {
                                SeedDefaultData(list, def);
                            }
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
                AboutPnuLog.Write("AboutPnuListProvisioner.EnsureListOnWeb:" + def.Name, ex);
            }
        }

        /// <summary>
        /// Manually forces re-seeding of default schema data into the specified list.
        /// </summary>
        public static bool ForceSeedDefaultData(string listName)
        {
            AboutPnuListDef def = AboutPnuListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            bool success = false;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = AboutPnuTargetWeb.Open(site))
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
                                list.OnQuickLaunch = false;
                                list.Update();
                            }

                            EnsureColumns(list, def);
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
                AboutPnuLog.Write("AboutPnuListProvisioner.ForceSeedDefaultData:" + listName, ex);
                success = false;
            }

            return success;
        }

        private static void EnsureColumns(SPList list, AboutPnuListDef def)
        {
            bool changed = false;

            foreach (AboutPnuFieldDef f in def.Fields)
            {
                if (!list.Fields.ContainsField(f.InternalName))
                {
                    string displayName = AboutPnuHelper.Pick(f.DisplayAr, f.DisplayEn);
                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);
                    field.Title = displayName;

                    if (f.Choices != null && f.Choices.Length > 0 && field is SPFieldChoice)
                    {
                        SPFieldChoice choiceField = (SPFieldChoice)field;
                        choiceField.Choices.Clear();
                        foreach (string choice in f.Choices)
                        {
                            choiceField.Choices.Add(choice);
                        }
                    }

                    field.Update();
                    changed = true;
                }
            }

            if (changed) list.Update();
        }

        private static void SeedDefaultData(SPList list, AboutPnuListDef def)
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
                        }
                        else if (list.Fields.ContainsField(kv.Key))
                        {
                            SPField field = list.Fields.GetFieldByInternalName(kv.Key);
                            if (field.Type == SPFieldType.Number)
                            {
                                double val;
                                if (double.TryParse(kv.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                                    item[field.InternalName] = val;
                            }
                            else if (field.Type == SPFieldType.Boolean)
                            {
                                bool bVal = kv.Value == "1" || kv.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
                                item[field.InternalName] = bVal;
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
                    }

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("AboutPnuListProvisioner.SeedDefaultData:" + def.Name, ex);
            }
        }
    }
}
