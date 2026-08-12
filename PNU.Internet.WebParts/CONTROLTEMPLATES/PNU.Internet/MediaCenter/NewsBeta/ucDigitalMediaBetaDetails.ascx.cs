using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.BusinessClassess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta
{
    public partial class ucDigitalMediaBetaDetails : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string WebUrl { get; set; } = "/ar/MediaCenter/NewsBeta/";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; } = "DigitalMedia";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["RequestID"] != null)
                    {
                        var ID = Page.Request.QueryString["RequestID"].ToString();

                        clsAdvertisementsNewsBeta _CurrentNew = new clsAdvertisementsNewsBeta();


                        SPListItem objNew = null;
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb(WebUrl))
                                {
                                    SPList reqList = web.Lists[ListName];
                                    objNew = reqList.GetItemById(Convert.ToInt32(ID));
                                }
                            }
                        });
                        if (objNew == null)
                            return;
                        _CurrentNew = SPFactory.MapListItemsToClass<clsAdvertisementsNewsBeta>(objNew);




                        AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/AllNews.aspx";

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb(WebUrl))
                                {
                                    SPList reqList = web.Lists[ListName];

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
                                        <Neq>
                                           <FieldRef Name='MainCategory' />
                                           <Value Type='Text'>الأخبار الرئيسية</Value>
                                        </Neq>
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

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsAdvertisementsNewsBeta>(sPListItem);

                                }
                            }
                        });


                        if (_CurrentNew != null)
                        {
                            List<clsAdvertisementsNewsBeta> _AllData = new List<clsAdvertisementsNewsBeta>();
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
