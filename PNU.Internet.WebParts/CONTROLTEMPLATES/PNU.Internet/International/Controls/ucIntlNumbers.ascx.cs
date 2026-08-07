using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlNumbers : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Numbers; } }

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

                rptNumbers.DataSource = Items;
                rptNumbers.DataBind();

                secNumbers.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlNumbers.Bind", ex);
                secNumbers.Visible = false;
            }
        }
    }
}
