using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.GeneralDest
{
    public partial class ucDestSections : UserControl
    {

        public string ListName { get; set; } = string.Empty;
        public string ColumnsCount { get; set; } = string.Empty;

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

                    List<DestSectionItem> items = LoadItems(web);
                    
                    rptItems.DataSource = items;
                    rptItems.DataBind();
                }
            });
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DestSectionItem club = (DestSectionItem)e.Item.DataItem;

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
            EnsureSectionList(web);
        }

        private void EnsureSectionList(SPWeb web)
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


            for (int i = 1; i <= int.Parse(ColumnsCount); i++)
            {
                ListHelper.EnsureField(list, "Sec"+i.ToString()+"Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Sec" + i.ToString() + "Content", SPFieldType.Note);
            }

            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }






        private List<DestSectionItem> LoadItems(SPWeb web)
        {
            List<DestSectionItem> result = new List<DestSectionItem>();

            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
            };


            foreach (SPListItem item in list.GetItems(query))
            {

                result.Add(new DestSectionItem
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


    }

    [Serializable]
    public class DestSectionItem
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<ContentSection> Sections = new List<ContentSection>();
    }

}