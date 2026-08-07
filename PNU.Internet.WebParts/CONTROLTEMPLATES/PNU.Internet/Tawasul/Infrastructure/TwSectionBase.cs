using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Base class for every Tawasul (تواصل) section control.
    /// Handles list checking/provisioning on the target web (/ar/Tawasul), reading,
    /// bilingual resolution and DTO projection. A control that reads a single list only
    /// overrides <see cref="ListName"/>; a control that reads several lists uses
    /// <see cref="LoadList"/> directly. Section headings/subtitles come from the shared
    /// TwSectionTitles list via <see cref="Section"/>.
    /// </summary>
    public abstract class TwSectionBase : UserControl
    {
        /// <summary>Internal name of the single list backing this section, or null.</summary>
        protected virtual string ListName { get { return null; } }

        /// <summary>Key of this section's heading row in the TwSectionTitles list.</summary>
        protected virtual string SectionKey { get { return null; } }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        /// <summary>0 = show all items.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxItems { get; set; }

        /// <summary>Set to "False" to hide the section heading.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowTitle { get; set; }

        /// <summary>Overrides the section heading text coming from the list.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected List<TwCard> Items { get; private set; }

        private List<TwCard> _sectionRows;

        protected TwSectionBase()
        {
            Items = new List<TwCard>();
            ShowTitle = "True";
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>Heading text: explicit override, then the TwSectionTitles row.</summary>
        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return TwHelper.Enc(SectionTitle);
                TwCard s = Section(SectionKey);
                return s != null ? s.Title : string.Empty;   // already HTML-encoded
            }
        }

        /// <summary>Subtitle text from the TwSectionTitles row, or empty.</summary>
        protected string SubtitleText
        {
            get
            {
                TwCard s = Section(SectionKey);
                return s != null ? s.SubTitle : string.Empty;
            }
        }

        protected TwCard FirstItem
        {
            get { return Items.Count > 0 ? Items[0] : null; }
        }

        /// <summary>Checks/provisions the single list then loads + projects its items.</summary>
        protected void LoadItems()
        {
            Items = LoadList(EffectiveListName);
            if (MaxItems > 0 && Items.Count > MaxItems)
                Items = Items.Take(MaxItems).ToList();
        }

        /// <summary>
        /// Checks/provisions a named list on the target web and returns its projected
        /// items. Used by controls that read more than one list.
        /// </summary>
        protected List<TwCard> LoadList(string listName)
        {
            var result = new List<TwCard>();
            if (string.IsNullOrEmpty(listName)) return result;

            try
            {
                TwListProvisioner.EnsureListExists(listName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = TwTargetWeb.Open(site))
                {
                    if (web == null) return result;
                    result = TwHelper.GetItems(web, listName).Select(Project).ToList();
                }
            }
            catch (Exception ex)
            {
                TwLog.Write(GetType().Name + ".LoadList:" + listName, ex);
            }

            return result;
        }

        /// <summary>Items of one list filtered to a raw category / group value.</summary>
        protected List<TwCard> ItemsInGroup(string listName, string groupValue)
        {
            return LoadList(listName)
                .Where(c => string.Equals(c.Category, groupValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>The TwSectionTitles row for a section key, or null (list cached once).</summary>
        protected TwCard Section(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if (_sectionRows == null) _sectionRows = LoadList(TwListNames.SectionTitles);
            return _sectionRows.FirstOrDefault(
                c => string.Equals(c.Category, key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Maps a list item to the flat DTO. Every text value is HTML-encoded here so
        /// the markup can emit it raw through Eval(...) without XSS risk.
        /// </summary>
        protected virtual TwCard Project(SPListItem item)
        {
            string category = TwHelper.SafeString(item, "SectionKey");
            if (string.IsNullOrEmpty(category)) category = TwHelper.SafeString(item, "ContactGroup");

            var card = new TwCard
            {
                Id = item.ID,
                Title = TwHelper.Enc(TwHelper.Pick(TwHelper.SafeString(item, "Title"),
                                                   TwHelper.SafeString(item, "Title_EN"))),
                Description = TwHelper.Enc(TwHelper.Pick(TwHelper.SafeString(item, "Description"),
                                                         TwHelper.SafeString(item, "Description_EN"))),
                SubTitle = TwHelper.Enc(TwHelper.Pick(TwHelper.SafeString(item, "Subtitle"),
                                                      TwHelper.SafeString(item, "Subtitle_EN"))),
                ContactValue = TwHelper.Enc(TwHelper.Pick(TwHelper.SafeString(item, "ContactValue"),
                                                          TwHelper.SafeString(item, "ContactValue_EN"))),
                Category = category,
                IconClass = TwHelper.Enc(TwHelper.SafeString(item, "IconClass")),
                LinkUrl = TwHelper.Enc(TwHelper.SafeUrl(item, "LinkUrl")),
                Phone = TwHelper.Enc(TwHelper.SafeString(item, "Phone")),
                PhoneLink = TwHelper.Enc(TwHelper.SafeUrl(item, "PhoneLink")),
                Email = TwHelper.Enc(TwHelper.SafeString(item, "Email")),
                ShortCode = TwHelper.Enc(TwHelper.SafeString(item, "ShortCode")),
                MapUrl = TwHelper.Enc(TwHelper.SafeUrl(item, "MapUrl")),
                ItemOrder = TwHelper.SafeInt(item, "ItemOrder")
            };

            return card;
        }
    }
}
