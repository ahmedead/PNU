using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class SEO : UserControl
    {
        public string CanonicalUrl { get; set; }
        public string ArabicUrl { get; set; }
        public string EnglishUrl { get; set; }
        public string DefaultUrl { get; set; }
        public string PageDescription { get; set; }
        public string PageTitle { get; set; }

        // عنوان الصفحة كما كان قبل أن تكتب عليه SEO.ascx، وآخر قيمة كتبتها هي.
        // تُقيَّم الوسوم مرتين (Page_Load ثم PreRenderComplete)، فلو قُرئ Page.Title
        // في المرة الثانية لعاد بما كتبته الأولى وتضاعفت البادئة:
        // "الكليات - الكليات - صفحة عضو هيئة التدريس - جامعة ... - جامعة ...".
        private string _originalPageTitle;
        private string _lastWrittenTitle;

        private static readonly HashSet<string> IgnoredQueryKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "utm_source", "utm_medium", "utm_campaign", "utm_term", "utm_content",
            "fbclid", "gclid", "rev", "lcid", "tag", "ControlMode", "PageView",
            "InitialTabId", "VisibilityContext", "DisplayMode", "_layouts", "IsDlg",
            "guestaccesstoken", "folderid"
        };

        /// <summary>
        /// يُرفع العلم في OnInit لا في Page_Load عن قصد.
        /// دورة حياة ASP.NET تُنهي Init لكل الضوابط قبل أن يبدأ أي Page_Load، فيرى
        /// كل ضابط فرعي - مثل ucAdvertisementDetails - العلم مرفوعاً مهما كان موضعه
        /// في شجرة الضوابط. لو بقي في Page_Load لاعتمد الحارس على ترتيب الوسوم داخل
        /// الماستر بيج: ماستر واحد يضع SEO.ascx بعد PlaceHolderMain يقلب الترتيب
        /// فتُصدَر description و og:* و twitter:* مرتين على كل صفحة.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items["PNU_SEO_Active"] = true;
            }
            if (Page != null)
            {
                Page.PreRenderComplete += Page_PreRenderComplete;
            }
        }

        private static bool HasMeaningfulQueryString(HttpRequest request)
        {
            if (request == null || request.QueryString.Count == 0) return false;
            foreach (string key in request.QueryString.AllKeys)
            {
                // A null key is a bare value (?abc) - treat it as meaningful to stay on the safe side.
                if (key == null || !IgnoredQueryKeys.Contains(key)) return true;
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                // PERF 2026-10-07: only when the URL carries a real query parameter. Calling SetNoServerCaching()
                // unconditionally made EVERY page on the DGA masters uncacheable - the home page went from ~4 ms
                // (output cache) to ~950 ms per request. Detail controls that read ItemId/RequestID/... already
                // call SetNoServerCaching() themselves (QueryString_CacheFix_20260920, 106 files).
                if (HasMeaningfulQueryString(Request))
                {
                    Response.Cache.SetNoServerCaching();
                }
                Response.Cache.VaryByParams["ItemId"] = true;
                Response.Cache.VaryByParams["ProgramCode"] = true;
                Response.Cache.VaryByParams["view"] = true;
                Response.Cache.VaryByParams["RequestId"] = true;
                Response.Cache.VaryByParams["RequestID"] = true;

                // PNU_SEO_Active يُرفع في OnInit أعلاه - لا هنا - حتى لا يعتمد على ترتيب الضوابط.

                // 1. Suppress SharePoint's built-in SeoCanonicalLink to avoid duplicate/inconsistent canonicals
                if (Page != null && Page.Header != null)
                {
                    HideCanonicalControls(Page.Header);
                }

                // 2. Initialize SEO attributes & URLs
                InitializePageTitle();
                InitializePageDescription();
                InitializeHreflangAndCanonicalUrls();
                RenderSeoTags();
            }
            catch (Exception)
            {
                EnsureFallbackUrls();
                RenderSeoTags();
            }
        }

        /// <summary>
        /// إعادة التقييم في PreRenderComplete لا في OnPreRender.
        ///
        /// SEO.ascx في الـ head، وضوابط المحتوى (ucViewFacultyMembers و ucNewsDetails
        /// و ucCollegeNewsDetails ...) تُحمَّل داخل Web Parts في الـ body. الـ Web Part
        /// لا يبني أبناءه إلا عند EnsureChildControls، وASP.NET تستدعيها في PreRender
        /// الخاص بالـ Web Part نفسه — أي بعد OnPreRender الخاص بـ SEO.ascx لأنها أسبق
        /// في الشجرة. فكانت HttpContext.Items["PNU_BrowserTitle"/"PNU_BrowserDescription"]
        /// تُنشَر بعد إصدار الوسوم، ويخرج وصف عام واحد لآلاف الصفحات (Semrush: 1,730 صفحة).
        ///
        /// PreRenderComplete يُطلَق بعد انتهاء PreRender لكل الضوابط في الصفحة.
        /// </summary>
        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                InitializePageTitle();
                InitializePageDescription();
                InitializeHreflangAndCanonicalUrls();
                RenderSeoTags();
            }
            catch (Exception)
            {
                EnsureFallbackUrls();
                RenderSeoTags();
            }
        }

        private void InitializePageDescription()
        {
            try
            {
                HttpRequest request = HttpContext.Current != null ? HttpContext.Current.Request : null;
                string path = request != null && request.Url != null ? request.Url.AbsolutePath.ToLowerInvariant() : string.Empty;
                bool isEnglish = IsCurrentRequestEnglish();
                string desc = string.Empty;

                // Priority 0: Explicit dynamic description set by child user controls (e.g., ucAdvertisementDetails)
                string dynamicDesc = HttpContext.Current != null ? HttpContext.Current.Items["PNU_BrowserDescription"] as string : null;
                if (!string.IsNullOrWhiteSpace(dynamicDesc))
                {
                    desc = CleanPlainText(dynamicDesc);
                }

                // 1. Author-defined Explicit SEO / Comments metadata
                if (string.IsNullOrEmpty(desc) && SPContext.Current != null && SPContext.Current.ListItem != null)
                {
                    SPListItem item = SPContext.Current.ListItem;

                    // Priority 1: SeoMetaDescription field (Page tab -> Edit SEO Properties)
                    if (item.Fields.ContainsField("SeoMetaDescription") && item["SeoMetaDescription"] != null)
                    {
                        string s = item["SeoMetaDescription"].ToString().Trim();
                        if (!string.IsNullOrEmpty(s)) desc = CleanPlainText(s);
                    }

                    // Priority 2: Comments field (Publishing page Description)
                    if (string.IsNullOrEmpty(desc) && item.Fields.ContainsField("Comments") && item["Comments"] != null)
                    {
                        string c = item["Comments"].ToString().Trim();
                        if (!string.IsNullOrEmpty(c)) desc = CleanPlainText(c);
                    }
                }

                // 2. Dynamic Details Pages (QueryString handling to guarantee uniqueness across 1000s of sub-items)
                if (string.IsNullOrEmpty(desc) && request != null)
                {
                    // Case A: Academic Programs (ProgramDetails.aspx) - Fixes 204 duplicate pages in Semrush
                    if (path.IndexOf("programdetails.aspx", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string progCode = request.QueryString["ProgramCode"];
                        string itemId = request.QueryString["ItemId"];
                        string progIdentifier = !string.IsNullOrEmpty(progCode) ? progCode.Trim() : (!string.IsNullOrEmpty(itemId) ? itemId.Trim() : string.Empty);

                        string dynamicTitle = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                        if (string.IsNullOrWhiteSpace(dynamicTitle) && !string.IsNullOrWhiteSpace(GetSourcePageTitle()))
                        {
                            dynamicTitle = GetSourcePageTitle();
                        }

                        string webTitle = SPContext.Current != null && SPContext.Current.Web != null ? SPContext.Current.Web.Title : string.Empty;

                        if (!string.IsNullOrEmpty(progIdentifier))
                        {
                            if (isEnglish)
                            {
                                desc = !string.IsNullOrEmpty(dynamicTitle) && !dynamicTitle.Equals("ProgramDetails", StringComparison.OrdinalIgnoreCase)
                                    ? $"Study plan, admission requirements, and learning outcomes for {dynamicTitle} (Code: {progIdentifier}) at {webTitle}, Princess Nourah bint Abdulrahman University."
                                    : $"Details, study plan, and learning outcomes for academic program ({progIdentifier}) at {webTitle}, Princess Nourah bint Abdulrahman University.";
                            }
                            else
                            {
                                desc = !string.IsNullOrEmpty(dynamicTitle) && !dynamicTitle.Equals("ProgramDetails", StringComparison.OrdinalIgnoreCase)
                                    ? $"تفاصيل وخطة وشروط القبول ومخرجات {dynamicTitle} (رمز البرنامج: {progIdentifier}) في {webTitle} بجامعة الأميرة نورة بنت عبد الرحمن."
                                    : $"تفاصيل وخطة وشروط القبول ومخرجات البرنامج الأكاديمي ({progIdentifier}) في {webTitle} بجامعة الأميرة نورة بنت عبد الرحمن.";
                            }
                        }
                    }
                    // Case B: Faculty Member Profiles (FacultyMemberDetails.aspx)
                    else if (path.IndexOf("facultymemberdetails.aspx", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string memberEmail = request.QueryString["view"];
                        string dynamicTitle = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                        if (string.IsNullOrWhiteSpace(dynamicTitle) && !string.IsNullOrWhiteSpace(GetSourcePageTitle()))
                        {
                            dynamicTitle = GetSourcePageTitle();
                        }

                        string memberLabel = !string.IsNullOrEmpty(dynamicTitle) && !dynamicTitle.Contains("FacultyMemberDetails")
                            ? dynamicTitle
                            : (!string.IsNullOrEmpty(memberEmail) ? memberEmail.Trim() : string.Empty);

                        string webTitle = SPContext.Current != null && SPContext.Current.Web != null ? SPContext.Current.Web.Title : string.Empty;

                        if (!string.IsNullOrEmpty(memberLabel))
                        {
                            desc = isEnglish
                                ? $"Academic profile, curriculum vitae, publications, and teaching details of faculty member ({memberLabel}) at {webTitle}, Princess Nourah bint Abdulrahman University."
                                : $"الصفحة الأكاديمية والسيرة الذاتية والأبحاث العلمية لعضو هيئة التدريس ({memberLabel}) في {webTitle} بجامعة الأميرة نورة بنت عبد الرحمن.";
                        }
                    }
                    // Case C: News, Events, Announcements and Advertisement Details
                    else if (path.IndexOf("newsdetails", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             path.IndexOf("eventdetails", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             path.IndexOf("announcementdetails", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             path.IndexOf("advertisementdetails", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string dynamicTitle = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                        if (string.IsNullOrWhiteSpace(dynamicTitle) && !string.IsNullOrWhiteSpace(GetSourcePageTitle()))
                        {
                            dynamicTitle = GetSourcePageTitle();
                        }

                        string webTitle = SPContext.Current != null && SPContext.Current.Web != null ? SPContext.Current.Web.Title : string.Empty;

                        if (!string.IsNullOrEmpty(dynamicTitle) &&
                            !dynamicTitle.Equals("تفاصيل الإعلان", StringComparison.OrdinalIgnoreCase) &&
                            !dynamicTitle.Equals("AdvertisementDetails", StringComparison.OrdinalIgnoreCase) &&
                            !dynamicTitle.Equals("تفاصيل الاعلان", StringComparison.OrdinalIgnoreCase))
                        {
                            desc = isEnglish
                                ? $"Coverage and details of: {dynamicTitle} - {webTitle}, Princess Nourah bint Abdulrahman University."
                                : $"تغطية وتفاصيل: {dynamicTitle} - {webTitle} - جامعة الأميرة نورة بنت عبد الرحمن.";
                        }
                        else
                        {
                            string reqId = request.QueryString["RequestId"] ?? request.QueryString["RequestID"];
                            if (!string.IsNullOrEmpty(reqId))
                            {
                                desc = isEnglish
                                    ? $"Details of advertisement no. {reqId} at {webTitle}, Princess Nourah bint Abdulrahman University."
                                    : $"تفاصيل الإعلان رقم {reqId} في {webTitle} بجامعة الأميرة نورة بنت عبد الرحمن.";
                            }
                        }
                    }
                }

                // 3. Extract unique sentence from actual page body content (PublishingPageContent)
                if (string.IsNullOrEmpty(desc) && SPContext.Current != null && SPContext.Current.ListItem != null)
                {
                    SPListItem item = SPContext.Current.ListItem;
                    if (item.Fields.ContainsField("PublishingPageContent") && item["PublishingPageContent"] != null)
                    {
                        string bodyContent = item["PublishingPageContent"].ToString();
                        string cleanBody = CleanPlainText(bodyContent);
                        if (!string.IsNullOrEmpty(cleanBody) && cleanBody.Length >= 35)
                        {
                            desc = cleanBody;
                        }
                    }
                }

                // 4. Hierarchical Contextual Fallback (Guarantees every subsite/page is 100% unique)
                if (string.IsNullOrEmpty(desc))
                {
                    string pageTitle = string.Empty;
                    if (SPContext.Current != null && SPContext.Current.ListItem != null && SPContext.Current.ListItem.Fields.ContainsField("Title") && SPContext.Current.ListItem["Title"] != null)
                    {
                        pageTitle = SPContext.Current.ListItem["Title"].ToString().Trim();
                    }
                    if (string.IsNullOrEmpty(pageTitle) && !string.IsNullOrWhiteSpace(GetSourcePageTitle()))
                    {
                        pageTitle = GetSourcePageTitle();
                    }
                    if (string.IsNullOrEmpty(pageTitle) && !string.IsNullOrEmpty(path))
                    {
                        pageTitle = System.IO.Path.GetFileNameWithoutExtension(path);
                    }

                    string webTitle = SPContext.Current != null && SPContext.Current.Web != null ? SPContext.Current.Web.Title : string.Empty;

                    if (!string.IsNullOrEmpty(pageTitle))
                    {
                        if (isEnglish)
                        {
                            desc = (!string.IsNullOrEmpty(webTitle) && !pageTitle.Equals(webTitle, StringComparison.OrdinalIgnoreCase))
                                ? $"{pageTitle} page within {webTitle} services and information at Princess Nourah bint Abdulrahman University."
                                : $"{pageTitle} portal - Official website of Princess Nourah bint Abdulrahman University, providing services, news, and academic information.";
                        }
                        else
                        {
                            desc = (!string.IsNullOrEmpty(webTitle) && !pageTitle.Equals(webTitle, StringComparison.OrdinalIgnoreCase))
                                ? $"صفحة {pageTitle} ضمن خدمات ومعلومات {webTitle} بالبوابة الرسمية لجامعة الأميرة نورة بنت عبد الرحمن."
                                : $"بوابة {pageTitle} - الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن، يقدّم أحدث المعلومات والخدمات والأخبار الأكاديمية.";
                        }
                    }
                }

                // 5. Ultimate Fallback (Home Page only)
                if (string.IsNullOrEmpty(desc))
                {
                    desc = isEnglish
                        ? "Official portal of Princess Nourah bint Abdulrahman University in Saudi Arabia, providing information, e-services, news and announcements."
                        : "الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن في المملكة العربية السعودية، ويقدّم معلومات وخدمات الجامعة وأحدث الأخبار والإعلانات.";
                }

                PageDescription = FormatDescriptionLength(desc);
            }
            catch
            {
                EnsureFallbackUrls();
            }
        }

        private string FormatDescriptionLength(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            string clean = CleanPlainText(text);
            if (clean.Length > 160)
            {
                int lastSpace = clean.LastIndexOf(' ', 156);
                if (lastSpace > 115)
                {
                    return clean.Substring(0, lastSpace) + "...";
                }
                return clean.Substring(0, 157) + "...";
            }
            return clean;
        }

        private bool IsCurrentRequestEnglish()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();

                // اللغة تُحدَّد من مقطع المسار الأول لا بالبحث في المسار كله.
                // كانت Contains("/en/") تُطابق رمز كلية الهندسة في
                // /ar/Faculties/EN/Pages/... فتُعامَل ٣٨ صفحة عربية على أنها
                // إنجليزية: وصف ولقب بقالب إنجليزي وقيم عربية داخله، مثل
                // "بيانات القسم page within كلية الهندسة".
                // StartsWith هنا يطابق ما يفعله InitializeHreflangAndCanonicalUrls
                // أصلاً، فيتوافق الملفان على تعريف واحد للغة.
                return path.StartsWith("/en/", StringComparison.Ordinal)
                    || path.StartsWith("/en-us/", StringComparison.Ordinal)
                    || path.Equals("/en", StringComparison.Ordinal)
                    || path.Equals("/en-us", StringComparison.Ordinal);
            }
            return false;
        }

        /// <summary>
        /// Page.Title كما وضعه شيربوينت أو ضابط محتوى — لا كما كتبته SEO.ascx نفسها.
        /// إن كان العنوان الحالي هو ما كتبناه في التقييم السابق عُدنا إلى الأصلي،
        /// وإن غيّره ضابط آخر بعدنا (مثل ucViewFacultyMembers) أُخذ بقيمته الجديدة.
        /// </summary>
        private string GetSourcePageTitle()
        {
            string current = Page != null && Page.Title != null ? Page.Title.Trim() : string.Empty;

            if (_originalPageTitle == null)
            {
                _originalPageTitle = current;
            }

            if (_lastWrittenTitle != null && string.Equals(current, _lastWrittenTitle, StringComparison.Ordinal))
            {
                return _originalPageTitle;
            }

            return current;
        }

        /// <summary>
        /// عنوان من سلسلة المواقع: {الموقع الأب} - {الموقع الحالي} - {الصفحة الرئيسية}.
        ///
        /// المشكلة المرصودة (Semrush - Duplicate title tags): صفحة أخبار كل كلية
        /// (/{lang}/Faculties/{X}/News/Pages/default.aspx، ضابط ucAllCollegeNews) تحمل
        /// عنوان الصفحة الافتراضي "Home" أو "الرئيسية"، فيتطابق وسم title بين كل الكليات.
        /// المطلوب:
        ///     كلية علوم الحاسب والمعلومات - الأخبار - الصفحة الرئيسية
        ///     College of Computer and Information Sciences - News - Home
        ///
        /// تُفعَّل فقط حين يطلبها ضابط المحتوى عبر HttpContext.Items["PNU_SiteChainTitle"] = true،
        /// فلا يتغيّر عنوان أي صفحة أخرى. يُعاد null إن لم تتوفّر السلسلة، فيسري المنطق المعتاد.
        /// </summary>
        private string BuildSiteChainTitle(bool isEnglish)
        {
            try
            {
                // من جذر اللغة نزولاً: [جميع الكليات، كلية علوم الحاسب والمعلومات، الأخبار]
                List<string> chain = Publics.BuildSeoSiteChain();
                if (chain == null || chain.Count == 0) return null;

                string current = CleanPlainText(chain[chain.Count - 1]);
                string parent = chain.Count > 1 ? CleanPlainText(chain[chain.Count - 2]) : string.Empty;
                string home = isEnglish ? "Home" : "الصفحة الرئيسية";

                return Publics.JoinNonEmpty(" - ", parent, current, home);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// وسم title في الماسترات يعرض PlaceHolderPageTitle وحده، فتعيين Page.Title
        /// لا يغيّره (يبقى "Home"). يُستبدل محتوى الـ placeholder - النهج نفسه في
        /// ucSectionHome و ucViewFacultyMembers.
        /// </summary>
        private void SetDocumentTitle(string title)
        {
            if (Page == null || string.IsNullOrWhiteSpace(title)) return;

            System.Web.UI.MasterPage master = Page.Master;
            while (master != null)
            {
                var ph = master.FindControl("PlaceHolderPageTitle") as System.Web.UI.WebControls.ContentPlaceHolder;
                if (ph != null)
                {
                    ph.Controls.Clear();
                    ph.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(title)));
                    return;
                }
                master = master.Master;
            }
        }

        private string CleanPlainText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            string clean = Regex.Replace(text, "<.*?>", string.Empty);
            clean = HttpUtility.HtmlDecode(clean);
            clean = Regex.Replace(clean, @"\s+", " ").Trim();
            return clean;
        }

        private void InitializePageTitle()
        {
            try
            {
                HttpRequest request = HttpContext.Current != null ? HttpContext.Current.Request : null;
                string path = request != null && request.Url != null ? request.Url.AbsolutePath.ToLowerInvariant() : string.Empty;
                bool isEnglish = IsCurrentRequestEnglish();

                string universityName = isEnglish
                    ? "Princess Nourah bint Abdulrahman University"
                    : "جامعة الأميرة نورة بنت عبد الرحمن";

                string webTitle = SPContext.Current != null && SPContext.Current.Web != null ? SPContext.Current.Web.Title : string.Empty;
                if (!string.IsNullOrEmpty(webTitle))
                {
                    webTitle = CleanPlainText(webTitle);
                }

                // Check if webTitle is the root site name
                bool isRootWeb = string.IsNullOrEmpty(webTitle) ||
                                 webTitle.Equals("جامعة الأميرة نورة بنت عبد الرحمن", StringComparison.OrdinalIgnoreCase) ||
                                 webTitle.Equals("Princess Nourah bint Abdulrahman University", StringComparison.OrdinalIgnoreCase) ||
                                 webTitle.Equals("PNU", StringComparison.OrdinalIgnoreCase);

                string dynamicTitle = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                if (string.IsNullOrWhiteSpace(dynamicTitle) && !string.IsNullOrWhiteSpace(GetSourcePageTitle()))
                {
                    dynamicTitle = GetSourcePageTitle();
                }

                string itemTitle = string.Empty;
                if (SPContext.Current != null && SPContext.Current.ListItem != null && SPContext.Current.ListItem.Fields.ContainsField("Title") && SPContext.Current.ListItem["Title"] != null)
                {
                    itemTitle = CleanPlainText(SPContext.Current.ListItem["Title"].ToString());
                }

                string basePageTitle = !string.IsNullOrWhiteSpace(dynamicTitle) ? dynamicTitle : itemTitle;
                if (string.IsNullOrWhiteSpace(basePageTitle) && !string.IsNullOrEmpty(path))
                {
                    basePageTitle = System.IO.Path.GetFileNameWithoutExtension(path);
                }

                // Prefix with Subsite name as requested: [اسم الموقع الموجود به الصفحة] - ...
                string prefix = (!isRootWeb && !string.IsNullOrEmpty(webTitle)) ? $"{webTitle} - " : string.Empty;

                // Case 0: صفحات رئيسية لمواقع فرعية تحمل عنواناً عاماً (ucAllCollegeNews ...)
                string siteChainTitle = HttpContext.Current.Items["PNU_SiteChainTitle"] is bool
                                        && (bool)HttpContext.Current.Items["PNU_SiteChainTitle"]
                    ? BuildSiteChainTitle(isEnglish)
                    : null;
                if (!string.IsNullOrEmpty(siteChainTitle))
                {
                    PageTitle = $"{siteChainTitle} - {universityName}";
                    SetDocumentTitle(siteChainTitle);
                }
                // Case 1: Academic Programs (ProgramDetails.aspx)
                else if (path.IndexOf("programdetails.aspx", StringComparison.OrdinalIgnoreCase) >= 0 && request != null)
                {
                    string progCode = request.QueryString["ProgramCode"];
                    string codeSuffix = !string.IsNullOrEmpty(progCode) ? (isEnglish ? $" (Code: {progCode.Trim()})" : $" (رمز: {progCode.Trim()})") : string.Empty;
                    string progName = !string.IsNullOrEmpty(basePageTitle) && !basePageTitle.Equals("ProgramDetails", StringComparison.OrdinalIgnoreCase)
                        ? basePageTitle
                        : (isEnglish ? "Academic Program" : "البرنامج الأكاديمي");

                    PageTitle = $"{prefix}{progName}{codeSuffix} - {universityName}";
                }
                // Case 2: Announcements & Advertisements (AdvertisementDetails.aspx / AnnouncementDetails.aspx)
                else if ((path.IndexOf("advertisementdetails", StringComparison.OrdinalIgnoreCase) >= 0 || path.IndexOf("announcementdetails", StringComparison.OrdinalIgnoreCase) >= 0) && request != null)
                {
                    string reqId = request.QueryString["RequestId"] ?? request.QueryString["RequestID"];
                    string adTitle = (!string.IsNullOrEmpty(basePageTitle) &&
                                      !basePageTitle.Equals("تفاصيل الإعلان", StringComparison.OrdinalIgnoreCase) &&
                                      !basePageTitle.Equals("AdvertisementDetails", StringComparison.OrdinalIgnoreCase) &&
                                      !basePageTitle.Equals("تفاصيل الاعلان", StringComparison.OrdinalIgnoreCase))
                        ? basePageTitle + (!string.IsNullOrEmpty(reqId) ? $" - {reqId.Trim()}" : string.Empty) // many ads share a title: the request id keeps <title> unique
                        : (!string.IsNullOrEmpty(reqId) ? (isEnglish ? $"Advertisement #{reqId}" : $"إعلان رقم {reqId}") : (isEnglish ? "Announcement Details" : "تفاصيل الإعلان"));
                    PageTitle = $"{prefix}{adTitle} - {universityName}";
                }
                // Case 3: News & Events
                else if ((path.IndexOf("newsdetails", StringComparison.OrdinalIgnoreCase) >= 0 || path.IndexOf("eventdetails", StringComparison.OrdinalIgnoreCase) >= 0) && request != null)
                {
                    string reqId = request.QueryString["RequestId"] ?? request.QueryString["RequestID"] ?? request.QueryString["ItemId"] ?? request.QueryString["ID"];
                    string itemLabel = (!string.IsNullOrEmpty(basePageTitle) &&
                                        !basePageTitle.Equals("تفاصيل الخبر", StringComparison.OrdinalIgnoreCase) &&
                                        !basePageTitle.Equals("تفاصيل الفعالية", StringComparison.OrdinalIgnoreCase) &&
                                        !basePageTitle.Equals("NewsDetails", StringComparison.OrdinalIgnoreCase) &&
                                        !basePageTitle.Equals("EventDetails", StringComparison.OrdinalIgnoreCase))
                        ? basePageTitle + (!string.IsNullOrEmpty(reqId) ? $" - {reqId.Trim()}" : string.Empty) // like ads: the request id keeps <title> unique
                        : (!string.IsNullOrEmpty(reqId) ? (isEnglish ? $"Details #{reqId}" : $"تفاصيل رقم {reqId}") : (isEnglish ? "Details" : "التفاصيل"));
                    PageTitle = $"{prefix}{itemLabel} - {universityName}";

                    // ضوابط الأخبار (ucNewsDetails و ucCollegeNewsDetails) تكتب العنوان المجرّد في
                    // PlaceHolderPageTitle، فيظهر في وسم title دون رقم الطلب رغم تعيين Page.Title.
                    // Semrush (2026-10-12): ~٤٥٥ عنوان خبر مكرَّر - الخبر نفسه منشور برقمين
                    // (1104 و 1105) أو منسوخ من أخبار الكلية إلى المركز الإعلامي.
                    // SEO.ascx تعمل في PreRenderComplete بعد تلك الضوابط، فيغلب ما تكتبه هنا.
                    SetDocumentTitle(PageTitle);
                }
                // Case 4: Faculty Member Details
                else if (path.IndexOf("facultymemberdetails", StringComparison.OrdinalIgnoreCase) >= 0 && request != null)
                {
                    string facultyName = !string.IsNullOrEmpty(basePageTitle) && !basePageTitle.Contains("FacultyMemberDetails")
                        ? basePageTitle
                        : request.QueryString["view"];
                    PageTitle = $"{prefix}{facultyName} - {universityName}";
                }
                // Case 5: General Publishing Page
                else
                {
                    if (!string.IsNullOrEmpty(basePageTitle))
                    {
                        if (!isRootWeb && !string.IsNullOrEmpty(webTitle) && !basePageTitle.Equals(webTitle, StringComparison.OrdinalIgnoreCase))
                            PageTitle = $"{webTitle} - {basePageTitle} - {universityName}";
                        else
                            PageTitle = $"{basePageTitle} - {universityName}";
                    }
                    else
                    {
                        PageTitle = !isRootWeb && !string.IsNullOrEmpty(webTitle)
                            ? $"{webTitle} - {universityName}"
                            : universityName;
                    }
                }

                // Update Page.Title so the HTML <title> tag renders with the unique title
                if (Page != null && !string.IsNullOrWhiteSpace(PageTitle))
                {
                    Page.Title = PageTitle;
                    _lastWrittenTitle = PageTitle.Trim();
                }
            }
            catch
            {
                PageTitle = IsCurrentRequestEnglish()
                    ? "Princess Nourah bint Abdulrahman University"
                    : "جامعة الأميرة نورة بنت عبد الرحمن";
            }
        }

        private void InitializeHreflangAndCanonicalUrls()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.Url == null)
            {
                EnsureFallbackUrls();
                return;
            }

            HttpRequest request = HttpContext.Current.Request;
            Uri requestUri = request.Url;

            // 1. Determine Standard Canonical Base Authority (HTTPS + Official Portal Domain)
            string standardAuthority = GetStandardPortalAuthority(requestUri);

            // 2. Clean Absolute Path, remove duplicate slashes, and normalize language prefix
            //
            // المسار يُؤخذ من الملف كما خزّنه شيربوينت لا كما طلبه الزائر.
            //
            // شيربوينت يخدم أي حالة أحرف بـ 200، وكان الـ canonical يردّ الحالة
            // المطلوبة كما هي، فكل صيغة تُعلن نفسها أصلاً:
            //     /ar/Pages/Home.aspx  ->  canonical = /ar/Pages/Home.aspx
            //     /ar/pages/home.aspx  ->  canonical = /ar/pages/home.aspx
            // فتُفهرَس الصفحة مرتين بمحتوى واحد. رصد Semrush ذلك على Home.aspx
            // و Speech.aspx و PNUinNumbers.aspx.
            //
            // ServerRelativeUrl يحمل الحالة المخزَّنة الواحدة، فتنهار كل الصيغ إلى
            // canonical واحد. وهذا لا يعيد عطل ToLowerInvariant القديم: الحالة
            // المستعملة هي حالة شيربوينت الأصلية لا حالة مفروضة.
            string cleanPath = requestUri.AbsolutePath;
            try
            {
                if (SPContext.Current != null &&
                    SPContext.Current.File != null &&
                    !string.IsNullOrEmpty(SPContext.Current.File.ServerRelativeUrl))
                {
                    cleanPath = SPContext.Current.File.ServerRelativeUrl;
                }
            }
            catch
            {
                // لا سياق ملف (صفحة تطبيق أو خطأ): يبقى مسار الطلب كما هو.
            }

            if (string.IsNullOrEmpty(cleanPath) || cleanPath.Equals("/", StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = "/ar/Pages/home.aspx";
            }
            else
            {
                // Remove consecutive duplicate slashes (e.g. /ar//MediaCenter/ -> /ar/MediaCenter/)
                cleanPath = Regex.Replace(cleanPath, @"(?<!:)/{2,}", "/");

                // Normalize language prefix to lowercase (/AR/ -> /ar/, /EN/ -> /en/)
                // while strictly preserving authentic casing for the rest of the path to match crawled URLs
                if (cleanPath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
                {
                    cleanPath = "/ar/" + cleanPath.Substring(4);
                }
                else if (cleanPath.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                {
                    cleanPath = "/en/" + cleanPath.Substring(4);
                }
                else if (cleanPath.StartsWith("/ar-sa/", StringComparison.OrdinalIgnoreCase))
                {
                    cleanPath = "/ar-sa/" + cleanPath.Substring(7);
                }
                else if (cleanPath.StartsWith("/en-us/", StringComparison.OrdinalIgnoreCase))
                {
                    cleanPath = "/en-us/" + cleanPath.Substring(7);
                }
            }

            // 3. Build sanitized query string preserving content-identifying parameters
            string queryString = BuildSanitizedQueryString(request.QueryString);

            // 4. Set Unified Canonical URL matching the exact page URL
            CanonicalUrl = standardAuthority + cleanPath + queryString;

            string lowerPath = cleanPath.ToLowerInvariant();

            // 5. Generate Alternate Language & Default URLs (anchored to root language prefix)
            if (lowerPath.StartsWith("/ar-sa/"))
            {
                ArabicUrl = standardAuthority + cleanPath + queryString;
                EnglishUrl = standardAuthority + Regex.Replace(cleanPath, "^/ar-sa/", "/en-us/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.StartsWith("/en-us/"))
            {
                EnglishUrl = standardAuthority + cleanPath + queryString;
                ArabicUrl = standardAuthority + Regex.Replace(cleanPath, "^/en-us/", "/ar-sa/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.StartsWith("/ar/"))
            {
                ArabicUrl = standardAuthority + cleanPath + queryString;
                EnglishUrl = standardAuthority + Regex.Replace(cleanPath, "^/ar/", "/en/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.StartsWith("/en/"))
            {
                EnglishUrl = standardAuthority + cleanPath + queryString;
                ArabicUrl = standardAuthority + Regex.Replace(cleanPath, "^/en/", "/ar/", RegexOptions.IgnoreCase) + queryString;
            }
            else
            {
                // مسار بلا بادئة لغة معروفة: لا يوجد مقابل لغوي يمكن إثباته.
                // توجيه ar و en و x-default إلى الصفحة الرئيسية يترك الـ canonical
                // خارج العنقود وبلا وسم مرجعي ذاتي - وهو بعينه صنف الخطأ الذي
                // عالجناه في ملاحظات Semrush. الصواب: canonical وحده بلا hreflang.
                ArabicUrl = null;
                EnglishUrl = null;
            }

            DefaultUrl = ArabicUrl ?? CanonicalUrl;
        }

        /// <summary>
        /// هل النسخة المقابلة بالّلغة الأخرى موجودة فعلاً؟
        /// وسم hreflang يشير إلى صفحة 404 خطأ مستقل في Semrush ويُفقد المجموعة تبادليتها،
        /// وقائمة sitemap_broken_404_urls.csv تثبت أن مئات مسارات /en/ غير موجودة.
        /// النتيجة مخزَّنة في الكاش لأن الفحص يمس قاعدة بيانات المحتوى.
        /// السلوك عند الفشل: افتراض الوجود (fail-open) - حذف وسم صحيح أسوأ من إبقاء وسم معطوب.
        /// </summary>
        private bool AlternateUrlExists(string absoluteUrl)
        {
            if (string.IsNullOrEmpty(absoluteUrl)) return false;
            try
            {
                return CounterpartIsPublic("file", Uri.UnescapeDataString(new Uri(absoluteUrl).AbsolutePath));
            }
            catch
            {
                return true; // fail-open
            }
        }

        /// <summary>
        /// هل الملف (kind = "file") أو الموقع (kind = "web") موجود ومنشور، أي يراه الزائر المجهول؟
        /// تستعمله SEO.ascx لوسوم hreflang و ucHomeHeader لزر تبديل اللغة.
        ///
        /// لماذا مرفوع الصلاحية (RunWithElevatedPrivileges):
        /// كان الفحص يجري بهوية الزائر. حين يكون المقابل محمياً أو غير منشور - مثل
        /// /en/Departments/DM/Pages/DataLeakReport.aspx - يلمس الزائر المجهول كائناً لا يحق
        /// له رؤيته، فيعلّم شيربوينت الاستجابة كلها 401 (WWW-Authenticate: NTLM) حتى لو
        /// التُقط الاستثناء. فتردّ الصفحة العربية نفسها 401.
        /// ولأن النتيجة مخزَّنة ٦ ساعات بمفتاح لا يتضمن المستخدم، كانت زيارة واحدة بتسجيل
        /// دخول تملأ الكاش فتعمل الصفحة للمجهول، ثم يعود العطل حين ينتهي الكاش - وهو
        /// بالضبط السلوك المرصود على DataLeakReport.aspx.
        ///
        /// لماذا "منشور" لا "موجود" فقط: الفحص المرفوع يرى المسودات، والصفحة التي لا نسخة
        /// منشورة لها تردّ 401/404 للمجهول؛ الربط إليها بـ hreflang أو بزر اللغة رابط معطوب.
        /// MajorVersion >= 1 يعني وجود نسخة رئيسية منشورة يراها الزوّار حتى لو فوقها مسودة.
        ///
        /// النتيجة واحدة لكل المستخدمين فتُخزَّن ٦ ساعات بأمان. عند الفشل: افتراض الوجود.
        /// </summary>
        internal static bool CounterpartIsPublic(string kind, string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl)) return false;

            string cacheKey = "PNU_Counterpart::" + kind + "::" + serverRelativeUrl.ToLowerInvariant();
            object cached = HttpRuntime.Cache[cacheKey];
            if (cached is bool) return (bool)cached;

            bool isPublic = true; // fail-open
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    bool result = true;

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteId))
                        {
                            if (kind == "web")
                            {
                                // OpenWeb(url) بلا requireExactUrl يعيد Exists=false للمسار المفقود بدل الاستثناء
                                using (SPWeb w = site.OpenWeb(serverRelativeUrl.TrimEnd('/')))
                                {
                                    result = w != null && w.Exists;
                                }
                            }
                            else
                            {
                                SPFile file = site.RootWeb.GetFile(serverRelativeUrl);
                                result = file != null && file.Exists &&
                                         (file.Level == SPFileLevel.Published || file.MajorVersion >= 1);
                            }
                        }
                    });

                    isPublic = result;
                }
            }
            catch
            {
                isPublic = true;
            }

            HttpRuntime.Cache.Insert(
                cacheKey, isPublic, null,
                DateTime.UtcNow.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration);

            return isPublic;
        }

        private string BuildSanitizedQueryString(NameValueCollection queryParams)
        {
            if (queryParams == null || queryParams.Count == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (string key in queryParams.AllKeys)
            {
                if (string.IsNullOrEmpty(key) || IgnoredQueryKeys.Contains(key))
                    continue;

                string val = queryParams[key];
                if (sb.Length > 0) sb.Append("&");
                else sb.Append("?");

                sb.Append(EncodeQueryComponent(key));
                if (val != null)
                {
                    sb.Append("=").Append(EncodeQueryComponent(val));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ترميز قيمة استعلام مع إبقاء "@" حرفياً.
        ///
        /// المشكلة المرصودة (Semrush 2026-10-12 - Incorrect pages found in sitemap.xml، ٣١٠ صفحة):
        /// HttpUtility.UrlEncode يرمّز "@" إلى %40، فيصير canonical صفحة عضو هيئة التدريس
        ///     FacultyMemberDetails.aspx?view=nmmohamed%40pnu.edu.sa
        /// بينما sitemap.xml وكل الروابط الداخلية (ucSectionMembers و ucSearchFacultyMembers)
        /// تستعمل ?view=nmmohamed@pnu.edu.sa - فيرى Semrush كل عنوان في الخريطة "غير قانوني".
        ///
        /// "@" مسموح حرفياً داخل الاستعلام (RFC 3986، query = *( pchar / "/" / "?" )
        /// و pchar يشمل "@")، فإبقاؤه يطابق الصيغة المنشورة دون أي التباس.
        /// يجب أن يبقى مطابقاً لـ cleanCanonicalUrl في ucViewFacultyMembers (JSON-LD url).
        /// </summary>
        internal static string EncodeQueryComponent(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return HttpUtility.UrlEncode(value).Replace("%40", "@");
        }

        private string GetStandardPortalAuthority(Uri requestUri)
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null && !string.IsNullOrEmpty(SPContext.Current.Site.Url))
                {
                    Uri spSiteUri = new Uri(SPContext.Current.Site.Url);
                    string scheme = "https";
                    return $"{scheme}://{spSiteUri.Authority}".TrimEnd('/');
                }
            }
            catch { }

            string host = requestUri.Authority;
            return $"https://{host}".TrimEnd('/');
        }

        private void HideCanonicalControls(Control root)
        {
            if (root == null) return;
            if (root.GetType().Name.IndexOf("Canonical", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                root.Visible = false;
                return;
            }
            foreach (Control child in root.Controls)
            {
                HideCanonicalControls(child);
            }
        }

        private void EnsureFallbackUrls()
        {
            string baseUrl = "https://pnu.edu.sa";
            CanonicalUrl = baseUrl + "/ar/pages/home.aspx";
            ArabicUrl = baseUrl + "/ar/pages/home.aspx";
            EnglishUrl = baseUrl + "/en/pages/home.aspx";
            DefaultUrl = ArabicUrl;
            PageTitle = "جامعة الأميرة نورة بنت عبد الرحمن";
            PageDescription = "الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن في المملكة العربية السعودية، ويقدّم معلومات وخدمات الجامعة وأحدث الأخبار والإعلانات.";
        }

        private void RenderSeoTags()
        {
            if (litSeoTags != null)
            {
                string encodedDesc = HttpUtility.HtmlAttributeEncode(PageDescription);
                string encodedTitle = HttpUtility.HtmlAttributeEncode(PageTitle);
                string ogImageTag = string.Empty;
                string dynamicImage = HttpContext.Current != null ? HttpContext.Current.Items["PNU_OgImage"] as string : null;
                if (!string.IsNullOrEmpty(dynamicImage))
                {
                    string encodedImg = HttpUtility.HtmlAttributeEncode(dynamicImage);
                    ogImageTag = string.Format("\r\n<meta property=\"og:image\" content=\"{0}\" />\r\n<meta name=\"twitter:image\" content=\"{0}\" />", encodedImg);
                }

                litSeoTags.Text = string.Format(
                    "<!-- Security Headers -->\r\n" +
                    "<meta http-equiv=\"X-Content-Type-Options\" content=\"nosniff\" />\r\n" +
                    "<!-- Dynamic Meta Description -->\r\n" +
                    "<meta name=\"description\" content=\"{1}\" />\r\n" +
                    "<meta property=\"og:description\" content=\"{1}\" />\r\n" +
                    "<meta name=\"twitter:description\" content=\"{1}\" />\r\n" +
                    "<!-- Dynamic Title, OpenGraph & Twitter -->\r\n" +
                    "<meta property=\"og:title\" content=\"{2}\" />\r\n" +
                    "<meta name=\"twitter:title\" content=\"{2}\" />\r\n" +
                    "<meta property=\"og:url\" content=\"{0}\" />\r\n" +
                    "{4}" +
                    "<!-- Canonical Tag (Self-Referencing Official URL) -->\r\n" +
                    "<link rel=\"canonical\" href=\"{0}\" />\r\n" +
                    "{3}",
                    CanonicalUrl,
                    encodedDesc,
                    encodedTitle,
                    BuildHreflangBlock(),
                    ogImageTag
                );
            }
        }

        /// <summary>
        /// يبني وسوم hreflang مع ضمانتين:
        /// 1. الوسم المرجعي الذاتي مطابق حرفياً للـ canonical (نفس السلسلة النصية).
        /// 2. لا يُصدَر وسم للّغة الأخرى إلا إذا كانت صفحتها موجودة فعلاً.
        /// </summary>
        private string BuildHreflangBlock()
        {
            // لا عنقود لغوي بلا طرفين. المسار الذي لا يحمل بادئة لغة معروفة يُكتفى فيه
            // بالـ canonical وحده؛ وسم x-default وحيداً بلا ar ولا en عنقود ناقص لا صحيح.
            if (string.IsNullOrEmpty(ArabicUrl) && string.IsNullOrEmpty(EnglishUrl))
            {
                return string.Empty;
            }

            bool isEnglishPage = !string.IsNullOrEmpty(EnglishUrl) &&
                                 string.Equals(EnglishUrl, CanonicalUrl, StringComparison.Ordinal);

            // الصفحة الحالية موجودة بداهةً؛ نفحص المقابلة فقط.
            bool arabicOk = isEnglishPage ? AlternateUrlExists(ArabicUrl) : true;
            bool englishOk = isEnglishPage ? true : AlternateUrlExists(EnglishUrl);

            StringBuilder sb = new StringBuilder();
            sb.Append("<!-- Hreflang (ar, en, x-default) - قيمة لغة واحدة لكل رابط -->\r\n");

            if (arabicOk && !string.IsNullOrEmpty(ArabicUrl))
                sb.AppendFormat("<link rel=\"alternate\" hreflang=\"ar\" href=\"{0}\" />\r\n", ArabicUrl);

            if (englishOk && !string.IsNullOrEmpty(EnglishUrl))
                sb.AppendFormat("<link rel=\"alternate\" hreflang=\"en\" href=\"{0}\" />\r\n", EnglishUrl);

            // x-default يشير للعربية إن وُجدت، وإلا للصفحة نفسها - ولا يُترك معلّقاً على رابط 404.
            string xDefault = (arabicOk && !string.IsNullOrEmpty(ArabicUrl)) ? ArabicUrl : CanonicalUrl;
            sb.AppendFormat("<link rel=\"alternate\" hreflang=\"x-default\" href=\"{0}\" />\r\n", xDefault);

            return sb.ToString();
        }
    }
}
