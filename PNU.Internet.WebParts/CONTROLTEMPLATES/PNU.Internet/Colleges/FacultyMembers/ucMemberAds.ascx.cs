using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    public partial class ucMemberAds : UserControl
    {
        readonly PagedDataSource _pgsourceAdv = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 10;
        string authorId = "vzD7zHcAAAAJ";
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPageTweets"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPageTweets"]);
            }
            set
            {
                ViewState["CurrentPageTweets"] = value;
            }
        }

        public string email { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["view"] == null)
                    return;
                string email = Request.QueryString["view"].ToString();
                email.ToLower().Trim();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            

        }



        // Bind PagedDataSource into Repeater
        public string BindDataIntoRepeater(string email)
        {
            string Count = "0";
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
                                          <FieldRef Name='Created' Ascending='False' />
                                       </OrderBy>";

                                SPListItemCollection PosItems = PositionsList.GetItems(query);
                                if (PosItems != null && PosItems.Count > 0)
                                {
                                    //var dt = PosItems.GetDataTable();

                                    List<Advertisements> AllAdvs = new List<Advertisements>();
                                    AllAdvs = SPFactory.MapListItemsToClass<Advertisements>(PosItems);

                                    if (AllAdvs != null && AllAdvs.Count > 0) { Count = AllAdvs.Count.ToString(); }

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


                                    rptTweets.DataSource = _pgsourceAdv;
                                    rptTweets.DataBind();
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

            
           
            return Count;
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
                BindDataIntoRepeater(email);



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
                BindDataIntoRepeater(email);



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
                BindDataIntoRepeater(email);



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
                BindDataIntoRepeater(email);



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
                BindDataIntoRepeater(email);



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
        public string ID { get; internal set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }

    }
}
