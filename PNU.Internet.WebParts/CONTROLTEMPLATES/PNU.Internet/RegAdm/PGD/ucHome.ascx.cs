using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD
{
    public partial class ucHome : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    
                    List<clsAdmissionServices> _AllItemsServices = new List<clsAdmissionServices>();
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList lstServices = web.Lists["scholarships"];
                                if (lstServices != null)
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>";

                                    SPListItemCollection collitem = lstServices.GetItems(query);

                                    if (collitem != null && collitem.Count > 0)
                                    {
                                        _AllItemsServices = SPFactory.MapListItemsToClass<clsAdmissionServices>(collitem);

                                    }


                                }
                            }
                        }
                    });

                    

                    rptServices.DataSource = _AllItemsServices;
                    rptServices.DataBind();



                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

    }
}
