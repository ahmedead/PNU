using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint;
using ImageMagick;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidents;
using System.Collections.Generic;
using System.Linq;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System.Web;
using System.Web.UI.HtmlControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details
{
    public partial class ucCollegeAgancies : UserControl
    {

        string ListName = "Agencies";
        string DeptListName = "AgencyDepartments";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public string CleanRichText(string html)
        {
            return ListHelper.CleanRichText(html);
        }

        private void LoadData()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        EnsureLists(web);

                        var agencies = LoadAgencies(web);
                        var depts = LoadCollageAgencyDepartments(web);

                        foreach (var item in depts)
                        {
                            var temp = agencies.Where(s => s.ID == item.LookupID).FirstOrDefault();
                            if (temp != null)
                                temp.Departments.Add(new AgencyDepartmentItem { Text = item.Text, URL = item.URL });
                        }

                        if (agencies != null && agencies.Count != 0)
                        {
                            rptAgencies.DataSource = agencies;
                            rptAgencies.DataBind();
                        }

                    }
                }
            });
        }

        protected void rptAgencies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            AgencyItem agency = (AgencyItem)e.Item.DataItem;

            Repeater rptSections = (Repeater)e.Item.FindControl("rptSections");
            rptSections.DataSource = agency.Sections;
            rptSections.DataBind();

            Repeater rptDepartments = (Repeater)e.Item.FindControl("rptDepartments");
            rptDepartments.DataSource = agency.Departments;
            rptDepartments.DataBind();

            HtmlGenericControl deptTitle = (HtmlGenericControl)e.Item.FindControl("DeptTitle");
            HtmlGenericControl deptRow = (HtmlGenericControl)e.Item.FindControl("DeptRow");

            bool hasDepartments = rptDepartments.Items.Count > 0;

            deptTitle.Visible = hasDepartments;
            deptRow.Visible = hasDepartments;
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureAgenicesList(web);
            EnsureAgenicyDepartmentList(web);
        }

        private void EnsureAgenicesList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null)
            {
                Guid listId = web.Lists.Add(ListName, "Stores Collage Agencies", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }
            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Sec2Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec2Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }



        private void EnsureAgenicyDepartmentList(SPWeb web)
        {
            SPList lookuplist = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(DeptListName);

            if (list == null)
            {
                Guid listId = web.Lists.Add(DeptListName, "Stores Collage Agency Departments", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }
            ListHelper.EnsureLookupField(list, "AgencyLookup", lookuplist, "Title", true);
            ListHelper.EnsureField(list, "Text", SPFieldType.Text);
            ListHelper.EnsureField(list, "URL", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private List<AgencyItem> LoadAgencies(SPWeb web)
        {
            var result = new List<AgencyItem>();
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AgencyItem());
                result.Last().ID = item.ID;
                result.Last().Title=item.Title;


                foreach (SPField field in item.Fields)
                {
                    if (!field.InternalName.EndsWith("Title", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!field.InternalName.StartsWith("sec", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string contentField = field.InternalName.Replace("Title", "Content");

                    result.Last().Sections.Add(new  ContentSection
                    {
                        Title = Convert.ToString(item[field.InternalName]),
                        Content = item.Fields.ContainsField(contentField)
                            ? Convert.ToString(item[contentField])
                            : string.Empty
                    });
                }
            }
            return result;
        }

        private List<AgencyDepartmentItem> LoadCollageAgencyDepartments(SPWeb web)
        {
            var result = new List<AgencyDepartmentItem>();
            SPList list = web.Lists.TryGetList(DeptListName);
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AgencyDepartmentItem
                {
                    ID = item.ID,
                    LookupID = ListHelper.GetLookupFieldValue(item, "AgencyLookup"),
                    Text = Convert.ToString(item["Text"]),
                    URL = Convert.ToString(item["URL"]),
                });
            }

            return result;
        }

    }

    [Serializable]
    public class AgencyItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<ContentSection> Sections = new List<ContentSection>();
        public List<AgencyDepartmentItem> Departments = new List<AgencyDepartmentItem>();
    }



    [Serializable]
    public class AgencyDepartmentItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public int SortOrder { get; set; }

    }
}