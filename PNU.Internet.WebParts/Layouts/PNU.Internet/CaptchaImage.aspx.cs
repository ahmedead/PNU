using Microsoft.SharePoint.WebControls;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet
{
    public partial class CaptchaImage : UnsecuredLayoutsPageBase
    {

        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string captchaCode = GenerateRandomCode();
            Session["CaptchaCode"] = captchaCode;

            byte[] imageBytes = GenerateCaptchaImage(captchaCode);
            Response.ContentType = "image/png";
            Response.BinaryWrite(imageBytes);
            Response.End();
        }

        private string GenerateRandomCode()
        {
            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            var result = new char[6];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private byte[] GenerateCaptchaImage(string captchaCode)
        {
            using (Bitmap bitmap = new Bitmap(180, 50))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                // Add noise
                var random = new Random();
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(0, 180);
                    int y = random.Next(0, 50);
                    bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                // Add text
                Font font = new Font("Arial", 25, FontStyle.Bold | FontStyle.Italic);
                graphics.DrawString(captchaCode, font, Brushes.Black, new PointF(10, 10));

                // Save to memory stream
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}
