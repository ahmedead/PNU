using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The frequently asked questions of one audience, as expandable cards.</summary>
    public partial class ucPgFaq : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Faq; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Faq; } }

        protected string FaqIconUrl { get { return PgListSchema.IconRoot + "faq-icon.png"; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var items = new List<PgFaqItem>();

            for (int i = 0; i < raw.Count; i++)
            {
                SPListItem item = raw[i];

                items.Add(new PgFaqItem
                {
                    Id = item.ID,
                    Question = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Answer = PgHelper.Enc(PgHelper.PickField(item, "Description")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(i)
                });
            }

            if (rptFaq != null)
            {
                rptFaq.DataSource = items;
                rptFaq.DataBind();
            }
        }
    }
}
