using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucAllCollegesHome : UserControl
    {
        public string ListName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string siteURL = SPFactory.GetSiteURL();
                    if (!string.IsNullOrEmpty(siteURL))
                    {
                        allCollegesLink.Attributes["href"] = string.Format("{0}Faculties/Pages/AllCollegesNew.aspx", siteURL);
                    }
                    else
                    {
                        // Handle the case where siteURL is null or empty
                        allCollegesLink.Attributes["href"] = "#"; // or some default action
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
                List<AllFaculties> data = new List<AllFaculties>();



                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                                              <Eq>
                                                 <FieldRef Name='Type' />
                                                 <Value Type='Choice'>كلية</Value>
                                              </Eq>
                                           </Where>");

                            SPList reqList = web.Lists.TryGetList("AllFaculties");
                            SPListItemCollection collitem = reqList.GetItems(query);
                            if (collitem != null && collitem.Count > 0)
                                data = SPFactory.MapListItemsToClass<AllFaculties>(collitem);

                            if (data != null && data.Count > 0)
                            {
                                rptColleges1.DataSource = data;
                                rptColleges1.DataBind();
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




        protected List<AllFaculties> LoadData()
        {
            try
            {//ArrayList qryParam = new ArrayList();
             //qryParam.Add("<Eq><FieldRef Name='Category'  /><Value Type='Choice'>" + Category + "</Value></Eq>");
                SPListItemCollection allItems = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, "Admin", ListName, "ItemOrder", "TRUE");
                if (allItems == null || allItems.Count == 0)
                    return null;
                return SPFactory.MapListItemsToClass<AllFaculties>(allItems);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return null;
            }
            



        }

    }
}
