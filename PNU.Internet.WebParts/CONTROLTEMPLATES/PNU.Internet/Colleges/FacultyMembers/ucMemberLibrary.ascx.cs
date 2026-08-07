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
    public partial class ucMemberLibrary : UserControl
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.Request.QueryString["view"] == null)
            //    return;
            //string email = Request.QueryString["view"].ToString();
            //email.ToLower().Trim();
            //BindDataIntoRepeater(email);

        }



        // Bind PagedDataSource into Repeater
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
                                SPList PositionsList = web.Lists["LibraryHours"];

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

                                    List<LibraryHours> AllAdvs = new List<LibraryHours>();
                                    AllAdvs = SPFactory.MapListItemsToClass<LibraryHours>(PosItems);

                                    rptLibraryHours.DataSource = AllAdvs;
                                    rptLibraryHours.DataBind();
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



        
    }


    public class LibraryHours
    {
        public string ID { get; internal set; }
        public string Title { get; set; }
        public string days { get; set; }
        public string daysEn { get; set; }

        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Email { get; set; }

        public string Days1
        {
            get
            {
                string d = "";
                try
                {
                    var x = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_DaysField");

                    if (x == "days")
                    {
                        if (days == null)
                            return d;
                        if (days != null)
                            d = days.Replace(";#", "-").ToString();
                        if (d[0] == '-')
                            d = d.Remove(0, 1);
                        if (d[d.Length - 1] == '-')
                            d = d.Remove(d.Length - 1, 1);
                        d = d.Replace("-", " - ").ToString();
                    }
                    else
                    {
                        if (daysEn == null)
                            return d;
                        if (daysEn != null)
                            d = daysEn.Replace(";#", "-").ToString();
                        if (d[0] == '-')
                            d = d.Remove(0, 1);
                        if (d[d.Length - 1] == '-')
                            d = d.Remove(d.Length - 1, 1);
                        d = d.Replace("-", " - ").ToString();
                    }



                }

                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"LibraryHours - Days1", ex.Message);
                }

               
                return d;

            }
                }



    }
}
