using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SR
{
    public partial class ucKnowMore : UserControl
    {
        public string ListName { get; set; }
        public string WebPartTitle { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ListName = "KnowMore";
                WebPartTitle = "تعرف أيضًا على";

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
            
        }
        private void BindKnowMore()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "SR/"))
                    {
                        SPList list = web.Lists[ListName];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = "<Query /> <ViewFields> <FieldRef Name='ImageLink' /> <FieldRef Name='URL' /> <FieldRef Name='Title' /> </ViewFields> <QueryOptions />a";

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
