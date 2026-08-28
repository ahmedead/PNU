using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes
{
    /// <summary>
    /// Model for general overview section (History story narrative & facility image / pillars header).
    /// </summary>
    public class AboutPnuOverviewItem
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string SubtitleAr { get; set; }
        public string SubtitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltAr { get; set; }
        public string ImageAltEn { get; set; }
        public string ImageCaptionAr { get; set; }
        public string ImageCaptionEn { get; set; }
        public int ItemOrder { get; set; }
        public bool Visibility { get; set; }

        public string Title { get { return AboutPnuHelper.Pick(TitleAr, TitleEn); } }
        public string Subtitle { get { return AboutPnuHelper.Pick(SubtitleAr, SubtitleEn); } }
        public string Description { get { return AboutPnuHelper.Pick(DescriptionAr, DescriptionEn); } }
        public string ImageAlt { get { return AboutPnuHelper.Pick(ImageAltAr, ImageAltEn); } }
        public string ImageCaption { get { return AboutPnuHelper.Pick(ImageCaptionAr, ImageCaptionEn); } }
    }

    /// <summary>
    /// Model for historical milestone cards.
    /// </summary>
    public class AboutPnuMilestoneItem
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string PeriodAr { get; set; }
        public string PeriodEn { get; set; }
        public string DateAttribute { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
        public bool Visibility { get; set; }

        public string Title { get { return AboutPnuHelper.Pick(TitleAr, TitleEn); } }
        public string Description { get { return AboutPnuHelper.Pick(DescriptionAr, DescriptionEn); } }
        public string Period { get { return AboutPnuHelper.Pick(PeriodAr, PeriodEn); } }
    }

    /// <summary>
    /// Model for university pillars (Vision, Mission, Values).
    /// </summary>
    public class AboutPnuPillarItem
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
        public bool Visibility { get; set; }

        public string Title { get { return AboutPnuHelper.Pick(TitleAr, TitleEn); } }
        public string Description { get { return AboutPnuHelper.Pick(DescriptionAr, DescriptionEn); } }
    }
}
