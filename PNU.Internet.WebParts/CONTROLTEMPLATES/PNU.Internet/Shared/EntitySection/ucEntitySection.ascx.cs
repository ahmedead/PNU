using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.EntitySection
{
    public partial class ucEntitySection : UserControl
    {
        #region Properties

        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("EntitySectionTracks"),
         Description("Name of the SharePoint list containing tracks/groups.")]
        public string TracksListName { get; set; }

        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("EntitySectionBullets"),
         Description("Name of the SharePoint list containing track bullet points.")]
        public string BulletsListName { get; set; }

        [WebBrowsable(true),
         Category("UI Settings"),
         DefaultValue(""),
         Description("Override section title. Leave empty for default localized title.")]
        public string CustomSectionTitle { get; set; }

        [WebBrowsable(true),
         Category("UI Settings"),
         DefaultValue(false),
         Description("Whether the first track accordion item should be expanded by default.")]
        public bool ExpandFirstItem { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null &&
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated &&
                    SPContext.Current != null &&
                    SPContext.Current.Web.CurrentUser != null)
                {
                    string tListName = string.IsNullOrWhiteSpace(TracksListName) ? EntitySectionProvisioner.DefaultTracksList : TracksListName;
                    string bListName = string.IsNullOrWhiteSpace(BulletsListName) ? EntitySectionProvisioner.DefaultBulletsList : BulletsListName;
                    
                    Guid siteId = SPContext.Current.Site.ID;
                    Guid webId = SPContext.Current.Web.ID;

                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (var site = new SPSite(siteId))
                        using (var web = site.OpenWeb(webId))
                        {
                            EntitySectionProvisioner.EnsureLists(web, tListName, bListName);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucEntitySection.Page_Init", ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(CustomSectionTitle))
                {
                    litSectionTitle.Text = CustomSectionTitle;
                }
                else
                {
                    litSectionTitle.Text = IsArabic ? "مسارات المستفيدين" : "Beneficiary Tracks";
                }

                var tracks = LoadTracks();
                rptTracks.DataSource = tracks;
                rptTracks.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucEntitySection.BindData", ex.Message);
            }
        }

        protected void rptTracks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var track = e.Item.DataItem as TrackData;
            if (track == null) return;

            var litOpen = e.Item.FindControl("litListOpenTag") as Literal;
            var litClose = e.Item.FindControl("litListCloseTag") as Literal;
            var rptBullets = e.Item.FindControl("rptBullets") as Repeater;

            if (track.Bullets != null && track.Bullets.Count > 0)
            {
                if (litOpen != null && litClose != null)
                {
                    if (string.Equals(track.ListStyle, "number", StringComparison.OrdinalIgnoreCase))
                    {
                        litOpen.Text = "<ol class=\"mb-0\">";
                        litClose.Text = "</ol>";
                    }
                    else
                    {
                        litOpen.Text = "<ul class=\"mb-0\">";
                        litClose.Text = "</ul>";
                    }
                }

                if (rptBullets != null)
                {
                    rptBullets.DataSource = track.Bullets;
                    rptBullets.DataBind();
                }
            }
        }

        private List<TrackData> LoadTracks()
        {
            var result = new List<TrackData>();
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;
                bool ar = IsArabic;

                string tListName = string.IsNullOrWhiteSpace(TracksListName) ? EntitySectionProvisioner.DefaultTracksList : TracksListName;
                string bListName = string.IsNullOrWhiteSpace(BulletsListName) ? EntitySectionProvisioner.DefaultBulletsList : BulletsListName;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var web = site.OpenWeb(webId))
                    {
                        var trackList = web.Lists.TryGetList(tListName);
                        var bulletList = web.Lists.TryGetList(bListName);
                        if (trackList == null) return;

                        var gq = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
                        };

                        int index = 0;
                        foreach (SPListItem g in trackList.GetItems(gq))
                        {
                            index++;
                            var track = new TrackData
                            {
                                Index = index,
                                IsExpanded = ExpandFirstItem && (index == 1),
                                Title = SafeString(g, ar ? "TitleAr" : "TitleEn"),
                                Description = SafeString(g, ar ? "DescriptionAr" : "DescriptionEn"),
                                ListStyle = SafeString(g, "ListStyle"),
                                Bullets = new List<BulletData>()
                            };

                            if (string.IsNullOrEmpty(track.Title))
                                track.Title = SafeString(g, "TitleAr");
                            if (string.IsNullOrEmpty(track.Description))
                                track.Description = SafeString(g, "DescriptionAr");

                            int groupId = g.ID;
                            if (bulletList != null)
                            {
                                var pq = new SPQuery
                                {
                                    Query = "<Where><Eq><FieldRef Name='GroupId'/>" +
                                            "<Value Type='Number'>" + groupId + "</Value></Eq></Where>" +
                                            "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE'/></OrderBy>"
                                };
                                foreach (SPListItem p in bulletList.GetItems(pq))
                                {
                                    string text = SafeString(p, ar ? "TextAr" : "TextEn");
                                    if (string.IsNullOrEmpty(text)) text = SafeString(p, "TextAr");
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        track.Bullets.Add(new BulletData { Text = text });
                                    }
                                }
                            }
                            result.Add(track);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucEntitySection.LoadTracks", ex.Message);
            }
            return result;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item[field]?.ToString() ?? string.Empty; }
            catch { return string.Empty; }
        }

        public class TrackData
        {
            public int Index { get; set; }
            public bool IsExpanded { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ListStyle { get; set; }
            public List<BulletData> Bullets { get; set; }
        }

        public class BulletData
        {
            public string Text { get; set; }
        }
    }
}
