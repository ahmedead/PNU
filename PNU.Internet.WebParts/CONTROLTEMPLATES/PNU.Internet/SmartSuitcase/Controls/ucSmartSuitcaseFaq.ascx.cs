using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSmartSuitcaseFaq : SscSectionBase
    {
        protected override string ListName { get { return SscListNames.Faq; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = SscHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptFaq.DataSource = Items;
                rptFaq.DataBind();

                secFaq.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcaseFaq.Bind", ex);
                secFaq.Visible = false;
            }
        }
    }
}
