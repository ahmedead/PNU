using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucDGAServiceDetails : UserControl
    {
        private const string ResourceFile = "PNUres";
        private int uid;
        private string _serviceTitle;

        protected bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        // ------------------------------------------------------------
        // Resource helper: PNUres first, hard-coded bilingual fallback
        // (missing keys never break the page — no parse-time expressions)
        // ------------------------------------------------------------
        private string GetRes(string key, string fallbackAr, string fallbackEn)
        {
            try
            {
                CultureInfo culture = IsArabic ? new CultureInfo("ar-SA") : new CultureInfo("en-US");
                object val = HttpContext.GetGlobalResourceObject(ResourceFile, key, culture);
                if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    return val.ToString();
            }
            catch { /* missing key / file — use fallback */ }
            return IsArabic ? fallbackAr : fallbackEn;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["eti"] = true;
                string testParameter = Request.QueryString["eti"];
                uid = Convert.ToInt32(testParameter);
                if (!IsPostBack)
                {
                    ServiceDetailsDgaProvisioner.EnsureAll(SPContext.Current.Site.Url);
                    Bindservices();

                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void Bindservices()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        
                        SPList list = web.Lists["EservicesList"];
                        if (list == null) return;

                        SPQuery query = new SPQuery();
                        query.Query = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + uid + "</Value></Eq></Where>";

                        SPListItemCollection collitem = list.GetItems(query);
                        if (collitem == null || collitem.Count == 0) return;

                        // Last modified from the SP item itself
                        DateTime modified = (DateTime)collitem[0]["Modified"];
                        string iso = modified.ToString("yyyy-MM-ddTHH:mmzzz");
                        string display = modified.ToString("dd/MM/yyyy - hh:mm");
                        string lastModTemplate = GetRes("res_LastModified",
                            "تاريخ آخر تعديل: <time datetime=\"{0}\">{1} بتوقيت السعودية</time>",
                            "Last Modified: <time datetime=\"{0}\">{1} KSA Time</time>");
                        litLastModified.Text = string.Format(lastModTemplate, iso, display);

                        List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(collitem);

                        _serviceTitle = SPFactory.GetLocalizedTitle(eserviceList[0].ARServiceName, eserviceList[0].ENServiceName);
                        SetBrowserTitle(_serviceTitle);

                        if (string.Equals(Request.QueryString["sladebug"], "1", StringComparison.Ordinal))
                            WriteSlaDebug(web, list, collitem[0], eserviceList[0]);

                        rptAllData.DataSource = eserviceList;
                        rptAllData.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void rptAllData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            try
            {
                SetTitles(e);

                // ---- Badges: hide any empty badge ----
                HideIfEmpty(e, "badge1", GetLocalized(e, "Badge1", "Badge1_EN"));
                HideIfEmpty(e, "badge2", GetLocalized(e, "Badge2", "Badge2_EN"));
                HideIfEmpty(e, "badge3", GetLocalized(e, "Badge3", "Badge3_EN"));

                // ---- Requirements: hide empty items ----
                HideIfEmpty(e, "req1", GetLocalized(e, "_x0052_eq1", "Req1_EN"));
                HideIfEmpty(e, "req2", GetLocalized(e, "_x0052_eq2", "Req2_EN"));
                HideIfEmpty(e, "req3", GetLocalized(e, "_x0052_eq3", "Req3_EN"));

                // ---- SLA link: bilingual link (ServiceAgreementURL_EN on EN; fallback to ServiceAgreement / ServiceAgreementURL) ----
                string slaUrl = null;
                EServicesList esvc = e.Item.DataItem as EServicesList;
                if (esvc != null)
                {
                    slaUrl = esvc.DisplayServiceAgreement;
                }
                else
                {
                    slaUrl = (DataBinder.Eval(e.Item.DataItem, "DisplayServiceAgreement") as string)
                        ?? (DataBinder.Eval(e.Item.DataItem, "ServiceAgreement") as string)
                        ?? (DataBinder.Eval(e.Item.DataItem, "ServiceAgreementURL") as string);
                }

                HideIfEmpty(e, "slaLink", slaUrl);
                SetAnchorHref(e, "slaLink", slaUrl ?? "");

                // ---- User manual: hide when no URL ----
                HideIfEmpty(e, "UserManualLink", DataBinder.Eval(e.Item.DataItem, "UserManualURL") as string);

                // ---- Language / Cost rows: hide when empty ----
                HideIfEmpty(e, "langRow", GetLocalized(e, "ServiceLanguage", "ServiceLanguage_EN"));
                HideIfEmpty(e, "costRow", GetLocalized(e, "ServiceCost", "ServiceCost_EN"));

                string notAvailable = GetRes("res_NotAvailable", "لا يوجد", "N/A");

                // ---- Steps (rich text) ----
                Literal litSteps = (Literal)e.Item.FindControl("litSteps");
                string steps = GetLocalized(e, "Steps", "Steps_EN");
                litSteps.Text = string.IsNullOrWhiteSpace(steps)
                    ? HttpUtility.HtmlEncode(notAvailable)
                    : steps; // rich HTML from the list

                // ---- Required documents (rich text) ----
                Literal litRequiredDocs = (Literal)e.Item.FindControl("litRequiredDocs");
                string docs = GetLocalized(e, "RequiredDocs", "RequiredDocs_EN");
                litRequiredDocs.Text = string.IsNullOrWhiteSpace(docs)
                    ? HttpUtility.HtmlEncode(notAvailable)
                    : docs;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------
        // All labels + static links from PNUres (set per bound item)
        // ------------------------------------------------------------
        private void SetTitles(RepeaterItemEventArgs e)
        {
            string langPrefix = IsArabic ? "/ar" : "/en";

            // Breadcrumb
            HtmlAnchor lnkHome = (HtmlAnchor)e.Item.FindControl("lnkHome");
            lnkHome.HRef = langPrefix;
            lnkHome.InnerText = GetRes("res_Home", "الرئيسية", "Home");

            HtmlAnchor lnkEServices = (HtmlAnchor)e.Item.FindControl("lnkEServices");
            //lnkEServices.HRef = langPrefix + "/Pages/Eservice.aspx";
            lnkEServices.InnerText = GetRes("res_EServices", "الخدمات الإلكترونية", "E-Services");

            // Start service button (href already databound from URL field, override with DisplayURL if bilingual URL is present)
            HtmlAnchor lnkStart = (HtmlAnchor)e.Item.FindControl("lnkStart");
            lnkStart.InnerText = GetRes("res_StartService", "بدء الخدمة", "Start Service");
            EServicesList svc = e.Item.DataItem as EServicesList;
            if (svc != null && !string.IsNullOrEmpty(svc.DisplayURL))
            {
                lnkStart.HRef = svc.DisplayURL;
            }

            // SLA + tabs
            SetLiteral(e, "litSla", GetRes("res_SLA", "اتفاقية مستوى الخدمة", "Service Level Agreement"));
            SetLiteral(e, "litTabSteps", GetRes("res_Steps", "الخطوات", "Steps"));
            SetLiteral(e, "litTabReq", GetRes("res_ServiceRequirements", "متطلبات الخدمة", "Service Requirements"));
            SetLiteral(e, "litTabDocs", GetRes("res_RequiredDocuments", "المستندات المطلوبة", "Required Documents"));

            // Side card headings
            SetLiteral(e, "litTargetAudience", GetRes("res_TargetAudience", "الفئة المستهدفة", "Target Audience"));
            SetLiteral(e, "litDuration", GetRes("res_ServiceDuration", "مدة الخدمة", "Service Duration"));
            SetLiteral(e, "litChannels", GetRes("res_ServiceChannels", "قنوات الخدمة", "Service Channels"));
            SetLiteral(e, "litLanguage", GetRes("res_ServiceLanguage", "الخدمة متاحة باللغة", "Service Available In"));
            SetLiteral(e, "litCost", GetRes("res_ServiceCost", "تكلفة الخدمة", "Service Cost"));

            // Static section (same for all services)
            SetLiteral(e, "litFaqTitle", GetRes("res_FAQ", "الأسئلة الشائعة", "FAQs"));
            SetLiteral(e, "litFaqPage", GetRes("res_FAQPage", "صفحة الأسئلة الشائعة", "FAQs Page"));
            HtmlAnchor lnkFaq = (HtmlAnchor)e.Item.FindControl("lnkFaq");
            lnkFaq.HRef = langPrefix + "/FAQs/Pages/default.aspx";

            SetLiteral(e, "litTelephone", GetRes("res_Telephone", "الهاتف", "Telephone"));
            SetLiteral(e, "litEmail", GetRes("res_Email", "البريد الإلكتروني", "E-mail"));
            SetLiteral(e, "litSharePage", GetRes("res_SharePage", "مشاركة الصفحة", "Share Page"));
            SetLiteral(e, "litUserManual", GetRes("res_DownloadUserGuide", "تحميل دليل المستخدم", "Download User Guide"));

            // Share links (built from the current page URL)
            string encodedUrl = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri);
            SetAnchorHref(e, "lnkShareWa", "https://wa.me/?text=" + encodedUrl);
            SetAnchorHref(e, "lnkShareLi", "https://www.linkedin.com/sharing/share-offsite/?url=" + encodedUrl);
            SetAnchorHref(e, "lnkShareEmail", "mailto:?subject=" + HttpUtility.UrlEncode(_serviceTitle ?? "") + "&body=" + encodedUrl);
            SetAnchorHref(e, "lnkShareX", "https://twitter.com/intent/tweet?url=" + encodedUrl);
            SetAnchorHref(e, "lnkShareFb", "https://www.facebook.com/sharer/sharer.php?u=" + encodedUrl);
        }

        private void SetLiteral(RepeaterItemEventArgs e, string id, string text)
        {
            Literal lit = e.Item.FindControl(id) as Literal;
            if (lit != null) lit.Text = text;
        }

        private void SetAnchorHref(RepeaterItemEventArgs e, string id, string href)
        {
            HtmlAnchor a = e.Item.FindControl(id) as HtmlAnchor;
            if (a != null) a.HRef = href;
        }

        private string GetLocalized(RepeaterItemEventArgs e, string arField, string enField)
        {
            return SPFactory.GetLocalizedTitle(
                DataBinder.Eval(e.Item.DataItem, arField),
                DataBinder.Eval(e.Item.DataItem, enField));
        }

        private void HideIfEmpty(RepeaterItemEventArgs e, string controlId, string value)
        {
            HtmlControl ctrl = e.Item.FindControl(controlId) as HtmlControl;
            if (ctrl != null && string.IsNullOrWhiteSpace(value))
                ctrl.Visible = false;
        }

        private void SetBrowserTitle(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title)) return;
                HttpContext.Current.Items["PNU_BrowserTitle"] = title;

                // العنوان وحده كان يُنشَر، فتسقط SEO.ascx إلى وصف عام مبني على عنوان
                // الصفحة القالبية "تفاصيل الخدمة" — يتقاسمه كل الخدمات:
                //     "صفحة تفاصيل الخدمة ضمن خدمات ومعلومات ..."
                // رقم الخدمة (eti) في آخر الوصف يضمن التفرّد.
                string eti = HttpContext.Current.Request.QueryString["eti"];
                HttpContext.Current.Items["PNU_BrowserDescription"] =
                    Publics.BuildSeoDescription(Publics.BuildSeoSiteChain(), title, eti);
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(title)));
                }
                else
                {
                    Page.Title = title;
                }
            }
            catch { }
        }

        /// <summary>
        /// ?sladebug=1 — writes an HTML comment (view source) with what the SLA link was built from:
        /// the web/list actually read, every ServiceAgreement* column with its raw value, and the DLL build time.
        /// </summary>
        private void WriteSlaDebug(SPWeb web, SPList list, SPListItem item, EServicesList svc)
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("SLA DEBUG");
                sb.AppendLine("CurrentWeb.Language = " + SPContext.Current.Web.Language + " (IsArabic=" + IsArabic + ")");
                sb.AppendLine("List read from = " + web.Url + " / " + list.RootFolder.ServerRelativeUrl + " (ListId " + list.ID + ")");
                sb.AppendLine("Item ID = " + item.ID);

                foreach (SPField f in list.Fields)
                {
                    if (f.InternalName.IndexOf("ServiceAgreement", StringComparison.OrdinalIgnoreCase) < 0
                        && f.Title.IndexOf("Service Agreement", StringComparison.OrdinalIgnoreCase) < 0
                        && f.Title.IndexOf("ServiceAgreement", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object raw = item[f.Id];
                    sb.AppendLine("Field Internal='" + f.InternalName + "' Title='" + f.Title + "' Type=" + f.TypeAsString
                        + " Value='" + (raw == null ? "<null>" : raw.ToString()) + "'");
                }

                sb.AppendLine("Mapped ServiceAgreement='" + svc.ServiceAgreement + "' ServiceAgreementURL='" + svc.ServiceAgreementURL
                    + "' ServiceAgreementURL_EN='" + svc.ServiceAgreementURL_EN + "'");
                sb.AppendLine("DisplayServiceAgreement='" + svc.DisplayServiceAgreement + "'");

                string asmPath = typeof(EServicesList).Assembly.Location;
                sb.AppendLine("DLL = " + asmPath + " built " + System.IO.File.GetLastWriteTime(asmPath).ToString("yyyy-MM-dd HH:mm:ss"));

                // "--" would end the HTML comment early
                Controls.Add(new LiteralControl("\n<!--\n" + HttpUtility.HtmlEncode(sb.ToString()).Replace("--", "- -") + "-->\n"));
            }
            catch (Exception ex)
            {
                Controls.Add(new LiteralControl("\n<!-- SLA DEBUG failed: " + HttpUtility.HtmlEncode(ex.Message).Replace("--", "- -") + " -->\n"));
            }
        }

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
