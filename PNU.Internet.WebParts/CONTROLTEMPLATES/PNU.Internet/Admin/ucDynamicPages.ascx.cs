using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Microsoft.SharePoint.WebControls;



namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucDynamicPages : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public SPFile CreatePage(string siteUrl, string pageTitle, string pageLayout)
        {
            // Connect to the SharePoint site
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    // Get the Pages library

                    string LibraryName = "الصفحات";//IsArabic ? "الصفحات" : "Pages";

                    SPList pagesLibrary = web.Lists[LibraryName];

                    // Create a new publishing page
                    string pageUrl = pagesLibrary.RootFolder.ServerRelativeUrl + "/" + pageTitle + ".aspx";
                    SPFile newPage = pagesLibrary.RootFolder.Files.Add(pageUrl, SPTemplateFileType.StandardPage);


                    // Set the page properties
                    newPage.Item["Title"] = pageTitle;
                    newPage.Item["PublishingPageLayout"] = GetPageLayoutUrl(web, pageLayout); //pageLayout;
                    newPage.Item.Update();

                    SPFile PageFile = web.GetFile("Pages/" + pageTitle + ".aspx");
                    return PageFile;
                }
            }
        }

        private string GetPageLayoutUrl(SPWeb web, string pageLayoutName)
        {
            //SPList masterPageGallery = web.GetCatalog(SPListTemplateType.MasterPageCatalog);



            //// Create a query to filter by ContentType and FileLeafRef (file name)
            //SPQuery query = new SPQuery();
            //query.Query = string.Format(
            //    $@"<Where>
            //          <Eq>
            //             <FieldRef Name='Title' />
            //             <Value Type='Text'>{pageLayoutName}</Value>
            //          </Eq>
            //       </Where>");

            //SPListItemCollection items = masterPageGallery.GetItems(query);

            //if (items.Count > 0)
            //{
            //    SPListItem pageLayoutItem = items[0];
            //    return pageLayoutItem.File.ServerRelativeUrl;
            //}

            if (PublishingWeb.IsPublishingWeb(web))
            {
                PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                PageLayout[] layouts = publishingWeb.GetAvailablePageLayouts();

                foreach (PageLayout layout in layouts)
                {
                    if (layout.Name.Equals(pageLayoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        return layout.ServerRelativeUrl;
                    }
                }
            }

            

            return string.Empty;
        }



        public void CreatePublishingPage(string siteUrl, string pageTitle, string pageLayout)
        {
            // Connect to the SharePoint site
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    // Enable publishing features on the web
                    if (PublishingWeb.IsPublishingWeb(web))
                    {
                        PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                        SPList pagesLibrary = publishingWeb.PagesList;

                        // Create a new publishing page
                        string pageUrl = pagesLibrary.RootFolder.ServerRelativeUrl + "/" + pageTitle + ".aspx";
                        PublishingPage newPage = publishingWeb.AddPublishingPage(pageUrl, GetPageLayoutUrlForPublishing(publishingWeb, pageLayout));

                        // Set the page properties
                        newPage.Title = pageTitle;
                        newPage.Update();
                    }
                }
            }
        }

        private PageLayout GetPageLayoutUrlForPublishing(PublishingWeb web, string pageLayoutName)
        {
            // Retrieve the server-relative URL of the page layout
            PageLayout[] layouts = web.GetAvailablePageLayouts();

            foreach (PageLayout layout in layouts)
            {
                if (layout.Name.Equals(pageLayoutName, StringComparison.OrdinalIgnoreCase))
                {
                    return layout;
                }
            }

            return null;
        }
        public static bool IsArabic
        {
            get
            {
                return SPContext.Current.Web.Language == 1025;
            }
        }
        protected void Button1_Click(object sender, EventArgs e) => SPSecurity.RunWithElevatedPrivileges(delegate
        {

            //string SiteUrl = "http://sp2019:90/ar/";
            List<clsDynamicPages> _AllPages = busclsDynamicPages.GetAllItems();
            if (_AllPages == null)
                return;
            if (_AllPages.Count == 0)
                return;
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                foreach (clsDynamicPages _Page in _AllPages)
                {
                    using (SPWeb web = site.OpenWeb(_Page.SiteURL))
                    {

                        web.AllowUnsafeUpdates = true;

                        //CreatePublishingPage(SiteUrl, "Test3", "BlankWebPartPage.aspx");//CreatePage(SiteUrl, "Test2", "BlankWebPartPage.aspx");//web.GetFile("Pages/Welcome.aspx");


                        SPList pagesLibrary = web.Lists[_Page.LibraryName];

                        // Create a new publishing page
                        string pageUrl = pagesLibrary.RootFolder.ServerRelativeUrl + "/" + _Page.PageName + ".aspx";
                        SPFile newPage = pagesLibrary.RootFolder.Files.Add(pageUrl, SPTemplateFileType.StandardPage);


                        // Set the page properties
                        newPage.Item["Title"] = _Page.PageTitle;
                        newPage.Item["PublishingPageLayout"] = GetPageLayoutUrl(web, _Page.PageLayout);
                        newPage.Item.Update();

                        SPFile PageFile = web.GetFile("Pages/" + _Page.PageName + ".aspx");

                        using (SPLimitedWebPartManager manager = PageFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                        {
                            string errMsg = string.Empty;
                            SPFile myWebPart = web.ParentWeb.GetFile(_Page.ControlLoaderURL);
                            XmlTextReader read = new XmlTextReader(myWebPart.OpenBinaryStream());
                            var wp = manager.ImportWebPart(read, out errMsg);

                            // Check if the web part is of type WebPart
                            if (wp is ControlLoaderWebPart.ControlLoaderWebPart visualWebPart)
                            {
                                // Set extended properties
                                visualWebPart.Title = "";
                                visualWebPart.UserControlPath = _Page.UserControlPath;

                                manager.AddWebPart(visualWebPart, "<Webpart Zone>", 0);
                                manager.SaveChanges(visualWebPart);
                            }
                            else
                            {
                                manager.AddWebPart(wp, "<Webpart Zone>", 1);
                                manager.SaveChanges(wp);
                            }

                            //_Page.Status = "Added";
                            //busclsDynamicPages.UpdateCurrentItem(_Page);


                            SPListItem pageItem = newPage.Item;
                            if (pageItem != null)
                            {
                                if (pageItem.File.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                {
                                    pageItem.File.CheckIn("Checked in by system");
                                }

                                //if (pageItem.ModerationInformation.Status == SPModerationStatusType.Draft)
                                //{
                                //    pageItem.File.Approve("Approved by system");
                                //}

                                pageItem.File.Publish("Published by system");
                            }
                        }

                        web.AllowUnsafeUpdates = false;

                    }

                }

            }
        });

        protected void Button2_Click(object sender, EventArgs e) => SPSecurity.RunWithElevatedPrivileges(delegate
        {
            string SiteUrl = "http://sp2019:90/ar/";
            using (SPSite site = new SPSite(SiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    //CreatePublishingPage(SiteUrl, "Test3", "BlankWebPartPage.aspx");//CreatePage(SiteUrl, "Test2", "BlankWebPartPage.aspx");//web.GetFile("Pages/Welcome.aspx");
                    string pageTitle = "Test4";
                    string pageLayout = "WebPartPageLayout.aspx";

                    // Enable publishing features on the web
                    if (PublishingWeb.IsPublishingWeb(web))
                    {
                        PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                        SPList pagesLibrary = publishingWeb.PagesList;

                        // Create a new publishing page
                        string pageUrl = pagesLibrary.RootFolder.ServerRelativeUrl + "/" + pageTitle + ".aspx";
                        PublishingPage newPage = publishingWeb.AddPublishingPage(pageUrl, GetPageLayoutUrlForPublishing(publishingWeb, pageLayout));

                        // Set the page properties
                        newPage.Title = pageTitle;
                        newPage.Update();
                    }
                    SPFile page = web.GetFile("Pages/" + "Test2" + ".aspx");
                    using (SPLimitedWebPartManager manager = page.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        string errMsg = string.Empty;
                        SPFile myWebPart = web.ParentWeb.GetFile("_catalogs/wp/MediaReq_ControlLoaderWebPart.webpart");
                        XmlTextReader read = new XmlTextReader(myWebPart.OpenBinaryStream());
                        var wp = manager.ImportWebPart(read, out errMsg);
                        manager.AddWebPart(wp, "<Webpart Zone>", 0);
                        manager.SaveChanges(wp);
                    }

                    web.AllowUnsafeUpdates = false;
                }
            }
        });


        


        public void AddWebPartToPublishingPage(string siteUrl, string pageUrl, string webPartTitle, string webPartAssembly, string webPartType, string webPartZoneId, int webPartZoneIndex)
        {
            // Connect to the SharePoint site
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    // Get the publishing page
                    PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                    PublishingPage publishingPage = publishingWeb.GetPublishingPage(pageUrl);

                    if (publishingPage != null)
                    {
                        // Load the web part page
                        using (SPLimitedWebPartManager webPartManager = publishingPage.ListItem.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
                        {
                            // Create a new instance of the web part
                            System.Web.UI.WebControls.WebParts.WebPart webPart = Activator.CreateInstance(Type.GetType(webPartAssembly, true)) as System.Web.UI.WebControls.WebParts.WebPart;

                            // Set the web part properties
                            webPart.Title = webPartTitle;

                            // Add the web part to the page
                            webPartManager.AddWebPart(webPart, webPartZoneId, webPartZoneIndex);

                            // Save the changes
                            webPartManager.SaveChanges(webPart);
                        }
                    }
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //SPHelper.SendMail();
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (txtEncrypt.Text != "")
            {
                txtDecrypt.Text = PortalHelper.Encrypt(txtEncrypt.Text);
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (txtEncrypt.Text != "")
            {
                txtDecrypt.Text = PortalHelper.Decrypt(txtEncrypt.Text);
            }
        }






    }
}
