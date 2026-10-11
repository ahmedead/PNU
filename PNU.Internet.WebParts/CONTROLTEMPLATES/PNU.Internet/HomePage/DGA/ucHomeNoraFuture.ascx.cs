using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeNoraFuture : UserControl
    {
        private const string DefaultTitleAr = "الدراسة في جامعة الأميرة نورة";
        private const string DefaultTitleEn = "Study at Princess Nourah University";

        private HomeNoraFutureTitleModel _titleData = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Lists are provisioned from the admin control only; the home page just reads.
            // Bind on every request (cached) so postbacks never depend on ViewState.
            try
            {
                LoadTitleAndCards();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFuture.Page_Load", ex.Message);
                LoadDefaultFallback();
            }
        }

        private void LoadTitleAndCards()
        {
            HomeNoraFutureSectionData data = HomeNoraFutureRepository.GetSectionData();

            // Lists unreadable (missing web/list, error) -> built-in fallback so the home page still looks right
            if (data == null || !data.Loaded)
            {
                LoadDefaultFallback();
                return;
            }

            _titleData = data.Title;

            // Hidden by the admin, or every card deactivated/deleted -> hide the whole section
            if ((_titleData != null && !_titleData.ShowSection) || data.ActiveItems == null || data.ActiveItems.Count == 0)
            {
                phSection.Visible = false;
                return;
            }

            phSection.Visible = true;
            string sectionTitle = _titleData != null ? SPFactory.GetLocalizedTitle(_titleData.Title, _titleData.Title_EN) : "";
            litSectionTitle.Text = HttpUtility.HtmlEncode(!string.IsNullOrEmpty(sectionTitle) ? sectionTitle : GetDefaultTitle());

            rptNoraFuture.DataSource = data.ActiveItems;
            rptNoraFuture.DataBind();
        }

        private void LoadDefaultFallback()
        {
            phSection.Visible = true;
            litSectionTitle.Text = HttpUtility.HtmlEncode(GetDefaultTitle());

            List<HomeNoraFutureItem> fallbackItems = HomeNoraFutureRepository.GetDefaultItems();
            rptNoraFuture.DataSource = fallbackItems;
            rptNoraFuture.DataBind();
        }

        private static string GetDefaultTitle()
        {
            string res = SPFactory.GetPNUresResource("StudyAtPNU");
            return !string.IsNullOrEmpty(res) ? res : (SPFactory.IsArabic ? DefaultTitleAr : DefaultTitleEn);
        }

        /// <summary>Aria label, already attribute-encoded for direct output.</summary>
        public string GetAriaLabel()
        {
            string label = null;
            if (_titleData != null)
                label = SPFactory.GetLocalizedTitle(_titleData.AriaLabel, _titleData.AriaLabel_EN);
            if (string.IsNullOrEmpty(label))
                label = SPFactory.IsArabic ? "برامج ومستقبل نورة" : "PNU Programs & Future";
            return HttpUtility.HtmlAttributeEncode(label);
        }

        /// <summary>Localized text, HTML-encoded.</summary>
        public static string GetLocalizedText(object ar, object en)
        {
            return HttpUtility.HtmlEncode(SPFactory.GetLocalizedTitle(ar, en));
        }

        /// <summary>Icon CSS classes, attribute-encoded.</summary>
        public static string FormatIconClass(object iconObj)
        {
            string icon = iconObj != null ? iconObj.ToString().Trim() : "";
            string css;
            if (string.IsNullOrEmpty(icon)) css = "hgi hgi-stroke fs-3 hgi-book-01";
            else if (icon.Contains("hgi-") && !icon.Contains("hgi ")) css = "hgi hgi-stroke fs-3 " + icon;
            else css = icon;
            return HttpUtility.HtmlAttributeEncode(css);
        }

        /// <summary>Localized link, restricted to safe schemes and attribute-encoded.</summary>
        public static string GetItemUrl(object linkArObj, object linkEnObj)
        {
            string ar = linkArObj != null ? linkArObj.ToString().Trim() : "";
            string en = linkEnObj != null ? linkEnObj.ToString().Trim() : "";

            string url = SPFactory.IsArabic
                ? (!string.IsNullOrEmpty(ar) ? ar : en)
                : (!string.IsNullOrEmpty(en) ? en : ar);

            return HttpUtility.HtmlAttributeEncode(HomeNoraFutureRepository.IsSafeUrl(url) ? url : "#");
        }
    }
}
