using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD
{
    public static class PostgraduateProgramsProvisioner
    {
        public const string ContentListName = "PostgraduateProgramsContent";
        public const string LinksListName = "PostgraduateProgramsLinks";

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
                Guid id = web.Lists.Add(ContentListName, "Postgraduate programs overview content", SPListTemplateType.GenericList);
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
            item["Title"] = "PostgraduatePrograms";
            item["TitleAr"] = "برامج الدراسات العليا";
            item["TitleEn"] = "Postgraduate Programs";
            item["DescriptionAr"] = "تعد برامج الدراسات العليا في جامعة الأميرة نورة بنت عبد الرحمن ركيزة أساسية في مسيرة التميز الأكاديمي وتمكين الكفاءات الوطنية، إذ تمثل مرحلة متقدمة من التعليم العالي تعنى بتعميق المعرفة التخصصية وتوسيع آفاق البحث والتحليل. تتميز البرامج بإقامة شراكات استراتيجية فاعلة على المستويين المحلي والدولي، بما يسهم في تعزيز جودة البرامج الأكاديمية وتبادل الخبرات ودعم البحث والابتكار.";
            item["DescriptionEn"] = "Postgraduate programs at Princess Nourah bint Abdulrahman University are a cornerstone of academic excellence and national talent empowerment, representing an advanced stage of higher education dedicated to deepening specialized knowledge and broadening horizons of research and analysis. The programs are distinguished by active strategic partnerships at the local and international levels, contributing to enhancing academic program quality, exchanging expertise, and supporting research and innovation.";
            item["BulletsAr"] = "تعميق المعرفة التخصصية ورفع كفاءة الأداء المهني والعلمي.\nإعداد كوادر مؤهلة لخدمة المجتمع ودعم مستهدفات التنمية الوطنية.\nفرص تعاون علمي وتدريب ومشروعات بحثية مشتركة.";
            item["BulletsEn"] = "Deepening specialized knowledge and raising professional and scientific performance.\nPreparing qualified cadres to serve the community and support national development goals.\nOpportunities for scientific cooperation, training, and joint research projects.";
            item["ImageUrl"] = "/Style Library/DGA/public/images/hero/hero-library-lg.avif";
            item["ImageCaptionAr"] = "مشهد من مرافق الجامعة يعكس اتساع الحرم الجامعي والطابع المعماري للمباني الرئيسة.";
            item["ImageCaptionEn"] = "A view of university facilities reflecting the breadth of the campus and the architectural character of the main buildings.";
            item["SectionTitleAr"] = "برامج وروابط الدراسات العليا";
            item["SectionTitleEn"] = "Postgraduate Programs and Links";
            item.Update();
        }

        #endregion

        #region Links List

        private static void EnsureLinksList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(LinksListName);
            if (list == null)
            {
                Guid id = web.Lists.Add(LinksListName, "Postgraduate programs navigation cards", SPListTemplateType.GenericList);
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
                { "برامج الدراسات العليا", "Postgraduate Programs",
                  "الاطلاع على البرامج والتفاصيل المعلنة رسميًا.", "View the officially announced programs and details.",
                  "hgi-diploma",
                  "/ar/RegAdm/PGD/Pages/ProgramsDetails.aspx?CatID=3",
                  "/en/RegAdm/PGD/Pages/ProgramsDetails.aspx?CatID=3" },
                { "كوادر نورة", "Nourah Cadres",
                  "مسار مرتبط بتطوير الكفاءات ضمن منظومة الدراسات العليا.", "A track for developing competencies within the postgraduate system.",
                  "hgi-user-group",
                  "/ar/RegAdm/PGD/Pages/pnur.aspx",
                  "/en/RegAdm/PGD/Pages/pnur.aspx" },
                { "جامعة سوانزي", "Swansea University",
                  "برامج دراسات عليا بالشراكة مع جامعة سوانزي.", "Postgraduate programs in partnership with Swansea University.",
                  "hgi-agreement-01",
                  "/ar/RegAdm/PGD/Pages/prog2.aspx",
                  "/en/RegAdm/PGD/Pages/prog2.aspx" },
                { "جامعة مدينة دبلن", "Dublin City University",
                  "برامج دراسات عليا بالشراكة مع جامعة مدينة دبلن.", "Postgraduate programs in partnership with Dublin City University.",
                  "hgi-libraries",
                  "/ar/RegAdm/PGD/Pages/prog11.aspx",
                  "/en/RegAdm/PGD/Pages/prog11.aspx" },
                { "جامعة ستراثكلايد", "University of Strathclyde",
                  "برامج دراسات عليا بالشراكة مع جامعة ستراثكلايد.", "Postgraduate programs in partnership with the University of Strathclyde.",
                  "hgi-university",
                  "/ar/RegAdm/PGD/Pages/prog3.aspx",
                  "/en/RegAdm/PGD/Pages/prog3.aspx" },
                { "بوابة القبول الإلكترونية", "Electronic Admission Portal",
                  "التقديم عبر بوابة القبول الإلكترونية عند إتاحة التقديم.", "Apply through the electronic admission portal when applications are open.",
                  "hgi-link-04",
                  "https://banssb.pnu.edu.sa/PROD_ar/bwykolad.P_ShowQuesAns?applno=MTQ0MzEwMDAwMDA3Nzgx",
                  "https://banssb.pnu.edu.sa/PROD_ar/bwykolad.P_ShowQuesAns?applno=MTQ0MzEwMDAwMDA3Nzgx" }
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
