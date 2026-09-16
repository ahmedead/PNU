using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Emits the kit specific CSS once per request, even when more than one kit control
    /// is placed on the same page.
    /// </summary>
    public partial class ucPgStyles : UserControl
    {
        private const string RenderedKey = "PgStylesRendered";

        public bool IsFarmAccount { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsFarmAccount = CheckIfFarmAccount();

            if (phStyles == null) return;

            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx == null) return;

                if (ctx.Items.Contains(RenderedKey))
                {
                    phStyles.Visible = false;
                    return;
                }

                ctx.Items[RenderedKey] = true;
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgStyles.Page_Load", ex);
            }
        }

        private static bool CheckIfFarmAccount()
        {
            try
            {
                if (HttpContext.Current == null) return false;
                SPContext spCtx = SPContext.Current;
                if (spCtx == null || spCtx.Web == null) return false;

                SPUser user = spCtx.Web.CurrentUser;
                if (user == null) return false;

                string login = (user.LoginName ?? string.Empty).ToLowerInvariant();

                // 1. SharePoint System Account (e.g. SHAREPOINT\system or claims equivalent)
                if (login.Contains("system") || login.Contains("sharepoint\\system"))
                    return true;

                // 2. Compare with Site SystemAccount
                if (spCtx.Site != null && spCtx.Site.SystemAccount != null)
                {
                    if (user.ID == spCtx.Site.SystemAccount.ID ||
                        string.Equals(login, (spCtx.Site.SystemAccount.LoginName ?? string.Empty).ToLowerInvariant(), StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                // 3. Compare with SPFarm DefaultServiceAccount or Farm Administrator
                try
                {
                    if (Microsoft.SharePoint.Administration.SPFarm.Local != null)
                    {
                        if (Microsoft.SharePoint.Administration.SPFarm.Local.CurrentUserIsAdministrator())
                            return true;

                        var defaultServiceAccount = Microsoft.SharePoint.Administration.SPFarm.Local.DefaultServiceAccount;
                        if (defaultServiceAccount != null)
                        {
                            string farmAccName = (defaultServiceAccount.LookupName() ?? string.Empty).ToLowerInvariant();
                            if (!string.IsNullOrEmpty(farmAccName) && login.Contains(farmAccName))
                                return true;

                            string farmAccStr = (defaultServiceAccount.ToString() ?? string.Empty).ToLowerInvariant();
                            if (!string.IsNullOrEmpty(farmAccStr) && login.Contains(farmAccStr))
                                return true;
                        }
                    }
                }
                catch { }
            }
            catch { }

            return false;
        }
    }
}
