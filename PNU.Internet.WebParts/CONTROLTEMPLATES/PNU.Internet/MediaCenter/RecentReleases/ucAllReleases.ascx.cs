using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.RecentReleases
{
    public partial class ucAllReleases : UserControl
    {
        private int iPageSize = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindScientificResearch();
                    //AllSectionsDDL.Items.Insert(0, "Select");
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        private void BindScientificResearch()
        {
            try
            {//string newPortalURL = "https://newportal.pnu.edu.sa/ar/MediaCenter/versions/";
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                // using (SPSite site = new SPSite(newPortalURL))
                {
                    using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "MediaCenter/RecentReleases/"))
                    {
                        SPList list = web.Lists["RecentReleases"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            SPQuery query2 = new SPQuery();

                            query.Query = "<Query> <OrderBy> <FieldRef Name='Created' Ascending='False' /> </OrderBy> </Query> <ViewFields> <FieldRef Name='Title' /> <FieldRef Name='Date' /> <FieldRef Name='Desc' /> <FieldRef Name='Faculty' /> <FieldRef Name='Tag' /> <FieldRef Name='ImageUrl' /> </ViewFields> <QueryOptions />";

                            SPListItemCollection collitem = list.GetItems(query);
                            SPListItemCollection collitem2 = null;
                            if (collitem != null)
                            {
                                DataTable dtData = collitem2 != null ? collitem2.GetDataTable() : collitem.GetDataTable();
                                PagedDataSource pdsData = new PagedDataSource();
                                DataView dv = new DataView(dtData);
                                pdsData.DataSource = dv;
                                pdsData.AllowPaging = true;
                                pdsData.PageSize = iPageSize;
                                if (ViewState["PageNumber"] != null)
                                    pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber"]) - 1;
                                else
                                    pdsData.CurrentPageIndex = 0;
                                if (pdsData.PageCount > 1)
                                {
                                    Repeater1.Visible = true;
                                    ArrayList alPages = new ArrayList();
                                    for (int i = 1; i <= pdsData.PageCount; i++)
                                        alPages.Add((i).ToString());
                                    Repeater1.DataSource = alPages;
                                    Repeater1.DataBind();
                                }
                                else
                                {
                                    Repeater1.Visible = false;
                                }
                                rptversions.DataSource = pdsData;
                                rptversions.DataBind();

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
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
                BindScientificResearch();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

    }
}
