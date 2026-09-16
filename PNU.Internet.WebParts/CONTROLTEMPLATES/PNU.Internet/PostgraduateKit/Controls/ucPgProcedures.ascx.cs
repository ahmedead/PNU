using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// The interactive academic (or graduation) procedures, grouped by their sub-heading.
    /// Each card expands to show requirements, the regulation article and the related forms.
    /// </summary>
    public partial class ucPgProcedures : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Procedures; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Procedures; } }

        protected string ToggleIconUrl { get { return PgListSchema.IconRoot + "arrow-r-l.png"; } }
        protected string ConditionsIconUrl { get { return PgListSchema.IconRoot + "step001.png"; } }
        protected string ArticleIconUrl { get { return PgListSchema.IconRoot + "book-0.png"; } }
        protected string FormsIconUrl { get { return PgListSchema.IconRoot + "true01.png"; } }
        protected string FormIconUrl { get { return PgListSchema.IconRoot + "small-download.png"; } }

        protected string ConditionsLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_Conditions", "الشروط والمتطلبات", "Requirements")); }
        }

        protected string ArticleLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_Article", "نص المادة من اللائحة", "The text of the article")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var groups = new List<PgProcedureGroup>();
            var index = new Dictionary<string, PgProcedureGroup>(StringComparer.Ordinal);
            int position = 0;

            foreach (SPListItem item in raw)
            {
                string label = PgHelper.Enc(PgHelper.PickField(item, "GroupLabel"));

                PgProcedureGroup group;
                if (!index.TryGetValue(label, out group))
                {
                    group = new PgProcedureGroup { Label = label };
                    index[label] = group;
                    groups.Add(group);
                }

                string rawTitle = PgHelper.PickField(item, "Title");
                string iconUrl = PgHelper.SafeUrl(item, "IconUrl");
                if (string.IsNullOrEmpty(iconUrl) && !string.IsNullOrEmpty(rawTitle))
                {
                    if (rawTitle.IndexOf("خطة البحث", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("الرسالة", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("thesis", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "book-search.png";
                    else if (rawTitle.IndexOf("إلغاء", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Cancel", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "circle-x.png";
                    else if (rawTitle.IndexOf("إنذار", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Warning", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "warning.png";
                    else if (rawTitle.IndexOf("إعادة القيد", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Reinstate", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "rejoin.png";
                    else if (rawTitle.IndexOf("مناقشة", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Defence", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "board.png";
                    else if (rawTitle.IndexOf("الدرجة", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Degree", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "cup.png";
                    else if (rawTitle.IndexOf("معادلة", StringComparison.OrdinalIgnoreCase) >= 0 || rawTitle.IndexOf("Equivalence", StringComparison.OrdinalIgnoreCase) >= 0)
                        iconUrl = PgListSchema.IconRoot + "arrow-r-l.png";
                }

                int procId = ++position;
                group.Items.Add(new PgProcedure
                {
                    Id = procId,
                    Title = PgHelper.Enc(rawTitle),
                    Badge = PgHelper.Enc(PgHelper.PickField(item, "Badge")),
                    Description = PgHelper.Enc(PgHelper.PickField(item, "Description")),
                    GroupLabel = label,
                    ConditionsHtml = PgHelper.BulletHtml(PgHelper.PickField(item, "Conditions"), "fs-custom-4"),
                    ArticleTitle = PgHelper.Enc(PgHelper.PickField(item, "ArticleTitle")),
                    ArticleTextHtml = PgHelper.ParagraphHtml(PgHelper.PickField(item, "ArticleText"), "fs-custom-4"),
                    IconUrl = iconUrl,
                    FormsHeading = PgHelper.Enc(PgHelper.PickField(item, "FormsHeading")),
                    Forms = PgHelper.ParseLinks(PgHelper.PickField(item, "Forms")),
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(position++)
                });
            }

            if (rptGroups != null)
            {
                rptGroups.DataSource = groups;
                rptGroups.DataBind();
            }
        }
    }
}
