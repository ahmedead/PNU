using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    public partial class ucAgencyTasksDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null &&
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated &&
                    SPContext.Current != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    SharedTitles.EnsureList(SPContext.Current.Web);
                    AgencyTasksProvisioner.EnsureLists(SPContext.Current.Web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyTasksDga.Page_Init", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        private void BindData()
        {
            try
            {
                var titles = SharedTitleReader.Load(IsArabic);
                litTasksHeading.Text = SharedTitleReader.Get(titles, SharedTitles.TasksHeading);

                rptTaskGroups.DataSource = LoadTaskGroups();
                rptTaskGroups.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyTasksDga.BindData", ex.Message);
            }
        }

        protected void rptTaskGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var group = (TaskGroup)e.Item.DataItem;
            var inner = e.Item.FindControl("rptTaskPoints") as Repeater;
            if (inner != null)
            {
                inner.DataSource = group.Points;
                inner.DataBind();
            }
        }

        private List<TaskGroup> LoadTaskGroups()
        {
            var groups = new List<TaskGroup>();
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId  = SPContext.Current.Web.ID;
                bool ar     = IsArabic;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var web  = site.OpenWeb(webId))
                    {
                        var groupList = web.Lists.TryGetList(AgencyTasksProvisioner.GroupsList);
                        var pointList = web.Lists.TryGetList(AgencyTasksProvisioner.PointsList);
                        if (groupList == null) return;

                        var gq = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
                        };

                        int index = 0;
                        foreach (SPListItem g in groupList.GetItems(gq))
                        {
                            index++;
                            var tg = new TaskGroup
                            {
                                Index   = index,
                                IsFirst = index == 1,
                                Title   = SafeString(g, ar ? "TitleAr" : "TitleEn"),
                                Points  = new List<TaskPoint>()
                            };
                            if (string.IsNullOrEmpty(tg.Title))
                                tg.Title = SafeString(g, "TitleAr");

                            int groupId = g.ID;
                            if (pointList != null)
                            {
                                var pq = new SPQuery
                                {
                                    Query = "<Where><Eq><FieldRef Name='GroupId'/>" +
                                            "<Value Type='Number'>" + groupId + "</Value></Eq></Where>" +
                                            "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
                                };
                                foreach (SPListItem p in pointList.GetItems(pq))
                                {
                                    string text = SafeString(p, ar ? "TextAr" : "TextEn");
                                    if (string.IsNullOrEmpty(text)) text = SafeString(p, "TextAr");
                                    if (!string.IsNullOrEmpty(text))
                                        tg.Points.Add(new TaskPoint { Text = text });
                                }
                            }
                            groups.Add(tg);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyTasksDga.LoadTaskGroups", ex.Message);
            }
            return groups;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item[field]?.ToString() ?? string.Empty; }
            catch { return string.Empty; }
        }

        public class TaskGroup
        {
            public int Index { get; set; }
            public bool IsFirst { get; set; }
            public string Title { get; set; }
            public List<TaskPoint> Points { get; set; }
        }

        public class TaskPoint
        {
            public string Text { get; set; }
        }
    }
}
