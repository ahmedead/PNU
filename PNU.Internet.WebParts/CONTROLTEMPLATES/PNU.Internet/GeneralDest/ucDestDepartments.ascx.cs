using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.GeneralDest
{
    public partial class ucDestDepartments : UserControl
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
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        EnsureLists(web);

                        var departments = LoadDepartments(web);

                        if (departments.Count > 0)
                        {
                            rptDepts.DataSource = departments;
                            rptDepts.DataBind();
                        }
                    }
                }
            });
        }

        protected void rptDepts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            EnsureAllLists(web);

            web.AllowUnsafeUpdates = false;
        }

        private void EnsureAllLists(SPWeb web)
        {
            EnsureDepartmentList(web);
        }

        private void EnsureDepartmentList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid listId = web.Lists.Add(
                    ListName,
                    "Stores Departments",
                    SPListTemplateType.GenericList);

                list = web.Lists[listId];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "URL", SPFieldType.Text);
            ListHelper.EnsureField(list, "Visible", SPFieldType.Boolean);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }

        private List<DestDepartmentItem> LoadDepartments(SPWeb web)
        {
            var result = new List<DestDepartmentItem>();

            SPList list = web.Lists.TryGetList(ListName);
            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query =
                    @"<Where>
                        <Eq>
                            <FieldRef Name='Visible' />
                            <Value Type='Boolean'>1</Value>
                        </Eq>
                      </Where>
                      <OrderBy>
                        <FieldRef Name='SortOrder' Ascending='TRUE' />
                      </OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new DestDepartmentItem
                {
                    ID = item.ID,
                    Title = Convert.ToString(item["Title"]),
                    URL = Convert.ToString(item["URL"]),
                    SortOrder = item["SortOrder"] != null
                        ? Convert.ToInt32(item["SortOrder"])
                        : 0
                });
            }

            return result;
        }
    }

    [Serializable]
    public class DestDepartmentItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public int SortOrder { get; set; }
    }
}