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

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details
{
    public partial class ucCollegeFacilities : UserControl
    {
        string ListName = "Facilities";
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

                        var facilities = LoadFacilities(web);

                        if (facilities != null && facilities.Count != 0)
                        {
                            rptFacilities.DataSource = facilities;
                            rptFacilities.DataBind();
                        }

                    }
                }
            });
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureFacilitiesList(web);
        }

        private void EnsureFacilitiesList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null)
            {
                Guid listId = web.Lists.Add(ListName, "Stores Collage Facilities", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }
            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private List<FacilityItem> LoadFacilities(SPWeb web)
        {
            var result = new List<FacilityItem>();
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new FacilityItem());
                result.Last().ID = item.ID;
                result.Last().Title = item.Title;
                result.Last().Content = Convert.ToString(item["Content"]);
            }
            return result;
        }

    }

    [Serializable]
    public class FacilityItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}