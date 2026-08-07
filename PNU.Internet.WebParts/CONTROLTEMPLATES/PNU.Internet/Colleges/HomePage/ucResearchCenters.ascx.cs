using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.HomePage
{
    public partial class ucResearchCenters : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["AboutCollege"];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                SPQuery query2 = new SPQuery();


                                SPListItemCollection collitem = list.GetItems();
                                //SPListItemCollection collitem2 = null;
                                if (collitem != null)
                                {

                                    BindScientificResearch(collitem[0]["Title"].ToString());

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

        private void BindScientificResearch(string FacultyName)
        {
            try
            {//string newPortalURL = "https://newportal.pnu.edu.sa/ar/SR/";
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                //using (SPSite site = new SPSite(newPortalURL))
                {
                    using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "SR/"))
                    {
                        SPList list = web.Lists["ResearchCenters"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            SPQuery query2 = new SPQuery();

                            query.Query = @"<Where>
                              <Eq>
                                 <FieldRef Name='Faculty' />
                                 <Value Type='Text'>" + FacultyName + @"</Value>
                              </Eq>
                           </Where>";

                            SPListItemCollection collitem = list.GetItems(query);
                            SPListItemCollection collitem2 = null;
                            if (collitem != null)
                            {

                                DataTable dtData = collitem2 != null ? collitem2.GetDataTable() : collitem.GetDataTable();

                                rptcenters.DataSource = dtData;
                                rptcenters.DataBind();

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
