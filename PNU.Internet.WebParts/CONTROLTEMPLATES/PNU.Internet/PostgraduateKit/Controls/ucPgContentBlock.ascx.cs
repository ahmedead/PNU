using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Free-form content cards: the English preparatory course, the graduate identity,
    /// the financial rules, the publication incentive and the reference footer.
    /// </summary>
    public partial class ucPgContentBlock : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.ContentBlocks; } }

        protected override string DefaultSectionKey { get { return BlockKey; } }

        /// <summary>Which kind of block this instance renders.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string BlockKey { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool RenderOuterContainer { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool RenderAsStep { get; set; }

        public string StepIconUrl
        {
            get { return PgListSchema.IconRoot + "faq-icon.png"; }
        }

        public ucPgContentBlock()
        {
            BlockKey = PgSectionKeys.EnglishCourse;
            RenderOuterContainer = true;
            RenderAsStep = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var blocks = new List<PgBlock>();

            foreach (SPListItem item in raw)
            {
                string key = PgHelper.SafeString(item, "BlockKey").Trim();
                if (!string.IsNullOrEmpty(BlockKey) && key.Length > 0 &&
                    !key.Equals(BlockKey, StringComparison.OrdinalIgnoreCase))
                    continue;

                string title = PgHelper.PickField(item, "Title");
                string subTitle = PgHelper.PickField(item, "SubTitle");
                string badge = PgHelper.PickField(item, "Badge");
                string statVal = PgHelper.SafeString(item, "StatValue");
                string icon = PgHelper.SafeUrl(item, "IconUrl");
                string paragraphs = PgHelper.PickField(item, "Paragraphs");
                string bullets = PgHelper.PickField(item, "Bullets");
                string highlights = PgHelper.PickField(item, "Highlights");
                string footerTitle = PgHelper.PickField(item, "FooterTitle");
                string footerParagraphs = PgHelper.PickField(item, "FooterParagraphs");
                bool collapsible = PgHelper.SafeBool(item, "Collapsible");
                int order = PgHelper.SafeInt(item, "ItemOrder");

                blocks.Add(BuildBlock(blocks.Count, key, title, subTitle, badge, statVal, icon, paragraphs, bullets, highlights, footerTitle, footerParagraphs, collapsible, order));
            }

            if (blocks.Count == 0)
            {
                PgListDef def = PgListSchema.Get(ListName);
                if (def != null && def.Seed != null)
                {
                    foreach (Dictionary<string, string> row in def.Seed)
                    {
                        string rowPhase = row.ContainsKey("Phase") ? row["Phase"] : string.Empty;
                        string key = row.ContainsKey("BlockKey") ? row["BlockKey"] : string.Empty;

                        if (!string.IsNullOrEmpty(EffectivePhase) && !string.IsNullOrEmpty(rowPhase) &&
                            !rowPhase.Equals(EffectivePhase, StringComparison.OrdinalIgnoreCase))
                            continue;

                        if (!string.IsNullOrEmpty(BlockKey) && !string.IsNullOrEmpty(key) &&
                            !key.Equals(BlockKey, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string title = PgHelper.Pick(row.ContainsKey("Title") ? row["Title"] : "", row.ContainsKey("Title_EN") ? row["Title_EN"] : "");
                        string subTitle = PgHelper.Pick(row.ContainsKey("SubTitle") ? row["SubTitle"] : "", row.ContainsKey("SubTitle_EN") ? row["SubTitle_EN"] : "");
                        string badge = PgHelper.Pick(row.ContainsKey("Badge") ? row["Badge"] : "", row.ContainsKey("Badge_EN") ? row["Badge_EN"] : "");
                        string statVal = row.ContainsKey("StatValue") ? row["StatValue"] : "";
                        string icon = row.ContainsKey("IconUrl") ? row["IconUrl"] : "";
                        string paragraphs = PgHelper.Pick(row.ContainsKey("Paragraphs") ? row["Paragraphs"] : "", row.ContainsKey("Paragraphs_EN") ? row["Paragraphs_EN"] : "");
                        string bullets = PgHelper.Pick(row.ContainsKey("Bullets") ? row["Bullets"] : "", row.ContainsKey("Bullets_EN") ? row["Bullets_EN"] : "");
                        string highlights = PgHelper.Pick(row.ContainsKey("Highlights") ? row["Highlights"] : "", row.ContainsKey("Highlights_EN") ? row["Highlights_EN"] : "");
                        string footerTitle = PgHelper.Pick(row.ContainsKey("FooterTitle") ? row["FooterTitle"] : "", row.ContainsKey("FooterTitle_EN") ? row["FooterTitle_EN"] : "");
                        string footerParagraphs = PgHelper.Pick(row.ContainsKey("FooterParagraphs") ? row["FooterParagraphs"] : "", row.ContainsKey("FooterParagraphs_EN") ? row["FooterParagraphs_EN"] : "");
                        bool collapsible = row.ContainsKey("Collapsible") && row["Collapsible"].Equals("Yes", StringComparison.OrdinalIgnoreCase);

                        int order = 0;
                        if (row.ContainsKey("ItemOrder")) int.TryParse(row["ItemOrder"], out order);

                        blocks.Add(BuildBlock(blocks.Count, key, title, subTitle, badge, statVal, icon, paragraphs, bullets, highlights, footerTitle, footerParagraphs, collapsible, order));
                    }
                }
            }

            if (rptBlocks != null)
            {
                rptBlocks.DataSource = blocks;
                rptBlocks.DataBind();
            }
        }

        private PgBlock BuildBlock(int index, string key, string title, string subTitle, string badge, string statValue, string iconUrl, string paragraphs, string bullets, string highlights, string footerTitle, string footerParagraphs, bool collapsible, int order)
        {
            string bulletsHtml;
            if (key.Equals(PgSectionKeys.GraduateIdentity, StringComparison.OrdinalIgnoreCase))
            {
                bulletsHtml = FormatGraduateIdentityGrid(bullets);
            }
            else if (key.Equals(PgSectionKeys.FinancialRules, StringComparison.OrdinalIgnoreCase))
            {
                bulletsHtml = FormatFinancialRulesList(bullets);
            }
            else
            {
                bulletsHtml = PgHelper.BulletHtml(bullets, "fs-custom-4");
            }

            string accent;
            if (key.Equals(PgSectionKeys.GraduateIdentity, StringComparison.OrdinalIgnoreCase))
                accent = "border-light-blue";
            else if (key.Equals(PgSectionKeys.FinancialRules, StringComparison.OrdinalIgnoreCase))
                accent = "border-dark-blue";
            else
                accent = Accent(index + 1);

            string collapseId;
            if (key.Equals(PgSectionKeys.GraduateIdentity, StringComparison.OrdinalIgnoreCase))
                collapseId = "collapse1";
            else if (key.Equals(PgSectionKeys.FinancialRules, StringComparison.OrdinalIgnoreCase))
                collapseId = "collapse2";
            else
                collapseId = "collapse_" + (!string.IsNullOrEmpty(key) ? key : "block") + "_" + (index + 1).ToString();

            return new PgBlock
            {
                Id = index + 1,
                CollapseId = collapseId,
                Key = PgHelper.Enc(key),
                Title = PgHelper.Enc(title),
                SubTitle = PgHelper.Enc(subTitle),
                Badge = PgHelper.Enc(badge),
                StatValue = PgHelper.Enc(statValue),
                IconUrl = PgHelper.Enc(iconUrl),
                ParagraphsHtml = PgHelper.ParagraphHtml(paragraphs, "mb-2 fs-custom-4"),
                BulletsHtml = bulletsHtml,
                Highlights = PgHelper.ParseHighlights(highlights),
                FooterTitle = PgHelper.Enc(footerTitle),
                FooterParagraphsHtml = PgHelper.ParagraphHtml(footerParagraphs, "mb-2 fs-custom-4"),
                Collapsible = collapsible,
                ItemOrder = order,
                AccentClass = accent
            };
        }

        public static string FormatGraduateIdentityGrid(string raw)
        {
            List<string> lines = PgHelper.Lines(raw);
            if (lines.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.Append("<div class=\"row\">");

            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ':' }, 2);
                string category = parts[0].Trim();
                string itemsStr = parts.Length > 1 ? parts[1].Trim().TrimEnd('.') : string.Empty;

                sb.Append("<div class=\"col-12 col-sm-4\">");
                sb.Append("<div class=\"bg-light text-dark bg-custom-dark text-center \">").Append(PgHelper.Enc(category)).Append(":</div>");
                sb.Append("<div class=\"d-flex flex-wrap gap-2 py-3\">");

                string[] items = itemsStr.Split(new char[] { '،', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string text = item.Trim();
                    if (text.Length > 0)
                    {
                        sb.Append("<span class=\"bg-custom-primary rounded-3\">").Append(PgHelper.Enc(text)).Append("</span>");
                    }
                }

                sb.Append("</div></div>");
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        public static string FormatFinancialRulesList(string raw)
        {
            List<string> lines = PgHelper.Lines(raw);
            if (lines.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.Append("<ul class=\"list-group\">");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string itemClass = (i % 2 == 0) ? "list-group-item list-group-item-primary" : "list-group-item";

                string[] parts = line.Split(new char[] { ':' }, 2);
                string title = parts[0].Trim();
                string text = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                sb.Append("<li class=\"").Append(itemClass).Append("\" aria-current=\"true\">");
                sb.Append("<p>").Append(PgHelper.Enc(title)).Append("</p>");
                if (text.Length > 0)
                {
                    sb.Append("<p>").Append(PgHelper.Enc(text)).Append("</p>");
                }
                sb.Append("</li>");
            }

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
