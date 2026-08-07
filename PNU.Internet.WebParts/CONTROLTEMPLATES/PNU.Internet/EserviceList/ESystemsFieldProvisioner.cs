using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public static class ESystemsFieldProvisioner
    {
        /// <summary>Use inside a page/control context (uses SPContext).</summary>
        public static void EnsureFields()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        EnsureFieldsOnWeb(web);
                    }
                }
            });
        }

        /// <summary>Use from a feature receiver — pass the (already elevated) Admin web.</summary>
        public static void EnsureFieldsOnWeb(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("ESystems");
            if (list == null) return;

            web.AllowUnsafeUpdates = true;

            // 1. Icon class (e.g. hgi-mail-01, hgi-book-open-01)
            AddTextField(list, "IconClass", "Icon Class");

            // 2. Category / tag badge (bilingual)
            AddTextField(list, "Category", "Category (AR)");
            AddTextField(list, "Category_EN", "Category (EN)");

            // 3. Target audience (bilingual multi-choice — badges + filter)
            AddMultiChoiceField(list, "TargetAudience", "Target Audience (AR)",
                new[] { "الطالبات", "أعضاء هيئة التدريس", "الموظفات", "الزوار", "القيادات", "المتقدمات", "الجميع" });

            AddMultiChoiceField(list, "TargetAudience_EN", "Target Audience (EN)",
                new[] { "Students", "Faculty Members", "Staff", "Visitors", "Leadership", "Applicants", "All" });

            AddFieldsToDefaultView(list,
                new[] { "IconClass", "Category", "Category_EN", "TargetAudience", "TargetAudience_EN" });

            web.AllowUnsafeUpdates = false;
        }

        private static void AddTextField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return;

            list.Fields.Add(internalName, SPFieldType.Text, false);
            SPField field = list.Fields.GetFieldByInternalName(internalName);
            field.Title = displayName;
            field.Update();
            list.Update();
        }

        private static void AddMultiChoiceField(SPList list, string internalName, string displayName, string[] choices)
        {
            if (list.Fields.ContainsField(internalName)) return;

            list.Fields.Add(internalName, SPFieldType.MultiChoice, false);
            SPFieldMultiChoice field = (SPFieldMultiChoice)list.Fields.GetFieldByInternalName(internalName);
            field.Title = displayName;
            field.Choices.Clear();
            foreach (string choice in choices)
            {
                field.Choices.Add(choice);
            }
            field.Update();
            list.Update();
        }

        private static void AddFieldsToDefaultView(SPList list, string[] internalNames)
        {
            SPView view = list.DefaultView;
            bool changed = false;

            foreach (string name in internalNames)
            {
                if (!view.ViewFields.Exists(name) && list.Fields.ContainsField(name))
                {
                    view.ViewFields.Add(list.Fields.GetFieldByInternalName(name));
                    changed = true;
                }
            }

            if (changed) view.Update();
        }
    }
}
