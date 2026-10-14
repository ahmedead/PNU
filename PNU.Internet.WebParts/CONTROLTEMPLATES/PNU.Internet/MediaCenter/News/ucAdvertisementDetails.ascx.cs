using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucAdvertisementDetails : UserControl
    {
        public string RedirectURL { get; set; } = "default.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["RequestID"] = true;
                Response.Cache.VaryByParams["RequestId"] = true;
                if (!IsPostBack)
                {
                    string reqId = Page.Request.QueryString["RequestId"] ?? Page.Request.QueryString["RequestID"];
                    if (!string.IsNullOrEmpty(reqId))
                    {

                        string CollegeCode = "";

                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = web.Lists.TryGetList("DepartmentDetails");
                                if (list != null)
                                {

                                    SPListItemCollection collitem1 = list.GetItems();
                                    if (collitem1 != null)
                                    {
                                        if (collitem1.Count > 0)
                                            if (collitem1[0]["DeptCode"] != null)
                                                CollegeCode = collitem1[0]["DeptCode"].ToString().Trim();


                                    }


                                }
                            }
                        }


                        // معرّف الإعلان يجب أن يثبت أنه رقم قبل أن يدخل في أي استعلام CAML.
                        // بدون هذا التحقق يمكن إغلاق وسم <Value> وإعادة كتابة شرط <Where> كاملاً
                        // (مثلاً إسقاط RequestStatus = 'Approved')، والاستعلام يُنفَّذ داخل
                        // RunWithElevatedPrivileges أي بصلاحيات النظام لا بصلاحيات الزائرة.
                        // التحقق هنا يسبق الاستعلامين معاً؛ Convert.ToInt32 بعدهما كان متأخراً.
                        int ID;
                        if (!int.TryParse(reqId.Trim(), out ID))
                        {
                            return;
                        }

                        clsAdvertisements _CurrentNew = new clsAdvertisements();

                        //AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/LatestAdvertisements.aspx";

                        string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

                        AllMCNews1.Visible = false;
                        AllMCNews.Visible = false;
                        if (currentUrl.Contains("/MediaCenter/"))
                        {
                            AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host + "/ar/MediaCenter/Pages/LatestAdvertisements.aspx";
                            AllMCNews.Visible = true;
                            
                        }
                        else
                        {
                            // Get the directory path of the current URL and append 'default.aspx'
                            string currentPath = HttpContext.Current.Request.Url.AbsolutePath;
                            string directoryPath = VirtualPathUtility.GetDirectory(currentPath);

                            AllMCNews1.HRef = directoryPath + RedirectURL;
                            AllMCNews1.Visible = true;
                        }

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                                {
                                    SPList reqList = web.Lists["AdvertisementsRequests"];

                                    SPQuery query = new SPQuery();

                                    if(CollegeCode == "")
                                        query.Query = $@"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='RequestStatus' />
                                                            <Value Type='Choice'>Approved</Value>
                                                         </Eq>
                                                        <Neq>
                                                               <FieldRef Name='ID' />
                                                               <Value Type='Counter'>{ID}</Value>
                                                            </Neq>
                                                      </And>
                                                   </Where>
                                                   <OrderBy>
                                                      <FieldRef Name='MediaDate' Ascending='False' />
                                                   </OrderBy>";
                                    else
                                        query.Query = $@"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='RequestStatus' />
                                                            <Value Type='Choice'>Approved</Value>
                                                         </Eq>
                                                         <And>
                                                            <Eq>
                                                               <FieldRef Name='FacultyName_EN' />
                                                               <Value Type='Text'>{CollegeCode}</Value>
                                                            </Eq>
                                                            <Neq>
                                                               <FieldRef Name='ID' />
                                                               <Value Type='Counter'>{ID}</Value>
                                                            </Neq>
                                                         </And>
                                                      </And>
                                                   </Where>
                                                   <OrderBy>
                                                      <FieldRef Name='MediaDate' Ascending='False' />
                                                   </OrderBy>";


                                    query.RowLimit = Convert.ToUInt32(3);
                                    SPListItemCollection _AllData = reqList.GetItems(query);
                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsAdvertisements> _AllItems = new List<clsAdvertisements>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsAdvertisements>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                    SPListItem sPListItem = reqList.GetItemById(ID);
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsAdvertisements>(sPListItem);

                                    string localizedTitle = SPFactory.GetLocalizedTitle(_CurrentNew.Title, _CurrentNew.Title_EN);
                                    ApplySeoMetadata(_CurrentNew, localizedTitle);
                                }
                            }
                        });


                        if (_CurrentNew != null)
                        {
                            List<clsAdvertisements> _AllData = new List<clsAdvertisements>();
                            _AllData.Add(_CurrentNew);
                            rptMainData.DataSource = _AllData;
                            rptMainData.DataBind();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        private bool DetectEnglish()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();

                // مقطع المسار الأول لا البحث في المسار كله: Contains("/en/")
                // كانت تُطابق رمز كلية الهندسة في /ar/Faculties/EN/... فتُبنى
                // بيانات إعلانات عربية بقوالب إنجليزية. نفس الإصلاح في SEO.ascx.cs.
                return path.StartsWith("/en/", StringComparison.Ordinal)
                    || path.StartsWith("/en-us/", StringComparison.Ordinal)
                    || path.Equals("/en", StringComparison.Ordinal)
                    || path.Equals("/en-us", StringComparison.Ordinal);
            }
            return false;
        }

        private void ApplySeoMetadata(clsAdvertisements adv, string localizedTitle)
        {
            try
            {
                if (adv == null || string.IsNullOrWhiteSpace(localizedTitle)) return;

                bool isEnglish = DetectEnglish();

                // 1. Build rich, high-quality description from Summary or MediaContent
                string rawDesc = isEnglish
                    ? (!string.IsNullOrWhiteSpace(adv.Summary_EN) ? adv.Summary_EN : (!string.IsNullOrWhiteSpace(adv.MediaContent_EN) ? adv.MediaContent_EN : (!string.IsNullOrWhiteSpace(adv.Summary) ? adv.Summary : adv.MediaContent)))
                    : (!string.IsNullOrWhiteSpace(adv.Summary) ? adv.Summary : (!string.IsNullOrWhiteSpace(adv.MediaContent) ? adv.MediaContent : (!string.IsNullOrWhiteSpace(adv.Summary_EN) ? adv.Summary_EN : adv.MediaContent_EN)));

                string cleanDesc = Publics.TruncateText(rawDesc, 160);

                string facultyName = isEnglish
                    ? (!string.IsNullOrWhiteSpace(adv.FacultyName_EN) ? adv.FacultyName_EN : adv.FacultyName)
                    : (!string.IsNullOrWhiteSpace(adv.FacultyName) ? adv.FacultyName : adv.FacultyName_EN);

                if (string.IsNullOrWhiteSpace(facultyName) && SPContext.Current != null && SPContext.Current.Web != null)
                {
                    string wTitle = SPContext.Current.Web.Title;
                    if (!string.IsNullOrWhiteSpace(wTitle) && !wTitle.Contains("جامعة") && !wTitle.Contains("Princess"))
                    {
                        facultyName = wTitle;
                    }
                }

                if (string.IsNullOrWhiteSpace(cleanDesc))
                {
                    cleanDesc = isEnglish
                        ? (!string.IsNullOrWhiteSpace(facultyName)
                            ? $"Details and information on announcement: {localizedTitle} - {facultyName}, Princess Nourah bint Abdulrahman University."
                            : $"Details and information on announcement: {localizedTitle} - Princess Nourah bint Abdulrahman University.")
                        : (!string.IsNullOrWhiteSpace(facultyName)
                            ? $"تفاصيل ومعلومات إعلان: {localizedTitle} في {facultyName} بجامعة الأميرة نورة بنت عبد الرحمن."
                            : $"تفاصيل ومعلومات إعلان: {localizedTitle} بجامعة الأميرة نورة بنت عبد الرحمن.");
                }

                // 2. Build full hierarchical title: [الكلية] - [عنوان الإعلان] - جامعة الأميرة نورة بنت عبد الرحمن
                string universityName = isEnglish
                    ? "Princess Nourah bint Abdulrahman University"
                    : "جامعة الأميرة نورة بنت عبد الرحمن";

                // many ads share the same title: append the request id so <title> stays unique
                string uniqueTitle = !string.IsNullOrWhiteSpace(adv.ID) ? $"{localizedTitle} - {adv.ID.Trim()}" : localizedTitle;

                string fullPageTitle = !string.IsNullOrWhiteSpace(facultyName)
                    ? $"{facultyName} - {uniqueTitle} - {universityName}"
                    : $"{uniqueTitle} - {universityName}";

                // 3. Publish to HttpContext.Current.Items for SEO.ascx and ucHomeHeader
                HttpContext.Current.Items["PNU_BrowserTitle"] = localizedTitle;
                HttpContext.Current.Items["PNU_BrowserDescription"] = cleanDesc;
                if (!string.IsNullOrWhiteSpace(adv.AttachmentURL))
                {
                    HttpContext.Current.Items["PNU_OgImage"] = adv.AttachmentURL;
                }

                // 4. Update browser PlaceHolderPageTitle and Page.Title
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(fullPageTitle)));
                }
                Page.Title = fullPageTitle;

                // 5. Fallback for MasterPages without SEO.ascx: Inject tags into PlaceHolderAdditionalPageHead
                if (HttpContext.Current.Items["PNU_SEO_Active"] == null)
                {
                    var headPh = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                    if (headPh != null)
                    {
                        string encodedDesc = HttpUtility.HtmlAttributeEncode(cleanDesc);
                        string encodedTitle = HttpUtility.HtmlAttributeEncode(fullPageTitle);
                        string metaTags = $"<meta name=\"description\" content=\"{encodedDesc}\" />\r\n" +
                                          $"<meta property=\"og:title\" content=\"{encodedTitle}\" />\r\n" +
                                          $"<meta property=\"og:description\" content=\"{encodedDesc}\" />\r\n" +
                                          $"<meta name=\"twitter:title\" content=\"{encodedTitle}\" />\r\n" +
                                          $"<meta name=\"twitter:description\" content=\"{encodedDesc}\" />";
                        headPh.Controls.Add(new LiteralControl(metaTags));
                    }
                }

                // 6. Client Script: Automatically synchronize the MasterPage banner H1 and breadcrumbs
                // to replace any static "تفاصيل الإعلان" text with the real dynamic announcement title
                string jsonTitle = HttpUtility.JavaScriptStringEncode(localizedTitle);
                string syncScript = @"<script type=""text/javascript"">
