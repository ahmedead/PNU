using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Jobs.webparts.JobsList
{
    [ToolboxItemAttribute(false)]
    public class JobsList :  Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PNU.Jobs.webparts/JobsList/JobsListUserControl.ascx";

        public static string _ListName = "JobsList";
        public static uint  _PageSize = 3;
        public static bool _Ascending = false;


        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("List Name"),
        Personalizable(true),
       Description("List Name")]
        public string ListName
        {
            get { return _ListName; }
            set { _ListName = value; }
        }

        [WebBrowsable(true),
       Category("Properties"),
        DefaultValue(""),
        WebPartStorage(Storage.Shared),
       FriendlyName("Page Size"),
        Personalizable(true),
       Description("Page Size")]
        public uint PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }


        [WebBrowsable(true),
      Category("Properties"),
       DefaultValue(""),
       WebPartStorage(Storage.Shared),
      FriendlyName("Ascending"),
       Personalizable(true),
      Description("Ascending")]
        public bool Ascending
        {
            get { return _Ascending; }
            set { Ascending = value; }
        }


        protected override void CreateChildControls()
        {
            this.ExportMode = WebPartExportMode.All;
            this.ChromeType = PartChromeType.None;
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
