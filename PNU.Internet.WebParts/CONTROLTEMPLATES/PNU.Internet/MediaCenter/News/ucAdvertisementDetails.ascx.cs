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
    public partial class ucAdvertisementDetails : UserControl
    {
        public string RedirectURL { get; set; } = "default.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["RequestID"] != null)
                    {

                        string CollegeCode = "";

                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = web.Lists.TryGetList("DepartmentDetails");
                                if (list != null)
                                {

                                    SPListItemCollection collitem1 = list.GetItems();
                                    if (collitem1 != null)
                                    {
                                        if (collitem1.Count > 0)
                                            if (collitem1[0]["DeptCode"] != null)
                                                CollegeCode = collitem1[0]["DeptCode"].ToString().Trim();


                                    }


                                }
                            }
                        }


                        var ID = Page.Request.QueryString["RequestID"].ToString();

                        clsAdvertisements _CurrentNew = new clsAdvertisements();

                        //AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/LatestAdvertisements.aspx";

                        string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

                        AllMCNews1.Visible = false;
                        AllMCNews.Visible = false;
                        if (currentUrl.Contains("/MediaCenter/"))
                        {
                            AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host + "/ar/MediaCenter/Pages/LatestAdvertisements.aspx";
                            AllMCNews.Visible = true;
                            
                        }
                        else
                        {
                            // Get the directory path of the current URL and append 'default.aspx'
                            string currentPath = HttpContext.Current.Request.Url.AbsolutePath;
                            string directoryPath = VirtualPathUtility.GetDirectory(currentPath);

                            AllMCNews1.HRef = directoryPath + RedirectURL;
                            AllMCNews1.Visible = true;
                        }

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                                {
                                    SPList reqList = web.Lists["AdvertisementsRequests"];

                                    SPQuery query = new SPQuery();

                                    if(CollegeCode == "")
                                        query.Query = $@"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='RequestStatus' />
                                                            <Value Type='Choice'>Approved</Value>
                                                         </Eq>
                                                        <Neq>
                                                               <FieldRef Name='ID' />
                                                               <Value Type='Counter'>{ID}</Value>
                                                            </Neq>
                                                      </And>
                                                   </Where>
                                                   <OrderBy>
                                                      <FieldRef Name='MediaDate' Ascending='False' />
                                                   </OrderBy>";
                                    else
                                        query.Query = $@"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='RequestStatus' />
                                                            <Value Type='Choice'>Approved</Value>
                                                         </Eq>
                                                         <And>
                                                            <Eq>
                                                               <FieldRef Name='FacultyName_EN' />
                                                               <Value Type='Text'>{CollegeCode}</Value>
                                                            </Eq>
                                                            <Neq>
                                                               <FieldRef Name='ID' />
                                                               <Value Type='Counter'>{ID}</Value>
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
                                        List<clsAdvertisements> _AllItems = new List<clsAdvertisements>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsAdvertisements>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsAdvertisements>(sPListItem);

                                    string localizedTitle = SPFactory.GetLocalizedTitle(_CurrentNew.Title, _CurrentNew.Title_EN);
                                    SetBrowserTitle(localizedTitle);
                                    //var head = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                                    //if (head != null)
                                    //{
                                    //    string desc = HttpUtility.HtmlEncode(
                                    //        Publics.TruncateText(SPFactory.GetLocalizedTitle(_CurrentNew.MediaContent, _CurrentNew.MediaContent_EN), 160));
                                    //    head.Controls.Add(new LiteralControl(
                                    //        $"<meta name=\"description\" content=\"{desc}\" />"));
                                    //}


                                }
                            }
                        });


                        if (_CurrentNew != null)
                        {
                            List<clsAdvertisements> _AllData = new List<clsAdvertisements>();
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