(function() {
    function syncAdvTitle() {
        var dynTitle = """ + jsonTitle + @""";
        if (!dynTitle) return;
        var h1Elements = document.querySelectorAll('.content h1.h2, main h1, .breadcrumbnew h2');
        for (var i = 0; i < h1Elements.length; i++) {
            var txt = h1Elements[i].textContent.trim();
            if (txt === 'تفاصيل الإعلان' || txt === 'Advertisement Details' || txt === 'تفاصيل الاعلان' || txt === '') {
                h1Elements[i].textContent = dynTitle;
            }
        }
        var breadcrumbs = document.querySelectorAll('.breadcrumb-item.active span, .breadcrumb li.active, .breadcrumb-item.active');
        for (var j = 0; j < breadcrumbs.length; j++) {
            var bTxt = breadcrumbs[j].textContent.trim();
            if (bTxt === 'تفاصيل الإعلان' || bTxt === 'Advertisement Details' || bTxt === 'تفاصيل الاعلان') {
                breadcrumbs[j].textContent = dynTitle;
            }
        }
    }
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', syncAdvTitle);
    } else {
        syncAdvTitle();
    }
})();
</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "PNU_Adv_TitleSync", syncScript, false);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAdvertisementDetails.ApplySeoMetadata", ex.Message);
            }
        }

        // Handles nested master pages (e.g. DGA_Internal.master under a root master)
        private ContentPlaceHolder FindPlaceHolder(MasterPage master, string id)
        {
            while (master != null)
            {
                var ph = master.FindControl(id) as ContentPlaceHolder;
                if (ph != null) return ph;
                master = master.Master;
            }
            return null;
        }

    }

}
