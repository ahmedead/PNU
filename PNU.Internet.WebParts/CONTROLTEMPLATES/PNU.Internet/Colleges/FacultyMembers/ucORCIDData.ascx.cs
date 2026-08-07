using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    public partial class ucORCIDData : UserControl
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            //if (Page.Request.QueryString["view"] == null)
            //    return;
            //string email = Request.QueryString["view"].ToString();
            //email.ToLower().Trim();
            //BindDataIntoRepeater(email);
        }



        // Bind PagedDataSource into Repeater
        public string BindDataIntoRepeater(string email)
        {
            string Count = "0";
            string ORCID = "";
            try
            {
                MainProfile _mainprofileobj = new MainProfile();
                List<MainProfile> _MainProfile = new List<MainProfile>();
                SPListItem NewItem = null;
                List<ORCIDArticle> _AllItems = new List<ORCIDArticle>();


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                
                    //using (SPSite site = new SPSite(newPortalURL))
                
                    {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists["MembersResumes"];
                        if (list != null)
                        {

                            SPQuery query = new SPQuery();

                            query.Query = @"<Where>
                                      <Eq>
                                         <FieldRef Name='Email' />
                                         <Value Type='Text'>" + email + @"</Value>
                                      </Eq>
                                   </Where>";
                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                if (collitem != null && collitem.Count > 0)
                                {
                                    if (collitem[0] != null)
                                    {
                                        SPListItem item = collitem[0];
                                        NewItem = collitem[0];
                                        if (item["ORCID"] != null)
                                        {
                                            ORCID = item["ORCID"].ToString();
                                                ORCID = ORCID.Trim();
                                        }
                                        else
                                                ORCID =  "0";
                                        if (item["ORCIDName"] != null)
                                        {
                                            _mainprofileobj.Name = item["ORCIDName"].ToString();
                                        }
                                        if (item["ORCIDBiography"] != null)
                                        {
                                            _mainprofileobj.Biography = item["ORCIDBiography"].ToString();
                                        }
                                        if (item["ORCIDAlsoKnownAs"] != null)
                                        {
                                            _mainprofileobj.AlsoKnownAs = item["ORCIDAlsoKnownAs"].ToString();
                                        }

                                    }

                                }
                            }

                        }

                        
                            if(ORCID != "0")
                            {
                                SPList listORCIDArticles = web.Lists["ORCIDArticles"];
                                if (listORCIDArticles != null)
                                {

                                    SPQuery query = new SPQuery();


                                    query.Query = $@"<Where>
                                  <Eq>
                                     <FieldRef Name='email' />
                                     <Value Type='Text'>{email}</Value>
                                  </Eq>
                               </Where>";
                                    SPListItemCollection collitemlistORCIDArticles = listORCIDArticles.GetItems(query);
                                    if (collitemlistORCIDArticles != null && collitemlistORCIDArticles.Count > 0)
                                    {
                                        _AllItems = SPFactory.MapListItemsToClass<ORCIDArticle>(collitemlistORCIDArticles);
                                        if (_mainprofileobj != null)
                                            _MainProfile.Add(_mainprofileobj);

                                    }
                                    else
                                    {
                                        //AESharaf
                                        if (!string.IsNullOrWhiteSpace(ORCID))
                                        {
                                            if (ORCID.Contains("https://orcid.org/"))
                                            {
                                                ORCID = ORCID.Substring(ORCID.IndexOf("https://orcid.org/") + "https://orcid.org/".Length);

                                            }
                                            listORCIDArticles = web.Lists["ORCIDArticles"];
                                            _mainprofileobj = new MainProfile();
                                            _MainProfile = new List<MainProfile>();

                                            _AllItems = new List<ORCIDArticle>();

                                            using (HttpClient client = new HttpClient())
                                            {
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                HttpResponseMessage response = client.GetAsync("https://pub.orcid.org/v3.0/" + ORCID + @"/").GetAwaiter().GetResult();
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var responseContent = response.Content;
                                                    string result = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                                                    Root works = JsonConvert.DeserializeObject<Root>(result);
                                                    if (works != null)
                                                    {

                                                        Person _person = new Person();
                                                        Biography _Biography = new Biography();
                                                        if (works.person != null)
                                                        {
                                                            _person = works.person;
                                                            string FamilyName = "";
                                                            if (_person.name.familyname != null)
                                                                FamilyName = _person.name.familyname.value;
                                                            _mainprofileobj.Name = _person.name.givennames.value + " " + FamilyName;
                                                            if (works.person.biography != null)
                                                            {
                                                                _mainprofileobj.Biography = _person.biography.content;
                                                            }
                                                            if (works.person.othernames != null)
                                                            {
                                                                if (works.person.othernames.othername != null && works.person.othernames.othername.Count > 0)
                                                                {

                                                                    string _OtherNames = "";
                                                                    foreach (var x in works.person.othernames.othername)
                                                                    {
                                                                        _OtherNames += x.content + " - ";
                                                                    }
                                                                    _OtherNames = _OtherNames.Remove(_OtherNames.Length - 2);
                                                                    _mainprofileobj.AlsoKnownAs = _OtherNames;
                                                                }

                                                            }
                                                        }

                                                        _MainProfile.Add(_mainprofileobj);


                                                        if (works.activitiessummary != null)
                                                            if (works.activitiessummary.works != null)
                                                                if (works.activitiessummary.works.group != null && works.activitiessummary.works.group.Count > 0)
                                                                {

                                                                    foreach (var group in works.activitiessummary.works.group)
                                                                    {
                                                                        foreach (var work in group.worksummary)
                                                                        {
                                                                            ORCIDArticle _Items = new ORCIDArticle();
                                                                            _Items.Type = work.type == null ? "" : work.type;
                                                                            if (work.title != null)
                                                                                if (work.title.title != null)
                                                                                    _Items.Title = work.title.title.value == null ? "" : work.title.title.value;
                                                                            if (work.title.subtitle != null)
                                                                                _Items.Subtitle = work.title.subtitle == null ? "" : work.title.subtitle.ToString();

                                                                            if (work.journaltitle != null)
                                                                                if (work.journaltitle.value != null)
                                                                                    _Items.journaltitle = work.journaltitle.value == null ? "" : work.journaltitle.value;

                                                                            if (work.createddate != null)
                                                                                _Items.Added = work.createddate.value.ToString() == null ? "" : ConvertUnixDate(work.createddate.value);
                                                                            if (work.lastmodifieddate != null)
                                                                                _Items.LastModified = work.lastmodifieddate.value.ToString() == null ? "" : ConvertUnixDate(work.lastmodifieddate.value);
                                                                            if (work.publicationdate != null)
                                                                            {
                                                                                if (work.publicationdate.year != null)
                                                                                    _Items.PublicationYear = work.publicationdate.year.value == null ? "" : work.publicationdate.year.value;
                                                                                if (work.publicationdate.month != null)
                                                                                    _Items.PublicationMonth = work.publicationdate.month.value == null ? "" : work.publicationdate.month.value;
                                                                                if (work.publicationdate.day != null)
                                                                                    _Items.PublicationDay = work.publicationdate.day.value == null ? "" : work.publicationdate.day.value;

                                                                            }
                                                                            Source source = work.source;
                                                                            if (source != null)
                                                                            {
                                                                                if (source.sourcename != null)
                                                                                {
                                                                                    if (source.sourcename.value != "")
                                                                                    {
                                                                                        _Items.Source = source.sourcename.value;
                                                                                        if (_Items.Source.Trim() != _MainProfile[0].Name.Trim())
                                                                                            _Items.Source = _MainProfile[0].Name.Trim() + " via " + source.sourcename.value;
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (work.url != null && work.url.value != "")
                                                                                _Items.Url = work.url.value == null ? "" : work.url.value;
                                                                            if (work.externalids != null && work.externalids.externalid != null && work.externalids.externalid.Count > 0)
                                                                            {
                                                                                StringBuilder sb = new StringBuilder();
                                                                                foreach (object x in work.externalids.externalid)
                                                                                {
                                                                                    ExternalIdObject obj = JsonConvert.DeserializeObject<ExternalIdObject>(x.ToString());
                                                                                    if (obj.externalidtype == "doi")
                                                                                    {
                                                                                        sb.AppendLine("<div><span></span><span>DOI:</span>");
                                                                                        sb.AppendLine("<a target='_blank' class='underline' href='https://doi.org/" + obj.externalidvalue + "'> ");
                                                                                        sb.AppendLine(obj.externalidvalue);
                                                                                        sb.AppendLine("</a> ");
                                                                                        sb.AppendLine("</div>");
                                                                                    }
                                                                                    else if (obj.externalidtype == "issn")
                                                                                    {
                                                                                        sb.AppendLine("<div><span></span><span>Part of ISSN: </span>");
                                                                                        sb.AppendLine("<a target='_blank' class='underline' href='https://portal.issn.org/resource/ISSN/" + obj.externalidvalue + "'> ");
                                                                                        sb.AppendLine(obj.externalidvalue);
                                                                                        sb.AppendLine("</a> ");
                                                                                        sb.AppendLine("</div>");
                                                                                    }
                                                                                }

                                                                                _Items.ExternalIds = sb.ToString();
                                                                            }

                                                                            _AllItems.Add(_Items);
                                                                            break;

                                                                        }
                                                                    }
                                                                }


                                                        if (_AllItems != null && _AllItems.Count > 0)
                                                        {
                                                            foreach (ORCIDArticle item in _AllItems)
                                                            {
                                                                web.AllowUnsafeUpdates = true;
                                                                SPListItem NewItem1 = listORCIDArticles.Items.Add();
                                                                item.email = email.Trim().ToLower();
                                                                NewItem1 = SPFactory.MapClassToSPListItem(NewItem1, item, true);
                                                                NewItem1.Update();
                                                                web.AllowUnsafeUpdates = false;

                                                            }
                                                        }

                                                        if (_mainprofileobj != null)
                                                        {
                                                            web.AllowUnsafeUpdates = true;
                                                            NewItem["ORCIDName"] = _mainprofileobj.Name;
                                                            NewItem["ORCIDBiography"] = _mainprofileobj.Biography;
                                                            NewItem["ORCIDAlsoKnownAs"] = _mainprofileobj.AlsoKnownAs;

                                                            NewItem["ORCID"] = ORCID;

                                                            NewItem.Update();
                                                            web.AllowUnsafeUpdates = false;
                                                        }
                                                    }
                                                }
                                            }



                                        }


                                    }


                                    rptProfile.DataSource = _MainProfile;
                                    rptProfile.DataBind();

                                    rptData1.DataSource = _AllItems;
                                    rptData1.DataBind();


                                }




                            }



                        }
                }

           
                });
                if (_AllItems != null && _AllItems.Count > 0)
                    return _AllItems.Count.ToString();
                else
                    return "0";

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }







            return Count;


        }


        static void DisplayPublicWorks(Root works)
        {
            try
            {
                Person _person = new Person();
                Biography _Biography = new Biography();
                if (works.person != null)
                {
                    _person = works.person;
                    Console.WriteLine($"Name: {_person.name.givennames.value} {_person.name.familyname.value}");

                    if (_person.biography != null)
                    {
                        _Biography = _person.biography;
                        Console.WriteLine($"Biography: {_Biography.content} ");
                    }
                    if (_person.othernames != null)
                    {
                        if (_person.othernames.othername != null && _person.othernames.othername.Count > 0)
                        {

                            string _OtherNames = "";
                            foreach (var x in _person.othernames.othername)
                            {
                                _OtherNames += x.content + " - ";
                            }
                            _OtherNames = _OtherNames.Remove(_OtherNames.Length - 2);
                        }

                    }


                }


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "DisplayPublicWorks", ex.Message);
            }





        }


        static public string ConvertUnixDate(long value)
        {
            try
            {
                if (value == 0)
                    return "";
                long unixTimestampMilliseconds = value;

                // Convert Unix timestamp to DateTimeOffset
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds);

                // Format DateTimeOffset as ISO 8601 string
                string iso8601String = dateTimeOffset.ToString("yyyy-MM-dd");
                return iso8601String;


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ConvertUnixDate", ex.Message);
                return "";
            }



        }

        public class ORCIDArticle
        {
            public string Type { get; internal set; }
            public string Title { get; internal set; }
            public string Subtitle { get; internal set; }
            public string Added { get; internal set; }
            public string LastModified { get; internal set; }
            public string PublicationYear { get; internal set; }
            public string PublicationMonth { get; internal set; }
            public string PublicationDay { get; internal set; }

            public string Url { get; internal set; }
            public string ExternalIds { get; internal set; }

            public string journaltitle { get; internal set; }

            public string Source { get; internal set; }

            public string email { get; internal set; }

        }

        public class MainProfile
        {
            public string Name { get; internal set; }
            public string Biography { get; internal set; }
            public string AlsoKnownAs { get; internal set; }


        }
    }


}
