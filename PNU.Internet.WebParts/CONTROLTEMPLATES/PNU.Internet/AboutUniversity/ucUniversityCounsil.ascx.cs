using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucUniversityCounsil : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                LoadData();
                
            }
        }

        private void LoadData()
        {
            try
            {
                List<UniversityCounsil> data = new List<UniversityCounsil>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("ar/AboutUniversity/"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = @"   <Where>
                               <Eq>
                                  <FieldRef Name='Visibility' />
                                  <Value Type='Boolean'>1</Value>
                               </Eq>
                            </Where>
                            <OrderBy>
                               <FieldRef Name='ItemOrder' Ascending='True' />
                            </OrderBy>
                         </Query>";

                            SPList reqList = web.Lists.TryGetList("UniversityCounsil");
                            SPListItemCollection objNew = reqList.GetItems(query);
                            if (objNew != null && objNew.Count > 0)
                                data = SPFactory.MapListItemsToClass<UniversityCounsil>(objNew);
                        }
                    }
                });

                rptMainData.DataSource = data;
                rptMainData.DataBind();


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
        }

        private class UniversityCounsil
        {
            public string ID { get; set; } = "";
            public string Title { get; set; } = "";
            public string TitleEn { get; set; } = "";
            public string NameAr { get; set; } = "";
            public string NameEn { get; set; } = "";
            public string PositionAr { get; set; } = "";
            public string PositionEn { get; set; } = "";
            public string ClassName { get; set; } = "";

        }
    }
}
