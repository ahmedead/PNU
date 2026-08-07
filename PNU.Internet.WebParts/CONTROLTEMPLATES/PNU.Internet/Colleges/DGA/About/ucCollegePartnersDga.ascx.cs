using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.About
{
    public partial class ucCollegePartnersDga : UserControl
    {
        protected bool IsArabic;

        private class PartnerDto
        {
            public string DisplayTitle { get; set; }
            public string LinkUrl { get; set; }
            public string LogoUrl { get; set; }
        }

        private class AccredDto
        {
            public string DisplayTitle { get; set; }
            public string DisplayText { get; set; }
            public string ImageUrl { get; set; }
            public string DateIso { get; set; }
            public string DateDisplay { get; set; }
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
                PartnersProvisioner.EnsureLists();
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = SPContext.Current.Web.Language == 1025;

            ltrPartnersTitle.Text = IsArabic ? "شركاء دوليون" : "International Partners";
            ltrPartnersIntro.Text = IsArabic
                ? "جامعات ومؤسسات دولية تعزز تبادل الخبرات والتعاون الأكاديمي والبحثي."
                : "International universities and institutions that enhance the exchange of expertise and academic and research cooperation.";
            ltrAccredTitle.Text = IsArabic ? "الإعتمادات" : "Accreditations";
            ltrAccredIntro.Text = IsArabic
                ? "الاعتمادات الأكاديمية والمؤسسية التي حصلت عليها الكلية."
                : "The academic and institutional accreditations obtained by the college.";

            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite eSite = new SPSite(siteId))
                    using (SPWeb web = eSite.OpenWeb(webId))
                    {
                        BindPartners(web);
                        BindAccreditations(web);
                    }
                });

                
                
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucCollegePartnersDga - BindData", ex.Message);
            }
        }

        private void BindPartners(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PartnersProvisioner.PartnersListName);
            phPartners.Visible = list != null && list.ItemCount > 0;
            if (list == null) return;

            var items = new List<PartnerItem>();
            foreach (SPListItem it in list.Items)
            {
                items.Add(new PartnerItem
                {
                    ID = it.ID,
                    Title = SafeString(it, "Title"),
                    TitleEn = SafeString(it, "TitleEn"),
                    LinkUrl = SafeString(it, "LinkUrl"),
                    LogoUrl = SafeString(it, "LogoUrl"),
                    SortOrder = SafeDouble(it, "SortOrder")
                });
            }

            var dtos = items.OrderBy(x => x.SortOrder).Select(x => new PartnerDto
            {
                DisplayTitle = IsArabic ? x.Title : Fallback(x.TitleEn, x.Title),
                LinkUrl = x.LinkUrl,
                LogoUrl = x.LogoUrl
            }).ToList();

            rptPartners.DataSource = dtos;
            rptPartners.DataBind();
        }

        private void BindAccreditations(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PartnersProvisioner.AccreditationsListName);
            phAccreditations.Visible = list != null && list.ItemCount > 0;
            if (list == null) return;

            var items = new List<AccreditationItem>();
            foreach (SPListItem it in list.Items)
            {
                items.Add(new AccreditationItem
                {
                    ID = it.ID,
                    Title = SafeString(it, "Title"),
                    TitleEn = SafeString(it, "TitleEn"),
                    Description = SafeString(it, "Description"),
                    DescriptionEn = SafeString(it, "DescriptionEn"),
                    ImageUrl = SafeString(it, "ImageUrl"),
                    AccreditedDate = SafeDate(it, "AccreditedDate"),
                    SortOrder = SafeDouble(it, "SortOrder")
                });
            }

            var arCulture = new CultureInfo("ar-SA");
            var dtos = items.OrderBy(x => x.SortOrder).Select(x => new AccredDto
            {
                DisplayTitle = IsArabic ? x.Title : Fallback(x.TitleEn, x.Title),
                DisplayText = IsArabic ? x.Description : Fallback(x.DescriptionEn, x.Description),
                ImageUrl = x.ImageUrl,
                DateIso = x.AccreditedDate.HasValue
                    ? x.AccreditedDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : "",
                DateDisplay = x.AccreditedDate.HasValue
                    ? (IsArabic
                        ? x.AccreditedDate.Value.ToString("dd MMMM yyyy", arCulture)
                        : x.AccreditedDate.Value.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture))
                    : ""
            }).ToList();

            rptAccreditations.DataSource = dtos;
            rptAccreditations.DataBind();
        }

        private static string Fallback(string preferred, string fallback)
        {
            return string.IsNullOrWhiteSpace(preferred) ? fallback : preferred;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try
            {
                return item.Fields.ContainsField(field) && item[field] != null
                    ? item[field].ToString()
                    : string.Empty;
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

        private static DateTime? SafeDate(SPListItem item, string field)
        {
            try
            {
                if (item.Fields.ContainsField(field) && item[field] != null)
                {
                    DateTime v;
                    if (DateTime.TryParse(item[field].ToString(), out v))
                        return v;
                }
            }
            catch { }
            return null;
        }
    }

}
