using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The tuition exemption and reduction table, followed by its condition cards.</summary>
    public partial class ucPgDiscounts : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Discounts; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Discounts; } }

        protected string CategoryHeader
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_DiscountCategory", "نوع الخصم / الفئة المستهدفة", "Type of reduction / eligible category")); }
        }

        protected string ValueHeader
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_DiscountValue", "نسبة الخصم على إجمالي الرسوم", "Reduction applied to the total fees")); }
        }

        protected string NotesHeading
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_DiscountNotes", "ضوابط تطبيق الإعفاء والتخفيض:", "Rules governing the exemption and the reduction:")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var rows = new List<PgDiscountRow>();

            for (int i = 0; i < raw.Count; i++)
            {
                SPListItem item = raw[i];

                rows.Add(new PgDiscountRow
                {
                    Id = item.ID,
                    Category = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Value = PgHelper.Enc(PgHelper.PickField(item, "DiscountValue")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    RowClass = (i % 2 == 0) ? "list-group-item" : "list-group-item list-group-item-primary"
                });
            }

            if (rptRows != null)
            {
                rptRows.DataSource = rows;
                rptRows.DataBind();
            }

            BindNotes();
        }

        private void BindNotes()
        {
            try
            {
                PgListProvisioner.EnsureListExists(PgListNames.DiscountNotes);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    var notes = new List<PgCard>();
                    List<SPListItem> raw = PgHelper.GetItems(web, PgListNames.DiscountNotes, EffectivePhase);

                    for (int i = 0; i < raw.Count; i++)
                    {
                        notes.Add(new PgCard
                        {
                            Id = raw[i].ID,
                            Title = PgHelper.Enc(PgHelper.PickField(raw[i], "Title")),
                            ItemOrder = PgHelper.SafeInt(raw[i], "ItemOrder"),
                            AccentClass = Accent(i)
                        });
                    }

                    if (rptNotes != null)
                    {
                        rptNotes.DataSource = notes;
                        rptNotes.DataBind();
                    }

                    if (pnlNotes != null) pnlNotes.Visible = notes.Count > 0;
                }
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgDiscounts.BindNotes", ex);
            }
        }
    }
}
