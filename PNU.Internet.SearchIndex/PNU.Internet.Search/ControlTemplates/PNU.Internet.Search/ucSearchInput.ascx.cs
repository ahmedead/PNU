using System;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.Search.ControlTemplates.PNU.Internet.Search
{
    /// <summary>
    /// Renders the search input. On submit it redirects to the search
    /// results page carrying the keyword on the query string. Place
    /// this user control in the home page header / banner area.
    ///
    /// You can override the target results URL in either:
    ///   - the ResultsPageUrl property (web part / control properties)
    ///   - or set RootWeb property bag "PNU_SearchResultsUrl"
    /// </summary>
    public partial class ucSearchInput : UserControl
    {
        public string ResultsPageUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string q = Request.QueryString["q"];
                    if (!string.IsNullOrEmpty(q))
                        txtKeyword.Text = q;
                }
            }
            catch { /* never break the page */ }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string q = (txtKeyword.Text ?? "").Trim();
                string target = ResolveResultsUrl();
                if (string.IsNullOrEmpty(target)) return;

                string sep = target.Contains("?") ? "&" : "?";
                Response.Redirect(target + sep + "q=" + HttpUtility.UrlEncode(q),
                    false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch { }
        }

        // ----------------------------------------------------------------
        private string ResolveResultsUrl()
        {
            if (!string.IsNullOrEmpty(ResultsPageUrl))
                return ResultsPageUrl;

            try
            {
                if (SPContext.Current != null
                    && SPContext.Current.Site != null
                    && SPContext.Current.Site.RootWeb != null)
                {
                    var pb = SPContext.Current.Site.RootWeb.AllProperties;
                    if (pb != null && pb.ContainsKey("PNU_SearchResultsUrl"))
                    {
                        string v = Convert.ToString(pb["PNU_SearchResultsUrl"]);
                        if (!string.IsNullOrEmpty(v)) return v;
                    }
                }
            }
            catch { }

            // Fallback: bilingual default
            bool isArabic = SPContext.Current != null
                         && SPContext.Current.Web != null
                         && SPContext.Current.Web.Language == 1025;
            return isArabic ? "/ar/Pages/search.aspx"
                            : "/en/Pages/search.aspx";
        }
    }
}
