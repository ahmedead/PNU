using Microsoft.SharePoint;
using PNU.Workflow.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Linq;


namespace PNU.Workflow.Classes
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
        public static string ExtractLoginName(this String value)
        {
            int startIndex = value.IndexOf("w|") + 2;
            int length = value.Length - startIndex;
            // exclude \\ from manager profile property value 
            string newValue = value.Substring(startIndex, length);
            return newValue;
        }
        public static void AddErrorToWindowsLog(string exceptionMsg, string exceptionStackTrace, string systemName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                try
                {
                    System.Diagnostics.EventLog.WriteEntry(systemName, string.Format("Exception Msg:{0}. \r\n Exception Stack Trace: {1}", exceptionMsg, exceptionStackTrace), System.Diagnostics.EventLogEntryType.Error);
                }
                catch (Exception ex)
                {

                }
            });

        }
        public static void AddErrorToWindowsLog(string exceptionMsg, string exceptionStackTrace, string serviceStatus, string ServiceStatusDescription, string systemStatus, string systemStatusDescription)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                try
                {
                    string error = string.Format("Exception Message: {0} \r\n Service Status: {2} \r\n Service Status Description: {3} \r\n System Status: {4} \r\n System Status Description: {5} \r\n Exception StackTrace: {1} \r\n ", exceptionMsg, exceptionStackTrace, serviceStatus, ServiceStatusDescription, systemStatus, systemStatusDescription);
                    System.Diagnostics.EventLog.WriteEntry("PNU", error, System.Diagnostics.EventLogEntryType.Error);
                }
                catch (Exception ex)
                {

                }
            });

        }
        public static List<PnuUser> GetUsersInGroup(string groupName)
        {
            List<PnuUser> UsersList = new List<PnuUser>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPUserCollection userColl = web.Groups[groupName].Users;

                        foreach (SPUser user in userColl)
                        {
                            var Pnuuser = new PnuUser()
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
                catch (Exception ex)
                {
                    throw;
                }
            }

            public static List<LookUp> GetChoicesLookup(string ListName, string FieldName)
            {
                try
                {
                    List<LookUp> ChoicesList = new List<LookUp>();
                    using (SPSite osite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb oweb = osite.OpenWeb())
                        {

                            SPList list = oweb.Lists.TryGetList(ListName);
                            SPFieldChoice choices = (SPFieldChoice)list.Fields[FieldName];
                            for (int i = 0; i < choices.Choices.Count; i++)
                            {
                                var lk = new LookUp();
                                lk.Name = choices.Choices[i].ToString();
                                lk.Value = choices.Choices[i].ToString();

                                ChoicesList.Add(lk);
                            }
                        }
                    }

                    return ChoicesList;
                }
                catch (Exception ex)
                {
                    return new List<LookUp>();
                }
            }

            public static List<LookUp> GetMultipleChoicesLookup(string ListName, string FieldName)
            {
                try
                {
                    List<LookUp> ChoicesList = new List<LookUp>();
                    using (SPSite osite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb oweb = osite.OpenWeb())
                        {

                            SPList list = oweb.Lists.TryGetList(ListName);
                            SPFieldMultiChoice choices = (SPFieldMultiChoice)list.Fields[FieldName];

                            for (int i = 0; i < choices.Choices.Count; i++)
                            {
                                var lk = new LookUp();
                                lk.Name = choices.Choices[i].ToString();
                                lk.Value = choices.Choices[i].ToString();

                                ChoicesList.Add(lk);
                            }
                        }
                    }

                    return ChoicesList;
                }
                catch (Exception ex)
                {
                    return new List<LookUp>();
                }


            }
        }
    }
}
