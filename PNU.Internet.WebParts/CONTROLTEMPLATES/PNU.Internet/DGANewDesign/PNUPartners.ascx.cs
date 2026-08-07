using Microsoft.SharePoint;
using PNU.Internet.WebParts.Helpers;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class PNUPartners : UserControl
    {
        // ------------------------------------------------------------------
        //  State
        // ------------------------------------------------------------------

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private List<PartnerCard> _localPartners = new List<PartnerCard>();
        private List<PartnerCard> _internationalPartners = new List<PartnerCard>();

        // ------------------------------------------------------------------
        //  Page lifecycle
        // ------------------------------------------------------------------

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    PartnersListProvisioner.EnsurePartnersLists();

                    LoadPartnersData();

                    rptLocalPartners.DataSource = _localPartners;
                    rptLocalPartners.DataBind();

                    rptInternationalPartners.DataSource = _internationalPartners;
                    rptInternationalPartners.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Repeater ItemDataBound - wires up each partner card
        //  (shared handler for both Local and International repeaters)
        // ------------------------------------------------------------------

        protected void rptPartners_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            PartnerCard partner = e.Item.DataItem as PartnerCard;
            if (partner == null) return;

            HtmlAnchor link = e.Item.FindControl("lnkPartner") as HtmlAnchor;
            HtmlImage img = e.Item.FindControl("imgLogo") as HtmlImage;

            string displayName = IsArabic ? partner.TitleAr : partner.TitleEn;
            string ariaLabel = IsArabic ? partner.AriaLabelAr : partner.AriaLabelEn;

            if (link != null)
            {
                link.HRef = string.IsNullOrEmpty(partner.Url) ? "#" : partner.Url;
                link.Attributes["aria-label"] = ariaLabel;
            }
            if (img != null)
            {
                img.Src = partner.LogoUrl;
                img.Alt = displayName;
            }
        }

        // ------------------------------------------------------------------
        //  Data loading
        // ------------------------------------------------------------------

        private void LoadPartnersData()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb rootWeb = site.RootWeb)
                    {
                        LoadSections(rootWeb);
                        _localPartners = LoadPartnerList(rootWeb, PartnersListProvisioner.LIST_LOCAL);
                        _internationalPartners = LoadPartnerList(rootWeb, PartnersListProvisioner.LIST_INTERNATIONAL);
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void LoadSections(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(PartnersListProvisioner.LIST_SECTIONS);
            if (list == null) return;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                string key = SafeString(item["Title"]);
                string heading = IsArabic ? SafeString(item["HeadingAr"]) : SafeString(item["HeadingEn"]);
                string desc = IsArabic ? SafeString(item["DescriptionAr"]) : SafeString(item["DescriptionEn"]);

                if (key == "Local")
                {
                    litLocalHeading.Text = Server.HtmlEncode(heading);
                    litLocalDescription.Text = Server.HtmlEncode(desc);
                }
                else if (key == "International")
                {
                    litInternationalHeading.Text = Server.HtmlEncode(heading);
                    litInternationalDescription.Text = Server.HtmlEncode(desc);
                }
            }
        }

        private List<PartnerCard> LoadPartnerList(SPWeb web, string listName)
        {
            List<PartnerCard> result = new List<PartnerCard>();

            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return result;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                SPFieldUrlValue url = new SPFieldUrlValue(SafeString(item["URL"]));
                string titleAr = SafeString(item["Title"]);
                string titleEn = SafeString(item["TitleEn"]);
                string ariaAr = SafeString(item["AriaLabelAr"]);
                string ariaEn = SafeString(item["AriaLabelEn"]);

                // Fall back to title when aria-label is empty
                if (string.IsNullOrEmpty(ariaAr)) ariaAr = titleAr;
                if (string.IsNullOrEmpty(ariaEn)) ariaEn = titleEn;

                result.Add(new PartnerCard
                {
                    TitleAr = titleAr,
                    TitleEn = titleEn,
                    Url = string.IsNullOrEmpty(url.Url) ? "#" : url.Url,
                    LogoUrl = BuildLogoUrl(SafeString(item["LogoFileName"])),
                    AriaLabelAr = ariaAr,
                    AriaLabelEn = ariaEn
                });
            }
            return result;
        }

        private string BuildLogoUrl(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            return PartnersListProvisioner.LOGO_PATH_PREFIX + fileName;
        }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        private static string SafeString(object v)
        {
            return v == null ? "" : v.ToString();
        }

        // ------------------------------------------------------------------
        //  DTO (internal - no Eval needed in markup anymore)
        // ------------------------------------------------------------------

        private class PartnerCard
        {
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string Url { get; set; }
            public string LogoUrl { get; set; }
            public string AriaLabelAr { get; set; }
            public string AriaLabelEn { get; set; }
        }
    }



}
