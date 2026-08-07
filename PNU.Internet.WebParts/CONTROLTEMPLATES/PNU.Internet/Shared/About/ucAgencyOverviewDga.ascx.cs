using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About
{
    public partial class ucAgencyOverviewDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Provision only for authenticated users; anonymous reads happen elevated at bind time.
            try
            {
                if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                SPContext.Current != null &&
                SPContext.Current.Web.CurrentUser != null)
                {
                    SharedTitles.EnsureList(SPContext.Current.Web);
                    AgencyOverviewProvisioner.EnsureList(SPContext.Current.Web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.Page_Init", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindData();
        }

        private void BindData()
        {
            try
            {
                var titles = SharedTitleReader.Load(IsArabic);

                litOverviewHeading.Text = SharedTitleReader.Get(titles, SharedTitles.OverviewHeading);
                litOverviewIntro.Text   = SharedTitleReader.Get(titles, SharedTitles.OverviewIntro);
                litVisionTitle.Text     = SharedTitleReader.Get(titles, SharedTitles.VisionTitle);
                litVisionBody.Text      = SharedTitleReader.Get(titles, SharedTitles.VisionBody);
                litMissionTitle.Text    = SharedTitleReader.Get(titles, SharedTitles.MissionTitle);
                litMissionBody.Text     = SharedTitleReader.Get(titles, SharedTitles.MissionBody);
                litObjectivesTitle.Text = SharedTitleReader.Get(titles, SharedTitles.ObjectivesTitle);

                LoadImage();

                rptObjectives.DataSource = LoadObjectives();
                rptObjectives.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.BindData", ex.Message);
            }
        }

        private void LoadImage()
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId  = SPContext.Current.Web.ID;
                string url = string.Empty;
                string alt = string.Empty;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var web  = site.OpenWeb(webId))
                    {
                        var list = web.Lists.TryGetList(AgencyOverviewProvisioner.SettingsList);
                        if (list == null || list.ItemCount == 0) return;

                        SPListItem item = list.Items[0];
                        var urlVal = item["ImageUrl"] as string;
                        if (!string.IsNullOrEmpty(urlVal))
                        {
                            // URL field renders as "url, description"
                            var parts = urlVal.Split(new[] { ',' }, 2);
                            url = parts[0].Trim();
                        }
                        alt = SafeString(item, "ImageAlt");
                    }
                });

                if (!string.IsNullOrEmpty(url)) imgOverview.Src = url;
                imgOverview.Alt = alt ?? string.Empty;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.LoadImage", ex.Message);
            }
        }

        private List<ObjectiveItem> LoadObjectives()
        {
            var result = new List<ObjectiveItem>();
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId  = SPContext.Current.Web.ID;
                bool ar     = IsArabic;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var web  = site.OpenWeb(webId))
                    {
                        var list = web.Lists.TryGetList(AgencyOverviewProvisioner.ObjectivesList);
                        if (list == null) return;

                        var query = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
                        };
                        foreach (SPListItem item in list.GetItems(query))
                        {
                            string text = SafeString(item, ar ? "TextAr" : "TextEn");
                            if (string.IsNullOrEmpty(text)) text = SafeString(item, "TextAr");
                            if (!string.IsNullOrEmpty(text))
                                result.Add(new ObjectiveItem { Text = text });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.LoadObjectives", ex.Message);
            }
            return result;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item[field]?.ToString() ?? string.Empty; }
            catch { return string.Empty; }
        }

        public class ObjectiveItem
        {
            public string Text { get; set; }
        }
    }
}
