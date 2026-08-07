using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlOffice : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Office; } }

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

                rptOffice.DataSource = Items;
                rptOffice.DataBind();

                secOffice.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlOffice.Bind", ex);
                secOffice.Visible = false;
            }
        }
    }
}
