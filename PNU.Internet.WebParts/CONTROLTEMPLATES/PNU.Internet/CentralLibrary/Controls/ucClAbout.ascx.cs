using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClAbout : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Numbers; }
        }

        public ClAboutModel Model { get; private set; }

        public ucClAbout()
        {
            Model = new ClAboutModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Load About metadata from ClAbout list
                ClListProvisioner.EnsureListExists(ClListNames.About);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = ClTargetWeb.Open(site))
                {
                    if (web != null)
                    {
                        List<SPListItem> rawAbout = ClHelper.GetItems(web, ClListNames.About);
                        if (rawAbout.Count > 0)
                        {
                            SPListItem item = rawAbout[0];
                            Model.Title = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Title"), ClHelper.SafeString(item, "Title_EN")));
                            Model.Paragraph1 = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Paragraph1"), ClHelper.SafeString(item, "Paragraph1_EN")));
                            Model.Paragraph2 = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Paragraph2"), ClHelper.SafeString(item, "Paragraph2_EN")));
                            Model.ImageUrlSm = ClHelper.Enc(ClHelper.SafeUrl(item, "ImageUrlSm"));
                            Model.ImageUrlMd = ClHelper.Enc(ClHelper.SafeUrl(item, "ImageUrlMd"));
                            Model.ImageUrlLg = ClHelper.Enc(ClHelper.SafeUrl(item, "ImageUrlLg"));
                            Model.ImageAlt = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "ImageAlt"), ClHelper.SafeString(item, "ImageAlt_EN")));
                            Model.NumbersTitle = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "NumbersTitle"), ClHelper.SafeString(item, "NumbersTitle_EN")));
                        }
                    }
                }

                // Load stat numbers
                LoadItems();
                if (rptNumbers != null)
                {
                    rptNumbers.DataSource = Items;
                    rptNumbers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClAbout.Page_Load", ex);
            }
        }
    }
}
