using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD
{
    public partial class ucPostgraduatePrograms : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class LinkCard
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string IconClass { get; set; }
            public string LinkUrl { get; set; }
            public string ArrowClass { get; set; }
            public int DisplayOrder { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    PostgraduateProgramsProvisioner.EnsureLists();
                    BindContent();
                    BindLinks();
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucPostgraduatePrograms.Page_Load", ex.Message);
                }
            }
        }

        private string GetVal(SPListItem item, string field)
        {
            return (item.Fields.ContainsField(field) && item[field] != null) ? item[field].ToString() : string.Empty;
        }

        private void BindContent()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(PostgraduateProgramsProvisioner.ContentListName);
            if (list == null || list.ItemCount == 0) return;

            SPListItem item = list.Items[0];

            litOverviewTitle.Text = IsArabic ? GetVal(item, "TitleAr") : GetVal(item, "TitleEn");
            litOverviewDesc.Text = IsArabic ? GetVal(item, "DescriptionAr") : GetVal(item, "DescriptionEn");
            litLinksTitle.Text = IsArabic ? GetVal(item, "SectionTitleAr") : GetVal(item, "SectionTitleEn");

            string bullets = IsArabic ? GetVal(item, "BulletsAr") : GetVal(item, "BulletsEn");
            rptBullets.DataSource = bullets.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(b => b.Trim()).ToList();
            rptBullets.DataBind();

            string imgUrl = GetVal(item, "ImageUrl");
            if (!string.IsNullOrEmpty(imgUrl))
            {
                imgOverview.Src = imgUrl;
                imgOverview.Alt = litOverviewTitle.Text;
                litImageCaption.Text = IsArabic ? GetVal(item, "ImageCaptionAr") : GetVal(item, "ImageCaptionEn");
            }
            else
            {
                divImage.Visible = false;
            }
        }

        private void BindLinks()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(PostgraduateProgramsProvisioner.LinksListName);
            if (list == null) return;

            SPQuery query = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq></Where>
                          <OrderBy><FieldRef Name='DisplayOrder' Ascending='TRUE'/></OrderBy>"
            };

            string arrowClass = IsArabic ? "hgi-arrow-left-02" : "hgi-arrow-right-02";

            List<LinkCard> cards = list.GetItems(query).Cast<SPListItem>().Select(i => new LinkCard
            {
                Title = IsArabic ? GetVal(i, "TitleAr") : GetVal(i, "TitleEn"),
                Description = IsArabic ? GetVal(i, "DescriptionAr") : GetVal(i, "DescriptionEn"),
                IconClass = GetVal(i, "IconClass"),
                LinkUrl = IsArabic ? GetVal(i, "LinkUrlAr") : GetVal(i, "LinkUrlEn"),
                ArrowClass = arrowClass
            }).ToList();

            rptLinks.DataSource = cards;
            rptLinks.DataBind();
        }
    }
}
