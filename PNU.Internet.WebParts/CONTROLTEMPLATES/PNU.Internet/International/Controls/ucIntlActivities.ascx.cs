
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlActivities : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Activities; } }

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

                rptActivities.DataSource = Items;
                rptActivities.DataBind();

                secActivities.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                IntlLog.Write("ucIntlActivities.Bind", ex);
                secActivities.Visible = false;
            }
        }
    }

}
