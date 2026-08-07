using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.Code_File
{
    public class GetSectors
    {
         internal static List<SectorsClass> GetSectorslist(List<SectorsClass> SectorsList)
        {
            try
            {
                GetAllItem("Sectors", " /ar")
               .OrderBy(item => Convert.ToInt32(item["ID"].ToString())).ToList().ForEach(item => SectorsList.Add(new SectorsClass
               {
                   ID = item["ID"] != null ? item["ID"].ToString() : string.Empty,
                   Title = item["Title"] != null ? item["Title"].ToString() : string.Empty,
                   TitleEn = item["TitleEn"] != null ? item["TitleEn"].ToString() : string.Empty,
                 
               }));

               return SectorsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class SectorsClass
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            
        }
        internal static ReadOnlyCollection<SPListItem> GetAllItem(string ListName, string webPath)
        {

            SPWeb elevatedWeb = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    elevatedWeb = site.OpenWeb(webPath);
                }
            });
            if (elevatedWeb.Lists.TryGetList(ListName) != null)
            {
                SPList list = elevatedWeb.Lists[ListName];
                return list.Items.Cast<SPListItem>().ToList().AsReadOnly();
            }
            return null;
        }


    }
}
    

