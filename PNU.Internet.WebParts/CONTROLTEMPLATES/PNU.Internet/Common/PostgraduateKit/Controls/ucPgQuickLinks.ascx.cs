using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>Single link cards used by the student housing and contact sections.</summary>
    public partial class ucPgQuickLinks : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.QuickLinks; } }

        protected override string DefaultSectionKey { get { return SectionKey; } }

        /// <summary>Which section this instance renders: Housing or Contact.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionKey { get; set; }

        public ucPgQuickLinks()
        {
            SectionKey = PgSectionKeys.Housing;
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
                string key = PgHelper.SafeString(item, "SectionKey").Trim();
                if (!string.IsNullOrEmpty(SectionKey) && key.Length > 0 &&
                    !key.Equals(SectionKey, StringComparison.OrdinalIgnoreCase))
                    continue;

                cards.Add(new PgCard
                {
                    Id = item.ID,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(cards.Count)
                });
            }

            if (rptLinks != null)
            {
                rptLinks.DataSource = cards;
                rptLinks.DataBind();
            }
        }
    }
}
