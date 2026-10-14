using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucViewFacultyMembers : UserControl
    {
        private const string UniversityNameAr = "جامعة الأميرة نورة بنت عبدالرحمن";
        private const string UniversityNameEn = "Princess Nourah bint Abdulrahman University";

        /// <summary>
        /// قيمة ?view= كما وردت، مقصوصة الأطراف. فارغة إن غاب المعامل -
        /// بدل Request.QueryString["view"].ToString() التي ترمي NullReferenceException.
        /// </summary>
        private string ViewEmail
        {
            get { return (Request.QueryString["view"] ?? string.Empty).Trim(); }
        }

        protected bool IsCollegeMemebers()
        {
            bool IsMemeber = false;
            try
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                if (user == null || string.IsNullOrEmpty(user.Email))
                    return false;

                var userEmail = user.Email;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists["CollMembersListName"];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                                "<Eq>" +
                                                    "<FieldRef Name='EMAIL_ADDRESS'/><Value Type='Text'>" + System.Security.SecurityElement.Escape(userEmail.ToLower().Trim()) + "</Value>" +
                                                "</Eq>" +
                                                "</Where>";
                            query.RowLimit = 1;

                            SPListItemCollection collection = list.GetItems(query);
                            IsMemeber = collection != null && collection.Count > 0;
                        }
                    }
                });

                return IsMemeber;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["view"] = true;
                if (!IsPostBack)
                {
                    if (!IsCollegeMemebers())
                    {
                        divEdit.Visible = false;
                    }

                    LoadData();

                    // تبويب المقررات محمَّل سلفاً: ucMemberCourses يربط بياناته في Page_Load
                    // الخاص به، فلا داعي لإعادة إرسال الصفحة كلها عند النقر عليه.
                    // بقية التبويبات (Google Scholar والمكتبة والتغريدات و ORCID) تُحمَّل
                    // عند الطلب عبر onserverclick، فتبقى قابلة للإرسال.
                    //
                    // كان هنا Task.Run(LoadCollegeTabFunctions) يعيد ربط المقررات على خيط
                    // خلفي ثم يضع "return false" على كل التبويبات. على ذلك الخيط لا يوجد
                    // HttpContext ولا SPContext، وهو يسابق عرض الصفحة: في الصفحة الحيّة
                    // كان يخسر السباق دائماً فلم يظهر أثره إلا كاستعلام مقررات مكرَّر،
                    // ولو ربحه لعطّل التحميل عند الطلب للتبويبات الأربعة.
                    college22tab.Attributes["onclick"] = "return false;";
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                string email = ViewEmail;

                clsCollMembersListName _FacultyMember;
                bool lookupSucceeded = busclsFacultyMembers.TryGetFacultyMemberByEmail(email, out _FacultyMember);

                if (_FacultyMember == null)
                {
                    // بلا ?view= أو ببريد غير مسجَّل: لا يوجد عضو يُعرض. كانت الصفحة
                    // تردّ 200 قابلة للفهرسة وبـ canonical يشير إليها، فيصير كل
                    // ?view=<أي نص> صفحةً مستقلة فارغة (soft 404) في أعين محركات البحث.
                    // فشل البحث نفسه (استثناء) لا يُعامَل كغياب: 404 هنا قد يُسقط من
                    // الفهرس صفحات أعضاء حقيقيين بسبب عطل عابر.
                    if (lookupSucceeded)
                    {
                        SetNotFound();
                    }
                    return;
                }

                SPSecurity.RunWithElevatedPrivileges(delegate () {

                    AllDataCollMembersListName Data = SPFactory.MapClassToClass<AllDataCollMembersListName>(_FacultyMember);

                    // عدّادات التبويبات المحمَّلة عند الطلب - تُعرض صفراً حتى يُفتح التبويب
                    Data.CountAnnounssements = "0";
                    Data.CountGoogle = "0";
                    Data.CountORCID = "0";
                    Data.CountTweets = "0";

                    rptMainData.DataSource = new List<AllDataCollMembersListName> { Data };
                    rptMainData.DataBind();
                });

                // SEO Fix: Near Duplicates (#21), Dynamic Title (#12), Meta Description (#13), Canonicals (#8)
                ApplyMemberSeoMetadata(_FacultyMember, email);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        /// <summary>
        /// 404 حقيقي للزائر ولمحركات البحث. TrySkipIisCustomErrors يمنع IIS من استبدال
        /// الاستجابة بصفحة خطأه، فتبقى صفحة الموقع المعتادة مع رمز 404.
        /// </summary>
        private void SetNotFound()
        {
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            Response.TrySkipIisCustomErrors = true;
        }

        /// <summary>
        /// اللغة من مقطع المسار الأول - التعريف نفسه في SEO.ascx.cs (IsCurrentRequestEnglish)،
        /// فيتّفق وصف الصفحة مع وسوم hreflang التي تُصدرها.
        /// </summary>
        private bool IsEnglishRequest()
        {
            string path = Request.Url.AbsolutePath;
            return path.StartsWith("/en/", StringComparison.OrdinalIgnoreCase)
                || path.Equals("/en", StringComparison.OrdinalIgnoreCase);
        }

        private static string Pick(bool english, string ar, string en)
        {
            if (english && !string.IsNullOrWhiteSpace(en)) return en.Trim();
            if (!string.IsNullOrWhiteSpace(ar)) return ar.Trim();
            return (en ?? string.Empty).Trim();
        }

        private void ApplyMemberSeoMetadata(clsCollMembersListName member, string email)
        {
            try
            {
                if (this.Page == null || member == null)
                    return;

                // الصفحة الإنجليزية كانت تأخذ قالباً عربياً بقيم إنجليزية داخله، مثل:
                //     "الصفحة الأكاديمية والإنتاج العلمي لـ Noura Mostafa Mohamed ..."
                // الحقول الإنجليزية (*_EN) موجودة، فيُبنى كل نص بلغة الصفحة.
                bool english = IsEnglishRequest();

                string memberName = Pick(english, member.FULL_NAME, member.ENGLISH_NAME);
                if (string.IsNullOrEmpty(memberName))
                    memberName = english ? "Faculty member" : "عضو هيئة التدريس";

                string profession = Pick(english, member.PROFESSION, member.PROFESSION_EN);
                string section = Pick(english, member.SECTION, member.SECTION_EN);
                string college = Pick(english, member.COLLEGE, member.COLLEGE_EN);
                string spec = Pick(english, member.SPECIALIZATION, member.SPECIALIZATION_EN);
                string minor = Pick(english, member.SPECIAL_CPECIALIZATION, member.SPECIAL_CPECIALIZATION_EN);
                string qual = Pick(english, member.QUALIFICATION_NAME, member.QUALIFICATION_NAME_EN);
                string universityName = english ? UniversityNameEn : UniversityNameAr;

                // 1. عنوان الصفحة: الاسم - الرتبة (الكلية) | الجامعة
                // JoinNonEmpty يُسقط الفارغ؛ كان الوصل المباشر يُنتج "الاسم -  (الكلية)"
                // حين تغيب الرتبة.
                string headline = Publics.JoinNonEmpty(" - ", memberName, profession);
                if (!string.IsNullOrEmpty(college)) headline += " (" + college + ")";
                SetSharePointPageTitle(headline + " | " + universityName);

                // 2. الوصف: الرتبة والمؤهل والقسم والكلية والتخصص
                List<string> details = new List<string>();
                if (!string.IsNullOrEmpty(profession)) details.Add(profession);
                if (!string.IsNullOrEmpty(qual)) details.Add(qual);
                if (!string.IsNullOrEmpty(section)) details.Add((english ? "Department of " : "قسم ") + section);
                if (!string.IsNullOrEmpty(college)) details.Add(college);
                if (!string.IsNullOrEmpty(spec)) details.Add((english ? "specialization: " : "تخصص ") + spec);
                if (!string.IsNullOrEmpty(minor)) details.Add((english ? "sub-specialization: " : "التخصص الدقيق: ") + minor);

                string detailsStr = details.Count > 0
                    ? " (" + string.Join(english ? ", " : "، ", details) + ")"
                    : "";
                string desc = english
                    ? string.Format("Academic profile and research of {0}{1} at {2}. Courses, publications and academic contact.", memberName, detailsStr, universityName)
                    : string.Format("الصفحة الأكاديمية والإنتاج العلمي لـ {0}{1} بجامعة الأميرة نورة بنت عبدالرحمن. المقررات الدراسية، الأبحاث المنشورة، والتواصل الأكاديمي.", memberName, detailsStr);

                // ١٦٠ حرفاً لا ٢٥٠: ما بعدها يُقتطع في نتائج البحث فلا يُقرأ.
                if (desc.Length > 160) desc = desc.Substring(0, 159).TrimEnd() + "…";

                // ملاحظة على ما كان هنا (سبب ١٬١٠٦ صفحة بوصف مكرَّر):
                // كان هذا الضابط يحذف أي canonical أو description موجود ثم يضيف نسخته،
                // بحلقة لا تجد شيئاً أبداً لأن SEO.ascx تكتب وسومها نصاً خاماً، فيبقى
                // الوسمان معاً. الصواب ألّا يتنازع ضابطان على نفس الوسم: يُنشَر المحتوى
                // في HttpContext.Items وتتولّى SEO.ascx الإصدار وحدها.
                HttpContext.Current.Items["PNU_BrowserDescription"] = desc;

                // واسم العضو ليبني SEO.ascx منه og:title و twitter:title.
                HttpContext.Current.Items["PNU_BrowserTitle"] = memberName;

                // 3. Schema.org Person JSON-LD
                if (this.Page.Header != null)
                {
                    string safeName = HttpUtility.JavaScriptStringEncode(memberName);
                    string safeProf = HttpUtility.JavaScriptStringEncode(profession);
                    string safeEmail = HttpUtility.JavaScriptStringEncode(member.EMAIL_ADDRESS ?? email);
                    string safeOrg = HttpUtility.JavaScriptStringEncode(Publics.JoinNonEmpty(" - ", universityName, college));
                    string safeUrl = HttpUtility.JavaScriptStringEncode(BuildCanonicalUrl());

                    string jsonLd = string.Format(@"
<script type=""application/ld+json"">
{{
  ""@context"": ""https://schema.org"",
  ""@type"": ""Person"",
  ""name"": ""{0}"",
  ""jobTitle"": ""{1}"",
  ""email"": ""{2}"",
  ""worksFor"": {{
    ""@type"": ""CollegeOrUniversity"",
    ""name"": ""{3}""
  }},
  ""url"": ""{4}""
}}
</script>", safeName, safeProf, safeEmail, safeOrg, safeUrl);

                    this.Page.Header.Controls.Add(new LiteralControl(jsonLd));
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ApplyMemberSeoMetadata", ex.Message);
            }
        }

        /// <summary>
        /// url في JSON-LD يجب أن يطابق canonical الذي تُصدره SEO.ascx حرفاً بحرف،
        /// فيُبنى بقواعدها نفسها:
        ///   - https + نطاق مجموعة المواقع (لا Request.Url الذي قد يكون http خلف موازن الحمل)
        ///   - مسار الملف كما خزّنه شيربوينت (ServerRelativeUrl) لا كما طلبه الزائر
        ///   - قيمة view كما وردت، مرمَّزة بـ SEO.EncodeQueryComponent ("@" حرفياً)
        /// الصيغة السابقة (Request.Url + بريد بأحرف صغيرة) كانت تختلف عنه في الحالة والنطاق.
        /// </summary>
        private string BuildCanonicalUrl()
        {
            string authority = "https://" + Request.Url.Authority;
            string path = Request.Url.AbsolutePath;
            try
            {
                if (SPContext.Current != null)
                {
                    if (SPContext.Current.Site != null && !string.IsNullOrEmpty(SPContext.Current.Site.Url))
                        authority = "https://" + new Uri(SPContext.Current.Site.Url).Authority;
                    if (SPContext.Current.File != null && !string.IsNullOrEmpty(SPContext.Current.File.ServerRelativeUrl))
                        path = SPContext.Current.File.ServerRelativeUrl;
                }
            }
            catch { }

            string view = Request.QueryString["view"] ?? string.Empty;
            return authority + path + "?view=" +
                   global::PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.SEO.EncodeQueryComponent(view);
        }

        private void SetSharePointPageTitle(string fullTitle)
        {
            try
            {
                this.Page.Title = fullTitle;

                if (this.Page.Master != null)
                {
                    var phTitle = FindPlaceHolder(this.Page.Master, "PlaceHolderPageTitle");
                    if (phTitle != null)
                    {
                        phTitle.Controls.Clear();
                        phTitle.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(fullTitle)));
                    }

                    var phTitleArea = FindPlaceHolder(this.Page.Master, "PlaceHolderPageTitleInTitleArea");
                    if (phTitleArea != null)
                    {
                        phTitleArea.Controls.Clear();
                        phTitleArea.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(fullTitle)));
                    }
                }

                // Client-side fallback to guarantee browser tab title updates
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateDocTitle",
                    $"document.title = {HttpUtility.JavaScriptStringEncode(fullTitle, true)};", true);
            }
            catch { }
        }

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

        // ------------------------------------------------------------------
        // التبويبات المحمَّلة عند الطلب: كل نقرة تُرسل الصفحة فيربط التبويب بياناته
        // ثم يُعطَّل إرساله ("return false") لأن بياناته صارت محفوظة في ViewState.
        // ------------------------------------------------------------------

        protected void college2tab_Click(object sender, EventArgs e)
        {
            try
            {
                ucMemberCourses ucMemberCoursesl = FindControl("ucMemberCourses") as ucMemberCourses;
                if (ucMemberCoursesl != null)
                {
                    ucMemberCoursesl.BindData();
                }
                ActivateTab(college22tab, "z2-tab-pane");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "college2tab_Click", ex.Message);
            }
        }

        protected void college3tab_Click(object sender, EventArgs e)
        {
            try
            {
                ucGoogleScolar ucGoogleScolar = FindControl("ucGoogleScolar") as ucGoogleScolar;
                if (ucGoogleScolar != null)
                {
                    ucGoogleScolar.BindDataIntoRepeater(ViewEmail);
                }
                ActivateTab(college33tab, "z3-tab-pane");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "college3tab_Click", ex.Message);
            }
        }

        protected void college4tab_Click(object sender, EventArgs e)
        {
            try
            {
                ucMemberLibrary ucMemberLibrary = FindControl("ucMemberLibrary") as ucMemberLibrary;
                if (ucMemberLibrary != null)
                {
                    ucMemberLibrary.BindDataIntoRepeater(ViewEmail);
                }
                ActivateTab(college44tab, "z4-tab-pane");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "college4tab_Click", ex.Message);
            }
        }

        protected void college6tab_Click(object sender, EventArgs e)
        {
            try
            {
                ucMemberTweets ucMemberTweets = FindControl("ucMemberTweets") as ucMemberTweets;
                if (ucMemberTweets != null)
                {
                    ucMemberTweets.BindDataIntoRepeater();
                }
                ActivateTab(college66tab, "z6-tab-pane");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "college6tab_Click", ex.Message);
            }
        }

        protected void college7tab_Click(object sender, EventArgs e)
        {
            try
            {
                ucORCIDData ucORCIDData = FindControl("ucORCIDData") as ucORCIDData;
                if (ucORCIDData != null)
                {
                    ucORCIDData.BindDataIntoRepeater(ViewEmail);
                }
                ActivateTab(college77tab, "z7-tab-pane");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "college7tab_Click", ex.Message);
            }
        }

        private void ActivateTab(System.Web.UI.HtmlControls.HtmlButton tab, string paneId)
        {
            tab.Attributes["onclick"] = "return false;";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('{paneId}');", true);
        }
    }
}
