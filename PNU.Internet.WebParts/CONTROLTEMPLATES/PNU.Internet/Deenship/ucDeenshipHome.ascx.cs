using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Deenship
{
    public partial class ucDeenshipHome : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //mgCoiledArrow.Attributes["src"] = SPFactory.GetPNUresResource("coiledarrow");
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
    }
}
