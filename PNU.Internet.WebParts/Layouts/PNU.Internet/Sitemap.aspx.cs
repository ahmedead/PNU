using System;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin;

namespace PNU.Internet.WebParts.Layouts.PNU.Internet
{
    public partial class Sitemap : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string baseUrl = "https://pnu.edu.sa";
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    baseUrl = SPContext.Current.Site.Url;
                }
                else if (Request.Url != null)
                {
                    baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                }
            }
            catch { }

            string xmlContent = SiteAuditSitemapGenerator.GenerateSitemapXml(baseUrl);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.ContentType = "text/xml; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(TimeSpan.FromHours(12));

            Response.Write(xmlContent);
            Response.Flush();
            Response.End();
        }
    }
}
