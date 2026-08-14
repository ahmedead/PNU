using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbariumContact : HerbariumSectionBase
    {
        protected override void BindSectionData(SPWeb web)
        {
            ltSectionTitle.Text = HerbariumHelper.GetRes("Herbarium_ContactSectionTitle", "التواصل مع المعشبة", "Contact the Herbarium");

            List<SPListItem> contactItems = HerbariumHelper.GetItems(web, HerbariumListNames.HerbariumContact);
            if (contactItems != null && contactItems.Count > 0)
            {
                SPListItem item = contactItems[0];
                string title = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Title"), HerbariumHelper.SafeString(item, "Title_EN")));
                string desc = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "Description"), HerbariumHelper.SafeString(item, "Description_EN")));
                string email = HerbariumHelper.SafeString(item, "Email");
                string phone = HerbariumHelper.SafeString(item, "Phone");
                string iconClass = HerbariumHelper.Icon(HerbariumHelper.SafeString(item, "IconClass"), "hgi hgi-stroke hgi-leaf-02 fs-3");
                string btnText = HerbariumHelper.Enc(HerbariumHelper.Pick(HerbariumHelper.SafeString(item, "ButtonText"), HerbariumHelper.SafeString(item, "ButtonText_EN")));

                ltContactTitle.Text = title;
                ltContactDesc.Text = desc;
                ltContactIcon.Text = iconClass;

                phEmail.Visible = !string.IsNullOrEmpty(email);
                ltEmailLink.Text = HerbariumHelper.Enc(email);
                ltEmailText.Text = HerbariumHelper.Enc(email);
                ltButtonEmailLink.Text = HerbariumHelper.Enc(email);

                phPhone.Visible = !string.IsNullOrEmpty(phone);
                ltPhoneLink.Text = HerbariumHelper.Enc(phone);
                ltPhoneText.Text = HerbariumHelper.Enc(phone);

                ltButtonText.Text = string.IsNullOrEmpty(btnText) ? HerbariumHelper.GetRes("Herbarium_ContactUs", "تواصل معنا", "Contact Us") : btnText;
            }
            else
            {
                ltContactTitle.Text = HerbariumHelper.GetRes("Herbarium_ContactTitle", "للتعاون والزيارات العلمية", "For Collaboration and Scientific Visits");
                ltContactDesc.Text = HerbariumHelper.GetRes("Herbarium_ContactDesc", "يمكن التواصل مع معشبة كلية العلوم للاستفسار عن العينات أو الزيارات أو فرص التعاون العلمي.", "You can contact the College of Science Herbarium...");
                ltContactIcon.Text = "hgi hgi-stroke hgi-leaf-02 fs-3";

                ltEmailLink.Text = "cs-herbarium@pnu.edu.sa";
                ltEmailText.Text = "cs-herbarium@pnu.edu.sa";
                ltButtonEmailLink.Text = "cs-herbarium@pnu.edu.sa";
                phEmail.Visible = true;

                ltPhoneLink.Text = "0118235980";
                ltPhoneText.Text = "0118235980";
                phPhone.Visible = true;

                ltButtonText.Text = HerbariumHelper.GetRes("Herbarium_ContactUs", "تواصل معنا", "Contact Us");
            }
        }
    }
}
