using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumOverview : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            // Bind Overview card
            List<SPListItem> overviewItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumOverview);
            if (overviewItems != null && overviewItems.Count > 0)
            {
                SPListItem item = overviewItems[0];
                ltOverviewTitle.Text = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN")));
                ltOverviewDesc.Text = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN")));
                ltOverviewIcon.Text = HerbariumHelper.Icon(HerbariumHelper.SafeString(item, "IconClass"), "hgi hgi-stroke hgi-leaf-01 fs-3");
            }
            else
            {
                ltOverviewTitle.Text = HerbariumHelper.GetRes("Herbarium_OverviewTitle", "نبذة عن المعشبة", "About the Herbarium");
                ltOverviewDesc.Text = HerbariumHelper.GetRes("Herbarium_OverviewDesc", "تضم المعشبة عينات نباتية مجففة ومضغوطة وموثقة ببياناتها العلمية، لتكون سجلًا مرجعيًا يساعد الباحثات والطالبات في التعرف على النباتات وتصنيفها ودراسة التنوع النباتي في المملكة.", "The herbarium contains dried, pressed, and scientifically documented plant specimens...");
                ltOverviewIcon.Text = "hgi hgi-stroke hgi-leaf-01 fs-3";
            }

            // Bind Statistics cards
            List<SPListItem> statItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumStats);
            List<HerbariumStatModel> list = new List<HerbariumStatModel>();

            if (statItems != null && statItems.Count > 0)
            {
                foreach (var item in statItems)
                {
                    list.Add(new HerbariumStatModel
                    {
                        Id = item.ID,
                        StatValue = HerbariumHelper.Enc(HerbariumHelper.SafeString(item, "StatValue")),
                        Title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN"))),
                        ItemOrder = HerbariumHelper.SafeInt(item, "ItemOrder")
                    });
                }
            }

            rptStats.DataSource = list;
            rptStats.DataBind();
        }
    }
}
