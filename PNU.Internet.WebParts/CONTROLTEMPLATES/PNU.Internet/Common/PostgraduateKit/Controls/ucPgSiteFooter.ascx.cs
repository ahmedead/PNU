using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// The footer of the postgraduate kit pages: three editable link columns from the
    /// PgFooterLinks list, the university's social accounts, and the bottom bar.
    /// </summary>
    public partial class ucPgSiteFooter : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.FooterLinks; } }

        protected override string DefaultSectionKey { get { return string.Empty; } }

        protected string ImageRoot { get { return PgListSchema.ImageRoot; } }

        protected string SocialHeading
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_FooterSocial", "منصاتنا الرقمية", "Our digital platforms")); }
        }

        protected string Copyright
        {
            get
            {
                string year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                return PgHelper.Enc(PgHelper.Pick(
                    "جميع الحقوق محفوظة " + year + " | جامعة الأميرة نورة بنت عبدالرحمن",
                    "All rights reserved " + year + " | Princess Nourah bint Abdulrahman University"));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var columns = new List<PgFooterColumn>();
            var index = new Dictionary<string, PgFooterColumn>(StringComparer.Ordinal);
            var bottom = new List<PgLinkItem>();

            foreach (SPListItem item in raw)
            {
                var link = new PgLinkItem
                {
                    Text = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    Url = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    NewTab = PgHelper.SafeBool(item, "NewTab")
                };

                if (string.IsNullOrEmpty(link.Url)) link.Url = "#";

                bool isBottom = PgListSchema.PgFooterAreas.BottomBar.Equals(
                    PgHelper.SafeString(item, "Area").Trim(), StringComparison.OrdinalIgnoreCase);

                if (isBottom)
                {
                    bottom.Add(link);
                    continue;
                }

                string heading = PgHelper.Enc(PgHelper.PickField(item, "GroupName"));

                PgFooterColumn column;
                if (!index.TryGetValue(heading, out column))
                {
                    column = new PgFooterColumn
                    {
                        Heading = heading,
                        CollapseId = "pgFooterCol" + (columns.Count + 1).ToString(CultureInfo.InvariantCulture)
                    };
                    index[heading] = column;
                    columns.Add(column);
                }

                column.Links.Add(link);
            }

            if (rptColumns != null)
            {
                rptColumns.DataSource = columns;
                rptColumns.DataBind();
            }

            if (rptBottomLinks != null)
            {
                rptBottomLinks.DataSource = bottom;
                rptBottomLinks.DataBind();
            }
        }
    }
}
