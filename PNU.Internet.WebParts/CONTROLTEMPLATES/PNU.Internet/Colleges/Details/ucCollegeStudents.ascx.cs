using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Web.UI.HtmlControls;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details
{
    public partial class ucCollegeStudents : UserControl
    {
        string ListName = "CollegeStudents";
        string ImageListName = "CollegeStudentsImages";
        string CooListName = "CollegeStudentsCoordinators";
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
                using (SPWeb web = site.OpenWeb())
                {
                    EnsureLists(web);

                    List<StudentItem> students = LoadStudents(web);
                    List<StudentImageItem> images = LoadStudentImages(web);
                    List<StudentCoordinatorItem> coordinators = LoadStudentCoordinators(web);

                    foreach (StudentImageItem image in images)
                    {
                        StudentItem student = students.FirstOrDefault(s => s.ID == image.LookupID);

                        if (student != null)
                            student.Images.Add(image);
                    }

                    foreach (StudentCoordinatorItem coordinator in coordinators)
                    {
                        StudentItem student = students.FirstOrDefault(s => s.ID == coordinator.LookupID);

                        if (student != null)
                            student.Coordinators.Add(coordinator);
                    }

                    rptStudents.DataSource = students;
                    rptStudents.DataBind();
                }
            });
        }

        protected void rptStudents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            StudentItem student = (StudentItem)e.Item.DataItem;

            Repeater rptImages = (Repeater)e.Item.FindControl("rptImages");
            rptImages.DataSource = student.Images;
            rptImages.DataBind();

            Repeater rptSections = (Repeater)e.Item.FindControl("rptSections");
            rptSections.DataSource = student.Sections;
            rptSections.DataBind();

            Repeater rptCoordinators = (Repeater)e.Item.FindControl("rptCoordinators");
            rptCoordinators.DataSource = student.Coordinators;
            rptCoordinators.DataBind();

            HyperLink lnkService = (HyperLink)e.Item.FindControl("lnkService");
            lnkService.Visible = !string.IsNullOrWhiteSpace(student.ServiceLink);

            ((HtmlGenericControl)e.Item.FindControl("CoordinatorsDiv")).Visible =
                rptCoordinators.Items.Count > 0;
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureStudentsList(web);
            EnsureStudentImagesList(web);
            EnsureStudentCoordinatorList(web);
        }

        private void EnsureStudentsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListName,
                    "Stores Students",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Sec2Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec2Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Location", SPFieldType.Text);
            ListHelper.EnsureField(list, "ServiceText", SPFieldType.Text);
            ListHelper.EnsureField(list, "ServiceLink", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }


        private void EnsureStudentImagesList(SPWeb web)
        {
            SPList lookup = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(ImageListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ImageListName,
                    "Stores Student Images",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureLookupField(list, "StudentLookup", lookup, "Title", true);
            ListHelper.EnsureField(list, "ImageUrl", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }

        private void EnsureStudentCoordinatorList(SPWeb web)
        {
            SPList lookup = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(CooListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    CooListName,
                    "Stores Student Coordinators",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureLookupField(list, "StudentLookup", lookup, "Title", true);
            ListHelper.EnsureField(list, "ProgramName", SPFieldType.Text);
            ListHelper.EnsureField(list, "CoordinatorName", SPFieldType.Text);
            ListHelper.EnsureField(list, "Email", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }

        private List<StudentItem> LoadStudents(SPWeb web)
        {
            List<StudentItem> result = new List<StudentItem>();

            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };


            foreach (SPListItem item in list.GetItems(query))
            {

                result.Add(new StudentItem
                {
                    ID = item.ID,
                    Title = item.Title,
                    Location = Convert.ToString(item["Location"]),
                    ServiceLink = Convert.ToString(item["ServiceLink"]),
                    ServiceText = Convert.ToString(item["ServiceText"])
                });


                foreach (SPField field in item.Fields)
                {
                    if (!field.InternalName.EndsWith("Title", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!field.InternalName.StartsWith("sec", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string contentField = field.InternalName.Replace("Title", "Content");

                    result.Last().Sections.Add(new ContentSection
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

        private List<StudentImageItem> LoadStudentImages(SPWeb web)
        {
            List<StudentImageItem> result = new List<StudentImageItem>();

            SPList list = web.Lists.TryGetList(ImageListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new StudentImageItem
                {
                    ID = item.ID,
                    LookupID = ListHelper.GetLookupFieldValue(item, "StudentLookup"),
                    ImageUrl = Convert.ToString(item["ImageUrl"]),
                    SortOrder = Convert.ToInt32(item["SortOrder"])
                });
            }

            return result;
        }

        private List<StudentCoordinatorItem> LoadStudentCoordinators(SPWeb web)
        {
            List<StudentCoordinatorItem> result = new List<StudentCoordinatorItem>();

            SPList list = web.Lists.TryGetList(CooListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new StudentCoordinatorItem
                {
                    ID = item.ID,
                    LookupID = ListHelper.GetLookupFieldValue(item, "StudentLookup"),
                    CoordinatorName = Convert.ToString(item["CoordinatorName"]),
                    Email = Convert.ToString(item["Email"]),
                    ProgramName = Convert.ToString(item["ProgramName"]),
                    SortOrder = Convert.ToInt32(item["SortOrder"])
                });
            }

            return result;
        }





    }

    [Serializable]
    public class StudentItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string ServiceLink { get; set; }

        public string ServiceText { get; set; }

        public List<StudentImageItem> Images = new List<StudentImageItem>();
        public List<ContentSection> Sections = new List<ContentSection>();
        public List<StudentCoordinatorItem> Coordinators = new List<StudentCoordinatorItem>();
    }

    [Serializable]
    public class StudentImageItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }

    }

    [Serializable]
    public class StudentCoordinatorItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string ProgramName { get; set; }
        public string CoordinatorName { get; set; }
        public string Email { get; set; }
        public int SortOrder { get; set; }

    }
}