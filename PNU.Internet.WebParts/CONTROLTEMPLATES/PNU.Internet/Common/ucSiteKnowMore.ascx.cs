using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common
{
    public partial class ucSiteKnowMore : UserControl
    {
        public string ListName { get; set; }
        public string WebPartTitle { get; set; }

        public string Category { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lblWPTitle.Text = WebPartTitle;
                    BindKnowMore();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            if (ListName == null || ListName == "")
                ListName = "KnowMore";
            if (WebPartTitle == null || WebPartTitle == "")
                WebPartTitle = "تعرف أيضًا على";


            
        }
        private void BindKnowMore()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists[ListName];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Visibility' />
                                        <Value Type='Boolean'>1</Value>
                                     </Eq>
                                     <Eq>
                                        <FieldRef Name='Category' />
                                        <Value Type='Text'>" + Category + @"</Value>
                                     </Eq>
                                  </And>
                               </Where>
                               <OrderBy>
                                  <FieldRef Name='ItemOrder' Ascending='True' />
                               </OrderBy>";

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {
                                rep.DataSource = collitem.GetDataTable();
                                rep.DataBind();
                            }

                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

    }
}
