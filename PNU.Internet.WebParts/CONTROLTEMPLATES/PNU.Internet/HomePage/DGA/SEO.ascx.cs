using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class SEO : UserControl
    {
        public string CanonicalUrl { get; set; }
        public string ArabicUrl { get; set; }
        public string EnglishUrl { get; set; }
        public string DefaultUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Hide SharePoint's built-in SeoCanonicalLink (to prevent :443 port duplication)
                if (Page != null && Page.Header != null)
                {
                    HideCanonicalControls(Page.Header);
                }

                // 2. Initialize and render clean Canonical and Hreflang tags
                InitializeHreflangAndCanonicalUrls();
                RenderSeoTags();
            }
            catch (Exception)
            {
                EnsureFallbackUrls();
                RenderSeoTags();
            }
        }

        private void InitializeHreflangAndCanonicalUrls()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.Url == null)
            {
                EnsureFallbackUrls();
                return;
            }

            HttpRequest request = HttpContext.Current.Request;
            Uri requestUri = request.Url;

            // 1. Determine Standard Canonical Base Authority (HTTPS + Official Portal Domain)
            string standardAuthority = GetStandardPortalAuthority(requestUri);

            // 2. Clean Absolute Path
            string cleanPath = requestUri.AbsolutePath;

            if (cleanPath.Equals("/", StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = "/ar/Pages/home.aspx";
            }

            // 3. Handle essential query parameters for dynamic pages (e.g. MediaDetails.aspx?Id=...)
            string queryString = string.Empty;
            if (cleanPath.EndsWith("MediaDetails.aspx", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(request.QueryString["Id"]))
            {
                queryString = "?Id=" + HttpUtility.UrlEncode(request.QueryString["Id"]);
            }

            // 4. Set Unified Canonical URL
            CanonicalUrl = standardAuthority + cleanPath + queryString;

            string lowerPath = cleanPath.ToLowerInvariant();

            // 5. Generate Alternate Language & Default URLs
            if (lowerPath.Contains("/ar-sa/"))
            {
                ArabicUrl = standardAuthority + cleanPath + queryString;
                EnglishUrl = standardAuthority + Regex.Replace(cleanPath, "/ar-sa/", "/en-us/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.Contains("/en-us/"))
            {
                EnglishUrl = standardAuthority + cleanPath + queryString;
                ArabicUrl = standardAuthority + Regex.Replace(cleanPath, "/en-us/", "/ar-sa/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.Contains("/ar/"))
            {
                ArabicUrl = standardAuthority + cleanPath + queryString;
                EnglishUrl = standardAuthority + Regex.Replace(cleanPath, "/ar/", "/en/", RegexOptions.IgnoreCase) + queryString;
            }
            else if (lowerPath.Contains("/en/"))
            {
                EnglishUrl = standardAuthority + cleanPath + queryString;
                ArabicUrl = standardAuthority + Regex.Replace(cleanPath, "/en/", "/ar/", RegexOptions.IgnoreCase) + queryString;
            }
            else
            {
                ArabicUrl = standardAuthority + "/ar/Pages/home.aspx";
                EnglishUrl = standardAuthority + "/en/Pages/home.aspx";
            }

            DefaultUrl = ArabicUrl;
        }

        private string GetStandardPortalAuthority(Uri requestUri)
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null && !string.IsNullOrEmpty(SPContext.Current.Site.Url))
                {
                    Uri spSiteUri = new Uri(SPContext.Current.Site.Url);
                    string scheme = "https";
                    return $"{scheme}://{spSiteUri.Authority}".TrimEnd('/');
                }
            }
            catch { }

            string host = requestUri.Authority;
            return $"https://{host}".TrimEnd('/');
        }

        private void HideCanonicalControls(Control root)
        {
            if (root == null) return;
            if (root.GetType().Name.IndexOf("Canonical", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                root.Visible = false;
                return;
            }
            foreach (Control child in root.Controls)
            {
                HideCanonicalControls(child);
            }
        }

        private void EnsureFallbackUrls()
        {
            string baseUrl = "https://pnu.edu.sa";
            CanonicalUrl = baseUrl + "/ar/Pages/home.aspx";
            ArabicUrl = baseUrl + "/ar/Pages/home.aspx";
            EnglishUrl = baseUrl + "/en/Pages/home.aspx";
            DefaultUrl = ArabicUrl;
        }

        private void RenderSeoTags()
        {
            if (litSeoTags != null)
            {
                litSeoTags.Text = string.Format(
                    "<!-- Canonical Tag (Unified Official Domain) -->\r\n" +
                    "<link rel=\"canonical\" href=\"{0}\" />\r\n" +
                    "<!-- Hreflang Tags for Multi-Language SEO & Compliance (ar-sa, en-us, x-default) -->\r\n" +
                    "<link rel=\"alternate\" hreflang=\"ar-sa\" href=\"{1}\" />\r\n" +
                    "<link rel=\"alternate\" hreflang=\"en-us\" href=\"{2}\" />\r\n" +
                    "<link rel=\"alternate\" hreflang=\"x-default\" href=\"{3}\" />\r\n",
                    CanonicalUrl,
                    ArabicUrl,
                    EnglishUrl,
                    DefaultUrl
                );
            }
        }
    }
}
