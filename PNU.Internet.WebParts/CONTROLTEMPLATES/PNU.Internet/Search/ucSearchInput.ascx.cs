using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search
{
    public partial class ucSearchInput : System.Web.UI.UserControl
    {
        /// <summary>
        /// Server-relative URL of the search results page. Set in the
        /// page that hosts the control:
        ///   &lt;pnu:SearchInput runat="server"
        ///        ResultsPageUrl="/ar/Pages/SearchResults.aspx" /&gt;
        /// </summary>
        public string ResultsPageUrl { get; set; }

        /// <summary>
        /// Optional comma-separated suggestion chips, e.g.
        ///   "التدريب,الخدمات,المقالات,الأنظمة,الأخبار"
        /// Leave empty to hide the suggestions block.
        /// </summary>
        public string SuggestionsCsv { get; set; }

        protected bool IsArabic { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = DetectArabic();

            txtKeyword.Attributes["type"] = "search";
            txtKeyword.Attributes["placeholder"] =
                IsArabic ? "ابحث في موقع الجامعة"
                         : "Search the university website";
            btnSearch.Text = IsArabic ? "بحث" : "Search";
            btnSearch.Attributes["aria-label"] =
                IsArabic ? "بحث" : "Search";

            if (!IsPostBack)
            {
                string q = Request.QueryString["q"];
                if (!string.IsNullOrEmpty(q)) txtKeyword.Text = q;

                BindSuggestions();
                DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = txtKeyword.Text == null ? "" : txtKeyword.Text.Trim();
            string target = !string.IsNullOrEmpty(ResultsPageUrl)
                ? ResultsPageUrl
                : (IsArabic
                    ? "/ar/Search/Pages/SearchResults.aspx"
                    : "/en/Search/Pages/SearchResults.aspx");

            string url = target + "?q=" + HttpUtility.UrlEncode(q);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void BindSuggestions()
        {
            litSuggestionsLabel.Text =
                IsArabic ? "اقتراحات" : "Suggestions";

            string csv = SuggestionsCsv;
            if (string.IsNullOrWhiteSpace(csv))
            {
                csv = IsArabic
                    ? "التدريب,الخدمات,المقالات,الأنظمة,الأخبار"
                    : "Training,Services,Articles,Regulations,News";
            }

            string target = !string.IsNullOrEmpty(ResultsPageUrl)
                ? ResultsPageUrl
                : (IsArabic
                    ? "/ar/Search/Pages/SearchResults.aspx"
                    : "/en/Search/Pages/SearchResults.aspx");

            var data = new List<object>();
            foreach (string raw in csv.Split(','))
            {
                string s = raw == null ? "" : raw.Trim();
                if (s.Length == 0) continue;
                data.Add(new
                {
                    Text = s,
                    Url = target + "?q=" + HttpUtility.UrlEncode(s)
                });
            }

            if (data.Count > 0)
            {
                pnlSuggestions.Visible = true;
                rptSuggestions.DataSource = data;
                rptSuggestions.DataBind();
            }
        }

        private static bool DetectArabic()
        {
            try
            {
                int lcid = Thread.CurrentThread.CurrentUICulture.LCID;
                if (lcid == 1025) return true;
                if (lcid == 1033) return false;
            }
            catch { }
            return true;
        }
    }
}