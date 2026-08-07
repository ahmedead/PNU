using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Search
{
    public partial class ucSearchResults : System.Web.UI.UserControl
    {
        private const int PAGE_SIZE = 10;
        private const int MAX_PAGER_LINKS = 5;

        protected int PageIndex { get; private set; }
        protected string Keyword { get; private set; }
        protected string Category { get; private set; }
        protected string SortBy { get; private set; }
        protected bool IsArabic { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = DetectArabic();

            txtKeyword.Attributes["type"] = "search";
            txtKeyword.Attributes["placeholder"] = IsArabic ? "بحث" : "Search";
            btnSearch.Text = IsArabic ? "بحث" : "Search";
            btnSearch.Attributes["aria-label"] = IsArabic ? "بحث" : "Search";

            litSortLabel.Text = IsArabic ? "ترتيب حسب" : "Sort by";
            litFilterLabel.Text = IsArabic ? "تصفية" : "Filter";

            Keyword = Request.QueryString["q"] ?? "";
            Category = Request.QueryString["cat"] ?? "";
            SortBy = Request.QueryString["sort"] ?? "rel";
            PageIndex = ParseInt(Request.QueryString["p"], 1);
            if (PageIndex < 1) PageIndex = 1;

            if (!IsPostBack) txtKeyword.Text = Keyword;

            BindCategories();
            BindResults();
            DataBind();   // resolve <%# %> in markup
        }

        // ----------------------------------------------------------------
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = txtKeyword.Text == null ? "" : txtKeyword.Text.Trim();
            string url = Request.Url.AbsolutePath
                + "?q=" + HttpUtility.UrlEncode(q)
                //+ "&cat=" + HttpUtility.UrlEncode(Category ?? "")
                + "&sort=" + HttpUtility.UrlEncode(SortBy)
                + "&p=1";
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // ----------------------------------------------------------------
        private void BindCategories()
        {
            var selected = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(Category))
            {
                foreach (string s in Category.Split(','))
                {
                    string t = s == null ? "" : s.Trim();
                    if (t.Length > 0) selected.Add(t);
                }
            }

            var data = new List<object>();
            try
            {
                int idx = 0;
                foreach (var c in SearchIndexDal.GetCategories())
                {
                    string display = c.Display(IsArabic);
                    if (string.IsNullOrEmpty(display)) continue;

                    string value = IsArabic
                        ? (string.IsNullOrEmpty(c.CategoryAr) ? c.CategoryEn : c.CategoryAr)
                        : (string.IsNullOrEmpty(c.CategoryEn) ? c.CategoryAr : c.CategoryEn);

                    data.Add(new
                    {
                        Index = idx++,
                        Value = value,
                        Display = display,
                        Checked = selected.Contains(value)
                    });
                }
            }
            catch { /* dropdown silently empty on DB error */ }

            rptCategories.DataSource = data;
            rptCategories.DataBind();
        }

        // ----------------------------------------------------------------
        private void BindResults()
        {
            int total;
            var rows = SearchIndexDal.Search(Keyword, IsArabic, Category,
                SortBy, PageIndex, PAGE_SIZE, out total);

            if (rows == null)
                rows = new List<SearchIndexItem>();

            // Title strip - matches search-result.html
            string queryDisplay = string.IsNullOrEmpty(Keyword)
                ? (IsArabic ? "كل النتائج" : "All results")
                : Keyword;
            litResultsTitle.Text = IsArabic
                ? "نتيجة البحث عن \"" + Server.HtmlEncode(queryDisplay) + "\""
                : "Search results for \"" + Server.HtmlEncode(queryDisplay) + "\"";

            if (total > 0)
            {
                string countText = IsArabic
                    ? total + " نتائج وجدت"
                    : total + " results found";
                litResultsCount.Text =
                    "<span class='text-body-secondary d-block mt-2'>"
                    + countText + "</span>";
            }
            else
            {
                litResultsCount.Text = "";
            }

            if (rows.Count == 0)
            {
                phResults.Visible = false;
                phEmpty.Visible = true;
                return;
            }

            phResults.Visible = true;
            phEmpty.Visible = false;

            var data = new List<object>();
            foreach (var it in rows)
            {
                string title = it.DisplayTitle(IsArabic);
                string content = it.DisplayContent(IsArabic);
                string snippet = MakeSnippet(content, Keyword, 240);

                string url = it.Url ?? "";
                if (IsArabic)
                    url = Regex.Replace(url, "/en/", "/ar/", RegexOptions.IgnoreCase);
                else
                    url = Regex.Replace(url, "/ar/", "/en/", RegexOptions.IgnoreCase);

                data.Add(new
                {
                    Title = string.IsNullOrEmpty(title)
                                    ? (IsArabic ? "(بدون عنوان)" : "(no title)")
                                    : title,
                    Url = url,
                    Category = it.DisplayCategory(IsArabic),
                    DateString = it.DisplayDateString,
                    Snippet = snippet,
                    Keyword = Keyword
                });
            }

            rptResults.DataSource = data;
            rptResults.DataBind();

            BuildPager(total);
        }

        // ----------------------------------------------------------------
        private void BuildPager(int total)
        {
            int totalPages = (total + PAGE_SIZE - 1) / PAGE_SIZE;
            if (totalPages < 1) totalPages = 1;

            // Prev
            if (PageIndex > 1)
            {
                lnkPrev.NavigateUrl = BuildPageUrl(PageIndex - 1);
                lnkPrev.CssClass = "page-link btn btn-secondary icon-btn navigation-link";
            }
            else
            {
                lnkPrev.NavigateUrl = "#";
                lnkPrev.CssClass =
                    "page-link btn btn-secondary icon-btn navigation-link active";
                lnkPrev.Attributes["tabindex"] = "-1";
            }

            // Next
            if (PageIndex < totalPages)
            {
                lnkNext.NavigateUrl = BuildPageUrl(PageIndex + 1);
                lnkNext.CssClass = "page-link btn btn-secondary icon-btn navigation-link";
            }
            else
            {
                lnkNext.NavigateUrl = "#";
                lnkNext.CssClass =
                    "page-link btn btn-secondary icon-btn navigation-link active";
                lnkNext.Attributes["tabindex"] = "-1";
            }

            int start = Math.Max(1, PageIndex - MAX_PAGER_LINKS / 2);
            int end = Math.Min(totalPages, start + MAX_PAGER_LINKS - 1);
            start = Math.Max(1, end - MAX_PAGER_LINKS + 1);

            var pages = new List<object>();
            for (int i = start; i <= end; i++)
            {
                pages.Add(new
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Url = BuildPageUrl(i),
                    IsCurrent = (i == PageIndex)
                });
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        // ----------------------------------------------------------------
        private string BuildPageUrl(int page)
        {
            var qs = HttpUtility.ParseQueryString(Request.Url.Query);
            qs["q"] = Keyword;
            qs["cat"] = Category;
            qs["sort"] = SortBy;
            qs["p"] = page.ToString(CultureInfo.InvariantCulture);
            return Request.Url.AbsolutePath + "?" + qs.ToString();
        }

        protected string BuildSortUrl(string sort)
        {
            var qs = HttpUtility.ParseQueryString(Request.Url.Query);
            qs["q"] = Keyword;
            qs["cat"] = Category;
            qs["sort"] = sort;
            qs["p"] = "1";
            return Request.Url.AbsolutePath + "?" + qs.ToString();
        }

        // ----------------------------------------------------------------
        // Highlight helper used from the repeater. Encodes the plain-text
        // snippet first then wraps every keyword occurrence in <mark>.
        // Method is INSTANCE so the repeater can resolve it without a
        // type qualifier (avoids CS0103).
        // ----------------------------------------------------------------
        public string HighlightSnippet(string snippet, string keyword)
        {
            if (string.IsNullOrEmpty(snippet)) return "";
            string encoded = HttpUtility.HtmlEncode(snippet);
            if (string.IsNullOrWhiteSpace(keyword)) return encoded;

            string encKw = HttpUtility.HtmlEncode(keyword.Trim());
            if (encKw.Length == 0) return encoded;

            try
            {
                string pattern = Regex.Escape(encKw);
                return Regex.Replace(encoded, pattern,
                    m => "<mark>" + m.Value + "</mark>",
                    RegexOptions.IgnoreCase);
            }
            catch { return encoded; }
        }

        // ----------------------------------------------------------------
        private static string MakeSnippet(string content, string keyword, int max)
        {
            if (string.IsNullOrEmpty(content)) return "";
            content = Regex.Replace(content, @"\s+", " ").Trim();
            if (content.Length <= max) return content;

            if (!string.IsNullOrEmpty(keyword))
            {
                int idx = content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    int start = Math.Max(0, idx - max / 4);
                    int len = Math.Min(max, content.Length - start);
                    string s = content.Substring(start, len);
                    if (start > 0) s = "..." + s;
                    if (start + len < content.Length) s = s + "...";
                    return s;
                }
            }
            return content.Substring(0, max) + "...";
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

        private static int ParseInt(string s, int defaultValue)
        {
            int v;
            if (!string.IsNullOrEmpty(s) && int.TryParse(s, out v)) return v;
            return defaultValue;
        }
    }
}

