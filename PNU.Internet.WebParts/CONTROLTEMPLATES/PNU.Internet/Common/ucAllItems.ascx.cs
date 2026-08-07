using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common
{
    public partial class ucAllItems : UserControl
    {
        public string ListName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ListName == null || ListName == "")
                    ListName = "AllItems";
                if (!Page.IsPostBack)
                {
                    BindKnowMore();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        private void BindKnowMore()
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList requestsList = web.Lists[ListName];

                                SPQuery query = new SPQuery();
                                query.Query = string.Concat(
                                     @"<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>");
                                SPListItemCollection items = requestsList.GetItems(query);

                                if (items != null && items.Count > 0)
                                {
                                    List<AllItems> _AllItems = SPFactory.MapListItemsToClass<AllItems>(items);

                                    rep.DataSource = _AllItems;
                                    rep.DataBind();
                                }

                            }
                        }
                    }
                });


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }


        }

    }

    public class AllItems
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string DisplayImageURL
        {

            get
            {
                if (ImageUrl == "")
                    return "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg";
                else
                    return ImageUrl;
            }
        }
    }
}
