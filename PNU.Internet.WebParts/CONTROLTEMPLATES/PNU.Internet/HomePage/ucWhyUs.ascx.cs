using Microsoft.SharePoint;
using Microsoft.Web.Hosting.Administration;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucWhyUs : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindWhyUsData();
            }
        }

        private void BindWhyUsData()
        {
            try
            {
                List<WhyUsMain> MainItem = new List<WhyUsMain>();

                List<WhyUsItem> items = new List<WhyUsItem>();





                SPListItemCollection coll;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("ar"))
                        {
                            SPList Mainlist = web.Lists.TryGetList("WhyUsMain");

                            SPList list = web.Lists.TryGetList("WhyUsDetails");
                            SPListItemCollection collMain = Mainlist.GetItems();

                            foreach (SPListItem item in collMain)
                            {
                                MainItem.Add(new WhyUsMain
                                {
                                    Title = Convert.ToString(item["Title"]),
                                    Title_EN = Convert.ToString(item["Title_EN"]),
                                     Desc = item["Desc"] != null ? item["Desc"].ToString(): "" ,
                                    Desc_EN = item["Desc_EN"] != null ? item["Desc_EN"].ToString()  :""

                                });
                            }
                            rptWhyUsMain.DataSource = MainItem;
                            rptWhyUsMain.DataBind();

                            SPQuery query = new SPQuery();
                            query.Query = $@"
                                       <OrderBy>
                                          <FieldRef Name='ItemOrder' Ascending='True' />
                                       </OrderBy>";
                            SPListItemCollection collNew = list.GetItems(query);
                            foreach (SPListItem item in collNew)
                            {
                                items.Add(new WhyUsItem
                                {
                                    Title = Convert.ToString(item["Title"]),
                                    Title_EN = Convert.ToString(item["Title_EN"]),
                                    // Fallback icon if the field is empty
                                    IconID = item["IconID"] != null ? item["IconID"].ToString() : "#path3780",
                                    IconClass = item["IconClass"] != null ? item["IconClass"].ToString() : "hgi-books-02 fs-3"
                                });
                            }

                            rptWhyUsDetails.DataSource = items;
                            rptWhyUsDetails.DataBind();




                        }
                    }


                });




                
            }
            catch (Exception ex)
            {
                // Log error (consider using SharePoint's ULS logs)
                // DiagnosticsService.LogException(ex);
            }
        }
    
    }

    public class WhyUsMain
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string Desc { get; set; }
        public string Desc_EN { get; set; }
    }
    public class WhyUsItem
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string IconID { get; set; } // e.g., "#path3780"
        public string IconClass { get; set; } 
    }

}
