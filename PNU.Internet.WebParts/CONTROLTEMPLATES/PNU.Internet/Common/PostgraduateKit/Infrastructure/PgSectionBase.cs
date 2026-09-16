using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Base class for every Postgraduate Academic Kit section control.
    /// Resolves the audience (phase), loads the section heading from PgSections,
    /// provisions the backing list and hands the raw items to the derived control.
    /// </summary>
    public abstract class PgSectionBase : UserControl
    {
        /// <summary>Name of the list this section reads from.</summary>
        protected abstract string ListName { get; }

        /// <summary>Key used to look the heading up in the PgSections list.</summary>
        protected abstract string DefaultSectionKey { get; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string Phase { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionKeyOverride { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxItems { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowTitle { get; set; }

        /// <summary>Hard-coded heading that wins over the PgSections list.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected PgSectionInfo Section { get; private set; }

        protected PgSectionBase()
        {
            ShowTitle = "True";
            Phase = PgPhases.Applicants;
            Section = new PgSectionInfo();
        }

        protected string EffectivePhase
        {
            get { return string.IsNullOrEmpty(Phase) ? PgPhases.Applicants : Phase; }
        }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected string EffectiveSectionKey
        {
            get { return string.IsNullOrEmpty(SectionKeyOverride) ? DefaultSectionKey : SectionKeyOverride; }
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return PgHelper.Enc(SectionTitle);
                return Section != null ? Section.Title : string.Empty;
            }
        }

        protected string LeadText
        {
            get { return Section != null ? Section.Lead : string.Empty; }
        }

        protected bool HeadingVisible
        {
            get
            {
                if ("False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase)) return false;
                return !string.IsNullOrEmpty(HeadingText);
            }
        }

        protected bool LeadVisible
        {
            get { return !string.IsNullOrEmpty(LeadText); }
        }

        /// <summary>True when an editor has switched this axis off for the current audience.</summary>
        protected bool SectionEnabled
        {
            get { return Section == null || Section.Visible; }
        }

        /// <summary>Anchor id used by aria-labelledby, unique per section and audience.</summary>
        protected string HeadingId
        {
            get { return "pg-" + EffectivePhase.ToLowerInvariant() + "-" + EffectiveSectionKey.ToLowerInvariant() + "-title"; }
        }

        /// <summary>Alternates the accent border colour exactly the way the static design does.</summary>
        protected static string Accent(int index)
        {
            return (index % 2 == 0) ? "border-light-blue" : "border-dark-blue";
        }

        /// <summary>
        /// Provisions the lists, reads the section heading, loads the phase-filtered items and
        /// calls <see cref="OnItemsLoaded"/> while the web is still open.
        /// </summary>
        protected void LoadSection()
        {
            try
            {
                PgListProvisioner.EnsureListExists(PgListNames.Sections);
                PgListProvisioner.EnsureListExists(EffectiveListName);

                if (SPContext.Current == null) return;

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PgTargetWeb.Open(site))
                {
                    if (web == null) return;

                    Section = ReadSectionInfo(web);

                    List<SPListItem> raw = PgHelper.GetItems(web, EffectiveListName, EffectivePhase);
                    if (MaxItems > 0 && raw.Count > MaxItems) raw = raw.Take(MaxItems).ToList();

                    OnItemsLoaded(raw);
                }
            }
            catch (Exception ex)
            {
                PgLog.Write(GetType().Name + ".LoadSection:" + EffectiveListName, ex);
            }
        }

        /// <summary>Called with the raw list items while the SPWeb is still open. Project and bind here.</summary>
        protected virtual void OnItemsLoaded(List<SPListItem> raw)
        {
        }

        private PgSectionInfo ReadSectionInfo(SPWeb web)
        {
            var info = new PgSectionInfo();
            string key = EffectiveSectionKey;
            if (string.IsNullOrEmpty(key)) return info;

            try
            {
                foreach (SPListItem item in PgHelper.GetItems(web, PgListNames.Sections, EffectivePhase))
                {
                    if (!key.Equals(PgHelper.SafeString(item, "SectionKey"), StringComparison.OrdinalIgnoreCase))
                        continue;

                    info.Key = key;
                    info.Title = PgHelper.Enc(PgHelper.PickField(item, "Title"));
                    info.Lead = PgHelper.Enc(PgHelper.PickField(item, "Lead"));

                    string visible = PgHelper.SafeString(item, "Visible").Trim();
                    info.Visible = visible.Length == 0 || !visible.Equals("No", StringComparison.OrdinalIgnoreCase);
                    break;
                }
            }
            catch (Exception ex)
            {
                PgLog.Write(GetType().Name + ".ReadSectionInfo:" + key, ex);
            }

            return info;
        }
    }
}
