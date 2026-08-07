using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.DAL;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search
{
    public partial class SearchResults : UserControl
    {
        // ----- public properties used by the .ascx ----------------------
        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public string Keyword     { get; set; }
        public int    CurrentPage { get; set; } = 1;

        protected const int PageSize = 10;

        // ----- ViewState wrappers ---------------------------------------
        protected string SortBy
        {
            get { return (ViewState["SortBy"] as string) ?? "rel"; }
            set { ViewState["SortBy"] = value; }
        }
        protected string CategoriesCsv
        {
            get { return (ViewState["Cats"] as string) ?? ""; }
            set { ViewState["Cats"] = value; }
        }

        // ============ Page lifecycle =====================================
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Keyword = (Request.QueryString["q"] ?? "").Trim();
                    txtSearch.Text = Keyword;
                    litKeyword.Text = HttpUtility.HtmlEncode(Keyword);

                    int p; int.TryParse(Request.QueryString["p"], out p);
                    CurrentPage = p < 1 ? 1 : p;

                    BindCategoryFilter();
                    RunSearch();
                    DataBind();           // resolves <%# %> in markup
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    this.Page.Title, ex.Message);
            }
        }

        // ============ Filter dropdown ====================================
        private void BindCategoryFilter()
        {
            try
            {
                var cats = SearchIndexDal.GetCategories();
                chkCategories.Items.Clear();
                foreach (var kv in cats)
                {
                    chkCategories.Items.Add(new ListItem(
                        string.Format("{0} ({1})", kv.Key, kv.Value),
                        kv.Key));
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("SearchResults.BindCategoryFilter",
                    "", ex.Message);
            }
        }

        // ============ Core: run query and bind ===========================
        private void RunSearch()
        {
            int total;
            List<SearchIndexItem> items = SearchIndexDal.Search(
                Keyword, IsArabic, CategoriesCsv, SortBy,
                CurrentPage, PageSize, out total);

            if (total == 0)
            {
                pnlResults.Visible = false;
                pnlEmpty.Visible   = true;
                pnlCount.Visible   = false;
                return;
            }

            pnlResults.Visible = true;
            pnlEmpty.Visible   = false;
            pnlCount.Visible   = true;
            litTotal.Text      = total.ToString();

            rptResults.DataSource = items;
            rptResults.DataBind();

            BuildPager(total);
        }

        // ============ Pagination =========================================
        private void BuildPager(int total)
        {
            int totalPages = (int)Math.Ceiling((double)total / PageSize);
            if (totalPages <= 1)
            {
                rptPages.DataSource = new int[0];
                rptPages.DataBind();
                lnkPrev.Enabled = false;
                lnkNext.Enabled = false;
                return;
            }

            // Show a sliding window of 5 page numbers around current
            int start = Math.Max(1, CurrentPage - 2);
            int end   = Math.Min(totalPages, start + 4);
            start     = Math.Max(1, end - 4);

            var window = new List<object>();
            for (int i = start; i <= end; i++)
                window.Add(new { PageNumber = i });

            rptPages.DataSource = window;
            rptPages.DataBind();

            lnkPrev.Enabled = CurrentPage > 1;
            lnkNext.Enabled = CurrentPage < totalPages;
        }

        // ============ Event handlers =====================================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = (txtSearch.Text ?? "").Trim();
            string url = Request.Path + "?q=" + HttpUtility.UrlEncode(q);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void Sort_Click(object sender, EventArgs e)
        {
            SortBy = ((LinkButton)sender).CommandArgument;
            CurrentPage = 1;
            Keyword = (txtSearch.Text ?? "").Trim();
            litKeyword.Text = HttpUtility.HtmlEncode(Keyword);
            RunSearch();
            DataBind();
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            var picked = new List<string>();
            foreach (ListItem li in chkCategories.Items)
                if (li.Selected) picked.Add(li.Value);

            CategoriesCsv = string.Join(",", picked);
            CurrentPage   = 1;
            Keyword       = (txtSearch.Text ?? "").Trim();
            litKeyword.Text = HttpUtility.HtmlEncode(Keyword);
            RunSearch();
            DataBind();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in chkCategories.Items) li.Selected = false;
            CategoriesCsv = "";
            CurrentPage   = 1;
            Keyword       = (txtSearch.Text ?? "").Trim();
            litKeyword.Text = HttpUtility.HtmlEncode(Keyword);
            RunSearch();
            DataBind();
        }

        protected void Page_Click(object sender, EventArgs e)
        {
            string arg = ((LinkButton)sender).CommandArgument;
            int p;
            if (arg == "prev")      CurrentPage = Math.Max(1, CurrentPage - 1);
            else if (arg == "next") CurrentPage = CurrentPage + 1;
            else if (int.TryParse(arg, out p)) CurrentPage = p;

            Keyword = (txtSearch.Text ?? "").Trim();
            litKeyword.Text = HttpUtility.HtmlEncode(Keyword);
            RunSearch();
            DataBind();
        }

        // ============ Helpers used in markup =============================
        /// <summary>
        /// Returns an HTML-safe snippet around the keyword with &lt;mark&gt;
        /// tags around the matching text - matches the search-result.html
        /// design.
        /// </summary>
        public string HighlightSnippet(string content, string keyword)
        {
            if (string.IsNullOrEmpty(content)) return "";
            string clipped = content.Length > 220
                ? content.Substring(0, 220) + "…"
                : content;

            string encoded = HttpUtility.HtmlEncode(clipped);
            if (string.IsNullOrEmpty(keyword)) return encoded;

            string pattern = Regex.Escape(HttpUtility.HtmlEncode(keyword));
            return Regex.Replace(encoded, pattern,
                "<mark>$0</mark>", RegexOptions.IgnoreCase);
        }
    }
}
