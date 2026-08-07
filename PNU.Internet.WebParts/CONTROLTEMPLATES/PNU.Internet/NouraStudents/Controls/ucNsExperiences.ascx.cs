using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// "تجارب ملهمة من طالباتنا" - one story per Bootstrap carousel slide.
    /// Slide state (active class, aria labels, indicator index) is computed here and
    /// carried on the DTO, so the markup stays declarative.
    /// </summary>
    public partial class ucNsExperiences : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.Experiences; } }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string IntroText { get; set; }

        // ---- labels used by <%# %> expressions outside the repeaters ----------------
        protected string CarouselLabel { get; private set; }
        protected string PrevLabel { get; private set; }
        protected string NextLabel { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = NsHelper.Enc(HeadingText);

                ltIntro.Text = NsHelper.Enc(string.IsNullOrEmpty(IntroText)
                    ? NsHelper.GetRes("Ns_ExperiencesIntro",
                        "قصص وتجارب تعكس أثر البيئة الجامعية في تمكين طالبات جامعة الأميرة نورة بنت عبدالرحمن.",
                        "Stories that show how the university environment empowers PNU students.")
                    : IntroText);

                CarouselLabel = NsHelper.Enc(HeadingText);
                PrevLabel = NsHelper.Enc(NsHelper.GetRes("Ns_PrevStory", "القصة السابقة", "Previous story"));
                NextLabel = NsHelper.Enc(NsHelper.GetRes("Ns_NextStory", "القصة التالية", "Next story"));

                PrepareSlides();

                rptExperiences.DataSource = Items;
                rptIndicators.DataSource = Items;

                // this.DataBind() - not rptX.DataBind() - so the <%# %> expressions on the
                // carousel wrapper and the prev/next buttons are evaluated as well.
                this.DataBind();

                secExperiences.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsExperiences.Bind", ex);
                secExperiences.Visible = false;
            }
        }

        /// <summary>
        /// Marks the first story as the active slide and builds the localized
        /// "1 من 2" / "القصة 1: سارة العنزي" labels.
        /// </summary>
        private void PrepareSlides()
        {
            int total = Items.Count;
            string of = NsHelper.GetRes("Ns_SlideOf", "من", "of");
            string story = NsHelper.GetRes("Ns_Story", "القصة", "Story");

            for (int i = 0; i < total; i++)
            {
                NsCard card = Items[i];

                card.SlideIndex = i;
                card.SlideCount = total;
                card.IsActiveSlide = i == 0;

                string position = (i + 1).ToString(CultureInfo.InvariantCulture);
                string count = total.ToString(CultureInfo.InvariantCulture);

                card.SlideAriaLabel = NsHelper.Enc(position + " " + of + " " + count);

                // card.Title is already HTML-encoded by NsSectionBase.Project.
                card.IndicatorAriaLabel = NsHelper.Enc(story + " " + position + ": ") + card.Title;
            }
        }
    }
}
