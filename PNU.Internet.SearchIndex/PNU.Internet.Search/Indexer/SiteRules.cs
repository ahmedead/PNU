using System;
using Microsoft.SharePoint;

namespace PNU.Internet.SearchIndex.Indexer
{
    /// <summary>
    /// Encodes the PNU site IA rules so the crawler knows what kind
    /// of treatment each web needs.
    /// </summary>
    public static class SiteRules
    {
        public enum WebKind
        {
            Ordinary,
            HasAllItems,
            FacultiesContainer,
            FacultyChild,
            SkipEntirely
        }

        private static readonly string[] AllItemsPrefixes =
        {
            "/ar/Agencies",   "/en/Agencies",
            "/ar/Deanship",   "/en/Deanship",
            "/ar/Departments","/en/Departments",
            "/ar/Centers",    "/en/Centers"
        };

        private static readonly string[] FacultiesContainers =
        {
            "/ar/Faculties", "/en/Faculties"
        };

        public static WebKind Classify(SPWeb web)
        {
            if (web == null) return WebKind.SkipEntirely;

            string url = (web.ServerRelativeUrl ?? "").TrimEnd('/');
            string lower = url.ToLowerInvariant();

            foreach (var c in FacultiesContainers)
            {
                if (string.Equals(lower, c.ToLowerInvariant(),
                    StringComparison.OrdinalIgnoreCase))
                    return WebKind.FacultiesContainer;
            }

            foreach (var c in FacultiesContainers)
            {
                string prefix = c.ToLowerInvariant() + "/";
                if (lower.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return WebKind.FacultyChild;
            }

            foreach (var p in AllItemsPrefixes)
            {
                string lp = p.ToLowerInvariant();
                if (string.Equals(lower, lp, StringComparison.OrdinalIgnoreCase) ||
                    lower.StartsWith(lp + "/", StringComparison.OrdinalIgnoreCase))
                    return WebKind.HasAllItems;
            }

            if (lower.EndsWith("/searchindexing"))
                return WebKind.SkipEntirely;

            return WebKind.Ordinary;
        }

        public static bool HasAboutList(SPWeb web)
        {
            if (web == null) return false;
            try
            {
                foreach (SPList l in web.Lists)
                {
                    if (l.Hidden) continue;
                    string t = (l.Title ?? "").Trim();
                    if (t.StartsWith("About", StringComparison.OrdinalIgnoreCase) ||
                        t.StartsWith("عن",  StringComparison.OrdinalIgnoreCase) ||
                        t.StartsWith("نبذة",StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch { }
            return false;
        }

        public static string ResolveHomePageUrl(SPWeb web)
        {
            if (web == null) return null;
            try
            {
                string baseUrl = web.Url.TrimEnd('/');
                string[] candidates = { "Pages/home.aspx", "Pages/default.aspx",
                                        "home.aspx", "default.aspx" };

                foreach (string rel in candidates)
                {
                    string full = baseUrl + "/" + rel;
                    SPFile f = web.GetFile(full);
                    if (f != null && f.Exists) return full;
                }

                if (!string.IsNullOrEmpty(web.RootFolder.WelcomePage))
                    return baseUrl + "/" + web.RootFolder.WelcomePage;
            }
            catch { }
            return null;
        }
    }
}
