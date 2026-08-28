using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    /// <summary>
    /// Base class for every Central Library section control.
    /// Handles list loading, checking/provisioning, and DTO projection.
    /// </summary>
    public abstract class ClSectionBase : UserControl
    {
        protected abstract string ListName { get; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxItems { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowTitle { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected List<ClCard> Items { get; private set; }

        protected ClSectionBase()
        {
            Items = new List<ClCard>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                ClListDef def = ClListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        protected void LoadItems()
        {
            try
            {
                ClListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = ClTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> raw = ClHelper.GetItems(web, EffectiveListName);

                    Items = raw.Select(Project).ToList();

                    if (MaxItems > 0 && Items.Count > MaxItems)
                        Items = Items.Take(MaxItems).ToList();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<ClCard>();
            }
        }

        protected virtual ClCard Project(SPListItem item)
        {
            var card = new ClCard
            {
                Id = item.ID,
                Title = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Title"),
                                                   ClHelper.SafeString(item, "Title_EN"))),
                Description = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Description"),
                                                         ClHelper.SafeString(item, "Description_EN"))),
                StatValue = ClHelper.Enc(ClHelper.SafeString(item, "StatValue")),
                SubTitle = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "SubTitle"),
                                                      ClHelper.SafeString(item, "SubTitle_EN"))),
                BadgeText = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "BadgeText"),
                                                       ClHelper.SafeString(item, "BadgeText_EN"))),
                IconClass = ClHelper.Enc(ClHelper.SafeString(item, "IconClass")),
                LinkUrl = ClHelper.Enc(ClHelper.SafeUrl(item, "LinkUrl")),
                ImageUrl = ClHelper.Enc(ClHelper.SafeUrl(item, "ImageUrl")),
                LogoUrl = ClHelper.Enc(ClHelper.SafeUrl(item, "LogoUrl")),
                ButtonText = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "ButtonText"),
                                                        ClHelper.SafeString(item, "ButtonText_EN"))),
                ItemOrder = ClHelper.SafeInt(item, "ItemOrder"),

                // Contact specific fields
                Row1Label = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row1Label"), ClHelper.SafeString(item, "Row1Label_EN"))),
                Row1Value = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row1Value"), ClHelper.SafeString(item, "Row1Value_EN"))),
                Row2Label = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row2Label"), ClHelper.SafeString(item, "Row2Label_EN"))),
                Row2Value = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row2Value"), ClHelper.SafeString(item, "Row2Value_EN"))),
                Row3Label = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row3Label"), ClHelper.SafeString(item, "Row3Label_EN"))),
                Row3Value = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Row3Value"), ClHelper.SafeString(item, "Row3Value_EN"))),
                Email = ClHelper.Enc(ClHelper.SafeString(item, "Email")),
                Phone1 = ClHelper.Enc(ClHelper.SafeString(item, "Phone1")),
                Phone1Tel = ClHelper.Enc(ClHelper.SafeString(item, "Phone1Tel")),
                Phone2 = ClHelper.Enc(ClHelper.SafeString(item, "Phone2")),
                Phone2Tel = ClHelper.Enc(ClHelper.SafeString(item, "Phone2Tel")),
                LocationText = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "LocationText"), ClHelper.SafeString(item, "LocationText_EN")))
            };

            if (string.IsNullOrEmpty(card.IconClass)) card.IconClass = "hgi-information-circle";
            return card;
        }
    }
}
