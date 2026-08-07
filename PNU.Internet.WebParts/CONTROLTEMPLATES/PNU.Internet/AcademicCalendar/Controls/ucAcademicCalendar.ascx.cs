using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>
    /// Renders the academic-procedures calendar as a Bootstrap accordion, one panel per
    /// semester. Rows come from the single AcCalendar list, grouped by the Semester choice
    /// (the same one-list-grouped-by-choice shape as ucNsImportantDates). The section
    /// heading, intro paragraph and external link come from the AcCalendarHeaders list,
    /// which stores each value in Arabic AND English. All content lives on
    /// /ar/AcademicCalendar and is resolved to the page language.
    /// </summary>
    public partial class ucAcademicCalendar : AcSectionBase
    {
        protected override string ListName { get { return AcListNames.Calendar; } }

        /// <summary>Fallback external link if the header row has none. Web part property.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string MoeUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                AcHeader header = LoadHeader();

                ltSectionTitle.Text = AcHelper.Enc(header.Heading);
                ltIntro.Text = AcHelper.Enc(header.Intro);
                phHeading.Visible = HeadingVisible && header.HasHeading;

                List<AcSemesterGroup> groups = BuildGroups();

                rptGroups.DataSource = groups;
                rptGroups.DataBind();

                lnkMoe.Text = AcHelper.Enc(header.MoeText);
                lnkMoe.NavigateUrl = header.MoeUrl;
                phMoe.Visible = header.HasMoe && !string.IsNullOrEmpty(header.MoeText);

                secAcademicCalendar.Visible = groups.Count > 0;
            }
            catch (Exception ex)
            {
                AcLog.Write("ucAcademicCalendar.Bind", ex);
                secAcademicCalendar.Visible = false;
            }
        }

        /// <summary>
        /// Reads the single AcCalendarHeaders row, resolved to the page language, with
        /// schema/property fallbacks so the section never renders without a heading.
        /// </summary>
        private AcHeader LoadHeader()
        {
            var h = new AcHeader();

            try
            {
                AcListProvisioner.EnsureListExists(AcListNames.Headers);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AcTargetWeb.Open(site))
                {
                    if (web != null)
                    {
                        SPList list = web.Lists.TryGetList(AcListNames.Headers);
                        if (list != null && list.ItemCount > 0)
                        {
                            SPListItemCollection items = list.GetItems(AcHelper.OrderedQuery());
                            if (items.Count > 0)
                            {
                                SPListItem item = items[0];
                                h.Heading = AcHelper.Pick(AcHelper.SafeString(item, "Title"),
                                                          AcHelper.SafeString(item, "Title_EN"));
                                h.Intro = AcHelper.Pick(AcHelper.SafeString(item, "Intro"),
                                                        AcHelper.SafeString(item, "Intro_EN"));
                                h.MoeText = AcHelper.Pick(AcHelper.SafeString(item, "MoeLinkText"),
                                                          AcHelper.SafeString(item, "MoeLinkText_EN"));
                                h.MoeUrl = AcHelper.SafeString(item, "MoeLinkUrl");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AcLog.Write("ucAcademicCalendar.LoadHeader", ex);
            }

            // Fallbacks so a missing/empty header list never blanks the section.
            if (string.IsNullOrEmpty(h.Heading))
            {
                AcListDef def = AcListSchema.Get(AcListNames.Calendar);
                h.Heading = def != null ? def.Display : string.Empty;
            }
            if (string.IsNullOrEmpty(h.MoeUrl)) h.MoeUrl = MoeUrl;

            return h;
        }

        /// <summary>
        /// Groups the rows by the Semester choice, keeping the schema order of the
        /// semesters so the three accordion panels always appear in the designed sequence.
        /// HeadingId / CollapseId are assigned per panel so the Bootstrap wiring is unique.
        /// </summary>
        private List<AcSemesterGroup> BuildGroups()
        {
            var groups = new List<AcSemesterGroup>();
            int index = 0;

            foreach (string semester in AcListSchema.Semesters)
            {
                List<AcRow> rows = Items
                    .Where(r => string.Equals(r.Semester, semester, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(r => r.ItemOrder)
                    .ThenBy(r => r.Id)
                    .ToList();

                if (rows.Count > 0)
                    groups.Add(MakeGroup(semester, rows, ref index));
            }

            // Any semester an editor added that is not in the schema still gets a panel.
            foreach (string extra in Items
                        .Select(r => r.Semester)
                        .Where(s => !string.IsNullOrEmpty(s)
                                 && !AcListSchema.Semesters.Contains(s, StringComparer.OrdinalIgnoreCase))
                        .Distinct())
            {
                List<AcRow> rows = Items
                    .Where(r => string.Equals(r.Semester, extra, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(r => r.ItemOrder).ThenBy(r => r.Id).ToList();

                groups.Add(MakeGroup(extra, rows, ref index));
            }

            return groups;
        }

        private static AcSemesterGroup MakeGroup(string semester, List<AcRow> rows, ref int index)
        {
            string suffix = index.ToString(CultureInfo.InvariantCulture);
            index++;

            return new AcSemesterGroup
            {
                // The stored choice value is Arabic; resolve the display label so the
                // English page does not show Arabic accordion headings.
                Title = AcHelper.Enc(AcListSchema.SemesterDisplay(semester)),
                HeadingId = "acHeading" + suffix,
                CollapseId = "acCollapse" + suffix,
                Items = rows
            };
        }
    }
}
