using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter
{
    public partial class ucNewsDetails1 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["RequestID"] != null)
                    {
                        var ID = Page.Request.QueryString["RequestID"].ToString();

                        clsRequestsList _CurrentNew = new clsRequestsList();

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                                {
                                    SPList reqList = web.Lists["RequestsList"];

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(sPListItem);

                                    // ----- Related news (same MainCategory, IsHome = true, exclude current) -----
                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<Where>
                                                        <And>
                                                           <Eq>
                                                              <FieldRef Name='RequestStatus' />
                                                              <Value Type='Choice'>Pending</Value>
                                                           </Eq>
                                                           <And>
                                                              <Neq>
                                                                 <FieldRef Name='ID' />
                                                                 <Value Type='Counter'>{ID}</Value>
                                                              </Neq>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='MainCategory' />
                                                                    <Value Type='Text'>{_CurrentNew.MainCategory}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='IsHome' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </And>
                                                        </And>
                                                     </Where>
                                                     <OrderBy>
                                                        <FieldRef Name='MediaDate' Ascending='False' />
                                                     </OrderBy>";
                                    query.RowLimit = 3;
                                    SPListItemCollection _AllData = reqList.GetItems(query);

                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsRequestsList> _AllItems =
                                            SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                        phRelatedSection.Visible = true;
                                    }
                                }
                            }
                        });

                        // "All news" link in the related section header
                        AllMCNews.HRef = "https://"
                            + HttpContext.Current.Request.Url.Host.ToString()
                            + "/ar/MediaCenter/Pages/AllNews.aspx?Id="
                            + (_CurrentNew != null ? _CurrentNew.CatID : "1");

                        // Bind the main news + breadcrumb (both use the same single-item source)
                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
                            _AllData.Add(_CurrentNew);

                            rptBreadcrumb.DataSource = _AllData;
                            rptBreadcrumb.DataBind();

                            rptMainData.DataSource = _AllData;
                            rptMainData.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }
    }

}
