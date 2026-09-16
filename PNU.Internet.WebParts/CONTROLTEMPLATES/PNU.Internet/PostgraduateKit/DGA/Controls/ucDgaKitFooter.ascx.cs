using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaKitFooter : UserControl
    {
        public bool IsArabic => DgaPgHelper.IsArabic;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string GetResource(string key, string ar, string en)
        {
            return DgaPgHelper.Pick(ar, en);
        }

        protected string ResolveHomeUrl()
        {
            return IsArabic ? "/ar/Pages/default.aspx" : "/en/Pages/default.aspx";
        }

        protected string ResolveLogoUrl()
        {
            return IsArabic
                ? "/Style%20Library/DGA/images/pnu-logo-ar-h.svg"
                : "/Style%20Library/DGA/images/pnu-logo-en-h.svg";
        }

        protected new string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl)) return "#";
            if (!IsArabic && relativeUrl.StartsWith("/ar/"))
            {
                relativeUrl = "/en/" + relativeUrl.Substring(4);
            }
            return DgaPgHelper.ResolveUrl(relativeUrl);
        }
    }
}
