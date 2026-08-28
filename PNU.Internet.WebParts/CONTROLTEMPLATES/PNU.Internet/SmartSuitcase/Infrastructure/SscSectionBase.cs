using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Base class for every Smart Suitcase section control.
    /// Handles list provisioning, reading, bilingual resolution and DTO projection.
    /// </summary>
    public abstract class SscSectionBase : UserControl
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

        protected List<SscCard> Items { get; private set; }

        protected SscSectionBase()
        {
            Items = new List<SscCard>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                SscListDef def = SscListSchema.Get(EffectiveListName);
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
                SmartSuitcaseListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = SscTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> raw = SscHelper.GetItems(web, EffectiveListName);

                    Items = raw.Select(Project).ToList();

                    if (MaxItems > 0 && Items.Count > MaxItems)
                        Items = Items.Take(MaxItems).ToList();
                }
            }
            catch (Exception ex)
            {
                SscLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<SscCard>();
            }
        }

        protected virtual SscCard Project(SPListItem item)
        {
            var card = new SscCard
            {
                Id = item.ID,
                Title = SscHelper.Enc(SscHelper.Pick(SscHelper.SafeString(item, "Title"),
                                                     SscHelper.SafeString(item, "Title_EN"))),
                Description = SscHelper.Enc(SscHelper.Pick(SscHelper.SafeString(item, "Description"),
                                                           SscHelper.SafeString(item, "Description_EN"))),
                ContactValue = SscHelper.Enc(SscHelper.Pick(SscHelper.SafeString(item, "ContactValue"),
                                                            SscHelper.SafeString(item, "ContactValue_EN"))),
                IconClass = SscHelper.Enc(SscHelper.SafeString(item, "IconClass")),
                LinkUrl = SscHelper.Enc(SscHelper.SafeUrl(item, "LinkUrl")),
                ButtonText = SscHelper.Enc(SscHelper.Pick(SscHelper.SafeString(item, "ButtonText"),
                                                          SscHelper.SafeString(item, "ButtonText_EN"))),
                ImageUrl = SscHelper.Enc(SscHelper.SafeUrl(item, "ImageUrl")),
                ImageAlt = SscHelper.Enc(SscHelper.Pick(SscHelper.SafeString(item, "ImageAlt"),
                                                        SscHelper.SafeString(item, "ImageAlt_EN"))),
                Badge1 = SscHelper.Enc(ResolveBadge(SscHelper.SafeString(item, "Badge1"), SscHelper.SafeString(item, "Badge1_EN"))),
                Badge2 = SscHelper.Enc(ResolveBadge(SscHelper.SafeString(item, "Badge2"), SscHelper.SafeString(item, "Badge2_EN"))),
                ItemOrder = SscHelper.SafeInt(item, "ItemOrder")
            };

            return card;
        }

        private static string ResolveBadge(string ar, string en)
        {
            if (SscHelper.IsArabic) return string.IsNullOrEmpty(ar) ? (en ?? string.Empty) : ar;

            if (!string.IsNullOrEmpty(en)) return en;

            // Fallback translations if EN field was not yet populated in existing lists
            if (ar == "طالبات") return "Students";
            if (ar == "منسوبو الجامعة") return "University Staff";
            if (ar == "أعضاء هيئة التدريس") return "Faculty";
            if (ar == "الموظفون") return "Staff";
            if (ar == "طلاب") return "Students";

            return ar ?? string.Empty;
        }
    }
}
