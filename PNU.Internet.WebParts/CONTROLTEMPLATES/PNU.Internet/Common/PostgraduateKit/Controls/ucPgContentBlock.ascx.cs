using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Free-form content cards: the English preparatory course, the graduate identity,
    /// the financial rules, the publication incentive and the reference footer.
    /// </summary>
    public partial class ucPgContentBlock : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.ContentBlocks; } }

        protected override string DefaultSectionKey { get { return BlockKey; } }

        /// <summary>Which kind of block this instance renders.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string BlockKey { get; set; }

        protected string ToggleIconUrl
        {
            get { return PgListSchema.IconRoot + "arrow-r-l.png"; }
        }

        public ucPgContentBlock()
        {
            BlockKey = PgSectionKeys.EnglishCourse;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var blocks = new List<PgBlock>();

            foreach (SPListItem item in raw)
            {
                string key = PgHelper.SafeString(item, "BlockKey").Trim();
                if (!string.IsNullOrEmpty(BlockKey) && key.Length > 0 &&
                    !key.Equals(BlockKey, StringComparison.OrdinalIgnoreCase))
                    continue;

                blocks.Add(new PgBlock
                {
                    Id = item.ID,
                    Key = PgHelper.Enc(key),
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    SubTitle = PgHelper.Enc(PgHelper.PickField(item, "SubTitle")),
                    Badge = PgHelper.Enc(PgHelper.PickField(item, "Badge")),
                    StatValue = PgHelper.Enc(PgHelper.SafeString(item, "StatValue")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    ParagraphsHtml = PgHelper.ParagraphHtml(PgHelper.PickField(item, "Paragraphs"), "mb-2 fs-custom-4"),
                    BulletsHtml = PgHelper.BulletHtml(PgHelper.PickField(item, "Bullets"), "fs-custom-4"),
                    Highlights = PgHelper.ParseHighlights(PgHelper.PickField(item, "Highlights")),
                    FooterTitle = PgHelper.Enc(PgHelper.PickField(item, "FooterTitle")),
                    FooterParagraphsHtml = PgHelper.ParagraphHtml(PgHelper.PickField(item, "FooterParagraphs"), "mb-2 fs-custom-4"),
                    Collapsible = PgHelper.SafeBool(item, "Collapsible"),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(blocks.Count + 1)
                });
            }

            if (rptBlocks != null)
            {
                rptBlocks.DataSource = blocks;
                rptBlocks.DataBind();
            }
        }
    }
}
