using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.Search.DAL;
using PNU.Internet.Search.Logging;

namespace PNU.Internet.Search.ControlTemplates.PNU.Internet.Search
{
    /// <summary>
    /// Renders the results page. Reads ?q= and ?cat= and ?sort= and ?p=
    /// from the query string, calls SearchIndexDal.Search, and renders
    /// the page-result repeater + the empty-state panel + the paginator.
    /// </summary>
    public partial class ucSearchResults : UserControl
    {
        private const int PAGE_SIZE = 10;

        private string Keyword
        {
            get { return (Request.QueryString["q"] ?? "").Trim(); }
        }

        private string SelectedCategory
        {
            get
            {
                if (ddlCategory != null && ddlCategory.SelectedItem != null
                    && ddlCategory.SelectedValue != "ALL")
                    return ddlCategory.SelectedValue;
                return Request.QueryString["cat"] ?? "";
            }
        }

        private string SortBy
        {
            get
            {
                if (ddlSort != null && ddlSort.SelectedItem != null)
                    return ddlSort.SelectedValue;
                return Request.QueryString["sort"] ?? "rel";
            }
        }

        private int PageIndex
        {
            get
            {
                int p;
                if (!int.TryParse(Request.QueryString["p"], out p) || p < 1)
                    p = 1;
                int hidden;
                if (int.TryParse(ViewState["page"] as string, out hidden) && hidden > 0)
                    return hidden;
                return p;
            }
            set { ViewState["page"] = value.ToString(); }
        }

        // ================================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtKeyword.Text = Keyword;

                    string sort = Request.QueryString["sort"];
                    if (!string.IsNullOrEmpty(sort)
                        && ddlSort.Items.FindByValue(sort) != null)
                        ddlSort.SelectedValue = sort;

                    LoadCategories();
                    string cat = Request.QueryString["cat"];
                    if (!string.IsNullOrEmpty(cat)
                        && ddlCategory.Items.FindByValue(cat) != null)
                        ddlCategory.SelectedValue = cat;
                }

                BindResults();
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("UI",
                    "ucSearchResults.Page_Load", ex.Message);
            }
        }

        // ================================================================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            BindResults();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndex = 1;
            BindResults();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndex = 1;
            BindResults();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            int p = PageIndex;
            if (p > 1) PageIndex = p - 1;
            BindResults();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            PageIndex = PageIndex + 1;
            BindResults();
        }

        // ================================================================
        private void LoadCategories()
        {
            try
            {
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(
                    "كل التصنيفات", "ALL"));

                foreach (var kv in SearchIndexDal.GetCategories())
                {
                    if (string.IsNullOrEmpty(kv.Key)) continue;
                    string text = kv.Key + " (" + kv.Value + ")";
                    ddlCategory.Items.Add(
                        new System.Web.UI.WebControls.ListItem(text, kv.Key));
                }
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("UI", "LoadCategories", ex.Message);
            }
        }

        // ================================================================
        private void BindResults()
        {
            string keyword = (txtKeyword.Text ?? "").Trim();
            if (string.IsNullOrEmpty(keyword)) keyword = Keyword;

            bool isArabic = SPContext.Current != null
                         && SPContext.Current.Web != null
                         && SPContext.Current.Web.Language == 1025;

            string cat = SelectedCategory;
            string catCsv = string.IsNullOrEmpty(cat)
                            || string.Equals(cat, "ALL",
                                StringComparison.OrdinalIgnoreCase)
                ? null : cat;

            int total;
            List<SearchIndexItem> items = SearchIndexDal.Search(
                keyword, isArabic, catCsv, SortBy,
                PageIndex, PAGE_SIZE, out total);

            if (items == null || items.Count == 0)
            {
                pnlResults.Visible = false;
                pnlEmpty.Visible   = true;
                pnlPaginator.Visible = false;
                pnlSummary.Visible = false;
                litEmptyKeyword.Text = Server.HtmlEncode(keyword ?? "");
                return;
            }

            pnlEmpty.Visible = false;
            pnlResults.Visible = true;
            pnlSummary.Visible = true;

            litSummary.Text = string.Format(
                "تم العثور على {0} نتيجة لـ \"{1}\"",
                total, Server.HtmlEncode(keyword ?? ""));

            var view = new List<object>();
            foreach (var it in items)
            {
                string title = it.DisplayTitle(isArabic);
                string body  = it.DisplayContent(isArabic);
                view.Add(new
                {
                    Url              = it.Url,
                    Category         = it.Category,
                    DisplayTitleHtml = title,
                    DisplayDateString= it.DisplayDateString,
                    Snippet          = MakeSnippet(body, keyword)
                });
            }

            rptResults.DataSource = view;
            rptResults.DataBind();

            int totalPages = (total + PAGE_SIZE - 1) / PAGE_SIZE;
            litPageInfo.Text = string.Format("صفحة {0} من {1}",
                PageIndex, Math.Max(totalPages, 1));
            lnkPrev.Visible = PageIndex > 1;
            lnkNext.Visible = PageIndex < totalPages;
            pnlPaginator.Visible = totalPages > 1;
        }

        private static string MakeSnippet(string body, string keyword)
        {
            if (string.IsNullOrEmpty(body)) return "";
            const int MAX = 220;

            if (!string.IsNullOrEmpty(keyword))
            {
                int idx = body.IndexOf(keyword,
                    StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    int start = Math.Max(0, idx - 60);
                    int len   = Math.Min(MAX, body.Length - start);
                    string s = body.Substring(start, len);
                    if (start > 0) s = "..." + s;
                    if (start + len < body.Length) s = s + "...";
                    return s;
                }
            }

            return body.Length <= MAX ? body : body.Substring(0, MAX) + "...";
        }
    }
}
