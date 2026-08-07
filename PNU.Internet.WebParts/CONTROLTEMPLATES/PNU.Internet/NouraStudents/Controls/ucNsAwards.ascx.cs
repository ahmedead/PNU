using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsAwards : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.Awards; } }

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

                rptAwards.DataSource = Items;
                rptAwards.DataBind();

                secAwards.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsAwards.Bind", ex);
                secAwards.Visible = false;
            }
        }
    }
}
