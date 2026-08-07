using ImageMagick;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    public static class ImageAutomation
    {
        // Hero variant target sizes (max bounds, fit semantics — never upscale, preserve aspect ratio)
        private static readonly (string Name, uint MaxWidth, uint MaxHeight)[] HeroSizes = new[]
        {
            ("sm", 768u,  197u),   // ~62% of md, matches < 768px breakpoint
            ("md", 1200u, 308u),   // ~62% of lg, matches 768–1199px breakpoint
            ("lg", 1920u, 492u)    // matches >= 1200px breakpoint
        };

        public static void GenerateAllFormats(string sourceImagePath, string targetPhysicalFolder)
        {
            if (!Directory.Exists(targetPhysicalFolder))
                Directory.CreateDirectory(targetPhysicalFolder);

            MagickNET.Initialize();

            using (var image = new MagickImage(sourceImagePath))
            {
                foreach (var size in HeroSizes)
                {
                    using (var copy = image.Clone())
                    {
                        // Fit semantics: resize to fit within MaxWidth x MaxHeight,
                        // preserve aspect ratio, never upscale ('>' = Greater flag).
                        var geom = new MagickGeometry(size.MaxWidth, size.MaxHeight)
                        {
                            Greater = true,    // only shrink if larger; do not upscale
                            IgnoreAspectRatio = false
                        };
                        copy.Resize(geom);

                        string basePath = Path.Combine(targetPhysicalFolder, $"hero-{size.Name}");

                        copy.Format = MagickFormat.Avif;
                        copy.Quality = 60;
                        copy.Write(basePath + ".avif");

                        copy.Format = MagickFormat.WebP;
                        copy.Quality = 75;
                        copy.Write(basePath + ".webp");
                    }
                }

                // JPEG fallback (also fit-resized to the largest bound)
                using (var copy = image.Clone())
                {
                    var geom = new MagickGeometry(1920u, 492u) { Greater = true, IgnoreAspectRatio = false };
                    copy.Resize(geom);
                    copy.Format = MagickFormat.Jpg;
                    copy.Quality = 80;
                    copy.Write(Path.Combine(targetPhysicalFolder, "hero-library-sm.jpg"));
                }
            }
        }

        public static void ConvertAndUploadToLibrary(SPWeb web, string sourceFileUrl, string targetLibraryName, string sliderId)
        {
            // 1. Get the source file from the Library
            SPFile sourceFile = web.GetFile(sourceFileUrl);
            if (!sourceFile.Exists) return;

            // 2. Ensure the Target Folder exists (/TargetLibrary/SliderID/)
            SPList targetList = web.Lists[targetLibraryName];
            SPFolder targetFolder = EnsureFolder(web, targetList, sliderId);

            using (Stream sourceStream = sourceFile.OpenBinaryStream())
            using (var image = new MagickImage(sourceStream))
            {
                foreach (var size in HeroSizes)
                {
                    using (var copy = image.Clone())
                    {
                        // Fit semantics: shrink to fit within MaxWidth x MaxHeight, no upscale, preserve aspect.
                        var geom = new MagickGeometry(size.MaxWidth, size.MaxHeight)
                        {
                            Greater = true,
                            IgnoreAspectRatio = false
                        };
                        copy.Resize(geom);

                        web.AllowUnsafeUpdates = true;
                        UploadToSP(targetFolder, copy, $"hero-{size.Name}.avif", MagickFormat.Avif, 60);
                        UploadToSP(targetFolder, copy, $"hero-{size.Name}.webp", MagickFormat.WebP, 75);
                        UploadToSP(targetFolder, copy, $"hero-{size.Name}.jpg", MagickFormat.Jpg, 80);
                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
        }

        private static void UploadToSP(SPFolder folder, IMagickImage img, string fileName, MagickFormat format, int quality)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Quality = (uint)quality;
                img.Write(ms, format);
                folder.Files.Add(fileName, ms.ToArray(), true); // overwrite=true
            }
        }

        private static SPFolder EnsureFolder(SPWeb web, SPList list, string folderName)
        {
            SPFolder folder = null;
            try
            {
                folder = list.RootFolder.SubFolders[folderName];
            }
            catch { /* Folder doesn't exist */ }

            if (folder == null)
            {
                web.AllowUnsafeUpdates = true;
                folder = list.RootFolder.SubFolders.Add(folderName);
                list.Update();
                web.AllowUnsafeUpdates = false;
            }
            return folder;
        }
    }
}
