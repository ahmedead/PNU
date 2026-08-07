using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    /// <summary>
    /// Base class for every Noura Students section control.
    /// Handles list checking/provisioning, reading, bilingual resolution and DTO
    /// projection, so each .ascx.cs only declares its list name and binds its repeater.
    /// </summary>
    public abstract class NsSectionBase : UserControl
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

        protected List<NsCard> Items { get; private set; }

        protected NsSectionBase()
        {
            Items = new List<NsCard>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                NsListDef def = NsListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Checks/provisions the list on the target web (/ar/NouraStudents) then
        /// loads + projects its items.
        /// Called on EVERY Page_Load - ViewState is often disabled in SharePoint zones.
        /// </summary>
        protected void LoadItems()
        {
            try
            {
                // Page-load safety net: verifies the list still exists on this web and
                // re-provisions it (columns + default data) if it is missing.
                NsListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = NsTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> raw = NsHelper.GetItems(web, EffectiveListName);

                    Items = raw.Select(Project).ToList();

                    if (MaxItems > 0 && Items.Count > MaxItems)
                        Items = Items.Take(MaxItems).ToList();
                }
            }
            catch (Exception ex)
            {
                NsLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<NsCard>();
            }
        }

        /// <summary>
        /// Maps a list item to the flat DTO. Every text value is HTML-encoded here so
        /// the markup can emit it raw through Eval(...) without XSS risk.
        /// </summary>
        protected virtual NsCard Project(SPListItem item)
        {
            var card = new NsCard
            {
                Id = item.ID,
                Title = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "Title"),
                                                   NsHelper.SafeString(item, "Title_EN"))),
                Description = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "Description"),
                                                         NsHelper.SafeString(item, "Description_EN"))),
                StatValue = NsHelper.Enc(NsHelper.SafeString(item, "StatValue")),
                ContactValue = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "ContactValue"),
                                                          NsHelper.SafeString(item, "ContactValue_EN"))),
                BadgeText = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "BadgeText"),
                                                       NsHelper.SafeString(item, "BadgeText_EN"))),
                RoleText = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "RoleText"),
                                                      NsHelper.SafeString(item, "RoleText_EN"))),
                SubTitle = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "SubTitle"),
                                                      NsHelper.SafeString(item, "SubTitle_EN"))),
                Category = NsHelper.SafeString(item, "DateCategory"),
                IconClass = NsHelper.Enc(NsHelper.SafeString(item, "IconClass")),
                LinkUrl = NsHelper.Enc(NsHelper.SafeUrl(item, "LinkUrl")),
                ImageUrl = NsHelper.Enc(NsHelper.SafeUrl(item, "ImageUrl")),
                ButtonText = NsHelper.Enc(NsHelper.Pick(NsHelper.SafeString(item, "ButtonText"),
                                                        NsHelper.SafeString(item, "ButtonText_EN"))),
                ItemOrder = NsHelper.SafeInt(item, "ItemOrder"),
                EventDate = NsHelper.SafeDate(item, "EventDate")
            };

            if (card.EventDate.HasValue)
            {
                DateTime value = card.EventDate.Value;
                card.DateIso = NsHelper.IsoDate(value);
                card.DateDay = NsHelper.DayNumber(value);
                card.DateMonth = NsHelper.Enc(NsHelper.MonthName(value));
            }

            if (string.IsNullOrEmpty(card.IconClass)) card.IconClass = "hgi-information-circle";
            return card;
        }
    }
}
