using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts
{
    public  class clsSlider
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string Desc { get; set; }
        public string BtnTitle { get; set; }
        public string BtnURL { get; set; }
        public string VideoTitle { get; set; }
        public string VideoURL { get; set; }
        public string ClassName { get; set; }
        public string ImageURL { get; set; }

        public string PublishingRollupImage { get; set; }
        public string MobileImgURL { get; set; }

        public string MobileImageURL
        {
            get
            {

                if (MobileImgURL == null)
                {
                    return PublishingRollupImage;
                }
                else
                    return MobileImgURL;

            }

        }

        public string ResizedImageUrl { get; set; }

        public string TabletImgURL { get; set; }

        public string TabletImageURL
        {
            get
            {

                if (TabletImgURL == null)
                {
                    return PublishingRollupImage;
                }
                else
                    return TabletImgURL;

            }

        }

        public string VideoHTML
        {
            get
            {

                if (VideoURL == null)
                {
                    return string.Empty;
                }
                string HTML = $@"<video id=""vplayerZZ"" class=""d-block w-100 rounded-4 object-fit-cover px-md-0 px-3 h-100"" playsinline autoplay muted loop>
                            <source src=""{VideoURL}"" controls type=""video/mp4"">
                            Your browser does not support the video tag.
                        </video>";
                return HTML;
            }
            
        }
    }

    public static class busclsSlider
    {
        public static List<clsSlider> GetAllItems()
        {
            //List<clsSlider> _AllItems = SPFactory.GetAllItems<clsSlider>(Settings.SliderList);
            return SPFactory.GetAllItems<clsSlider>(Settings.SliderList, PortalHelper.ParentLangSite);


        }
    }


    public class clsServices
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string Desc { get; set; }
        public string BtnTitle { get; set; }
        public string BtnURL { get; set; }
        public string VideoTitle { get; set; }
        public string VideoURL { get; set; }
        public string ClassName { get; set; }
        public string ImageURL { get; set; }

        public string PublishingRollupImage { get; set; }


    }

    public static class busclsServices
    {
        public static List<clsServices> GetAllItems()
        {
            //List<clsSlider> _AllItems = SPFactory.GetAllItems<clsSlider>(Settings.SliderList);
            return SPFactory.GetAllItems<clsServices>(Settings.SliderList, PortalHelper.ParentLangSite);


        }

        
    }
}
