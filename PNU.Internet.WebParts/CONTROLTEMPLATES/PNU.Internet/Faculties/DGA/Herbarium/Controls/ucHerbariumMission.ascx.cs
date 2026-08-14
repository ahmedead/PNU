using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumMission : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            ltSectionTitle.Text = HerbariumHelper.GetRes("Herbarium_MissionSectionTitle", "رسالة المعشبة وأهدافها", "Herbarium Mission & Objectives");

            // Bind Mission box
            List<SPListItem> missionItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumMission);
            if (missionItems != null && missionItems.Count > 0)
            {
                SPListItem item = missionItems[0];
                ltMissionTitle.Text = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN")));
                ltMissionDesc.Text = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN")));
            }
            else
            {
                ltMissionTitle.Text = HerbariumHelper.GetRes("Herbarium_MissionTitle", "الرسالة", "Mission");
                ltMissionDesc.Text = HerbariumHelper.GetRes("Herbarium_MissionDesc", "جمع وحفظ وتوثيق المجاميع النباتية في الفلورا السعودية، وتصنيفها علميًا بما يخدم البيئة والمجتمع والبحث العلمي.", "Collecting, preserving, and documenting plant collections...");
            }

            // Bind Objectives Accordion
            List<SPListItem> objItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumObjectives);
            List<HerbariumObjectiveModel> list = new List<HerbariumObjectiveModel>();

            if (objItems != null && objItems.Count > 0)
            {
                int index = 1;
                foreach (var item in objItems)
                {
                    list.Add(new HerbariumObjectiveModel
                    {
                        Id = index++,
                        Title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN"))),
                        Description = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN"))),
                        ItemOrder = HerbariumHelper.SafeInt(item, "ItemOrder")
                    });
                }
            }

            rptObjectives.DataSource = list;
            rptObjectives.DataBind();
        }
    }
}
