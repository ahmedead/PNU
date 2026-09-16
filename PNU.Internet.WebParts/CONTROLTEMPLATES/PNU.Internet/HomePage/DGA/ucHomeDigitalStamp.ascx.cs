using Microsoft.SharePoint;
using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeDigitalStamp : UserControl
    {
        public bool IsArabic
        {
            get
            {
                try
                {
                    if (SPContext.Current != null && SPContext.Current.Web != null)
                    {
                        return SPContext.Current.Web.Language == 1025;
                    }
                    int lcid = Thread.CurrentThread.CurrentUICulture.LCID;
                    if (lcid == 1025) return true;
                    if (lcid == 1033) return false;
                }
                catch { }
                return true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
