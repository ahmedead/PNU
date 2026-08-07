using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU_SP
{
    public static class LookupManager
    {

        public static SPListItemCollection LoadLookup(string SiteURL, string ListName)
        {
            try
            {
                using (SPSite osite = new SPSite(SiteURL))
                {
                    using (SPWeb oweb = osite.OpenWeb())
                    {
                        SPQuery qry = new SPQuery();
                        SPList list = oweb.Lists[ListName];
                      
                        SPListItemCollection itemcol = list.GetItems();
                        return itemcol;
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }


    }
}
