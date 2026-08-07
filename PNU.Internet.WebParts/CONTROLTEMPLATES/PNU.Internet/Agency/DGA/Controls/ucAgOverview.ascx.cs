using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// "نظرة عامة عن وكالة الجامعة" - intro paragraph plus the responsive hero image.
    /// Single-item section: only the first row of AgOverview is rendered.
    /// </summary>
    public partial class ucAgOverview : AgSectionBase
    {
        protected override string ListName { get { return AgListNames.Overview; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                AgCard card = FirstItem;
                if (card == null)
                {
                    rowOverview.Visible = false;
                    return;
                }

                // The item title IS the section heading for this single-item section.
                ltSectionTitle.Text = string.IsNullOrEmpty(SectionTitle)
                    ? card.Title
                    : AgHelper.Enc(SectionTitle);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(ltSectionTitle.Text);

                ltBody.Text = card.Description;
                ltPicture.Text = card.PictureHtml;

                rowOverview.Visible = true;
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgOverview.Bind", ex);
                rowOverview.Visible = false;
            }
        }
    }
}
