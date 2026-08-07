using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet
{
    public partial class UserControl1 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.ToString();
            Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"test", "test test test");
            

        }
    }
}
