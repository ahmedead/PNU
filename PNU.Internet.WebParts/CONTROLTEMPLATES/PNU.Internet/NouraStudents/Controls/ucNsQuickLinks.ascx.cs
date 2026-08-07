using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsQuickLinks : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.QuickLinks; } }

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

                rptQuickLinks.DataSource = Items;
                rptQuickLinks.DataBind();

                secQuickLinks.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsQuickLinks.Bind", ex);
                secQuickLinks.Visible = false;
            }
        }
    }
}
