using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The "choose your station" cards on the kit landing page.</summary>
    public partial class ucPgHubCards : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Hub; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Audiences; } }

        protected string PreviousLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_PrevCard", "المحطة السابقة", "Previous station")); }
        }

        protected string NextLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_NextCard", "المحطة التالية", "Next station")); }
        }

        public ucPgHubCards()
        {
            Phase = PgPhases.Hub;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var cards = new List<PgHubCardModel>();

            foreach (SPListItem item in raw)
            {
                if (!PgListSchema.PgHubKinds.Card.Equals(
                        PgHelper.SafeString(item, "Kind").Trim(), StringComparison.OrdinalIgnoreCase))
                    continue;

                cards.Add(new PgHubCardModel
                {
                    Id = item.ID,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Description = PgHelper.Enc(PgHelper.PickField(item, "Paragraphs")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    ButtonText = PgHelper.Enc(PgHelper.PickField(item, "ButtonText")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder")
                });
            }

            if (rptCards != null)
            {
                rptCards.DataSource = cards;
                rptCards.DataBind();
            }

            // The three stations fit without paging on a desktop; the arrows only earn
            // their place once an editor adds a fourth card.
            if (phNav != null) phNav.Visible = cards.Count > 3;
        }
    }
}
