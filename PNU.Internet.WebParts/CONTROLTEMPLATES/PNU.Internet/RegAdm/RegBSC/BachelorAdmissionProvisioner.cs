using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.RegBSC
{
    public static class BachelorAdmissionProvisioner
    {
        public const string ContentListName = "BachelorAdmissionContent";
        public const string LinksListName = "BachelorAdmissionLinks";

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
                Guid id = web.Lists.Add(ContentListName, "Bachelor admission overview content", SPListTemplateType.GenericList);
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
            AddTextField(list, "SectionTitleAr"); // "الالتحاق بالجامعة"
            AddTextField(list, "SectionTitleEn");

            if (list.ItemCount == 0)
                SeedContent(list);
        }

        private static void SeedContent(SPList list)
        {
            SPListItem item = list.AddItem();
            item["Title"] = "BachelorAdmission";
            item["TitleAr"] = "التقديم على البكالوريوس";
            item["TitleEn"] = "Bachelor Admission";
            item["DescriptionAr"] = "يتم التقديم على جامعة الأميرة نورة بنت عبد الرحمن عن طريق منصة القبول الموحد، وهي بوابة إلكترونية وطنية تمكن من التقديم على الجامعات السعودية الحكومية من مختلف مناطق المملكة في مكان واحد. تهدف المنصة إلى تسهيل عملية القبول ومنح فرص متساوية للجميع بتوفير معلومات دقيقة وشفافة حول التخصصات والنسب الموزونة ومتطلبات كل جامعة.";
            item["DescriptionEn"] = "Admission to Princess Nourah bint Abdulrahman University is done through the Unified Admission Platform, a national electronic portal that enables applying to Saudi government universities from all regions of the Kingdom in one place. The platform aims to simplify the admission process and grant equal opportunities by providing accurate and transparent information about majors, weighted percentages, and requirements of each university.";
            item["BulletsAr"] = "تقديم إلكتروني عبر منصة وطنية موحدة.\nمعلومات واضحة عن التخصصات والنسب الموزونة ومتطلبات القبول.\nمسارات للطالبات المستجدات والمنح والموهوبات والتحويل والزيارة.";
            item["BulletsEn"] = "Electronic application through a unified national platform.\nClear information about majors, weighted percentages, and admission requirements.\nTracks for new students, scholarships, gifted students, transfers, and visiting students.";
            item["ImageUrl"] = "/Style%20Library/DGA/public/images/hero/hero-library-lg.avif";
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
                Guid id = web.Lists.Add(LinksListName, "Bachelor admission navigation cards", SPListTemplateType.GenericList);
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
                { "الطالبات المستجدات", "New Students",
                  "إرشادات وبداية المسار للطالبات المقبولات.", "Guidance and onboarding for admitted students.",
                  "hgi-graduate-female", "/ar/RegAdm/RegBSC/Pages/dar.aspx", "/en/RegAdm/RegBSC/Pages/dar.aspx" },
                { "المنح الدراسية الداخلية", "Internal Scholarships",
                  "تفاصيل المنح الدراسية الداخلية بحسب الإعلانات الرسمية.", "Internal scholarship details per official announcements.",
                  "hgi-award-01", "/ar/RegAdm/RegBSC/Pages/Internal.aspx", "/en/RegAdm/RegBSC/Pages/Internal.aspx" },
                { "المنح الدراسية الخارجية", "External Scholarships",
                  "تفاصيل المنح الدراسية الخارجية بحسب الإعلانات الرسمية.", "External scholarship details per official announcements.",
                  "hgi-global", "/ar/RegAdm/RegBSC/Pages/External.aspx", "/en/RegAdm/RegBSC/Pages/External.aspx" },
                { "الطالبات الموهوبات", "Gifted Students",
                  "مسار الطالبات الموهوبات ضمن بوابة القبول في البكالوريوس.", "Gifted students track within the bachelor admission portal.",
                  "hgi-star", "/ar/RegAdm/RegBSC/Pages/Talented.aspx", "/en/RegAdm/RegBSC/Pages/Talented.aspx" },
                { "الزيارة من خارج الجامعة", "Visiting from Outside PNU",
                  "إجراءات الزيارة للطالبات من خارج جامعة الأميرة نورة.", "Visiting procedures for students from outside PNU.",
                  "hgi-location-01", "/ar/RegAdm/RegBSC/Pages/StudyOnVisitBasis.aspx", "/en/RegAdm/RegBSC/Pages/StudyOnVisitBasis.aspx" },
                { "التحويل للجامعة", "Transfer to PNU",
                  "التحويل لجامعة الأميرة نورة بنت عبد الرحمن.", "Transfer to Princess Nourah bint Abdulrahman University.",
                  "hgi-arrow-data-transfer-horizontal", "/ar/RegAdm/RegBSC/Pages/TransferToPNU.aspx", "/en/RegAdm/RegBSC/Pages/TransferToPNU.aspx" }
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
