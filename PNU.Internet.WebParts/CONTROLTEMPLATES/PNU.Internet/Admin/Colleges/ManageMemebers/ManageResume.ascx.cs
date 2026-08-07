using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges;
using System.Runtime.Remoting;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using static PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucORCIDData;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers
{
    public partial class ManageResume : UserControl
    {
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "MembersResumes";
        public string CollMemberListName { get; set; } = "CollMembersListName";
        public bool EditMode
        {
            get
            {
                try
                {
                    if (ViewState["EditMode"] != null)
                        return (bool)(ViewState["EditMode"]);
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                    return false;
                }




            }
            set
            {
                try
                {
                    ViewState["EditMode"] = value;
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                }


            }
        }

        public List<CurrentPosition> PosList
        {
            get
            {
                try
                {
                    if (ViewState["CurrentPositions"] != null)
                        return (List<CurrentPosition>)(ViewState["CurrentPositions"]);
                    else
                        return new List<CurrentPosition>();
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                    return null;
                }




            }
            set
            {
                try
                {
                    ViewState["CurrentPositions"] = value;
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                }


            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsCollegeMemebers())
                {
                    // ShowMessage((Control)Resume_alert_container,Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Unauthorized"), MessageType.Unauthorized);
                    Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Unauthorized");
                    Resume_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
                    dvMain.Visible = false;
                    return;
                }

                SetNewFieldTitles();

                if (!IsPostBack)
                {

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

                    lblEmail.Text = userEmail;
                    LoadMemeberResume();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }



        }
        private void SetNewFieldTitles()
        {
            try
            {
                bool isArabic = SPContext.Current.Web.Language == 1025;
                lit_interests.Text = isArabic ? "الاهتمامات البحثية" : "Research Interests";
                lit_degrees.Text = isArabic ? "المؤهلات العلمية" : "Academic Degrees";
                lit_bachelor.Text = isArabic ? "البكالوريوس" : "Bachelor";
                lit_master.Text = isArabic ? "الماجستير" : "Master";
                lit_doctorate.Text = isArabic ? "الدكتوراه" : "Doctorate";
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ManageResume - SetNewFieldTitles", ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!EditMode)
                    Save();
                else
                    Edit();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }
        protected void Save()
        {
            try
            {

                string email = lblEmail.Text.ToLower().Trim();
                var json = JsonConvert.SerializeObject(PosList);
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))//this.WebUrl
                        {
                            SPList requestsList = web.Lists[this.ListName];

                            SPListItem listItem = requestsList.Items.Add();

                            var userId = ValidateGoogleScholarUrl(txtScholarId.Text);
                            if (string.IsNullOrWhiteSpace(userId))
                            {
                                //ShowMessage((Control)Resume_alert_container, "Invalid google scholer url", MessageType.Error);
                                Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Invalidgooglescholarurl");
                                Resume_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
                                return;
                            }
                            else
                            {
                                listItem["GoogleScolarID"] = userId;

                                ////AESharaf




                                //SPQuery query = new SPQuery();
                                //query.Query = string.Concat(
                                //                 @"<Where>
                                //                      <Eq>
                                //                         <FieldRef Name='email' />
                                //                         <Value Type='Text'>" + email + @"</Value>
                                //                      </Eq>
                                //                   </Where>
                                //                    <OrderBy>
                                //                      <FieldRef Name='PublicationYear' Ascending='False' />
                                //                   </OrderBy>");

                                //SPList reqList = web.Lists.TryGetList("GoogleScholarArticles");
                                //List<Article> articles = new List<Article>();
                                //SPListItemCollection objNew = reqList.GetItems(query);

                                //if (objNew != null && objNew.Count > 0)
                                //{
                                //    articles = SPFactory.MapListItemsToClass<Article>(objNew);

                                //    foreach (Article article in articles)
                                //    {
                                //        objNew.Delete(Convert.ToInt32(article.ID));
                                //        web.AllowUnsafeUpdates = true;
                                //        reqList.Update();
                                //        web.AllowUnsafeUpdates = false;
                                //    }
                                //}
                                //string url = "https://scholar.google.com/citations?user=" + userId;
                                //articles = new List<Article>();
                                //articles = clsGoogleScolar.GetAllGoogleScholarArticles(url, email);
                                //if (articles != null && articles.Count > 0)
                                //{
                                //    foreach (Article article in articles)
                                //    {
                                //        SPListItem NewItem = reqList.Items.Add();
                                //        NewItem = SPFactory.MapClassToSPListItem(NewItem, article, true);
                                //        web.AllowUnsafeUpdates = true;
                                //        NewItem.Update();
                                //        web.AllowUnsafeUpdates = false;
                                //    }
                                //}
                                //string divMainProfilw = clsGoogleScolar.GetAllGoogleScholarProfileDiv(url);
                                //listItem["GoogleScolarProfileDiv"] = divMainProfilw;
                                ////AESharaf

                            }

                            ////AESharaf
                            //if (!string.IsNullOrWhiteSpace(txtOrcid.Text))
                            //{
                            //    SPList listORCIDArticles = web.Lists["ORCIDArticles"];
                            //    MainProfile _mainprofileobj = new MainProfile();
                            //    List<MainProfile> _MainProfile = new List<MainProfile>();
                            //    //SPListItem NewItem = null;
                            //    List<ORCIDArticle> _AllItems = new List<ORCIDArticle>();
                            //    string ORCID = txtOrcid.Text;
                            //    using (HttpClient client = new HttpClient())
                            //    {
                            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            //        HttpResponseMessage response = client.GetAsync("https://pub.orcid.org/v3.0/" + ORCID + @"/").GetAwaiter().GetResult();
                            //        if (response.IsSuccessStatusCode)
                            //        {
                            //            var responseContent = response.Content;
                            //            string result = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                            //            Root works = JsonConvert.DeserializeObject<Root>(result);
                            //            if (works != null)
                            //            {

                            //                Person _person = new Person();
                            //                Biography _Biography = new Biography();
                            //                if (works.person != null)
                            //                {
                            //                    _person = works.person;
                            //                    _mainprofileobj.Name = _person.name.givennames.value + " " + _person.name.familyname.value;
                            //                    if (works.person.biography != null)
                            //                    {
                            //                        _mainprofileobj.Biography = _person.biography.content;
                            //                    }
                            //                    if (works.person.othernames != null)
                            //                    {
                            //                        if (works.person.othernames.othername != null && works.person.othernames.othername.Count > 0)
                            //                        {

                            //                            string _OtherNames = "";
                            //                            foreach (var x in works.person.othernames.othername)
                            //                            {
                            //                                _OtherNames += x.content + " - ";
                            //                            }
                            //                            _OtherNames = _OtherNames.Remove(_OtherNames.Length - 2);
                            //                            _mainprofileobj.AlsoKnownAs = _OtherNames;
                            //                        }

                            //                    }
                            //                }

                            //                _MainProfile.Add(_mainprofileobj);


                            //                if (works.activitiessummary != null)
                            //                    if (works.activitiessummary.works != null)
                            //                        if (works.activitiessummary.works.group != null && works.activitiessummary.works.group.Count > 0)
                            //                        {

                            //                            foreach (var group in works.activitiessummary.works.group)
                            //                            {
                            //                                foreach (var work in group.worksummary)
                            //                                {
                            //                                    ORCIDArticle _Items = new ORCIDArticle();
                            //                                    _Items.Type = work.type == null ? "" : work.type;
                            //                                    if (work.title != null)
                            //                                        if (work.title.title != null)
                            //                                            _Items.Title = work.title.title.value == null ? "" : work.title.title.value;
                            //                                    if (work.title.subtitle != null)
                            //                                        _Items.Subtitle = work.title.subtitle == null ? "" : work.title.subtitle.ToString();

                            //                                    if (work.journaltitle != null)
                            //                                        if (work.journaltitle.value != null)
                            //                                            _Items.journaltitle = work.journaltitle.value == null ? "" : work.journaltitle.value;

                            //                                    if (work.createddate != null)
                            //                                        _Items.Added = work.createddate.value.ToString() == null ? "" : ConvertUnixDate(work.createddate.value);
                            //                                    if (work.lastmodifieddate != null)
                            //                                        _Items.LastModified = work.lastmodifieddate.value.ToString() == null ? "" : ConvertUnixDate(work.lastmodifieddate.value);
                            //                                    if (work.publicationdate != null)
                            //                                    {
                            //                                        if (work.publicationdate.year != null)
                            //                                            _Items.PublicationYear = work.publicationdate.year.value == null ? "" : work.publicationdate.year.value;
                            //                                        if (work.publicationdate.month != null)
                            //                                            _Items.PublicationMonth = work.publicationdate.month.value == null ? "" : work.publicationdate.month.value;
                            //                                        if (work.publicationdate.day != null)
                            //                                            _Items.PublicationDay = work.publicationdate.day.value == null ? "" : work.publicationdate.day.value;

                            //                                    }
                            //                                    Source source = work.source;
                            //                                    if (source != null)
                            //                                    {
                            //                                        if (source.sourcename != null)
                            //                                        {
                            //                                            if (source.sourcename.value != "")
                            //                                            {
                            //                                                _Items.Source = source.sourcename.value;
                            //                                                if (_Items.Source.Trim() != _MainProfile[0].Name.Trim())
                            //                                                    _Items.Source = _MainProfile[0].Name.Trim() + " via " + source.sourcename.value;
                            //                                            }
                            //                                        }
                            //                                    }
                            //                                    if (work.url != null && work.url.value != "")
                            //                                        _Items.Url = work.url.value == null ? "" : work.url.value;
                            //                                    if (work.externalids != null && work.externalids.externalid != null && work.externalids.externalid.Count > 0)
                            //                                    {
                            //                                        StringBuilder sb = new StringBuilder();
                            //                                        foreach (object x in work.externalids.externalid)
                            //                                        {
                            //                                            ExternalIdObject obj = JsonConvert.DeserializeObject<ExternalIdObject>(x.ToString());
                            //                                            if (obj.externalidtype == "doi")
                            //                                            {
                            //                                                sb.AppendLine("<div><span></span><span>DOI:</span>");
                            //                                                sb.AppendLine("<a target='_blank' class='underline' href='https://doi.org/" + obj.externalidvalue + "'> ");
                            //                                                sb.AppendLine(obj.externalidvalue);
                            //                                                sb.AppendLine("</a> ");
                            //                                                sb.AppendLine("</div>");
                            //                                            }
                            //                                            else if (obj.externalidtype == "issn")
                            //                                            {
                            //                                                sb.AppendLine("<div><span></span><span>Part of ISSN: </span>");
                            //                                                sb.AppendLine("<a target='_blank' class='underline' href='https://portal.issn.org/resource/ISSN/" + obj.externalidvalue + "'> ");
                            //                                                sb.AppendLine(obj.externalidvalue);
                            //                                                sb.AppendLine("</a> ");
                            //                                                sb.AppendLine("</div>");
                            //                                            }
                            //                                        }

                            //                                        _Items.ExternalIds = sb.ToString();
                            //                                    }

                            //                                    _AllItems.Add(_Items);
                            //                                    break;

                            //                                }
                            //                            }
                            //                        }


                            //                if (_AllItems != null && _AllItems.Count > 0)
                            //                {
                            //                    foreach (ORCIDArticle item in _AllItems)
                            //                    {
                            //                        web.AllowUnsafeUpdates = true;
                            //                        SPListItem NewItem1 = listORCIDArticles.Items.Add();
                            //                        item.email = email.Trim().ToLower();
                            //                        NewItem1 = SPFactory.MapClassToSPListItem(NewItem1, item, true);
                            //                        NewItem1.Update();
                            //                        web.AllowUnsafeUpdates = false;

                            //                    }
                            //                }

                            //                if (_mainprofileobj != null)
                            //                {
                            //                    listItem["ORCIDName"] = _mainprofileobj.Name;
                            //                    listItem["ORCIDBiography"] = _mainprofileobj.Biography;
                            //                    listItem["ORCIDAlsoKnownAs"] = _mainprofileobj.AlsoKnownAs;
                            //                }
                            //            }
                            //        }
                            //    }



                            //}



                            listItem["GoogleScholarUrl"] = txtScholarId.Text;
                            listItem["Email"] = lblEmail.Text;
                            if (txtOrcid.Text != "")
                                listItem["ORCID"] = txtOrcid.Text;
                            else
                                listItem["ORCID"] = "";
                            listItem["BriefAbout"] = txtSummary.Text;
                            listItem["CurrentPositions"] = json;

                            // New DGA faculty-member fields
                            if (requestsList.Fields.ContainsField("ResearchInterests"))
                                listItem["ResearchInterests"] = txtInterests.Text;
                            if (requestsList.Fields.ContainsField("Bachelor"))
                                listItem["Bachelor"] = txtBachelor.Text;
                            if (requestsList.Fields.ContainsField("Master"))
                                listItem["Master"] = txtMaster.Text;
                            if (requestsList.Fields.ContainsField("Doctorate"))
                                listItem["Doctorate"] = txtDoctorate.Text;

                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;

                            // ShowMessage((Control)Resume_alert_container, Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg"), MessageType.Success);
                            Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg");
                            Resume_alert_container.Attributes.Add("class", "alert alert-success mb-4");


                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                //ShowMessage((Control)Resume_alert_container, Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg") + ex.Message , MessageType.Error);
                Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg");
                Resume_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
            }
        }
        protected void Edit()
        {
            try
            {

                var json = JsonConvert.SerializeObject(PosList);
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
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList requestsList = web.Lists[this.ListName];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                           "<Eq>" +
                                               "<FieldRef Name='Email'/><Value Type='Text'>" + userEmail + "</Value>" +
                                           "</Eq>" +
                                           "</Where>";

                            SPListItem listItem = requestsList.GetItems(query)[0];

                            var userId = ValidateGoogleScholarUrl(txtScholarId.Text);
                            if (string.IsNullOrWhiteSpace(userId))
                            {
                                // ShowMessage((Control)Resume_alert_container, "Invalid google scholer url", MessageType.Error);
                                Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Invalidgooglescholarurl");
                                Resume_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
                                return;
                            }
                            else
                                listItem["GoogleScolarID"] = userId;

                            listItem["GoogleScholarUrl"] = txtScholarId.Text;
                            if (txtOrcid.Text != "")
                                listItem["ORCID"] = txtOrcid.Text;
                            else
                                listItem["ORCID"] = "";
                            listItem["BriefAbout"] = txtSummary.Text;
                            listItem["CurrentPositions"] = json;

                            // New DGA faculty-member fields
                            if (requestsList.Fields.ContainsField("ResearchInterests"))
                                listItem["ResearchInterests"] = txtInterests.Text;
                            if (requestsList.Fields.ContainsField("Bachelor"))
                                listItem["Bachelor"] = txtBachelor.Text;
                            if (requestsList.Fields.ContainsField("Master"))
                                listItem["Master"] = txtMaster.Text;
                            if (requestsList.Fields.ContainsField("Doctorate"))
                                listItem["Doctorate"] = txtDoctorate.Text;


                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;

                            //ShowMessage((Control)Resume_alert_container, Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg"), MessageType.Success);
                            Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg");
                            Resume_alert_container.Attributes.Add("class", "alert alert-success mb-4");



                        }
                    }
                });
            }
            catch (Exception ex)
            {
                //ShowMessage((Control)Resume_alert_container,Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg") + ex.Message, MessageType.Error);
                Resume_alert_container.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg");
                Resume_alert_container.Attributes.Add("class", "alert alert-danger mb-4");
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected void ShowMessage(Control ctl, string Message, MessageType type)
        {
            try
            {
                // dvMain.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void btnPositions_Click(object sender, EventArgs e)
        {
            AddNewCurPos();
            BindRepeater();
        }
        protected void AddNewCurPos()
        {
            try
            {
                int index = 0;
                var lst = new List<CurrentPosition>();

                if (PosList.Count > 0)
                {
                    var curpos = new CurrentPosition
                    {
                        Id = (PosList.Select(x => x.Id).Max()) + 1,
                        Order = Convert.ToInt32(txtOrder.Text),
                        Title = txtCurPos.Text

                    };

                    PosList.Add(curpos);
                    ViewState["CurrentPositions"] = PosList;
                }
                else
                {
                    index += 1;
                    var curpos = new CurrentPosition
                    {
                        Id = index,
                        Order = Convert.ToInt32(txtOrder.Text),
                        Title = txtCurPos.Text

                    };

                    lst.Add(curpos);
                    ViewState["CurrentPositions"] = lst;
                }



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }
        protected void DeleteCurPos(int Id)
        {
            try
            {
                var itemToRemove = PosList.Single(r => r.Id == Id);
                PosList.Remove(itemToRemove);
                ViewState["CurrentPositions"] = PosList;
                BindRepeater();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void rep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteCurPos(id);

                }

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void BindRepeater()
        {
            try
            {
                var posList = (List<CurrentPosition>)ViewState["CurrentPositions"];
                rep.DataSource = posList;
                rep.DataBind();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void LoadMemeberResume()
        {
            try
            {
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

                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                                "<Eq>" +
                                                    "<FieldRef Name='Email'/><Value Type='Text'>" + userEmail + "</Value>" +
                                                "</Eq>" +
                                                "</Where>";

                            SPListItemCollection collection = list.GetItems(query);
                            if (collection == null || collection.Count == 0)
                                return;
                            SPListItem Item = collection[0];
                            if (Item == null)
                                return;


                            ViewState["EditMode"] = true;

                            txtSummary.Text = !String.IsNullOrEmpty(Convert.ToString(Item["BriefAbout"])) ? Convert.ToString(Item["BriefAbout"]) : "";
                            txtScholarId.Text = !String.IsNullOrEmpty(Convert.ToString(Item["GoogleScholarUrl"])) ? Convert.ToString(Item["GoogleScholarUrl"]) : "";
                            txtOrcid.Text = !String.IsNullOrEmpty(Convert.ToString(Item["ORCID"])) ? Convert.ToString(Item["ORCID"]) : "";

                            // New DGA faculty-member fields
                            if (list.Fields.ContainsField("ResearchInterests"))
                                txtInterests.Text = !String.IsNullOrEmpty(Convert.ToString(Item["ResearchInterests"])) ? Convert.ToString(Item["ResearchInterests"]) : "";
                            if (list.Fields.ContainsField("Bachelor"))
                                txtBachelor.Text = !String.IsNullOrEmpty(Convert.ToString(Item["Bachelor"])) ? Convert.ToString(Item["Bachelor"]) : "";
                            if (list.Fields.ContainsField("Master"))
                                txtMaster.Text = !String.IsNullOrEmpty(Convert.ToString(Item["Master"])) ? Convert.ToString(Item["Master"]) : "";
                            if (list.Fields.ContainsField("Doctorate"))
                                txtDoctorate.Text = !String.IsNullOrEmpty(Convert.ToString(Item["Doctorate"])) ? Convert.ToString(Item["Doctorate"]) : "";
                            var json = !String.IsNullOrEmpty(Convert.ToString(Item["CurrentPositions"])) ? Convert.ToString(Item["CurrentPositions"]) : "";
                            var curPosObj = JsonConvert.DeserializeObject<List<CurrentPosition>>(json);


                            ViewState["CurrentPositions"] = curPosObj;
                            BindRepeater();

                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }
        protected bool IsCollegeMemebers()
        {

            try
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

                return IsMemeber;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return false;
            }


        }
        protected string ValidateGoogleScholarUrl(string url)
        {
            try
            {
                Uri myUri = new Uri(url);
                string userId = HttpUtility.ParseQueryString(myUri.Query).Get("user");
                if (string.IsNullOrWhiteSpace(userId))
                    return "";
                else return userId;

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }

        }

    }
    [Serializable]
    public class CurrentPosition
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
