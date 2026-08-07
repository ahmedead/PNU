using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public class clsDynamicPages
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string SiteURL { get; set; }
        public string PageName { get; internal set; }
        public string PageTitle { get; internal set; }
        public string PageLayout { get; internal set; }
        public string LibraryName { get; internal set; }
        public string ControlLoaderURL { get; internal set; }
        public string UserControlPath { get; internal set; }
        public string Status { get; internal set; }

    }


    public static class busclsDynamicPages
    {
        public static List<clsDynamicPages> GetAllItems()
        {
            List<clsDynamicPages> _AllData = new List<clsDynamicPages>();

            using (SPSite site = new SPSite(Settings.RootSite))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    SPList reqList = oWeb.GetList(Settings.RootSite + Settings.DynamicPagesListURL);

                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                      <Neq>
                                         <FieldRef Name='Status' />
                                         <Value Type='Text'>Added</Value>
                                      </Neq>
                                   </Where>";
                    SPListItemCollection items = reqList.GetItems(query);

                    if (items == null || items.Count == 0)
                        _AllData = null;

                    _AllData = SPFactory.MapListItemsToClass<clsDynamicPages>(items);

                    return _AllData;
                }
            }
        }


        public static clsDynamicPages GetItemsByID(string ID)
        {
            using (SPSite site = new SPSite(Settings.siteUrl))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    SPQuery query = new SPQuery();
                    query.ViewFieldsOnly = true;
                    SPList MediaReqList = oWeb.GetList(Settings.RootSite + Settings.DynamicPagesListURL);

                    SPListItem item = MediaReqList.GetItemById(Convert.ToInt32(ID));
                    clsDynamicPages Data = SPFactory.MapListItemsToClass<clsDynamicPages>(item);
                    return Data;
                }
            }


        }

        public static void UpdateCurrentItem(clsDynamicPages _obj)
        {
            using (SPSite site = new SPSite(Settings.siteUrl))
            {
                using (SPWeb oWeb = site.OpenWeb())
                {
                    oWeb.AllowUnsafeUpdates = true;
                    SPQuery query = new SPQuery();
                    query.ViewFieldsOnly = true;
                    SPList MediaReqList = oWeb.GetList(Settings.RootSite + Settings.DynamicPagesListURL);

                    SPListItem item = MediaReqList.GetItemById(Convert.ToInt32(Convert.ToInt32(_obj.ID)));
                    item = SPFactory.MapClassToSPListItemForUpdating(item, _obj);
                    item.Update();
                    oWeb.AllowUnsafeUpdates = false;


                }
            }


        }



    }


}
