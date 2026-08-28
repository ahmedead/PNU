using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClAwards : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Awards; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (rptAwards != null)
                {
                    rptAwards.DataSource = Items;
                    rptAwards.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAwards.Page_Load", ex);
            }
        }
    }
}
