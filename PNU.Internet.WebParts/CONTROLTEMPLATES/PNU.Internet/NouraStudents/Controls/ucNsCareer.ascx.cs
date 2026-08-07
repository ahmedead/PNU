using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsCareer : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.Career; } }

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

                rptCareer.DataSource = Items;
                rptCareer.DataBind();

                secCareer.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsCareer.Bind", ex);
                secCareer.Visible = false;
            }
        }
    }
}
