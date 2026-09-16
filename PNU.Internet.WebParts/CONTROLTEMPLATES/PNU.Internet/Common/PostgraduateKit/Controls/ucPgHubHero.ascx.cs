using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>The hero carousel at the top of the kit landing page.</summary>
    public partial class ucPgHubHero : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Hub; } }

        protected override string DefaultSectionKey { get { return string.Empty; } }

        protected string CarouselLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_HeroLabel", "أبرز المحتوى", "Featured content")); }
        }

        public ucPgHubHero()
        {
            Phase = PgPhases.Hub;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var slides = new List<PgHeroSlide>();

            foreach (SPListItem item in raw)
            {
                if (!PgListSchema.PgHubKinds.Slide.Equals(
                        PgHelper.SafeString(item, "Kind").Trim(), StringComparison.OrdinalIgnoreCase))
                    continue;

                string imageBase = PgHelper.SafeString(item, "ImageBase").Trim();
                if (imageBase.Length == 0) imageBase = PgListSchema.HeroBase;

                slides.Add(new PgHeroSlide
                {
                    Id = item.ID,
                    Index = slides.Count,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    ParagraphsHtml = PgHelper.ParagraphHtml(PgHelper.PickField(item, "Paragraphs"),
                                                            "pnu-hero-text fs-4 mb-2"),
                    ImageAlt = PgHelper.Enc(PgHelper.PickField(item, "ImageAlt")),
                    ImageBase = PgHelper.Enc(imageBase)
                });
            }

            foreach (PgHeroSlide slide in slides) slide.Total = slides.Count;

            if (rptSlides != null)
            {
                rptSlides.DataSource = slides;
                rptSlides.DataBind();
            }

            if (rptIndicators != null)
            {
                rptIndicators.DataSource = slides;
                rptIndicators.DataBind();
            }

            // A single slide needs no indicators, and no carousel chrome at all when empty.
            if (phIndicators != null) phIndicators.Visible = slides.Count > 1;
            if (phHero != null) phHero.Visible = slides.Count > 0;
        }
    }
}
