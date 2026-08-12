using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ITAdmin
{
    /// <summary>
    /// Page Creator admin tool — hosted on /ar/ITAdmin/.
    /// Access is gated by the PageCreatorAdmins list (+ site collection admins).
    /// ViewState is unreliable in SP zones, so lookups are rebound on every Page_Load.
    /// </summary>
    public partial class ucPageCreator : UserControl
    {
        /// <summary>DTO for the results repeater — no code-behind members in markup.</summary>
        public class clsResultLine
        {
            public string Message { get; set; }
            public string CssClass { get; set; }
        }

        private bool _isAdmin;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                // Provision lists for authenticated users only
                if (SPContext.Current.Web.CurrentUser != null)
                    PageCreatorProvisioner.EnsureAll();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.OnInit", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _isAdmin = busclsPageCreator.IsPageCreatorAdmin();

                pnlDenied.Visible = !_isAdmin;
                pnlForm.Visible = _isAdmin;

                SetTitles();

                if (!_isAdmin) return;

                // Rebind on every load (ViewState off in SP zones); posted
                // selections are restored by the second post-data pass.
                BindTemplates();
                BindLayouts();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.Page_Load", ex.Message);
            }
        }

        #region Binding

        private void BindTemplates()
        {
            try
            {
                string selected = Request.Form[ddlTemplate.UniqueID];

                ddlTemplate.Items.Clear();
                ddlTemplate.Items.Add(new ListItem(T("PC_SelectTemplate", "-- اختر القالب --", "-- Select template --"), ""));

                foreach (clsPageTemplate t in busclsPageCreator.GetTemplates())
                    ddlTemplate.Items.Add(new ListItem(t.TemplateName, t.ID.ToString()));

                if (!string.IsNullOrEmpty(selected) && ddlTemplate.Items.FindByValue(selected) != null)
                    ddlTemplate.SelectedValue = selected;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.BindTemplates", ex.Message);
            }
        }

        private void BindLayouts()
        {
            try
            {
                string selected = Request.Form[ddlPageLayout.UniqueID];

                ddlPageLayout.Items.Clear();
                ddlPageLayout.Items.Add(new ListItem(T("PC_SelectLayout", "-- اختر تخطيط الصفحة --", "-- Select page layout --"), ""));

                foreach (clsPageLayout l in busclsPageCreator.GetLayouts())
                    ddlPageLayout.Items.Add(new ListItem(l.Name, l.PageLayoutURL));

                if (!string.IsNullOrEmpty(selected) && ddlPageLayout.Items.FindByValue(selected) != null)
                    ddlPageLayout.SelectedValue = selected;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.BindLayouts", ex.Message);
            }
        }

        #endregion

        #region Events

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hfUcPath.Value = string.Empty;

                int templateId;
                if (!int.TryParse(ddlTemplate.SelectedValue, out templateId)) return;

                clsPageTemplate template = busclsPageCreator.GetTemplates()
                    .FirstOrDefault(t => t.ID == templateId);
                if (template == null) return;

                hfUcPath.Value = template.UserControlPath;
                txtUcProperties.Text = template.DefaultProperties;

                // Pre-select the template's layout in the layout DDL when it is catalogued
                if (!string.IsNullOrEmpty(template.PageLayoutURL))
                {
                    ListItem match = ddlPageLayout.Items.Cast<ListItem>()
                        .FirstOrDefault(i => i.Value.Equals(template.PageLayoutURL, StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                        ddlPageLayout.SelectedValue = match.Value;
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.ddlTemplate_SelectedIndexChanged", ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isAdmin) return;

                pnlValidation.Visible = false;
                pnlResults.Visible = false;

                string error = Validate();
                if (!string.IsNullOrEmpty(error))
                {
                    litValidationMsg.Text = error;
                    pnlValidation.Visible = true;
                    return;
                }

                clsPageCreateRequest req = BuildRequest();
                List<string> log = busclsPageCreator.CreatePages(req);

                rptResults.DataSource = log.Select(ToResultLine).ToList();
                rptResults.DataBind();
                pnlResults.Visible = true;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucPageCreator.btnCreate_Click", ex.Message);
            }
        }

        #endregion

        #region Helpers

        private clsPageCreateRequest BuildRequest()
        {
            // Layout precedence: selected template's layout, else layout DDL
            string layoutUrl = ddlPageLayout.SelectedValue;
            string ucPath = hfUcPath.Value;

            int templateId;
            if (int.TryParse(ddlTemplate.SelectedValue, out templateId))
            {
                clsPageTemplate template = busclsPageCreator.GetTemplates()
                    .FirstOrDefault(t => t.ID == templateId);
                if (template != null)
                {
                    if (!string.IsNullOrEmpty(template.PageLayoutURL))
                        layoutUrl = template.PageLayoutURL;
                    ucPath = template.UserControlPath;
                }
            }

            return new clsPageCreateRequest
            {
                WebSiteUrl = txtWebSiteUrl.Text,
                PageName = txtPageName.Text,
                PageTitleAr = txtPageTitleAr.Text.Trim(),
                PageTitleEn = txtPageTitleEn.Text.Trim(),
                PageLayoutURL = layoutUrl,
                UserControlPath = ucPath,
                UserControlProperties = txtUcProperties.Text.Trim(),
                AddToBothSites = chkBothSites.Checked
            };
        }

        private string Validate()
        {
            if (string.IsNullOrEmpty(txtWebSiteUrl.Text.Trim()))
                return T("PC_ValUrl", "الرجاء إدخال رابط الموقع.", "Please enter the web site URL.");

            if (string.IsNullOrEmpty(busclsPageCreator.NormalizePageName(txtPageName.Text)))
                return T("PC_ValName", "الرجاء إدخال اسم صفحة صحيح (حروف إنجليزية وأرقام و - فقط).",
                    "Please enter a valid page name (letters, numbers and - only).");

            if (string.IsNullOrEmpty(txtPageTitleAr.Text.Trim()) && string.IsNullOrEmpty(txtPageTitleEn.Text.Trim()))
                return T("PC_ValTitle", "الرجاء إدخال عنوان الصفحة بالعربية أو الإنجليزية.",
                    "Please enter the page title in Arabic or English.");

            bool hasTemplate = !string.IsNullOrEmpty(ddlTemplate.SelectedValue);
            bool hasLayout = !string.IsNullOrEmpty(ddlPageLayout.SelectedValue);
            if (!hasTemplate && !hasLayout)
                return T("PC_ValLayout", "الرجاء اختيار قالب أو تخطيط صفحة.",
                    "Please select a template or a page layout.");

            if (chkBothSites.Checked)
            {
                string normalized = busclsPageCreator.NormalizeWebUrl(txtWebSiteUrl.Text);
                if (string.IsNullOrEmpty(busclsPageCreator.GetPairedWebUrl(normalized)))
                    return T("PC_ValPair", "لإنشاء الصفحة في الموقعين يجب أن يبدأ الرابط بـ /ar/ أو /en/.",
                        "To create on both sites the URL must start with /ar/ or /en/.");
            }

            return string.Empty;
        }

        private clsResultLine ToResultLine(string logLine)
        {
            string css = "alert-secondary";
            string msg = logLine;

            int idx = logLine.IndexOf('|');
            if (idx > 0)
            {
                string level = logLine.Substring(0, idx);
                msg = logLine.Substring(idx + 1);
                if (level == "OK") css = "alert-success";
                else if (level == "WARN") css = "alert-warning";
                else if (level == "ERROR") css = "alert-danger";
            }

            return new clsResultLine { Message = msg, CssClass = css };
        }

        private void SetTitles()
        {
            litPageHeader.Text = T("PC_Header", "إنشاء صفحات البوابة", "Portal Page Creator");
            litDeniedMsg.Text = T("PC_Denied", "عذراً، لا تملك صلاحية استخدام هذه الأداة.", "Sorry, you are not authorized to use this tool.");

            lblWebSiteUrl.Text = T("PC_WebUrl", "رابط الموقع", "Web Site URL");
            litWebSiteUrlHint.Text = T("PC_WebUrlHint", "رابط نسبي مثل /ar/Faculties/Science", "Server-relative URL, e.g. /ar/Faculties/Science");

            lblTemplate.Text = T("PC_Template", "قالب الصفحة", "Page Template");
            litTemplateHint.Text = T("PC_TemplateHint", "يحدد تخطيط الصفحة ووحدة التحكم تلقائياً", "Sets the layout and user control automatically");

            lblPageLayout.Text = T("PC_Layout", "تخطيط الصفحة", "Page Layout");
            litLayoutHint.Text = T("PC_LayoutHint", "يُستخدم عند عدم اختيار قالب", "Used when no template is selected");

            lblPageName.Text = T("PC_PageName", "اسم الصفحة", "Page Name");
            lblTitleAr.Text = T("PC_TitleAr", "عنوان الصفحة (عربي)", "Page Title (Arabic)");
            lblTitleEn.Text = T("PC_TitleEn", "عنوان الصفحة (إنجليزي)", "Page Title (English)");

            lblUcProperties.Text = T("PC_UcProps", "خصائص وحدة التحكم", "User Control Properties");
            litUcPropertiesHint.Text = T("PC_UcPropsHint", "أزواج Key=Value مفصولة بفاصلة منقوطة", "Key=Value pairs separated by semicolons");

            lblBothSites.Text = T("PC_BothSites", "إنشاء في الموقعين العربي والإنجليزي", "Create on both AR and EN sites");

            btnCreate.Text = T("PC_Create", "إنشاء الصفحة", "Create Page");
            litResultsHeader.Text = T("PC_Results", "النتائج", "Results");
        }

        /// <summary>
        /// Resource lookup with hardcoded bilingual fallbacks (standard portal pattern).
        /// </summary>
        private string T(string key, string fallbackAr, string fallbackEn)
        {
            bool isArabic = SPContext.Current.Web.Language == 1025;
            try
            {
                object res = HttpContext.GetGlobalResourceObject("PNUres", key);
                if (res != null && !string.IsNullOrEmpty(res.ToString()))
                    return res.ToString();
            }
            catch { /* fall through to fallback */ }

            return isArabic ? fallbackAr : fallbackEn;
        }

        #endregion
    }
}
