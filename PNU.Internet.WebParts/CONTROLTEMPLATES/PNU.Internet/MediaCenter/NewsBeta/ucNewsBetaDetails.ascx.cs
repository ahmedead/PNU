using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.BusinessClassess;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta
{

    public partial class ucNewsBetaDetails : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string WebUrl { get; set; } = "/ar/MediaCenter/NewsBeta/";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; } = "RequestsList";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["RequestID"] != null)
                    {
                        var ID = Page.Request.QueryString["RequestID"].ToString();

                        //clsRequestsListNewsBeta _CurrentNew = busclsRequestsListNewsBeta.GetItemByID(ID);
                        ////clsRequestsListNewsBeta _CurrentNew = new clsRequestsListNewsBeta();
                        clsRequestsListNewsBeta _CurrentNew = new clsRequestsListNewsBeta();
                        AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/AllNews.aspx?Id=" + _CurrentNew.CatID;

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb(WebUrl))
                                {
                                    SPList reqList = web.Lists[ListName];

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsListNewsBeta>(sPListItem);

                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='RequestStatus' />
                                        <Value Type='Choice'>Approved</Value>
                                     </Eq>
                                     <And>
                                        <Neq>
                                           <FieldRef Name='ID' />
                                           <Value Type='Counter'>{ID}</Value>
                                        </Neq>
                                        <Eq>
                                           <FieldRef Name='MainCategory' />
                                           <Value Type='Text'>{_CurrentNew.MainCategory}</Value>
                                        </Eq>
                                     </And>
                                  </And>
                               </Where>
                               <OrderBy>
                                  <FieldRef Name='MediaDate' Ascending='False' />
                               </OrderBy>";


                                    query.RowLimit = Convert.ToUInt32(3);
                                    SPListItemCollection _AllData = reqList.GetItems(query);
                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsRequestsListNewsBeta> _AllItems = new List<clsRequestsListNewsBeta>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsRequestsListNewsBeta>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }



                                }
                            }
                        });


                        if (_CurrentNew != null)
                        {
                            List<clsRequestsListNewsBeta> _AllData = new List<clsRequestsListNewsBeta>();
                            _AllData.Add(_CurrentNew);
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
