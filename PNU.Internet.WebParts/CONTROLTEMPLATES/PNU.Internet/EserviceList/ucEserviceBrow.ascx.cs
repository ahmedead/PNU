using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucEserviceBrow : UserControl
    {
        public ucEserviceBrow()
        {
            // Default for this control: show all items except TargetGroup = Visitor
            this.ExcludeVisitor = true;
            this.ShowVisitorOnly = false;
        }

        /// <summary>
        /// Show all items except items with TargetGroup = Visitor.
        /// </summary>
        public bool ExcludeVisitor
        {
            get { return ViewState["ExcludeVisitor"] != null && (bool)ViewState["ExcludeVisitor"]; }
            set { ViewState["ExcludeVisitor"] = value; }
        }

        /// <summary>
        /// Show only items with TargetGroup = Visitor.
        /// </summary>
        public bool ShowVisitorOnly
        {
            get { return ViewState["ShowVisitorOnly"] != null && (bool)ViewState["ShowVisitorOnly"]; }
            set { ViewState["ShowVisitorOnly"] = value; }
        }

        public bool NafathLogin
        {
            get { return ViewState["NafathLogin"] != null && (bool)ViewState["NafathLogin"]; }
            set { ViewState["NafathLogin"] = value; }
        }

        public bool ExcludeVisitors
        {
            get { return ExcludeVisitor; }
            set { ExcludeVisitor = value; }
        }

        public bool ShowAllExceptVisitor
        {
            get { return ExcludeVisitor; }
            set { ExcludeVisitor = value; }
        }

        public bool ShowVisitor
        {
            get { return ShowVisitorOnly; }
            set { ShowVisitorOnly = value; }
        }

        public bool VisitorOnly
        {
            get { return ShowVisitorOnly; }
            set { ShowVisitorOnly = value; }
        }

        public bool IsVisitor
        {
            get { return ShowVisitorOnly; }
            set { ShowVisitorOnly = value; }
        }

        public bool ShowNafathOnly
        {
            get { return NafathLogin; }
            set { NafathLogin = value; }
        }

        public bool IsNafath
        {
            get { return NafathLogin; }
            set { NafathLogin = value; }
        }

        public bool NafathOnly
        {
            get { return NafathLogin; }
            set { NafathLogin = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureUrlEnColumn();

            if (!IsPostBack)
            {
                int pageNumber = 1;
                if (Request.QueryString["page"] != null)
                {
                    // int.Parse كان يرمي على ?page=abc فتسقط الصفحة كلها إلى صفحة الخطأ.
                    // قيمة غير رقمية لا تقابلها صفحة: 404 بدل استثناء غير معالَج.
                    if (!int.TryParse(Request.QueryString["page"], out pageNumber))
                    {
                        SetNotFound();
                        pageNumber = 1;
                    }
                    BindServices(pageNumber);
                }
                else
                {
                    BindServices(pageNumber);
                    FillDropDownList();
                    LoadCollegeServices();
                }
            }

            else
            {
                ddlFilterSelectedValue.Value = string.Empty;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindServices(1); // Reset to the first page when searching
        }

        protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterServices(1); // Reset to the first page when filtering
        }

        private bool IsVisitorGroup(EServicesList item)
        {
            if (item == null) return false;
            return string.Equals((item.TargetGroup ?? "").Trim(), "Visitor", StringComparison.OrdinalIgnoreCase)
                || string.Equals((item.TargetGroup_EN ?? "").Trim(), "Visitor", StringComparison.OrdinalIgnoreCase)
                || (!string.IsNullOrEmpty(item.TargetGroup) && item.TargetGroup.IndexOf("زائر", StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(item.DP_TargetGroup) && item.DP_TargetGroup.IndexOf("زائر", StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(item.DP_TargetGroup_EN) && item.DP_TargetGroup_EN.IndexOf("Visitor", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// ShowVisitorOnly: only TargetGroup = Visitor.
        /// ExcludeVisitor: every item whose TargetGroup is not Visitor.
        /// </summary>
        private bool MatchesTargetGroup(EServicesList item)
        {
            if (ShowVisitorOnly) return IsVisitorGroup(item);
            if (ExcludeVisitor) return !IsVisitorGroup(item);
            return true;
        }

        /// <summary>
        /// Maps, applies the TargetGroup filter, then pages the result
        /// (filtering must happen before paging so page counts stay correct).
        /// </summary>
        private void BindPagedItems(SPListItemCollection collitem, int pageNumber)
        {
            List<EServicesList> allItems = SPFactory.MapListItemsToClass<EServicesList>(collitem)
                .Where(MatchesTargetGroup)
                .ToList();

            // Pagination logic
            int pageSize = 6; // Number of items per page
            int totalItems = allItems.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // صفحة خارج المدى (?page=0 أو ما بعد الأخيرة) لا محتوى لها. كانت تردّ 200
            // بقائمة فارغة و canonical يشير إليها، فرصد Semrush (2026-10-12) الصفحات
            // ?page=3 حتى 12 كمحتوى مكرَّر: كلها الصفحة الفارغة نفسها.
            // تبقى الصفحة 1 صالحة حتى لو لم توجد خدمات (totalPages = 0).
            if (pageNumber < 1 || pageNumber > Math.Max(totalPages, 1))
            {
                SetNotFound();
            }

            // Calculate the items to display for the current page
            int skip = (pageNumber - 1) * pageSize;
            if (skip < 0) skip = 0;
            List<EServicesList> eserviceList = allItems.Skip(skip).Take(pageSize).ToList();
            ApplyUrlLanguage(eserviceList);

            // Bind data to the repeater
            rptAllData.DataSource = eserviceList;
            rptAllData.DataBind();

            // Generate pagination links
            GeneratePagination(totalPages, pageNumber);
        }

        private void BindServices(int pageNumber)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            // Build the query based on search and filters
                            // (user text is XML-escaped: a '<' or '&' would otherwise break the CAML)
                            string searchQuery = System.Security.SecurityElement.Escape(txtSearch.Value.Trim());
                            
                                
                            if (!string.IsNullOrEmpty(searchQuery))
                            {
                                query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='ARServiceName' />
                                                    <Value Type='Text'>{searchQuery}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                            }
                            else
                            {
                                string Filterquery = System.Security.SecurityElement.Escape(ddlFilterSelectedValue.Value ?? "");
                                

                                string TargetGroup = PortalHelper.IsArabic ? "TargetGroup_EN" : "TargetGroup";


                                if (!string.IsNullOrEmpty(Filterquery))
                                {
                                    query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='{TargetGroup}' />
                                                    <Value Type='Text'>{Filterquery}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                                }
                                else
                                {
                                    query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";
                                }

                            }

                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                BindPagedItems(collitem, pageNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void FilterServices(int pageNumber)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            // Build the query based on search and filters
                            string Filterquery = string.Empty;
                            if (ddlFilter.SelectedValue == "All")
                            {
                                Filterquery = string.Empty;
                            }
                            else 
                            {
                                Filterquery = ddlFilter.SelectedValue;
                            }
                            ddlFilterSelectedValue.Value = Filterquery;

                            string TargetGroup = PortalHelper.IsArabic ? "TargetGroup_EN" : "TargetGroup";
                            

                            string FilterqueryXml = System.Security.SecurityElement.Escape(Filterquery);
                            if (!string.IsNullOrEmpty(Filterquery))
                            {
                                query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Contains>
                                                    <FieldRef Name='{TargetGroup}' />
                                                    <Value Type='Text'>{FilterqueryXml}</Value>
                                                 </Contains>
                                              </And>
                                           </Where>
                                        </Query>";

                            }
                            else
                            {
                                query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";
                            }

                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                BindPagedItems(collitem, pageNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void FillDropDownList()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();

                            query.Query = $@"
                                                <Where>
                                                        <Eq>
                                                            <FieldRef Name='Active' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                </Where>";



                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                
                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(collitem)
                                    .Where(MatchesTargetGroup)
                                    .ToList();

                                var distinctTargetGroups = eserviceList
                                    .GroupBy(service => service.TargetGroup)  // Group by TargetGroup
                                    .Select(group => group.First())           // Select the first item from each group
                                    .ToList();

                                for (int i = 0; i < distinctTargetGroups.Count; i++)
                                {
                                    distinctTargetGroups[i].TargetGroup = SPFactory.GetLocalizedTitle(distinctTargetGroups[i].TargetGroup_EN, distinctTargetGroups[i].TargetGroup);
                                }
                                ddlFilter.DataSource = distinctTargetGroups;
                                ddlFilter.DataTextField = "TargetGroup" ;
                                ddlFilter.DataValueField = "TargetGroup";
                                ddlFilter.DataBind();
                                if(PortalHelper.IsArabic)
                                    ddlFilter.Items.Insert(0, new ListItem("الكل", "All"));
                                else
                                    ddlFilter.Items.Insert(0, new ListItem("All", "All"));

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void GeneratePagination(int totalPages, int currentPage)
        {
            // Clear existing pagination
            pagination.Controls.Clear();

            // الزرّان المعطَّلان يُرسَمان بلا href. الصنف disabled يخفي الرابط بصرياً فقط،
            // وكان الزاحف يتبعه: "التالي" في الصفحة الأخيرة -> ?page=3 -> ?page=4 ... بلا نهاية،
            // و"السابق" في الصفحة الأولى -> ?page=0.
            bool prevDisabled = currentPage <= 1;
            bool nextDisabled = currentPage >= totalPages;

            // Add "Previous" button
            var prevButton = new HtmlGenericControl("li");
            prevButton.Attributes["class"] = "page-item" + (prevDisabled ? " disabled" : "");
            prevButton.InnerHtml = prevDisabled
                ? "<span class='page-link' aria-disabled='true'>&laquo;</span>"
                : $"<a class='page-link' href='?page={currentPage - 1}'>&laquo;</a>";
            pagination.Controls.Add(prevButton);

            // Add page numbers
            for (int i = 1; i <= totalPages; i++)
            {
                var pageItem = new HtmlGenericControl("li");
                pageItem.Attributes["class"] = "page-item" + (i == currentPage ? " active" : "");
                pageItem.InnerHtml = $"<a class='page-link' href='?page={i}'>{i}</a>";
                pagination.Controls.Add(pageItem);
            }

            // Add "Next" button
            var nextButton = new HtmlGenericControl("li");
            nextButton.Attributes["class"] = "page-item" + (nextDisabled ? " disabled" : "");
            nextButton.InnerHtml = nextDisabled
                ? "<span class='page-link' aria-disabled='true'>&raquo;</span>"
                : $"<a class='page-link' href='?page={currentPage + 1}'>&raquo;</a>";
            pagination.Controls.Add(nextButton);
        }

        /// <summary>
        /// 404 حقيقي مع إبقاء صفحة الموقع المعتادة (TrySkipIisCustomErrors يمنع IIS من استبدالها).
        /// </summary>
        private void SetNotFound()
        {
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            Response.TrySkipIisCustomErrors = true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Value = "";
            BindServices(1);
            FillDropDownList();
            LoadCollegeServices();
        }


        private void LoadCollegeServices()
        {
            try
            {


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {

                            SPList list = web.Lists["ESystems"];
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
                                                </OrderBy>
                                             </Query>";
                                query.RowLimit = 6;
                                SPListItemCollection collitem = list.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {

                                    List<ESystems> _AllData = new List<ESystems>();
                                    _AllData = SPFactory.MapListItemsToClass<ESystems>(collitem);


                                    rptSystems.DataSource = _AllData;
                                    rptSystems.DataBind();

                                }


                            }

                        }
                    }
                });




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        private static volatile bool _urlEnEnsured = false;
        private static readonly object _urlEnLock = new object();

        /// <summary>
        /// Ensures that the URL_EN column exists in the EservicesList list.
        /// </summary>
        public static void EnsureUrlEnColumn()
        {
            if (_urlEnEnsured) return;

            lock (_urlEnLock)
            {
                if (_urlEnEnsured) return;

                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = null;
                                try { list = web.Lists["EservicesList"]; } catch { }

                                if (list != null)
                                {
                                    bool changed = false;
                                    if (!list.Fields.ContainsField("URL_EN"))
                                    {
                                        bool allowUnsafe = web.AllowUnsafeUpdates;
                                        web.AllowUnsafeUpdates = true;
                                        try
                                        {
                                            string internalName = list.Fields.Add("URL_EN", SPFieldType.Note, false);
                                            SPFieldMultiLineText f = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(internalName);
                                            f.Title = "URL (EN)";
                                            f.RichText = false;
                                            f.NumberOfLines = 2;
                                            f.Update();
                                            changed = true;
                                        }
                                        catch
                                        {
                                            if (!list.Fields.ContainsField("URL_EN"))
                                            {
                                                list.Fields.Add("URL_EN", SPFieldType.Text, false);
                                                SPField f = list.Fields.GetFieldByInternalName("URL_EN");
                                                f.Title = "URL (EN)";
                                                f.Update();
                                                changed = true;
                                            }
                                        }
                                        finally
                                        {
                                            web.AllowUnsafeUpdates = allowUnsafe;
                                        }
                                    }

                                    if (changed)
                                    {
                                        list.Update();
                                        try
                                        {
                                            SPView view = list.DefaultView;
                                            if (view != null && !view.ViewFields.Exists("URL_EN"))
                                            {
                                                view.ViewFields.Add(list.Fields.GetFieldByInternalName("URL_EN"));
                                                view.Update();
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    });

                    _urlEnEnsured = true;
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(
                        HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "ucEserviceBrow",
                        "EnsureUrlEnColumn",
                        ex.Message
                    );
                }
            }
        }

        /// <summary>
        /// True on an English web. Uses the web language (same rule as the page labels);
        /// substring checks on the URL ("/en") also matched paths such as /environment.
        /// </summary>
        public static bool IsEnglishSite()
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    return SPContext.Current.Web.Language != 1025;
                }

                return !PortalHelper.IsArabic;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Returns URL_EN if on /en/ site and not empty; otherwise falls back to URL.
        /// </summary>
        public string GetServiceUrl(object urlAr, object urlEn)
        {
            string ar = urlAr != null ? urlAr.ToString().Trim() : string.Empty;
            string en = urlEn != null ? urlEn.ToString().Trim() : string.Empty;

            if (IsEnglishSite())
            {
                return !string.IsNullOrEmpty(en) ? en : ar;
            }

            return !string.IsNullOrEmpty(ar) ? ar : en;
        }

        private void ApplyUrlLanguage(List<EServicesList> list)
        {
            if (list == null || list.Count == 0) return;
            if (IsEnglishSite())
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrWhiteSpace(item.URL_EN))
                    {
                        item.URL = item.URL_EN.Trim();
                    }
                }
            }
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Page.IsPostBack)
        //        {
        //            int pageNumber = 1;
        //            if (Request.QueryString["page"] != null)
        //            {
        //                pageNumber = int.Parse(Request.QueryString["page"]);
        //            }
        //            BindServices(pageNumber);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
        //    }
        //}

        //private void BindServices(int pageNumber)
        //{
        //    try
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
        //        {
        //            using (SPWeb web = site.OpenWeb())
        //            {
        //                SPList list = web.Lists["EservicesList"];
        //                if (list != null)
        //                {
        //                    SPQuery query = new SPQuery();
        //                    query.Query = "<Where><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></Where>";
        //                    //query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Student</Value></Eq></And></Where>";

        //                    // Fetch all items
        //                    SPListItemCollection collitem = list.GetItems(query);

        //                    if (collitem != null)
        //                    {
        //                        // Pagination logic
        //                        int pageSize = 6; // Number of items per page
        //                        int totalItems = collitem.Count;
        //                        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //                        // Calculate the items to display for the current page
        //                        int skip = (pageNumber - 1) * pageSize;
        //                        var pagedItems = collitem.Cast<SPListItem>().Skip(skip).Take(pageSize).ToList();

        //                        // Map items to your custom class
        //                        List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(pagedItems);

        //                        // Bind data to the repeater
        //                        rptAllData.DataSource = eserviceList;
        //                        rptAllData.DataBind();

        //                        // Generate pagination links
        //                        GeneratePagination(totalPages, pageNumber);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
        //    }
        //}

        //private void GeneratePagination(int totalPages, int currentPage)
        //{
        //    // Clear existing pagination
        //    pagination.Controls.Clear();

        //    // Add "Previous" button
        //    var prevButton = new HtmlGenericControl("li");
        //    prevButton.Attributes["class"] = "page-item" + (currentPage == 1 ? " disabled" : "");
        //    prevButton.InnerHtml = $"<a class='page-link' href='?page={currentPage - 1}' tabindex='-1' aria-disabled='true'>&laquo;</a>";
        //    pagination.Controls.Add(prevButton);

        //    // Add page numbers
        //    for (int i = 1; i <= totalPages; i++)
        //    {
        //        var pageItem = new HtmlGenericControl("li");
        //        pageItem.Attributes["class"] = "page-item" + (i == currentPage ? " active" : "");
        //        pageItem.InnerHtml = $"<a class='page-link' href='?page={i}'>{i}</a>";
        //        pagination.Controls.Add(pageItem);
        //    }

        //    // Add "Next" button
        //    var nextButton = new HtmlGenericControl("li");
        //    nextButton.Attributes["class"] = "page-item" + (currentPage == totalPages ? " disabled" : "");
        //    nextButton.InnerHtml = $"<a class='page-link' href='?page={currentPage + 1}'>&raquo;</a>";
        //    pagination.Controls.Add(nextButton);
        //}


    }
}
