
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlBeforeArrival : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.BeforeArrival; } }

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

                rptBeforeArrival.DataSource = Items;
                rptBeforeArrival.DataBind();

                secBeforeArrival.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                
                IntlLog.Write("ucIntlBeforeArrival.Bind", ex);
                secBeforeArrival.Visible = false;
            }
        }
    }
}
