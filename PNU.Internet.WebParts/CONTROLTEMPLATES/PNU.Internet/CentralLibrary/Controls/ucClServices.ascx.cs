using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClServices : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Services; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (rptServices != null)
                {
                    rptServices.DataSource = Items;
                    rptServices.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClServices.Page_Load", ex);
            }
        }
    }
}
