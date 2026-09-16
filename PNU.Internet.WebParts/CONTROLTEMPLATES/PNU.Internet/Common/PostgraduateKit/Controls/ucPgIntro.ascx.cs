using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The introductory paragraph card at the top of each audience page.</summary>
    public partial class ucPgIntro : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Intro; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Intro; } }

        protected string Body { get; private set; }

        public ucPgIntro()
        {
            Body = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            if (raw.Count == 0) return;

            // One paragraph per line, so the hub overview can run to several.
            Body = PgHelper.ParagraphHtml(PgHelper.PickField(raw[0], "Description"),
                                          "fw-custom-bold fs-custom-4");
        }
    }
}
