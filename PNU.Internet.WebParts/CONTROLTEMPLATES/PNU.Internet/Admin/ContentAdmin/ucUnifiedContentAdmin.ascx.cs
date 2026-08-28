using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin
{
    public enum AdminTemplateLayout
    {
        SidebarWorkspace = 1,
        CardsDashboard = 2
    }

    public partial class ucUnifiedContentAdmin : UserControl
    {
        #region Configurable Properties

        [Browsable(true)]
        [Category("PNU Admin Configuration")]
        [Description("Name of the SharePoint list storing authorized admin users (Default: AdminUsers)")]
        public string AdminListName { get; set; }

        [Browsable(true)]
        [Category("PNU Admin Configuration")]
        [Description("Server-relative web URL where the admin list is located (Default: /ar/ContentAdmin)")]
        public string AdminWebUrl { get; set; }

        [Browsable(true)]
        [Category("PNU Admin Configuration")]
        [Description("Default target web for content lists (Default: /ar/ContentAdmin)")]
        public string TargetSiteUrl { get; set; }

        [Browsable(true)]
        [Category("PNU Admin Configuration")]
        [Description("Default Layout: SidebarWorkspace (Template 1) or CardsDashboard (Template 2)")]
        public AdminTemplateLayout LayoutTemplate { get; set; }

        #endregion

        public ucUnifiedContentAdmin()
        {
            AdminListName = "AdminUsers";
            AdminWebUrl = "";
            TargetSiteUrl = "";
            LayoutTemplate = AdminTemplateLayout.SidebarWorkspace;
        }

        private string EffectiveAdminWebUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(AdminWebUrl))
                    return AdminWebUrl;
                if (SPContext.Current != null && SPContext.Current.Web != null)
                    return SPContext.Current.Web.ServerRelativeUrl;
                return "/ar/ContentAdmin";
            }
        }

        #region State Resolution for ViewState Stability

        private string _activeModuleKey;
        private string ActiveModuleKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_activeModuleKey))
                    return _activeModuleKey;
                _activeModuleKey = ResolveActiveModuleKey();
                return _activeModuleKey;
            }
            set { _activeModuleKey = value; }
        }

        private AdminTemplateLayout _currentLayout;
        private bool _layoutResolved;
        private AdminTemplateLayout CurrentLayout
        {
            get
            {
                if (!_layoutResolved)
                {
                    _currentLayout = ResolveCurrentLayout();
                    _layoutResolved = true;
                }
                return _currentLayout;
            }
            set
            {
                _currentLayout = value;
                _layoutResolved = true;
            }
        }

        private bool IsDetailViewInCards
        {
            get
            {
                try
                {
                    if (Page != null && Page.IsPostBack && hfIsDetailView != null)
                    {
                        string posted = Request.Form[hfIsDetailView.UniqueID];
                        if (!string.IsNullOrEmpty(posted))
                            return posted == "1";
                    }
                }
                catch { }

                string q = Request != null ? Request.QueryString["detail"] : null;
                return q == "1";
            }
            set
            {
                if (hfIsDetailView != null)
                    hfIsDetailView.Value = value ? "1" : "0";
            }
        }

        private string ResolveActiveModuleKey()
        {
            try
            {
                if (Page != null && Page.IsPostBack && hfActiveModule != null)
                {
                    string posted = Request.Form[hfActiveModule.UniqueID];
                    if (!string.IsNullOrEmpty(posted) && AdminModuleRegistry.GetModules(EffectiveAdminWebUrl).Any(m => m.Key == posted))
                        return posted;
                }
            }
            catch { }

            string q = Request != null ? Request.QueryString["module"] : null;
            if (!string.IsNullOrEmpty(q) && AdminModuleRegistry.GetModules(EffectiveAdminWebUrl).Any(m => m.Key == q))
                return q;

            var first = AdminModuleRegistry.GetModules(EffectiveAdminWebUrl).FirstOrDefault();
            return first != null ? first.Key : "AboutPnu";
        }

        private AdminTemplateLayout ResolveCurrentLayout()
        {
            try
            {
                if (Page != null && Page.IsPostBack)
                {
                    if (ddlLayoutSelector != null)
                    {
                        string ddlVal = Request.Form[ddlLayoutSelector.UniqueID];
                        AdminTemplateLayout chosen;
                        if (!string.IsNullOrEmpty(ddlVal) && Enum.TryParse(ddlVal, out chosen))
                            return chosen;
                    }

                    if (hfCurrentLayout != null)
                    {
                        string posted = Request.Form[hfCurrentLayout.UniqueID];
                        AdminTemplateLayout chosen;
                        if (!string.IsNullOrEmpty(posted) && Enum.TryParse(posted, out chosen))
                            return chosen;
                    }
                }
            }
            catch { }

            string q = Request != null ? Request.QueryString["layout"] : null;
            AdminTemplateLayout qLayout;
            if (!string.IsNullOrEmpty(q) && Enum.TryParse(q, out qLayout))
                return qLayout;

            return LayoutTemplate;
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // 1. Ensure ContentAdmin dynamic lists & configuration list exist
            ContentAdminListProvisioner.EnsureAllListsExist(EffectiveAdminWebUrl);

            // 2. Authorize user against the configurable AdminListName & AdminWebUrl
            bool isAuthorized = CheckUserAuthorization();
            phDenied.Visible = !isAuthorized;
            phMain.Visible = isAuthorized;

            if (!isAuthorized)
            {
                ltrAdminListName.Text = AdminListName;
                ltrAdminWebPath.Text = EffectiveAdminWebUrl;
                return;
            }

            // 3. Mount matching child control in OnInit before LoadViewState is called
            MountActiveChildControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!phMain.Visible) return;

            if (!IsPostBack)
            {
                BindWebsDropdown();
                BindLayoutSelector();
            }

            BindModulesNavigation();
            RenderTemplateView();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (phMain != null && phMain.Visible)
            {
                if (hfActiveModule != null) hfActiveModule.Value = ActiveModuleKey ?? "AboutPnu";
                if (hfCurrentLayout != null) hfCurrentLayout.Value = CurrentLayout.ToString();
                if (hfIsDetailView != null) hfIsDetailView.Value = IsDetailViewInCards ? "1" : "0";
            }
        }

        #region Authorization Logic

        private bool CheckUserAuthorization()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return false;
                SPUser user = SPContext.Current.Web.CurrentUser;
                if (user == null) return false;

                // Site collection administrators are always authorized
                if (user.IsSiteAdmin) return true;

                int userId = user.ID;
                string userName = user.Name;
                Guid siteId = SPContext.Current.Site.ID;
                bool allowed = false;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        string adminRelativeUrl = EffectiveAdminWebUrl.StartsWith("/") ? EffectiveAdminWebUrl : "/" + EffectiveAdminWebUrl;
                        using (SPWeb adminWeb = site.OpenWeb(adminRelativeUrl))
                        {
                            if (adminWeb == null || !adminWeb.Exists) return;

                            SPList list = null;
                            try { list = adminWeb.Lists[AdminListName]; }
                            catch { }

                            if (list == null) return;

                            string safeName = System.Security.SecurityElement.Escape(userName ?? string.Empty);
                            string safeUserId = userId.ToString(System.Globalization.CultureInfo.InvariantCulture);

                            // 1. If using the default AdminUsers list, reuse the proven ContentAdm logic
                            if (string.Equals(AdminListName, ContentAdm.AdminUsersList, StringComparison.OrdinalIgnoreCase))
                            {
                                allowed = ContentAdm.IsListedAdmin(adminWeb, userId, userName);
                            }
                            else
                            {
                                // 2. Generic query for custom admin lists
                                SPQuery query = new SPQuery
                                {
                                    RowLimit = 10,
                                    Query = string.Format(
                                        "<Where><Or><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>" +
                                        "<Eq><FieldRef Name='User' LookupId='TRUE' /><Value Type='Integer'>{1}</Value></Eq></Or></Where>",
                                        safeName, safeUserId)
                                };

                                SPListItemCollection items = list.GetItems(query);
                                allowed = (items != null && items.Count > 0);
                            }
                        }
                    }
                });

                return allowed;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucUnifiedContentAdmin.CheckAuth", ex.Message);
                return false;
            }
        }

        #endregion

        #region Dynamic Control Mounting

        private void MountActiveChildControl()
        {
            PlaceHolder targetHost = (CurrentLayout == AdminTemplateLayout.SidebarWorkspace)
                ? phControlHost_Sidebar
                : phControlHost_Cards;

            if (targetHost == null) return;
            targetHost.Controls.Clear();

            var modules = AdminModuleRegistry.GetModules(EffectiveAdminWebUrl);
            var module = modules.FirstOrDefault(m => m.Key == ActiveModuleKey) ?? modules.FirstOrDefault();
            if (module == null) return;

            try
            {
                // Auto-provision lists for the module if required
                string autoProv = ContentAdminListProvisioner.GetConfigValue("AutoProvisionOnLoad", "true", EffectiveAdminWebUrl);
                if (string.Equals(autoProv, "true", StringComparison.OrdinalIgnoreCase) && module.ProvisionAction != null)
                {
                    module.ProvisionAction.Invoke();
                }

                // Load existing child user control untouched
                Control ctrl = Page.LoadControl(module.ControlPath);
                ctrl.ID = "ctrl_" + module.Key;
                targetHost.Controls.Add(ctrl);
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                ltrAlertMessage.Text = "<strong>تعذر تحميل لوحة التحكم:</strong> " + Server.HtmlEncode(ex.Message);
            }
        }

        #endregion

        #region Data Binding & UI Helpers

        private void BindWebsDropdown()
        {
            ddlTargetWeb.Items.Clear();
            if (SPContext.Current == null) return;

            foreach (SPWeb w in SPContext.Current.Site.AllWebs)
            {
                try
                {
                    string label = string.IsNullOrEmpty(w.Title) ? w.ServerRelativeUrl : string.Format("{0} ({1})", w.Title, w.ServerRelativeUrl);
                    ddlTargetWeb.Items.Add(new ListItem(label, w.ServerRelativeUrl));
                }
                finally { w.Dispose(); }
            }

            string currentVal = !string.IsNullOrEmpty(TargetSiteUrl) ? TargetSiteUrl : SPContext.Current.Web.ServerRelativeUrl;
            string qTarget = Request != null ? Request.QueryString["target"] : null;
            if (!string.IsNullOrEmpty(qTarget)) currentVal = qTarget;

            ListItem item = ddlTargetWeb.Items.FindByValue(currentVal);
            if (item != null) ddlTargetWeb.SelectedValue = currentVal;
        }

        private void BindLayoutSelector()
        {
            ddlLayoutSelector.SelectedValue = CurrentLayout.ToString();
        }

        private void BindModulesNavigation()
        {
            var modules = AdminModuleRegistry.GetModules(EffectiveAdminWebUrl);
            ltrModuleCount.Text = modules.Count.ToString();

            rptSidebarModules.DataSource = modules;
            rptSidebarModules.DataBind();

            rptCards.DataSource = modules;
            rptCards.DataBind();
        }

        private void RenderTemplateView()
        {
            phTemplateSidebar.Visible = (CurrentLayout == AdminTemplateLayout.SidebarWorkspace);
            phTemplateCards.Visible = (CurrentLayout == AdminTemplateLayout.CardsDashboard);

            var modules = AdminModuleRegistry.GetModules(EffectiveAdminWebUrl);
            var module = modules.FirstOrDefault(m => m.Key == ActiveModuleKey) ?? modules.FirstOrDefault();
            string title = module != null ? module.TitleAr : "لوحة الإدارة";
            string category = module != null ? module.Category : string.Empty;
            string webUrl = ddlTargetWeb.SelectedValue ?? TargetSiteUrl;

            if (CurrentLayout == AdminTemplateLayout.SidebarWorkspace)
            {
                ltrActiveTitle.Text = title;
                ltrActiveCategory.Text = category;
                lblTargetWebBadge.Text = "الموقع: " + webUrl;
            }
            else
            {
                phCardsOverview.Visible = !IsDetailViewInCards;
                phCardsDetailView.Visible = IsDetailViewInCards;
                ltrCardsDetailTitle.Text = title;
                lblCardsDetailBadge.Text = "الموقع: " + webUrl;
            }
        }

        public string GetSidebarNavClass(string moduleKey)
        {
            return moduleKey == ActiveModuleKey ? "nav-link active" : "nav-link";
        }

        private void NavigateToState(string moduleKey, AdminTemplateLayout layout, bool isDetail, string targetWeb)
        {
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Path);
            string targetParam = !string.IsNullOrEmpty(targetWeb) ? targetWeb : (ddlTargetWeb != null ? ddlTargetWeb.SelectedValue : TargetSiteUrl);
            string query = string.Format("?module={0}&layout={1}&detail={2}&target={3}",
                HttpUtility.UrlEncode(moduleKey),
                layout.ToString(),
                isDetail ? "1" : "0",
                HttpUtility.UrlEncode(targetParam));

            Response.Redirect(baseUrl + query, false);
            if (Context != null && Context.ApplicationInstance != null)
            {
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion

        #region Event Handlers

        protected void ddlLayoutSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminTemplateLayout layout;
            if (Enum.TryParse(ddlLayoutSelector.SelectedValue, out layout))
            {
                NavigateToState(ActiveModuleKey, layout, false, ddlTargetWeb.SelectedValue);
            }
        }

        protected void ddlTargetWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            TargetSiteUrl = ddlTargetWeb.SelectedValue;
            NavigateToState(ActiveModuleKey, CurrentLayout, IsDetailViewInCards, TargetSiteUrl);
        }

        protected void rptSidebarModules_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectModule")
            {
                string key = e.CommandArgument.ToString();
                NavigateToState(key, CurrentLayout, false, ddlTargetWeb.SelectedValue);
            }
        }

        protected void rptCards_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string key = e.CommandArgument.ToString();
            if (e.CommandName == "OpenModule")
            {
                NavigateToState(key, AdminTemplateLayout.CardsDashboard, true, ddlTargetWeb.SelectedValue);
            }
            else if (e.CommandName == "ProvisionModule")
            {
                ExecuteProvisioning(key);
            }
        }

        protected void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            NavigateToState(ActiveModuleKey, AdminTemplateLayout.CardsDashboard, false, ddlTargetWeb.SelectedValue);
        }

        protected void btnProvisionActive_Click(object sender, EventArgs e)
        {
            ExecuteProvisioning(ActiveModuleKey);
        }

        private void ExecuteProvisioning(string moduleKey)
        {
            var module = AdminModuleRegistry.GetModules(EffectiveAdminWebUrl).FirstOrDefault(m => m.Key == moduleKey);
            if (module != null && module.ProvisionAction != null)
            {
                try
                {
                    module.ProvisionAction.Invoke();
                    pnlAlert.CssClass = "alert alert-success alert-dismissible fade show rounded-3 shadow-sm";
                    pnlAlert.Visible = true;
                    ltrAlertMessage.Text = string.Format("تم التحقق من قوائم ({0}) وتهيئتها بنجاح.", module.TitleAr);
                }
                catch (Exception ex)
                {
                    pnlAlert.CssClass = "alert alert-danger alert-dismissible fade show rounded-3 shadow-sm";
                    pnlAlert.Visible = true;
                    ltrAlertMessage.Text = "خطأ أثناء التهيئة: " + Server.HtmlEncode(ex.Message);
                }
            }
        }

        #endregion
    }
}
