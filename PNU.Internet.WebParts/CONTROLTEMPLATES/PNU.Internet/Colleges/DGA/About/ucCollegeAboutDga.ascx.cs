using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.About
{
    public partial class ucCollegeAboutDga : UserControl
    {
        protected bool IsArabic;

        private class CardDto
        {
            public string DisplayTitle { get; set; }
            public string DisplayText { get; set; }
            public string IconClass { get; set; }
        }

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                SPContext.Current != null &&
                SPContext.Current.Web.CurrentUser != null)
            {
                CollegeAboutProvisioner.EnsureList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = SPContext.Current.Web.Language == 1025;
            secAbout.Attributes["aria-label"] = IsArabic ? "عن الكلية" : "About the college";

            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite eSite = new SPSite(siteId))
                    using (SPWeb web = eSite.OpenWeb(webId))
                    {
                        SPList list = web.Lists.TryGetList(CollegeAboutProvisioner.ListName);
                        if (list == null) return;

                        var items = new List<CollegeAboutItem>();
                        foreach (SPListItem it in list.Items)
                        {
                            items.Add(new CollegeAboutItem
                            {
                                ID = it.ID,
                                Title = SafeString(it, "Title"),
                                TitleEn = SafeString(it, "TitleEn"),
                                Description = SafeString(it, "Description"),
                                DescriptionEn = SafeString(it, "DescriptionEn"),
                                SectionKey = SafeString(it, "SectionKey"),
                                PersonName = SafeString(it, "PersonName"),
                                PersonNameEn = SafeString(it, "PersonNameEn"),
                                PersonPosition = SafeString(it, "PersonPosition"),
                                PersonPositionEn = SafeString(it, "PersonPositionEn"),
                                SortOrder = SafeDouble(it, "SortOrder")
                            });
                        }
                        items = items.OrderBy(x => x.SortOrder).ToList();

                        // Vision / Mission cards
                        var cards = items
                            .Where(x => x.SectionKey == "Vision" || x.SectionKey == "Mission")
                            .Select(x => new CardDto
                            {
                                DisplayTitle = IsArabic ? x.Title : Fallback(x.TitleEn, x.Title),
                                DisplayText = IsArabic ? x.Description : Fallback(x.DescriptionEn, x.Description),
                                IconClass = x.SectionKey == "Vision" ? "hgi-target-02" : "hgi-message-01"
                            })
                            .ToList();
                        rptCards.DataSource = cards;
                        rptCards.DataBind();

                        // Goals: one goal per line in the Note field
                        var goals = items.FirstOrDefault(x => x.SectionKey == "Goals");
                        phGoals.Visible = goals != null;
                        if (goals != null)
                        {
                            ltrGoalsTitle.Text = IsArabic ? goals.Title : Fallback(goals.TitleEn, goals.Title);
                            string body = IsArabic ? goals.Description : Fallback(goals.DescriptionEn, goals.Description);
                            var lines = (body ?? "")
                                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(l => l.Trim())
                                .Where(l => l.Length > 0)
                                .ToList();
                            rptGoals.DataSource = lines;
                            rptGoals.DataBind();
                        }

                    }
                });

                
                
                
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucCollegeAboutDga - BindData", ex.Message);
            }
        }

        private static string Fallback(string preferred, string fallback)
        {
            return string.IsNullOrWhiteSpace(preferred) ? fallback : preferred;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try
            {
                return item.Fields.ContainsField(field) && item[field] != null
                    ? item[field].ToString()
                    : string.Empty;
            }
            catch { return string.Empty; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null
                    && double.TryParse(item[field].ToString(), out v) ? v : 0;
            }
            catch { return 0; }
        }
    }

}
