using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;



namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.HomePage
{
    public partial class ucOtherKnowMore : UserControl
    {
        public string FacultyName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (FacultyName == null || FacultyName == "")
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
                                            FacultyName = collitem[0]["Title"].ToString();
                                        }
                                    }
                                }
                            }
                        });
                    }

                    BindColleges();



                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

           
        }

        private void BindColleges()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    //string newPortalURL = "https://newportal.pnu.edu.sa/ar/Faculties";
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    //using (SPSite site = new SPSite(newPortalURL))
                    {
                        using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "Faculties"))
                        {
                            SPList list = web.Lists["Faculties"];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = @"<Where>
                                          <Neq>
                                             <FieldRef Name='FacultyName' />
                                             <Value Type='Text'>" + FacultyName + @"</Value>
                                          </Neq>
                                       </Where>";
                                query.RowLimit = 3;
                                SPListItemCollection collitem = list.GetItems(query);
                                if (collitem != null)
                                {
                                    rptColleges.DataSource = collitem.GetDataTable();
                                    rptColleges.DataBind();
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
}
