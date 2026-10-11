using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet.ContentManagement
{
    public partial class PageContentEditor : LayoutsPageBase
    {
        /// <summary>
        /// Without this the page inherits the LayoutsPageBase default, which any reader
        /// satisfies. The control edits and checks out publishing pages, so require at
        /// least item-edit rights on the web the tool is opened from. The control also
        /// re-checks EditListItems on each target page, which may live in another web.
        /// </summary>
        protected override SPBasePermissions RightsRequired
        {
            get { return SPBasePermissions.EditListItems; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
