using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public static class ServiceDetailsDgaProvisioner
    {
        // The service details page is public and calls EnsureAll on every view.
        // Run the checks once per process per site, and only write when something changed.
        private static readonly HashSet<string> _provisionedSites = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _lock = new object();

        public static void EnsureAll(string siteUrl)
        {
            if (string.IsNullOrEmpty(siteUrl)) return;
            lock (_lock)
            {
                if (_provisionedSites.Contains(siteUrl)) return;
            }

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteUrl))
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        EnsureEservicesListFields(web);
                        EnsurePageRatingsList(web);
                        EnsurePageSurveysList(web);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

            lock (_lock)
            {
                _provisionedSites.Add(siteUrl);
            }
        }

        public static void ForceProvision()
        {
            lock (_lock)
            {
                _provisionedSites.Clear();
            }
            if (SPContext.Current != null && SPContext.Current.Site != null)
            {
                EnsureAll(SPContext.Current.Site.Url);
            }
        }

        public static void EnsureAllListsExist()
        {
            if (SPContext.Current != null && SPContext.Current.Site != null)
            {
                EnsureAll(SPContext.Current.Site.Url);
            }
        }

        // ------------------------------------------------------------
        // 1) EservicesList — new DGA columns + default view
        // ------------------------------------------------------------
        private static void EnsureEservicesListFields(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("EservicesList");
            if (list == null) return;

            bool changed = false;

            // Steps tab (rich text)
            changed |= AddRichNoteField(list, "Steps", "Steps (AR)");
            changed |= AddRichNoteField(list, "Steps_EN", "Steps (EN)");

            // Required documents tab (rich text)
            changed |= AddRichNoteField(list, "RequiredDocs", "Required Documents (AR)");
            changed |= AddRichNoteField(list, "RequiredDocs_EN", "Required Documents (EN)");

            // Side card — language + cost
            changed |= AddTextField(list, "ServiceLanguage", "Service Language (AR)");
            changed |= AddTextField(list, "ServiceLanguage_EN", "Service Language (EN)");
            changed |= AddTextField(list, "ServiceCost", "Service Cost (AR)");
            changed |= AddTextField(list, "ServiceCost_EN", "Service Cost (EN)");

            // Header badges (hidden automatically when empty)
            changed |= AddTextField(list, "Badge1", "Badge 1 (AR)");
            changed |= AddTextField(list, "Badge1_EN", "Badge 1 (EN)");
            changed |= AddTextField(list, "Badge2", "Badge 2 (AR)");
            changed |= AddTextField(list, "Badge2_EN", "Badge 2 (EN)");
            changed |= AddTextField(list, "Badge3", "Badge 3 (AR)");
            changed |= AddTextField(list, "Badge3_EN", "Badge 3 (EN)");

            // English URL
            changed |= AddPlainNoteField(list, "URL_EN", "URL (EN)");

            // Service Agreement URL (EN) - default to PNU SLA page
            changed |= AddTextField(list, "ServiceAgreementURL_EN", "Service Agreement URL (EN)", defaultValue: "https://pnu.edu.sa/en/Pages/SLA.aspx");

            if (changed) list.Update();

            AddFieldsToDefaultView(list, new[]
            {
                "Steps", "Steps_EN",
                "RequiredDocs", "RequiredDocs_EN",
                "ServiceLanguage", "ServiceLanguage_EN",
                "ServiceCost", "ServiceCost_EN",
                "Badge1", "Badge1_EN",
                "Badge2", "Badge2_EN",
                "Badge3", "Badge3_EN",
                "URL_EN",
                "ServiceAgreementURL_EN"
            });
        }

        // ------------------------------------------------------------
        // 2) PageRatings list
        // ------------------------------------------------------------
        private static void EnsurePageRatingsList(SPWeb web)
        {
            bool changed = false;
            SPList list = web.Lists.TryGetList("PageRatings");
            if (list == null)
            {
                Guid id = web.Lists.Add("PageRatings", "Page rating feedback (DGA)", SPListTemplateType.GenericList);
                list = web.Lists[id];
                changed = true;
            }

            changed |= AddTextField(list, "PageUrl", "Page Url", indexed: true);
            changed |= AddNumberField(list, "RatingValue", "Rating Value", 1, 5);
            changed |= AddPlainNoteField(list, "Comment", "Comment");
            if (changed) list.Update();

            AddFieldsToDefaultView(list, new[] { "PageUrl", "RatingValue", "Comment" });
        }

        // ------------------------------------------------------------
        // 3) PageSurveys list
        // ------------------------------------------------------------
        private static void EnsurePageSurveysList(SPWeb web)
        {
            bool changed = false;
            SPList list = web.Lists.TryGetList("PageSurveys");
            if (list == null)
            {
                Guid id = web.Lists.Add("PageSurveys", "Page helpfulness survey (DGA)", SPListTemplateType.GenericList);
                list = web.Lists[id];
                changed = true;
            }

            changed |= AddTextField(list, "PageUrl", "Page Url", indexed: true);
            changed |= AddBooleanField(list, "IsHelpful", "Is Helpful");
            changed |= AddPlainNoteField(list, "Reasons", "Reasons");
            changed |= AddPlainNoteField(list, "Comment", "Comment");
            changed |= AddTextField(list, "Gender", "Gender");
            if (changed) list.Update();

            AddFieldsToDefaultView(list, new[] { "PageUrl", "IsHelpful", "Reasons", "Comment", "Gender" });
        }

        // ============================================================
        // Field helpers (all guarded by ContainsField; return true when a field was added)
        // ============================================================
        private static bool AddTextField(SPList list, string internalName, string displayName, bool indexed = false, string defaultValue = null)
        {
            if (list.Fields.ContainsField(internalName))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    try
                    {
                        SPField existing = list.Fields.GetFieldByInternalName(internalName);
                        if (string.IsNullOrEmpty(existing.DefaultValue))
                        {
                            existing.DefaultValue = defaultValue;
                            existing.Update();
                        }
                    }
                    catch { }
                }
                return false;
            }

            list.Fields.Add(internalName, SPFieldType.Text, false);
            SPField f = list.Fields.GetFieldByInternalName(internalName);
            f.Title = displayName;
            if (!string.IsNullOrEmpty(defaultValue))
                f.DefaultValue = defaultValue;
            if (indexed) f.Indexed = true;
            f.Update();
            return true;
        }

        private static bool AddRichNoteField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return false;

            list.Fields.Add(internalName, SPFieldType.Note, false);
            SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
            f.Title = displayName;
            f.RichText = true;
            f.RichTextMode = SPRichTextMode.FullHtml;
            f.Update();
            return true;
        }

        private static bool AddPlainNoteField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return false;

            list.Fields.Add(internalName, SPFieldType.Note, false);
            SPField f = list.Fields.GetFieldByInternalName(internalName);
            f.Title = displayName;
            f.Update();
            return true;
        }

        private static bool AddNumberField(SPList list, string internalName, string displayName, double min, double max)
        {
            if (list.Fields.ContainsField(internalName)) return false;

            list.Fields.Add(internalName, SPFieldType.Number, false);
            SPFieldNumber f = (SPFieldNumber)list.Fields.GetFieldByInternalName(internalName);
            f.Title = displayName;
            f.MinimumValue = min;
            f.MaximumValue = max;
            f.Update();
            return true;
        }

        private static bool AddBooleanField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return false;

            list.Fields.Add(internalName, SPFieldType.Boolean, false);
            SPField f = list.Fields.GetFieldByInternalName(internalName);
            f.Title = displayName;
            f.Update();
            return true;
        }

        // ============================================================
        // Default view helper
        // ============================================================
        private static void AddFieldsToDefaultView(SPList list, string[] internalNames)
        {
            SPView view = list.DefaultView;
            bool changed = false;

            foreach (string name in internalNames)
            {
                if (!list.Fields.ContainsField(name)) continue;
                if (!view.ViewFields.Exists(name))
                {
                    view.ViewFields.Add(list.Fields.GetFieldByInternalName(name));
                    changed = true;
                }
            }

            if (changed) view.Update();
        }
    }

}
