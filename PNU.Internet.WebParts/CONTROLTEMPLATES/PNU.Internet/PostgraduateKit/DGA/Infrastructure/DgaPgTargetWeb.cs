using Microsoft.SharePoint;
using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    /// <summary>
    /// Opens the web that holds the DGA Postgraduate Academic Kit lists.
    /// Lists are stored under /ar/Deanship/postgraduate so Arabic and English
    /// pages share a single source of truth.
    /// </summary>
    public static class DgaPgTargetWeb
    {
        public const string DefaultPath = "ar/Deanship/postgraduate";

        public static SPWeb Open(SPSite site)
        {
            if (site == null) return null;

            try
            {
                string siteUrl = site.ServerRelativeUrl.TrimEnd('/');
                string targetUrl = string.IsNullOrEmpty(siteUrl)
                    ? "/" + DefaultPath
                    : siteUrl + "/" + DefaultPath;

                SPWeb web = site.OpenWeb(targetUrl);
                if (web != null && web.Exists)
                {
                    return web;
                }
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgTargetWeb.Open: Failed to open " + DefaultPath + ", falling back to RootWeb", ex);
            }

            try
            {
                return site.RootWeb;
            }
            catch (Exception ex)
            {
                DgaPgLog.Write("DgaPgTargetWeb.Open: Failed to open RootWeb", ex);
                return null;
            }
        }
    }
}
