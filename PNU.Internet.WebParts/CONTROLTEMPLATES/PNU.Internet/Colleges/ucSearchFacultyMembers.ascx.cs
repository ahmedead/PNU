// ucSearchFacultyMembers.ascx.cs
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucSearchFacultyMembers : UserControl
    {
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "CollMembersListName";

        private readonly PagedDataSource _pgsource = new PagedDataSource();
        private int _firstIndex, _lastIndex;
        private int _pageSize = 9; // 3 x 3 card grid

        // Bilingual labels (set in SetTitles — no new resource keys needed)
        protected bool IsArabic;
        protected string FilterText, CollegeText, SectionText, ApplyText, ResetText,
                         ViewProfileText, EmptyTitleText, EmptyHintText, SearchPlaceholder;

        private int CurrentPage
        {
            get => int.TryParse(hfCurrentPage.Value, out var v) ? v : 0;
            set => hfCurrentPage.Value = value.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetTitles();
            txtSearch.Attributes["placeholder"] = SearchPlaceholder;

            btnSearch.ServerClick += btnSearch_Click;
            btnApplyFilter.ServerClick += btnSearch_Click;
            btnClear.ServerClick += btnClear_Click;

            if (!Page.IsPostBack)
            {
                FillSearchLists();
                BindDataFromSession(); // show all members initially (DGA card grid)
            }
        }

        private void SetTitles()
        {
            IsArabic = SPContext.Current.Web.Language == 1025;

            FilterText = IsArabic ? "تصفية" : "Filter";
            CollegeText = IsArabic ? "الكلية" : "College";
            SectionText = IsArabic ? "القسم" : "Section";
            ApplyText = IsArabic ? "تطبيق الاختيارات" : "Apply";
            ResetText = IsArabic ? "إعادة تعيين" : "Reset";
            ViewProfileText = IsArabic ? "عرض الملف" : "View Profile";
            EmptyTitleText = IsArabic ? "لا توجد نتائج مطابقة لبحثك" : "No results match your search";
            EmptyHintText = IsArabic
                ? "يمكنك تجربة التأكد من كتابة كلمة البحث بشكل صحيح، أو استخدام كلمات مختلفة أو مرادفات أخرى."
                : "Try checking your spelling, or use different or more general keywords.";
            SearchPlaceholder = IsArabic
                ? "ابحث بالاسم أو البريد الإلكتروني"
                : "Search by name or email";
        }

        private void FillSearchLists()
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb("Admin"))
                {
                    SPList reqList = web.Lists.TryGetList("CollMembersListName");
                    if (reqList != null)
                    {
                        SPQuery query = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='COLLEGE' /></OrderBy>"
                        };

                        SPListItemCollection items = reqList.GetItems(query);
                        List<clsCollMembersListName> dt = SPFactory.MapListItemsToClass<clsCollMembersListName>(items);

                        if (dt != null && dt.Count > 0)
                        {
                            dt = dt.Where(x => x.COLLEGE != null).ToList();
                        }

                        var distinctColleges = dt.Where(row => row.COLLEGE != null && row.COLLEGE.StartsWith("كلية")).Select(row => row.COLLEGE).Distinct().ToList();
                        var distinctSections = dt.Where(x => x.SECTION != null).Select(x => x.SECTION).Distinct().ToList();

                        ddlColleges.DataSource = distinctColleges;
                        ddlColleges.DataBind();
                        ddlColleges.Items.Insert(0, new ListItem(SPFactory.GetPNUresResource("SelectCollege"), "-1"));

                        ddlSections.DataSource = distinctSections;
                        ddlSections.DataBind();
                        ddlSections.Items.Insert(0, new ListItem(SPFactory.GetPNUresResource("SelectSection"), "-1"));

                        Session["AllFacultyMembersData"] = dt;
                    }
                }
            });
        }

        public void BindDataIntoRepeater(List<clsCollMembersListName> dt)
        {
            try
            {
                if (dt == null || dt.Count == 0)
                {
                    ShowEmpty();
                    return;
                }

                pnlEmpty.Visible = false;
                pnlPaginator.Visible = true;

                _pgsource.DataSource = dt;
                _pgsource.AllowPaging = true;
                _pgsource.PageSize = _pageSize;
                _pgsource.CurrentPageIndex = CurrentPage;

                lbPrevious.Enabled = !_pgsource.IsFirstPage;
                lbNext.Enabled = !_pgsource.IsLastPage;

                rptData.DataSource = _pgsource;
                rptData.DataBind();

                pnlPaginator.Visible = _pgsource.PageCount > 1;
                HandlePaging();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), Page.Title, ex.Message);
            }
        }

        private void ShowEmpty()
        {
            rptData.DataSource = null;
            rptData.DataBind();
            pnlEmpty.Visible = true;
            pnlPaginator.Visible = false;
        }

        // DGA active-page styling (instance method — callable from markup without qualifier)
        protected string GetPageClass(object value)
        {
            int page = Convert.ToInt32(value);
            return page == CurrentPage
                ? "page-link btn btn-secondary active"
                : "page-link btn btn-secondary";
        }

        private void HandlePaging()
        {
            var dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");

            _firstIndex = CurrentPage - 5;
            _lastIndex = CurrentPage > 5 ? CurrentPage + 5 : 10;

            if (_lastIndex > _pgsource.PageCount)
                _lastIndex = _pgsource.PageCount;

            _firstIndex = _lastIndex - 10;
            if (_firstIndex < 0) _firstIndex = 0;

            for (int i = _firstIndex; i < _lastIndex; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Text"] = (i + 1).ToString();
                dr["Value"] = i.ToString();
                dt.Rows.Add(dr);
            }

            rptPaging.DataSource = dt;
            rptPaging.DataBind();
        }

        private void BindDataFromSession()
        {
            if (Session["AllFacultyMembersData"] is List<clsCollMembersListName> dt)
            {
                BindDataIntoRepeater(dt);
            }
        }

        protected void ddlColleges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["AllFacultyMembersData"] is List<clsCollMembersListName> dt)
            {
                if (ddlColleges.SelectedValue == "-1")
                {
                    var distinctSections = dt.Where(x => x.SECTION != null).Select(x => x.SECTION).Distinct().ToList();
                    ddlSections.DataSource = distinctSections;
                }
                else
                {
                    List<clsCollMembersListName> AllCollegeSections = dt.Where(x => x.COLLEGE == ddlColleges.SelectedValue).ToList();
                    var distinctSections = AllCollegeSections.Where(x => x.SECTION != null).Select(x => x.SECTION).Distinct().ToList();
                    ddlSections.DataSource = distinctSections;
                }

                ddlSections.DataBind();
                ddlSections.Items.Insert(0, new ListItem(SPFactory.GetPNUresResource("SelectSection"), "-1"));

                // Keep the filter dropdown open after this AutoPostBack
                hfFilterOpen.Value = "1";

                // Keep current results rendered under the open filter
                BindDataFromSession();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;

            string searchText = txtSearch.Value;
            string selectedCollege = ddlColleges.SelectedValue != "-1" ? ddlColleges.SelectedValue : "";
            string selectedSection = ddlSections.SelectedValue != "-1" ? ddlSections.SelectedValue : "";

            BindData(searchText, selectedCollege, selectedSection);
        }

        private void BindData(string searchText, string selectedCollege, string selectedSection)
        {
            ArrayList qryParam = new ArrayList();

            if (!string.IsNullOrEmpty(searchText))
                qryParam.Add(
                    $"<Or>" +
                        $"<Contains><FieldRef Name='FULL_NAME'/><Value Type='Text'>{searchText}</Value></Contains>" +
                        $"<Or>" +
                            $"<Contains><FieldRef Name='ENGLISH_NAME'/><Value Type='Text'>{searchText}</Value></Contains>" +
                            $"<Contains><FieldRef Name='EMAIL_ADDRESS'/><Value Type='Text'>{searchText}</Value></Contains>" +
                        $"</Or>" +
                    $"</Or>");
            if (!string.IsNullOrEmpty(selectedCollege))
                qryParam.Add($"<Eq><FieldRef Name='COLLEGE'/><Value Type='Text'>{selectedCollege}</Value></Eq>");
            if (!string.IsNullOrEmpty(selectedSection))
                qryParam.Add($"<Eq><FieldRef Name='SECTION'/><Value Type='Text'>{selectedSection}</Value></Eq>");

            var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, 0, "FULL_NAME", "True");
            if (CollITems != null && CollITems.Count > 0)
            {
                List<clsCollMembersListName> dt = SPFactory.MapListItemsToClass<clsCollMembersListName>(CollITems);
                Session["AllFacultyMembersData"] = dt;
                BindDataIntoRepeater(dt);
            }
            else
            {
                ShowEmpty();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            txtSearch.Value = string.Empty;
            FillSearchLists();
            BindDataFromSession();
        }

        protected void lbPrevious_Click(object sender, EventArgs e) { CurrentPage = Math.Max(0, CurrentPage - 1); BindDataFromSession(); }
        protected void lbNext_Click(object sender, EventArgs e) { CurrentPage = CurrentPage + 1; BindDataFromSession(); }

        protected void rptPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            CurrentPage = Convert.ToInt32(e.CommandArgument);
            BindDataFromSession();
        }
    }
}
