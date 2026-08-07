using System;
using Microsoft.SharePoint;
using PNU.Internet.Search.Indexer;
using Portal.Main.Helper;

namespace PNU.Internet.Search.Receivers
{
    /// <summary>
    /// Bind this receiver to every list/library you want indexed. It
    /// updates the custom search index whenever an item is added,
    /// updated or deleted.
    ///
    /// Wire it up at runtime via SearchEventReceiverInstaller.Install(list)
    /// (see SearchEventReceiverInstaller.cs) or declaratively in elements.xml.
    /// </summary>
    public class SearchListEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
            ProcessUpsert(properties, "ItemAdded");
            base.ItemAdded(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            ProcessUpsert(properties, "ItemUpdated");
            base.ItemUpdated(properties);
        }

        public override void ItemCheckedIn(SPItemEventProperties properties)
        {
            // For publishing pages, indexing should follow check-in/publish
            ProcessUpsert(properties, "ItemCheckedIn");
            base.ItemCheckedIn(properties);
        }

        public override void ItemDeleting(SPItemEventProperties properties)
        {
            // Soft-delete BEFORE the item is gone, so we have the IDs.
            try
            {
                if (properties == null || properties.ListItem == null) return;
                var item = properties.ListItem;
                if (IsPublishingPage(item))
                    SearchIndexer.RemovePage(item.Web.ID, item.ID);
                else
                    SearchIndexer.RemoveListItem(item.Web.ID,
                        item.ParentList.ID, item.ID);
            }
            catch (Exception ex)
            {
                
            }
            base.ItemDeleting(properties);
        }

        // ------------------------------------------------------------------
        private void ProcessUpsert(SPItemEventProperties properties, string trigger)
        {
            try
            {
                if (properties == null || properties.ListItem == null) return;
                this.EventFiringEnabled = false;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    using (SPWeb  web  = site.OpenWeb(properties.RelativeWebUrl))
                    {
                        SPList list = web.Lists[properties.ListId];
                        SPListItem item = list.GetItemById(properties.ListItemId);

                        if (IsPublishingPage(item))
                            SearchIndexer.IndexSinglePage(item);
                        else
                            SearchIndexer.IndexSingleListItem(item);
                    }
                });
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                this.EventFiringEnabled = true;
            }
        }

        private static bool IsPublishingPage(SPListItem item)
        {
            try
            {
                return item != null
                    && item.ParentList != null
                    && item.ParentList.BaseTemplate == SPListTemplateType.DocumentLibrary
                    && item.ParentList.Title.Equals("Pages",
                            StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }
    }
}
