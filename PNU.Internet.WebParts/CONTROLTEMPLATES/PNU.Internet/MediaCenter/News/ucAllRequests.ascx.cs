using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucAllRequests : UserControl
    {
        private int iPageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var mediaAdminUser = Helper.IsAllowedUser();
                    int AdminUser = CheckUserType(mediaAdminUser);
                    if (AdminUser <= 0)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");
                      

                        return;
                    }
                    else
                    {
                        dvAddNew.Visible = true;


                        // Disable btnDelete based on mediaAdminUser.CanDelete
                        

                        //  hMsg.Attributes.Add("class", "title text-dark fw-bold px-2  mb-4 mt-md-0 mt-4");
                        // hMsg.InnerHtml = "قائمة الطلبات";
                    }

                    GetRequestsList(mediaAdminUser);

                    


                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected int CheckUserType(NewsUserEditor mediaAdminUser)
        {
            
            if (mediaAdminUser?.Id > 0)
            {
                return (int)UserTypeEnum.MediaAdminUser;
            }

            var contentEditorUser = Helper.IsContentEditorUser();
            if (contentEditorUser?.Id > 0)
            {
                return (int)UserTypeEnum.ContentEditorUser;
            }

            return 0;
        }

        private void GetRequestsList(NewsUserEditor mediaAdminUser = null)
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


                                var isApprovers = Helper.IsAdminUser();

                                SPList requestsList = web.Lists["RequestsList"];
                                SPQuery query = new SPQuery();
                                query.Query = @"<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                                //if (!isApprovers)
                                //{
                                //    query.Query = @"<Eq><FieldRef Name='RequesterName'/><Value Type='Text'>" + currentUser + "</Value></Eq><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                                //}
                                query.RowLimit = 200;
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

                //PagedDataSource pdsData = new PagedDataSource();

                //pdsData.DataSource = reqList;
                //pdsData.AllowPaging = true;
                //pdsData.PageSize = iPageSize;
                //if (ViewState["PageNumber"] != null)
                //    pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber"]) - 1;
                //else
                //    pdsData.CurrentPageIndex = 0;

                //if (pdsData.PageCount > 1)
                //{
                //    Repeater1.Visible = true;

                //    List<string> pages = new List<string>();

                //    // Add << (First)
                //    pages.Add("First");

                //    // Add < (Previous)
                //    pages.Add("Prev");

                //    // Add page numbers
                //    for (int i = 1; i <= pdsData.PageCount; i++)
                //        pages.Add(i.ToString());

                //    // Add > (Next)
                //    pages.Add("Next");

                //    // Add >> (Last)
                //    pages.Add("Last");

                //    Repeater1.DataSource = pages;
                //    Repeater1.DataBind();
                //}
                //else
                //{
                //    Repeater1.Visible = false;
                //}


                //rep.DataSource = pdsData;
                //rep.DataBind();

                rep.DataSource = reqList;
                rep.DataBind();

                if (mediaAdminUser != null)
                {
                    foreach (RepeaterItem item in rep.Items)
                    {
                        LinkButton btnDelete = (LinkButton)item.FindControl("btnDelete");
                        if (btnDelete != null)
                        {
                            btnDelete.Visible = mediaAdminUser.CanDelete;
                        }

                        LinkButton btnEdit = (LinkButton)item.FindControl("btnEdit");
                        if (btnDelete != null)
                        {
                            btnEdit.Visible = mediaAdminUser.CanEdit;
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        //protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    try
        //    {
        //        int currentPage = ViewState["PageNumber"] != null ? Convert.ToInt32(ViewState["PageNumber"]) : 1;
        //        int totalPages = Convert.ToInt32(((List<string>)Repeater1.DataSource)?.Count ?? 0) - 4; // minus 4 for First, Prev, Next, Last

        //        switch (e.CommandArgument.ToString())
        //        {
        //            case "First":
        //                ViewState["PageNumber"] = 1;
        //                break;
        //            case "Prev":
        //                ViewState["PageNumber"] = Math.Max(1, currentPage - 1);
        //                break;
        //            case "Next":
        //                ViewState["PageNumber"] = Math.Min(totalPages, currentPage + 1);
        //                break;
        //            case "Last":
        //                ViewState["PageNumber"] = totalPages;
        //                break;
        //            default:
        //                ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
        //                break;
        //        }

        //        GetRequestsList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
        //    }

        //}



        protected string GetPaginationSymbol(string item)
        {
            switch (item)
            {
                case "First":
                    return "&laquo;&laquo;"; // <<
                case "Prev":
                    return "&laquo;"; // <
                case "Next":
                    return "&raquo;"; // >
                case "Last":
                    return "&raquo;&raquo;"; // >>
                default:
                    return item; // Page number
            }
        }


        protected void rep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string itemId = e.CommandArgument.ToString();
                DeleteItemFromSharePointList(itemId);
            }
            if (e.CommandName == "Edit")
            {
                string itemId = e.CommandArgument.ToString();
                Response.Redirect("EditNews.aspx?RequestId=" + itemId);
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
                                SPList list = web.Lists["RequestsList"];
                                SPListItem item = list.GetItemById(Convert.ToInt32(itemId));

                                if (item != null)
                                {
                                    web.AllowUnsafeUpdates = true;
                                    item.Recycle();
                                    //item.Delete();

                                    web.Update();

                                    
                                    web.AllowUnsafeUpdates = false;
                                    Response.Redirect("/ar/MediaCenter/News/Pages/requestlist.aspx");
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
