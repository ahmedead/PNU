using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucAchievements : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string IsHome { get; set; } = "False";

        private const string ListName = "AchievementsAndAwards";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(IsHome == "False")
                    divViewMore.Visible = false;
                EnsureAchievementsList();
                BindTabData();
            }
        }

        private void EnsureAchievementsList()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb("ar"))
                {
                    web.AllowUnsafeUpdates = true;
                    SPList list = web.Lists.TryGetList(ListName);

                    if (list == null)
                    {
                        // Create the List
                        Guid listId = web.Lists.Add(ListName, "List for Achievements, Awards, and Rankings", SPListTemplateType.GenericList);
                        list = web.Lists[listId];

                        // Add Fields
                        list.Fields.Add("Title_EN", SPFieldType.Text, false);
                        list.Fields.Add("Description", SPFieldType.Note, false);
                        list.Fields.Add("Description_EN", SPFieldType.Note, false);
                        list.Fields.Add("DisplayDate", SPFieldType.Text, false);
                        list.Fields.Add("ItemImage", SPFieldType.URL, false);
                        list.Fields.Add("ItemOrder", SPFieldType.Number, false);


                        // 3. Correct way to add a Choice Field
                        string categoryInternalName = list.Fields.Add("Category", SPFieldType.Choice, false);
                        SPFieldChoice categoryField = (SPFieldChoice)list.Fields.GetFieldByInternalName(categoryInternalName);

                        // Set the choices
                        categoryField.Choices.Add("Achievements");
                        categoryField.Choices.Add("Awards");
                        categoryField.Choices.Add("Rankings");

                        categoryField.DefaultValue = "Achievements";
                        categoryField.Update();

                        //// Add Category Choice Field
                        //System.Collections.Specialized.StringCollection choices = new System.Collections.Specialized.StringCollection();
                        //choices.Add("Achievements");
                        //choices.Add("Awards");
                        //choices.Add("Rankings");
                        //list.Fields.AddMenuChoices("Category", choices);

                        list.Update();

                        // Add the fields to the default view
                        SPView view = list.DefaultView;
                        view.ViewFields.Add("Title_EN");
                        view.ViewFields.Add("Description");
                        view.ViewFields.Add("Description_EN");
                        view.ViewFields.Add("DisplayDate");
                        view.ViewFields.Add("DisplayDate_EN");
                        view.ViewFields.Add("Category");
                        view.ViewFields.Add("ItemOrder");

                        view.Update();
                    }
                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        private void BindTabData()
        {
            List<NewsEventItem> allItems = new List<NewsEventItem>();
            
            SPSite site = new SPSite(SPContext.Current.Site.ID);
            SPWeb web = site.OpenWeb("ar");
                
            SPList list = web.Lists.TryGetList(ListName);

            if (list != null)
            {
                SPQuery query = new SPQuery();
                query.Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='False' /></OrderBy>";
                SPListItemCollection spItems = list.GetItems(query);

                foreach (SPListItem item in spItems)
                {
                    string imgUrl = item["ItemImage"] != null ? new SPFieldUrlValue(item["ItemImage"].ToString()).Url : "";

                    allItems.Add(new NewsEventItem
                    {
                        Title = Convert.ToString(item["Title"]),
                        Title_EN = Convert.ToString(item["Title_EN"]),
                        Description = Convert.ToString(item["Description"]),
                        Description_EN = Convert.ToString(item["Description_EN"]),
                        DisplayDate = Convert.ToString(item["DisplayDate"]),
                        DisplayDate_EN = Convert.ToString(item["DisplayDate_EN"]),
                        Category = Convert.ToString(item["Category"]),
                        ImageUrl = imgUrl
                    });
                }

                // Bind to Repeaters using LINQ to filter by Category
                var Achievements = allItems.Where(x => x.Category == "Achievements").ToList();
                var Awards = allItems.Where(x => x.Category == "Awards").ToList();
                var Rankings = allItems.Where(x => x.Category == "Rankings").ToList();

                if (IsHome == "TRUE")
                    rptAchievements.DataSource = Achievements.Take(3).ToList();
                else
                    rptAchievements.DataSource = Achievements;
                rptAchievements.DataBind();
                pnlNoAchievements.Visible = !Achievements.Any();


                if (IsHome == "TRUE")
                    rptAwards.DataSource = Awards.Take(3).ToList();
                else
                    rptAwards.DataSource = Awards;
                rptAwards.DataBind();
                phNoAwards.Visible = !Awards.Any();

                if (IsHome == "TRUE")
                    rptRankings.DataSource = Rankings.Take(3).ToList();
                else
                    rptRankings.DataSource = Rankings;
                rptRankings.DataBind();
                pnlNoRankings.Visible = !Rankings.Any();
            }
        }
    }


    public class NewsEventItem
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string Description { get; set; }
        public string Description_EN { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayDate { get; set; }
        public string Category { get; set; } // Achievements, Awards, Rankings
        public string DisplayDate_EN { get; set; }
    }
}
