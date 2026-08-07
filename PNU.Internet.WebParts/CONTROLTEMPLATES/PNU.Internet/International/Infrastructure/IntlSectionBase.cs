using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Base class for every International Students section control.
    /// Handles list provisioning, reading, bilingual resolution and DTO projection,
    /// so each .ascx.cs only declares its list name and binds its repeater.
    /// </summary>
    public abstract class IntlSectionBase : UserControl
    {
        /// <summary>Internal name of the SharePoint list backing this section.</summary>
        protected abstract string ListName { get; }

        /// <summary>Optional override of the list name from the ASCX tag / page layout.</summary>
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

        protected List<IntlCard> Items { get; private set; }

        protected IntlSectionBase()
        {
            Items = new List<IntlCard>();
            ShowTitle = "True";
        }

        /// <summary>Section heading, resolved from the schema unless explicitly overridden.</summary>
        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                IntlListDef def = IntlListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Checks/provisions the list on the target web (/ar/International) then
        /// loads + projects its items.
        /// Called on EVERY Page_Load - ViewState is often disabled in SharePoint zones.
        /// </summary>
        protected void LoadItems()
        {
            try
            {
                // Page-load safety net: verifies the list still exists on this web and
                // re-provisions it (columns + default data) if it is missing.
                InternationalListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = IntlTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> raw = IntlHelper.GetItems(web, EffectiveListName);

                    Items = raw.Select(Project).ToList();

                    if (MaxItems > 0 && Items.Count > MaxItems)
                        Items = Items.Take(MaxItems).ToList();
                }
            }
            catch (Exception ex)
            {
                IntlLog.Write(
                    GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<IntlCard>();
            }
        }

        /// <summary>
        /// Maps a list item to the flat DTO. Every text value is HTML-encoded here so
        /// the markup can emit it raw through <%# Eval(...) %> without XSS risk.
        /// </summary>
        protected virtual IntlCard Project(SPListItem item)
        {
            var card = new IntlCard
            {
                Id = item.ID,
                Title = IntlHelper.Enc(IntlHelper.Pick(IntlHelper.SafeString(item, "Title"),
                                                       IntlHelper.SafeString(item, "Title_EN"))),
                Description = IntlHelper.Enc(IntlHelper.Pick(IntlHelper.SafeString(item, "Description"),
                                                             IntlHelper.SafeString(item, "Description_EN"))),
                StatValue = IntlHelper.Enc(IntlHelper.SafeString(item, "StatValue")),
                ContactValue = IntlHelper.Enc(IntlHelper.Pick(IntlHelper.SafeString(item, "ContactValue"),
                                                              IntlHelper.SafeString(item, "ContactValue_EN"))),
                IconClass = IntlHelper.Enc(IntlHelper.SafeString(item, "IconClass")),
                LinkUrl = IntlHelper.Enc(IntlHelper.SafeUrl(item, "LinkUrl")),
                ButtonText = IntlHelper.Enc(IntlHelper.Pick(IntlHelper.SafeString(item, "ButtonText"),
                                                            IntlHelper.SafeString(item, "ButtonText_EN"))),
                ItemOrder = IntlHelper.SafeInt(item, "ItemOrder")
            };

            foreach (string bullet in IntlHelper.SplitLines(
                         IntlHelper.Pick(IntlHelper.SafeString(item, "Bullets"),
                                         IntlHelper.SafeString(item, "Bullets_EN"))))
            {
                card.Bullets.Add(IntlHelper.Enc(bullet));
            }

            if (string.IsNullOrEmpty(card.IconClass)) card.IconClass = "hgi-information-circle";
            return card;
        }
    }
}
