using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>
    /// Idempotent C# provisioner for the University President Office lists.
    ///
    /// Creating a list is one transaction: create -> add every column ->
    /// INSERT THE DEFAULT DATA, so a freshly provisioned list is never empty.
    /// Existing lists are never re-seeded.
    /// </summary>
    public static class UpoListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        // =====================================================================
        // page-load entry points
        // =====================================================================
        /// <summary>Per-request check for one list on the target web.</summary>
        public static void EnsureListExists(string listName)
        {
            if (UpoListSchema.Get(listName) == null) return;
            if (DoneThisRequest("UpoList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = UpoTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("UpoList:" + listName);
            }
            catch (Exception ex)
            {
                UpoLog.Write("UpoListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        /// <summary>Per-request check for every list.</summary>
        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("UpoAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = UpoTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in UpoListNames.AllLists)
                    {
                        EnsureOnResolvedWeb(site, web, listName);
                        MarkDoneThisRequest("UpoList:" + listName);
                    }
                }

                MarkDoneThisRequest("UpoAllLists");
            }
            catch (Exception ex)
            {
                UpoLog.Write("UpoListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            UpoListDef def = UpoListSchema.Get(listName);
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
                return ctx != null && ctx.Items.Contains("UpoProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["UpoProv:" + key] = true;
            }
            catch { /* non-web context */ }
        }

        // =====================================================================
        // full provisioning
        // =====================================================================
        public static void EnsureListOnWeb(Guid siteId, Guid webId, UpoListDef def)
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
                    UpoLog.Write("UpoListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
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
        private static void EnsureFields(SPList list, UpoListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = UpoHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                UpoLog.Write("UpoListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (UpoFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = UpoHelper.Pick(f.DisplayAr, f.DisplayEn);

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
                    UpoLog.Write("UpoListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void EnsureDefaultView(SPList list, UpoListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (UpoFieldDef f in def.Fields)
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
                UpoLog.Write("UpoListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        /// <summary>Inserts the default rows defined in the schema.</summary>
        private static void SeedItems(SPList list, UpoListDef def)
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
                UpoLog.Write("UpoListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
