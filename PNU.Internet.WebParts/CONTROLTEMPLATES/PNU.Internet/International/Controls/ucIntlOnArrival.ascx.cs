using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlOnArrival : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.OnArrival; } }

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

                rptOnArrival.DataSource = Items;
                rptOnArrival.DataBind();

                secOnArrival.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlOnArrival.Bind", ex);
                secOnArrival.Visible = false;
            }
        }
    }
}
