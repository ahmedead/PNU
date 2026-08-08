using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSmartSuitcaseSupport : SscSectionBase
    {
        protected override string ListName { get { return SscListNames.Support; } }

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

                rptSupport.DataSource = Items;
                rptSupport.DataBind();

                secSupport.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcaseSupport.Bind", ex);
                secSupport.Visible = false;
            }
        }
    }
}
