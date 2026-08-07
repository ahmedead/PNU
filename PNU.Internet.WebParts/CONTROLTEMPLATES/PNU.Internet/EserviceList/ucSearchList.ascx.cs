using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucSearchList : UserControl
    {
        private string testParameter;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                testParameter = Request.QueryString["eti"];
                Bind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        public void Bind()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
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
                                    query.Query = "<Where><And><Eq><FieldRef Name='ARServiceName' /><Value Type='Text'>" + testParameter + "</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Student</Value></Eq></And></Where>";

                                    SPListItemCollection collitem = list.GetItems(query);
                                    if (collitem != null)
                                    {
                                        DataTable dt = collitem.GetDataTable();

                                        grdcrud.DataSource = dt;
                                        grdcrud.DataBind();
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);

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
