using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes; // SPFactory, Publics

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
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                    if (list == null) return;

                    SPQuery query = new SPQuery
                    {
                        Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>",
                        RowLimit = 200
                    };

                    SPListItemCollection collitem = list.GetItems(query);
                    if (collitem != null && collitem.Count > 0)
                    {
                        List<NavCardsList> cards = SPFactory.MapListItemsToClass<NavCardsList>(collitem);
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
    }
}
