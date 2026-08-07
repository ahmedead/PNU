using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace EserviceList.ServiceSearch
{
    public partial class ServiceSearchUserControl : UserControl
    {
        private string URParameter;
        protected void Page_Load(object sender, EventArgs e)
        {
            URParameter = Request.QueryString["eti"];
            Bindservices();
        }
        private void Bindservices()
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
    
    }
}
