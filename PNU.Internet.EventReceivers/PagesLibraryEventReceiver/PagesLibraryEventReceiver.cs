using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System;
using System.Security.Permissions;

namespace PNU.Internet.EventReceivers.PagesLibraryEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class PagesLibraryEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);
        }

        /// <summary>
        /// An item was added
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    SPListItem newPage = properties.ListItem;

                    if (newPage != null && properties.Web.Url.Contains("/ar"))
                    {
                        string enSiteUrl = properties.Web.Url.Replace("/ar", "/en");

                        using (SPSite enSite = new SPSite(enSiteUrl))
                        {
                            using (SPWeb enWeb = enSite.OpenWeb())
                            {
                                enWeb.AllowUnsafeUpdates = true;
                                SPList enPagesLibrary = enWeb.Lists.TryGetList("Pages");

                                if (enPagesLibrary != null)
                                {
                                    // Copy the file content to the "en" site
                                    SPFile sourceFile = newPage.File;
                                    byte[] fileContent = sourceFile.OpenBinary();
                                    SPFolder enFolder = enPagesLibrary.RootFolder;

                                    SPFile newFile = enFolder.Files.Add(newPage.File.Name, fileContent, true);

                                    SPListItem enPage = newFile.Item;


                                    // Set metadata fields
                                    if (enPage.Fields.ContainsField("Title")) enPage["Title"] = newPage["Title"];
                                    if (enPage.Fields.ContainsField("PublishingPageLayout")) enPage["PublishingPageLayout"] = newPage["PublishingPageLayout"];

                                    enPage.Update();

                                    enWeb.AllowUnsafeUpdates = false;


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0,
                        new SPDiagnosticsCategory("Event Receiver", TraceSeverity.Unexpected, EventSeverity.Error),
                        TraceSeverity.Unexpected,
                        $"Error in ItemAdded Event Receiver: {ex.Message}\nStack Trace: {ex.StackTrace}",
                        ex.StackTrace);
                }
            });
        }

    }
}