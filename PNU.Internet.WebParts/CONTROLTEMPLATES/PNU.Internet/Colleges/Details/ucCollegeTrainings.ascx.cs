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
    public partial class ucCollegeTrainings : UserControl
    {
        string ListName = "StudentTrainings";
        string TraListName = "TrainingCoordinators";

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

                    List<StudentTrainingItem> trainings = LoadStudentTrainings (web);

                    List<StudentTrainingCoordinatorItem> coordinators = LoadStudentTrainingCoordinators(web);

                    foreach (StudentTrainingCoordinatorItem coordinator in coordinators)
                    {
                        StudentTrainingItem student = trainings.FirstOrDefault(s => s.ID == coordinator.LookupID);

                        if (student != null)
                            student.Coordinators.Add(coordinator);
                    }

                    rptStudentTraining.DataSource = trainings;
                    rptStudentTraining.DataBind();
                }
            });
        }

        protected void rptStudentTraining_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            StudentTrainingItem training = (StudentTrainingItem)e.Item.DataItem;

            Repeater rptSections = (Repeater)e.Item.FindControl("rptSections");
            rptSections.DataSource = training.Sections;
            rptSections.DataBind();

            Repeater rptCoordinators = (Repeater)e.Item.FindControl("rptCoordinators");
            rptCoordinators.DataSource = training.Coordinators;
            rptCoordinators.DataBind();

            HtmlGenericControl coordinatorsDiv =
                (HtmlGenericControl)e.Item.FindControl("CoordinatorsDiv");

            coordinatorsDiv.Visible = rptCoordinators.Items.Count > 0;
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureStudentTrainingList(web);
            EnsureStudentTrainingsCoordinatorList(web);
        }

        private void EnsureStudentTrainingList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListName,
                    "Stores Student trainings",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Sec2Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec2Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }



        private void EnsureStudentTrainingsCoordinatorList(SPWeb web)
        {
            SPList lookup = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(TraListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    TraListName,
                    "Stores Student Coordinators",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureLookupField(list, "TrainingLookup", lookup, "Title", true);
            ListHelper.EnsureField(list, "ProgramName", SPFieldType.Text);
            ListHelper.EnsureField(list, "CoordinatorName", SPFieldType.Text);
            ListHelper.EnsureField(list, "Email", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }

        private List<StudentTrainingItem> LoadStudentTrainings(SPWeb web)
        {
            List<StudentTrainingItem> result = new List<StudentTrainingItem>();

            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };


            foreach (SPListItem item in list.GetItems(query))
            {

                result.Add(new StudentTrainingItem
                {
                    ID = item.ID,
                    Title = item.Title,
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



        private List<StudentTrainingCoordinatorItem> LoadStudentTrainingCoordinators(SPWeb web)
        {
            List<StudentTrainingCoordinatorItem> result = new List<StudentTrainingCoordinatorItem>();

            SPList list = web.Lists.TryGetList(TraListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new StudentTrainingCoordinatorItem
                {
                    ID = item.ID,
                    LookupID =ListHelper.GetLookupFieldValue(item, "TrainingLookup"),
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
    public class StudentTrainingItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public List<ContentSection> Sections = new List<ContentSection>();
        public List<StudentTrainingCoordinatorItem> Coordinators = new List<StudentTrainingCoordinatorItem>();
    }



    [Serializable]
    public class StudentTrainingCoordinatorItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string ProgramName { get; set; }
        public string CoordinatorName { get; set; }
        public string Email { get; set; }
        public int SortOrder { get; set; }

    }
}