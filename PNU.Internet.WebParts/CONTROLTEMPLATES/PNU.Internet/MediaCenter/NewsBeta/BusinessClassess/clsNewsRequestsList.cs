using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.BusinessClassess
{
    public class clsRequestsListNewsBeta
    {
        public string ID { get; set; } = "";
        public string Title { get; set; } = "";
        public string Title_EN { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Summary_EN { get; set; } = "";
        public string MediaContent { get; set; } = "";
        public string MediaContent_EN { get; set; } = "";
        public string FacultyName { get; set; } = "";
        public string FacultyName_EN { get; set; } = "";
        public string MainCategory { get; set; } = "";
        public string MainCategory_EN { get; set; } = "";
        public string MediaTypes { get; set; } = "";
        public string MediaTypes_EN { get; set; } = "";
        public string NextRequestStatus { get; set; } = "";


        public string AdditionalFileURL { get; set; } = "";

        public bool IsVideo
        {
            get
            {
                if (MediaTypes_EN != null)
                {
                    if (MediaTypes_EN.Contains("Video"))
                    {
                        return true;
                    }
                    else
                        return false;
                }

                return false;
            }
        }

        public bool IsYouTubeVideo
        {
            get
            {
                if (MediaTypes_EN != null)
                {
                    if (MediaTypes_EN.Contains("Video"))
                    {
                        if (_VideoURL != null)
                            if (_VideoURL.Contains("youtube"))
                                return true;

                        return false;
                    }
                    else
                        return false;
                }

                return false;
            }
        }


        public bool IsHome { get; set; } = false;



        public string VideoVisiable
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }


        public string MP4Visible
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("mp4"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string ImgVisible
        {
            get
            {

                if (AttachmentURL == null)
                {
                    return "none";
                }

                return "block";
            }
        }

        public string YouTubeVisible
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("youtube"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string TitleVisible
        {
            get
            {

                if (MediaContent == null)
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }


        private string _VideoURL;

        public string VideoURL
        {
            get
            {

                if (_VideoURL == null)
                {
                    return string.Empty;
                }

                return _VideoURL;
            }
            set
            {

                _VideoURL = value;
            }
        }

        public string VideoID
        {
            get
            {

                if (_VideoURL == null)
                {
                    return string.Empty;
                }
                if (_VideoURL.Contains("https://www.youtube.com/embed/"))
                {
                    return _VideoURL.Replace("https://www.youtube.com/embed/", "").ToString();
                }
                if (_VideoURL.Contains("https://www.youtube.com/watch?v="))
                {
                    return _VideoURL.Replace("https://www.youtube.com/watch?v=", "").ToString();
                }
                if (_VideoURL.Contains("https://www.youtube.com/shorts/"))
                {
                    return _VideoURL.Replace("https://www.youtube.com/shorts/", "").ToString();
                }
                return "";
            }
            set
            {

                _VideoURL = value;
            }
        }

        public string Created { get; set; }
        //Created
        //CreatedDate
        public string CreatedDate
        {
            get
            {

                if (Created == null)
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(Created.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }

        }


        public string DetailsURL
        {
            get
            {
                if (FacultyName_EN == null || FacultyName_EN == "")
                {
                    if (CatID == "2")
                    {
                        if (PortalHelper.IsArabic)
                            return "/ar/MediaCenter/NewsBeta/Pages/DigitalMediaDetails.aspx?RequestID=" + ID;
                        else
                            return "/en/MediaCenter/News/Pages/DigitalMediaDetails.aspx?RequestID=" + ID;
                    }
                    else
                    {
                        if (PortalHelper.IsArabic)
                            return "/ar/MediaCenter/NewsBeta/Pages/NewsDetails.aspx?RequestID=" + ID;
                        else
                            return "/en/MediaCenter/News/Pages/NewsDetails.aspx?RequestID=" + ID;
                    }

                }
                else if (FacultyName_EN == "Media Center")
                {
                    if (CatID == "2")
                    {
                        if (PortalHelper.IsArabic)
                            return "/ar/MediaCenter/NewsBeta/Pages/DigitalMediaDetails.aspx?RequestID=" + ID;
                        else
                            return "/en/MediaCenter/News/Pages/DigitalMediaDetails.aspx?RequestID=" + ID;
                    }
                    else
                    {
                        if (PortalHelper.IsArabic)
                            return "/ar/MediaCenter/NewsBeta/Pages/NewsDetails.aspx?RequestID=" + ID;
                        else
                            return "/en/MediaCenter/News/Pages/NewsDetails.aspx?RequestID=" + ID;
                    }

                }
                else
                {
                    //if (PortalHelper.IsArabic)
                    //    return "/ar/MediaCenter/NewsBeta/Pages/NewsDetails.aspx?RequestID=" + ID;
                    //else
                    //    return "/en/MediaCenter/News/Pages/NewsDetails.aspx?RequestID=" + ID;
                    if (IsHome == true)
                    {
                        if (PortalHelper.IsArabic)
                            return "/ar/MediaCenter/NewsBeta/Pages/NewsDetails.aspx?RequestID=" + ID;
                        else
                            return "/en/MediaCenter/News/Pages/NewsDetails.aspx?RequestID=" + ID;

                    }
                    else
                    {
                        if (PortalHelper.IsArabic)
                            return "NewsDetails.aspx?RequestID=" + ID;
                        else
                            return "NewsDetails.aspx?RequestID=" + ID;
                    }
                }



            }

        }


        public string DisplayCatName
        {
            get
            {
                if (PortalHelper.IsArabic)
                    return MainCategory;
                else
                    return MainCategory_EN;


            }

        }

        public string Author { get; set; }

        private string _MediaDate;

        public string MediaDate
        {
            get
            {

                if (_MediaDate == null)
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(_MediaDate.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }
            set
            {

                _MediaDate = value;
            }
        }
        public string UserComments { get; set; }
        public string RequestStatus { get; set; }
        public string ApprovalGroupName { get; set; }
        public string RequesterName { get; set; }

        public string RequesterNameDisplay
        {
            get
            {
                if (this.RequesterName == null || this.RequesterName == "")
                    return "";

                else
                    return RequesterName;

            }
        }
        public string RequesterEmail { get; set; }
        public string AttachmentURL { get; set; }

        public string ClassActive
        {
            get
            {
                if (this.ID == null || this.ID == "")
                    return "";
                if (this.ID == "1")
                    return " active show";
                else
                    return "";

            }
        }

        public string CatID
        {
            get
            {

                if (MainCategory == null || MainCategory == "")
                {
                    return "1";
                }
                else if (MainCategory == "الأخبار الرئيسية")
                {
                    return "1";
                }
                else
                {
                    return "2";
                }

            }
        }
    }

    public class clsAdvertisementsNewsBeta
    {
        public string ID { get; set; } = "";
        public string Title { get; set; } = "";
        public string Title_EN { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Summary_EN { get; set; } = "";
        public string MediaContent { get; set; } = "";
        public string MediaContent_EN { get; set; } = "";
        public string FacultyName { get; set; } = "";
        public string FacultyName_EN { get; set; } = "";
        public string MainCategory { get; set; } = "";
        public string MainCategory_EN { get; set; } = "";
        public string MediaTypes { get; set; } = "";
        public string MediaTypes_EN { get; set; } = "";

        public string PublisherName { get; set; } = "";
        public string PublisherPosition { get; set; } = "";

        public string VideoID
        {
            get
            {

                if (_VideoURL == null)
                {
                    return string.Empty;
                }
                if (_VideoURL.Contains("https://www.youtube.com/embed/"))
                {
                    return _VideoURL.Replace("https://www.youtube.com/embed/", "").ToString();
                }
                if (_VideoURL.Contains("https://www.youtube.com/watch?v="))
                {
                    return _VideoURL.Replace("https://www.youtube.com/watch?v=", "").ToString();
                }
                if (_VideoURL.Contains("https://www.youtube.com/shorts/"))
                {
                    return _VideoURL.Replace("https://www.youtube.com/shorts/", "").ToString();
                }

                //https://www.youtube.com/shorts/dYby7KpYiQo
                return "";
            }
            set
            {

                _VideoURL = value;
            }
        }

        public bool IsVideo
        {
            get
            {
                if (MediaTypes_EN != null)
                {
                    if (MediaTypes_EN.Contains("Video"))
                        return true;
                    else
                        return false;
                }

                return false;
            }
        }


        public bool IsHome { get; set; } = false;



        public string VideoVisiable
        {
            get
            {

                if (_VideoURL == null || _VideoURL == "")
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }

        public bool IsYouTubeVideo
        {
            get
            {
                if (MediaTypes_EN != null)
                {
                    if (MediaTypes_EN.Contains("Video"))
                    {
                        if (_VideoURL != null)
                            if (_VideoURL.Contains("youtube"))
                                return true;

                        return false;
                    }
                    else
                        return false;
                }

                return false;
            }
        }

        public string MP4Visible
        {
            get
            {

                if (_VideoURL == null || _VideoURL == "")
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("mp4"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string ImgVisible
        {
            get
            {

                if (AttachmentURL == null || AttachmentURL == "")
                {
                    return "none";
                }

                return "block";
            }
        }

        public string ImgDisplay
        {
            get
            {

                if (AttachmentURL == null || AttachmentURL == "")
                {
                    return "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg";
                }

                return AttachmentURL;
            }
        }

        public string PublisherVisible
        {
            get
            {

                if (PublisherName == null || PublisherName == "")
                {
                    return "none";
                }

                return "block";
            }
        }

        public string YouTubeVisible
        {
            get
            {

                if (_VideoURL == null || _VideoURL == "")
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("youtube"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string TitleVisible
        {
            get
            {

                if (MediaContent == null || MediaContent == "")
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }


        private string _VideoURL;

        public string VideoURL
        {
            get
            {

                if (_VideoURL == null || _VideoURL == "")
                {
                    return string.Empty;
                }

                return _VideoURL;
            }
            set
            {

                _VideoURL = value;
            }
        }

        public string Created { get; set; }
        //Created
        //CreatedDate
        public string CreatedDate
        {
            get
            {

                if (Created == null || Created == "")
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(Created.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }

        }


        public string DetailsURL
        {
            get
            {
                if (PortalHelper.IsArabic)
                    return "/ar/MediaCenter/Pages/AdvertisementDetails.aspx?RequestID=" + ID;
                else
                    return "/en/MediaCenter/Pages/AdvertisementDetails.aspx?RequestID=" + ID;



            }

        }


        public string DisplayCatName
        {
            get
            {
                if (PortalHelper.IsArabic)
                    return MainCategory;
                else
                    return MainCategory_EN;


            }

        }

        public string Author { get; set; }

        private string _MediaDate;

        public string MediaDate
        {
            get
            {

                if (_MediaDate == null || _MediaDate == "")
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(_MediaDate.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }
            set
            {

                _MediaDate = value;
            }
        }
        public string UserComments { get; set; } = "";
        public string RequestStatus { get; set; } = "";
        public string ApprovalGroupName { get; set; } = "";
        public string RequesterName { get; set; } = "";

        public string RequesterNameDisplay
        {
            get
            {
                if (this.RequesterName == null || this.RequesterName == "")
                    return "";

                else
                    return RequesterName;

            }
        }
        public string RequesterEmail { get; set; } = "";
        public string AttachmentURL { get; set; } = "";

        public string ClassActive
        {
            get
            {
                if (this.ID == null || this.ID == "")
                    return "";
                if (this.ID == "1")
                    return " active show";
                else
                    return "";

            }
        }

        public string CatID
        {
            get
            {

                if (MainCategory == null || MainCategory == "")
                {
                    return "1";
                }
                else if (MainCategory == "الأخبار الرئيسية")
                {
                    return "1";
                }
                else
                {
                    return "2";
                }

            }
        }
    }

    public class clsLookUPNewsBeta
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string TitleEn { get; set; }




    }

    public class PnuUserNewsBeta
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }


    }

    public class clsAllRequestsListNewsBeta
    {
        public string ID { get; set; }
        public string MainCategory { get; set; }
        public string MainCategory_EN { get; set; }
        public List<clsRequestsListNewsBeta> Requests { get; set; }


        public List<dataEvent> Events { get; set; }

        public List<clsAdvertisementsNewsBeta> Advertisements { get; set; }

        public string ClassActive
        {
            get
            {
                if (this.ID == "1")
                    return " active";
                else
                    return "";

            }
        }
        public string ClassActiveShow
        {
            get
            {
                if (this.ID == "1")
                    return " active show";
                else
                    return "";

            }
        }


        public string AllNewsLink
        {
            get
            {
                return SPFactory.GetSiteURL() + "MediaCenter/Pages/AllNews.aspx?Id=" + this.ID;

            }
        }

        public string AllEventsLink
        {
            get
            {
                return SPFactory.GetSiteURL() + "MediaCenter/Pages/LatestAdvertisements.aspx";

            }
        }
        //ClassActive


    }

    public static class busclsRequestsListNewsBeta
    {

        public static string RequestsListName = "RequestsList";
        public static string MediaTypesListName = "MediaTypes";
        public static string MainCategoryListName = "MainCategory";
        public static string SiteURL = "/ar/MediaCenter/NewsBeta/";
        public static clsRequestsListNewsBeta GetItemByID(string SiteURL,string RequestsListName, string ID)    
        {
            try
            {
                SPListItem objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists[RequestsListName];
                            objNew = reqList.GetItemById(Convert.ToInt32(ID));
                        }
                    }
                });
                if (objNew == null)
                    return null;
                clsRequestsListNewsBeta _Data = SPFactory.MapListItemsToClass<clsRequestsListNewsBeta>(objNew);
                return _Data;

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetItemByID", ex.Message);
                return null;
            }




        }

        public static List<clsLookUPNewsBeta> GetAllItemsLookup(string SiteURL,string ListName)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = @"<OrderBy>
                          <FieldRef Name='ItemOrder' Ascending='False' />
                       </OrderBy>";
                return SPFactory.GetAllDataByQuery<clsLookUPNewsBeta>(SiteURL, ListName, query);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItemsLookup", ex.Message);
                return null;
            }

        }

        public static List<clsRequestsListNewsBeta> GetAllItems(string SiteURL, string RequestsListName, int RowLimit = 0)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = @"<Where>
                          <Eq>
                             <FieldRef Name='RequestStatus' />
                             <Value Type='Choice'>Approved</Value>
                          </Eq>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                //else
                //    query.RowLimit = 20;
                return SPFactory.GetAllDataByQuery<clsRequestsListNewsBeta>(SiteURL, RequestsListName, query);


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItems", ex.Message);
                return null;
            }


        }

        public static List<clsRequestsListNewsBeta> GetAllItemsIsHome(string SiteURL, string RequestsListName, int RowLimit = 0) 
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = @"<Where>
                          <And>
                             <Eq>
                                <FieldRef Name='RequestStatus' />
                                <Value Type='Choice'>Approved</Value>
                             </Eq>
                             <Eq>
                                <FieldRef Name='IsHome' />
                                <Value Type='Boolean'>1</Value>
                             </Eq>
                          </And>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                //else
                //    query.RowLimit = 20;
                return SPFactory.GetAllDataByQuery<clsRequestsListNewsBeta>(SiteURL, RequestsListName, query);


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItems", ex.Message);
                return null;
            }


        }

        public static List<clsRequestsListNewsBeta> GetAllItemsIsHomePending(string SiteURL, string RequestsListName, int RowLimit = 0)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = @"<Where>
                          <And>
                             <Eq>
                                <FieldRef Name='RequestStatus' />
                                <Value Type='Choice'>Pending</Value>
                             </Eq>
                             <Eq>
                                <FieldRef Name='IsHome' />
                                <Value Type='Boolean'>1</Value>
                             </Eq>
                          </And>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                //else
                //    query.RowLimit = 20;
                return SPFactory.GetAllDataByQuery<clsRequestsListNewsBeta>(SiteURL, RequestsListName, query);


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItems", ex.Message);
                return null;
            }


        }

        public static List<clsRequestsListNewsBeta> GetCollegeNews(string SiteURL, string RequestsListName, string CollegeCode, int RowLimit = 0)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = $@"<Where>
                          <And>
                             <Eq>
                                <FieldRef Name='RequestStatus' />
                                <Value Type='Choice'>Approved</Value>
                             </Eq>
                             <Eq>
                                <FieldRef Name='FacultyName_EN' />
                                <Value Type='Text'>{CollegeCode}</Value>
                             </Eq>
                          </And>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                return SPFactory.GetAllDataByQuery<clsRequestsListNewsBeta>(SiteURL, RequestsListName, query);


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItems", ex.Message);
                return null;
            }


        }

        public static List<clsRequestsListNewsBeta> GetAllItemsByMediaCategory(string SiteURL, string RequestsListName, string MediaCategory, int RowLimit = 0)
        {
            try
            {
                SPQuery query = new SPQuery();
                
                query.Query = $@"<Where>
                          <And>
                             <Eq>
                                <FieldRef Name='IsHome' />
                                <Value Type='Boolean'>1</Value>
                             </Eq>
                             <And>
                                <Eq>
                                   <FieldRef Name='RequestStatus' />
                                   <Value Type='Choice'>Approved</Value>
                                </Eq>
                                <Eq>
                                   <FieldRef Name='MainCategory' />
                                   <Value Type='Text'>{MediaCategory}</Value>
                                </Eq>
                             </And>
                          </And>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                else
                    query.RowLimit = 20;
                return SPFactory.GetAllDataByQuery<clsRequestsListNewsBeta>(SiteURL, RequestsListName, query);


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetItemByID", ex.Message);
                return null;
            }


        }

        public static List<clsRequestsListNewsBeta> GetAllItemsByMediaCategoryNotMainCat(string SiteURL, string RequestsListName, int RowLimit = 0)
        {
            try
            {
                //string ListName = "DigitalMedia";
                SPQuery query = new SPQuery();

                query.Query = $@"
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                SPListItemCollection objNew;
                List<clsRequestsListNewsBeta> _returnList = new List<clsRequestsListNewsBeta>();
                if (RowLimit != 0)
                    query.RowLimit = Convert.ToUInt32(RowLimit);
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists[RequestsListName];
                            objNew = reqList.GetItems(query);

                            if (objNew != null && objNew.Count > 0)
                            {
                                _returnList = SPFactory.MapListItemsToClass<clsRequestsListNewsBeta>(objNew);
                            }
                        }
                    }
                });




                return _returnList;


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetAllItemsByMediaCategoryNotMainCat", ex.Message);
                return null;
            }


        }

        public static List<PnuUserNewsBeta> GetUsersInGroup(string groupName)
        {
            try
            {
                List<PnuUserNewsBeta> UsersList = new List<PnuUserNewsBeta>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            SPUserCollection userColl = web.Groups[groupName].Users;

                            foreach (SPUser user in userColl)
                            {
                                var Pnuuser = new PnuUserNewsBeta()
                                {
                                    Id = user.ID,
                                    LoginName = user.LoginName,//ExtractLoginName(user.LoginName),
                                    DisplayName = user.Name,
                                    Email = user.Email
                                };

                                UsersList.Add(Pnuuser);
                            }


                        }
                    }
                });
                return UsersList;

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsRequestsList - GetItemByID", ex.Message);
                return null;
            }

        }



    }


}
