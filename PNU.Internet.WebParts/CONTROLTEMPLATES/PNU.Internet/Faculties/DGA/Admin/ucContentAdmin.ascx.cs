using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Admin
{
    /// <summary>
    /// Container for the content-administration screen.
    ///
    /// Layout, top to bottom:
    ///   1. the target subweb (which faculty am I editing)
    ///   2. the page section (which part of the faculty page)
    ///   3. one ucListEditor per list in that section - each shows existing data
    ///      and allows add / edit / delete.
    ///
    /// Only users approved in the PortalAdmins list on the /admin/ web (or site
    /// collection administrators) see anything at all. Everyone else gets a notice.
    /// </summary>
    public partial class ucContentAdmin : UserControl
    {
        private const string EditorFileName = "ucListEditor.ascx";

        /// <summary>
        /// Path to the editor control, resolved from THIS control's own folder at
        /// runtime rather than hard-coded. ucListEditor.ascx is deployed alongside
        /// ucContentAdmin.ascx, so the pair keeps working wherever the hive folder
        /// ends up (…/PNU.Internet/Faculties/DGA/Admin/ or anywhere else).
        /// </summary>
        private string EditorPath
        {
            get
            {
                string dir = AppRelativeTemplateSourceDirectory;   // e.g. ~/_controltemplates/15/PNU.Internet/Faculties/DGA/Admin/
                if (string.IsNullOrEmpty(dir)) return EditorFileName;
                if (!dir.EndsWith("/")) dir += "/";
                return dir + EditorFileName;
            }
        }

        private string SelectedWeb { get; set; }
        private string SelectedSection { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // Make sure the admin list exists so the first administrator can populate it.
                AdminUsersProvisioner.EnsureList();
            }
            

            if (!AdminSecurity.IsAuthorized()) return;

            // Read the current choices straight from the posted form: the dynamic
            // editors must be rebuilt in OnInit, before ViewState/events are applied.
            SelectedWeb = PostedValue(ddlWeb, DefaultWebUrl());
            SelectedSection = PostedValue(ddlSection, DefaultSectionKey());

            BuildEditors();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!AdminSecurity.IsAuthorized())
                {
                    phDenied.Visible = true;
                    phAdmin.Visible = false;
                    return;
                }

                phDenied.Visible = false;
                phAdmin.Visible = true;

                if (!IsPostBack)
                {
                    BindWebs();
                    BindSections();
                }

                SelectValue(ddlWeb, SelectedWeb);
                SelectValue(ddlSection, SelectedSection);

                ShowContext();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucContentAdmin - Page_Load", ex.Message);
            }
        }

        // ---------------- pickers ----------------

        private void BindWebs()
        {
            List<KeyValuePair<string, string>> webs = AdminSecurity.GetSiteWebs();
            ddlWeb.Items.Clear();
            foreach (KeyValuePair<string, string> w in webs)
                ddlWeb.Items.Add(new ListItem(w.Value, w.Key));
        }

        private void BindSections()
        {
            ddlSection.Items.Clear();
            foreach (SectionDef s in ContentSchema.Sections)
                ddlSection.Items.Add(new ListItem(s.Display, s.Key));
        }

        protected void ddlWeb_Changed(object sender, EventArgs e)
        {
            // Editors were rebuilt in OnInit against the new selection; just refresh labels.
            SelectedWeb = ddlWeb.SelectedValue;
            ShowContext();
        }

        protected void ddlSection_Changed(object sender, EventArgs e)
        {
            SelectedSection = ddlSection.SelectedValue;
            ShowContext();
        }

        private void ShowContext()
        {
            SectionDef sec = ContentSchema.FindSection(SelectedSection);
            ltrContext.Text = HttpUtility.HtmlEncode(
                "الموقع: " + (SelectedWeb ?? "-") + "   |   القسم: " + (sec != null ? sec.Display : "-"));

            bool inScope = AdminSecurity.IsWebInScope(SelectedWeb);
            phScopeWarning.Visible = !inScope;
            phEditors.Visible = inScope;

            // A blank screen is impossible to diagnose; say what failed instead.
            phLoadError.Visible = !string.IsNullOrEmpty(_loadError);
            ltrLoadError.Text = HttpUtility.HtmlEncode(_loadError ?? "");
        }

        // ---------------- dynamic editors ----------------

        /// <summary>Set when the editor control could not be loaded, so the screen
        /// reports the reason instead of rendering an empty page.</summary>
        private string _loadError;

        private void BuildEditors()
        {
            SectionDef sec = ContentSchema.FindSection(SelectedSection);
            if (sec == null) return;
            if (!AdminSecurity.IsWebInScope(SelectedWeb)) return;

            foreach (ListDef def in sec.Lists)
            {
                try
                {
                    ucListEditor editor = (ucListEditor)Page.LoadControl(EditorPath);
                    // Stable ID so postbacks map back to the same control instance.
                    editor.ID = "ed_" + def.ListName;
                    editor.WebUrl = SelectedWeb;
                    editor.ListName = def.ListName;
                    phEditors.Controls.Add(editor);
                }
                catch (Exception ex)
                {
                    _loadError = "تعذر تحميل محرر القائمة (" + def.ListName + ") من المسار: "
                               + EditorPath + " — " + ex.Message;
                    Publics.WriteToLog("", "ucContentAdmin - BuildEditors:" + def.ListName,
                        EditorPath + " | " + ex.Message);
                    break;      // the path is wrong for every editor; report once
                }
            }
        }

        // ---------------- helpers ----------------

        /// <summary>
        /// Value posted for a dropdown, or the fallback. Used during OnInit when
        /// SelectedValue is not populated yet.
        /// </summary>
        private string PostedValue(DropDownList ddl, string fallback)
        {
            try
            {
                if (ddl != null && Page.IsPostBack)
                {
                    string v = Request.Form[ddl.UniqueID];
                    if (!string.IsNullOrEmpty(v)) return v;
                }
            }
            catch { }
            return fallback;
        }

        private static void SelectValue(DropDownList ddl, string value)
        {
            if (ddl == null || string.IsNullOrEmpty(value)) return;
            ListItem li = ddl.Items.FindByValue(value);
            if (li != null)
            {
                ddl.ClearSelection();
                li.Selected = true;
            }
        }

        private static string DefaultWebUrl()
        {
            try { return SPContext.Current.Web.ServerRelativeUrl; }
            catch { return ""; }
        }

        private static string DefaultSectionKey()
        {
            List<SectionDef> all = ContentSchema.Sections;
            return all.Count > 0 ? all[0].Key : "";
        }
    }

}
