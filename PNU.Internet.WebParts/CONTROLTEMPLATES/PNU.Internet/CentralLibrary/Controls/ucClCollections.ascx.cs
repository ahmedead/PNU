using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClCollections : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Collections; }
        }

        protected string LocationLabel
        {
            get { return ClHelper.GetRes("ClLocation_Label", "الموقع", "Location"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (rptCollections != null)
                {
                    rptCollections.DataSource = Items;
                    rptCollections.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClCollections.Page_Load", ex);
            }
        }
    }
}
