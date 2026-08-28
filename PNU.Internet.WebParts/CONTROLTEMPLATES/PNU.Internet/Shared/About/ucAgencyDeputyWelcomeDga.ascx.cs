using System;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    public partial class ucAgencyDeputyWelcomeDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                SPContext.Current != null &&
                SPContext.Current.Web.CurrentUser != null)
                    SharedTitles.EnsureList(SPContext.Current.Web);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyDeputyWelcomeDga.Page_Init", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        private void BindData()
        {
            try
            {
                var titles = SharedTitleReader.Load(IsArabic);
                string heading      = SharedTitleReader.Get(titles, SharedTitles.DeputyHeading);
                string body         = SharedTitleReader.Get(titles, SharedTitles.DeputyBody);
                string roleTitle    = SharedTitleReader.Get(titles, SharedTitles.DeputyRoleTitle);
                string roleSubtitle = SharedTitleReader.Get(titles, SharedTitles.DeputyRoleSubtitle);

                bool hasData = !string.IsNullOrWhiteSpace(heading) ||
                               !string.IsNullOrWhiteSpace(body) ||
                               !string.IsNullOrWhiteSpace(roleTitle) ||
                               !string.IsNullOrWhiteSpace(roleSubtitle);

                if (secDeputyWelcome != null)
                    secDeputyWelcome.Visible = hasData;

                if (hasData)
                {
                    litDeputyHeading.Text      = heading;
                    litDeputyBody.Text         = body;
                    litDeputyRoleTitle.Text    = roleTitle;
                    litDeputyRoleSubtitle.Text = roleSubtitle;
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyDeputyWelcomeDga.BindData", ex.Message);
            }
        }
    }
}
