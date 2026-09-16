using System;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Emits the kit specific CSS once per request, even when more than one kit control
    /// is placed on the same page.
    /// </summary>
    public partial class ucPgStyles : UserControl
    {
        private const string RenderedKey = "PgStylesRendered";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (phStyles == null) return;

            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx == null) return;

                if (ctx.Items.Contains(RenderedKey))
                {
                    phStyles.Visible = false;
                    return;
                }

                ctx.Items[RenderedKey] = true;
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgStyles.Page_Load", ex);
            }
        }
    }
}
