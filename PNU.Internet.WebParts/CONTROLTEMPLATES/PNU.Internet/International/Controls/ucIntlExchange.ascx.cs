using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlExchange : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Exchange; } }

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

                rptExchange.DataSource = Items;
                rptExchange.DataBind();

                secExchange.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlExchange.Bind", ex);
                secExchange.Visible = false;
            }
        }
    }
}
