using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The numbered journey steps (admission, academic or graduation).</summary>
    public partial class ucPgJourney : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Journey; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Journey; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var cards = new List<PgCard>();

            for (int i = 0; i < raw.Count; i++)
            {
                SPListItem item = raw[i];

                cards.Add(new PgCard
                {
                    Id = item.ID,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Description = PgHelper.Enc(PgHelper.PickField(item, "Description")),
                    SubTitle = PgHelper.Enc(PgHelper.PickField(item, "StepLabel")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(i)
                });
            }

            if (rptSteps != null)
            {
                rptSteps.DataSource = cards;
                rptSteps.DataBind();
            }
        }
    }
}
