using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers
{
    public partial class ManageAdvertisements : UserControl
    {
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "Advertisements";
        public string CollMemberListName { get; set; } ="CollMembersListName";
        readonly PagedDataSource _pgsourceAdv = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 10;
       
        private int CurrentPage
        {
            get
            {
                try
                {
                    if (ViewState["CurrentPageAds"] == null)
                    {
                        return 0;
                    }
                    return ((int)ViewState["CurrentPageAds"]);

                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                    return 0;
                }

                
            }
            set
            {
                try
                {
                    ViewState["CurrentPageAds"] = value;

                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                }
                
            }
        }

        public string email { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsCollegeMemebers())
                {
                    //ShowMessage(Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Unauthorized"), MessageType.Unauthorized);
                    Ads_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Unauthorized");
                    Ads_alert_container.Attributes.Add("class", "alert alert-danger mb-4");

                    dvMain.Visible = false;
                    return;
                }

                if (!IsPostBack)
                {

                    lblEmail.Text = Helper.GetUserEmail();
                    BindDataIntoRepeater(Helper.GetUserEmail());
                }

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList requestsList = web.Lists[this.ListName];

                            SPListItem listItem = requestsList.Items.Add();

                            listItem["Title"] = txtTitle.Text;
                            listItem["Email"] = lblEmail.Text;
                            listItem["Desc"] = txtSummary.Text;
                            listItem["Date"] = AdsDate.SelectedDate;



                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;

                            //ShowMessage(Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg"), MessageType.Success);
                            Ads_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg");
                            Ads_alert_container.Attributes.Add("class", "alert alert-success mb-4");

                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                // ShowMessage(Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg") + ex.Message, MessageType.Error);
                Ads_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg");
                Ads_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
            }

            BindDataIntoRepeater(Helper.GetUserEmail());
        }
      
        protected bool IsCollegeMemebers()
        {
            bool IsMemeber = false;
            string userEmail = "";
            if (Request.QueryString["IsAdmin"] != null)
            {
                string IsAdmin = Request.QueryString["IsAdmin"].ToString();
                if (IsAdmin == "YesIsAdmin")
                {
                    string email = Request.QueryString["email"].ToString();
                    if (email != null)
                    {
                        userEmail = email;
                    }
                }
            }
            else
                userEmail = Helper.GetUserEmail();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList(CollMemberListName);
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                                "<Eq>" +
                                                    "<FieldRef Name='EMAIL_ADDRESS'/><Value Type='Text'>" + userEmail + "</Value>" +
                                                "</Eq>" +
                                                "</Where>";

                            SPListItemCollection collection = list.GetItems(query);
                            if (collection == null || collection.Count == 0)
                                IsMemeber = false;
                            SPListItem Item = collection[0];
                            if (Item == null)
                                IsMemeber = false;


                            IsMemeber = true;


                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                IsMemeber = false;
            }

            return IsMemeber;
        }
        protected void ShowMessage(string Message, MessageType type)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected bool DeleteAdvertisement(int id)
        {
            bool retVal = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList List = web.Lists[ListName];
                            SPListItem itemToDelete = List.GetItemById(id);
                            web.AllowUnsafeUpdates = true;

                            itemToDelete.Delete();

                            web.AllowUnsafeUpdates = false;
                            retVal = true;
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
            return retVal;
        }
        protected void rptAds_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    //delete function
                    if (!DeleteAdvertisement(id))
                    {
                        //ShowMessage(Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg"), MessageType.Error);
                        Ads_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg");
                        Ads_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
                        return;
                    }

                    BindDataIntoRepeater(Helper.GetUserEmail());

                }

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        public void BindDataIntoRepeater(string email)
        {

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList PositionsList = web.Lists["Advertisements"];

                                SPQuery query = new SPQuery();

                                query.Query = @"<Where>
                                          <Eq>
                                             <FieldRef Name='Email' />
                                             <Value Type='Text'>" + email + @"</Value>
                                          </Eq>
                                       </Where>
                                       <OrderBy>
                                          <FieldRef Name='ItemOrder' Ascending='False' />
                                       </OrderBy>";

                                SPListItemCollection PosItems = PositionsList.GetItems(query);
                                if (PosItems != null && PosItems.Count > 0)
                                {
                                    //var dt = PosItems.GetDataTable();

                                    List<Advertisements> AllAdvs = new List<Advertisements>();
                                    AllAdvs = SPFactory.MapListItemsToClass<Advertisements>(PosItems);

                                    _pgsourceAdv.DataSource = AllAdvs;
                                    _pgsourceAdv.AllowPaging = true;
                                    _pgsourceAdv.PageSize = _pageSize;
                                    _pgsourceAdv.CurrentPageIndex = CurrentPage;
                                    ViewState["TotalPagesTweets"] = _pgsourceAdv.PageCount;
                                    lblpageTweets.Text = "Page " + (CurrentPage + 1) + " of " + _pgsourceAdv.PageCount;
                                    lbPreviousTweets.Enabled = !_pgsourceAdv.IsFirstPage;
                                    lbNextTweets.Enabled = !_pgsourceAdv.IsLastPage;
                                    lbFirstTweets.Enabled = !_pgsourceAdv.IsFirstPage;
                                    lbLastTweets.Enabled = !_pgsourceAdv.IsLastPage;


                                    rptAds.DataSource = _pgsourceAdv;
                                    rptAds.DataBind();
                                    HandlePaging();
                                }


                            }
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }



        private void HandlePaging()
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("PageIndex"); //Start from 0
                dt.Columns.Add("PageText"); //Start from 1

                _firstIndex = CurrentPage - 5;
                if (CurrentPage > 5)
                    _lastIndex = CurrentPage + 5;
                else
                    _lastIndex = 10;

                // Check last page is greater than total page then reduced it 
                // to total no. of page is last index
                if (_lastIndex > Convert.ToInt32(ViewState["TotalPagesTweets"]))
                {
                    _lastIndex = Convert.ToInt32(ViewState["TotalPagesTweets"]);
                    _firstIndex = _lastIndex - 10;
                }

                if (_firstIndex < 0)
                    _firstIndex = 0;

                // Now creating page number based on above first and last page index
                for (var i = _firstIndex; i < _lastIndex; i++)
                {
                    var dr = dt.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    dt.Rows.Add(dr);
                }

                rptPagingTweets.DataSource = dt;
                rptPagingTweets.DataBind();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage = 0;
                BindDataIntoRepeater(Helper.GetUserEmail());

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void lbLast_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage = (Convert.ToInt32(ViewState["TotalPagesTweets"]) - 1);
                BindDataIntoRepeater(Helper.GetUserEmail());

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage -= 1;
                BindDataIntoRepeater(Helper.GetUserEmail());

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void lbNext_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage += 1;
                BindDataIntoRepeater(Helper.GetUserEmail());

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                if (!e.CommandName.Equals("newPageTweets")) return;
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                BindDataIntoRepeater(Helper.GetUserEmail());

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                var lnkPage = (LinkButton)e.Item.FindControl("lbPagingTweets");
                if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
                lnkPage.Enabled = false;
                lnkPage.BackColor = Color.FromName("#007580");
                lnkPage.ForeColor = Color.White;

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
    }

    public class Advertisements
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Desc { get; set; }
        public string URL { get; set; }
        public string ItemOrder { get; set; }

    }
}
