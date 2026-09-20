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
            HttpContext ctx = HttpContext.Current;
            if (ctx == null || ctx.Response == null) return;

            ctx.Response.Headers.Remove("Server");
            ctx.Response.Headers.Remove("X-AspNet-Version");
            ctx.Response.Headers.Remove("X-SharePointHealthScore");
            ctx.Response.Headers.Remove("SPRequestGuid");

            CollapseDuplicateCsp(ctx.Response);
        }

        /// <summary>
        /// دمج ترويسات Content-Security-Policy المكررة في ترويسة واحدة.
        ///
        /// المشكلة المرصودة حياً (2026-09-20): الاستجابة تحمل ترويستَي CSP:
        ///     frame-ancestors 'self' teams.microsoft.com ... onedrive.live.com;
        ///     frame-ancestors 'self'
        /// وعند تعدد ترويسات CSP يطبّق المتصفح "تقاطعها" أي الأشد.
        /// النتيجة الفعلية 'self' فقط، فقائمة السماح لـ Teams/Office معطّلة صامتة.
        ///
        /// نُبقي الترويسة الأطول (الأكثر تفصيلاً) ونحذف الباقي.
        /// </summary>
        private static void CollapseDuplicateCsp(HttpResponse response)
        {
            try
            {
                const string HeaderName = "Content-Security-Policy";
                string[] values = response.Headers.GetValues(HeaderName);
                if (values == null || values.Length < 2) return;

                string winner = values[0];
                for (int i = 1; i < values.Length; i++)
                {
                    if (!string.IsNullOrEmpty(values[i]) && values[i].Length > winner.Length)
                        winner = values[i];
                }

                response.Headers.Remove(HeaderName);
                response.Headers.Add(HeaderName, winner);
            }
            catch
            {
                // لا نُسقط الاستجابة بسبب ترويسة
            }
        }
        public void Dispose()
        {
        }
    }
}