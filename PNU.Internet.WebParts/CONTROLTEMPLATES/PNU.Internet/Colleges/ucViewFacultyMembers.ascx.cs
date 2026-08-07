using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Http.Formatting;
using iTextSharp.text;
using System.Net.Security;
using System.Web.Services;
using System.Threading;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucViewFacultyMembers : UserControl
    {
        protected bool IsCollegeMemebers()
        {
            bool IsMemeber = false;
            try
            {
                if (SPContext.Current.Web.CurrentUser == null)
                    return false;


                if (SPContext.Current.Web.CurrentUser.Email == null || SPContext.Current.Web.CurrentUser.Email == "")
                {
                    return false;
                }
                var userEmail = SPContext.Current.Web.CurrentUser.Email;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists["CollMembersListName"];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                                "<Eq>" +
                                                    "<FieldRef Name='EMAIL_ADDRESS'/><Value Type='Text'>" + userEmail.ToLower().Trim() + "</Value>" +
                                                "</Eq>" +
                                                "</Where>";

                            SPListItemCollection collection = list.GetItems(query);
                            if (collection == null || collection.Count == 0)
                                return;
                            SPListItem Item = collection[0];
                            if (Item == null)
                                return;


                            IsMemeber = true;


                        }
                    }
                });


                return IsMemeber;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return false;
            }
        }



        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        if (!IsCollegeMemebers())
        //        {
        //            divEdit.Visible = false;
        //        }

        //        // Load data synchronously
        //        LoadData();

        //        // Load tab functionalities asynchronously using threads
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCollegeTabFunctions));
        //    }
        //}



        //private void LoadCollegeTabFunctions(object state)
        //{
        //    try
        //    {
        //        string email = (string)state;

        //        // Call each tab function asynchronously
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege2TabClick), email);
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege3TabClick), email);
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege4TabClick), email);
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege5TabClick), email);
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege6TabClick), email);
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(WrappedCollege7TabClick), email);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        //Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
        //    }
        //}

        //// Wrapper methods for tab click event handlers
        //private void WrappedCollege2TabClick(object state)
        //{
        //    string email = (string)state;
        //    college2tab_Click(null, null);
        //}

        //private void WrappedCollege3TabClick(object state)
        //{
        //    string email = (string)state;
        //    college3tab_Click(null, null);
        //}

        //private void WrappedCollege4TabClick(object state)
        //{
        //    string email = (string)state;
        //    college4tab_Click(null, null);
        //}

        //private void WrappedCollege5TabClick(object state)
        //{
        //    string email = (string)state;
        //    college5tab_Click(null, null);
        //}

        //private void WrappedCollege6TabClick(object state)
        //{
        //    string email = (string)state;
        //    college6tab_Click(null, null);
        //}

        //private void WrappedCollege7TabClick(object state)
        //{
        //    string email = (string)state;
        //    college7tab_Click(null, null);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!IsCollegeMemebers())
                    {
                        divEdit.Visible = false;
                    }

                    // Load data synchronously
                    LoadData();

                    // Load tab functionalities asynchronously using tasks
                    Task.Run(() => LoadCollegeTabFunctions());
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        private void LoadCollegeTabFunctions()
        {
            try
            {
                string email = Request.QueryString["view"].ToString();

                ucMemberCourses ucMemberCoursesl = FindControl("ucMemberCourses") as ucMemberCourses;

                //ucGoogleScolar ucGoogleScolar = FindControl("ucGoogleScolar") as ucGoogleScolar;

                
                //ucMemberLibrary ucMemberLibrary = FindControl("ucMemberLibrary") as ucMemberLibrary;



                
                //ucMemberTweets ucMemberTweets = FindControl("ucMemberTweets") as ucMemberTweets;

                
                //ucORCIDData ucORCIDData = FindControl("ucORCIDData") as ucORCIDData;

                

                // Create tasks for each tab function and run them in parallel
                Task[] tabTasks = new Task[]
                {
                        Task.Run(() => ucMemberCoursesl.BindData())
                        //,
                        //Task.Run(() => ucGoogleScolar.BindDataIntoRepeater(email)),
                        //Task.Run(() => ucMemberLibrary.BindDataIntoRepeater(email)),
                        ////Task.Run(() => ucMemberAds.BindDataIntoRepeater(email)),
                        //Task.Run(() => ucMemberTweets.BindDataIntoRepeater()),
                        //Task.Run(() => ucORCIDData.BindDataIntoRepeater(email))
                };

                // Wait for all tasks to complete
                Task.WaitAll(tabTasks);


                college22tab.Attributes["onclick"] = "return false;";
                college33tab.Attributes["onclick"] = "return false;";
                college44tab.Attributes["onclick"] = "return false;";
                //college55tab.Attributes["onclick"] = "return false;";
                college66tab.Attributes["onclick"] = "return false;";
                college77tab.Attributes["onclick"] = "return false;";
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"LoadCollegeTabFunctions", ex.Message);
            }
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        if (!IsCollegeMemebers())
        //        {
        //            divEdit.Visible = false;

        //        }

        //        LoadData();
        //    }



        //}





        private void LoadData()
        {
            try
            {
                string email = Request.QueryString["view"].ToString();
            
                email.ToLower().Trim();
                clsCollMembersListName _FacultyMember = busclsFacultyMembers.GetFacultyMemberByEmail(email);

                List<AllDataCollMembersListName> _AllMainData = new List<AllDataCollMembersListName>();

                //List<lstCourses> _AllCourses = busclsCourses.GetCoursesByEmail(email);
            
                SPSecurity.RunWithElevatedPrivileges(delegate () {

                    if (_FacultyMember != null)

                    {
                        AllDataCollMembersListName Data = new AllDataCollMembersListName();
                        Data = SPFactory.MapClassToClass<AllDataCollMembersListName>(_FacultyMember);


                        //ucMemberResume ucMemberResumel = FindControl("ucMemberResume") as ucMemberResume;

                        //if (ucMemberResumel != null)
                        //{
                        //    ucMemberResumel.GetData(email);
                        //}

                        //ucMemberLibrary ucMemberLibraryl = FindControl("ucMemberLibrary") as ucMemberLibrary;

                        //if (ucMemberLibraryl != null)
                        //{
                        //    ucMemberLibraryl.BindDataIntoRepeater(email);
                        //}

                        //ucMemberAds ucMemberAds1 = FindControl("ucMemberAds") as ucMemberAds;

                        //if (ucMemberAds1 != null)
                        //{
                        //    Data.CountAnnounssements = ucMemberAds1.BindDataIntoRepeater(email);
                        //    ucMemberAds1.email = email;
                        //}


                        //ucORCIDData ucORCIDData = FindControl("ucORCIDData") as ucORCIDData;

                        //if (ucORCIDData != null)
                        //{
                        //    Data.CountORCID = ucORCIDData.BindDataIntoRepeater(email.Trim().ToLower());
                        //}

                        //ucGoogleScolar ucGoogleScolar = FindControl("ucGoogleScolar") as ucGoogleScolar;

                        //if (ucGoogleScolar != null)
                        //{
                        //    Data.CountGoogle = ucGoogleScolar.BindDataIntoRepeater(email.Trim().ToLower());
                        //}

                        //ucMemberTweets ucMemberTweets = FindControl("ucMemberTweets") as ucMemberTweets;

                        //if (ucMemberTweets != null)
                        //{
                        //    Data.CountTweets = ucMemberTweets.BindDataIntoRepeater();
                        //}

                        Data.CountAnnounssements = "0";
                        Data.CountGoogle = "0";
                        Data.CountORCID = "0";
                        Data.CountTweets = "0";

                        _AllMainData.Add(Data);
                        rptMainData.DataSource = _AllMainData;
                        rptMainData.DataBind();
                    }

                });

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        protected void college2tab_Click(object sender, EventArgs e)
        {
            try 
            {
                string email = Request.QueryString["view"].ToString();
                ucMemberCourses ucMemberCoursesl = FindControl("ucMemberCourses") as ucMemberCourses;

                if (ucMemberCoursesl != null)
                {
                    ucMemberCoursesl.BindData();
                }
                //college22tab.Attributes.Remove("onclick");

                //college22tab.Attributes.Add("onclientclick", "return false;");
                //college22tab.Attributes["onserverclick"] = "";
                college22tab.Attributes["onclick"] = "return false;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z2-tab-pane');", true);

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
            }
    
            




        }

        protected void college3tab_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Request.QueryString["view"].ToString();
                ucGoogleScolar ucGoogleScolar = FindControl("ucGoogleScolar") as ucGoogleScolar;

                if (ucGoogleScolar != null)
                {
                    ucGoogleScolar.BindDataIntoRepeater(email);
                }
                college33tab.Attributes["onclick"] = "return false;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z3-tab-pane');", true);

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
            }
            
        }

        protected void college4tab_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Request.QueryString["view"].ToString();
                ucMemberLibrary ucMemberLibrary = FindControl("ucMemberLibrary") as ucMemberLibrary;

                if (ucMemberLibrary != null)
                {
                    ucMemberLibrary.BindDataIntoRepeater(email);
                }
                college44tab.Attributes["onclick"] = "return false;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z4-tab-pane');", true);

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
            }

        }

        //protected void college5tab_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string email = Request.QueryString["view"].ToString();
        //        ucMemberAds ucMemberAds = FindControl("ucMemberAds") as ucMemberAds;

        //        if (ucMemberAds != null)
        //        {
        //            ucMemberAds.BindDataIntoRepeater(email);
        //        }
        //        college55tab.Attributes["onclick"] = "return false;";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z5-tab-pane');", true);

        //    }
        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
        //    }

        //}

        protected void college6tab_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Request.QueryString["view"].ToString();
                ucMemberTweets ucMemberTweets = FindControl("ucMemberTweets") as ucMemberTweets;

                if (ucMemberTweets != null)
                {
                    ucMemberTweets.BindDataIntoRepeater();
                }
                college66tab.Attributes["onclick"] = "return false;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z6-tab-pane');", true);

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
            }

        }

        protected void college7tab_Click(object sender, EventArgs e)
        {
            try
            {
            string email = Request.QueryString["view"].ToString();
            ucORCIDData ucORCIDData = FindControl("ucORCIDData") as ucORCIDData;

            if (ucORCIDData != null)
            {
                ucORCIDData.BindDataIntoRepeater(email);
            }
            college77tab.Attributes["onclick"] = "return false;";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('z7-tab-pane');", true);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),sender.ToString(), ex.Message);
            }
            
        }
    
    }

   
}
