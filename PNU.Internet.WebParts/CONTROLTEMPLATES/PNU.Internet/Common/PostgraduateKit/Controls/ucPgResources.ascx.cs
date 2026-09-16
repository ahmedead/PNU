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

            if (rptResources != null)
            {
                rptResources.DataSource = cards;
                rptResources.DataBind();
            }
        }
    }
}
