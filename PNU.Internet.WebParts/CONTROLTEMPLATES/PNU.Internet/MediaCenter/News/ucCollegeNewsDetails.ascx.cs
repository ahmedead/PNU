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
    public partial class ucCollegeNewsDetails : UserControl
    {
        public string RedirectURL { get; set; } = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string url = HttpContext.Current.Request.Url.ToString();
                    var queryParameters = HttpUtility.ParseQueryString(new Uri(url).Query);
                    string idValue = queryParameters["RequestID"];

                    if (url.Contains("RequestID="))
                    {
                        // معرّف الخبر يجب أن يثبت أنه رقم قبل أن يدخل في استعلام CAML.
                        // بدونه يمكن إغلاق وسم <Value> وإعادة كتابة شرط <Where> كاملاً —
                        // مثل إسقاط RequestStatus = 'Approved' فتظهر أخبار غير معتمدة —
                        // والاستعلام يعمل داخل RunWithElevatedPrivileges أي بصلاحيات النظام.
                        // نفس الإصلاح المطبَّق في ucAdvertisementDetails.
                        int ID;
                        if (!int.TryParse((idValue ?? string.Empty).Trim(), out ID))
                        {
                            return;
                        }

                        clsRequestsList _CurrentNew = new clsRequestsList();

                        string CollegeCode = "";
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    SPList list = web.Lists["DepartmentDetails"];
                                    if (list != null)
                                    {

                                        SPListItemCollection collitem = list.GetItems();
                                        if (collitem != null)
                                        {
                                            if (collitem.Count > 0)
                                                if (collitem[0]["DeptCode"] != null)
                                                    CollegeCode = collitem[0]["DeptCode"].ToString().Trim();
                                        }


                                    }
                                }
                            }

                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                                {
                                    SPList reqList = web.Lists["RequestsList"];

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
                                                        <FieldRef Name='FacultyName_EN' />
                                                        <Value Type='Text'>{CollegeCode}</Value>
                                                    </Eq>
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
                                        List<clsRequestsList> _AllItems = new List<clsRequestsList>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                    SPListItem sPListItem = reqList.GetItemById(ID);
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(sPListItem);

                                    ApplySeoMetadata(_CurrentNew, ID);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        /// <summary>
        /// هل الطلب الحالي إنجليزي؟ يُقاس من مقطع المسار الأول لا بالبحث في المسار كله:
        /// Contains("/en/") تُطابق رمز كلية الهندسة في /ar/Faculties/EN/... فتُعامَل
        /// صفحات عربية على أنها إنجليزية. نفس الإصلاح في SEO.ascx و ucAdvertisementDetails.
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

        /// <summary>
        /// ينشر عنوان الخبر ووصفه ليلتقطهما SEO.ascx.
        ///
        /// المشكلة التي يعالجها: هذا الضابط لم يكن ينشر أي بيانات وصفية إطلاقاً، فكانت
        /// SEO.ascx تسقط إلى وصف عام مبني على عنوان الصفحة القالبية "الأخبار" — فتتقاسمه
        /// كل أخبار الكلية. رصد Semrush ٢٩ صفحة متطابقة الوصف لكل كلية، و١٬٨٦٥ صفحة إجمالاً.
        ///
        /// صيغة الوصف المطلوبة: اسم الموقع - عنوان الخبر - رقم الطلب
        /// رقم الطلب في آخره يضمن التفرّد حتى لو تشابهت عناوين خبرين.
        /// </summary>
        private void ApplySeoMetadata(clsRequestsList news, int requestId)
        {
            try
            {
                if (news == null || HttpContext.Current == null) return;

                bool isEnglish = DetectEnglish();

                // عنوان الخبر باللغة المطلوبة، مع السقوط إلى اللغة الأخرى إن كان فارغاً
                string newsTitle = isEnglish
                    ? (!string.IsNullOrWhiteSpace(news.Title_EN) ? news.Title_EN : news.Title)
                    : (!string.IsNullOrWhiteSpace(news.Title) ? news.Title : news.Title_EN);

                if (string.IsNullOrWhiteSpace(newsTitle)) return;
                newsTitle = newsTitle.Trim();

                // الوصف: سلسلة المواقع - عنوان الخبر - رقم الطلب.
                // المنطق مشترك في Publics حتى لا يتكرّر في كل ضابط تفاصيل.
                string description = Publics.BuildSeoDescription(
                    Publics.BuildSeoSiteChain(), newsTitle, requestId.ToString());

                // 1) للالتقاط بواسطة SEO.ascx
                HttpContext.Current.Items["PNU_BrowserTitle"] = newsTitle;
                HttpContext.Current.Items["PNU_BrowserDescription"] = description;

                // 2) عنوان المتصفح — يُستبدَل محتوى العنصر النائب بدل الإضافة إليه،
                //    وإلا التصق عنوان القالب "تفاصيل الخبر" بعنوان الخبر الحقيقي.
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(newsTitle)));
                }
                Page.Title = newsTitle;

                // 3) احتياطي لماستر بيج لا تحوي SEO.ascx: نحقن الوسوم مباشرةً.
                //    العلم يُرفع في OnInit داخل SEO.ascx فلا يعتمد هذا على ترتيب الضوابط.
                if (HttpContext.Current.Items["PNU_SEO_Active"] == null)
                {
                    var headPh = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                    if (headPh != null)
                    {
                        string encDesc = HttpUtility.HtmlAttributeEncode(description);
                        string encTitle = HttpUtility.HtmlAttributeEncode(newsTitle);
                        string metaTags =
                            "<meta name=\"description\" content=\"" + encDesc + "\" />\r\n" +
                            "<meta property=\"og:title\" content=\"" + encTitle + "\" />\r\n" +
                            "<meta property=\"og:description\" content=\"" + encDesc + "\" />\r\n" +
                            "<meta name=\"twitter:title\" content=\"" + encTitle + "\" />\r\n" +
                            "<meta name=\"twitter:description\" content=\"" + encDesc + "\" />";
                        headPh.Controls.Add(new LiteralControl(metaTags));
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                                   "ucCollegeNewsDetails.ApplySeoMetadata", ex.Message);
            }
        }

        /// <summary>يتعامل مع الماستر بيج المتداخلة.</summary>
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

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            if (RedirectURL != null && RedirectURL != "")
            {
                Response.Redirect(RedirectURL);
            }
            else
                Response.Redirect("default.aspx");

        }
    }
}
