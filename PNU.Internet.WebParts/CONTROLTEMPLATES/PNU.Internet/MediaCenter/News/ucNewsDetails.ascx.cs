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
    public partial class ucNewsDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. منع شيربوينت و IIS من تقديم نفس الكاش للأخبار المختلفة
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["RequestID"] = true;

                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["RequestID"] != null)
                    {
                        // رقم الخبر يجب أن يثبت أنه رقم قبل أن يدخل في CAML: بدونه يمكن
                        // إغلاق وسم <Value> وإعادة كتابة شرط <Where> — مثل إسقاط
                        // RequestStatus = 'Approved' فتظهر أخبار غير معتمدة — والاستعلام
                        // يعمل داخل RunWithElevatedPrivileges أي بصلاحيات النظام.
                        int ID;
                        if (!int.TryParse(Page.Request.QueryString["RequestID"].Trim(), out ID))
                        {
                            return;
                        }

                        clsRequestsList _CurrentNew = new clsRequestsList();

                        // 2. تصحيح رابط "جميع الأخبار" ليدعم اللغة الحالية (ar / en) بدلاً من تثبيت /ar/
                        AllMCNews.HRef = SPFactory.GetSiteURL() + "/MediaCenter/Pages/AllNews.aspx";

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                                {
                                    SPList reqList = web.Lists["RequestsList"];
                                    SPListItem sPListItem = reqList.GetItemById(ID);
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(sPListItem);

                                    // نشر عنوان الخبر ووصفه ليلتقطهما SEO.ascx.
                                    // هذا الضابط لم يكن ينشر شيئاً، فكانت SEO.ascx تسقط إلى
                                    // وصف عام مبني على عنوان الصفحة القالبية "تفاصيل الخبر"
                                    // فيتقاسمه كل أخبار المركز الإعلامي:
                                    //   "Coverage and details of: News - News - News Details - Princess Nourah"
                                    ApplySeoMetadata(_CurrentNew, ID);

                                    // جلب الأخبار ذات الصلة
                                    // MainCategory يأتي من القائمة لا من الرابط، لكن تصنيفاً
                                    // يحوي & أو < يكسر الاستعلام — فالترميز صواب في الحالتين.
                                    string safeCategory = System.Security.SecurityElement.Escape(_CurrentNew.MainCategory) ?? string.Empty;

                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<Where>
                                          <And>
                                             <Eq>
                                                <FieldRef Name='RequestStatus' />
                                                <Value Type='Choice'>Approved</Value>
                                             </Eq>
                                             <And>
                                                <Neq>
                                                   <FieldRef Name='ID' />
                                                   <Value Type='Counter'>{ID}</Value>
                                                </Neq>
                                                <Eq>
                                                   <FieldRef Name='MainCategory' />
                                                   <Value Type='Text'>{safeCategory}</Value>
                                                </Eq>
                                             </And>
                                          </And>
                                       </Where>
                                       <OrderBy>
                                          <FieldRef Name='MediaDate' Ascending='False' />
                                       </OrderBy>";

                                    query.RowLimit = 3;
                                    SPListItemCollection _AllData = reqList.GetItems(query);
                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsRequestsList> _AllItems = SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();
                                    }
                                }
                            }
                        });

                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
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

        /// <summary>
        /// ينشر عنوان الخبر ووصفه في HttpContext.Items لتُصدرهما SEO.ascx مرة واحدة.
        ///
        /// الوصف: سلسلة المواقع - عنوان الخبر - رقم الطلب.
        /// رقم الطلب في آخره يضمن التفرّد ولو تشابهت العناوين.
        /// منطق البناء مشترك في Publics مع بقية ضوابط التفاصيل.
        /// </summary>
        private void ApplySeoMetadata(clsRequestsList news, int requestId)
        {
            try
            {
                if (news == null || HttpContext.Current == null) return;

                bool isEnglish = DetectEnglish();

                string newsTitle = isEnglish
                    ? (!string.IsNullOrWhiteSpace(news.Title_EN) ? news.Title_EN : news.Title)
                    : (!string.IsNullOrWhiteSpace(news.Title) ? news.Title : news.Title_EN);

                if (string.IsNullOrWhiteSpace(newsTitle)) return;
                newsTitle = newsTitle.Trim();

                HttpContext.Current.Items["PNU_BrowserTitle"] = newsTitle;
                HttpContext.Current.Items["PNU_BrowserDescription"] =
                    Publics.BuildSeoDescription(Publics.BuildSeoSiteChain(), newsTitle, requestId.ToString());
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                                   "ucNewsDetails.ApplySeoMetadata", ex.Message);
            }
        }

        /// <summary>
        /// مقطع المسار الأول لا البحث في المسار كله: Contains("/en/") تُطابق رمز
        /// كلية الهندسة في /ar/Faculties/EN/... فتُعامَل صفحات عربية على أنها إنجليزية.
        /// </summary>
        private bool DetectEnglish()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();
                return path.StartsWith("/en/", StringComparison.Ordinal)
                    || path.StartsWith("/en-us/", StringComparison.Ordinal)
                    || path.Equals("/en", StringComparison.Ordinal)
                    || path.Equals("/en-us", StringComparison.Ordinal);
            }
            return false;
        }

    }
}
