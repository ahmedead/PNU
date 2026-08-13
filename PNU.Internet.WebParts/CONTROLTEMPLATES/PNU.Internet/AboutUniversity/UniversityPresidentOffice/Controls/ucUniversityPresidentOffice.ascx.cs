using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    public partial class ucUniversityPresidentOffice : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = UpoTargetWeb.Open(site))
                        {
                            if (web == null) return;

                            // Ensure lists exist
                            UpoListProvisioner.EnsureAllListsExist();

                            // Load sections
                            var sections = LoadSections(web);
                            if (sections != null && sections.Count > 0)
                            {
                                rptSections.DataSource = sections;
                                rptSections.DataBind();
                            }

                            // Load signature
                            var signature = LoadSignature(web);
                            if (signature != null && signature.Count > 0)
                            {
                                rptSignature.DataSource = signature;
                                rptSignature.DataBind();
                            }

                            // Load contacts
                            var contacts = LoadContacts(web);
                            if (contacts != null && contacts.Count > 0)
                            {
                                rptContacts.DataSource = contacts;
                                rptContacts.DataBind();
                            }

                            // Set headings
                            litSignatureHeading.Text = UpoHelper.Enc(
                                UpoHelper.Pick("رئيسة الجامعة", "University President"));
                            litContactHeading.Text = UpoHelper.Enc(
                                UpoHelper.Pick("تواصل مع مكتب رئيسة الجامعة المُكلَّفة", "Contact the Acting President's Office"));
                            litChannelHeader.Text = UpoHelper.Enc(
                                UpoHelper.Pick("القناة", "Channel"));
                            litContactDataHeader.Text = UpoHelper.Enc(
                                UpoHelper.Pick("بيانات التواصل", "Contact details"));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUniversityPresidentOffice.LoadData", ex);
            }
        }

        private List<UpoSection> LoadSections(SPWeb web)
        {
            var result = new List<UpoSection>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Sections))
            {
                result.Add(new UpoSection
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    Description = UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Description"),
                        UpoHelper.SafeString(item, "Description_EN")),
                    LeadText = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "LeadText"),
                        UpoHelper.SafeString(item, "LeadText_EN"))),
                    IconClass = UpoHelper.Enc(UpoHelper.SafeString(item, "IconClass")),
                    ShowLeadCard = UpoHelper.SafeBool(item, "ShowLeadCard"),
                    ImageUrl = UpoHelper.SafeUrl(item, "ImageUrl"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        private List<UpoContact> LoadContacts(SPWeb web)
        {
            var result = new List<UpoContact>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Contacts))
            {
                result.Add(new UpoContact
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    ContactValue = UpoHelper.SafeString(item, "ContactValue"),
                    ContactType = UpoHelper.SafeString(item, "ContactType"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        private List<UpoSignatureCard> LoadSignature(SPWeb web)
        {
            var result = new List<UpoSignatureCard>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Signature))
            {
                result.Add(new UpoSignatureCard
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    Description = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Description"),
                        UpoHelper.SafeString(item, "Description_EN"))),
                    ImageUrl = UpoHelper.SafeUrl(item, "ImageUrl"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        protected void rptContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var item = (UpoContact)e.Item.DataItem;
            var lnk = (HyperLink)e.Item.FindControl("lnkContact");

            lnk.Text = UpoHelper.Enc(item.ContactValue);
            lnk.NavigateUrl = item.ContactHref;

            switch ((item.ContactType ?? "").ToLower())
            {
                case "phone":
                    lnk.Attributes["dir"] = "ltr";
                    break;
                case "email":
                    lnk.Attributes["dir"] = "ltr";
                    break;
                case "link":
                    lnk.Target = "_blank";
                    lnk.CssClass = "external-link";
                    lnk.Attributes["rel"] = "noopener noreferrer";
                    lnk.Attributes["dir"] = "ltr";
                    break;
            }
        }
    }
}
