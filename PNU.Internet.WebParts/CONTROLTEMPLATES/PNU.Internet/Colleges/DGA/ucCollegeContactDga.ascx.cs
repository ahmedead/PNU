using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Contact the college (تواصل مع الكلية) tab - DGA design.
    /// Markup lives in the .ascx; this only binds data.
    /// Lists: CollegeContactInfo / CollegeContactDirectory / CollegeContactHours,
    /// provisioned + seeded on the current web at page load.
    /// </summary>
    public partial class ucCollegeContactDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class InfoDto
        {
            public string Label { get; set; }
            public string Value { get; set; }
            public string IconClass { get; set; }
            public string Url { get; set; }
            public bool HasLink { get; set; }
        }

        public class DirectoryDto
        {
            public string Office { get; set; }
            public List<string> Extensions { get; set; }
            public string Room { get; set; }
            public string Email { get; set; }
            public bool HasEmail { get; set; }
        }

        public class HoursDto
        {
            public string Unit { get; set; }
            public string Day { get; set; }
            public string Time { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                SPContext.Current != null &&
                SPContext.Current.Web.CurrentUser != null)
            {
                CollegeContactProvisioner.EnsureLists();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //SetTitles();
                string url = HttpContext.Current.Request.Url.AbsolutePath; // e.g. /ar/Faculties/Pages/default.aspx

                if (url.IndexOf("Faculties", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SetTitles();
                }
                else if (url.IndexOf("Agencies", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SetTitlesAgency();
                }

                if (!IsPostBack)
                {
                    SPWeb web = SPContext.Current.Web;
                    BindInfo(web);
                    BindDirectory(web);
                    BindHours(web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            //ltrHeading.Text = IsArabic ? "تواصل مع الكلية" : "Contact the College";

            ltrInfoTitle.Text = IsArabic ? "بيانات الموقع والتواصل" : "Location and Contact Details";

            ltrDirectoryTitle.Text = IsArabic ? "دليل التواصل مع مكاتب الكلية" : "College Offices Directory";
            ltrThOffice.Text = IsArabic ? "المكتب" : "Office";
            ltrThExtension.Text = IsArabic ? "التحويلة" : "Extension";
            ltrThRoom.Text = IsArabic ? "رقم المكتب" : "Room";
            ltrThEmail.Text = IsArabic ? "البريد الرسمي" : "Email";

            ltrHoursTitle.Text = IsArabic ? "ساعات المستفيدين" : "Beneficiary Hours";
            ltrThUnit.Text = IsArabic ? "الوكالة / القسم" : "Unit / Department";
            ltrThDay.Text = IsArabic ? "اليوم" : "Day";
            ltrThTime.Text = IsArabic ? "الساعة" : "Time";

            ltrUnifiedTitle.Text = IsArabic ? "أو عن طريق نموذج التواصل الموحد" : "Or via the unified contact form";
            ltrFormTitle.Text = IsArabic ? "نموذج التواصل الموحد" : "Unified Contact Form";
            ltrFormText.Text = IsArabic
                ? "أرسل استفسارك أو مقترحك إلى الكلية من خلال نموذج التواصل."
                : "Send your inquiry or suggestion to the college through the contact form.";
            lnkForm.Attributes["aria-label"] = IsArabic
                ? "فتح نموذج التواصل الموحد"
                : "Open the unified contact form";
        }

        private void SetTitlesAgency()
        {
            //ltrHeading.Text = IsArabic ? "تواصل مع الوكالة" : "Contact the Vice Rectorate";

            ltrInfoTitle.Text = IsArabic ? "بيانات الموقع والتواصل" : "Location and Contact Details";

            ltrDirectoryTitle.Text = IsArabic ? "دليل التواصل مع مكاتب الوكالة" : "Vice Rectorate Offices Directory";
            ltrThOffice.Text = IsArabic ? "المكتب" : "Office";
            ltrThExtension.Text = IsArabic ? "التحويلة" : "Extension";
            ltrThRoom.Text = IsArabic ? "رقم المكتب" : "Room";
            ltrThEmail.Text = IsArabic ? "البريد الرسمي" : "Email";

            ltrHoursTitle.Text = IsArabic ? "ساعات المستفيدين" : "Beneficiary Hours";
            ltrThUnit.Text = IsArabic ? "الوكالة / القسم" : "Unit / Department";
            ltrThDay.Text = IsArabic ? "اليوم" : "Day";
            ltrThTime.Text = IsArabic ? "الساعة" : "Time";

            ltrUnifiedTitle.Text = IsArabic ? "أو عن طريق نموذج التواصل الموحد" : "Or via the unified contact form";
            ltrFormTitle.Text = IsArabic ? "نموذج التواصل الموحد" : "Unified Contact Form";
            ltrFormText.Text = IsArabic
                ? "أرسل استفسارك أو مقترحك إلى الكلية من خلال نموذج التواصل."
                : "Send your inquiry or suggestion to the college through the contact form.";
            lnkForm.Attributes["aria-label"] = IsArabic
                ? "فتح نموذج التواصل الموحد"
                : "Open the unified contact form";
        }

        private void BindInfo(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(CollegeContactProvisioner.InfoListName);
            if (list == null) { phInfo.Visible = false; return; }

            var dtos = new List<InfoDto>();
            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string icon = SafeString(it, "IconClass");
                string url = SafeUrl(it, "LinkUrl");

                dtos.Add(new InfoDto
                {
                    Label = DisplayName(it, "Title", "TitleEn"),
                    Value = SafeString(it, "Value"),
                    IconClass = string.IsNullOrEmpty(icon) ? "hgi-location-01" : icon,
                    Url = url,
                    HasLink = !string.IsNullOrEmpty(url)
                });
            }

            phInfo.Visible = dtos.Count > 0;
            rptInfo.DataSource = dtos;
            rptInfo.DataBind();
        }

        private void BindDirectory(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(CollegeContactProvisioner.DirectoryListName);
            if (list == null) { phDirectory.Visible = false; return; }

            var dtos = new List<DirectoryDto>();
            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string email = SafeString(it, "Email");
                var extensions = (SafeString(it, "Extension") ?? "")
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .Where(l => l.Length > 0)
                    .ToList();
                if (extensions.Count == 0) extensions.Add("-");

                dtos.Add(new DirectoryDto
                {
                    Office = DisplayName(it, "Title", "OfficeEn"),
                    Extensions = extensions,
                    Room = SafeString(it, "RoomNumber"),
                    Email = email,
                    HasEmail = !string.IsNullOrEmpty(email)
                });
            }

            phDirectory.Visible = dtos.Count > 0;
            rptDirectory.DataSource = dtos;
            rptDirectory.DataBind();
        }

        /// <summary>Binds the stacked extension numbers inside each directory row.</summary>
        protected void rptDirectory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DirectoryDto row = e.Item.DataItem as DirectoryDto;
            if (row == null) return;

            Repeater rptExtensions = e.Item.FindControl("rptExtensions") as Repeater;
            if (rptExtensions == null) return;

            rptExtensions.DataSource = row.Extensions;
            rptExtensions.DataBind();
        }

        private void BindHours(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(CollegeContactProvisioner.HoursListName);
            if (list == null) { phHours.Visible = false; return; }

            var dtos = new List<HoursDto>();
            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string dayAr = SafeString(it, "Day");
                string dayEn = SafeString(it, "DayEn");

                dtos.Add(new HoursDto
                {
                    Unit = DisplayName(it, "Title", "UnitEn"),
                    Day = IsArabic ? dayAr : (!string.IsNullOrEmpty(dayEn) ? dayEn : dayAr),
                    Time = SafeString(it, "Time")
                });
            }

            phHours.Visible = dtos.Count > 0;
            rptHours.DataSource = dtos;
            rptHours.DataBind();
        }

        // ---- helpers ----
        private string DisplayName(SPListItem item, string arField, string enField)
        {
            string ar = SafeString(item, arField);
            string en = SafeString(item, enField);
            return IsArabic
                ? (!string.IsNullOrEmpty(ar) ? ar : en)
                : (!string.IsNullOrEmpty(en) ? en : ar);
        }

        private static string SafeString(SPListItem item, string field)
        {
            try
            {
                return item.Fields.ContainsField(field) && item[field] != null
                    ? item[field].ToString() : string.Empty;
            }
            catch { return string.Empty; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null
                    && double.TryParse(item[field].ToString(), out v) ? v : 0;
            }
            catch { return 0; }
        }

        private static string SafeUrl(SPListItem item, string field)
        {
            try
            {
                if (item.Fields.ContainsField(field) && item[field] != null)
                {
                    var v = new SPFieldUrlValue(item[field].ToString());
                    return v.Url ?? string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }
    
    }
}
