using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    public class PgBreadcrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsLast { get; set; }
    }

    public class PgLinkItem
    {
        public string Text { get; set; }
        public string Url { get; set; }

        /// <summary>Optional icon shown beside the text (used by the highlight rows).</summary>
        public string Icon { get; set; }

        /// <summary>Whether the link opens in a new tab (used by the footer links).</summary>
        public bool NewTab { get; set; }

        public bool HasIcon { get { return !string.IsNullOrEmpty(Icon); } }
        public bool HasUrl { get { return !string.IsNullOrEmpty(Url) && Url != "#"; } }

        public string Target { get { return NewTab ? "_blank" : "_self"; } }
        public string Rel { get { return NewTab ? "noopener noreferrer" : string.Empty; } }
    }

    public class PgHeaderModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<PgBreadcrumbItem> BreadcrumbItems { get; set; }

        public PgHeaderModel()
        {
            BreadcrumbItems = new List<PgBreadcrumbItem>();
        }

        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
    }

    /// <summary>Heading and lead paragraph of one axis, edited through the PgSections list.</summary>
    public class PgSectionInfo
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Lead { get; set; }
        public bool Visible { get; set; }

        public PgSectionInfo()
        {
            Visible = true;
            Key = string.Empty;
            Title = string.Empty;
            Lead = string.Empty;
        }

        public bool HasTitle { get { return !string.IsNullOrEmpty(Title); } }
        public bool HasLead { get { return !string.IsNullOrEmpty(Lead); } }
    }

    /// <summary>
    /// Generic card used by the journey, resources, services and quick-link sections.
    /// Every string is already HTML-encoded by the projecting control.
    /// </summary>
    public class PgCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubTitle { get; set; }
        public string Badge { get; set; }
        public string StatValue { get; set; }
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public int ItemOrder { get; set; }
        public bool ShowDownload { get; set; }

        /// <summary>Alternates the accent colour the way the static design does.</summary>
        public string AccentClass { get; set; }

        public PgCard()
        {
            AccentClass = "border-light-blue";
            IconUrl = string.Empty;
            LinkUrl = string.Empty;
        }

        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool HasIcon { get { return !string.IsNullOrEmpty(IconUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasBadge { get { return !string.IsNullOrEmpty(Badge); } }
        public bool HasSubTitle { get { return !string.IsNullOrEmpty(SubTitle); } }
        public bool HasStatValue { get { return !string.IsNullOrEmpty(StatValue); } }

        public string SafeLinkUrl { get { return HasLink ? LinkUrl : "#"; } }

        /// <summary>Wraps the title in an anchor when a link exists, otherwise renders plain text.</summary>
        public string TitleHtml
        {
            get
            {
                if (!HasLink) return Title ?? string.Empty;
                return "<a class=\"text-body\" href=\"" + LinkUrl + "\">" + (Title ?? string.Empty) + "</a>";
            }
        }

        public string DescriptionHtml
        {
            get
            {
                if (!HasDescription) return string.Empty;
                if (!HasLink) return Description;
                return "<a class=\"text-body\" href=\"" + LinkUrl + "\">" + Description + "</a>";
            }
        }
    }

    /// <summary>One row of the fee exemption / reduction table.</summary>
    public class PgDiscountRow
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public int ItemOrder { get; set; }

        /// <summary>Zebra striping identical to the static design.</summary>
        public string RowClass { get; set; }
    }

    /// <summary>A rich free-form block: the English course card, the publishing incentive, the footer note.</summary>
    public class PgBlock
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Badge { get; set; }
        public string StatValue { get; set; }
        public string IconUrl { get; set; }
        public string ParagraphsHtml { get; set; }
        public string BulletsHtml { get; set; }
        public List<PgLinkItem> Highlights { get; set; }
        public bool Collapsible { get; set; }
        public int ItemOrder { get; set; }
        public string AccentClass { get; set; }

        /// <summary>Optional closing sub-heading and paragraphs rendered inside the same card.</summary>
        public string FooterTitle { get; set; }
        public string FooterParagraphsHtml { get; set; }

        public PgBlock()
        {
            Highlights = new List<PgLinkItem>();
            AccentClass = "border-dark-blue";
        }

        private string _collapseId;
        public string CollapseId
        {
            get
            {
                if (!string.IsNullOrEmpty(_collapseId)) return _collapseId;
                if (!string.IsNullOrEmpty(Key))
                    return "pgBlock_" + Key + "_" + Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
                return "pgBlock" + Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            set { _collapseId = value; }
        }
        public string CollapseHref { get { return "#" + CollapseId; } }

        public bool HasTitle { get { return !string.IsNullOrEmpty(Title); } }
        public bool HasSubTitle { get { return !string.IsNullOrEmpty(SubTitle); } }
        public bool HasBadge { get { return !string.IsNullOrEmpty(Badge); } }
        public bool HasStatValue { get { return !string.IsNullOrEmpty(StatValue); } }
        public bool HasIcon { get { return !string.IsNullOrEmpty(IconUrl); } }
        public bool HasParagraphs { get { return !string.IsNullOrEmpty(ParagraphsHtml); } }
        public bool HasBullets { get { return !string.IsNullOrEmpty(BulletsHtml); } }
        public bool HasHighlights { get { return Highlights != null && Highlights.Count > 0; } }
        public bool HasFooter { get { return !string.IsNullOrEmpty(FooterTitle) || !string.IsNullOrEmpty(FooterParagraphsHtml); } }
        public bool HasFooterTitle { get { return !string.IsNullOrEmpty(FooterTitle); } }
    }

    /// <summary>One academic procedure card with its conditions, regulation article and forms.</summary>
    public class PgProcedure
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Badge { get; set; }
        public string Description { get; set; }
        public string GroupLabel { get; set; }
        public string ConditionsHtml { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleTextHtml { get; set; }
        public string IconUrl { get; set; }
        public string FormsHeading { get; set; }
        public List<PgLinkItem> Forms { get; set; }
        public int ItemOrder { get; set; }
        public string AccentClass { get; set; }

        public PgProcedure()
        {
            Forms = new List<PgLinkItem>();
            AccentClass = "border-light-blue";
        }

        public string CollapseId { get { return "pgProcedure" + Id.ToString(System.Globalization.CultureInfo.InvariantCulture); } }
        public string CollapseHref { get { return "#" + CollapseId; } }

        public bool HasBadge { get { return !string.IsNullOrEmpty(Badge); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasConditions { get { return !string.IsNullOrEmpty(ConditionsHtml); } }
        public bool HasArticle { get { return !string.IsNullOrEmpty(ArticleTitle) || !string.IsNullOrEmpty(ArticleTextHtml); } }
        public bool HasForms { get { return Forms != null && Forms.Count > 0; } }
    }

    /// <summary>A group of procedures rendered under one "3.1 / 3.2" sub-heading.</summary>
    public class PgProcedureGroup
    {
        public string Label { get; set; }
        public List<PgProcedure> Items { get; set; }

        public PgProcedureGroup()
        {
            Items = new List<PgProcedure>();
        }

        public bool HasLabel { get { return !string.IsNullOrEmpty(Label); } }
    }

    /// <summary>A group of service links rendered under one sub-heading.</summary>
    public class PgServiceGroup
    {
        public string Name { get; set; }
        public List<PgCard> Items { get; set; }

        public PgServiceGroup()
        {
            Items = new List<PgCard>();
        }

        public bool HasName { get { return !string.IsNullOrEmpty(Name); } }
    }

    /// <summary>One entry of the strip above the main navigation.</summary>
    public class PgTopBarItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public string AriaLabel { get; set; }

        /// <summary>"Date" or "Time" when the browser fills the value in, otherwise empty.</summary>
        public string LiveValue { get; set; }

        public int ItemOrder { get; set; }

        public bool HasIcon { get { return !string.IsNullOrEmpty(IconUrl); } }
        public bool HasText { get { return !string.IsNullOrEmpty(Text); } }
        public bool HasLink { get { return !string.IsNullOrEmpty(LinkUrl); } }
        public bool IsLive { get { return !string.IsNullOrEmpty(LiveValue); } }

        public string SafeLinkUrl { get { return HasLink ? LinkUrl : "#"; } }

        /// <summary>Class the inline clock script looks for.</summary>
        public string LiveClass
        {
            get
            {
                if (!IsLive) return string.Empty;
                return "Time".Equals(LiveValue, StringComparison.OrdinalIgnoreCase)
                    ? "js-pg-time" : "js-pg-date";
            }
        }
    }

    /// <summary>One slide of the landing page carousel.</summary>
    public class PgHeroSlide
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int Total { get; set; }
        public string Title { get; set; }
        public string ParagraphsHtml { get; set; }
        public string ImageAlt { get; set; }

        /// <summary>Path without the size suffix, e.g. ".../images/hero/hero-library".</summary>
        public string ImageBase { get; set; }

        public bool IsFirst { get { return Index == 0; } }

        public string SmallAvif { get { return ImageBase + "-sm.avif"; } }
        public string MediumAvif { get { return ImageBase + "-md.avif"; } }
        public string LargeAvif { get { return ImageBase + "-lg.avif"; } }
        public string SmallWebp { get { return ImageBase + "-sm.webp"; } }
        public string MediumWebp { get { return ImageBase + "-md.webp"; } }
        public string LargeWebp { get { return ImageBase + "-lg.webp"; } }
        public string Fallback { get { return ImageBase + "-sm.jpg"; } }

        /// <summary>"1 من 3" / "1 of 3", matching the design's aria-label.</summary>
        public string SlideLabel
        {
            get
            {
                string n = (Index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
                string t = Total.ToString(System.Globalization.CultureInfo.InvariantCulture);
                return PgHelper.Enc(PgHelper.Pick(n + " من " + t, "Slide " + n + " of " + t));
            }
        }
    }

    /// <summary>One station card on the landing page.</summary>
    public class PgHubCardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string ButtonText { get; set; }
        public string LinkUrl { get; set; }
        public int ItemOrder { get; set; }

        public bool HasIcon { get { return !string.IsNullOrEmpty(IconUrl); } }
        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }
        public bool HasButton { get { return !string.IsNullOrEmpty(ButtonText) && !string.IsNullOrEmpty(LinkUrl); } }
        public string SafeLinkUrl { get { return string.IsNullOrEmpty(LinkUrl) ? "#" : LinkUrl; } }
    }

    /// <summary>One column of the kit footer.</summary>
    public class PgFooterColumn
    {
        public string Heading { get; set; }
        public string CollapseId { get; set; }
        public List<PgLinkItem> Links { get; set; }

        public PgFooterColumn()
        {
            Links = new List<PgLinkItem>();
        }

        public bool HasHeading { get { return !string.IsNullOrEmpty(Heading); } }
        public string CollapseHref { get { return "#" + (CollapseId ?? string.Empty); } }
    }

    /// <summary>One question / answer pair.</summary>
    public class PgFaqItem
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int ItemOrder { get; set; }
        public string AccentClass { get; set; }

        public PgFaqItem()
        {
            AccentClass = "border-light-blue";
        }

        public string CollapseId { get { return "pgFaq" + Id.ToString(System.Globalization.CultureInfo.InvariantCulture); } }
        public string CollapseHref { get { return "#" + CollapseId; } }
    }
}
