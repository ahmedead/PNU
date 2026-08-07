using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public class RequestsList
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string Summary { get; set; }
        public string Summary_EN { get; set; }
        public string MediaContent { get; set; }
        public string MediaContent_EN { get; set; }
        public string FacultyName { get; set; }
        public string FacultyName_EN { get; set; }
        public string MainCategory { get; set; }
        public string MainCategory_EN { get; set; }
        public string MediaTypes { get; set; }
        public string MediaTypes_EN { get; set; }
        public string MediaDate { get; set; }
        public string UserComments { get; set; }
        public string RequestStatus { get; set; }
        public string ApprovalGroupName { get; set; }
        public string RequesterName { get; set; }
        public string RequesterEmail { get; set; }
        public string IsHome { get; set; }
        public string VideoURL { get; set; }
        public string NextRequestStatus { get; set; }



    }

    public static class busclsNewsRequestsList
    {

    }



    public class NewsUsers
    {
        public string Title { get; set; }
        public bool IsAdmin { get; set; }
        public string UserAccount { get; set; }
        public bool CanDelete { get; set; }
        public string WorkFlowSteps { get; set; }
        public string ID { get; set; }


    }

    public static class busclsNewsUsers
    {
        public static NewsUsers GetCurrentUser()
        {
            NewsUsers newsUsers = new NewsUsers();
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
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList reqList = web.Lists["NewsUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;

                newsUsers = SPFactory.MapListItemsToClass<NewsUsers>(objNew[0]);
                return newsUsers;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                return null;
            }
        }

    }

    public class NewsWorkflowSteps
    {
        public string Title { get; set; }
        public string WorkflowStepName { get; set; }
        public string ItemOrder { get; set; }
        public string ID { get; set; }



    }

    public static class busclsNewsWorkflowSteps
    {
        public static List<NewsWorkflowSteps> GetAllSteps()
        {
            List<NewsWorkflowSteps> steps = new List<NewsWorkflowSteps>();
            try
            {
                SPQuery query = new SPQuery();
                query.Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>";

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList reqList = web.Lists["NewsWorkflowSteps"];
                            objNew = reqList.GetItems(query);
                        }
                    }
                });

                if (objNew == null || objNew.Count == 0)
                    return steps;

                foreach (SPListItem item in objNew)
                {
                    steps.Add(SPFactory.MapListItemsToClass<NewsWorkflowSteps>(item));
                }

                return steps;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsNewsWorkflowSteps - GetAllSteps", ex.Message);
                return steps;
            }
        }
        public static NewsWorkflowSteps GetCurrentStep(string CurrentStepName)
        {
            NewsWorkflowSteps CurrentStep = new NewsWorkflowSteps();
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 $@"<Where>
                                      <Eq>
                                         <FieldRef Name='WorkflowStepName' />
                                         <Value Type='Text'>{CurrentStepName}</Value>
                                      </Eq>
                                   </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList reqList = web.Lists["NewsWorkflowSteps"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;

                CurrentStep = SPFactory.MapListItemsToClass<NewsWorkflowSteps>(objNew[0]);
                return CurrentStep;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                return null;
            }
        }


        public static NewsWorkflowSteps GetNextStep(string CurrentStepName)
        {
            
            try
            {
                NewsWorkflowSteps CurrentStep= GetCurrentStep(CurrentStepName);
                NewsWorkflowSteps NextStep = new NewsWorkflowSteps();

                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 $@"<Where>
                                          <Eq>
                                             <FieldRef Name='ItemOrder' />
                                             <Value Type='Number'>{Convert.ToInt32(CurrentStep.ItemOrder) + 1}</Value>
                                          </Eq>
                                       </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList reqList = web.Lists["NewsWorkflowSteps"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;

                NextStep = SPFactory.MapListItemsToClass<NewsWorkflowSteps>(objNew[0]);
                return NextStep;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                return null;
            }
        }

        public static NewsWorkflowSteps GetFirstStep()
        {

            try
            {
                
                NewsWorkflowSteps NextStep = new NewsWorkflowSteps();

                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 $@"<Where>
                                          <Eq>
                                             <FieldRef Name='ItemOrder' />
                                             <Value Type='Number'>{1}</Value>
                                          </Eq>
                                       </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList reqList = web.Lists["NewsWorkflowSteps"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;

                NextStep = SPFactory.MapListItemsToClass<NewsWorkflowSteps>(objNew[0]);
                return NextStep;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
                return null;
            }
        }

    }

}
