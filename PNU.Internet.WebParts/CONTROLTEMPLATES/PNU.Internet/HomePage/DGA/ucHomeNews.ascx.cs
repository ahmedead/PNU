using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeNews : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string RowsCount { get; set; } = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindNews();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }



        }

        
        private void BindNews()
        {
            try
            {
                List<clsLookUP> MainCategory = busclsRequestsList.GetAllItemsLookup("MainCategory");
                
                List<clsAllRequestsList> _allCategories = new List<clsAllRequestsList>();
                int i = 1;

                if (MainCategory != null && MainCategory.Count > 0)
                {

                    foreach (clsLookUP c in MainCategory)
                    {
                        clsAllRequestsList objCat = new clsAllRequestsList();
                        objCat.ID = i.ToString();
                        i = i + 1;
                        objCat.MainCategory = c.Title;
                        objCat.MainCategory_EN = c.TitleEn;

                        List<clsRequestsList> _aalDataByLevel = new List<clsRequestsList>();
                        _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategory(objCat.MainCategory, RowsCount.ToInt());

                        if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                        {
                            objCat.Requests = new List<clsRequestsList>();
                            objCat.Requests = new List<clsRequestsList>();

                            objCat.Requests.AddRange(_aalDataByLevel);
                        }

                        

                        _allCategories.Add(objCat);


                        

                        break;

                    }
                }


                //masterRepeater.DataSource = _allCategories;
                //masterRepeater.DataBind();

                NewsRepeater.DataSource = _allCategories[0].Requests;
                NewsRepeater.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        // PERF 2026-10-06: cards are 400x250 but rendered the original attachment (one was 2.1 MB).
        // List attachments are also outside the BlobCache, so they were never browser-cached.
        // A 480w/800w WebP thumbnail is generated once per image into a document library (BlobCache-covered).
        // Magick.NET is used because System.Drawing cannot decode WebP sources.
        private const string ThumbLibrary = "SliderImages";
        private const string ThumbFolder = "news-thumbs";
        private static readonly uint[] ThumbWidths = { 480u, 800u };

        /// <summary>src for the card image: the 480w thumbnail, or the original if no thumbnail could be made.</summary>
        protected string GetThumbSrc(object attachmentUrl)
        {
            string[] thumbs = GetThumbs(Convert.ToString(attachmentUrl));
            return thumbs != null ? thumbs[0] : Convert.ToString(attachmentUrl);
        }

        /// <summary>srcset for the card image.</summary>
        protected string GetThumbSrcSet(object attachmentUrl)
        {
            string[] thumbs = GetThumbs(Convert.ToString(attachmentUrl));
            if (thumbs == null) return Convert.ToString(attachmentUrl);
            return thumbs[0] + " " + ThumbWidths[0] + "w, " + thumbs[1] + " " + ThumbWidths[1] + "w";
        }

        private static string[] GetThumbs(string sourceUrl)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl) || !sourceUrl.StartsWith("/")) return null;

            string cacheKey = "PNU.DGA.NewsThumb:" + sourceUrl;
            object cached = HttpRuntime.Cache[cacheKey];
            if (cached != null) return cached as string[]; // string.Empty marks a recent failure

            string[] result = null;
            try
            {
                result = EnsureThumbs(sourceUrl);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(sourceUrl, "ucHomeNews thumbnail", ex.Message);
            }

            // Success is stable - keep it long. Failure is retried after 30 minutes, not on every request.
            HttpRuntime.Cache.Insert(cacheKey, (object)result ?? string.Empty, null,
                DateTime.UtcNow.Add(result != null ? TimeSpan.FromHours(12) : TimeSpan.FromMinutes(30)),
                System.Web.Caching.Cache.NoSlidingExpiration);
            return result;
        }

        private static string[] EnsureThumbs(string sourceUrl)
        {
            string hash;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(sourceUrl)))
                    .Replace("-", "").Substring(0, 16).ToLowerInvariant();
            }

            string[] result = null;
            Guid siteId = SPContext.Current.Site.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = site.OpenWeb("ar"))
                {
                    SPList lib = web.Lists.TryGetList(ThumbLibrary);
                    if (lib == null) return;

                    string folderUrl = lib.RootFolder.ServerRelativeUrl.TrimEnd('/') + "/" + ThumbFolder;
                    string[] urls = ThumbWidths.Select(w => folderUrl + "/" + hash + "-" + w + ".webp").ToArray();

                    if (urls.All(u => web.GetFile(u).Exists))
                    {
                        result = urls;
                        return;
                    }

                    // Server-relative attachment URL -> the web that actually holds it.
                    byte[] sourceBytes;
                    using (SPWeb srcWeb = site.OpenWeb(sourceUrl, false))
                    {
                        SPFile src = srcWeb.GetFile(sourceUrl);
                        if (!src.Exists) return;
                        sourceBytes = src.OpenBinary();
                    }

                    SPFolder folder = web.GetFolder(folderUrl);
                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        if (!folder.Exists) folder = lib.RootFolder.SubFolders.Add(ThumbFolder);

                        using (var image = new ImageMagick.MagickImage(sourceBytes))
                        {
                            image.AutoOrient();
                            image.Strip(); // drop EXIF/ICC metadata - often tens of KB in phone photos
                            for (int i = 0; i < ThumbWidths.Length; i++)
                            {
                                using (var copy = image.Clone())
                                {
                                    // Width-bound, keep aspect ratio, never upscale.
                                    copy.Resize(new ImageMagick.MagickGeometry(ThumbWidths[i], 0) { Greater = true });
                                    copy.Quality = 75;
                                    using (var ms = new MemoryStream())
                                    {
                                        copy.Write(ms, ImageMagick.MagickFormat.WebP);
                                        folder.Files.Add(urls[i], ms.ToArray(), true);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                    result = urls;
                }
            });
            return result;
        }


    }
}
