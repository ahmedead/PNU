using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>A parsed body block: either a paragraph or a bulleted/numbered list.</summary>
    public class BodyBlock
    {
        public bool IsList { get; set; }
        public bool IsOrdered { get; set; }
        public bool IsParagraph { get { return !IsList; } }

        /// <summary>Raw paragraph text (plain, as the author typed it).</summary>
        public string Text { get; set; }
        /// <summary>Paragraph, HTML-encoded then auto-linkified. Bind this in markup.</summary>
        public string Html { get; set; }

        /// <summary>Raw list items.</summary>
        public List<string> Items { get; set; }
        /// <summary>List items, HTML-encoded then auto-linkified. Bind this in markup.</summary>
        public List<string> ItemsHtml { get; set; }
    }

    /// <summary>
    /// Shared provisioning + display helpers for the faculty section controls.
    /// Kept as one small static class so each section stays self-contained.
    /// </summary>
    public static class FacultyProvisioningHelper
    {
        /// <summary>Break inheritance (once) and grant anonymous read on the list.</summary>
        public static void GrantAnonymousRead(SPList list)
        {
            try
            {
                if (!list.HasUniqueRoleAssignments)
                    list.BreakRoleInheritance(true, false);

                list.AnonymousPermMask64 = SPBasePermissions.ViewListItems
                                         | SPBasePermissions.ViewVersions
                                         | SPBasePermissions.ViewPages
                                         | SPBasePermissions.Open
                                         | SPBasePermissions.UseClientIntegration;
                list.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "",
                    "FacultyProvisioningHelper - GrantAnonymousRead", ex.Message);
            }
        }

        private static readonly Regex EmailRe =
            new Regex(@"[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}", RegexOptions.Compiled);
        // Standalone runs of 7-12 digits (room numbers like 0.100.06 are not matched).
        private static readonly Regex PhoneRe =
            new Regex(@"(?<![\d.])\d{7,12}(?![\d.])", RegexOptions.Compiled);

        /// <summary>
        /// HTML-encode plain author text, then turn emails into mailto: links and
        /// long digit runs into tel: links, so التواصل blocks render like the design.
        /// </summary>
        public static string ToHtml(string plain)
        {
            if (string.IsNullOrEmpty(plain)) return string.Empty;
            string s = HttpUtility.HtmlEncode(plain);
            s = EmailRe.Replace(s, delegate(Match m)
            {
                return "<a href=\"mailto:" + m.Value + "\" dir=\"ltr\">" + m.Value + "</a>";
            });
            s = PhoneRe.Replace(s, delegate(Match m)
            {
                return "<a href=\"tel:" + m.Value + "\" dir=\"ltr\">" + m.Value + "</a>";
            });
            return s;
        }

        /// <summary>
        /// Parse a PLAIN-TEXT body (typed by the content team) into display blocks.
        /// Rules (no HTML needed from the author):
        ///   - Blank line separates blocks.
        ///   - A block whose lines all start with "- " renders as a bulleted list.
        ///   - A block whose lines all start with "1." "2." ... renders as a numbered list.
        ///   - Otherwise the block is a paragraph (internal single newlines become spaces).
        /// The .ascx turns these blocks into &lt;p&gt; / &lt;ul&gt; / &lt;ol&gt;.
        /// </summary>
        public static List<BodyBlock> ParseBody(string plain)
        {
            var blocks = new List<BodyBlock>();
            if (string.IsNullOrEmpty(plain)) return blocks;

            string normalized = plain.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] rawBlocks = normalized.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string raw in rawBlocks)
            {
                string[] lines = raw.Split('\n');
                var trimmed = new List<string>();
                foreach (string l in lines)
                    if (!string.IsNullOrWhiteSpace(l)) trimmed.Add(l.Trim());
                if (trimmed.Count == 0) continue;

                bool allBullets = true;
                bool allNumbered = true;
                foreach (string l in trimmed)
                {
                    if (!l.StartsWith("- ")) allBullets = false;
                    if (!StartsWithNumber(l)) allNumbered = false;
                }

                if (allBullets || allNumbered)
                {
                    var items = new List<string>();
                    var itemsHtml = new List<string>();
                    foreach (string l in trimmed)
                    {
                        string v = allBullets ? l.Substring(2).Trim() : StripNumber(l);
                        items.Add(v);
                        itemsHtml.Add(ToHtml(v));
                    }
                    blocks.Add(new BodyBlock
                    {
                        IsList = true,
                        IsOrdered = allNumbered && !allBullets,
                        Items = items,
                        ItemsHtml = itemsHtml
                    });
                }
                else
                {
                    string text = string.Join(" ", trimmed.ToArray());
                    blocks.Add(new BodyBlock { IsList = false, Text = text, Html = ToHtml(text) });
                }
            }
            return blocks;
        }

        private static bool StartsWithNumber(string line)
        {
            int dot = line.IndexOf('.');
            if (dot <= 0) return false;
            string prefix = line.Substring(0, dot);
            int n;
            return int.TryParse(prefix, out n);
        }

        private static string StripNumber(string line)
        {
            int dot = line.IndexOf('.');
            return dot > 0 ? line.Substring(dot + 1).Trim() : line.Trim();
        }
    }
}
