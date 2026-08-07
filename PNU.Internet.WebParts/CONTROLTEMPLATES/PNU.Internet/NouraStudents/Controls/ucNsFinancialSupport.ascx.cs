using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsFinancialSupport : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.FinancialSupport; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = NsHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptFinancial.DataSource = Items;
                rptFinancial.DataBind();

                secFinancialSupport.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsFinancialSupport.Bind", ex);
                secFinancialSupport.Visible = false;
            }
        }
    }
}
