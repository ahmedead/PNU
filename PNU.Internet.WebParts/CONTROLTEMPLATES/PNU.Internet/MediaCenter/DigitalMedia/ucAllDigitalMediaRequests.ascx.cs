using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.DigitalMedia
{
    public partial class ucAllDigitalMediaRequests : UserControl
    {
        private int iPageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var mediaAdminUser = Helper.IsAllowedUser();
                    bool CanDelete = false;
                    ContentEditorUsers contentEditorUser = Helper.IsContentEditorUser();
                    if(mediaAdminUser == null )
                    {
                        if(contentEditorUser == null )
                        {
                            dvForm.Attributes.Add("class", "d-none");
                            hMsg.Attributes.Add("class", "block");

                        }
                        else
                        {
                            if (contentEditorUser.Id > 0)
                            {
                                dvAddNew.Visible = true;
                                

                            }
                        }
                        return;
                    }

                    else if(mediaAdminUser.Id > 0 )
                    {
                        dvAddNew.Visible = true;
                        if(mediaAdminUser.CanDelete == true)
                            CanDelete = true;
                    }

                    GetRequestsList(CanDelete);
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected int CheckUserType(NewsUserEditor mediaAdminUser,ContentEditorUsers contentEditorUser)
        {
            
            if (mediaAdminUser?.Id > 0)
            {
                return (int)UserTypeEnum.MediaAdminUser;
            }

            
            if (contentEditorUser?.Id > 0)
            {
                return (int)UserTypeEnum.ContentEditorUser;
            }

            return 0;
        }

        private void GetRequestsList(bool CanDelete)
        {
            try
            {
                List<clsRequestsList> reqList = new List<clsRequestsList>();
                if (SPContext.Current.Web.CurrentUser == null)
                {

                    return;
                }
                var currentUser = SPContext.Current.Web.CurrentUser.Name;

                if (currentUser == null)
                {
                    return;
                }

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                            {


                                //var isApprovers = Helper.IsAdminUser();

                                SPList requestsList = web.Lists["DigitalMedia"];
                                SPQuery query = new SPQuery();
                                query.Query = @"<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                                SPListItemCollection listItemColl = requestsList.GetItems(query);
                                if (listItemColl == null || listItemColl.Count <= 0)
                                    return;

                                reqList = SPFactory.MapListItemsToClass<clsRequestsList>(listItemColl);

                                if (reqList == null || reqList.Count <= 0)
                                    return;


                            }
                        }
                    }
                });

                

                rep.DataSource = reqList;
                rep.DataBind();

                if (CanDelete != true)
                {
                    foreach (RepeaterItem item in rep.Items)
                    {
                        LinkButton btnDelete = (LinkButton)item.FindControl("btnDelete");
                        if (btnDelete != null)
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        


        protected void rep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string itemId = e.CommandArgument.ToString();
                DeleteItemFromSharePointList(itemId);
            }
        }

        private void DeleteItemFromSharePointList(string itemId)
        {
            try
            {
                
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                            {
                                SPList list = web.Lists["DigitalMedia"];
                                SPListItem item = list.GetItemById(Convert.ToInt32(itemId));

                                if (item != null)
                                {
                                    web.AllowUnsafeUpdates = true;
                                    item.Recycle();
                                    //item.Delete();

                                    web.Update();

                                    
                                    web.AllowUnsafeUpdates = false;
                                    Response.Redirect("/ar/MediaCenter/News/Pages/DigitalMediaRequestList.aspx");
                                }

                            }
                        }

                    }
                });
            }
            catch (Exception ex)
            {
                // Handle exception
                // You can log the error or display a message
            }
        }

    
    }
}
