using System;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClFacilities : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Facilities; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (rptFacilities != null)
                {
                    rptFacilities.DataSource = Items;
                    rptFacilities.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClFacilities.Page_Load", ex);
            }
        }

        protected void rptFacilities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ClCard card = e.Item.DataItem as ClCard;
                Repeater rptBadges = e.Item.FindControl("rptBadges") as Repeater;
                if (card != null && rptBadges != null)
                {
                    rptBadges.DataSource = card.BadgesList;
                    rptBadges.DataBind();
                }
            }
        }
    }
}
