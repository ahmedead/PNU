using System;
using System.Web;
using Microsoft.SharePoint;
using static PNU.Internet.WebParts.SPFactory;

namespace PNU.Internet.WebParts
{
    public class PartnerItem
    {
        public int ID { get; set; }
        public string Title { get; set; }        // Arabic name (aria-label / alt)
        public string TitleEn { get; set; }
        public string LinkUrl { get; set; }
        public string LogoUrl { get; set; }
        public double SortOrder { get; set; }
    }

    public class AccreditationItem
    {
        public int ID { get; set; }
        public string Title { get; set; }        // Arabic title
        public string TitleEn { get; set; }

        [SPField(SPFieldType.Note)]
        public string Description { get; set; }

        [SPField(SPFieldType.Note)]
        public string DescriptionEn { get; set; }

        public string ImageUrl { get; set; }
        public DateTime? AccreditedDate { get; set; }
        public double SortOrder { get; set; }
    }

    public static class PartnersProvisioner
    {
        public const string PartnersListName = "CollegePartners";
        public const string AccreditationsListName = "CollegeAccreditations";
        private static readonly object _lock = new object();

        public static void EnsureLists()
        {
            try
            {
                lock (_lock)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList partners = EnsureList(web, PartnersListName, "College partners");
                            SPFactory.MapListFieldsFromClass<PartnerItem>(partners);
                            if (partners.ItemCount == 0) SeedPartners(partners);

                            SPList accred = EnsureList(web, AccreditationsListName, "College accreditations");
                            SPFactory.MapListFieldsFromClass<AccreditationItem>(accred);
                            if (accred.ItemCount == 0) SeedAccreditations(accred);

                            web.AllowUnsafeUpdates = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "PartnersProvisioner - EnsureLists", ex.Message);
            }
        }

        private static SPList EnsureList(SPWeb web, string name, string desc)
        {
            SPList list = web.Lists.TryGetList(name);
            if (list == null)
            {
                Guid id = web.Lists.Add(name, desc, SPListTemplateType.GenericList);
                list = web.Lists[id];
                list.OnQuickLaunch = false;
                list.Update();
            }
            return list;
        }

        private static void SeedPartners(SPList list)
        {
            AddPartner(list, "جامعة دبلن سيتي", "Dublin City University", "https://www.dcu.ie/", "partner-logo-19.png", 1);
            AddPartner(list, "جامعة إكستر", "University of Exeter", "https://www.exeter.ac.uk/", "partner-logo-20.png", 2);
            AddPartner(list, "جامعة إسكس", "University of Essex", "https://www.essex.ac.uk/", "partner-logo-21.jpeg", 3);
            AddPartner(list, "إنترناشونال هاوس", "International House", "https://ihworld.com/", "partner-logo-22.jpg", 4);
            AddPartner(list, "جامعة نيوكاسل", "Newcastle University", "https://www.ncl.ac.uk/", "partner-logo-23.png", 5);
            AddPartner(list, "جامعة غلاسكو", "University of Glasgow", "https://www.gla.ac.uk/", "partner-logo-24.png", 6);
            AddPartner(list, "جامعة دندي", "University of Dundee", "https://www.dundee.ac.uk/", "partner-logo-25.jpg", 7);
            AddPartner(list, "جامعة ستراثكلايد", "University of Strathclyde", "https://www.strath.ac.uk/", "partner-logo-26.jpg", 8);
            AddPartner(list, "جامعة ساوثهامبتون", "University of Southampton", "https://www.southampton.ac.uk/", "partner-logo-27.png", 9);
            AddPartner(list, "شريك دولي", "International Partner", "https://pnu.edu.sa/ar/AboutUniversity/Pages/allPartners.aspx", "partner-logo-28.jpg", 10);
            AddPartner(list, "GRC Seoul", "GRC Seoul", "https://pnu.edu.sa/ar/AboutUniversity/Pages/allPartners.aspx", "partner-logo-29.png", 11);
            AddPartner(list, "إنسياد", "INSEAD", "https://www.insead.edu/", "partner-logo-30.jpg", 12);
            AddPartner(list, "جامعة زايد", "Zayed University", "https://www.zu.ac.ae/", "partner-logo-31.png", 13);
            AddPartner(list, "First Info Tech", "First Info Tech", "https://pnu.edu.sa/ar/AboutUniversity/Pages/allPartners.aspx", "partner-logo-32.jpg", 14);
            AddPartner(list, "أكاديمية باريس سان جيرمان", "PSG Academy", "https://www.psgacademy.com/", "partner-logo-33.png", 15);
            AddPartner(list, "جامعة بكين للغة والثقافة", "Beijing Language and Culture University", "https://www.blcu.edu.cn/", "partner-logo-34.png", 16);
            AddPartner(list, "SKEMA Business School", "SKEMA Business School", "https://www.skema.edu/", "partner-logo-35.png", 17);
            AddPartner(list, "جامعة أبردين", "University of Aberdeen", "https://www.abdn.ac.uk/", "partner-logo-36.jpg", 18);
            AddPartner(list, "جامعة دورهام", "Durham University", "https://www.durham.ac.uk/", "partner-logo-37.png", 19);
        }

        private static void AddPartner(SPList list, string ar, string en, string url, string logo, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["LinkUrl"] = url;
            item["LogoUrl"] = "/Style Library/DGA/public/images/partners/partners-logos/" + logo;
            item["SortOrder"] = sort;
            item.Update();
        }

        private static void SeedAccreditations(SPList list)
        {
            AddAccreditation(list,
                "الاعتماد المؤسسي الكامل حتى 2032",
                "Full institutional accreditation until 2032",
                "من هيئة تقويم التعليم والتدريب، ممثلةً في المركز الوطني للتقويم والاعتماد الأكاديمي.",
                "From the Education and Training Evaluation Commission, represented by the National Center for Academic Accreditation and Evaluation.",
                "/Style Library/DGA/public/images/news/achievement-accreditation-1x.webp",
                new DateTime(2025, 3, 23), 1);
        }

        private static void AddAccreditation(SPList list, string ar, string en, string descAr, string descEn,
            string img, DateTime date, double sort)
        {
            SPListItem item = list.AddItem();
            item["Title"] = ar;
            item["TitleEn"] = en;
            item["Description"] = descAr;
            item["DescriptionEn"] = descEn;
            item["ImageUrl"] = img;
            item["AccreditedDate"] = date;
            item["SortOrder"] = sort;
            item.Update();
        }
    }
}
