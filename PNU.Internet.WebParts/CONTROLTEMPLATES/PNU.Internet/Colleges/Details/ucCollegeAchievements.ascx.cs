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
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details
{
    public partial class ucCollegeAchievements : UserControl
    {
        string ListName = "Achievements";
        string ImageListName = "AchievementImages";

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

                        var achievements = LoadAchievements(web);
                        var achievementImages = LoadAchievementImages(web);

                        foreach (var item in achievementImages)
                        {
                            var temp = achievements.Where(s => s.ID == item.LookupID).FirstOrDefault();
                            if (temp != null)
                                temp.Images.Add(item.ImageUrl);
                        }

                        if (achievements != null && achievements.Count != 0)
                        {
                            rptAchievements.DataSource = achievements;
                            rptAchievements.DataBind();
                        }
                    }
                }
            });
        }


        protected void rptAchievements_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            AchievementItem item = (AchievementItem)e.Item.DataItem;

            Repeater rptImages = (Repeater)e.Item.FindControl("rptImages");

            rptImages.DataSource = item.Images;
            rptImages.DataBind();
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            EnsureAllLists(web);
            web.AllowUnsafeUpdates = false;
        }


        private void EnsureAllLists(SPWeb web)
        {
            EnsureAchievementsList(web);
            EnsureAchievementsImageList(web); 
        }



        private void EnsureAchievementsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null)
            {
                Guid listId = web.Lists.Add(ListName, "Stores Collage Achievements", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Content", SPFieldType.Note);
            ListHelper.EnsureField(list, "Date", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }


        private void EnsureAchievementsImageList(SPWeb web)
        {
            SPList lookuplist = web.Lists.TryGetList(ListName);

            SPList list = web.Lists.TryGetList(ImageListName);

            if (list == null)
            {
                Guid listId = web.Lists.Add(ImageListName, "Stores Collage Achievements Images", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }
            ListHelper.EnsureLookupField(list, "AchievementLookup", lookuplist, "Title", true);
            ListHelper.EnsureField(list, "ImageUrl", SPFieldType.Text);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private List<AchievementItem> LoadAchievements(SPWeb web)
        {
            var result = new List<AchievementItem>();
            SPList list = web.Lists.TryGetList(ListName);
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AchievementItem
                {
                    ID = item.ID,
                    Title = Convert.ToString(item["Title"]),
                    Content = Convert.ToString(item["Content"]),
                    Date = Convert.ToString(item["Date"]),
                    SortOrder = ListHelper.ParseInt(Convert.ToString(item["SortOrder"])),
                    Images = new List<string>()
                });
            }

            return result;
        }

        private List<AchievementImageItem> LoadAchievementImages(SPWeb web)
        {
            var result = new List<AchievementImageItem>();
            SPList list = web.Lists.TryGetList(ImageListName);
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AchievementImageItem
                {
                    ID = item.ID,
                    LookupID = ListHelper.GetLookupFieldValue(item, "AchievementLookup"),
                    ImageUrl = Convert.ToString(item["ImageUrl"]),
                });
            }

            return result;
        }

    }

    [Serializable]
    public class AchievementItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public int SortOrder { get; set; }
        public List<string> Images { get; set; }
    }

    [Serializable]
    public class AchievementImageItem
    {
        public int ID { get; set; }
        public int LookupID { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }

    }
}
