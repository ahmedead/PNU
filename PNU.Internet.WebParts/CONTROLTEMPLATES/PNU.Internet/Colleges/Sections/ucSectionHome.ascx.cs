using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using System.Runtime.Remoting;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections
{
    public partial class ucSectionHome : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["SecCode"] = true;
                Response.Cache.VaryByParams["Source"] = true;
                string CollegeCode = "";
                if (Page.Request.QueryString["Source"] != null)
                {
                    CollegeCode = Page.Request.QueryString["Source"].ToString();
                }
                else
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["AboutCollege"];
                            if (list != null)
                            {

                                SPListItemCollection collitem = list.GetItems();
                                if (collitem != null)
                                {
                                    if (collitem.Count > 0)
                                        if (collitem[0]["College_Code"] != null)
                                            CollegeCode = collitem[0]["College_Code"].ToString().Trim();


                                }


                            }
                        }
                    }


                }

                if (Page.Request.QueryString["SecCode"] != null)
                {
                    var Source = Page.Request.QueryString["SecCode"].ToString();

                    // كلتا القيمتين تأتيان من عنوان URL وتدخلان في CAML كنص.
                    // هنا لا ينفع int.TryParse كما في ضوابط الأخبار: القيمة نصية
                    // مشروعة، فالحل هو ترميز XML. بدونه يمكن إغلاق وسم <Value>
                    // وإعادة كتابة شرط <Where> كاملاً.
                    string safeSecCode = System.Security.SecurityElement.Escape(Source) ?? string.Empty;
                    string safeCollegeCode = System.Security.SecurityElement.Escape(CollegeCode) ?? string.Empty;

                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList _CollegesList = web.Lists["AllFacultyDepartments"];
                            SPQuery query = new SPQuery();


                            query.Query = $@"<Where>
                                          <And>
                                             <Eq>
                                                <FieldRef Name='Code' />
                                                <Value Type='Text'>{safeSecCode}</Value>
                                             </Eq>
                                             <Eq>
                                                <FieldRef Name='COLL_CODE' />
                                                <Value Type='Text'>{safeCollegeCode}</Value>
                                             </Eq>
                                          </And>
                                       </Where>";



                            List<AllFacultyDepartments> _Data = SPFactory.GetAllItemsByQuery<AllFacultyDepartments>("Admin", Settings.AllFacultyDepartments, query);
                            if (_Data != null && _Data.Count > 0)
                            {
                                // اسم القسم وحده — لا COLL_DESC + " - " + Title.
                                //
                                // السبب: SEO.ascx تبني العنوان بصيغة
                                //     {عنوان الموقع} - {هذا النص} - {اسم الجامعة}
                                // وعنوان الموقع هو اسم الكلية أصلاً. فإذا أرسلنا
                                // "كلية التمريض - التمريض" تكرّرت الكلية، وإذا كان
                                // Title فارغاً بقيت " - " معلّقة، والناتج المرصود حياً:
                                //     كلية التمريض - كلية التمريض - - - جامعة الأميرة نورة
                                // JoinNonEmpty يُسقط الفارغ فلا تبقى فواصل معلّقة.
                                string sectionName = SPFactory.GetLocalizedTitle(
                                    Publics.JoinNonEmpty(" - ", _Data[0].Title),
                                    Publics.JoinNonEmpty(" - ", _Data[0].Title_EN));

                                string collegeName = SPFactory.GetLocalizedTitle(_Data[0].COLL_DESC, _Data[0].COLL_DESC_EN);

                                if (string.IsNullOrWhiteSpace(sectionName)) sectionName = collegeName;
                                if (string.IsNullOrWhiteSpace(sectionName)) sectionName = Source;

                                // عنوانان مختلفان عن قصد:
                                //
                                //  <title>  = الكلية - القسم
                                //     العنوان المعروض يحتاج سياق الكلية. المرصود حياً كان
                                //     "كلية التمريضتمريض الأمومة والطفولة" — الاسمان ملتصقان
                                //     بلا فاصل، لأن الوصل كان بجمع النصوص لا بـ JoinNonEmpty.
                                //
                                //  PNU_BrowserTitle = القسم وحده
                                //     لأن SEO.ascx تبني og:title بصيغة
                                //         {عنوان الموقع} - {هذا النص} - {اسم الجامعة}
                                //     وعنوان الموقع هو اسم الكلية أصلاً، فإرسال الكلية هنا
                                //     يكرّرها. المرصود حياً:
                                //         كلية التمريض - كلية التمريض - - - جامعة الأميرة نورة
                                //
                                // JoinNonEmpty يُسقط الفارغ فلا تبقى فواصل معلّقة عند غياب أحدهما.
                                string fullTitle = Publics.JoinNonEmpty(" - ", collegeName, sectionName);

                                SetBrowserTitle(fullTitle, sectionName);

                                // الوصف: سلسلة المواقع - اسم القسم - رمز القسم.
                                // هذا الضابط لم يكن ينشر وصفاً إطلاقاً، فكانت SEO.ascx
                                // تسقط إلى وصف عام مبني على عنوان الصفحة القالبية
                                // "بيانات القسم" — فتتقاسمه كل أقسام الكلية.
                                // رمز القسم في آخر الوصف يضمن التفرّد.
                                HttpContext.Current.Items["PNU_BrowserDescription"] =
                                    Publics.BuildSeoDescription(Publics.BuildSeoSiteChain(), sectionName, Source);

                                rptMainData.DataSource = _Data;
                                rptMainData.DataBind();


                            }


                        }
                    }


                }





            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
           


            
        }

        /// <param name="pageTitle">ما يظهر في وسم title: الكلية - القسم.</param>
        /// <param name="seoTitle">
        /// ما تلتقطه SEO.ascx: القسم وحده. هي تضيف الكلية والجامعة حوله،
        /// فإرسال الكلية هنا يُنتج تكرارها في og:title.
        /// </param>
        private void SetBrowserTitle(string pageTitle, string seoTitle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageTitle)) return;
                if (string.IsNullOrWhiteSpace(seoTitle)) seoTitle = pageTitle;

                // make the dynamic title available to ucHomeHeader
                HttpContext.Current.Items["PNU_BrowserTitle"] = seoTitle;
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(pageTitle)));
                }
                else
                {
                    // fallback if the master exposes a normal <head runat="server">
                    Page.Title = pageTitle;
                }
            }
            catch (Exception ex)
            {
                //Publics.WriteToLog("ucMediaDetails.SetBrowserTitle", ex);
            }
        }

        // Handles nested master pages (e.g. DGA_Internal.master under a root master)
        private ContentPlaceHolder FindPlaceHolder(System.Web.UI.MasterPage master, string id)
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
