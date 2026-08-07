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
    public partial class ucMemberTweets : UserControl
    {
        readonly PagedDataSource _pgsourceAdv = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 10;
        string authorId = "vzD7zHcAAAAJ";
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPageAdv"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPageAdv"]);
            }
            set
            {
                ViewState["CurrentPageAdv"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            //if (!Page.IsPostBack)
            //{
            //    if (Page.Request.QueryString["view"] == null)
            //        return;
            //    BindDataIntoRepeater();
            //}

        }



        // Bind PagedDataSource into Repeater
        public string BindDataIntoRepeater()
        {
            


            try
            {
                if (Page.Request.QueryString["view"] == null)
                    return "0";
                string Count = "0";

                var email = Page.Request.QueryString["view"].ToString();
                email.ToLower().Trim();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList PositionsList = web.Lists["MemberTweets"];

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
                                    Count = PosItems.Count.ToString();
                                    //var dt = PosItems.GetDataTable();

                                    List<Advertisements> AllAdvs = new List<Advertisements>();
                                    AllAdvs = SPFactory.MapListItemsToClass<Advertisements>(PosItems);

                                    _pgsourceAdv.DataSource = AllAdvs;
                                    _pgsourceAdv.AllowPaging = true;
                                    _pgsourceAdv.PageSize = _pageSize;
                                    _pgsourceAdv.CurrentPageIndex = CurrentPage;
                                    ViewState["TotalPagesAdv"] = _pgsourceAdv.PageCount;
                                    lblpageAdv.Text = "Page " + (CurrentPage + 1) + " of " + _pgsourceAdv.PageCount;
                                    lbPreviousAdv.Enabled = !_pgsourceAdv.IsFirstPage;
                                    lbNextAdv.Enabled = !_pgsourceAdv.IsLastPage;
                                    lbFirstAdv.Enabled = !_pgsourceAdv.IsFirstPage;
                                    lbLastAdv.Enabled = !_pgsourceAdv.IsLastPage;


                                    rptAdvs.DataSource = _pgsourceAdv;
                                    rptAdvs.DataBind();
                                    HandlePaging();
                                }


                            }
                        }
                    }
                });

                return Count;

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return "0";
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
                if (_lastIndex > Convert.ToInt32(ViewState["TotalPagesAdv"]))
                {
                    _lastIndex = Convert.ToInt32(ViewState["TotalPagesAdv"]);
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

                rptPagingAdv.DataSource = dt;
                rptPagingAdv.DataBind();



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
                BindDataIntoRepeater();



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
                CurrentPage = (Convert.ToInt32(ViewState["TotalPagesAdv"]) - 1);
                BindDataIntoRepeater();



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
                BindDataIntoRepeater();



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
                BindDataIntoRepeater();

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
                if (!e.CommandName.Equals("newPageAdv")) return;
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                BindDataIntoRepeater();



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
                var lnkPage = (LinkButton)e.Item.FindControl("lbPagingAdv");
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


    public class Tweets
    {
        public string ID { get; internal set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }

    }
}
