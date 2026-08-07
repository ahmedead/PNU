using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// "تواصل مع الكلية" tab - DGA design.
    /// New lists (provisioned from the page, idempotent):
    ///   CollegeContactInfo      : ContentKey (Building | Station | Phone | MapUrl | MapTitle),
    ///                             ItemValue / ItemValue_EN
    ///   CollegeContactDirectory : Title(Office)/Title_EN, Extension, RoomNo, Email, ItemOrder
    ///   CollegeVisitHours       : Title(Dept)/Title_EN, DayName/DayName_EN, HourText/HourText_EN, ItemOrder
    /// Renders: location cards row + directory table + visit-hours table.
    /// </summary>
    public partial class ucCollegeContact : UserControl
    {
        public const string LIST_INFO = "CollegeContactInfo";
        public const string LIST_DIRECTORY = "CollegeContactDirectory";
        public const string LIST_HOURS = "CollegeVisitHours";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private class DirectoryRow { public string Office, Extension, Room, Email; }
        private class HoursRow { public string Dept, Day, Hour; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureLists();
                if (!IsPostBack || ltrContact.Text.Length == 0)
                    RenderContact();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Render
        // ------------------------------------------------------------------
        private static string Get(Dictionary<string, string> dict, string key)
        {
            return dict.ContainsKey(key) ? dict[key] : "";
        }
        private void RenderContact()
        {
            Dictionary<string, string> info = LoadInfo();
            List<DirectoryRow> directory = LoadDirectory();
            List<HoursRow> hours = LoadHours();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<h2 class=\"mb-4\">{0}</h2>",
                IsArabic ? "تواصل مع الكلية" : "Contact the College");

            // ---- location cards ----
            sb.Append("<section aria-labelledby=\"faculty-contact-location-title\">");
            sb.AppendFormat("<h3 class=\"h5 mb-3\" id=\"faculty-contact-location-title\">{0}</h3>",
                IsArabic ? "بيانات الموقع والتواصل" : "Location and Contact");
            sb.Append("<div class=\"row g-4\">");

            AppendInfoCard(sb, "hgi-location-01", IsArabic ? "رقم المبنى" : "Building No.", Get(info, "Building"));
            AppendInfoCard(sb, "hgi-location-01", IsArabic ? "محطة القطار" : "Train Station", Get(info, "Station"));
            AppendInfoCard(sb, "hgi-call", IsArabic ? "رقم الهاتف" : "Phone", Get(info, "Phone"));

            string mapUrl = Get(info, "MapUrl");
            string mapTitle = Get(info, "MapTitle");
            if (!string.IsNullOrEmpty(mapUrl))
            {
                sb.Append("<div class=\"col-12 col-md-6 col-xl-3\"><div class=\"card h-100\">");
                sb.Append("<div class=\"card-body d-flex flex-column gap-3\">");
                sb.Append("<div class=\"icon-container\"><i class=\"hgi hgi-stroke hgi-location-01 fs-3\" aria-hidden=\"true\"></i></div>");
                sb.AppendFormat("<div><h4 class=\"h6\">{0}</h4>" +
                    "<a href=\"{1}\" target=\"_blank\" rel=\"noopener noreferrer\">{2}</a></div>",
                    IsArabic ? "موقع الكلية" : "College Location",
                    mapUrl, HttpUtility.HtmlEncode(mapTitle));
                sb.Append("</div></div></div>");
            }

            sb.Append("</div></section>");

            // ---- directory table ----
            if (directory.Count > 0)
            {
                sb.Append("<section class=\"mt-5\" aria-labelledby=\"faculty-contact-directory-title\">");
                sb.AppendFormat("<h3 class=\"h5 mb-3\" id=\"faculty-contact-directory-title\">{0}</h3>",
                    IsArabic ? "دليل التواصل مع مكاتب الكلية" : "Office Contact Directory");
                sb.Append("<div class=\"table-responsive border border-top-0 border-bottom-0 rounded-2\">");
                sb.Append("<table class=\"table table-striped mb-0\"><thead><tr>");
                sb.AppendFormat("<th scope=\"col\">{0}</th><th scope=\"col\">{1}</th><th scope=\"col\">{2}</th><th scope=\"col\">{3}</th>",
                    IsArabic ? "المكتب" : "Office",
                    IsArabic ? "التحويلة" : "Extension",
                    IsArabic ? "رقم المكتب" : "Room No.",
                    IsArabic ? "البريد الرسمي" : "Official Email");
                sb.Append("</tr></thead><tbody>");

                foreach (DirectoryRow row in directory)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.Office));
                    sb.AppendFormat("<td dir=\"ltr\">{0}</td>", HttpUtility.HtmlEncode(row.Extension));
                    sb.AppendFormat("<td dir=\"ltr\">{0}</td>", HttpUtility.HtmlEncode(row.Room));
                    sb.AppendFormat("<td><a href=\"mailto:{0}\" dir=\"ltr\">{0}</a></td>", HttpUtility.HtmlEncode(row.Email));
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table></div></section>");
            }

            // ---- visit hours ----
            if (hours.Count > 0)
            {
                sb.Append("<section class=\"mt-5\" aria-labelledby=\"faculty-contact-hours-title\">");
                sb.AppendFormat("<h3 class=\"h5 mb-3\" id=\"faculty-contact-hours-title\">{0}</h3>",
                    IsArabic ? "ساعات المستفيدين" : "Visiting Hours");
                sb.Append("<div class=\"table-responsive border border-top-0 border-bottom-0 rounded-2\">");
                sb.Append("<table class=\"table table-striped mb-0\"><thead><tr>");
                sb.AppendFormat("<th scope=\"col\">{0}</th><th scope=\"col\">{1}</th><th scope=\"col\">{2}</th>",
                    IsArabic ? "الوكالة / القسم" : "Deanship / Department",
                    IsArabic ? "اليوم" : "Day",
                    IsArabic ? "الساعة" : "Hour");
                sb.Append("</tr></thead><tbody>");

                foreach (HoursRow row in hours)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>",
                        HttpUtility.HtmlEncode(row.Dept),
                        HttpUtility.HtmlEncode(row.Day),
                        HttpUtility.HtmlEncode(row.Hour));
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table></div></section>");
            }

            ltrContact.Text = sb.ToString();
        }

        private void AppendInfoCard(StringBuilder sb, string icon, string label, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            sb.Append("<div class=\"col-12 col-md-6 col-xl-3\"><div class=\"card h-100\">");
            sb.Append("<div class=\"card-body d-flex flex-column gap-3\">");
            sb.AppendFormat("<div class=\"icon-container\"><i class=\"hgi hgi-stroke {0} fs-3\" aria-hidden=\"true\"></i></div>", icon);
            sb.AppendFormat("<div><h4 class=\"h6\">{0}</h4><p class=\"mb-0\" dir=\"ltr\">{1}</p></div>",
                HttpUtility.HtmlEncode(label), HttpUtility.HtmlEncode(value));
            sb.Append("</div></div></div>");
        }

        // ------------------------------------------------------------------
        //  Data
        // ------------------------------------------------------------------

        private Dictionary<string, string> LoadInfo()
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_INFO);
                if (list == null) return result;

                foreach (SPListItem item in list.GetItems())
                {
                    string key = ucSideMenu.GetFirstFieldValue(item, "ContentKey");
                    if (string.IsNullOrEmpty(key)) continue;
                    result[key] = IsArabic
                        ? ucSideMenu.GetFirstFieldValue(item, "ItemValue")
                        : ucSideMenu.GetFirstFieldValue(item, "ItemValue_EN", "ItemValue");
                }
            }
            return result;
        }

        private List<DirectoryRow> LoadDirectory()
        {
            var result = new List<DirectoryRow>();
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_DIRECTORY);
                if (list == null) return result;

                SPQuery q = new SPQuery
                {
                    Query = @"<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                };
                foreach (SPListItem item in list.GetItems(q))
                {
                    result.Add(new DirectoryRow
                    {
                        Office = IsArabic
                            ? ucSideMenu.GetFirstFieldValue(item, "Title")
                            : ucSideMenu.GetFirstFieldValue(item, "Title_EN", "Title"),
                        Extension = ucSideMenu.GetFirstFieldValue(item, "Extension"),
                        Room = ucSideMenu.GetFirstFieldValue(item, "RoomNo"),
                        Email = ucSideMenu.GetFirstFieldValue(item, "Email")
                    });
                }
            }
            return result;
        }

        private List<HoursRow> LoadHours()
        {
            var result = new List<HoursRow>();
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList(LIST_HOURS);
                if (list == null) return result;

                SPQuery q = new SPQuery
                {
                    Query = @"<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                };
                foreach (SPListItem item in list.GetItems(q))
                {
                    result.Add(new HoursRow
                    {
                        Dept = IsArabic
                            ? ucSideMenu.GetFirstFieldValue(item, "Title")
                            : ucSideMenu.GetFirstFieldValue(item, "Title_EN", "Title"),
                        Day = IsArabic
                            ? ucSideMenu.GetFirstFieldValue(item, "DayName")
                            : ucSideMenu.GetFirstFieldValue(item, "DayName_EN", "DayName"),
                        Hour = IsArabic
                            ? ucSideMenu.GetFirstFieldValue(item, "HourText")
                            : ucSideMenu.GetFirstFieldValue(item, "HourText_EN", "HourText")
                    });
                }
            }
            return result;
        }

        // ------------------------------------------------------------------
        //  Provisioning (from the page - idempotent)
        // ------------------------------------------------------------------

        private void EnsureLists()
        {
            bool needsWork;
            using (SPSite s = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb w = s.OpenWeb())
            {
                needsWork = (w.Lists.TryGetList(LIST_INFO) == null ||
                             w.Lists.TryGetList(LIST_DIRECTORY) == null ||
                             w.Lists.TryGetList(LIST_HOURS) == null);
            }
            if (!needsWork) return;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // CollegeContactInfo (key/value)
                    SPList info = web.Lists.TryGetList(LIST_INFO);
                    if (info == null)
                        info = web.Lists[web.Lists.Add(LIST_INFO,
                            "Contact info key/value (Building, Station, Phone, MapUrl, MapTitle)",
                            SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(info, "ContentKey");
                    ucCollegeContentSection.AddTextField(info, "ItemValue");
                    ucCollegeContentSection.AddTextField(info, "ItemValue_EN");
                    info.Update();

                    if (info.ItemCount == 0)
                    {
                        SeedInfo(info, "Building", "", "");
                        SeedInfo(info, "Station", "", "");
                        SeedInfo(info, "Phone", "", "");
                        SeedInfo(info, "MapUrl", "", "");
                        SeedInfo(info, "MapTitle", "موقع الكلية", "College Location");
                    }

                    // CollegeContactDirectory
                    SPList dir = web.Lists.TryGetList(LIST_DIRECTORY);
                    if (dir == null)
                        dir = web.Lists[web.Lists.Add(LIST_DIRECTORY,
                            "College office contact directory", SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(dir, "Title_EN");
                    ucCollegeContentSection.AddTextField(dir, "Extension");
                    ucCollegeContentSection.AddTextField(dir, "RoomNo");
                    ucCollegeContentSection.AddTextField(dir, "Email");
                    ucCollegeContentSection.AddNumberField(dir, "ItemOrder");
                    dir.Update();

                    // CollegeVisitHours
                    SPList hrs = web.Lists.TryGetList(LIST_HOURS);
                    if (hrs == null)
                        hrs = web.Lists[web.Lists.Add(LIST_HOURS,
                            "College visiting hours", SPListTemplateType.GenericList)];
                    ucCollegeContentSection.AddTextField(hrs, "Title_EN");
                    ucCollegeContentSection.AddTextField(hrs, "DayName");
                    ucCollegeContentSection.AddTextField(hrs, "DayName_EN");
                    ucCollegeContentSection.AddTextField(hrs, "HourText");
                    ucCollegeContentSection.AddTextField(hrs, "HourText_EN");
                    ucCollegeContentSection.AddNumberField(hrs, "ItemOrder");
                    hrs.Update();

                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        private static void SeedInfo(SPList list, string key, string valAr, string valEn)
        {
            SPListItem item = list.AddItem();
            item["Title"] = key;
            item["ContentKey"] = key;
            item["ItemValue"] = valAr;
            item["ItemValue_EN"] = valEn;
            item.Update();
        }
    }

}
