using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SharePoint;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public static class HeroImageResizer
    {
        private const int MaxWidth = 1920;
        private const int MaxHeight = 492;

        /// <summary>
        /// Reads the source image (from a server-relative URL or absolute URL),
        /// resizes it using FIT semantics (preserve aspect ratio, no crop) so it 
        /// fits within 1920x492, and saves it as JPEG to:
        ///   {targetLibrary}/{itemId}/hero-original.jpg
        /// Returns the server-relative URL of the saved file, or null on failure.
        /// </summary>
        public static string ResizeAndSaveFit(SPWeb web, string sourceImageUrl, string targetLibrary, string itemId)
        {
            if (string.IsNullOrWhiteSpace(sourceImageUrl)) return null;

            try
            {
                // 1. Load source bytes from SharePoint (handles server-relative URLs)
                byte[] sourceBytes = LoadImageBytes(web, sourceImageUrl);
                if (sourceBytes == null || sourceBytes.Length == 0) return null;

                // 2. Resize in memory
                byte[] resizedBytes;
                using (var inStream = new MemoryStream(sourceBytes))
                using (var original = Image.FromStream(inStream))
                {
                    Size newSize = CalculateFitSize(original.Width, original.Height, MaxWidth, MaxHeight);

                    using (var resized = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
                    {
                        resized.SetResolution(original.HorizontalResolution, original.VerticalResolution);

                        using (var g = Graphics.FromImage(resized))
                        {
                            g.CompositingMode = CompositingMode.SourceCopy;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawImage(original, 0, 0, newSize.Width, newSize.Height);
                        }

                        using (var outStream = new MemoryStream())
                        {
                            // Save as JPEG, quality 85 (good balance for hero banners)
                            var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                            var encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                            resized.Save(outStream, jpegEncoder, encoderParams);
                            resizedBytes = outStream.ToArray();
                        }
                    }
                }

                // 3. Upload to SharePoint: {targetLibrary}/{itemId}/hero-original.jpg
                string targetFolderUrl = string.Format("{0}/{1}/{2}", web.ServerRelativeUrl.TrimEnd('/'), targetLibrary, itemId);
                EnsureFolder(web, targetLibrary, itemId);

                string targetFileUrl = targetFolderUrl + "/hero-original.jpg";
                web.AllowUnsafeUpdates = true;
                SPFile uploaded = web.Files.Add(targetFileUrl, resizedBytes, true); // overwrite=true
                web.AllowUnsafeUpdates = false;

                return uploaded.ServerRelativeUrl;
            }
            catch (Exception ex)
            {
                // Use your existing logger if you have one; falling back to ULS-style write
                System.Diagnostics.Trace.WriteLine("HeroImageResizer.ResizeAndSaveFit failed: " + ex);
                return null;
            }
        }

        private static Size CalculateFitSize(int srcW, int srcH, int maxW, int maxH)
        {
            // Fit: scale down so the whole image fits within maxW x maxH; never upscale
            double ratio = Math.Min((double)maxW / srcW, (double)maxH / srcH);
            if (ratio >= 1.0)
            {
                // Source is already smaller than the bounding box — keep original size
                return new Size(srcW, srcH);
            }
            int newW = (int)Math.Round(srcW * ratio);
            int newH = (int)Math.Round(srcH * ratio);
            return new Size(Math.Max(1, newW), Math.Max(1, newH));
        }

        private static byte[] LoadImageBytes(SPWeb web, string url)
        {
            // Server-relative path -> read directly from SharePoint
            if (url.StartsWith("/"))
            {
                SPFile file = web.Site.RootWeb.GetFile(url);
                if (file != null && file.Exists)
                    return file.OpenBinary();
            }
            else if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                // Absolute URL — try as SP file first, fall back to web request
                try
                {
                    Uri u = new Uri(url);
                    SPFile file = web.Site.RootWeb.GetFile(u.AbsolutePath);
                    if (file != null && file.Exists) return file.OpenBinary();
                }
                catch { /* fall through */ }

                using (var wc = new WebClient { UseDefaultCredentials = true })
                {
                    return wc.DownloadData(url);
                }
            }
            return null;
        }

        private static void EnsureFolder(SPWeb web, string library, string subFolder)
        {
            SPFolder libFolder = web.GetFolder(library);
            if (!libFolder.Exists) return;

            web.AllowUnsafeUpdates = true;
            if (!libFolder.SubFolders.OfType<SPFolder>().Any(f => f.Name.Equals(subFolder, StringComparison.OrdinalIgnoreCase)))
            {
                libFolder.SubFolders.Add(subFolder);
            }
            web.AllowUnsafeUpdates = false;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (var codec in ImageCodecInfo.GetImageEncoders())
                if (codec.FormatID == format.Guid) return codec;
            return null;
        }
    }

}



