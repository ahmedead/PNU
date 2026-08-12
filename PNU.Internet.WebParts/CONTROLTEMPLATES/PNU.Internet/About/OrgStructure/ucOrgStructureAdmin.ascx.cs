using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About
{
    /// <summary>
    /// In-page admin editor for the OrgStructureUnits list. Authorized via the
    /// PortalAdmins list (same gate as ucContentAdmin). Provides create / edit /
    /// delete of org units. ViewState is disabled in SharePoint zones — the grid
    /// is rebound on every load and item identity is carried in HiddenField / command args.
    /// </summary>
    public partial class ucOrgStructureAdmin : UserControl
    {
        private const string PortalAdminsList = "AdminUsers";
        private bool _authorized;

        /// <summary>Server-relative URL of the web that holds the OrgStructureUnits list.</summary>
        [System.ComponentModel.Browsable(true)]
        public string ListWebUrl { get; set; }

        private string EffectiveWebUrl
        {
            get { return string.IsNullOrWhiteSpace(ListWebUrl) ? "/ar/AboutUniversity/" : ListWebUrl.Trim(); }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                if (SPContext.Current != null &&
                    SPContext.Current.Web != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    OrgStructureProvisioner.EnsureListAt(SPContext.Current.Site.ID, EffectiveWebUrl);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructureAdmin.OnInit", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorized = IsAuthorized();

            SetLabels();

            pnlAdmin.Visible = _authorized;
            pnlDenied.Visible = !_authorized;

            if (!_authorized)
            {
                litDenied.Text = IsArabic
                    ? "ليس لديك صلاحية إدارة الهيكل التنظيمي."
                    : "You are not authorized to manage the org structure.";
                return;
            }

            BindGrid();
        }

        // ---------------------------------------------------------------
        // Authorization — three checks: authenticated, PortalAdmins entry,
        // or site-collection administrator.
        // ---------------------------------------------------------------
        private bool IsAuthorized()
        {
            return ContentAdm.IsAdmin();
        }

        private void BindGrid()
        {
            try
            {
                var repo = new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl);
                rptUnits.DataSource = repo.GetAllUnits();
                rptUnits.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "ucOrgStructureAdmin.BindGrid", ex.Message);
            }
        }

        // ---------------------------------------------------------------
        // Grid commands
        // ---------------------------------------------------------------
        protected void rptUnits_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!IsAuthorized()) return;

            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

            if (e.CommandName == "EditUnit")
            {
                LoadIntoEditor(id);
            }
            else if (e.CommandName == "DeleteUnit")
            {
                new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl).Delete(id);
                ShowMessage(IsArabic ? "تم حذف الوحدة." : "Unit deleted.");
                pnlEditor.Visible = false;
                BindGrid();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (!IsAuthorized()) return;
            ClearEditor();
            hidId.Value = "0";
            txtOrder.Text = NextOrder().ToString(CultureInfo.InvariantCulture);
            chkActive.Checked = true;
            pnlEditor.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEditor.Visible = false;
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsAuthorized()) return;

            int id;
            int.TryParse(hidId.Value, out id);
            int order;
            int.TryParse(txtOrder.Text, out order);

            var unit = new OrgStructureUnit
            {
                Id = id,
                Order = order,
                Selector = txtSelector.Text.Trim(),
                Title = txtTitle.Text.Trim(),
                TitleEn = txtTitleEn.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                DescriptionEn = txtDescEn.Text.Trim(),
                Badge = txtBadge.Text.Trim(),
                BadgeEn = txtBadgeEn.Text.Trim(),
                Meta = txtMeta.Text.Trim(),
                MetaEn = txtMetaEn.Text.Trim(),
                Icon = txtIcon.Text.Trim(),
                Theme = ddlTheme.SelectedValue,
                TextColor = txtTextColor.Text.Trim(),
                LinkUrl = txtLinkUrl.Text.Trim(),
                Active = chkActive.Checked
            };

            new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl).Save(unit);
            ShowMessage(IsArabic ? "تم حفظ الوحدة." : "Unit saved.");
            pnlEditor.Visible = false;
            BindGrid();
        }

        // ---------------------------------------------------------------
        // Editor helpers
        // ---------------------------------------------------------------
        private void LoadIntoEditor(int id)
        {
            OrgStructureUnit u = new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl).GetById(id);
            if (u == null) return;

            hidId.Value = u.Id.ToString(CultureInfo.InvariantCulture);
            txtOrder.Text = u.Order.ToString(CultureInfo.InvariantCulture);
            txtSelector.Text = u.Selector;
            txtTitle.Text = u.Title;
            txtTitleEn.Text = u.TitleEn;
            txtDesc.Text = u.Description;
            txtDescEn.Text = u.DescriptionEn;
            txtBadge.Text = u.Badge;
            txtBadgeEn.Text = u.BadgeEn;
            txtMeta.Text = u.Meta;
            txtMetaEn.Text = u.MetaEn;
            txtIcon.Text = u.Icon;
            SelectTheme(u.Theme);
            txtTextColor.Text = u.TextColor;
            txtLinkUrl.Text = u.LinkUrl;
            chkActive.Checked = u.Active;

            pnlEditor.Visible = true;
            BindGrid();
        }

        private void SelectTheme(string theme)
        {
            System.Web.UI.WebControls.ListItem li = ddlTheme.Items.FindByValue(theme ?? "primary");
            ddlTheme.ClearSelection();
            (li ?? ddlTheme.Items.FindByValue("primary")).Selected = true;
        }

        private void ClearEditor()
        {
            hidId.Value = "0";
            txtOrder.Text = string.Empty;
            txtSelector.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtTitleEn.Text = string.Empty;
            txtDesc.Text = string.Empty;
            txtDescEn.Text = string.Empty;
            txtBadge.Text = string.Empty;
            txtBadgeEn.Text = string.Empty;
            txtMeta.Text = string.Empty;
            txtMetaEn.Text = string.Empty;
            txtIcon.Text = string.Empty;
            SelectTheme("primary");
            txtTextColor.Text = string.Empty;
            txtLinkUrl.Text = string.Empty;
            chkActive.Checked = true;
        }

        private int NextOrder()
        {
            try
            {
                int max = 0;
                foreach (OrgStructureUnit u in new OrgStructureRepository(SPContext.Current.Web, EffectiveWebUrl).GetAllUnits())
                    if (u.Order > max) max = u.Order;
                return max + 10;
            }
            catch { return 10; }
        }

        private void ShowMessage(string msg)
        {
            litMessage.Text = msg;
            pnlMessage.Visible = true;
        }

        // ---------------------------------------------------------------
        // Labels (bilingual, set from code-behind — no markup expressions)
        // ---------------------------------------------------------------
        private void SetLabels()
        {
            bool ar = IsArabic;
            litHeader.Text = ar ? "إدارة الهيكل التنظيمي" : "Manage org structure";
            litNew.Text = ar ? "وحدة جديدة" : "New unit";
            litSave.Text = ar ? "حفظ" : "Save";
            litCancel.Text = ar ? "إلغاء" : "Cancel";

            litLblOrder.Text = ar ? "الترتيب" : "Order";
            litLblTheme.Text = ar ? "النمط" : "Theme";
            litLblIcon.Text = ar ? "الأيقونة" : "Icon";
            litLblTextColor.Text = ar ? "لون النص" : "Text color";
            litLblSelector.Text = ar ? "محدد المربع (SVG)" : "SVG selector";
            litHintSelector.Text = ar
                ? "يطابق مستطيل الهيكل في الرسم، مثل: rect[x=\"482.5\"][y=\"79.5\"][width=\"280\"][height=\"56\"]"
                : "Matches the SVG rect, e.g. rect[x=\"482.5\"][y=\"79.5\"][width=\"280\"][height=\"56\"]";
            litLblTitle.Text = ar ? "العنوان (عربي)" : "Title (AR)";
            litLblTitleEn.Text = ar ? "العنوان (إنجليزي)" : "Title (EN)";
            litLblDesc.Text = ar ? "الوصف (عربي)" : "Description (AR)";
            litLblDescEn.Text = ar ? "الوصف (إنجليزي)" : "Description (EN)";
            litLblBadge.Text = ar ? "الوسم (عربي)" : "Badge (AR)";
            litLblBadgeEn.Text = ar ? "الوسم (إنجليزي)" : "Badge (EN)";
            litLblMeta.Text = ar ? "التصنيف (عربي)" : "Meta (AR)";
            litLblMetaEn.Text = ar ? "التصنيف (إنجليزي)" : "Meta (EN)";
            litLblLink.Text = ar ? "رابط صفحة التفاصيل" : "Details page link";
            litLblActive.Text = ar ? "مُفعّل" : "Active";

            litColOrder.Text = ar ? "الترتيب" : "Order";
            litColTitle.Text = ar ? "العنوان" : "Title";
            litColBadge.Text = ar ? "الوسم" : "Badge";
            litColActive.Text = ar ? "مُفعّل" : "Active";
            litColActions.Text = ar ? "إجراءات" : "Actions";
        }

        private bool IsArabic
        {
            get
            {
                try { return PortalHelper.IsArabic; }
                catch
                {
                    return SPContext.Current != null &&
                           SPContext.Current.Web != null &&
                           SPContext.Current.Web.Language == 1025;
                }
            }
        }

        private static string SafeCaml(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private static string GetUrl()
        {
            try
            {
                return System.Web.HttpContext.Current != null
                    ? System.Web.HttpContext.Current.Request.Url.ToString()
                    : string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}
