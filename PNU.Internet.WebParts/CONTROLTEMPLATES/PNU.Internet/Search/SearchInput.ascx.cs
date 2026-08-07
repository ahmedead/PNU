using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.DAL;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search
{
    public partial class SearchInput : UserControl
    {
        /// <summary>
        /// Where to redirect when the user submits the search.
        /// e.g. "/ar/Pages/SearchResults.aspx" - set in the web part properties
        /// or hard-code below.
        /// </summary>
        public string ResultsPageUrl { get; set; } = "/ar/Pages/SearchResults.aspx";

        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindSuggestions();
                    DataBind();             // resolve <%# %> in the .ascx
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    this.Page.Title, ex.Message);
            }
        }

        private void BindSuggestions()
        {
            try
            {
                List<KeyValuePair<string, int>> cats =
                    SearchIndexDal.GetCategories();

                // top 5 most-frequent categories => quick suggestion chips
                var top = new List<object>();
                int n = 0;
                foreach (var kv in cats)
                {
                    if (n++ >= 5) break;
                    top.Add(new { Category = kv.Key, Count = kv.Value });
                }

                rptSuggestions.DataSource = top;
                rptSuggestions.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchInput.BindSuggestions", "", ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = (txtSearch.Text ?? "").Trim();
            string url = ResultsPageUrl + "?q=" + HttpUtility.UrlEncode(q);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    
    }
}
