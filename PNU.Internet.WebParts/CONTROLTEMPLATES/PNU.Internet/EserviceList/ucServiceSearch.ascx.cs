using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucServiceSearch : UserControl
    {
        private string URParameter;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                URParameter = Request.QueryString["eti"];
                Bindservices();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        private void Bindservices()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            //query.Query = "<Where><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></Where>";
                            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Contains><FieldRef Name='ARServiceName' /><Value Type='Text'>" + URParameter + "</Value></Contains></And></Where>";

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                rptprod.DataSource = collitem.GetDataTable();
                                rptprod.DataBind();
                                // stuN = collitem.Count;

                                //   lblStNser.Text = stuN.ToString();


                                string[] array = new string[collitem.Count];

                                foreach (SPListItem item1 in collitem)
                                {




                                }


                                // Session["PaID"] = collitem[]
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
