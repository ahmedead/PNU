using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsNumbers : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.Numbers; } }

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

                rptNumbers.DataSource = Items;
                rptNumbers.DataBind();

                secNumbers.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsNumbers.Bind", ex);
                secNumbers.Visible = false;
            }
        }
    }
}
