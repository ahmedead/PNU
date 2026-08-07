using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Idempotent C# provisioner for the Agency lists.
    ///
    /// Creating a list is one transaction: create -> add every column ->
    /// INSERT THE DEFAULT DATA, so a freshly provisioned list is never empty.
    /// Existing lists are never re-seeded.
    ///
    /// Every page load calls <see cref="EnsureListExists"/> (a cheap TryGetList on the
    /// context web); anything missing is re-created and re-seeded on that request.
    ///
    /// All work targets the CURRENT web - never RootWeb. No PowerShell.
    /// </summary>
    public static class AgListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        // =====================================================================
        // page-load entry points
        // =====================================================================
        public static void EnsureListExists(string listName)
        {
            try
            {
                if (SPContext.Current == null) return;
                if (AgListSchema.Get(listName) == null) return;

                SPWeb contextWeb = SPContext.Current.Web;
                string key = contextWeb.ID + "|" + listName;

                // Created earlier in THIS request: SPWeb.Lists is cached and would still
                // report the list as missing, so trust the request flag instead.
                if (ProvisionedThisRequest(key)) return;

                if (contextWeb.Lists.TryGetList(listName) == null)
                {
                    Invalidate(contextWeb.ID, listName);
                    EnsureList(listName);
                    MarkProvisionedThisRequest(key);
                }
                else
                {
                    EnsureList(listName);   // verifies columns once per app-pool lifetime
                }
            }
            catch (Exception ex)
            {
                AgLog.Write("AgListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        public static void EnsureAllListsExist()
        {
            foreach (string listName in AgListNames.AllLists)
                EnsureListExists(listName);
        }

        private static bool ProvisionedThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                return ctx != null && ctx.Items.Contains("AgProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkProvisionedThisRequest(string key)
        {
            try
            {
                System.Web.HttpContext ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["AgProv:" + key] = true;
            }
            catch { /* non-web context */ }
        }

        // =====================================================================
        // full provisioning
        // =====================================================================
        public static void EnsureAllLists()
        {
            foreach (string listName in AgListNames.AllLists)
                EnsureList(listName);
        }

        public static void EnsureList(string listName)
        {
            AgListDef def = AgListSchema.Get(listName);
            if (def == null) return;

            Guid siteId, webId;
            try
            {
                if (SPContext.Current == null) return;
                siteId = SPContext.Current.Site.ID;
                webId = SPContext.Current.Web.ID;
            }
            catch (Exception ex)
            {
                AgLog.Write("AgListProvisioner.EnsureList.Context:" + listName, ex);
                return;
            }

            EnsureListOnWeb(siteId, webId, def);
        }

        /// <summary>Context-free provisioning of one list on a specific web.</summary>
        public static void EnsureListOnWeb(Guid siteId, Guid webId, AgListDef def)
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
                    AgLog.Write("AgListProvisioner.EnsureListOnWeb:" + def.Name, ex);
                }
            }
        }

        public static void EnsureAllListsOnWeb(Guid siteId, Guid webId)
        {
            foreach (string listName in AgListNames.AllLists)
                EnsureListOnWeb(siteId, webId, AgListSchema.Get(listName));
        }

        // =====================================================================
        // maintenance helpers
        // =====================================================================

        /// <summary>Re-inserts the default rows into an existing list that is empty.</summary>
        public static bool SeedIfEmpty(string listName)
        {
            AgListDef def = AgListSchema.Get(listName);
            if (def == null || SPContext.Current == null) return false;

            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            bool seeded = false;

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
                AgLog.Write("AgListProvisioner.SeedIfEmpty:" + listName, ex);
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
        private static void EnsureFields(SPList list, AgListDef def)
        {
            bool dirty = false;

            try
            {
                SPField title = list.Fields.GetFieldByInternalName("Title");
                string wanted = AgHelper.Pick("العنوان (عربي)", "Title (AR)");
                if (title != null && title.Title != wanted)
                {
                    title.Title = wanted;
                    title.Update();
                    dirty = true;
                }
            }
            catch (Exception ex)
            {
                AgLog.Write("AgListProvisioner.EnsureFields.Title:" + def.Name, ex);
            }

            foreach (AgFieldDef f in def.Fields)
            {
                try
                {
                    if (list.Fields.ContainsField(f.InternalName)) continue;

                    string added = list.Fields.Add(f.InternalName, f.Type, false);
                    SPField field = list.Fields.GetFieldByInternalName(added);

                    field.Title = AgHelper.Pick(f.DisplayAr, f.DisplayEn);

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
                    AgLog.Write("AgListProvisioner.EnsureFields:" + def.Name + "." + f.InternalName, ex);
                }
            }

            if (dirty) list.Update();
        }

        private static void EnsureDefaultView(SPList list, AgListDef def)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool dirty = false;
                foreach (AgFieldDef f in def.Fields)
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
                AgLog.Write("AgListProvisioner.EnsureDefaultView:" + def.Name, ex);
            }
        }

        /// <summary>Inserts the default rows defined in the schema.</summary>
        private static void SeedItems(SPList list, AgListDef def)
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
                AgLog.Write("AgListProvisioner.SeedItems:" + def.Name, ex);
            }
        }
    }
}
