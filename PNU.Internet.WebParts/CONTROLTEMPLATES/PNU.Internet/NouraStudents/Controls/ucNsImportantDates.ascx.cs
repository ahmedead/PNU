using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsImportantDates : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.ImportantDates; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = NsHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                List<NsDateGroup> groups = BuildGroups();

                rptGroups.DataSource = groups;
                rptGroups.DataBind();

                secImportantDates.Visible = groups.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsImportantDates.Bind", ex);
                secImportantDates.Visible = false;
            }
        }

        /// <summary>
        /// Groups the items by the DateCategory choice, keeping the schema order of the
        /// categories so the three columns always appear in the designed sequence.
        /// Items without a date are dropped - the badge would render empty.
        /// </summary>
        private List<NsDateGroup> BuildGroups()
        {
            var groups = new List<NsDateGroup>();
            List<NsCard> dated = Items.Where(c => c.HasDate).ToList();

            foreach (string category in NsListSchema.DateCategories)
            {
                List<NsCard> rows = dated
                    .Where(c => string.Equals(c.Category, category, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(c => c.ItemOrder)
                    .ThenBy(c => c.EventDate)
                    .ToList();

                if (rows.Count > 0)
                {
                    groups.Add(new NsDateGroup
                    {
                        // The stored choice value is Arabic; resolve the display label
                        // so the English page does not show Arabic column headings.
                        Title = NsHelper.Enc(NsListSchema.DateCategoryDisplay(category)),
                        Items = rows
                    });
                }
            }

            // Any category an editor added that is not in the schema still gets a column.
            foreach (string extra in dated
                        .Select(c => c.Category)
                        .Where(c => !string.IsNullOrEmpty(c)
                                 && !NsListSchema.DateCategories.Contains(c, StringComparer.OrdinalIgnoreCase))
                        .Distinct())
            {
                groups.Add(new NsDateGroup
                {
                    Title = NsHelper.Enc(NsListSchema.DateCategoryDisplay(extra)),
                    Items = dated.Where(c => c.Category == extra)
                                 .OrderBy(c => c.ItemOrder).ThenBy(c => c.EventDate).ToList()
                });
            }

            return groups;
        }
    }
}
