using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public class DgaPgLinkItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string IconClass { get; set; }
    }

    public class DgaPgLinkModel : DgaPgLinkItem
    {
    }

    public class DgaPgSectionModel
    {
        public string SectionKey { get; set; }
        public string Phase { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string LeadText { get; set; }
        public string BadgeText { get; set; }
        public string IconClass { get; set; }

        /// <summary>Small footnote rendered under the section (v4 "small text-muted" line).</summary>
        public string NoteText { get; set; }

        public int ItemOrder { get; set; }
        public bool IsVisible { get; set; }
    }

    public class DgaPgHeaderModel
    {
        public string Phase { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<DgaPgLinkItem> Breadcrumbs { get; set; }

        public DgaPgHeaderModel()
        {
            Breadcrumbs = new List<DgaPgLinkItem>();
        }
    }

    public class DgaPgIntroModel
    {
        public string Phase { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string LeadText { get; set; }
        public List<string> Paragraphs { get; set; }
        public List<string> Badges { get; set; }
        public string ReturnUrl { get; set; }
        public string ReturnText { get; set; }
        public string IconClass { get; set; }

        public DgaPgIntroModel()
        {
            Paragraphs = new List<string>();
            Badges = new List<string>();
        }
    }

    public class DgaPgHubCardModel
    {
        public string Phase { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BadgeText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
    }

    public class DgaPgJourneyStepModel
    {
        public string Phase { get; set; }
        public int StepNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
    }

    public class DgaPgResourceCardModel
    {
        public string Phase { get; set; }
        public string GroupKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public string BadgeText { get; set; }
        public string IconClass { get; set; }
        public List<string> Bullets { get; set; }
        public int ItemOrder { get; set; }

        public DgaPgResourceCardModel()
        {
            Bullets = new List<string>();
        }
    }

    public class DgaPgContentBlockModel
    {
        public string Phase { get; set; }
        public string BlockKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Paragraphs { get; set; }
        public List<string> Bullets { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public string IconClass { get; set; }

        /// <summary>Optional pill shown next to the accordion title (v4 kit-procedure-badge).</summary>
        public string BadgeText { get; set; }

        /// <summary>
        /// Headed sub-blocks: the graduate-identity traits (heading = trait, body = its tags)
        /// and the financial-regulations cases (heading = case, body = its rule).
        /// </summary>
        public List<DgaPgArticleBlock> Groups { get; set; }

        /// <summary>Closing note rendered under the block (v4 kit-financial-regulations-note).</summary>
        public string NoteText { get; set; }

        /// <summary>Optional illustration URL (v4 kit-pub-badge-icon).</summary>
        public string IconUrl { get; set; }

        public int ItemOrder { get; set; }

        public DgaPgContentBlockModel()
        {
            Paragraphs = new List<string>();
            Bullets = new List<string>();
            Groups = new List<DgaPgArticleBlock>();
        }
    }

    public class DgaPgProcedureModel
    {
        public int Id { get; set; }
        public string Phase { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public List<string> Requirements { get; set; }
        public List<DgaPgArticleBlock> Articles { get; set; }
        public List<DgaPgLinkItem> Forms { get; set; }
        public string FlowText { get; set; }
        public string DirectServiceUrl { get; set; }

        /// <summary>Optional illustration shown in the accordion button (v4 kit-procedure-icon).</summary>
        public string IconUrl { get; set; }

        /// <summary>Optional pill shown next to the accordion title (v4 kit-procedure-badge).</summary>
        public string BadgeText { get; set; }

        /// <summary>
        /// Splits the enrolled procedures into the design's two groups:
        /// "College" (3.1, college-initiated) and "Student" (3.2, student-initiated).
        /// </summary>
        public string SubGroupKey { get; set; }

        public int ItemOrder { get; set; }

        public DgaPgProcedureModel()
        {
            Requirements = new List<string>();
            Articles = new List<DgaPgArticleBlock>();
            Forms = new List<DgaPgLinkItem>();
        }
    }

    public class DgaPgArticleBlock
    {
        public string Heading { get; set; }
        public string Body { get; set; }
    }

    public class DgaPgFaqItemModel
    {
        public int Id { get; set; }
        public string Phase { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Category { get; set; }
        public int ItemOrder { get; set; }
    }

    public class DgaPgDiscountModel
    {
        public string CategoryTitle { get; set; }
        public string Percentage { get; set; }
        public List<string> Conditions { get; set; }
        public int ItemOrder { get; set; }

        public DgaPgDiscountModel()
        {
            Conditions = new List<string>();
        }
    }

    public class DgaPgServiceModel
    {
        public string Phase { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
    }

    public class DgaPgDiscountRowModel
    {
        public string Category { get; set; }
        public string Percentage { get; set; }
    }

    public class DgaPgServiceGroupModel
    {
        public string GroupName { get; set; }
        public List<DgaPgServiceModel> Services { get; set; } = new List<DgaPgServiceModel>();
    }

    public class DgaPgQuickLinkModel
    {
        public string Phase { get; set; }
        public string SectionKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
    }
}
