using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    public partial class ucTwEntities : TwSectionBase
    {
        protected override string ListName { get { return TwListNames.Entities; } }
        protected override string SectionKey { get { return TwListSchema.KeyEntities; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = HeadingText;
                ltSubtitle.Text = SubtitleText;
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptEntities.DataSource = Items;
                rptEntities.DataBind();

                secEntities.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwEntities.Bind", ex);
                secEntities.Visible = false;
            }
        }
    }
}
