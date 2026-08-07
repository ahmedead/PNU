
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlAdmission : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Admission; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = IntlHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptAdmission.DataSource = Items;
                rptAdmission.DataBind();

                secAdmission.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                
                IntlLog.Write("ucIntlAdmission.Bind", ex);
                secAdmission.Visible = false;
            }
        }
    }
}
