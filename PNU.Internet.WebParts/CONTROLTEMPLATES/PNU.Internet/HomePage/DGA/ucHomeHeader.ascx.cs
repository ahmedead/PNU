using System.IdentityModel.Services;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeHeader : UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            FixCanonicalLink();
            // Meta description is centrally managed by SEO.ascx to avoid duplicate <meta name="description"> tags
            // InjectMetaDescription();
        }
        private void InjectMetaDescription()
        {
            try
            {
                if (Page.Header == null) return;

                string desc = GetPageDescription();
                if (string.IsNullOrWhiteSpace(desc)) return;

                Page.Header.Controls.Add(new LiteralControl(string.Format(
                    "<meta name=\"description\" content=\"{0}\" />",
                    HttpUtility.HtmlEncode(desc))));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.InjectMetaDescription", ex.Message);
            }
        }

        private string GetPageDescription()
        {
            try
            {
                var item = SPContext.Current.ListItem;
                if (item != null)
                {
                    // 1. SEO Properties field (Page tab → Edit SEO Properties → Meta Description)
                    string seo = GetFieldValue(item, "SeoMetaDescription");
                    if (!string.IsNullOrWhiteSpace(seo))
                        return Publics.TruncateText(seo, 160);

                    // 2. Publishing page Description field
                    string comments = GetFieldValue(item, "Comments");
                    if (!string.IsNullOrWhiteSpace(comments))
                        return Publics.TruncateText(comments, 160);

                    //// 3. Fallback: page title + site-wide sentence
                    //string title = GetFieldValue(item, "Title");
                    //if (!string.IsNullOrWhiteSpace(title))
                    //    return Publics.TruncateText(
                    //        title + " - " + GetDefaultDescription(), 160);

                    // 3. Fallback: current browser title + site-wide sentence
                    string title = GetCurrentBrowserTitle();
                    if (!string.IsNullOrWhiteSpace(title))
                        return Publics.TruncateText(title + " - " + GetDefaultDescription(), 160);
                }

                // 4. Non-publishing pages: site-wide default
                return GetDefaultDescription();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.GetPageDescription", ex.Message);
                return string.Empty;
            }
        }

        private string GetFieldValue(SPListItem item, string internalName)
        {
            return item.Fields.ContainsField(internalName) && item[internalName] != null
                ? item[internalName].ToString().Trim()
                : string.Empty;
        }

        private string GetDefaultDescription()
        {
            // the current site-wide text from the master, AR/EN aware
            return SPContext.Current.Web.Locale.LCID == 1025
                ? "الموقع الرسمي لجامعة الأميرة نورة بنت عبد الرحمن في المملكة العربية السعودية، ويقدّم معلومات وخدمات الجامعة وأحدث الأخبار والإعلانات."
                : "The official website of Princess Nourah bint Abdulrahman University in Saudi Arabia, providing university information, services, news and announcements.";
        }

        private string GetCurrentBrowserTitle()
        {
            try
            {
                // 1. Dynamic title set by details controls this request
                var dynamic = HttpContext.Current.Items["PNU_BrowserTitle"] as string;
                if (!string.IsNullOrWhiteSpace(dynamic)) return dynamic;

                // 2. Anything set via Page.Title
                if (!string.IsNullOrWhiteSpace(Page.Title)) return Page.Title;

                // 3. Static publishing page title
                var item = SPContext.Current.ListItem;
                if (item != null) return GetFieldValue(item, "Title");

                return string.Empty;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.GetCurrentBrowserTitle", ex.Message);
                return string.Empty;
            }
        }

        private static bool DetectArabic()
        {
            try
            {
                int lcid = Thread.CurrentThread.CurrentUICulture.LCID;
                if (lcid == 1025) return true;
                if (lcid == 1033) return false;
            }
            catch { }
            return true;
        }
        protected bool IsArabic { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = DetectArabic();
            try
            {
                if (!Page.IsPostBack)
                {
                    ArabicLink.HRef = BuildLanguageSwitchUrl("ar");
                    EnglishLink.HRef = BuildLanguageSwitchUrl("en");


                    img2.Attributes["src"] = SPFactory.GetPNUresResource("LogoOnly");
                    aLinkHome.Attributes.Add("href", SPFactory.GetSiteURL());

                    LoadMenu();
                }

                Publics.AddVisitorsCount(HttpContext.Current.Request.Url.ToString(), this.Page.Title);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void LoadMenu()
        {
            try
            {
                string MenuLevel1 = "TopMenuLevel1";
                string MenuLevel2 = "TopMenuLevel2";
                bool rootMenuOnly = GetLoadRootMenuOnly();
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList mainLlist = null;
                        string Parentweb = web.Url + "/";
                        if (Parentweb.Contains(site.Url))
                        {
                            Parentweb = Parentweb.Replace(site.Url, "");
                        }

                        // LoadRootMenuOnly = True: القائمة العليا تُقرأ دائماً من جذر اللغة (/ar/ أو /en/)
                        // بدل البحث صعوداً من الموقع الحالي عن أقرب موقع يملك TopMenuLevel1
                        if (rootMenuOnly)
                        {
                            Parentweb = IsArabic ? "/ar/" : "/en/";
                        }

                        while (mainLlist == null)
                        {
                            try
                            {
                                using (SPWeb Pweb = site.OpenWeb(Parentweb))
                                {
                                    mainLlist = Pweb.Lists.TryGetList(MenuLevel1);
                                    if (mainLlist != null)
                                    {
                                        SPQuery query = new SPQuery()
                                        {
                                            Query = $@"<Where><Eq><FieldRef Name='Visibility' /><Value Type='Boolean'>1</Value></Eq></Where>
                                                      <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>"
                                        };
                                        SPListItemCollection sPListItemCollection = mainLlist.GetItems(query);
                                        List<TopMenuLevel1> _Level1 = new List<TopMenuLevel1>();
                                        List<TopMenuLevel2> _Level2 = new List<TopMenuLevel2>();
                                        List<TopMenuLevel3> _Level3 = new List<TopMenuLevel3>();
                                        _Level1 = SPFactory.MapListItemsToClass<TopMenuLevel1>(sPListItemCollection);

                                        for (int i = 0; i < _Level1.Count; i++)
                                        {
                                            _Level1[i].LVL2 = new List<TopMenuLevel2>();
                                            query = new SPQuery()
                                            {
                                                Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level1[i].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                            };
                                            SPList Sublist = Pweb.Lists.TryGetList(MenuLevel2);
                                            if (Sublist == null) continue;

                                            SPListItemCollection sPListItemCollectionLVL2 = Sublist.GetItems(query);
                                            _Level2 = SPFactory.MapListItemsToClass<TopMenuLevel2>(sPListItemCollectionLVL2);
                                            _Level1[i].LVL2 = new List<TopMenuLevel2>();

                                            if (_Level2 != null && _Level2.Count > 0)
                                            {
                                                _Level1[i].LVL2 = _Level2;

                                                for (int j = 0; j < _Level2.Count; j++)
                                                {
                                                    SPQuery Subquery = new SPQuery()
                                                    {
                                                        Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level2[j].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                                    };
                                                    string MenuLevel3 = "TopMenuLevel3";
                                                    SPList SubSublist = Pweb.Lists.TryGetList(MenuLevel3);
                                                    if (SubSublist != null)
                                                    {
                                                        SPListItemCollection sPListItemCollectionLVL3 = SubSublist.GetItems(Subquery);
                                                        _Level3 = SPFactory.MapListItemsToClass<TopMenuLevel3>(sPListItemCollectionLVL3);
                                                        _Level2[j].LVL3 = new List<TopMenuLevel3>();
                                                        _Level2[j].LVL3 = _Level3;
                                                    }
                                                }
                                            }

                                            _Level1[i].LVL2 = _Level2;
                                        }

                                        // Build flattened model (same pattern as old ucHeader)
                                        List<MenuLevel1> menuList = new List<MenuLevel1>();
                                        foreach (var level1 in _Level1)
                                        {
                                            MenuLevel1 menu1 = new MenuLevel1
                                            {
                                                Title = level1.Title,
                                                URL = RootMenuUrl(level1.URL, Pweb),
                                                LVL2 = new List<MenuLevel2>(),
                                                LVL3 = new List<MenuLevel3>()
                                            };
                                            if (level1.LVL2 != null && level1.LVL2.Count > 0)
                                            {
                                                foreach (var level2 in level1.LVL2)
                                                {
                                                    MenuLevel2 menu2 = new MenuLevel2
                                                    {
                                                        Title = level2.Title,
                                                        URL = RootMenuUrl(level2.URL, Pweb)
                                                    };
                                                    menu1.LVL2.Add(menu2);

                                                    if (level2.LVL3 != null && level2.LVL3.Count > 0)
                                                    {
                                                        foreach (var level3 in level2.LVL3)
                                                        {
                                                            MenuLevel3 menu3 = new MenuLevel3
                                                            {
                                                                Title = level3.Title,
                                                                URL = RootMenuUrl(level3.URL, Pweb),
                                                                ParentTitle = level2.Title
                                                            };
                                                            menu1.LVL3.Add(menu3);
                                                        }
                                                    }
                                                }
                                            }
                                            menuList.Add(menu1);
                                        }

                                        // القائمة الجوّالة تُبنى من _Level1 مباشرةً لا من menuList، فلم تمرّ
                                        // روابطها على RootMenuUrl: بقي "ProgramDetails.aspx?ProgramCode=AD-BA-PRDE"
                                        // نسبياً في نسخة الجوّال وحدها، فيُحَل من /AD/Announcements/Pages/ إلى 404
                                        // (Semrush 2026-10-12: ١٣٠ رابطاً معطوباً). التجذير هنا يوحّد النسختين.
                                        foreach (var l1 in _Level1)
                                        {
                                            l1.URL = RootMenuUrl(l1.URL, Pweb);
                                            if (l1.LVL2 == null) continue;
                                            foreach (var l2 in l1.LVL2)
                                            {
                                                l2.URL = RootMenuUrl(l2.URL, Pweb);
                                                if (l2.LVL3 == null) continue;
                                                foreach (var l3 in l2.LVL3)
                                                {
                                                    l3.URL = RootMenuUrl(l3.URL, Pweb);
                                                }
                                            }
                                        }

                                        // Render desktop + mobile using the new DGA design
                                        Get_MenuHTML_NewDesign(menuList);
                                        Get_MenuHTML(_Level1);
                                    }

                                    // في وضع الجذر فقط لا يُصعد إلى الموقع الأب إن لم توجد القائمة
                                    if (rootMenuOnly) break;

                                    Parentweb = Pweb.ParentWeb.Url + "/";
                                    if (Parentweb.Contains(site.Url))
                                    {
                                        Parentweb = Parentweb.Replace(site.Url, "");
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }

                        if (mainLlist == null)
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private const string LoadRootMenuOnlyKey = "LoadRootMenuOnly";
        private const string LoadRootMenuOnlyCacheKey = "PNU_Settings_LoadRootMenuOnly";
        private static readonly object LoadRootMenuOnlyLock = new object();

        /// <summary>
        /// يقرأ المفتاح LoadRootMenuOnly من قائمة Settings في /Admin/.
        /// إن لم يوجد العنصر يُضاف بالقيمة الافتراضية False. عند أي خطأ يُعاد false
        /// فيبقى سلوك القائمة كما هو.
        ///
        /// القيمة مخزَّنة دقيقتين، والإضافة داخل قفل مع إعادة الفحص: بدونهما كانت
        /// الطلبات المتزامنة كلها ترى القائمة خالية من المفتاح فتضيف كلٌّ منها عنصراً مكرراً.
        /// </summary>
        private static bool GetLoadRootMenuOnly()
        {
            object cached = HttpRuntime.Cache[LoadRootMenuOnlyCacheKey];
            if (cached is bool) return (bool)cached;

            lock (LoadRootMenuOnlyLock)
            {
                cached = HttpRuntime.Cache[LoadRootMenuOnlyCacheKey];
                if (cached is bool) return (bool)cached;

                bool result = false;
                try
                {
                    string siteURL = string.Format("{0}{1}", SPContext.Current.Site.Url, "/admin");
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(siteURL))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = web.GetList($"{siteURL}/Lists/Settings");
                                if (list == null) return;

                                SPQuery query = new SPQuery
                                {
                                    Query = $"<Where><Eq><FieldRef Name='Key' /><Value Type='Text'>{LoadRootMenuOnlyKey}</Value></Eq></Where>" +
                                            "<OrderBy><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>",
                                    RowLimit = 1
                                };
                                SPListItemCollection items = list.GetItems(query);
                                if (items.Count > 0)
                                {
                                    string value = Convert.ToString(items[0]["Value"]).Trim();
                                    result = value.Equals("True", StringComparison.OrdinalIgnoreCase);
                                    return;
                                }

                                bool allowUnsafe = web.AllowUnsafeUpdates;
                                try
                                {
                                    web.AllowUnsafeUpdates = true;
                                    SPListItem newItem = list.Items.Add();
                                    if (list.Fields.ContainsField("Title")) newItem["Title"] = LoadRootMenuOnlyKey;
                                    newItem["Key"] = LoadRootMenuOnlyKey;
                                    newItem["Value"] = "False";
                                    newItem.Update();
                                }
                                finally
                                {
                                    web.AllowUnsafeUpdates = allowUnsafe;
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeHeader.GetLoadRootMenuOnly", ex.Message);
                }

                HttpRuntime.Cache.Insert(LoadRootMenuOnlyCacheKey, result, null,
                    DateTime.UtcNow.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration);
                return result;
            }
        }

        /// <summary>
        /// يجعل رابط عنصر القائمة مُجذَّراً إن كان مخزَّناً نسبياً.
        ///
        /// المشكلة المرصودة (Semrush 2026-09-29، ٢٢٥ رابطاً معطوباً):
        /// بعض عناصر قوائم TopMenuLevel* مخزَّنة بروابط نسبية مثل
        /// "Sections.aspx?SecCode=AP-01". الرابط النسبي يُحَل حسب مجلد الصفحة
        /// المعروضة لا حسب الموقع صاحب القائمة، فينتج:
        ///
        ///   من /ar/Faculties/AP/Pages/               -> 200  صحيح
        ///   من /ar/Faculties/AP/Announcements/Pages/ -> 404
        ///   من /ar/Faculties/AP/News/Pages/          -> 404
        ///
        /// والقائمة تُقرأ من Pweb — الموقع الأب الذي يملك القائمة — فهو المرجع
        /// الصحيح للتجذير، لا الصفحة الحالية.
        ///
        /// اسم صفحة مجرَّد بلا شرطة مائلة يعني <الموقع>/Pages/<الاسم>: هي الصيغة
        /// التي يكتبها SideMenuListProvisioner أصلاً (webPath + "Pages/" + ...).
        ///
        /// ملاحظة: الإصلاح الجذري هو تصحيح البيانات المخزَّنة في القوائم. هذا
        /// تحصين وقت العرض يعالج العناصر القديمة ويمنع تكرار العطل مع أي عنصر
        /// يُدخَل نسبياً مستقبلاً.
        /// </summary>
        private static string RootMenuUrl(string url, SPWeb menuWeb)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;

            string u = url.Trim();

            // مُجذَّر أو مطلق أو مرساة أو بروتوكول خاص: يُترك كما هو
            if (u.StartsWith("/")
                || u.StartsWith("#")
                || u.IndexOf("://", StringComparison.Ordinal) >= 0
                || u.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
                || u.StartsWith("tel:", StringComparison.OrdinalIgnoreCase)
                || u.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
            {
                return u;
            }

            string webUrl = "/";
            try
            {
                if (menuWeb != null && !string.IsNullOrEmpty(menuWeb.ServerRelativeUrl))
                {
                    webUrl = menuWeb.ServerRelativeUrl;
                }
            }
            catch { }

            webUrl = webUrl.TrimEnd('/');

            return u.IndexOf('/') < 0
                ? webUrl + "/Pages/" + u
                : webUrl + "/" + u;
        }

        /// <summary>
        /// رابط زر تبديل اللغة إلى targetLang ("ar" أو "en").
        ///
        /// المشكلة المرصودة (Semrush 2026-09-29): كان الرابط يُبنى بـ
        /// PathAndQuery.Replace("/ar/", "/en/") فيظهر عطلان:
        ///   1. الاستبدال يطال كل تطابق في المسار والاستعلام لا البادئة وحدها
        ///      (مثل ?Source=/ar/... داخل الاستعلام).
        ///   2. لا فحص لوجود الصفحة المقابلة، فصفحات عربية بلا نسخة إنجليزية
        ///      (مثل /ar/Faculties/HR/Pages/ResearchandInnovationAgency.aspx)
        ///      تحمل رابطاً داخلياً إلى 404.
        ///
        /// الحل على نهج SEO.ascx.cs (AlternateUrlExists) - التعريف نفسه للمقابل:
        ///   الصفحة المقابلة موجودة          -> هي نفسها مع الاستعلام
        ///   غير موجودة والموقع المقابل موجود -> جذر الموقع المقابل (صفحته الترحيبية)
        ///   ولا هذا ولا ذاك                 -> الصفحة الرئيسية للّغة الهدف
        /// </summary>
        private string BuildLanguageSwitchUrl(string targetLang)
        {
            string languageHome = "/" + targetLang + "/Pages/home.aspx";
            try
            {
                string path = Request.Url.AbsolutePath;
                string query = Request.Url.Query;

                string sourceLang = targetLang == "ar" ? "en" : "ar";
                string sourcePrefix = "/" + sourceLang + "/";
                string targetPrefix = "/" + targetLang + "/";

                // الصفحة أصلاً بلغة الهدف: الرابط يشير إليها نفسها
                if (path.StartsWith(targetPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return path + query;
                }

                // مسار بلا بادئة لغة معروفة (صفحات _layouts وغيرها): لا مقابل يمكن إثباته
                if (!path.StartsWith(sourcePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return languageHome;
                }

                string targetPath = targetPrefix + path.Substring(sourcePrefix.Length);
                // AbsolutePath مُرمَّز (%20 والأحرف العربية)، و GetFile يريد المسار المفكوك
                if (CounterpartExists("file", Uri.UnescapeDataString(targetPath)))
                {
                    return targetPath + query;
                }

                string webPath = SPContext.Current != null && SPContext.Current.Web != null
                    ? SPContext.Current.Web.ServerRelativeUrl.TrimEnd('/') + "/"
                    : string.Empty;
                if (webPath.StartsWith(sourcePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    string targetWeb = targetPrefix + webPath.Substring(sourcePrefix.Length);
                    if (CounterpartExists("web", targetWeb))
                    {
                        return targetWeb;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
            return languageHome;
        }

        /// <summary>
        /// هل الملف أو الموقع الفرعي المقابل موجود؟ النتيجة مخزَّنة ٦ ساعات لأن الفحص
        /// يمس قاعدة بيانات المحتوى في كل طلب. عند الفشل يُفترض الوجود (fail-open)
        /// كما في SEO.ascx.cs - رابط قد يكون معطوباً أهون من إخفاء مقابل صحيح.
        /// </summary>
        private static bool CounterpartExists(string kind, string serverRelativeUrl)
        {
            // الفحص مرفوع الصلاحية ويشترط النشر - انظر SEO.CounterpartIsPublic.
            // كان يجري بهوية الزائر، فإن كان المقابل محمياً أعاد شيربوينت الصفحة كلها 401.
            return SEO.CounterpartIsPublic(kind, serverRelativeUrl);
        }

        public string GetDynamicURL1(string URL)
        {
            try
            {
                return String.Format("{0}{1}", SPFactory.GetSiteURL(), URL);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Builds the DESKTOP navigation using the new DGA design classes.
        /// Output shape (per Level 1 with children):
        ///   <li class="nav-item dropdown">
        ///     <a ...>Title <i hgi-arrow-down-01/></a>
        ///     <ul class="dropdown-menu">
        ///       <li role="presentation">
        ///         <div class="container">
        ///           <ul class="list-unstyled w-100 d-flex m-auto flex-wrap">
        ///             [per Level 2] <li class="flex-fill list-unstyled"> ... <ul class="list-unstyled text-start"> [Level 3] </ul> </li>
        ///           </ul>
        ///         </div>
        ///       </li>
        ///     </ul>
        ///   </li>
        /// </summary>
        public void Get_MenuHTML_NewDesign(List<MenuLevel1> _AllData)
        {
            string HTML = "";

            if (_AllData != null && _AllData.Count > 0)
            {
                int lvl1Counter = 0;
                foreach (MenuLevel1 item in _AllData)
                {
                    lvl1Counter++;
                    string dropdownId = $"navDropdown{lvl1Counter}";
                    string menuId = $"navMenu{lvl1Counter}";

                    if (item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        // Has Level 3 under Level 2 -> grouped submenu (column-per-group layout)
                        if (item.LVL3 != null && item.LVL3.Count > 0)
                        {
                            string LVL1 = $@"<li class=""nav-item dropdown"">
                                <a id=""{dropdownId}"" role=""button"" data-bs-toggle=""dropdown"" data-bs-auto-close=""outside""
                                   class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium""
                                   aria-expanded=""false"" aria-haspopup=""true"" aria-controls=""{menuId}"" href=""#"">
                                    <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    <span class=""d-inline-flex fs-5"">
                                        <i class=""nav-link-icon rotation-transition hgi hgi-stroke hgi-arrow-down-01"" aria-hidden=""true""></i>
                                    </span>
                                </a>
                                <ul class=""dropdown-menu"" id=""{menuId}"" aria-labelledby=""{dropdownId}"">
                                    <li role=""presentation"">
                                        <div class=""container"">
                                            <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap"">
                                                @LVL2
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                              </li>";

                            string LVL2 = "";
                            foreach (MenuLevel2 item2 in item.LVL2)
                            {
                                List<MenuLevel3> itemsLVL3 = item.LVL3
                                    .Where(e => e != null && e.ParentTitle == item2.Title)
                                    .ToList();

                                if (itemsLVL3 != null && itemsLVL3.Count > 0)
                                {
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                <div>
                                                    <span class=""dga-submenutitle text-decoration-none fw-semibold pe-none text-start mb-2"">
                                                        <div class=""menu-item-title fs-6 px-2"">{item2.Title}</div>
                                                    </span>
                                                    <div class=""tree-menu"">
                                                        <ul class=""list-unstyled text-start"">
                                                            @LVL3
                                                        </ul>
                                                    </div>
                                                </div>
                                              </li>";

                                    string LVL3 = "";
                                    foreach (MenuLevel3 item3 in itemsLVL3)
                                    {
                                        LVL3 += $@"<li><a class=""p-2"" href=""{item3.URL}"">{item3.Title}</a></li>";
                                    }
                                    LVL2 = LVL2.Replace("@LVL3", LVL3);
                                }
                                else
                                {
                                    // Level 2 without Level 3 -> clickable heading
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                <div>
                                                    <a class=""dga-submenutitle text-decoration-none fw-semibold text-start mb-2"" href=""{item2.URL}"">
                                                        <div class=""menu-item-title fs-6 px-2"">{item2.Title}</div>
                                                    </a>
                                                </div>
                                              </li>";
                                }
                            }

                            LVL1 = LVL1.Replace("@LVL2", LVL2);
                            HTML += LVL1;
                        }
                        else
                        {
                            // Level 1 with only Level 2 children -> 3-column grid of links
                            string LVL1 = $@"<li class=""nav-item dropdown"">
                                <a id=""{dropdownId}"" role=""button"" data-bs-toggle=""dropdown"" data-bs-auto-close=""outside""
                                   class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium""
                                   aria-expanded=""false"" aria-haspopup=""true"" aria-controls=""{menuId}"" href=""#"">
                                    <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    <span class=""d-inline-flex fs-5"">
                                        <i class=""nav-link-icon rotation-transition hgi hgi-stroke hgi-arrow-down-01"" aria-hidden=""true""></i>
                                    </span>
                                </a>
                                <ul class=""dropdown-menu"" id=""{menuId}"" aria-labelledby=""{dropdownId}"">
                                    <li role=""presentation"">
                                        <div class=""container"">
                                            <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap"">
                                                <li class=""flex-fill list-unstyled"">
                                                    <div class=""tree-menu"">
                                                        <ul class=""list-unstyled text-start row gx-3"">
                                                            @LVL2
                                                        </ul>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                              </li>";

                            string LVL2 = "";
                            foreach (MenuLevel2 item2 in item.LVL2)
                            {
                                LVL2 += $@"<li class=""col-4""><a class=""p-2"" href=""{item2.URL}"">{item2.Title}</a></li>";
                            }

                            LVL1 = LVL1.Replace("@LVL2", LVL2);
                            HTML += LVL1;
                        }
                    }
                    else
                    {
                        // Level 1 as direct link (no dropdown)
                        HTML += $@"<li class=""nav-item"">
                                    <a class=""nav-link position-relative p-0 px-2 h-100 rounded-1 d-flex justify-content-center align-items-center fw-medium"" href=""{item.URL}"">
                                        <span class=""px-2 nav-link-text text-nowrap"">{item.Title}</span>
                                    </a>
                                   </li>";
                    }
                }
            }

            litMenu.Text = HTML;
        }

        /// <summary>
        /// Builds the MOBILE offcanvas navigation using the new DGA accordion classes.
        /// Output shape:
        ///   <li class="nav-item mb-1">
        ///     <a data-bs-toggle="collapse" data-bs-target="#menu_itemN">Title <i hgi-arrow-down-01/></a>
        ///     <ul id="menu_itemN" class="list-unstyled collapse text-muted" data-bs-parent="#nav_accordion">
        ///       [if LVL3] <li><a data-bs-toggle="collapse" href="#menu_itemN_subM">Title</a><ul id="menu_itemN_subM"> [LVL3] </ul></li>
        ///       [else]    <li><a href="url">Title</a></li>
        ///     </ul>
        ///   </li>
        /// </summary>
        public void Get_MenuHTML(List<TopMenuLevel1> _AllData)
        {
            StringBuilder sb = new StringBuilder();

            if (_AllData != null && _AllData.Count > 0)
            {
                int lvl1Counter = 0;
                foreach (TopMenuLevel1 item in _AllData)
                {
                    lvl1Counter++;
                    string menuId = $"menu_item{lvl1Counter}";

                    if (item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        sb.Append($@"
                <li class=""nav-item mb-1"">
                    <a class=""nav-link d-inline-flex w-100 justify-content-between collapsed fs-5 text-dark""
                       data-bs-toggle=""collapse"" data-bs-target=""#{menuId}"" aria-expanded=""false""
                       aria-controls=""{menuId}"">
                        {item.Title}
                        <i class=""verify-icon hgi hgi-stroke hgi-arrow-down-01 rotation-transition"" aria-hidden=""true""></i>
                    </a>
                    <ul id=""{menuId}"" class=""list-unstyled collapse text-muted"" data-bs-parent=""#nav_accordion"">");

                        int lvl2Counter = 0;
                        foreach (TopMenuLevel2 item2 in item.LVL2)
                        {
                            lvl2Counter++;
                            string subMenuId = $"{menuId}_sub{lvl2Counter}";

                            if (item2.LVL3 != null && item2.LVL3.Count > 0)
                            {
                                sb.Append($@"
                        <li class=""mt-2"">
                            <a class=""nav-link d-inline-flex w-100 justify-content-between collapsed fs-6 fw-semibold text-sa-600""
                               href=""#{subMenuId}"" role=""button"" data-bs-toggle=""collapse""
                               aria-expanded=""false"" aria-controls=""{subMenuId}"">
                                {item2.Title}
                                <i class=""verify-icon hgi hgi-stroke hgi-arrow-down-01 rotation-transition"" aria-hidden=""true""></i>
                            </a>
                            <ul id=""{subMenuId}"" class=""list-unstyled collapse ps-2 bg-light rounded-2 px-2 py-1 mt-1""
                                data-bs-parent=""#{menuId}"">");

                                foreach (TopMenuLevel3 item3 in item2.LVL3)
                                {
                                    sb.Append($@"<li><a class=""nav-link fs-6"" href=""{item3.URL}"">{item3.Title}</a></li>");
                                }

                                sb.Append("</ul></li>");
                            }
                            else
                            {
                                sb.Append($@"<li><a class=""nav-link fs-6"" href=""{item2.URL}"">{item2.Title}</a></li>");
                            }
                        }

                        sb.Append("</ul></li>");
                    }
                    else
                    {
                        sb.Append($@"<li class=""nav-item mb-1""><a class=""nav-link fs-5 text-dark"" href=""{item.URL}"">{item.Title}</a></li>");
                    }
                }
            }

            // The outer <ul id="nav_accordion"> is in the markup; we only emit its <li> children.
            litMenuMobile.Text = sb.ToString();
        }

        protected void lnkbtn_signout_Click1(object sender, EventArgs e)
        {
            try
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                FormsAuthentication.SignOut();
                if (HttpContext.Current == null)
                    return;
                HttpContext.Current.Response.Redirect(PortalHelper.ParentLangSite, false);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
        }
        public string ResultsPageUrl { get; set; }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string q = txtKeyword.Text == null ? "" : txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(q) && txtKeywordDesktop != null && !string.IsNullOrEmpty(txtKeywordDesktop.Text))
            {
                q = txtKeywordDesktop.Text.Trim();
            }
            DoSearch(q);
        }

        protected void btnSearchDesktop_Click(object sender, EventArgs e)
        {
            string q = txtKeywordDesktop.Text == null ? "" : txtKeywordDesktop.Text.Trim();
            if (string.IsNullOrEmpty(q) && txtKeyword != null && !string.IsNullOrEmpty(txtKeyword.Text))
            {
                q = txtKeyword.Text.Trim();
            }
            DoSearch(q);
        }

        private void DoSearch(string q)
        {
            string target = !string.IsNullOrEmpty(ResultsPageUrl)
                ? ResultsPageUrl
                : (IsArabic
                    ? "/ar/Search/Pages/SearchResults.aspx"
                    : "/en/Search/Pages/SearchResults.aspx");

            string url = target + "?q=" + HttpUtility.UrlEncode(q);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }



        private void FixCanonicalLink()
        {
            try
            {
                if (SPContext.Current == null || Page.Header == null) return;

                // Hide SharePoint's built-in SeoCanonicalLink (handled centrally by SEO.ascx)
                HideCanonicalControls(Page.Header);
            }
            catch (Exception)
            {
            }
        }

        private void HideCanonicalControls(Control root)
        {
            if (root == null) return;
            if (root.GetType().Name.IndexOf("Canonical",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                root.Visible = false;
                return;
            }
            foreach (Control child in root.Controls)
                HideCanonicalControls(child);
        }

    }
}
