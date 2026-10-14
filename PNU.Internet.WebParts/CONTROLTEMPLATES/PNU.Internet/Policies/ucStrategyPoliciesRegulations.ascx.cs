using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    public partial class ucStrategyPoliciesRegulations : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                if (SPContext.Current != null &&
                    SPContext.Current.Web != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    StrategyPoliciesProvisioner.EnsureLists();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindSections();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindSections()
        {
            try
            {
                bool isArabic = IsArabicContext();

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                {
                    if (web == null) return;

                    List<StrategyPoliciesSection> sections = StrategyPoliciesProvisioner.ReadSectionsWithCards(web, isArabic);
                    if (sections != null && sections.Count > 0)
                    {
                        rptSections.DataSource = sections;
                        rptSections.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected void rptSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                StrategyPoliciesSection section = e.Item.DataItem as StrategyPoliciesSection;
                if (section != null && section.Cards != null)
                {
                    Repeater rptCards = e.Item.FindControl("rptCards") as Repeater;
                    if (rptCards != null)
                    {
                        rptCards.DataSource = section.Cards;
                        rptCards.DataBind();
                    }
                }
            }
        }

        private bool IsArabicContext()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    if (IsEnglishPath(HttpContext.Current.Request.Url.AbsolutePath))
                        return false;
                }

                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    if (IsEnglishPath(SPContext.Current.Web.ServerRelativeUrl))
                        return false;

                    if (SPContext.Current.Web.Language == 1033)
                        return false;
                }

                if (!Portal.Main.Helper.PortalHelper.IsArabic)
                    return false;
            }
            catch { }

            return true;
        }

        // Matches an "/en" path segment only (e.g. /en/AboutUniversity), not /ar/Endowment
        private static bool IsEnglishPath(string path)
        {
            string p = (path ?? string.Empty).ToLowerInvariant();
            return p.Contains("/en/") || p.EndsWith("/en");
        }

        #region Rendering helpers

        public static string Text(object value) { return SafeHtml.Text(value); }
        public static string Attr(object value) { return SafeHtml.Attr(value); }
        public static string Href(object value) { return SafeHtml.Href(value); }
        public static string Target(object value) { return SafeHtml.Target(value); }

        public static string RenderBadge(object badgeText, object badgeClass)
        {
            string text = badgeText != null ? badgeText.ToString().Trim() : string.Empty;
            if (string.IsNullOrEmpty(text)) return string.Empty;

            string cls = badgeClass != null && !string.IsNullOrEmpty(badgeClass.ToString().Trim())
                ? badgeClass.ToString().Trim()
                : "badge-warning";

            // Ensure the base "badge" class is present (e.g. "badge-warning" -> "badge badge-warning")
            if (Array.IndexOf(cls.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), "badge") < 0)
                cls = "badge " + cls;

            return "<span class=\"" + SafeHtml.Attr(cls) + "\">" + SafeHtml.Text(text) + "</span>";
        }

        #endregion
    }
}
