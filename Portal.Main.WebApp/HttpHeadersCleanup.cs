using System;
using System.Web;

namespace Portal.Main.WebApp
{
    public class HttpHeadersCleanup : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += PreSendRequestHeaders;
        }
        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
			HttpContext.Current.Response.Headers.Remove("Server");
			HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
			HttpContext.Current.Response.Headers.Remove("X-SharePointHealthScore");
			HttpContext.Current.Response.Headers.Remove("SPRequestGuid");
		}
        public void Dispose()
        {
        }
    }
}