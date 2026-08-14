using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumServices : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            ltServicesTitle.Text = HerbariumHelper.GetRes("Herbarium_ServicesTitle", "ما تقدمه المعشبة", "What the Herbarium Offers");

            List<SPListItem> serviceItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumServices);
            List<HerbariumServiceModel> list = new List<HerbariumServiceModel>();

            if (serviceItems != null && serviceItems.Count > 0)
            {
                foreach (var item in serviceItems)
                {
                    list.Add(new HerbariumServiceModel
                    {
                        Id = item.ID,
                        Title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN"))),
                        Description = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN"))),
                        IconClass = HerbariumHelper.Icon(HerbariumHelper.SafeString(item, "IconClass"), "hgi hgi-stroke hgi-plant-01 fs-3"),
                        ItemOrder = HerbariumHelper.SafeInt(item, "ItemOrder")
                    });
                }
            }

            rptServices.DataSource = list;
            rptServices.DataBind();
        }
    }
}
