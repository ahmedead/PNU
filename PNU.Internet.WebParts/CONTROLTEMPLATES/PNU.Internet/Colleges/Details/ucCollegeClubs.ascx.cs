using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details
{
    public partial class ucCollegeClubs : UserControl
    {
        string ListName = "StudentClubs";
        string ImageListName = "ClubsImages";

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

                    List<StudentClubItem> studentClubs = LoadStudentClubs(web);
                    List<StudentClubImageItem> images = LoadStudentClubsImages(web);


                    foreach (StudentClubImageItem image in images)
                    {
                        StudentClubItem student = studentClubs.FirstOrDefault(s => s.ID == image.LookupID);

                        if (student != null)
                            student.Images.Add(image);
                    }


                    rptStudentClubs.DataSource = studentClubs;
                    rptStudentClubs.DataBind();
                }
            });
        }

        protected void rptStudentClubs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            StudentClubItem club = (StudentClubItem)e.Item.DataItem;

            Repeater rptImages = (Repeater)e.Item.FindControl("rptImages");
            rptImages.DataSource = club.Images;
            rptImages.DataBind();

            Repeater rptSections = (Repeater)e.Item.FindControl("rptSections");
            rptSections.DataSource = club.Sections;
            rptSections.DataBind();
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureStudentsClubList(web);
            EnsureStudentClubImagesList(web);
        }

        private void EnsureStudentsClubList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListName,
                    "Stores Student Clubs",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Sec2Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec2Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Location", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }


        private void EnsureStudentClubImagesList(SPWeb web)
        {
            SPList lookup = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(ImageListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ImageListName,
                    "Stores Student club Images",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureLookupField(list, "ClupLookup", lookup, "Title", true);
            ListHelper.EnsureField(list, "ImageUrl", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }



        private List<StudentClubItem> LoadStudentClubs(SPWeb web)
        {
            List<StudentClubItem> result = new List<StudentClubItem>();

            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };


            foreach (SPListItem item in list.GetItems(query))
            {

                result.Add(new StudentClubItem
                {
                    ID = item.ID,
                    Title = item.Title,
                    Location = Convert.ToString(item["Location"]),
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

        private List<StudentClubImageItem> LoadStudentClubsImages(SPWeb web)
        {
            List<StudentClubImageItem> result = new List<StudentClubImageItem>();

            SPList list = web.Lists.TryGetList(ImageListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new StudentClubImageItem
                {
                    ID = item.ID,
                    LookupID = ListHelper.GetLookupFieldValue(item, "ClupLookup"),
                    ImageUrl = Convert.ToString(item["ImageUrl"]),
                    SortOrder = Convert.ToInt32(item["SortOrder"])
                });
            }

            return result;
        }
    }

    [Serializable]
    public class StudentClubItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }

        public List<StudentClubImageItem> Images = new List<StudentClubImageItem>();
        public List<ContentSection> Sections = new List<ContentSection>();
    }

    [Serializable]
    public class StudentClubImageItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }

    }
}