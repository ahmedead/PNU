using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeBanner : UserControl
    {
        public string ListName { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string RowsCount { get; set; } = "0";

        // PERF 2026-10-06: the slide list used to be rebuilt on every request - 8 elevated SPSite/SPWeb opens,
        // ~16 SPFile.Exists round-trips, and for every .webp source a full read + failed GDI+ decode
        // (System.Drawing cannot decode WebP, so hero-original.jpg was never written and the attempt repeated forever).
        // Measured: home page server time ~900-1300 ms vs ~200 ms for other pages. The built list is now cached.
        private const int CacheMinutes = 10;
        private static readonly string[] NonGdiFormats = { ".webp", ".avif", ".svg" };

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string cacheKey = "PNU.DGA.HomeBanner:" + SPContext.Current.Web.ID + ":" + RowsCount;
                    List<clsSlider> _AllItems = HttpRuntime.Cache[cacheKey] as List<clsSlider>;

                    if (_AllItems == null)
                    {
                        _AllItems = BuildSlides();
                        if (_AllItems.Count > 0)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, _AllItems, null,
                                DateTime.UtcNow.AddMinutes(CacheMinutes), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }

                    rptSlider.DataSource = _AllItems;
                    rptSlider.DataBind();

                    rptIndicators.DataSource = _AllItems;
                    rptIndicators.DataBind();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        private List<clsSlider> BuildSlides()
        {
            List<clsSlider> _AllItems = busclsSlider.GetAllItems() ?? new List<clsSlider>();

            if (_AllItems.Count > 0)
                _AllItems[0].ClassName = " active";
            int limit;
            if (int.TryParse(RowsCount, out limit) && limit > 0)
            {
                // 2. Take only the specified number of rows
                _AllItems = _AllItems.Take(limit).ToList();
            }

            if (_AllItems.Count == 0) return _AllItems;

            Guid siteId = SPContext.Current.Site.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                // One SPSite/SPWeb for all slides instead of one per slide.
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb("ar"))
                    {
                        string targetLib = "SliderImages";

                        foreach (var slider in _AllItems)
                        {
                            try
                            {
                                // --- Existing AVIF/WebP variant generation ---
                                string checkUrl = string.Format("{0}/{1}/hero-lg.avif", targetLib, slider.ID);
                                SPFile checkFile = web.GetFile(checkUrl);
                                if (!checkFile.Exists)
                                {
                                    ImageAutomation.ConvertAndUploadToLibrary(web, slider.PublishingRollupImage, targetLib, slider.ID);
                                }

                                // --- Fit-resized fallback (1920x492 max, no crop) ---
                                // GDI+ cannot decode WebP/AVIF/SVG: use the source as-is instead of retrying a resize that always fails.
                                string ext = Path.GetExtension((slider.PublishingRollupImage ?? string.Empty).Split('?')[0]).ToLowerInvariant();
                                if (NonGdiFormats.Contains(ext))
                                {
                                    slider.ResizedImageUrl = slider.PublishingRollupImage;
                                    continue;
                                }

                                string resizedFallbackUrl = string.Format("/{0}/{1}/{2}/hero-original.jpg",
                                    web.ServerRelativeUrl.Trim('/'), targetLib, slider.ID).Replace("//", "/");

                                SPFile resizedFile = web.GetFile(resizedFallbackUrl);
                                if (!resizedFile.Exists)
                                {
                                    string saved = HeroImageResizer.ResizeAndSaveFit(
                                        web, slider.PublishingRollupImage, targetLib, slider.ID);
                                    slider.ResizedImageUrl = saved ?? slider.PublishingRollupImage;
                                }
                                else
                                {
                                    slider.ResizedImageUrl = resizedFile.ServerRelativeUrl;
                                }
                            }
                            catch (Exception ex)
                            {
                                // One bad slide must not drop the whole carousel.
                                slider.ResizedImageUrl = slider.PublishingRollupImage;
                                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHomeBanner slide " + slider.ID, ex.Message);
                            }
                        }
                    }
                }
            });

            return _AllItems;
        }


    }
}
