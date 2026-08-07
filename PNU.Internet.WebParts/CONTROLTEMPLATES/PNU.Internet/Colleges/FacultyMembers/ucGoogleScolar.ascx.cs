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
    public partial class ucGoogleScolar : UserControl
    {
        readonly PagedDataSource _pgsource = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 10;
        string authorId = "vzD7zHcAAAAJ";
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!Page.IsPostBack)
            //{
            //    if (Page.Request.QueryString["view"] == null)
            //        return;
            //    string email = Request.QueryString["view"].ToString();
            //    email.ToLower().Trim();
            //    BindDataIntoRepeater(email);

            //}



        }



        // Bind PagedDataSource into Repeater
        public string BindDataIntoRepeater(string email)
        {
            try
            {
                string Count = "0";


                List<Article> dt = new List<Article>();
                string divMainProfilw = "";
                //string email = Request.QueryString["view"].ToString();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList requestsList = web.Lists["MembersResumes"];


                                SPQuery query = new SPQuery();
                                query.Query = @"<Where>
                                      <Eq>
                                         <FieldRef Name='Email' />
                                         <Value Type='Text'>" + email.Trim().ToLower() + @"</Value>
                                      </Eq>
                                   </Where>";
                                //query.Query = string.Concat(
                                //                     @"<Where>
                                //          <Eq>
                                //             <FieldRef Name='Email' />
                                //             <Value Type='Text'>" + email + @"</Value>
                                //          </Eq>
                                //       </Where>
                                //    ");
                                SPListItemCollection items = requestsList.GetItems(query);

                                if (items != null && items.Count > 0)
                                {
                                    SPListItem _Item = items[0];

                                    if (_Item["GoogleScolarID"] != null)
                                    {
                                        dt = clsGoogleScolar.GetAllGoogleScholarArticlesListByEmail(email);
                                        if (_Item["GoogleScolarProfileDiv"] != null)
                                        {
                                            divMainProfilw = _Item["GoogleScolarProfileDiv"].ToString();

                                        }
                                    }
                                    if (dt == null || dt.Count == 0)
                                    {
                                        string url = "";
                                        url = "https://scholar.google.com/citations?user=" + _Item["GoogleScolarID"].ToString();
                                        dt = clsGoogleScolar.GetAllGoogleScholarArticles(url, email);

                                        List<Article> articles = new List<Article>();
                                        articles = clsGoogleScolar.GetAllGoogleScholarArticles(url, email);
                                        SPList reqList = web.Lists.TryGetList("GoogleScholarArticles");
                                        if (articles != null && articles.Count > 0)
                                        {
                                            foreach (Article article in articles)
                                            {
                                                SPListItem NewItem = reqList.Items.Add();
                                                NewItem = SPFactory.MapClassToSPListItem(NewItem, article, true);
                                                web.AllowUnsafeUpdates = true;
                                                NewItem.Update();
                                                web.AllowUnsafeUpdates = false;
                                            }
                                        }

                                        if (_Item["GoogleScolarProfileDiv"] == null)
                                        {
                                            url = "https://scholar.google.com/citations?user=" + _Item["GoogleScolarID"].ToString();
                                            divMainProfilw = clsGoogleScolar.GetAllGoogleScholarProfileDiv(url);

                                            //Add GoogleScolarProfileDiv to MemberResume
                                            web.AllowUnsafeUpdates = true;

                                            _Item["GoogleScolarProfileDiv"] = divMainProfilw;
                                            _Item.Update();
                                            web.AllowUnsafeUpdates = false;

                                        }
                                        else
                                        {
                                            divMainProfilw = _Item["GoogleScolarProfileDiv"].ToString();
                                        }



                                    }


                                    if (!string.IsNullOrEmpty(divMainProfilw))
                                        divMainProfilw = divMainProfilw.Replace("/citations/", "https://scholar.google.com/citations/");


                                }
                                else
                                    return;





                            }
                        }
                    }
                });



                //divMainProfilw = clsGoogleScolar.GetAllGoogleScholarProfileDiv(url);
                int i = 0;
                foreach (Article obj in dt)
                {
                    dt[i].ID = (i + 1).ToString();
                    i = i + 1;
                }
                Count = dt.Count.ToString();
                divMainProfile.InnerHtml = divMainProfilw;

                //var dt = GetDataFromDb();
                _pgsource.DataSource = dt;
                _pgsource.AllowPaging = true;
                // Number of items to be displayed in the Repeater
                _pgsource.PageSize = _pageSize;
                _pgsource.CurrentPageIndex = CurrentPage;
                // Keep the Total pages in View State
                ViewState["TotalPages"] = _pgsource.PageCount;
                // Example: "Page 1 of 10"
                lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                // Enable First, Last, Previous, Next buttons
                lbPrevious.Enabled = !_pgsource.IsFirstPage;
                lbNext.Enabled = !_pgsource.IsLastPage;
                lbFirst.Enabled = !_pgsource.IsFirstPage;
                lbLast.Enabled = !_pgsource.IsLastPage;

                // Bind data into repeater
                rptData.DataSource = _pgsource;
                rptData.DataBind();

                // Call the function to do paging
                HandlePaging();

                return dt.Count.ToString();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "0";
            }


            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "loadGoogleScolarTab()", true);
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
                if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
                {
                    _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
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

                rptPaging.DataSource = dt;
                rptPaging.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage = 0;
                string email = Request.QueryString["view"].ToString();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void lbLast_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
                string email = Request.QueryString["view"].ToString();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage -= 1;
                string email = Request.QueryString["view"].ToString();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void lbNext_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentPage += 1;
                string email = Request.QueryString["view"].ToString();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                if (!e.CommandName.Equals("newPage")) return;
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                string email = Request.QueryString["view"].ToString();
                BindDataIntoRepeater(email);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
                if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
                lnkPage.Enabled = false;
                lnkPage.BackColor = Color.FromName("#007580");
                lnkPage.ForeColor = Color.White;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
    }



}
