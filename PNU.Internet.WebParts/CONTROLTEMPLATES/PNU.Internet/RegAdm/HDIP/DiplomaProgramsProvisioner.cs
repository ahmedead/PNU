using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.HDIP
{
    public static class DiplomaProgramsProvisioner
    {
        public const string ContentListName = "DiplomaProgramsContent";
        public const string LinksListName = "DiplomaProgramsLinks";

        public static void EnsureLists()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        EnsureContentList(web);
                        EnsureLinksList(web);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Content List

        private static void EnsureContentList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ContentListName);
            if (list == null)
            {
                Guid id = web.Lists.Add(ContentListName, "Diploma programs overview content", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }

            AddTextField(list, "TitleAr");
            AddTextField(list, "TitleEn");
            AddNoteField(list, "DescriptionAr");
            AddNoteField(list, "DescriptionEn");
            AddNoteField(list, "BulletsAr");   // one bullet per line
            AddNoteField(list, "BulletsEn");
            AddTextField(list, "ImageUrl");
            AddTextField(list, "ImageCaptionAr");
            AddTextField(list, "ImageCaptionEn");
            AddTextField(list, "SectionTitleAr");
            AddTextField(list, "SectionTitleEn");

            if (list.ItemCount == 0)
                SeedContent(list);
        }

        private static void SeedContent(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "DiplomaPrograms";
            item["TitleAr"] = "برامج الدبلوم";
            item["TitleEn"] = "Diploma Programs";
            item["DescriptionAr"] = "تتم إجراءات القبول في برامج الدبلوم في الكلية التطبيقية في جامعة الأميرة نورة بنت عبد الرحمن من خلال بوابة القبول الإلكترونية. تعرض الصفحة الرسمية البرامج المجانية والبرامج الخاصة، إضافة إلى الضوابط الأكاديمية والمالية المرتبطة بالاعتذار والتأجيل والانسحاب واسترداد الرسوم.";
            item["DescriptionEn"] = "Admission to diploma programs at the Applied College of Princess Nourah bint Abdulrahman University is processed through the electronic admission portal. The official page presents free and paid programs, in addition to the academic and financial regulations related to apology, postponement, withdrawal, and fee refunds.";
            item["BulletsAr"] = "برامج مجانية وبرامج خاصة ضمن الكلية التطبيقية.\nضوابط الاعتذار والتأجيل والانسحاب واسترداد الرسوم.\nالرسوم الدراسية والإعفاء من مقررات اللغة الإنجليزية وآلية طلب المعادلة.";
            item["BulletsEn"] = "Free and paid programs within the Applied College.\nRegulations for apology, postponement, withdrawal, and fee refunds.\nTuition fees, exemption from English language courses, and the equivalency request mechanism.";
            item["ImageUrl"] = "/Style Library/DGA/public/images/hero/hero-library-lg.avif";
            item["ImageCaptionAr"] = "مشهد من مرافق الجامعة يعكس اتساع الحرم الجامعي والطابع المعماري للمباني الرئيسة.";
            item["ImageCaptionEn"] = "A view of university facilities reflecting the breadth of the campus and the architectural character of the main buildings.";
            item["SectionTitleAr"] = "الالتحاق بالجامعة";
            item["SectionTitleEn"] = "Joining the University";
            item.Update();
        }

        #endregion

        #region Links List

        private static void EnsureLinksList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LinksListName);
            if (list == null)
            {
                Guid id = web.Lists.Add(LinksListName, "Diploma programs navigation cards", SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }

            AddTextField(list, "TitleAr");
            AddTextField(list, "TitleEn");
            AddNoteField(list, "DescriptionAr");
            AddNoteField(list, "DescriptionEn");
            AddTextField(list, "IconClass");
            AddTextField(list, "LinkUrlAr");
            AddTextField(list, "LinkUrlEn");
            AddNumberField(list, "DisplayOrder");
            AddBoolField(list, "IsActive");

            if (list.ItemCount == 0)
                SeedLinks(list);
        }

        private static void SeedLinks(SPList list)
        {
            string[,] data = {
                { "البرامج المجانية", "Free Programs",
                  "مسار البرامج المجانية المعلن ضمن صفحة الدبلوم الرسمية.", "The free programs track announced on the official diploma page.",
                  "hgi-book-01",
                  "/ar/RegAdm/HDIP/Pages/Free-software.aspx",
                  "/en/RegAdm/HDIP/Pages/Free-software.aspx" },
                { "البرامج الخاصة", "Paid Programs",
                  "معلومات البرامج الخاصة وضوابطها ضمن الكلية التطبيقية.", "Information and regulations of paid programs within the Applied College.",
                  "hgi-wallet-02",
                  "/ar/RegAdm/HDIP/Pages/Paid-programs.aspx",
                  "/en/RegAdm/HDIP/Pages/Paid-programs.aspx" },
                { "الاعتذار والتأجيل", "Apology and Postponement",
                  "ضوابط الاعتذار والتأجيل للبرامج الخاصة.", "Apology and postponement regulations for paid programs.",
                  "hgi-calendar-03",
                  "/ar/RegAdm/HDIP/Pages/Apology-and-postponement-of-paid-programs.aspx",
                  "/en/RegAdm/HDIP/Pages/Apology-and-postponement-of-paid-programs.aspx" },
                { "تقويم الإجراءات", "Procedures Calendar",
                  "تقويم الإجراءات الأكاديمية والمالية للبرامج الخاصة.", "Calendar of academic and financial procedures for paid programs.",
                  "hgi-task-01",
                  "/ar/RegAdm/HDIP/Pages/Evaluation-of-academic-and-financial-procedures-for-the-year-1445-AH---second-semester.aspx",
                  "/en/RegAdm/HDIP/Pages/Evaluation-of-academic-and-financial-procedures-for-the-year-1445-AH---second-semester.aspx" },
                { "الانسحاب واسترداد الرسوم", "Withdrawal and Fee Refund",
                  "ضوابط الانسحاب واسترداد الرسوم للبرامج الخاصة.", "Withdrawal and fee refund regulations for paid programs.",
                  "hgi-arrow-turn-backward",
                  "/ar/RegAdm/HDIP/Pages/Withdrawing-from-paid-programs.aspx",
                  "/en/RegAdm/HDIP/Pages/Withdrawing-from-paid-programs.aspx" },
                { "الرسوم الدراسية", "Tuition Fees",
                  "الرسوم الدراسية للبرامج الخاصة.", "Tuition fees for paid programs.",
                  "hgi-money-bag-02",
                  "/ar/RegAdm/HDIP/Pages/Tuition-fees-for-paid-programs---two-semester-system.aspx",
                  "/en/RegAdm/HDIP/Pages/Tuition-fees-for-paid-programs---two-semester-system.aspx" },
                { "الإعفاء من اللغة الإنجليزية", "English Language Exemption",
                  "الإعفاء من مقررات اللغة الإنجليزية وفق الضوابط المنشورة.", "Exemption from English language courses per the published regulations.",
                  "hgi-validation-approval",
                  "/ar/RegAdm/HDIP/Pages/Exemption-from-English-language-courses.aspx",
                  "/en/RegAdm/HDIP/Pages/Exemption-from-English-language-courses.aspx" },
                { "طلب المعادلة", "Equivalency Request",
                  "آلية طلب المعادلة للبرامج الخاصة وشروطها.", "Equivalency request mechanism for paid programs and its conditions.",
                  "hgi-file-verified",
                  "/ar/RegAdm/HDIP/Pages/Mechanism-for-requesting-equivalency-for-paid-programs-and-its-conditions.aspx",
                  "/en/RegAdm/HDIP/Pages/Mechanism-for-requesting-equivalency-for-paid-programs-and-its-conditions.aspx" }
            };

            for (int i = 0; i < data.GetLength(0); i++)
            {
                SPListItem item = list.AddItem();
                item["Title"] = data[i, 1];
                item["TitleAr"] = data[i, 0];
                item["TitleEn"] = data[i, 1];
                item["DescriptionAr"] = data[i, 2];
                item["DescriptionEn"] = data[i, 3];
                item["IconClass"] = data[i, 4];
                item["LinkUrlAr"] = data[i, 5];
                item["LinkUrlEn"] = data[i, 6];
                item["DisplayOrder"] = i + 1;
                item["IsActive"] = true;
                item.Update();
            }
        }

        #endregion

        #region Field Helpers

        private static void AddTextField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Text, false);
                list.Update();
            }
        }

        private static void AddNoteField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Note, false);
                SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields[name];
                f.RichText = false;
                f.Update();
                list.Update();
            }
        }

        private static void AddNumberField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Number, false);
                list.Update();
            }
        }

        private static void AddBoolField(SPList list, string name)
        {
            if (!list.Fields.ContainsField(name))
            {
                list.Fields.Add(name, SPFieldType.Boolean, false);
                SPFieldBoolean f = (SPFieldBoolean)list.Fields[name];
                f.DefaultValue = "1";
                f.Update();
                list.Update();
            }
        }

        #endregion
    }
}
