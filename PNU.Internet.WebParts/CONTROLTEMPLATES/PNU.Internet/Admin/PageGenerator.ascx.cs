using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class PageGenerator : UserControl
    {
        private StringBuilder _log = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Intentionally empty
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string listName = txtListName.Text.Trim();
                if (string.IsNullOrEmpty(listName))
                {
                    ShowMessage("يرجى إدخال اسم القائمة", "error");
                    return;
                }

                ProcessList(listName, chkOverwrite.Checked);

                pnlResults.Visible = true;
                litResults.Text = _log.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ عام: " + ex.Message, "error");
            }
        }

        private void ProcessList(string listName, bool overwrite)
        {
            // Elevated privileges to ensure page creation works
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.Url))
                using (SPWeb currentWeb = currentSite.OpenWeb("/ar/ITAdmin"))
                {
                    SPList list = currentWeb.Lists.TryGetList(listName);
                    if (list == null)
                    {
                        Log("القائمة غير موجودة: " + listName, "error");
                        return;
                    }

                    // Query unprocessed items
                    SPQuery query = new SPQuery
                    {
                        Query = @"<Where>
                                    <Eq>
                                        <FieldRef Name='IsProcessed'/>
                                        <Value Type='Boolean'>0</Value>
                                    </Eq>
                                  </Where>",
                        ViewFields = @"<FieldRef Name='Title'/>
                                       <FieldRef Name='PageFileName'/>
                                       <FieldRef Name='WebUrl'/>
                                       <FieldRef Name='UserControlPath'/>
                                       <FieldRef Name='PageLayoutUrl'/>",
                        RowLimit = 500
                    };

                    SPListItemCollection items = list.GetItems(query);
                    Log(string.Format("عدد العناصر للمعالجة: {0}", items.Count), "info");

                    foreach (SPListItem item in items)
                    {
                        ProcessSingleItem(item, overwrite);
                    }
                }
            });
        }

        private void ProcessSingleItem(SPListItem item, bool overwrite)
        {
            string title = SafeString(item["Title"]);
            string fileName = SafeString(item["PageFileName"]);
            string webUrl = SafeString(item["WebUrl"]);
            string userControlPath = SafeString(item["UserControlPath"]);
            string pageLayoutUrl = SafeString(item["PageLayoutUrl"]);

            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(webUrl)
                    || string.IsNullOrEmpty(userControlPath) || string.IsNullOrEmpty(pageLayoutUrl))
                {
                    Log("حقول مطلوبة مفقودة للعنصر: " + title, "error");
                    return;
                }

                // Ensure file name ends with .aspx
                if (!fileName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    fileName += ".aspx";

                using (SPSite targetSite = new SPSite(SPContext.Current.Site.MakeFullUrl(webUrl)))
                using (SPWeb targetWeb = targetSite.OpenWeb(webUrl))
                {
                    targetWeb.AllowUnsafeUpdates = true;

                    try
                    {
                        CreatePublishingPage(targetSite, targetWeb, title, fileName,
                            userControlPath, pageLayoutUrl, overwrite);

                        // Mark as processed
                        item["IsProcessed"] = true;
                        item.SystemUpdate(false);

                        Log(string.Format("تم إنشاء الصفحة بنجاح: {0} في {1}",
                            fileName, webUrl), "success");
                    }
                    finally
                    {
                        targetWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("فشل إنشاء {0}: {1}", fileName, ex.Message), "error");
            }
        }

        private void CreatePublishingPage(SPSite site, SPWeb web, string title,
            string fileName, string userControlPath, string pageLayoutUrl, bool overwrite)
        {
            PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
            PublishingPageCollection pages = pubWeb.GetPublishingPages();

            // Check existing page
            string pageServerRelativeUrl = pubWeb.PagesListName + "/" + fileName;
            SPFile existingFile = web.GetFile(web.ServerRelativeUrl.TrimEnd('/')
                + "/" + pageServerRelativeUrl);

            if (existingFile.Exists)
            {
                if (!overwrite)
                {
                    Log("الصفحة موجودة بالفعل، تم التخطي: " + fileName, "warning");
                    return;
                }
                // Delete existing to recreate
                if (existingFile.CheckOutType != SPFile.SPCheckOutType.None)
                    existingFile.UndoCheckOut();
                existingFile.Delete();
            }

            // Resolve the page layout from master page gallery
            SPFile layoutFile = site.RootWeb.GetFile(pageLayoutUrl);
            if (!layoutFile.Exists)
            {
                throw new Exception("تخطيط الصفحة غير موجود: " + pageLayoutUrl);
            }

            PageLayout layout = null;
            foreach (PageLayout pl in pubWeb.GetAvailablePageLayouts())
            {
                if (pl.ServerRelativeUrl.Equals(layoutFile.ServerRelativeUrl,
                    StringComparison.OrdinalIgnoreCase))
                {
                    layout = pl;
                    break;
                }
            }

            if (layout == null)
            {
                // Fallback: load directly
                layout = new PageLayout(site.RootWeb.GetFile(pageLayoutUrl).Item);
            }

            // Create the page
            PublishingPage newPage = pages.Add(fileName, layout);
            newPage.Title = title;
            newPage.Update();

            // Add ControlLoader WebPart and set UserControlPath property
            AddControlLoaderWebPart(web, newPage, userControlPath);

            // Check in, publish, approve
            SPFile pageFile = newPage.ListItem.File;
            if (pageFile.CheckOutType != SPFile.SPCheckOutType.None)
                pageFile.CheckIn("تم الإنشاء تلقائياً", SPCheckinType.MajorCheckIn);

            if (pageFile.Item.ParentList.EnableModeration)
                pageFile.Approve("تمت الموافقة تلقائياً");

            if (pageFile.Item.ParentList.EnableMinorVersions)
                pageFile.Publish("تم النشر تلقائياً");
        }

        private void AddControlLoaderWebPart(SPWeb web, PublishingPage page,
            string userControlPath)
        {
            using (SPLimitedWebPartManager wpManager =
                page.ListItem.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                // IMPORTANT: Replace with your WebPart's actual namespace and assembly
                // Example values — adjust to match your project
                string typeName = "PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart";
                string assemblyName = "PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3";

                // Instantiate WebPart via reflection so this control doesn't need a 
                // hard compile-time reference
                System.Reflection.Assembly asm =
                    System.Reflection.Assembly.Load(assemblyName);
                Type wpType = asm.GetType(typeName);

                if (wpType == null)
                    throw new Exception("لم يتم العثور على نوع WebPart: " + typeName);

                System.Web.UI.WebControls.WebParts.WebPart wp =
                    (System.Web.UI.WebControls.WebParts.WebPart)Activator.CreateInstance(wpType);

                wp.Title = "Control Loader WebPart";
                wp.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.None;

                // Set UserControlPath property via reflection
                var prop = wpType.GetProperty("UserControlPath");
                if (prop == null)
                    throw new Exception("الخاصية UserControlPath غير موجودة على WebPart");

                prop.SetValue(wp, userControlPath, null);

                // Add to the main content zone — adjust ZoneId to match your page layout
                // Common zones: "TopZone", "CenterZone", "Header", "MainZone"
                wpManager.AddWebPart(wp, "TopZone", 0);
            }
        }

        #region Helpers

        private string SafeString(object value)
        {
            return value == null ? string.Empty : value.ToString().Trim();
        }

        private void Log(string message, string cssClass)
        {
            _log.AppendFormat("<div class='{0}'>• {1}</div>",
                cssClass, Server.HtmlEncode(message));
        }

        private void ShowMessage(string message, string cssClass)
        {
            pnlResults.Visible = true;
            litResults.Text = string.Format("<div class='{0}'>{1}</div>",
                cssClass, Server.HtmlEncode(message));
        }

        #endregion
    }
}
