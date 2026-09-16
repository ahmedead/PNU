using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Link and document cards for one group: programs, regulations, official guides
    /// or graduation requirements.
    /// </summary>
    public partial class ucPgResources : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Resources; } }

        protected override string DefaultSectionKey { get { return GroupKey; } }

        /// <summary>Which group of resource cards this instance renders.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string GroupKey { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool RenderOuterContainer { get; set; }

        protected string DownloadIconUrl
        {
            get { return PgListSchema.IconRoot + "download-icon.png"; }
        }

        protected string DownloadLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_Download", "تحميل الملف", "Download the file")); }
        }

        public ucPgResources()
        {
            GroupKey = PgSectionKeys.Programs;
            RenderOuterContainer = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var cards = new List<PgCard>();

            foreach (SPListItem item in raw)
            {
                string group = PgHelper.SafeString(item, "GroupKey").Trim();
                if (!string.IsNullOrEmpty(GroupKey) && group.Length > 0 &&
                    !group.Equals(GroupKey, StringComparison.OrdinalIgnoreCase))
                    continue;

                cards.Add(new PgCard
                {
                    Id = item.ID,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Description = PgHelper.Enc(PgHelper.PickField(item, "Description")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    GroupKey = PgHelper.Enc(group),
                    ShowDownload = PgHelper.SafeBool(item, "ShowDownload"),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(cards.Count)
                });
            }

            if (cards.Count == 0)
            {
                PgListDef def = PgListSchema.Get(ListName);
                if (def != null && def.Seed != null)
                {
                    foreach (Dictionary<string, string> row in def.Seed)
                    {
                        string rowPhase = row.ContainsKey("Phase") ? row["Phase"] : string.Empty;
                        string rowGroup = row.ContainsKey("GroupKey") ? row["GroupKey"] : string.Empty;

                        if (!string.IsNullOrEmpty(EffectivePhase) && !string.IsNullOrEmpty(rowPhase) &&
                            !rowPhase.Equals(EffectivePhase, StringComparison.OrdinalIgnoreCase))
                            continue;

                        if (!string.IsNullOrEmpty(GroupKey) && !string.IsNullOrEmpty(rowGroup) &&
                            !rowGroup.Equals(GroupKey, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string title = PgHelper.Pick(row.ContainsKey("Title") ? row["Title"] : "", row.ContainsKey("Title_EN") ? row["Title_EN"] : "");
                        string desc = PgHelper.Pick(row.ContainsKey("Description") ? row["Description"] : "", row.ContainsKey("Description_EN") ? row["Description_EN"] : "");
                        string url = row.ContainsKey("LinkUrl") ? row["LinkUrl"] : "#";
                        string icon = row.ContainsKey("IconUrl") ? row["IconUrl"] : "";
                        bool showDownload = row.ContainsKey("ShowDownload") && row["ShowDownload"].Equals("Yes", StringComparison.OrdinalIgnoreCase);
                        int order = 0;
                        if (row.ContainsKey("ItemOrder")) int.TryParse(row["ItemOrder"], out order);

                        cards.Add(new PgCard
                        {
                            Title = PgHelper.Enc(title),
                            Description = PgHelper.Enc(desc),
                            LinkUrl = PgHelper.Enc(url),
                            IconUrl = PgHelper.Enc(icon),
                            GroupKey = PgHelper.Enc(rowGroup),
                            ShowDownload = showDownload,
                            ItemOrder = order,
                            AccentClass = Accent(cards.Count)
                        });
                    }
                }
            }

            if (rptResources != null)
            {
                rptResources.DataSource = cards;
                rptResources.DataBind();
            }
        }
    }
}
