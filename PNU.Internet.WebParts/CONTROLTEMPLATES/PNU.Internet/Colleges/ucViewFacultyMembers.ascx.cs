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
using System.Web.UI.HtmlControls;

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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "LoadCollegeTabFunctions", ex.Message);
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

                        // SEO Fix: Near Duplicates (#21), Dynamic Title (#12), Meta Description (#13), Canonicals (#8)
                        ApplyMemberSeoMetadata(_FacultyMember, email);
                    }

                });

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void ApplyMemberSeoMetadata(clsCollMembersListName member, string email)
        {
            try
            {
                if (this.Page == null || member == null)
                    return;

                string memberName = SPFactory.GetLocalizedTitle(member.FULL_NAME, member.ENGLISH_NAME);
                if (string.IsNullOrEmpty(memberName))
                    memberName = member.FULL_NAME ?? member.ENGLISH_NAME ?? "عضو هيئة التدريس";

                string profession = member.PROFESSION ?? "";
                string section = member.SECTION ?? "";
                string college = member.COLLEGE ?? "";
                string spec = member.SPECIALIZATION ?? "";
                string minor = member.SPECIAL_CPECIALIZATION ?? "";
                string qual = member.QUALIFICATION_NAME ?? "";

                // 1. Dynamic Page Title (Updates SharePoint PlaceHolderPageTitle so HTML <title> is modified)
                string fullTitle = string.Format("{0} - {1}{2} | جامعة الأميرة نورة بنت عبدالرحمن",
                    memberName,
                    profession,
                    (!string.IsNullOrEmpty(college) ? " (" + college + ")" : "")).Trim();

                SetSharePointPageTitle(fullTitle);

                if (this.Page.Header != null)
                {
                    // 2. Self-Referencing Canonical URL (Collapses millions of query string variations to one official URL)
                    string rawUrl = Request.Url.GetLeftPart(UriPartial.Path);
                    string cleanCanonicalUrl = string.Format("{0}?view={1}", rawUrl, HttpUtility.UrlEncode(email.ToLower().Trim()));

                    for (int i = this.Page.Header.Controls.Count - 1; i >= 0; i--)
                    {
                        var link = this.Page.Header.Controls[i] as HtmlLink;
                        if (link != null && link.Attributes["rel"] == "canonical")
                        {
                            this.Page.Header.Controls.RemoveAt(i);
                        }
                    }

                    HtmlLink canonicalLink = new HtmlLink();
                    canonicalLink.Attributes.Add("rel", "canonical");
                    canonicalLink.Attributes.Add("href", cleanCanonicalUrl);
                    this.Page.Header.Controls.Add(canonicalLink);

                    // 3. Distinct Academic Meta Description (Combines rank, qual, department, and sub-specialization to prevent duplicate description warnings)
                    List<string> details = new List<string>();
                    if (!string.IsNullOrEmpty(profession)) details.Add(profession);
                    if (!string.IsNullOrEmpty(qual)) details.Add(qual);
                    if (!string.IsNullOrEmpty(section)) details.Add("قسم " + section);
                    if (!string.IsNullOrEmpty(college)) details.Add(college);
                    if (!string.IsNullOrEmpty(spec)) details.Add("تخصص " + spec);
                    if (!string.IsNullOrEmpty(minor)) details.Add("التخصص الدقيق: " + minor);

                    string detailsStr = details.Count > 0 ? " (" + string.Join("، ", details) + ")" : "";
                    string desc = string.Format("الصفحة الأكاديمية والإنتاج العلمي لـ {0}{1} بجامعة الأميرة نورة بنت عبدالرحمن. المقررات الدراسية، الأبحاث المنشورة، والتواصل الأكاديمي.",
                        memberName, detailsStr);

                    if (desc.Length > 250) desc = desc.Substring(0, 247) + "...";

                    for (int i = this.Page.Header.Controls.Count - 1; i >= 0; i--)
                    {
                        var meta = this.Page.Header.Controls[i] as HtmlMeta;
                        if (meta != null && string.Equals(meta.Name, "description", StringComparison.OrdinalIgnoreCase))
                        {
                            this.Page.Header.Controls.RemoveAt(i);
                        }
                    }

                    HtmlMeta descMeta = new HtmlMeta();
                    descMeta.Name = "description";
                    descMeta.Content = desc;
                    this.Page.Header.Controls.Add(descMeta);

                    // 4. Schema.org Person JSON-LD
                    string safeName = HttpUtility.JavaScriptStringEncode(memberName);
                    string safeProf = HttpUtility.JavaScriptStringEncode(profession);
                    string safeColl = HttpUtility.JavaScriptStringEncode(college);
                    string safeEmail = HttpUtility.JavaScriptStringEncode(member.EMAIL_ADDRESS ?? email);

                    string jsonLd = string.Format(@"
<script type=""application/ld+json"">
{{
  ""@context"": ""https://schema.org"",
  ""@type"": ""Person"",
  ""name"": ""{0}"",
  ""jobTitle"": ""{1}"",
  ""email"": ""{2}"",
  ""worksFor"": {{
    ""@type"": ""CollegeOrUniversity"",
    ""name"": ""جامعة الأميرة نورة بنت عبدالرحمن - {3}""
  }},
  ""url"": ""{4}""
}}
</script>", safeName, safeProf, safeEmail, safeColl, cleanCanonicalUrl);

                    this.Page.Header.Controls.Add(new LiteralControl(jsonLd));
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ApplyMemberSeoMetadata", ex.Message);
            }
        }

        private void SetSharePointPageTitle(string fullTitle)
        {
            try
            {
                this.Page.Title = fullTitle;

                if (this.Page.Master != null)
                {
                    var phTitle = FindPlaceHolder(this.Page.Master, "PlaceHolderPageTitle");
                    if (phTitle != null)
                    {
                        phTitle.Controls.Clear();
                        phTitle.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(fullTitle)));
                    }

                    var phTitleArea = FindPlaceHolder(this.Page.Master, "PlaceHolderPageTitleInTitleArea");
                    if (phTitleArea != null)
                    {
                        phTitleArea.Controls.Clear();
                        phTitleArea.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(fullTitle)));
                    }
                }

                // Client-side fallback to guarantee browser tab title updates
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateDocTitle",
                    $"document.title = {HttpUtility.JavaScriptStringEncode(fullTitle, true)};", true);
            }
            catch { }
        }

        private ContentPlaceHolder FindPlaceHolder(System.Web.UI.MasterPage master, string id)
        {
            while (master != null)
            {
                var ph = master.FindControl(id) as ContentPlaceHolder;
                if (ph != null) return ph;
                master = master.Master;
            }
            return null;
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
            }

        }

    }


}
