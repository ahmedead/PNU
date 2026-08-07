using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// The two top cards of the Contact page: "التواصل مع الجامعة" (channels + national
    /// address + digital platforms) and "تواصل نورة" (channels). Headings and subtitles
    /// come from TwSectionTitles; channels from TwContactChannels grouped by ContactGroup;
    /// platforms from TwSocialLinks; the address from the shared TwLocation list.
    /// </summary>
    public partial class ucTwContactCards : TwSectionBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                // ----- University card -----
                TwCard uni = Section(TwListSchema.KeyUniversity);
                ltUniTitle.Text = uni != null ? uni.Title : string.Empty;
                ltUniSubtitle.Text = uni != null ? uni.SubTitle : string.Empty;

                List<TwCard> uniChannels = ItemsInGroup(TwListNames.ContactChannels, TwListSchema.GroupUniversity);
                rptUniChannels.DataSource = uniChannels;
                rptUniChannels.DataBind();

                TwCard location = LoadList(TwListNames.Location).FirstOrDefault();
                ltUniAddress.Text = location != null ? location.AddressHtml : string.Empty;

                // ----- Digital platforms -----
                List<TwCard> social = LoadList(TwListNames.SocialLinks);
                TwCard platforms = Section(TwListSchema.KeyPlatforms);
                ltPlatforms.Text = platforms != null
                    ? platforms.Title
                    : TwHelper.Enc(TwHelper.Pick("منصاتنا الرقمية", "Our digital platforms"));
                rptSocial.DataSource = social;
                rptSocial.DataBind();
                phPlatforms.Visible = social.Count > 0;

                // ----- Tawasul Nourah card -----
                TwCard nourah = Section(TwListSchema.KeyTawasul);
                ltNourahTitle.Text = nourah != null ? nourah.Title : string.Empty;
                ltNourahSubtitle.Text = nourah != null ? nourah.SubTitle : string.Empty;

                List<TwCard> nourahChannels = ItemsInGroup(TwListNames.ContactChannels, TwListSchema.GroupTawasul);
                rptNourahChannels.DataSource = nourahChannels;
                rptNourahChannels.DataBind();

                secContactCards.Visible = uniChannels.Count > 0 || nourahChannels.Count > 0
                                          || location != null || social.Count > 0;
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwContactCards.Bind", ex);
                secContactCards.Visible = false;
            }
        }
    }
}
