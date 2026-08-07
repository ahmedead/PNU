using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class usAdvertismentsRequestsList : UserControl
    {
        private int iPageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   
                    if (CheckUserType() <= 0)
                    {
                        dvForm.Attributes.Add("class", "d-none");
                        hMsg.Attributes.Add("class", "block");
                      

                        return;
                    }
                    else
                    {
                        dvAddNew.Visible = true;

                      //  hMsg.Attributes.Add("class", "title text-dark fw-bold px-2  mb-4 mt-md-0 mt-4");
                      //  hMsg.InnerHtml = "قائمة الطلبات";
                    }

                    GetRequestsList();
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected int CheckUserType()
        {
            var mediaAdminUser = Helper.IsAdsAllowedUser();
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

        private void GetRequestsList()
        {
            try
            {
                List<clsAdsRequestsList> reqList = new List<clsAdsRequestsList>();
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
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {

                                var isApprovers = Helper.IsAdminUser();

                                SPList requestsList = web.Lists["AdvertisementsRequests"];
                                SPQuery query = new SPQuery();
                                query.Query = @"<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                                //if (!isApprovers)
                                //{
                                //    query.Query = @"<Eq><FieldRef Name='RequesterName'/><Value Type='Text'>" + currentUser + "</Value></Eq><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                                //}

                                SPListItemCollection listItemColl = requestsList.GetItems(query);
                                if (listItemColl == null || listItemColl.Count <= 0)
                                    return;

                                reqList = SPFactory.MapListItemsToClass<clsAdsRequestsList>(listItemColl);

                                if (reqList == null || reqList.Count <= 0)
                                    return;


                            }
                        }
                    }
                });

                PagedDataSource pdsData = new PagedDataSource();

                pdsData.DataSource = reqList;
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


                rep.DataSource = pdsData;
                rep.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
                GetRequestsList();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
    }
}
