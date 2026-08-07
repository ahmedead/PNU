using Microsoft.SharePoint;
using Portal.Main.Helper;
using Portal.Main.WebApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts
{
    public static class busclsAllFaculties
    {
        public static AllFaculties GetAllFaculiesByCode(string Code)
        {

            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                          <Eq>
                             <FieldRef Name='Code' />
                             <Value Type='Text'>" + Code + @"</Value>
                          </Eq>
                       </Where>");

                List<AllFaculties> _Data = SPFactory.GetAllItemsByQuery<AllFaculties>("Admin", Settings.AllFaculties, query);
                if (_Data != null && _Data.Count > 0)
                    return _Data[0];




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsAllFaculties - GetAllFaculiesByCode", ex.Message);
            }

            
            return null;



        }
    }

    public class AllFaculties
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string ClassName { get; set; }
        //public string FacultyName { get; set; }
        public string Category { get; set; }
        //public string Description { get; set; }
        
        public string DescriptionDisplay
        {
            get
            {
                
                if (_description == null)
                {
                    return string.Empty;
                }

                
                return _description.Length > 600 ? _description.Substring(0, 300) : _description;
            }
            
        }
        public string DescriptionDisplay_EN
        {
            get
            {

                if (_description_EN == null)
                {
                    return string.Empty;
                }


                return _description_EN.Length > 300 ? _description_EN.Substring(0, 300) : _description_EN;
            }
            set
            {

                _description_EN = value;
            }
        }

        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description;
            }
            set
            {

                _description = value;
            }
        }

        //public string Description_EN { get; set; }

        private string _description_EN;

        public string Description_EN
        {
            get
            {
                
                if (_description_EN == null)
                {
                    return string.Empty;
                }

                
                return  _description_EN;
            }
            set
            {
                
                _description_EN = value;
            }
        }
       
        private string _ImageUrl;

        public string ImageUrl
        {
            get
            {

                if (_ImageUrl == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }
        public string LinkUrl { get; set; }
        public string ItemOrder { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        private string _Speech;

        public string Speech
        {
            get
            {
                
                if (_Speech == null)
                {
                    return string.Empty;
                }

                
                return _Speech;
            }
            set
            {
                
                _Speech = value;
            }
        }
        private string _Speech_EN;

        public string Speech_EN
        {
            get
            {
                
                if (_Speech_EN == null)
                {
                    return string.Empty;
                }

                
                return _Speech_EN;
            }
            set
            {
                
                _Speech_EN = value;
            }
        }

        private string _PublishingRollupImage;

        public string PublishingRollupImage
        {
            get
            {
                
                if (_PublishingRollupImage == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _PublishingRollupImage;
            }
            set
            {
                _PublishingRollupImage = value;
            }
        }

        public string ID { get; set; }

        public string COLL_CLASS_EN { get; set; }
        public string COLL_CLASS_AR { get; set; }

        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                {
                    
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }

            }


        }




        


    }



    public class AllFacultyDepartments
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
       
        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return  _description;
            }
            set
            {

                _description = value;
            }
        }

        private string _description_EN;

        public string Description_EN
        {
            get
            {

                if (_description_EN == null)
                {
                    return string.Empty;
                }


                return  _description_EN;
            }
            set
            {

                _description_EN = value;
            }
        }

        private string _ImageUrl;

        public string ImageUrl
        {
            get
            {

                if (_ImageUrl == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }
        public string LinkUrl { get; set; }
        public string ItemOrder { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        private string _Speech;

        public string Speech
        {
            get
            {

                if (_Speech == null)
                {
                    return string.Empty;
                }


                return _Speech;
            }
            set
            {

                _Speech = value;
            }
        }
        private string _Speech_EN;

        public string Speech_EN
        {
            get
            {

                if (_Speech_EN == null)
                {
                    return string.Empty;
                }


                return _Speech_EN;
            }
            set
            {

                _Speech_EN = value;
            }
        }

        private string _PublishingRollupImage;

        public string PublishingRollupImage
        {
            get
            {

                if (_PublishingRollupImage == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _PublishingRollupImage;
            }
            set
            {
                _PublishingRollupImage = value;
            }
        }

        public string ID { get; set; }

        public string COLL_CLASS_EN { get; set; }
        public string COLL_CLASS_AR { get; set; }

        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                {

                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }

            }


        }

        public string COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string COLL_DESC_EN { get; set; }








    }

    public class GRPBannerMapping
    {
        public string Title { get; set; }
        public string GRPDeptCode { get; set; }
        public string GRPDEPTName { get; set; }
        public string BannerDeptCode { get; set; }
        public string BannerNameAR { get; set; }
        public string BannerNameEN { get; set; }
        public string ID { get; set; }
    }

}

