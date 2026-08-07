using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucSystemsBrow : UserControl
    {
        private const int PageSize = 9; // 3 x 3 grid

        protected bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected string OpenSystemLabel
        {
            get { return IsArabic ? "فتح النظام" : "Open System"; }
        }

        #region State (ViewState)

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] == null ? 1 : (int)ViewState["CurrentPage"]; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalPages
        {
            get { return ViewState["TotalPages"] == null ? 1 : (int)ViewState["TotalPages"]; }
            set { ViewState["TotalPages"] = value; }
        }

        private string SearchText
        {
            get { return ViewState["SearchText"] as string ?? string.Empty; }
            set { ViewState["SearchText"] = value; }
        }

        private List<string> SelectedAudiences
        {
            get
            {
                string raw = ViewState["SelectedAudiences"] as string;
                return string.IsNullOrEmpty(raw)
                    ? new List<string>()
                    : raw.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            set { ViewState["SelectedAudiences"] = value == null ? string.Empty : string.Join("|", value); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();

                if (!Page.IsPostBack)
                {
                    ESystemsFieldProvisioner.EnsureFields();
                    BindAudienceFilter();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            bool isAr = IsArabic;

            hlHome.Text = isAr ? "الرئيسية" : "Home";
            hlHome.NavigateUrl = isAr ? "/ar" : "/en";

            litBreadcrumbTitle.Text = isAr ? "الأنظمة الإلكترونية" : "E-Systems";
            litPageTitle.Text = isAr ? "الأنظمة الإلكترونية" : "E-Systems";
            litPageDesc.Text = isAr
                ? "واجهة منظمة للأنظمة الإلكترونية مع شرح مختصر لكل نظام ودوره في الرحلة الرقمية."
                : "An organized interface for e-systems with a brief description of each system and its role in the digital journey.";

            txtSearch.Attributes["placeholder"] = isAr ? "ابحث عن نظام" : "Search for a system";
            txtSearch.Attributes["type"] = "search";
            btnSearch.Text = isAr ? "بحث" : "Search";

            litFilterBtn.Text = isAr ? "تصفية" : "Filter";
            litFilterTitle.Text = isAr ? "الفئة المستهدفة" : "Target Audience";
            txtFilterSearch.Attributes["placeholder"] = isAr ? "بحث" : "Search";
            btnApplyFilter.Text = isAr ? "تطبيق الاختيارات" : "Apply";
            btnResetFilter.Text = isAr ? "إعادة تعيين" : "Reset";

            litEmptyTitle.Text = isAr ? "لا توجد نتائج" : "No results found";
            litEmptyDesc.Text = isAr
                ? "جرّب تعديل كلمات البحث أو إعادة تعيين التصفية."
                : "Try adjusting your search terms or resetting the filter.";
        }

        #region Data

        private List<ESystems> GetAllItems()
        {
            List<ESystems> allData = new List<ESystems>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList("ESystems");
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = @"<Where>
                                               <Eq>
                                                  <FieldRef Name='Visibility' />
                                                  <Value Type='Boolean'>1</Value>
                                               </Eq>
                                            </Where>
                                            <OrderBy>
                                               <FieldRef Name='ItemOrder' Ascending='True' />
                                            </OrderBy>";

                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null && collitem.Count > 0)
                            {
                                allData = SPFactory.MapListItemsToClass<ESystems>(collitem);
                            }
                        }
                    }
                }
            });

            // Compute localized display fields
            bool isAr = IsArabic;
            foreach (ESystems item in allData)
            {
                item.DisplayCategory = isAr ? item.Category : item.Category_EN;
                item.DisplayAudiences = ParseMultiChoice(isAr ? item.TargetAudience : item.TargetAudience_EN);
            }

            return allData;
        }

        private static List<string> ParseMultiChoice(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return new List<string>();

            return raw.Split(new[] { ";#" }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(v => v.Trim())
                      .Where(v => v.Length > 0)
                      .Distinct()
                      .ToList();
        }

        private void BindAudienceFilter()
        {
            try
            {
                List<string> audiences = GetAllItems()
                    .SelectMany(i => i.DisplayAudiences)
                    .Distinct()
                    .OrderBy(a => a)
                    .ToList();

                rptAudienceFilter.DataSource = audiences;
                rptAudienceFilter.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindData()
        {
            try
            {
                List<ESystems> allData = GetAllItems();

                // 1. Search filter (language-appropriate title + description)
                string term = SearchText.Trim();
                if (!string.IsNullOrEmpty(term))
                {
                    bool isAr = IsArabic;
                    allData = allData.Where(i =>
                    {
                        string title = (isAr ? i.Title : i.Title_EN) ?? string.Empty;
                        string desc = (isAr ? i.Description : i.Description_EN) ?? string.Empty;
                        return title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                            || desc.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                    }).ToList();
                }

                // 2. Audience filter (any match)
                List<string> selected = SelectedAudiences;
                if (selected.Count > 0)
                {
                    allData = allData
                        .Where(i => i.DisplayAudiences.Any(a => selected.Contains(a)))
                        .ToList();
                }

                // 3. Paging
                TotalPages = Math.Max(1, (int)Math.Ceiling(allData.Count / (double)PageSize));
                if (CurrentPage > TotalPages) CurrentPage = TotalPages;
                if (CurrentPage < 1) CurrentPage = 1;

                List<ESystems> pageData = allData
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                rptServices.DataSource = pageData;
                rptServices.DataBind();

                // Empty state / pager visibility
                pnlEmpty.Visible = pageData.Count == 0;
                pnlPager.Visible = TotalPages > 1;

                if (pnlPager.Visible)
                {
                    rptPager.DataSource = Enumerable.Range(1, TotalPages).ToList();
                    rptPager.DataBind();

                    lnkPrev.Enabled = CurrentPage > 1;
                    lnkNext.Enabled = CurrentPage < TotalPages;
                    lnkPrev.CssClass = "page-link btn btn-secondary icon-btn navigation-link" + (CurrentPage <= 1 ? " active" : "");
                    lnkNext.CssClass = "page-link btn btn-secondary icon-btn navigation-link" + (CurrentPage >= TotalPages ? " active" : "");
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        #endregion

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchText = txtSearch.Text;
            CurrentPage = 1;
            BindData();
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            List<string> selected = new List<string>();

            foreach (RepeaterItem item in rptAudienceFilter.Items)
            {
                HtmlInputCheckBox chk = item.FindControl("chkAudience") as HtmlInputCheckBox;
                Label lbl = item.FindControl("lblAudience") as Label;
                if (chk != null && chk.Checked && lbl != null)
                {
                    selected.Add(lbl.Text);
                }
            }

            SelectedAudiences = selected;
            CurrentPage = 1;
            BindData();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptAudienceFilter.Items)
            {
                HtmlInputCheckBox chk = item.FindControl("chkAudience") as HtmlInputCheckBox;
                if (chk != null) chk.Checked = false;
            }

            SelectedAudiences = new List<string>();
            SearchText = string.Empty;
            txtSearch.Text = string.Empty;
            CurrentPage = 1;
            BindData();
        }

        protected void Pager_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "PrevPage" && CurrentPage > 1) CurrentPage--;
            if (e.CommandName == "NextPage" && CurrentPage < TotalPages) CurrentPage++;
            BindData();
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GoPage")
            {
                int page;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out page))
                {
                    CurrentPage = page;
                    BindData();
                }
            }
        }

        protected void rptPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnk = e.Item.FindControl("lnkPage") as LinkButton;
            if (lnk == null) return;

            int page = (int)e.Item.DataItem;
            lnk.CssClass = "page-link btn btn-secondary" + (page == CurrentPage ? " active" : "");
        }

        #endregion
    }

    public class ESystems
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }

        private string _description;
        public string Description
        {
            get
            {
                if (_description == null) return string.Empty;
                return _description.Length > 600 ? _description.Substring(0, 600) : _description;
            }
            set { _description = value; }
        }

        private string _description_EN;
        public string Description_EN
        {
            get
            {
                if (_description_EN == null) return string.Empty;
                return _description_EN.Length > 600 ? _description_EN.Substring(0, 600) : _description_EN;
            }
            set { _description_EN = value; }
        }

        public string Url { get; set; }
        public string ID { get; set; }

        // ===== New fields =====
        public string IconClass { get; set; }
        public string Category { get; set; }
        public string Category_EN { get; set; }
        public string TargetAudience { get; set; }        // multi-choice raw (;#val;#val;#)
        public string TargetAudience_EN { get; set; }

        // ===== Computed (set in code-behind, not mapped) =====
        public string DisplayCategory { get; set; }
        public List<string> DisplayAudiences { get; set; } = new List<string>();

        public string SafeIconClass
        {
            get { return string.IsNullOrEmpty(IconClass) ? "hgi-computer" : IconClass.Trim(); }
        }
    }
}
