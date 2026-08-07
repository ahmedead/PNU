using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public static class clsGoogleScolar
    {
        public static List<Article> GetGoogleScholarArticles(string authorId)
        {
            try
            {
                string url = $"https://scholar.google.com/citations?user={authorId}";

                var web = new HtmlWeb();
                var doc = web.Load(url);

                var articles = new List<Article>();

                var articleNodes = doc.DocumentNode.SelectNodes("//tr[@class='gsc_a_tr']");

                if (articleNodes != null)
                {
                    foreach (var articleNode in articleNodes)
                    {
                        var titleNode = articleNode.SelectSingleNode(".//a[@class='gsc_a_at']");
                        var authorsNode = articleNode.SelectSingleNode(".//div[@class='gs_gray']");
                        var yearNode = articleNode.SelectSingleNode(".//td[@class='gsc_a_y']");
                        var citationNode = articleNode.SelectSingleNode(".//td[@class='gsc_a_c']");
                        var urlNode = articleNode.SelectSingleNode(".//a[@class='gsc_a_at']");

                        string title = titleNode?.InnerText.Trim() ?? "";
                        string authors = authorsNode?.InnerText.Trim() ?? "";
                        string publicationYear = yearNode?.InnerText.Trim() ?? "";
                        string citationCount = citationNode?.InnerText.Trim() ?? "";
                        string articleUrl = urlNode?.GetAttributeValue("href", "") ?? "";

                        articles.Add(new Article
                        {
                            Title = title,
                            Authors = authors,
                            PublicationYear = publicationYear,
                            CitationCount = citationCount,
                            Url = "https://scholar.google.com" + articleUrl
                        });
                    }
                }

                return articles;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - GetGoogleScholarArticles", ex.Message);
                return null;
            }

        }
        public static List<Article> GetAllGoogleScholarArticlesListByEmail(string email)
        {
            try
            {
                email = email.Trim().ToLower();
                SPListItemCollection objNew = null;
                List<Article> articles = new List<Article>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                              <Eq>
                                 <FieldRef Name='email' />
                                 <Value Type='Text'>" + email + @"</Value>
                              </Eq>
                           </Where>
                            <OrderBy>
                              <FieldRef Name='PublicationYear' Ascending='False' />
                           </OrderBy>");

                            SPList reqList = web.Lists.TryGetList("GoogleScholarArticles");
                            objNew = reqList.GetItems(query);
                            if (objNew != null && objNew.Count > 0)
                                articles = SPFactory.MapListItemsToClass<Article>(objNew);
                        }
                    }
                });





                return articles;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - GetAllGoogleScholarArticlesListByEmail", ex.Message);
                return null;
            }

        }


        public static List<Article> GetAllGoogleScholarArticles(string url, string email)
        {
            try
            {
                var articles = new List<Article>();
                int page = 1;

                while (true)
                {
                    var pageUrl = $"{url}&view_op=list_works&pagesize=100&sortby=pubdate&hl=ar&cstart={(page - 1) * 100}";

                    var web = new HtmlWeb();
                    var doc = web.Load(pageUrl);

                    var articleNodes = doc.DocumentNode.SelectNodes("//tr[@class='gsc_a_tr']");
                    var ProfieNode = doc.DocumentNode.SelectNodes("//div[@class='gsc_lcl']");//gsc_prf
                    if (ProfieNode != null)
                    {
                        foreach (var Prof in ProfieNode)
                        {
                            var x = Prof.SelectSingleNode(".//div[@class='gsc_prf']");

                        }

                        //var MainProfile = ProfieNode.SelectSingleNode(".//div[@class='gsc_prf']");
                    }
                    if (articleNodes == null)
                    {
                        break;
                    }
                    else if (articleNodes.Count == 1)
                    {
                        if (articleNodes[0].InnerHtml.Contains("لا توجد مقالات في هذا الملف الشخصي."))
                            break;
                    }
                    int index = 1;
                    foreach (var articleNode in articleNodes)
                    {
                        var titleNode = articleNode.SelectSingleNode(".//a[@class='gsc_a_at']");
                        if (titleNode == null)
                        {
                            break;
                        }
                        var authorsNode = articleNode.SelectSingleNode(".//div[@class='gs_gray']");
                        var magazine = "";
                        if (authorsNode != null)
                            if (authorsNode.NextSibling != null)
                                magazine = authorsNode.NextSibling.InnerText.Trim();
                        var yearNode = articleNode.SelectSingleNode(".//td[@class='gsc_a_y']");
                        var citationNode = articleNode.SelectSingleNode(".//td[@class='gsc_a_c']");
                        var urlNode = articleNode.SelectSingleNode(".//a[@class='gsc_a_at']");

                        string title = titleNode?.InnerText.Trim() ?? "";
                        string authors = authorsNode?.InnerText.Trim() ?? "";
                        string publicationYear = yearNode?.InnerText.Trim() ?? "";
                        string citationCount = citationNode?.InnerText.Trim() ?? "";
                        string articleUrl = urlNode?.GetAttributeValue("href", "") ?? "";

                        articles.Add(new Article
                        {
                            ID = index.ToString(),
                            Title = title,
                            Authors = authors,
                            PublicationYear = publicationYear,
                            CitationCount = citationCount,
                            Magazine = magazine.Replace(", " + publicationYear, ""),
                            Url = "https://scholar.google.com" + articleUrl
                        });
                        index = index + 1;

                    }

                    page++;
                }


                email = email.Trim().ToLower();
                //SPListItemCollection objNew = null;
                List<Article> Newarticles = new List<Article>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                              <Eq>
                                 <FieldRef Name='email' />
                                 <Value Type='Text'>" + email + @"</Value>
                              </Eq>
                           </Where>");

                            SPList reqList = web.Lists.TryGetList("GoogleScholarArticles");

                            SPListItemCollection _AllData = reqList.GetItems(query);

                            List<Article> _AllListData = SPFactory.MapListItemsToClass<Article>(_AllData);
                            if (articles != null && articles.Count > 0)
                            {
                                List<Article> itemsNotInOtherList = articles.Where(college => !_AllListData.Any(faculty => faculty.Title == college.Title)).ToList();

                                if (itemsNotInOtherList != null && itemsNotInOtherList.Count > 0)
                                {
                                    foreach (Article item in itemsNotInOtherList)
                                    {
                                        web.AllowUnsafeUpdates = true;
                                        SPListItem _Item = reqList.Items.Add();
                                        _Item = SPFactory.MapClassToSPListItem(_Item, item, true);
                                        _Item["email"] = email;
                                        _Item.Update();
                                        web.AllowUnsafeUpdates = false;

                                    }
                                }

                            }

                        }
                    }
                });
                return articles;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - GetAllGoogleScholarArticles", ex.Message);
                return null;
            }


        }
        public static string GetAllGoogleScholarProfileDiv(string url)
        {
            string _return = "";
            try
            {
                var pageUrl = $"{url}&view_op=list_works&pagesize=100&sortby=pubdate&hl=en";

                var web = new HtmlWeb();
                var doc = web.Load(pageUrl);


                var ProfieNode = doc.DocumentNode.SelectNodes("//div[@class='gsc_lcl']");//gsc_prf
                if (ProfieNode != null)
                {
                    foreach (var Prof in ProfieNode)
                    {
                        var x = Prof.InnerHtml;
                        if (x != null)
                            _return = x;
                    }


                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - GetAllGoogleScholarProfileDiv", ex.Message);
            }




            return _return;
        }

        public static string GetAllGoogleScholarProfileDivFromMemberResume(string email)
        {
            try
            {
                email = email.Trim().ToLower();
                string _return = "";


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList requestsList = web.Lists["MembersResumes"];


                                SPQuery query = new SPQuery();
                                query.Query = string.Concat(
                                                     @"<Where>
                                      <Eq>
                                         <FieldRef Name='Email' />
                                         <Value Type='Text'>" + email + @"</Value>
                                      </Eq>
                                   </Where>
                                ");



                                SPListItemCollection items = requestsList.GetItems(query);

                                if (items != null && items.Count > 0)
                                {
                                    _return = items[0]["GoogleScolarProfileDiv"] == null ? "" : items[0]["GoogleScolarProfileDiv"].ToString();
                                }





                            }
                        }
                    }
                });

                return _return;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - GetAllGoogleScholarProfileDivFromMemberResume", ex.Message);
                return "";
            }

        }


        static string FindGoogleScholarProfileByEmail(string email)
        {
            try
            {// You would need to implement a search mechanism to find the author's profile using their email.
             // This could involve web searching or querying Google Scholar's website.

                // Example: Google Scholar search URL
                string searchUrl = "https://scholar.google.com/scholar?q=email:" + email;

                // Perform a search and extract the author's profile URL from the search results.
                // Return the URL if found, or an empty string if not found.

                // For simplicity, let's assume a URL for testing:
                return "https://scholar.google.com/citations?user=vzD7zHcAAAAJ";



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "clsGoogleScolar - FindGoogleScholarProfileByEmail", ex.Message);
                return "";
            }

        }


    }
}





public class Article
{
    public string Title { get; set; }
    public string Authors { get; set; }
    public string PublicationYear { get; set; }
    public string CitationCount { get; set; }
    public string Url { get; set; }
    public string ID { get; internal set; }
    public string Magazine { get; internal set; }
    public string Email { get; internal set; }
}





