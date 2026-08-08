using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSmartSuitcaseServices : SscSectionBase
    {
        protected override string ListName { get { return SscListNames.Services; } }

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

                rptServices.DataSource = Items;
                rptServices.DataBind();

                secServices.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcaseServices.Bind", ex);
                secServices.Visible = false;
            }
        }
    }
}
