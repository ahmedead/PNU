using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.MoreRecentReleases_WP
{
    [ToolboxItemAttribute(false)]
    public class MoreRecentReleases_WP : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PNU_SP.webparts/MoreRecentReleases-WP/MoreRecentReleases-WPUserControl.ascx";
        public static string _ListName = "RecentReleases";

        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("ListName"),
        Personalizable(true),
       Description("ListName")]
        public string ListName
        {
            get { return _ListName; }
            set { _ListName = value; }
        }

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
