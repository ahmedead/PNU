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

                string heading         = SharedTitleReader.Get(titles, SharedTitles.OverviewHeading);
                string intro           = SharedTitleReader.Get(titles, SharedTitles.OverviewIntro);
                string visionTitle     = SharedTitleReader.Get(titles, SharedTitles.VisionTitle);
                string visionBody      = SharedTitleReader.Get(titles, SharedTitles.VisionBody);
                string missionTitle    = SharedTitleReader.Get(titles, SharedTitles.MissionTitle);
                string missionBody     = SharedTitleReader.Get(titles, SharedTitles.MissionBody);
                string objectivesTitle = SharedTitleReader.Get(titles, SharedTitles.ObjectivesTitle);

                // 1. Overview Heading
                bool hasHeading = !string.IsNullOrWhiteSpace(heading);
                litOverviewHeading.Text = heading;
                litOverviewHeading.Visible = hasHeading;
                if (hOverviewHeading != null) hOverviewHeading.Visible = hasHeading;

                // 2. Overview Intro
                bool hasIntro = !string.IsNullOrWhiteSpace(intro);
                litOverviewIntro.Text = intro;
                litOverviewIntro.Visible = hasIntro;
                if (pOverviewIntro != null) pOverviewIntro.Visible = hasIntro;

                // Text column visibility
                if (pnlOverviewText != null) pnlOverviewText.Visible = (hasHeading || hasIntro);

                // 3. Image
                bool hasImage = LoadImage();
                if (pnlOverviewImage != null) pnlOverviewImage.Visible = hasImage;

                // Text column full-width if no image present
                if ((hasHeading || hasIntro) && !hasImage && pnlOverviewText != null)
                {
                    pnlOverviewText.Attributes["class"] = "col-12";
                }
                else if (pnlOverviewText != null)
                {
                    pnlOverviewText.Attributes["class"] = "col-12 col-md-6 col-lg-8";
                }

                // Overview Header Row visibility
                if (pnlOverviewHeader != null) pnlOverviewHeader.Visible = (hasHeading || hasIntro || hasImage);

                // 4. Vision Card
                bool hasVisionTitle = !string.IsNullOrWhiteSpace(visionTitle);
                litVisionTitle.Text = visionTitle;
                litVisionTitle.Visible = hasVisionTitle;
                if (hVisionTitle != null) hVisionTitle.Visible = hasVisionTitle;

                bool hasVisionBody = !string.IsNullOrWhiteSpace(visionBody);
                litVisionBody.Text = visionBody;
                litVisionBody.Visible = hasVisionBody;
                if (pVisionBody != null) pVisionBody.Visible = hasVisionBody;

                bool hasVision = (hasVisionTitle || hasVisionBody);
                if (pnlVision != null) pnlVision.Visible = hasVision;

                // 5. Mission Card
                bool hasMissionTitle = !string.IsNullOrWhiteSpace(missionTitle);
                litMissionTitle.Text = missionTitle;
                litMissionTitle.Visible = hasMissionTitle;
                if (hMissionTitle != null) hMissionTitle.Visible = hasMissionTitle;

                bool hasMissionBody = !string.IsNullOrWhiteSpace(missionBody);
                litMissionBody.Text = missionBody;
                litMissionBody.Visible = hasMissionBody;
                if (pMissionBody != null) pMissionBody.Visible = hasMissionBody;

                bool hasMission = (hasMissionTitle || hasMissionBody);
                if (pnlMission != null) pnlMission.Visible = hasMission;

                // Responsive layout tuning when only one of Vision or Mission card exists
                if (hasVision && !hasMission && pnlVision != null)
                {
                    pnlVision.Attributes["class"] = "col-12";
                }
                else if (hasMission && !hasVision && pnlMission != null)
                {
                    pnlMission.Attributes["class"] = "col-12";
                }
                else
                {
                    if (pnlVision != null) pnlVision.Attributes["class"] = "col-12 col-md-6 col-sm-12";
                    if (pnlMission != null) pnlMission.Attributes["class"] = "col-12 col-md-6 col-sm-12";
                }

                // 6. Objectives Card
                bool hasObjectivesTitle = !string.IsNullOrWhiteSpace(objectivesTitle);
                litObjectivesTitle.Text = objectivesTitle;
                litObjectivesTitle.Visible = hasObjectivesTitle;
                if (hObjectivesTitle != null) hObjectivesTitle.Visible = hasObjectivesTitle;

                var objectives = LoadObjectives();
                bool hasObjectivesList = (objectives != null && objectives.Count > 0);
                rptObjectives.DataSource = objectives;
                rptObjectives.DataBind();
                rptObjectives.Visible = hasObjectivesList;
                if (ulObjectives != null) ulObjectives.Visible = hasObjectivesList;

                bool hasObjectives = (hasObjectivesTitle || hasObjectivesList);
                if (pnlObjectives != null) pnlObjectives.Visible = hasObjectives;

                // Cards Row visibility
                bool hasCards = (hasVision || hasMission || hasObjectives);
                if (pnlCardsRow != null) pnlCardsRow.Visible = hasCards;

                // Section visibility
                if (secOverview != null) secOverview.Visible = (hasHeading || hasIntro || hasImage || hasCards);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.BindData", ex.Message);
            }
        }

        private bool LoadImage()
        {
            bool hasImage = false;
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

                if (!string.IsNullOrEmpty(url))
                {
                    imgOverview.Src = url;
                    imgOverview.Alt = alt ?? string.Empty;
                    imgOverview.Visible = true;
                    hasImage = true;
                }
                else
                {
                    imgOverview.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucAgencyOverviewDga.LoadImage", ex.Message);
            }
            return hasImage;
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
