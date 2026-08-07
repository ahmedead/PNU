using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU_SP
{
    public static class Helper
    {
        public static SPListItemCollection LoadListDynamicByCML(string SiteURL, string ListName, ArrayList qryParams)
        {
            SPListItemCollection ItemCol = null;

            try
            {
                string whereQry = "<Where>{0}</Where>";

                for (int i = 0; i < qryParams.Count; i++)
                {
                    if (i < qryParams.Count - 1)
                        whereQry = String.Format(whereQry, "<And>" + qryParams[i] +
                          "{0}</And>");
                    else
                        whereQry = String.Format(whereQry, qryParams[i]);
                }

                whereQry = String.Format(whereQry, String.Empty);
                SPQuery qry = new SPQuery();

                qry.Query = "<OrderBy><FieldRef Name='ID' " +
                  "Ascending='False' /></OrderBy>" + whereQry;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            ItemCol = web.Lists[ListName].GetItems(qry);
                        }
                    }
                });


            }
            catch (Exception ex)
            {


            }
            return ItemCol;
        }
    }
}
