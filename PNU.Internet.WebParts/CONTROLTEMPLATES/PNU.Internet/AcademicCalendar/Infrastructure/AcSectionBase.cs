using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>
    /// Base class for the Academic Calendar display control.
    /// Handles list checking/provisioning, reading and DTO projection, so the .ascx.cs
    /// only groups the rows by semester and binds its repeater.
    /// </summary>
    public abstract class AcSectionBase : UserControl
    {
        /// <summary>Internal name of the SharePoint list backing this section.</summary>
        protected abstract string ListName { get; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        /// <summary>Set to "False" to hide the section heading.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowTitle { get; set; }

        /// <summary>Overrides the section heading text coming from the schema.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected List<AcRow> Items { get; private set; }

        protected AcSectionBase()
        {
            Items = new List<AcRow>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                AcListDef def = AcListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Checks/provisions the list on the target web (/ar/AcademicCalendar) then
        /// loads + projects its items. Called on EVERY Page_Load - ViewState is often
        /// disabled in SharePoint zones.
        /// </summary>
        protected void LoadItems()
        {
            try
            {
                AcListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AcTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> raw = AcHelper.GetItems(web, EffectiveListName);
                    Items = raw.Select(Project).ToList();
                }
            }
            catch (Exception ex)
            {
                AcLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<AcRow>();
            }
        }

        /// <summary>
        /// Maps a list item to the flat DTO. Every text value is HTML-encoded here so
        /// the markup can emit it raw through Eval(...) without XSS risk. The Semester
        /// value is kept RAW because it is used for grouping/matching, not display.
        /// </summary>
        protected virtual AcRow Project(SPListItem item)
        {
            return new AcRow
            {
                Id = item.ID,
                Procedure = EncPick(item, "Title", "Title_EN"),
                Week = EncPick(item, "Week", "Week_EN"),
                DayPeriod = EncPick(item, "DayPeriod", "DayPeriod_EN"),
                HijriDate = EncPick(item, "HijriDate", "HijriDate_EN"),
                GregorianDate = EncPick(item, "GregorianDate", "GregorianDate_EN"),
                Semester = AcHelper.SafeString(item, "Semester"),
                ItemOrder = AcHelper.SafeInt(item, "ItemOrder")
            };
        }

        /// <summary>Picks the Arabic or English field for the page language, then HTML-encodes it.</summary>
        private static string EncPick(SPListItem item, string arField, string enField)
        {
            return AcHelper.Enc(AcHelper.Pick(
                AcHelper.SafeString(item, arField),
                AcHelper.SafeString(item, enField)));
        }
    }
}
