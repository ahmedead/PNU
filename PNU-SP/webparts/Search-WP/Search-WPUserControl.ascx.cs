using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.Search_WP
{
    public partial class Search_WPUserControl : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            gvResult.Visible = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPSiteDataQuery dataQuery = new SPSiteDataQuery();
                    dataQuery.Webs = "<Webs Scope=\"SiteCollection\">";
                    //dataQuery.Lists = "<Lists ServerTemplate=\"0\" />";
                    dataQuery.Lists = "<Lists BaseType=\"0\" />";
                    dataQuery.ViewFields = "<FieldRef Name=\"Title\" />";
                    string where = "<Where><Eq>";
                    where += "<FieldRef Name=\"Content\"/>";
                    where += "<Value Type='Text'>" + txtEmpId.Text + "</Value>";
                    where += "</Eq></Where>";

                    dataQuery.Query = where;
                    DataTable dt = web.GetSiteData(dataQuery);
                    DataView dv = new DataView(dt);
                    gvResult.DataSource = dv;
                    gvResult.DataBind();
                    gvResult.Visible = true;
                }

            }
        }
    }
}
