using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Idempotent list provisioner for DGA Postgraduate Academic Kit lists.
    /// Creates missing lists, adds columns, and seeds default bilingual data.
    /// </summary>
    public static class DgaPgListProvisioner
    {
        private static readonly object _lock = new object();
        private static readonly HashSet<string> _provisioned = new HashSet<string>();

        /// <summary>
        /// Robust list finder that checks by internal name, display title, and server-relative URL.
        /// Prevents duplicates caused by display title renaming.
        /// </summary>
        public static SPList FindList(SPWeb web, string listName)
        {
            if (web == null || string.IsNullOrEmpty(listName)) return null;

            // 1. Try by internal name
            SPList list = web.Lists.TryGetList(listName);
            if (list != null) return list;

            // 2. Try by schema display title
            DgaPgListDef def = DgaPgListSchema.Get(listName);
            if (def != null && !string.IsNullOrEmpty(def.DisplayTitle))
            {
                list = web.Lists.TryGetList(def.DisplayTitle);
                if (list != null) return list;
            }

            // 3. Try by list URL
            try
            {
                string listUrl = web.ServerRelativeUrl.TrimEnd('/') + "/Lists/" + listName;
                list = web.GetList(listUrl);
                if (list != null) return list;
            }
            catch { }

            return null;
        }

        public static void EnsureListExists(string listName)
        {
            if (DgaPgListSchema.Get(listName) == null) return;
            if (DoneThisRequest("DgaPgList:" + listName)) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = DgaPgTargetWeb.Open(site))
                {
                    if (web == null) return;
                    EnsureOnResolvedWeb(site, web, listName);
                }

                MarkDoneThisRequest("DgaPgList:" + listName);
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgListProvisioner.EnsureListExists:" + listName, ex);
            }
        }

        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest("DgaPgAllLists")) return;

            try
            {
                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = DgaPgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    foreach (string listName in DgaPgListNames.All)
                    {
                        try
                        {
                            EnsureOnResolvedWeb(site, web, listName);
                            MarkDoneThisRequest("DgaPgList:" + listName);
                        }
                        catch (Exception exList)
                        {
                            DgaPgLog.Write("DgaPgListProvisioner.EnsureAllListsExist:" + listName, exList);
                        }
                    }
                }

                MarkDoneThisRequest("DgaPgAllLists");
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgListProvisioner.EnsureAllListsExist", ex);
            }
        }

        private static void EnsureOnResolvedWeb(SPSite site, SPWeb web, string listName)
        {
            DgaPgListDef def = DgaPgListSchema.Get(listName);
            if (def == null) return;

            if (FindList(web, def.Name) == null)
                Invalidate(web.ID, def.Name);

            EnsureListOnWeb(site.ID, web.ID, def);
        }

        private static bool DoneThisRequest(string key)
        {
            try
            {
                var ctx = System.Web.HttpContext.Current;
                return ctx != null && ctx.Items.Contains("DgaPgProv:" + key);
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest(string key)
        {
            try
            {
                var ctx = System.Web.HttpContext.Current;
                if (ctx != null) ctx.Items["DgaPgProv:" + key] = true;
            }
            catch { }
        }

        private static void Invalidate(Guid webId, string listName)
        {
            lock (_lock)
            {
                _provisioned.Remove(webId + ":" + listName);
            }
        }

        public static SPField GetFieldSafely(SPList list, string internalName)
        {
            if (list == null || string.IsNullOrEmpty(internalName)) return null;
            try
            {
                if (list.Fields.ContainsField(internalName))
                {
                    return list.Fields.GetFieldByInternalName(internalName);
                }
            }
            catch { }
            return null;
        }

        private static void EnsureListOnWeb(Guid siteId, Guid webId, DgaPgListDef def)
        {
            string cacheKey = webId + ":" + def.Name;
            lock (_lock)
            {
                if (_provisioned.Contains(cacheKey)) return;
            }

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite elevatedSite = new SPSite(siteId))
                using (SPWeb elevatedWeb = elevatedSite.OpenWeb(webId))
                {
                    elevatedWeb.AllowUnsafeUpdates = true;

                    SPList list = FindList(elevatedWeb, def.Name);
                    bool wasCreated = false;

                    if (list == null)
                    {
                        Guid listId = elevatedWeb.Lists.Add(
                            def.Name,
                            def.Description ?? def.DisplayTitle ?? def.Name,
                            SPListTemplateType.GenericList);

                        list = elevatedWeb.Lists[listId];
                        list.Title = def.DisplayTitle ?? def.Name;
                        list.OnQuickLaunch = false;
                        list.EnableVersioning = false;
                        list.Update();
                        wasCreated = true;
                    }

                    bool schemaChanged = false;
                    foreach (DgaPgFieldDef f in def.Fields)
                    {
                        if (!list.Fields.ContainsField(f.Name))
                        {
                            AddFieldToList(list, f);
                            schemaChanged = true;
                        }
                        else if (f.Type == SPFieldType.Choice && f.Choices != null && f.Choices.Length > 0)
                        {
                            try
                            {
                                SPFieldChoice choiceField = GetFieldSafely(list, f.Name) as SPFieldChoice;
                                if (choiceField != null)
                                {
                                    bool choiceAdded = false;
                                    foreach (string c in f.Choices)
                                    {
                                        if (!choiceField.Choices.Contains(c))
                                        {
                                            choiceField.Choices.Add(c);
                                            choiceAdded = true;
                                        }
                                    }
                                    if (choiceAdded) choiceField.Update();
                                }
                            }
                            catch { }
                        }
                    }

                    if (schemaChanged)
                    {
                        list.Update();
                        AddFieldsToDefaultView(list, def.Fields);
                    }

                    if (def.SeedDataFactory != null)
                    {
                        if (wasCreated || list.ItemCount == 0)
                        {
                            SeedData(list, def.SeedDataFactory());
                        }
                        else
                        {
                            SyncMissingSeedData(list, def.SeedDataFactory());
                        }
                    }

                    elevatedWeb.AllowUnsafeUpdates = false;

                    lock (_lock)
                    {
                        _provisioned.Add(cacheKey);
                    }
                }
            });
        }

        private static void AddFieldToList(SPList list, DgaPgFieldDef f)
        {
            try
            {
                if (f.Type == SPFieldType.Choice && f.Choices != null && f.Choices.Length > 0)
                {
                    System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
                    sc.AddRange(f.Choices);
                    list.Fields.Add(f.Name, SPFieldType.Choice, f.Required, false, sc);
                }
                else
                {
                    list.Fields.Add(f.Name, f.Type, f.Required);
                }

                SPField field = GetFieldSafely(list, f.Name);
                if (field != null)
                {
                    if (!string.IsNullOrEmpty(f.DisplayName))
                    {
                        field.Title = f.DisplayName;
                    }

                    if (f.Type == SPFieldType.Note)
                    {
                        SPFieldMultiLineText note = field as SPFieldMultiLineText;
                        if (note != null)
                        {
                            note.RichText = false;
                            note.NumberOfLines = 6;
                        }
                    }
                    else if (f.Type == SPFieldType.Choice && !string.IsNullOrEmpty(f.DefaultValue))
                    {
                        SPFieldChoice choice = field as SPFieldChoice;
                        if (choice != null)
                        {
                            choice.DefaultValue = f.DefaultValue;
                        }
                    }
                    else if (f.Type == SPFieldType.Boolean && !string.IsNullOrEmpty(f.DefaultValue))
                    {
                        SPFieldBoolean boolField = field as SPFieldBoolean;
                        if (boolField != null)
                        {
                            boolField.DefaultValue = f.DefaultValue;
                        }
                    }

                    field.Update();
                }
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgListProvisioner.AddFieldToList:" + f.Name, ex);
            }
        }

        private static void AddFieldsToDefaultView(SPList list, List<DgaPgFieldDef> fields)
        {
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                foreach (DgaPgFieldDef f in fields)
                {
                    if (!view.ViewFields.Exists(f.Name))
                    {
                        view.ViewFields.Add(f.Name);
                    }
                }
                view.Update();
            }
            catch { }
        }

        public static int SeedData(SPList list, List<Dictionary<string, object>> rows)
        {
            if (list == null || rows == null || rows.Count == 0) return 0;

            int count = 0;
            foreach (var row in rows)
            {
                try
                {
                    SPListItem item = list.Items.Add();
                    foreach (var kvp in row)
                    {
                        if (list.Fields.ContainsField(kvp.Key))
                        {
                            object val = kvp.Value;
                            SPField field = GetFieldSafely(list, kvp.Key);
                            if (field != null && field.Type == SPFieldType.Text && val is string strVal)
                            {
                                string clean = Uri.UnescapeDataString(strVal);
                                if (clean.Length > 255) clean = clean.Substring(0, 255);
                                item[kvp.Key] = clean;
                            }
                            else
                            {
                                item[kvp.Key] = val;
                            }
                        }
                    }
                    item.Update();
                    count++;
                }
                catch (Exception ex)
                {
                    DgaPgLog.Write("DgaPgListProvisioner.SeedData item error", ex);
                }
            }
            return count;
        }

        public static int SyncMissingSeedData(SPList list, List<Dictionary<string, object>> rows)
        {
            if (list == null || rows == null || rows.Count == 0) return 0;

            int changed = 0;
            try
            {
                var existingItemsByTitlePhase = new Dictionary<string, SPListItem>(StringComparer.OrdinalIgnoreCase);
                var existingItemsByTitle = new Dictionary<string, SPListItem>(StringComparer.OrdinalIgnoreCase);

                foreach (SPListItem item in list.Items)
                {
                    string t = Convert.ToString(item["Title"] ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(t)) continue;

                    string phase = list.Fields.ContainsField("Phase") ? Convert.ToString(item["Phase"] ?? string.Empty).Trim() : string.Empty;
                    string key = t + "::" + phase;

                    if (!existingItemsByTitlePhase.ContainsKey(key))
                        existingItemsByTitlePhase[key] = item;

                    if (!existingItemsByTitle.ContainsKey(t))
                        existingItemsByTitle[t] = item;
                }

                foreach (var row in rows)
                {
                    string title = row.ContainsKey("Title") ? Convert.ToString(row["Title"] ?? string.Empty).Trim() : string.Empty;
                    string phase = row.ContainsKey("Phase") ? Convert.ToString(row["Phase"] ?? string.Empty).Trim() : string.Empty;
                    string key = title + "::" + phase;

                    SPListItem matched = null;
                    if (existingItemsByTitlePhase.TryGetValue(key, out SPListItem pItem))
                    {
                        matched = pItem;
                    }
                    else if (existingItemsByTitle.TryGetValue(title, out SPListItem tItem))
                    {
                        matched = tItem;
                    }

                    if (matched == null)
                    {
                        // Missing item! Insert it automatically into SharePoint list.
                        try
                        {
                            SPListItem newItem = list.Items.Add();
                            foreach (var kvp in row)
                            {
                                if (list.Fields.ContainsField(kvp.Key))
                                {
                                    object val = kvp.Value;
                                    SPField field = GetFieldSafely(list, kvp.Key);
                                    if (field != null && field.Type == SPFieldType.Text && val is string strVal)
                                    {
                                        string clean = Uri.UnescapeDataString(strVal);
                                        if (clean.Length > 255) clean = clean.Substring(0, 255);
                                        newItem[kvp.Key] = clean;
                                    }
                                    else
                                    {
                                        newItem[kvp.Key] = val;
                                    }
                                }
                            }
                            newItem.Update();
                            existingItemsByTitlePhase[key] = newItem;
                            existingItemsByTitle[title] = newItem;
                            changed++;
                        }
                        catch (Exception ex)
                        {
                            DgaPgLog.Write("DgaPgListProvisioner.SyncMissingSeedData insert error", ex);
                        }
                    }
                    else
                    {
                        // Item exists: check if key attributes need backfilling
                        bool itemModified = false;
                        if (row.ContainsKey("SubGroupKey") && list.Fields.ContainsField("SubGroupKey"))
                        {
                            string currSubGroup = Convert.ToString(matched["SubGroupKey"] ?? string.Empty).Trim();
                            string targetSubGroup = Convert.ToString(row["SubGroupKey"] ?? string.Empty).Trim();
                            if (string.IsNullOrEmpty(currSubGroup) && !string.IsNullOrEmpty(targetSubGroup))
                            {
                                matched["SubGroupKey"] = targetSubGroup;
                                itemModified = true;
                            }
                        }

                        if (row.ContainsKey("DirectServiceUrl") && list.Fields.ContainsField("DirectServiceUrl"))
                        {
                            string currUrl = Convert.ToString(matched["DirectServiceUrl"] ?? string.Empty).Trim();
                            string targetUrl = Convert.ToString(row["DirectServiceUrl"] ?? string.Empty).Trim();
                            if (string.IsNullOrEmpty(currUrl) && !string.IsNullOrEmpty(targetUrl))
                            {
                                matched["DirectServiceUrl"] = targetUrl;
                                itemModified = true;
                            }
                        }

                        if (itemModified)
                        {
                            matched.Update();
                            changed++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgListProvisioner.SyncMissingSeedData", ex);
            }

            return changed;
        }

        public static int ReSeedList(string listName)
        {
            DgaPgListDef def = DgaPgListSchema.Get(listName);
            if (def == null || def.SeedDataFactory == null) return 0;

            int seeded = 0;
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                if (SPContext.Current == null) return;
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = DgaPgTargetWeb.Open(site))
                {
                    if (web == null) return;
                    web.AllowUnsafeUpdates = true;

                    SPList list = FindList(web, def.Name);
                    if (list == null)
                    {
                        EnsureListOnWeb(site.ID, web.ID, def);
                        list = FindList(web, def.Name);
                    }

                    if (list != null)
                    {
                        // Add any columns the definition has gained since the list was created.
                        bool schemaChanged = false;
                        foreach (DgaPgFieldDef f in def.Fields)
                        {
                            if (!list.Fields.ContainsField(f.Name))
                            {
                                AddFieldToList(list, f);
                                schemaChanged = true;
                            }
                        }

                        if (schemaChanged)
                        {
                            list.Update();
                            AddFieldsToDefaultView(list, def.Fields);
                        }

                        while (list.ItemCount > 0)
                        {
                            SPListItemCollection items = list.GetItems(new SPQuery { RowLimit = 100 });
                            if (items.Count == 0) break;
                            for (int i = items.Count - 1; i >= 0; i--)
                            {
                                items[i].Delete();
                            }
                        }

                        seeded = SeedData(list, def.SeedDataFactory());
                    }
                    web.AllowUnsafeUpdates = false;
                }
            });

            return seeded;
        }
    }
}
