using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// "المهام" - Bootstrap accordion. Rows are grouped by the TaskGroup choice,
    /// keeping the schema order of the groups so the accordion always appears in the
    /// designed sequence. A group with a single row renders as a paragraph, a group
    /// with several renders as a bullet list.
    /// </summary>
    public partial class ucAgMainTasks : AgSectionBase
    {
        protected override string ListName { get { return AgListNames.MainTasks; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = AgHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                List<AgTaskGroup> groups = BuildGroups();

                rptGroups.DataSource = groups;
                rptGroups.DataBind();

                secMainTasks.Visible = groups.Count > 0;
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgMainTasks.Bind", ex);
                secMainTasks.Visible = false;
            }
        }

        private List<AgTaskGroup> BuildGroups()
        {
            var groups = new List<AgTaskGroup>();

            foreach (string group in AgListSchema.TaskGroups)
            {
                List<AgCard> rows = Items
                    .Where(c => string.Equals(c.Category, group, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(c => c.ItemOrder)
                    .ThenBy(c => c.Id)
                    .ToList();

                if (rows.Count > 0)
                    Add(groups, AgListSchema.TaskGroupDisplay(group), rows);
            }

            // Any group an editor added that is not in the schema still gets an accordion item.
            foreach (string extra in Items
                        .Select(c => c.Category)
                        .Where(c => !string.IsNullOrEmpty(c)
                                 && !AgListSchema.TaskGroups.Contains(c, StringComparer.OrdinalIgnoreCase))
                        .Distinct())
            {
                Add(groups, extra, Items
                        .Where(c => c.Category == extra)
                        .OrderBy(c => c.ItemOrder).ThenBy(c => c.Id).ToList());
            }

            return groups;
        }

        private static void Add(List<AgTaskGroup> groups, string title, List<AgCard> rows)
        {
            groups.Add(new AgTaskGroup
            {
                Title = AgHelper.Enc(title),
                Index = groups.Count,
                Items = rows
            });
        }
    }
}
