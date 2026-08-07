using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucHomeStatisticsNew : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SPListItemCollection coll;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList("Statistics");


                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
                                          <Eq>
                                             <FieldRef Name='ShowOnHome' />
                                             <Value Type='Boolean'>1</Value>
                                          </Eq>
                                       </Where>
                                       <OrderBy>
                                          <FieldRef Name='ItemOrder' Ascending='True' />
                                       </OrderBy>";
                            SPListItemCollection collNew = list.GetItems(query);
                            if (collNew != null && collNew.Count > 0)
                            {
                                List<lstStatistics> _AllData = SPFactory.MapListItemsToClass<lstStatistics>(collNew);
                                if (_AllData != null)
                                {
                                    rptStatistics.DataSource = _AllData;
                                    rptStatistics.DataBind();
                                }
                            }




                        }
                    }


                });

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

    
    }

    

}



