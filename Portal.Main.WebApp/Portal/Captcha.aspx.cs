using System;
using System.Drawing.Imaging;
using System.Web.UI;
using XtractPro.Multimedia;

namespace Portal.Main.WebApp
{
    public partial class Captcha : Page
    {
        public const string CAPTCHA_ID = "__CaptchaImage__";

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateCaptchaImage();
            GetCaptchaImage();
        }

        /// <summary>
        /// Renders IMG SRC content for the client-side CAPTCHA image
        /// </summary>
        private void GetCaptchaImage()
        {
            CaptchaImage image = Session[CAPTCHA_ID] as CaptchaImage;
            image.CharacterSet = "1234567890";

            //Debug.Assert(image != null);
            Response.Clear();
            Response.ContentType = "image/jpeg";
            image.Image.Save(Response.OutputStream, ImageFormat.Jpeg);
        }

        // Generate a new CaptchaImage instance, with custom values
        private void CreateCaptchaImage()
        {
            CaptchaImage image = new CaptchaImage();
            Session[CAPTCHA_ID] = image;

            // customize CaptchaImage here
            image.NoiseFactor = 45;
            image.LinesFactor = 34;
            image.Length = 6;
            image.CharacterSet = "1234567890";
        }
    }
}