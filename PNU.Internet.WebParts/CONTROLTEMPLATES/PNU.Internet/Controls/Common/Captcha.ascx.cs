using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Controls.Common
{
    public partial class Captcha : UserControl
    {
        public bool ValidateCaptcha()
        {
            if (Session["CaptchaCode"] == null || string.IsNullOrEmpty(txtCaptcha.Text))
                return false;

            return string.Equals(Session["CaptchaCode"].ToString(),
                   txtCaptcha.Text,
                   StringComparison.OrdinalIgnoreCase);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCaptcha.Text = string.Empty;
            }
        }
    }
}
