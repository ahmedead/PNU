using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace PNU.Internet.SearchIndex.Indexer
{
    /// <summary>
    /// Inspects publishing pages under a given SPWeb and returns
    /// per-page metadata: title, url, layout, user controls, and
    /// web-part property dumps.
    /// </summary>
    public static class PageInspector
    {
        public class InspectedPage
        {
            // Primary (usually the Arabic page)
            public string PageTitle { get; set; }
            public string PageURL { get; set; }
            public string PageLayout { get; set; }
            public string UserControlPath { get; set; }
            public string UserControlProperties { get; set; }
            public string WebUrl { get; set; }

            // English mirror (/ar/ -> /en/)
            public string PageTitleEn { get; set; }
            public string PageURLEn { get; set; }
            public string PageLayoutEn { get; set; }
            public string UserControlPathEn { get; set; }
            public string UserControlPropertiesEn { get; set; }
            public string WebUrlEn { get; set; }

            /// <summary>True when the /en/ counterpart actually exists.</summary>
            public bool EnExists { get; set; }
        }

        // ================================================================
        // Webs excluded from the page catalog. Matched as a case-insensitive
        // prefix against each web's server-relative URL, so '/ar/ITAdmin'
        // also excludes '/ar/ITAdmin/AnySubWeb'.
        //
        // These are admin / utility / non-content webs that would only add
        // noise to the audit.
        // ================================================================
        private static readonly string[] ExcludedWebPrefixes =
        {
            "/ar/Announcements",
            "/ar/VirtualTour",
            "/en/VirtualTour",
            "/ar/NewStudents",
            "/ar/NewsActivities",
            "/ar/ITAdmin",
            "/ar/ContentAdmin",
            "/en/NewsActivities"
        };

        /// <summary>
        /// True when the given web should be skipped entirely.
        /// </summary>
        public static bool IsWebExcluded(SPWeb web)
        {
            if (web == null) return true;
            return IsUrlExcluded(web.ServerRelativeUrl);
        }

        /// <summary>
        /// True when a server-relative URL falls under an excluded web.
        /// </summary>
        public static bool IsUrlExcluded(string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl)) return false;

            string url = serverRelativeUrl.TrimEnd('/');
            foreach (string prefix in ExcludedWebPrefixes)
            {
                string p = prefix.TrimEnd('/');
                if (url.Equals(p, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (url.StartsWith(p + "/", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public class InspectionResult
        {
            public List<InspectedPage> Pages { get; set; }
                = new List<InspectedPage>();

            /// <summary>True when the requested target URL doesn't map
            /// to an actual SPWeb.</summary>
            public bool WebNotFound { get; set; }

            /// <summary>True when the requested web is on the exclusion
            /// list and was deliberately skipped.</summary>
            public bool WebExcluded { get; set; }

            /// <summary>The web URL we actually inspected, or empty when
            /// <see cref="WebNotFound"/> is true.</summary>
            public string ResolvedWebUrl { get; set; }
        }

        /// <summary>
        /// Enumerates pages under a target web. When
        /// <paramref name="currentSiteOnly"/> is true, only the target
        /// web's Pages library is inspected; otherwise every descendant
        /// SPWeb is walked too.
        /// </summary>
        public static InspectionResult InspectPages(SPSite site,
            string webServerRelativeUrl, bool currentSiteOnly)
        {
            var result = new InspectionResult();
            if (site == null) return result;

            try
            {
                using (SPWeb web = OpenWeb(site, webServerRelativeUrl))
                {
                    if (web == null || !web.Exists)
                    {
                        result.WebNotFound = true;
                        return result;
                    }
                    if (IsWebExcluded(web))
                    {
                        result.WebExcluded = true;
                        result.ResolvedWebUrl = web.Url;
                        return result;
                    }
                    result.ResolvedWebUrl = web.Url;
                    InspectWeb(web, currentSiteOnly, result.Pages);
                }
            }
            catch (Exception ex)
            {
                Logging.SearchLogger.WriteToLog("PageInspector",
                    "InspectPages", ex.Message);
            }
            return result;
        }

        // ================================================================
        private static void InspectWeb(SPWeb web, bool currentSiteOnly,
            List<InspectedPage> results)
        {
            if (web == null) return;

            // Excluded webs are skipped entirely - including their
            // descendants, since the recursion happens at the bottom of
            // this method and we return before reaching it.
            if (IsWebExcluded(web))
            {
                Logging.SearchLogger.WriteToLog("PageInspector",
                    "InspectWeb", "Skipped excluded web: "
                    + web.ServerRelativeUrl);
                return;
            }

            try
            {
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);

                    // GetPublishingPages() by itself hides drafts and
                    // checked-out pages (it defaults to the approved
                    // version filter). Walk the underlying Pages list
                    // directly so EVERY item is captured regardless of
                    // moderation / checkout state.
                    SPList pagesList = pubWeb.PagesList;
                    if (pagesList != null)
                    {
                        var query = new SPQuery
                        {
                            ViewAttributes = "Scope=\"Recursive\"",
                            Query = ""
                        };

                        foreach (SPListItem item in pagesList.GetItems(query))
                        {
                            string pageUrl = "(unknown)";
                            try
                            {
                                PublishingPage page = PublishingPage.GetPublishingPage(item);
                                if (page == null) continue;
                                pageUrl = page.Url;

                                InspectedPage row = InspectPage(web, page);
                                if (row != null) results.Add(row);
                            }
                            catch (Exception ex)
                            {
                                Logging.SearchLogger.WriteToLog("PageInspector",
                                    "InspectPage " + pageUrl, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    // Non-publishing web - try the "SitePages" or "Pages" list
                    InspectPlainPagesLibrary(web, results);
                }
            }
            catch (Exception ex)
            {
                Logging.SearchLogger.WriteToLog("PageInspector",
                    "InspectWeb " + web.Url, ex.Message);
            }

            if (currentSiteOnly) return;

            foreach (SPWeb child in web.Webs)
            {
                try { InspectWeb(child, false, results); }
                finally { child.Dispose(); }
            }
        }

        // ----------------------------------------------------------------
        private static InspectedPage InspectPage(SPWeb web, PublishingPage page)
        {
            // page.Url is SITE-collection-relative ("ar/ContentAdmin/Pages/x.aspx");
            // ListItem.File.ServerRelativeUrl is the true server-relative
            // path we want (e.g. "/ar/ContentAdmin/Pages/x.aspx").
            string serverRel = page.ListItem != null && page.ListItem.File != null
                ? page.ListItem.File.ServerRelativeUrl
                : "";
            string absolute = !string.IsNullOrEmpty(serverRel)
                ? new Uri(new Uri(web.Site.Url), serverRel).AbsoluteUri
                : MakeAbsolute(web.Site, page.Url);

            // The URL that must be passed to GetLimitedWebPartManager is
            // ALSO server-relative (or site-collection-relative), NOT the
            // raw page.Url which loses the web's path segment for subwebs.
            string wpmUrl = !string.IsNullOrEmpty(serverRel)
                ? serverRel : page.Url;

            var row = new InspectedPage
            {
                PageTitle = page.Title,
                PageURL = absolute,
                PageLayout = page.Layout != null
                    ? page.Layout.ServerRelativeUrl
                    : "",
                WebUrl = web.Url
            };

            var ucPaths = new List<string>();
            var propsBuffer = new StringBuilder();

            try
            {
                using (SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(
                    wpmUrl, PersonalizationScope.Shared))
                {
                    foreach (WebPart wp in mgr.WebParts)
                    {
                        try
                        {
                            CollectFromWebPart(wp, ucPaths, propsBuffer);
                        }
                        catch (Exception ex)
                        {
                            propsBuffer.AppendLine("[error reading WP "
                                + SafeName(wp) + "]: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                propsBuffer.AppendLine("[error opening web-part manager]: "
                    + ex.Message);
            }

            row.UserControlPath = string.Join(Environment.NewLine, ucPaths);
            row.UserControlProperties = propsBuffer.ToString().TrimEnd();

            // ---- English mirror -----------------------------------------
            // Swap the language segment and see whether the counterpart
            // page exists. When it does, inspect it too so the catalog
            // row carries both languages side by side.
            FillEnglishMirror(web.Site, serverRel, row);

            return row;
        }

        // ================================================================
        // Resolves the /en/ counterpart of a server-relative page URL and,
        // when it exists, inspects it into the *En fields of the row.
        // If the primary page is already an /en/ page, the mirror lookup
        // is skipped (the row is its own English record).
        // ================================================================
        private static void FillEnglishMirror(SPSite site,
            string primaryServerRelUrl, InspectedPage row)
        {
            if (site == null || row == null) return;
            if (string.IsNullOrEmpty(primaryServerRelUrl)) return;

            // Already English? Then there is nothing to mirror.
            if (primaryServerRelUrl.StartsWith("/en/",
                    StringComparison.OrdinalIgnoreCase))
            {
                row.PageTitleEn = row.PageTitle;
                row.PageURLEn = row.PageURL;
                row.PageLayoutEn = row.PageLayout;
                row.UserControlPathEn = row.UserControlPath;
                row.UserControlPropertiesEn = row.UserControlProperties;
                row.WebUrlEn = row.WebUrl;
                row.EnExists = true;
                return;
            }

            string enUrl = ToEnglishUrl(primaryServerRelUrl);
            if (string.IsNullOrEmpty(enUrl)
                || enUrl.Equals(primaryServerRelUrl,
                       StringComparison.OrdinalIgnoreCase))
                return;

            // Record the expected EN URL even when the page is missing,
            // so the export shows what *should* exist.
            row.PageURLEn = new Uri(new Uri(site.Url), enUrl).AbsoluteUri;

            if (IsUrlExcluded(enUrl)) return;

            try
            {
                using (SPWeb enWeb = OpenWebForPage(site, enUrl))
                {
                    if (enWeb == null || !enWeb.Exists) return;
                    if (IsWebExcluded(enWeb)) return;

                    SPFile file = enWeb.GetFile(enUrl);
                    if (file == null || !file.Exists) return;

                    row.EnExists = true;
                    row.WebUrlEn = enWeb.Url;

                    SPListItem item = file.Item;
                    if (item != null)
                    {
                        row.PageTitleEn = Convert.ToString(
                            item["Title"] ?? file.Name);

                        try
                        {
                            PublishingPage enPage =
                                PublishingPage.GetPublishingPage(item);
                            if (enPage != null && enPage.Layout != null)
                                row.PageLayoutEn =
                                    enPage.Layout.ServerRelativeUrl;
                        }
                        catch { /* not a publishing page - layout stays empty */ }
                    }
                    else
                    {
                        row.PageTitleEn = file.Name;
                    }

                    // Web parts on the EN page
                    var ucPathsEn = new List<string>();
                    var propsEn = new StringBuilder();
                    try
                    {
                        using (SPLimitedWebPartManager mgr =
                            enWeb.GetLimitedWebPartManager(
                                enUrl, PersonalizationScope.Shared))
                        {
                            foreach (WebPart wp in mgr.WebParts)
                            {
                                try
                                {
                                    CollectFromWebPart(wp, ucPathsEn, propsEn);
                                }
                                catch (Exception ex)
                                {
                                    propsEn.AppendLine("[error reading WP "
                                        + SafeName(wp) + "]: " + ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        propsEn.AppendLine("[error opening web-part manager]: "
                            + ex.Message);
                    }

                    row.UserControlPathEn =
                        string.Join(Environment.NewLine, ucPathsEn);
                    row.UserControlPropertiesEn = propsEn.ToString().TrimEnd();
                }
            }
            catch (Exception ex)
            {
                Logging.SearchLogger.WriteToLog("PageInspector",
                    "FillEnglishMirror " + enUrl, ex.Message);
            }
        }

        /// <summary>
        /// Swaps the leading '/ar/' language segment for '/en/'.
        /// </summary>
        public static string ToEnglishUrl(string serverRelativeUrl)
        {
            if (string.IsNullOrEmpty(serverRelativeUrl)) return serverRelativeUrl;
            return System.Text.RegularExpressions.Regex.Replace(
                serverRelativeUrl, "^/ar/", "/en/",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Opens the SPWeb that actually contains the given page URL by
        /// walking up the path until a real web is found.
        /// </summary>
        private static SPWeb OpenWebForPage(SPSite site, string serverRelPageUrl)
        {
            try
            {
                string path = serverRelPageUrl;
                int lastSlash = path.LastIndexOf('/');
                if (lastSlash > 0) path = path.Substring(0, lastSlash);

                while (!string.IsNullOrEmpty(path) && path != "/")
                {
                    try
                    {
                        SPWeb w = site.OpenWeb(path, true);
                        if (w != null && w.Exists) return w;
                        if (w != null) w.Dispose();
                    }
                    catch { /* not a web at this level - walk up */ }

                    int idx = path.LastIndexOf('/');
                    if (idx <= 0) break;
                    path = path.Substring(0, idx);
                }

                return site.OpenWeb(site.ServerRelativeUrl);
            }
            catch { return null; }
        }

        private static void InspectPlainPagesLibrary(SPWeb web,
            List<InspectedPage> results)
        {
            string[] candidates = { "SitePages", "Pages" };
            foreach (var listTitle in candidates)
            {
                SPList list = web.Lists.TryGetList(listTitle);
                if (list == null) continue;

                foreach (SPListItem it in list.Items)
                {
                    try
                    {
                        string url = web.Site.MakeFullUrl(
                            web.ServerRelativeUrl.TrimEnd('/')
                            + "/" + list.RootFolder.Url.TrimEnd('/')
                            + "/" + it.Name);

                        var row = new InspectedPage
                        {
                            PageTitle = Convert.ToString(it["Title"] ?? it.Name),
                            PageURL = url,
                            PageLayout = "",
                            WebUrl = web.Url
                        };

                        var ucPaths = new List<string>();
                        var propsBuffer = new StringBuilder();
                        string pageUrl = list.RootFolder.Url.TrimEnd('/')
                            + "/" + it.Name;
                        using (var mgr = web.GetLimitedWebPartManager(pageUrl,
                            PersonalizationScope.Shared))
                        {
                            foreach (WebPart wp in mgr.WebParts)
                                CollectFromWebPart(wp, ucPaths, propsBuffer);
                        }
                        row.UserControlPath = string.Join(
                            Environment.NewLine, ucPaths);
                        row.UserControlProperties = propsBuffer.ToString().TrimEnd();

                        FillEnglishMirror(web.Site,
                            web.ServerRelativeUrl.TrimEnd('/')
                            + "/" + pageUrl.TrimStart('/'), row);

                        results.Add(row);
                    }
                    catch (Exception ex)
                    {
                        Logging.SearchLogger.WriteToLog("PageInspector",
                            "PlainPage " + it.Name, ex.Message);
                    }
                }
            }
        }

        // ================================================================
        private static void CollectFromWebPart(WebPart wp,
            List<string> ucPaths, StringBuilder propsBuffer)
        {
            if (wp == null) return;

            string ucPath = "";
            string wpName = SafeName(wp);
            string wpType = wp.GetType().FullName;

            // Detect .ascx-hosted parts. Two safe cases in SP 2019 on-prem:
            //  a) SharePoint wraps any control that isn't a WebPart in a
            //     GenericWebPart. Its ChildControl is the real
            //     UserControl - AppRelativeVirtualPath points at .ascx.
            //  b) Custom "SPUserControl" style parts (PNU's own
            //     ControlLoaderWebPart included) store the .ascx path in
            //     a public string property. We probe common names.
            //
            // We NEVER touch wp.Controls / wp.HasControls() - accessing
            // those triggers CreateChildControls on the target WP, which
            // dies when Page is null (as it is under
            // SPLimitedWebPartManager). Persisted properties are safer.
            //
            // Everything else falls through and we record the WP's type.
            try
            {
                // (a) Adding an .ascx as a "web part" wraps it in a
                //     GenericWebPart whose ChildControl IS the UserControl.
                var generic = wp as System.Web.UI.WebControls.WebParts.GenericWebPart;
                if (generic != null)
                {
                    var child = generic.ChildControl as System.Web.UI.UserControl;
                    if (child != null
                        && !string.IsNullOrEmpty(child.AppRelativeVirtualPath))
                    {
                        ucPath = child.AppRelativeVirtualPath;
                    }
                    // GenericWebPart type name is useless - report the child
                    if (generic.ChildControl != null)
                        wpType = generic.ChildControl.GetType().FullName;
                }

                // (b) SPUserControl / third-party wrappers expose the .ascx
                //     path as a string property. PNU's own ControlLoaderWebPart
                //     uses "UserControlPath" - check that first.
                if (string.IsNullOrEmpty(ucPath))
                    ucPath = ReadStringProperty(wp,
                        "UserControlPath", "UserControlSrc",
                        "Path", "ControlPath", "ControlSrc", "Src");

                // (c) NOTE: we deliberately do NOT walk wp.Controls here.
                //     Accessing HasControls() or Controls forces the WP
                //     to run CreateChildControls(), which many custom
                //     web parts (including PNU's ControlLoaderWebPart)
                //     do not survive when they are instantiated by
                //     SPLimitedWebPartManager without a live page
                //     context. Persisted property probing (steps a + b)
                //     is enough for an audit.
            }
            catch
            {
                // Non-fatal - fall back to type name below.
            }

            // Fallback: record the WP's full type name so the auditor still
            // sees which XSLT/CQWP/custom part is on the page.
            if (string.IsNullOrEmpty(ucPath))
                ucPath = wpType;

            if (!ucPaths.Contains(ucPath)) ucPaths.Add(ucPath);

            // Dump the WP's public properties, filtering out framework noise
            propsBuffer.AppendLine("### " + wpName + "  [" + wpType + "]");
            DumpProperties(wp, propsBuffer);
            propsBuffer.AppendLine();
        }

        // ================================================================
        // Property dump with an inherited-framework filter so the output
        // stays readable.
        // ================================================================
        // Skip only truly noisy internals (control-tree pointers, styling
        // objects that dump multi-line class names). Everything else -
        // including Title, ZoneID, PartOrder, IsVisible, Dir, Permissions,
        // Enabled, Caption, ClientName, etc. - is preserved in the output.
        private static readonly HashSet<string> SkipProperties =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ID","UniqueID","ClientID","ClientIDMode",
            "NamingContainer","Parent","Site","TemplateControl","Page",
            "TemplateSourceDirectory","AppRelativeTemplateSourceDirectory",
            "AppRelativeVirtualPath","SkinID","ViewStateMode",
            "BindingContainer","EnableTheming","EnableViewState",
            "SkinFilePath","Attributes","ControlStyle","ControlStyleCreated",
            "Font","Style","WebBrowsableObject","WebPartManager","Zone",
            "Verbs","AppDomain","MissingAssembly","AllowsDefinition"
        };

        private static void DumpProperties(object obj, StringBuilder sb)
        {
            if (obj == null) return;

            PropertyInfo[] props = obj.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                if (!p.CanRead) continue;
                if (p.GetIndexParameters().Length > 0) continue;
                if (SkipProperties.Contains(p.Name)) continue;

                string display = p.Name;
                string value;
                try
                {
                    object raw = p.GetValue(obj, null);
                    value = raw == null ? "" : Convert.ToString(raw);
                }
                catch (Exception ex)
                {
                    value = "[error: " + ex.Message + "]";
                }

                // Trim gigantic values so the export stays sane
                if (value != null && value.Length > 800)
                    value = value.Substring(0, 800) + "…";

                sb.Append("  ").Append(display).Append(" = ")
                  .AppendLine(value ?? "");
            }
        }

        // ================================================================
        /// <summary>
        /// Opens the SPWeb at the given URL. Unlike SPSite.OpenWeb, which
        /// silently falls back to the closest matching parent web when
        /// the path doesn't map to an actual web, this method returns
        /// null when the requested path isn't a real SPWeb. That's what
        /// we want for an auditor: better an empty grid than pages from
        /// the wrong web.
        /// </summary>
        private static SPWeb OpenWeb(SPSite site, string serverRelativeUrl)
        {
            try
            {
                // Empty → site root
                if (string.IsNullOrEmpty(serverRelativeUrl))
                    return site.OpenWeb(site.ServerRelativeUrl);

                // Normalise: absolute URLs → server-relative path
                string path = serverRelativeUrl;
                if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    path = new Uri(path).AbsolutePath;

                if (!path.StartsWith("/")) path = "/" + path;
                path = path.TrimEnd('/');
                if (path.Length == 0) path = "/";

                // OpenWeb(url, requireExactUrl=true) throws when the path
                // isn't an actual SPWeb - catch and return null.
                try
                {
                    SPWeb w = site.OpenWeb(path, true);
                    if (w != null && w.Exists) return w;
                    if (w != null) w.Dispose();
                }
                catch (Microsoft.SharePoint.SPException)
                {
                    // Not a real SPWeb - fall through and return null.
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Same - path doesn't map to a web.
                }

                Logging.SearchLogger.WriteToLog("PageInspector",
                    "OpenWeb", "No SPWeb found at '" + path
                    + "'. The path may be a folder inside the root web's " +
                    "Pages library, not a separate subsite.");
                return null;
            }
            catch (Exception ex)
            {
                Logging.SearchLogger.WriteToLog("PageInspector",
                    "OpenWeb " + serverRelativeUrl, ex.Message);
                return null;
            }
        }

        private static string MakeAbsolute(SPSite site, string urlOrRelative)
        {
            if (string.IsNullOrEmpty(urlOrRelative)) return "";
            if (urlOrRelative.StartsWith("http://",
                    StringComparison.OrdinalIgnoreCase) ||
                urlOrRelative.StartsWith("https://",
                    StringComparison.OrdinalIgnoreCase))
                return urlOrRelative;
            string baseUrl = site.Url.TrimEnd('/');
            return baseUrl + (urlOrRelative.StartsWith("/")
                ? urlOrRelative : "/" + urlOrRelative);
        }

        private static string ReadStringProperty(object obj, params string[] names)
        {
            if (obj == null || names == null) return "";
            var type = obj.GetType();
            foreach (string name in names)
            {
                try
                {
                    var pi = type.GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public);
                    if (pi == null || !pi.CanRead) continue;
                    if (pi.GetIndexParameters().Length > 0) continue;
                    object val = pi.GetValue(obj, null);
                    string s = val as string;
                    if (!string.IsNullOrEmpty(s)) return s;
                }
                catch { /* try next */ }
            }
            return "";
        }

        private static string SafeName(WebPart wp)
        {
            try
            {
                if (!string.IsNullOrEmpty(wp.Title)) return wp.Title;
                if (!string.IsNullOrEmpty(wp.DisplayTitle)) return wp.DisplayTitle;
            }
            catch { }
            return wp.GetType().Name;
        }
    }
}
