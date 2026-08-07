using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// "كلمة الوكيلة" - quote card. Single-item section: only the first row of
    /// AgDeputyWord is rendered.
    /// </summary>
    public partial class ucAgDeputyWord : AgSectionBase
    {
        private const string DefaultIcon = "hgi-quote-down";

        protected override string ListName { get { return AgListNames.DeputyWord; } }

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
                if (card == null || !card.HasDescription)
                {
                    secDeputyWord.Visible = false;
                    return;
                }

                string icon = AgHelper.Icon(card.IconClass, DefaultIcon);
                ltIcon.Text = "<i class=\"hgi hgi-stroke " + icon + " fs-4\" aria-hidden=\"true\"></i>";

                // The item title IS the section heading for this single-item section.
                ltSectionTitle.Text = string.IsNullOrEmpty(SectionTitle)
                    ? card.Title
                    : AgHelper.Enc(SectionTitle);

                ltBody.Text = card.Description;
                ltRole.Text = card.RoleHtml;
                ltOrg.Text = card.SubTitleHtml;

                secDeputyWord.Visible = true;
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgDeputyWord.Bind", ex);
                secDeputyWord.Visible = false;
            }
        }
    }
}
