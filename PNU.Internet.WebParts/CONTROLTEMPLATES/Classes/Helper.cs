using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts
{
    public class LookUp
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public static class Helper
    {
        public static SPListItemCollection LoadListDynamicByCML(string SiteURL, string ListName, ArrayList qryParams,string OrderByFieldName="ID", string AscendingTrue="False")
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

                qry.Query = "<OrderBy><FieldRef Name='"+ OrderByFieldName + "' " +
                  "Ascending='"+ AscendingTrue + "' /></OrderBy>" + whereQry;

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
        public static SPListItemCollection LoadListDynamicByCML(Guid SiteID, string WebUrl, string ListName, ArrayList qryParams,int RowLimit, string OrderByFieldName = "ID", string AscendingTrue = "False")
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

                qry.Query = "<OrderBy><FieldRef Name='" + OrderByFieldName + "' " +
                  "Ascending='" + AscendingTrue + "' /></OrderBy>" + whereQry;
                if(RowLimit > 0) 
                {
                    qry.RowLimit = Convert.ToUInt32(RowLimit);
                }

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SiteID))
                    {
                        using (SPWeb web = site.OpenWeb(WebUrl))
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

        public static SPListItemCollection LoadListDynamicByCML(string SiteURL,string WebURL, string ListName, ArrayList qryParams, string OrderByFieldName = "ID", string AscendingTrue = "False")
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

                qry.Query = "<OrderBy><FieldRef Name='" + OrderByFieldName + "' " +
                  "Ascending='" + AscendingTrue + "' /></OrderBy>" + whereQry;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SiteURL))
                    {
                        using (SPWeb web = site.OpenWeb(WebURL))
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

        public static SPListItemCollection LoadListDynamicByCML(string SiteURL, string WebURL, string ListName, string OrderByFieldName = "ID", string AscendingTrue = "False")
        {
            SPListItemCollection ItemCol = null;

            try
            {
                string whereQry = "<Where>{0}</Where>";
                SPQuery qry = new SPQuery();

                qry.Query = "<OrderBy><FieldRef Name='" + OrderByFieldName + "' " +
                  "Ascending='" + AscendingTrue + "' /></OrderBy>" ;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SiteURL))
                    {
                        using (SPWeb web = site.OpenWeb(WebURL))
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

        public static string GetCustomFormsGlobalResourceValue(string ResourceFile, string key)
        {
            string retValue = string.Empty;
            try
            {
                retValue = Convert.ToString(HttpContext.GetGlobalResourceObject(ResourceFile, key));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retValue;
        }


        public static List<LookUp> GetChoicesLookup(string ListName, string FieldName ,Guid site,string web)
        {
            try
            {
                List<LookUp> ChoicesList = new List<LookUp>();
                using (SPSite osite = new SPSite(site))
                {
                    using (SPWeb oweb = osite.OpenWeb(web))
                    {
                        SPList list = oweb.Lists.TryGetList(ListName);
                        SPFieldChoice choices = (SPFieldChoice)list.Fields[FieldName];
                        for (int i = 0; i < choices.Choices.Count; i++)
                        {
                            var lk = new LookUp();
                            lk.Name = choices.Choices[i].ToString();
                            lk.Value = choices.Choices[i].ToString(); ChoicesList.Add(lk);
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
        public static List<LookUp> GetMultipleChoicesLookup(string ListName, string FieldName, Guid site, string web)
        {
            try
            {
                List<LookUp> ChoicesList = new List<LookUp>();
                using (SPSite osite = new SPSite(site))
                {
                    using (SPWeb oweb = osite.OpenWeb(web))
                    {
                        SPList list = oweb.Lists.TryGetList(ListName);
                        SPFieldMultiChoice choices = (SPFieldMultiChoice)list.Fields[FieldName];
                        for (int i = 0; i < choices.Choices.Count; i++)
                        {
                            var lk = new LookUp();
                            lk.Name = choices.Choices[i].ToString();
                            lk.Value = choices.Choices[i].ToString(); ChoicesList.Add(lk);
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
        public static string GetUserEmail()
        {
            return SPContext.Current.Web.CurrentUser.Email.ToLower().Trim();

        }

        public static NewsUserEditor IsAllowedUser(string SiteURL= "/ar/MediaCenter/News/")
        {
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='UserAccount' />
                                 <Value Type='User'>" + user.Name + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb( SiteURL ))
                        {
                            SPList reqList = web.Lists["NewsUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return new NewsUserEditor();
                if (objNew.Count == 0)
                    return new NewsUserEditor();

                return new NewsUserEditor { Id = Convert.ToInt32(objNew[0]["ID"])
                    , IsAdmin = Convert.ToBoolean(objNew[0]["IsAdmin"]) 
                    , CanDelete = Convert.ToBoolean(objNew[0]["CanDelete"])
                    , CanEdit = Convert.ToBoolean(objNew[0]["CanEdit"]) };



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                
            }


            return null;



        }

        public static NewsUserEditor IsAdsAllowedUser(string SiteURL = "/ar/MediaCenter/News/")
        {
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='UserAccount' />
                                 <Value Type='User'>" + user.Name + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists["AdsUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return new NewsUserEditor();
                if (objNew.Count == 0)
                    return new NewsUserEditor();

                return new NewsUserEditor
                {
                    Id = Convert.ToInt32(objNew[0]["ID"])
                    ,
                    IsAdmin = Convert.ToBoolean(objNew[0]["IsAdmin"])
                    ,
                    CanDelete = Convert.ToBoolean(objNew[0]["CanDelete"])
                    ,
                    CanEdit = Convert.ToBoolean(objNew[0]["CanEdit"])
                };



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);

            }


            return null;



        }

        public static ContentEditorUsers IsContentEditorUser()
        {
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='DeptUsers' />
                                 <Value Type='User'>" + user.Name + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                        {
                            SPList reqList = web.Lists["ContentAdminUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return new ContentEditorUsers();
                if (objNew.Count == 0)
                    return new ContentEditorUsers();

                return new ContentEditorUsers { Id=Convert.ToInt32(objNew[0]["ID"]), UserName = user.Name, UserEmail = user.Email, DeptTitle = Convert.ToString(objNew[0]["Title"]) };

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "get-adminuser-for-faculty - IsfacultyUser", ex.Message);
            }


            return null;

        }
        public static bool IsAdminUser(string SiteURL = "/ar/MediaCenter/News/")
        {
            bool retVal = false;
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                                    <And>
                                 <Eq>
                                 <FieldRef Name='IsAdmin'  />
                                 <Value Type='Boolean'>1</Value>
                              </Eq>
                              <Eq>
                                 <FieldRef Name='UserAccount' />
                                 <Value Type='User'>" + user.Name + @"</Value>
                              </Eq>
                               </And>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists["NewsUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return false;
                if (objNew.Count == 0)
                    return false;

                retVal = objNew.Count > 0;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                return false;
            }

            return retVal;


        }
        public static List<NewsUserEditor> GetAdminUsers(string SiteURL = "/ar/MediaCenter/News/")
        {
            try
            {
                List<NewsUserEditor> LstUsers = new List<NewsUserEditor>();
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='IsAdmin'  />
                                 <Value Type='Boolean'>1</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists["NewsUsers"];
                            objNew = reqList.GetItems(query);
                            foreach (SPListItem item in objNew)
                            {
                                SPFieldUserValue userField = new SPFieldUserValue(web, item["UserAccount"].ToString());
                                var user = new NewsUserEditor()
                                {
                                    Id = Convert.ToInt32(item["ID"]),
                                    UserName = userField.User.LoginName,
                                    UserEmail = userField.User.Email
                                };

                                LstUsers.Add(user);

                            }

                        }
                    }
                });
                if (objNew == null)
                    return new List<NewsUserEditor>();
                if (objNew.Count == 0)
                    return new List<NewsUserEditor>();

                return LstUsers;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
            }


            return null;



        }
        public static List<NewsUserEditor> GetAdsAdminUsers(string SiteURL = "/ar/MediaCenter/News/")
        {
            try
            {
                List<NewsUserEditor> LstUsers = new List<NewsUserEditor>();
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='IsAdmin'  />
                                 <Value Type='Boolean'>1</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists["AdsUsers"];
                            objNew = reqList.GetItems(query);
                            foreach (SPListItem item in objNew)
                            {
                                SPFieldUserValue userField = new SPFieldUserValue(web, item["UserAccount"].ToString());
                                var user = new NewsUserEditor()
                                {
                                    Id = Convert.ToInt32(item["ID"]),
                                    UserName = userField.User.LoginName,
                                    UserEmail = userField.User.Email
                                };

                                LstUsers.Add(user);

                            }

                        }
                    }
                });
                if (objNew == null)
                    return new List<NewsUserEditor>();
                if (objNew.Count == 0)
                    return new List<NewsUserEditor>();

                return LstUsers;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
            }


            return null;



        }


    }

}
