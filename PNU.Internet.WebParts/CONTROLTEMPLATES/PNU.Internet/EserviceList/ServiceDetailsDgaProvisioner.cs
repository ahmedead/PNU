using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public static class ServiceDetailsDgaProvisioner
    {
        public static void EnsureAll(string siteUrl)
        {
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
        }

        // ------------------------------------------------------------
        // 1) EservicesList — new DGA columns + default view
        // ------------------------------------------------------------
        private static void EnsureEservicesListFields(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("EservicesList");
            if (list == null) return;

            // Steps tab (rich text)
            AddRichNoteField(list, "Steps", "Steps (AR)");
            AddRichNoteField(list, "Steps_EN", "Steps (EN)");

            // Required documents tab (rich text)
            AddRichNoteField(list, "RequiredDocs", "Required Documents (AR)");
            AddRichNoteField(list, "RequiredDocs_EN", "Required Documents (EN)");

            // Side card — language + cost
            AddTextField(list, "ServiceLanguage", "Service Language (AR)");
            AddTextField(list, "ServiceLanguage_EN", "Service Language (EN)");
            AddTextField(list, "ServiceCost", "Service Cost (AR)");
            AddTextField(list, "ServiceCost_EN", "Service Cost (EN)");

            // Header badges (hidden automatically when empty)
            AddTextField(list, "Badge1", "Badge 1 (AR)");
            AddTextField(list, "Badge1_EN", "Badge 1 (EN)");
            AddTextField(list, "Badge2", "Badge 2 (AR)");
            AddTextField(list, "Badge2_EN", "Badge 2 (EN)");
            AddTextField(list, "Badge3", "Badge 3 (AR)");
            AddTextField(list, "Badge3_EN", "Badge 3 (EN)");

            list.Update();

            AddFieldsToDefaultView(list, new[]
            {
                "Steps", "Steps_EN",
                "RequiredDocs", "RequiredDocs_EN",
                "ServiceLanguage", "ServiceLanguage_EN",
                "ServiceCost", "ServiceCost_EN",
                "Badge1", "Badge1_EN",
                "Badge2", "Badge2_EN",
                "Badge3", "Badge3_EN"
            });
        }

        // ------------------------------------------------------------
        // 2) PageRatings list
        // ------------------------------------------------------------
        private static void EnsurePageRatingsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PageRatings");
            if (list == null)
            {
                Guid id = web.Lists.Add("PageRatings", "Page rating feedback (DGA)", SPListTemplateType.GenericList);
                list = web.Lists[id];
            }

            AddTextField(list, "PageUrl", "Page Url", indexed: true);
            AddNumberField(list, "RatingValue", "Rating Value", 1, 5);
            AddPlainNoteField(list, "Comment", "Comment");
            list.Update();

            AddFieldsToDefaultView(list, new[] { "PageUrl", "RatingValue", "Comment" });
        }

        // ------------------------------------------------------------
        // 3) PageSurveys list
        // ------------------------------------------------------------
        private static void EnsurePageSurveysList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PageSurveys");
            if (list == null)
            {
                Guid id = web.Lists.Add("PageSurveys", "Page helpfulness survey (DGA)", SPListTemplateType.GenericList);
                list = web.Lists[id];
            }

            AddTextField(list, "PageUrl", "Page Url", indexed: true);
            AddBooleanField(list, "IsHelpful", "Is Helpful");
            AddPlainNoteField(list, "Reasons", "Reasons");
            AddPlainNoteField(list, "Comment", "Comment");
            AddTextField(list, "Gender", "Gender");
            list.Update();

            AddFieldsToDefaultView(list, new[] { "PageUrl", "IsHelpful", "Reasons", "Comment", "Gender" });
        }

        // ============================================================
        // Field helpers (all guarded by ContainsField)
        // ============================================================
        private static void AddTextField(SPList list, string internalName, string displayName, bool indexed = false)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Text, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                if (indexed) f.Indexed = true;
                f.Update();
            }
        }

        private static void AddRichNoteField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Note, false);
                SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.RichText = true;
                f.RichTextMode = SPRichTextMode.FullHtml;
                f.Update();
            }
        }

        private static void AddPlainNoteField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Note, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.Update();
            }
        }

        private static void AddNumberField(SPList list, string internalName, string displayName, double min, double max)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Number, false);
                SPFieldNumber f = (SPFieldNumber)list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.MinimumValue = min;
                f.MaximumValue = max;
                f.Update();
            }
        }

        private static void AddBooleanField(SPList list, string internalName, string displayName)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Boolean, false);
                SPField f = list.Fields.GetFieldByInternalName(internalName);
                f.Title = displayName;
                f.Update();
            }
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
