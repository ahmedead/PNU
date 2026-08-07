using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlDocuments : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Documents; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                // Button label fallback when the editor left ButtonText empty.
                foreach (IntlCard card in Items)
                {
                    if (string.IsNullOrEmpty(card.ButtonText))
                        card.ButtonText = IntlHelper.GetRes("Intl_Open", "فتح", "Open");
                }

                ltSectionTitle.Text = IntlHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptDocuments.DataSource = Items;
                rptDocuments.DataBind();

                secDocuments.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlDocuments.Bind", ex);
                secDocuments.Visible = false;
            }
        }
    }
}
