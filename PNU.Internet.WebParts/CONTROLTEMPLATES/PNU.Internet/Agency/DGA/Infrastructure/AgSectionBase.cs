using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// Base class for every Agency section control.
    /// Handles list checking/provisioning, reading, bilingual resolution and DTO
    /// projection, so each .ascx.cs only declares its list name and binds its repeater.
    /// </summary>
    public abstract class AgSectionBase : UserControl
    {
        /// <summary>Internal name of the SharePoint list backing this section.</summary>
        protected abstract string ListName { get; }

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

        /// <summary>Overrides the section heading text coming from the schema.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected List<AgCard> Items { get; private set; }

        protected AgSectionBase()
        {
            Items = new List<AgCard>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                AgListDef def = AgListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>First item of the list, or null - used by the single-item sections.</summary>
        protected AgCard FirstItem
        {
            get { return Items.Count > 0 ? Items[0] : null; }
        }

        /// <summary>
        /// Checks/provisions the list then loads + projects its items.
        /// Called on EVERY Page_Load - ViewState is often disabled in SharePoint zones.
        /// </summary>
        protected void LoadItems()
        {
            try
            {
                // Page-load safety net: verifies the list still exists on this web and
                // re-provisions it (columns + default data) if it is missing.
                AgListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    List<SPListItem> raw = AgHelper.GetItems(web, EffectiveListName);

                    Items = raw.Select(Project).ToList();

                    if (MaxItems > 0 && Items.Count > MaxItems)
                        Items = Items.Take(MaxItems).ToList();
                }
            }
            catch (Exception ex)
            {
                AgLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<AgCard>();
            }
        }

        /// <summary>
        /// Maps a list item to the flat DTO. Every text value is HTML-encoded here so
        /// the markup can emit it raw through Eval(...) without XSS risk.
        /// </summary>
        protected virtual AgCard Project(SPListItem item)
        {
            string rawSm = AgHelper.SafeUrl(item, "ImageUrl");
            string rawMd = AgHelper.SafeUrl(item, "ImageUrlMd");

            var card = new AgCard
            {
                Id = item.ID,
                Title = AgHelper.Enc(AgHelper.Pick(AgHelper.SafeString(item, "Title"),
                                                   AgHelper.SafeString(item, "Title_EN"))),
                Description = AgHelper.Enc(AgHelper.Pick(AgHelper.SafeString(item, "Description"),
                                                         AgHelper.SafeString(item, "Description_EN"))),
                RoleText = AgHelper.Enc(AgHelper.Pick(AgHelper.SafeString(item, "RoleText"),
                                                      AgHelper.SafeString(item, "RoleText_EN"))),
                SubTitle = AgHelper.Enc(AgHelper.Pick(AgHelper.SafeString(item, "SubTitle"),
                                                      AgHelper.SafeString(item, "SubTitle_EN"))),
                Category = AgHelper.SafeString(item, "TaskGroup"),
                IconClass = AgHelper.Enc(AgHelper.SafeString(item, "IconClass")),
                LinkUrl = AgHelper.Enc(AgHelper.SafeUrl(item, "LinkUrl")),
                ImageUrl = AgHelper.Enc(rawSm),
                ImageUrlMd = AgHelper.Enc(rawMd),
                ColumnClass = AgHelper.Enc(AgHelper.SafeString(item, "ColumnClass")),
                ItemOrder = AgHelper.SafeInt(item, "ItemOrder"),
                RawSm = rawSm,
                RawMd = string.IsNullOrEmpty(rawMd) ? rawSm : rawMd
            };

            if (string.IsNullOrEmpty(card.IconClass)) card.IconClass = "hgi-information-circle";
            return card;
        }
    }
}
