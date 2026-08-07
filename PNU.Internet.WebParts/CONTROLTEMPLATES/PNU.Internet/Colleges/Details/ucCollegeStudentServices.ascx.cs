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
    public partial class ucCollegeStudentServices : UserControl
    {
        string ListName = "StudentSerives";

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

                    List<StudentServiceItem> studentServices = LoadStudentServices(web);

                    rptStudentServices.DataSource = studentServices;
                    rptStudentServices.DataBind();
                }
            });
        }

        protected void rptStudentServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            StudentServiceItem service = (StudentServiceItem)e.Item.DataItem;

            Repeater rptSections = (Repeater)e.Item.FindControl("rptSections");
            rptSections.DataSource = service.Sections;
            rptSections.DataBind();

            HyperLink lnkService = (HyperLink)e.Item.FindControl("lnkService");
            lnkService.Visible = !string.IsNullOrWhiteSpace(service.ServiceLink);
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
        }

        private void EnsureStudentsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid id = web.Lists.Add(
                    ListName,
                    "Stores Student services",
                    SPListTemplateType.GenericList);

                list = web.Lists[id];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec1Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Sec2Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Sec2Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "ServiceText", SPFieldType.Text);
            ListHelper.EnsureField(list, "ServiceLink", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }



        private List<StudentServiceItem> LoadStudentServices(SPWeb web)
        {
            List<StudentServiceItem> result = new List<StudentServiceItem>();

            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };


            foreach (SPListItem item in list.GetItems(query))
            {

                result.Add(new StudentServiceItem
                {
                    ID = item.ID,
                    Title = item.Title,
                    ServiceLink = Convert.ToString(item["ServiceLink"]),
                    ServiceText= Convert.ToString(item["ServiceText"])
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





    }

    [Serializable]
    public class StudentServiceItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ServiceLink { get; set; }
        public string ServiceText { get; set; }

        public List<ContentSection> Sections = new List<ContentSection>();
    }
}