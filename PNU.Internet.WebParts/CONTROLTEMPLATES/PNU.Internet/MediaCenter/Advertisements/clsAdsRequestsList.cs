using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
   
    public class clsAdsRequestsList
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string MediaContent { get; set; }
        public string MediaContent_EN { get; set; }
        public string FacultyName { get; set; }
        public string FacultyName_EN { get; set; }
        public bool IsVideo { get; set; } = false;
        public bool IsHome { get; set; } = false;



        public string VideoVisiable
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }


        public string MP4Visible
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("mp4"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string ImgVisible
        {
            get
            {

                if (AttachmentURL == null)
                {
                    return "none";
                }

                return "block";
            }
        }

        public string YouTubeVisible
        {
            get
            {

                if (_VideoURL == null)
                {
                    return "none";
                }
                else if (_VideoURL.ToLower().Contains("youtube"))
                {
                    return "block";
                }
                return "none";
            }
        }

        public string TitleVisible
        {
            get
            {

                if (MediaContent == null)
                {
                    return "none";
                }

                return "block";
            }
        }
        //public string VideoURL { get; set; }


        private string _VideoURL;

        public string VideoURL
        {
            get
            {

                if (_VideoURL == null)
                {
                    return string.Empty;
                }

                return _VideoURL;
            }
            set
            {

                _VideoURL = value;
            }
        }

        public string Created { get; set; }
        //Created
        //CreatedDate
        public string CreatedDate
        {
            get
            {

                if (Created == null)
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(Created.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }

        }

       

       

        public string Author { get; set; }

        private string _MediaDate;

        public string MediaDate
        {
            get
            {

                if (_MediaDate == null)
                {
                    return string.Empty;
                }


                DateTime dtDate = Convert.ToDateTime(_MediaDate.ToString());
                string strDate = "";
                if (PortalHelper.IsArabic)
                    strDate = dtDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                else
                    strDate = dtDate.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));

                return strDate;
            }
            set
            {

                _MediaDate = value;
            }
        }
        public string UserComments { get; set; }
        public string RequestStatus { get; set; }
        public string ApprovalGroupName { get; set; }
        public string RequesterName { get; set; }

        public string RequesterNameDisplay
        {
            get
            {
                if (this.RequesterName == null || this.RequesterName == "")
                    return "";

                else
                    return RequesterName;

            }
        }
        public string RequesterEmail { get; set; }
        public string AttachmentURL { get; set; }

        public string ClassActive
        {
            get
            {
                if (this.ID == null || this.ID == "")
                    return "";
                if (this.ID == "1")
                    return " active show";
                else
                    return "";

            }
        }

      
    }

}
