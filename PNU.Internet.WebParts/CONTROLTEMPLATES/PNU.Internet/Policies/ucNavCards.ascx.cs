using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes; // SPFactory, Publics, SafeHtml

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies
{
    public partial class ucNavCards : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                // Provision + seed only for authenticated users; anonymous users just read.
                // Guarded once per process, so this is a no-op after the first call.
                if (SPContext.Current != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    NavCardsProvisioner.EnsureList();
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
                    BindCards();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindCards()
        {
            try
            {
                bool isArabic = SPContext.Current == null || SPContext.Current.Web == null || SPContext.Current.Web.Language == 1025;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                {
                    if (web == null) return;
                    SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                    if (list == null) return;

                    List<NavCardsList> cards = NavCardsProvisioner.ReadCards(list, isArabic);
                    if (cards.Count > 0)
                    {
                        rptCards.DataSource = cards;
                        rptCards.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        #region Rendering helpers (encoded output)

        public static string Text(object value) { return SafeHtml.Text(value); }
        public static string Attr(object value) { return SafeHtml.Attr(value); }
        public static string Href(object value) { return SafeHtml.Href(value); }
        public static string Target(object value) { return SafeHtml.Target(value); }

        #endregion
    }
}
