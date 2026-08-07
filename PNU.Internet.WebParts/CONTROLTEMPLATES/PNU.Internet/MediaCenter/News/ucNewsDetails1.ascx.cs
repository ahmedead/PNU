using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
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

                        //clsRequestsList _CurrentNew = busclsRequestsList.GetItemByID(ID);
                        ////clsRequestsList _CurrentNew = new clsRequestsList();
                        clsRequestsList _CurrentNew = new clsRequestsList();
                        AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/AllNews.aspx?Id=" + _CurrentNew.CatID;

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                                {
                                    SPList reqList = web.Lists["RequestsList"];

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(sPListItem);
                                    
                                    string localizedTitle = SPFactory.GetLocalizedTitle(_CurrentNew.Title, _CurrentNew.Title_EN);
                                    SetBrowserTitle(localizedTitle);
                                    

                                    SPQuery query = new SPQuery();
                               //     query.Query = $@"<Where>
                               //   <And>
                               //      <Eq>
                               //         <FieldRef Name='RequestStatus' />
                               //         <Value Type='Choice'>Approved</Value>
                               //      </Eq>
                               //      <And>
                               //         <Neq>
                               //            <FieldRef Name='ID' />
                               //            <Value Type='Counter'>{ID}</Value>
                               //         </Neq>
                               //         <Eq>
                               //            <FieldRef Name='MainCategory' />
                               //            <Value Type='Text'>{_CurrentNew.MainCategory}</Value>
                               //         </Eq>
                               //      </And>
                               //   </And>
                               //</Where>
                               //<OrderBy>
                               //   <FieldRef Name='MediaDate' Ascending='False' />
                               //</OrderBy>";


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
                                    query.RowLimit = Convert.ToUInt32(3);
                                    SPListItemCollection _AllData = reqList.GetItems(query);
                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsRequestsList> _AllItems = new List<clsRequestsList>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                    

                                }
                            }
                        });
  

                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
                            _AllData.Add(_CurrentNew);
                            rptMainData.DataSource = _AllData;
                            rptMainData.DataBind();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }


        private void SetBrowserTitle(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title)) return;

                // make the dynamic title available to ucHomeHeader
                HttpContext.Current.Items["PNU_BrowserTitle"] = title;

                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(title)));
                }
                else
                {
                    // fallback if the master exposes a normal <head runat="server">
                    Page.Title = title;
                }
            }
            catch (Exception ex)
            {
                //Publics.WriteToLog("ucMediaDetails.SetBrowserTitle", ex);
            }
        }

        // Handles nested master pages (e.g. DGA_Internal.master under a root master)
        private ContentPlaceHolder FindPlaceHolder(MasterPage master, string id)
        {
            while (master != null)
            {
                var ph = master.FindControl(id) as ContentPlaceHolder;
                if (ph != null) return ph;
                master = master.Master;
            }
            return null;
        }
    
    }
}
