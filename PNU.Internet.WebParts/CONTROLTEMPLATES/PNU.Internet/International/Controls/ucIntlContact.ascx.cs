
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    public partial class ucIntlContact : IntlSectionBase
    {
        protected override string ListName { get { return IntlListNames.Contact; } }

        /// <summary>Target of the "Send a message" button. Set "" to hide the button.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ContactPageUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }
        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }
        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = IntlHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                rptContact.DataSource = Items;
                rptContact.DataBind();

                // Language-aware default. Build it here, not in the markup:
                // <%= %> is not allowed inside a runat="server" attribute.
                string ctaUrl = string.IsNullOrEmpty(ContactPageUrl)
                    ? "/" + (IsArabic ? "ar" : "en") + "/Pages/ContactUsForm.aspx"
                    : ContactPageUrl;
                divContactCta.Visible = !string.IsNullOrEmpty(ctaUrl);
                lnkSendMessage.NavigateUrl = ctaUrl;
                lnkSendMessage.Text = IntlHelper.GetRes("Intl_SendMessage", "إرسال رسالة", "Send a message");

                secContact.Visible = Items.Count > 0 || divContactCta.Visible;
            }
            catch (Exception ex)
            {
                
                secContact.Visible = false;
            }
        }
    }
}
