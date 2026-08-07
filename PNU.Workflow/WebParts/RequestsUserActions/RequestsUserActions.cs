using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using Portal.Main.Helper;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Workflow.WebParts.RequestsUserActions
{
    [ToolboxItemAttribute(false)]
    public class RequestsUserActions : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PNU.Workflow.WebParts/RequestsUserActions/RequestsUserActionsUserControl.ascx";
        public static string _NewsSite = PortalHelper.ParentLangSite+"MediaCenter/news/";
        public static string _PageLayout = "/_catalogs/masterpage/News.aspx";
        public static string _PageLibrary = "الصفحات";

        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("News Site Name"),
        Personalizable(true),
       Description("News Site Name")]
        public string NewsSite
        {
            get { return _NewsSite; }
            set { _NewsSite = value; }
        }

        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("Pages Library"),
        Personalizable(true),
       Description("Pages Library")]
        public string PageLibrary
        {
            get { return _PageLibrary; }
            set { _PageLibrary = value; }
        }

        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("News Page Layout"),
        Personalizable(true),
       Description("News Page Layout")]
        public string PageLayout
        {
            get { return _PageLayout; }
            set { _PageLayout = value; }
        }
        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
