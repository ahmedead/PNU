using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlFeatures : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Features; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = IntlHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptFeatures.DataSource = Items;
                rptFeatures.DataBind();

                secFeatures.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlFeatures.Bind", ex);
                secFeatures.Visible = false;
            }
        }
    }
}
