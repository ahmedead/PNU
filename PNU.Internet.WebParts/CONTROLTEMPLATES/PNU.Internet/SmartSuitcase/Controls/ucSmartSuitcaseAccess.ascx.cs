using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSmartSuitcaseAccess : SscSectionBase
    {
        protected override string ListName { get { return SscListNames.Access; } }

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

                rptAccess.DataSource = Items;
                rptAccess.DataBind();

                secAccess.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcaseAccess.Bind", ex);
                secAccess.Visible = false;
            }
        }
    }
}
