using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    /// <summary>
    /// Idempotent C# provisioner for the Herbarium lists on /ar/Faculties/SC.
    /// Runs safely under elevated privileges; ensures lists exist, fields are present,
    /// and seed data is populated.
    /// </summary>
    public static class HerbariumListProvisioner
    {
        private static readonly object SyncLock = new object();
        private static readonly HashSet<string> ProvisionedCache = new HashSet<string>();

        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("HerbariumAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = HerbariumTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (var kvp in HerbariumListSchema.All)
                    {
                        EnsureOnResolvedWeb(site, web, kvp.Key);
                        MarkDoneThisRequest("HerbariumList:" + kvp.Key);
                    }
                }

                MarkDoneThisRequest("HerbariumAllLists");
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("HerbariumListProvisioner.EnsureAllListsExist", ex);
            }
        }

        public static void EnsureAllListsExist(SPSite contextSite, string targetWebUrl = null)
        {
            EnsureAllListsExist();
        }

        public static void EnsureListExists(string listName)
        {
            if (HerbariumListSchema.Get(listName) == null) return;
            if (DoneThisRequest("HerbariumList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = HerbariumTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("HerbariumList:" + listName);
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("HerbariumListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            HerbariumListDef def = HerbariumListSchema.Get(listName);
            if (def == null) return;

            if (web.Lists.TryGetList(def.Name) == null)
                Invalidate(web.ID, def.Name);

            EnsureListOnWeb(site.ID, web.ID, def);
        }

        public static void EnsureListOnWeb(Guid siteId, Guid webId, HerbariumListDef def)
        {
            if (def == null) return;

            string key = webId + "|" + def.Name;

            lock (SyncLock)
            {
                if (ProvisionedCache.Contains(key)) return;

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

                                if (created || list.ItemCount == 0)
                                {
                                    SeedItems(list, def);
                                }
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = prevUnsafe;
                            }
                        }
                    });

                    ProvisionedCache.Add(key);
                }
                catch (Exception ex)
                {
                    HerbariumLog.Write("HerbariumListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        private static void EnsureFields(SPList list, HerbariumListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = HerbariumHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch { }

            foreach (HerbariumFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = HerbariumHelper.Pick(f.DisplayAr, f.DisplayEn);

                    SPFieldMultiLineText note = field as SPFieldMultiLineText;
                    if (note != null)
                    {
                        note.RichText = false;
                        note.NumberOfLines = f.Rows > 0 ? f.Rows : 4;
                    }

                    SPFieldNumber num = field as SPFieldNumber;
                    if (num != null) num.DisplayFormat = SPNumberFormatTypes.NoDecimal;

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
                    HerbariumLog.Write("HerbariumListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void SeedItems(SPList list, HerbariumListDef def)
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
                HerbariumLog.Write("HerbariumListProvisioner.SeedItems:" + def.Name, ex);
            }
        }

        private static bool DoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                return ctx != null && ctx.Items.Contains("HerbariumProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["HerbariumProv:" + key] = true;
            }
            catch { }
        }

        public static void Invalidate(Guid webId, string listName)
        {
            lock (SyncLock) { ProvisionedCache.Remove(webId + "|" + listName); }
        }

        public static void ResetCache()
        {
            lock (SyncLock) { ProvisionedCache.Clear(); }
        }
    }
}
