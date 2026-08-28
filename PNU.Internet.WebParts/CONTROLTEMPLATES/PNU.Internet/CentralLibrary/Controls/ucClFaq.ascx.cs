using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClFaq : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Faq; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (rptFaq != null)
                {
                    rptFaq.DataSource = Items;
                    rptFaq.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClFaq.Page_Load", ex);
            }
        }
    }
}
