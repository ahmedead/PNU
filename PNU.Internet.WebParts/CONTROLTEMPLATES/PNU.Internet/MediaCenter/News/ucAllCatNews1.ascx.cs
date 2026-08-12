using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucAllCatNews1 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    var Id = 0;
                    if (Page.Request.QueryString["ID"] != null)
                    {
                        Id = Convert.ToInt32(Page.Request.QueryString["ID"]);

                    }

                    clsRequestsList _CurrentNew = new clsRequestsList();

                    SPListItem objNew = null;
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                            {
                                SPList reqList = web.Lists["RequestsList"];
                                objNew = reqList.GetItemById(Convert.ToInt32(ID));

                                if (objNew != null)

                                {
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(objNew);
                                }



                                if (_CurrentNew != null)
                                {

                                    AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/News/Pages/AllNews.aspx";//?Id=" + _CurrentNew.CatID;
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
                                           <Value Type='Counter'>{Id}</Value>
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
                                        List<clsRequestsList> _AllItems = new List<clsRequestsList>();
                                        //foreach (var r in _AllItems)
                                        //{
                                        //    if (r.MediaTypes_EN.Contains("Video"))
                                        //        r.IsVideo = true;
                                        //    else
                                        //        r.IsVideo = false;
                                        //}
                                        _AllItems = SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                }
                            }
                        }
                    });



                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

    }
}
