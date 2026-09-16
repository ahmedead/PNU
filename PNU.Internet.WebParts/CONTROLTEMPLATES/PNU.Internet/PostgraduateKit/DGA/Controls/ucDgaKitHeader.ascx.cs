using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaKitHeader : UserControl
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

        protected string ResolveDeanshipUrl()
        {
            return IsArabic ? "/ar/Deanship/postgraduate/Pages/home.aspx" : "/en/Deanship/postgraduate/Pages/home.aspx";
        }

        protected string ResolveLogoUrl()
        {
            return IsArabic
                ? "/Style%20Library/DGA/images/pnu-logo-ar-h.svg"
                : "/Style%20Library/DGA/images/pnu-logo-en-h.svg";
        }

        protected string ResolveLangSwitchUrl()
        {
            try
            {
                string rawUrl = HttpContext.Current?.Request?.RawUrl ?? string.Empty;
                if (IsArabic)
                {
                    if (rawUrl.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
                        return "/en/" + rawUrl.Substring(4);
                    return "/en/Deanship/postgraduate/Pages/academic-kit.aspx";
                }
                else
                {
                    if (rawUrl.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
                        return "/ar/" + rawUrl.Substring(4);
                    return "/ar/Deanship/postgraduate/Pages/academic-kit.aspx";
                }
            }
            catch
            {
                return IsArabic ? "/en/Pages/default.aspx" : "/ar/Pages/default.aspx";
            }
        }
    }
}
