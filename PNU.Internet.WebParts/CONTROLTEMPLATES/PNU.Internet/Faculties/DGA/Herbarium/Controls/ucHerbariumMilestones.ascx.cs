using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumMilestones : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            ltMilestonesTitle.Text = HerbariumHelper.GetRes("Herbarium_MilestonesTitle", "محطات بارزة", "Key Milestones");

            List<SPListItem> milestoneItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumMilestones);
            List<HerbariumMilestoneModel> list = new List<HerbariumMilestoneModel>();

            if (milestoneItems != null && milestoneItems.Count > 0)
            {
                foreach (var item in milestoneItems)
                {
                    string badgeCls = HerbariumHelper.SafeString(item, "BadgeClass");
                    if (string.IsNullOrEmpty(badgeCls)) badgeCls = "badge-info";

                    list.Add(new HerbariumMilestoneModel
                    {
                        Id = item.ID,
                        Title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN"))),
                        Description = HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN")),
                        BadgeText = HerbariumHelper.Enc(HerbariumHelper.SafeString(item, "BadgeText")),
                        BadgeClass = HerbariumHelper.Enc(badgeCls),
                        ItemOrder = HerbariumHelper.SafeInt(item, "ItemOrder")
                    });
                }
            }

            rptMilestones.DataSource = list;
            rptMilestones.DataBind();
        }
    }
}
