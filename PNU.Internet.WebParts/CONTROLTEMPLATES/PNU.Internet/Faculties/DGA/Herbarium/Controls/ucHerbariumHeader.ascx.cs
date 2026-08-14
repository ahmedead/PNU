using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumHeader : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            List<SPListItem> items = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumHeader);
            if (items != null && items.Count > 0)
            {
                SPListItem item = items[0];
                string title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN")));
                string subtitle = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "SubTitle"), HerbariumHelper.SafeString(item, "SubTitle_EN")));
                string desc = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN")));
                string imgUrl = HerbariumHelper.SafeUrl(item, "ImageUrl");
                string imgAlt = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "ImageAlt"), HerbariumHelper.SafeString(item, "ImageAlt_EN")));
                string caption = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "ImageCaption"), HerbariumHelper.SafeString(item, "ImageCaption_EN")));

                ltTitle.Text = title;
                ltDescription.Text = desc;
                ltSubTitle.Text = subtitle;
                phSubTitle.Visible = !string.IsNullOrEmpty(subtitle);

                imgHero.ImageUrl = string.IsNullOrEmpty(imgUrl) ? "https://taffy-modify-59445061.figma.site/_components/v2/6cc9e692147db66a673f8f1e07b75c437d197f83/image-9.5a9231f5.png" : imgUrl;
                imgHero.AlternateText = imgAlt;
                ltCaption.Text = caption;
            }
            else
            {
                ltTitle.Text = HerbariumHelper.GetRes("Herbarium_Title", "المعشبة النباتية", "Plant Herbarium");
                ltDescription.Text = HerbariumHelper.GetRes("Herbarium_Desc", "تُعد معشبة جامعة الأميرة نورة بنت عبدالرحمن مرجعًا علميًا لتوثيق النباتات المحلية وحفظ العينات النباتية ودعم البحث العلمي والتوعية البيئية.", "Princess Nourah Bint Abdulrahman University Herbarium serves as a scientific reference...");
                ltSubTitle.Text = "(PNUH)";
                phSubTitle.Visible = true;
                imgHero.ImageUrl = "https://taffy-modify-59445061.figma.site/_components/v2/6cc9e692147db66a673f8f1e07b75c437d197f83/image-9.5a9231f5.png";
                imgHero.AlternateText = "مقتنيات وخزائن حفظ العينات في معشبة جامعة الأميرة نورة";
                ltCaption.Text = "معشبة جامعة الأميرة نورة بنت عبدالرحمن — كلية العلوم.";
            }
        }
    }
}
