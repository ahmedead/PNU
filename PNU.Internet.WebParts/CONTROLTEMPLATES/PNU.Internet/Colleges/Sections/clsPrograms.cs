using Microsoft.SharePoint;
using PNU.Internet.WebParts;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections
{
    public static class busclsPrograms
    {

        public static AllPrograms GetNAllProgramMainDataByProgramCode(string ProgramCode)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='Prog_Code' />
                                 <Value Type='Text'>" + ProgramCode + @"</Value>
                              </Eq>
                           </Where>");

                List<AllPrograms> _Data = SPFactory.GetAllItemsByQuery<AllPrograms>("Admin", Settings.AllPrograms, query);
                if (_Data != null && _Data.Count > 0)
                    return _Data[0];




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsPrograms - GetNAllProgramMainDataByProgramCode", ex.Message);
            }

            
            return null;



        }

    }


    public class AllPrograms
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }

        public string ProgramNature_EN { get; set; }
        public string ProgramFields_EN { get; set; }

        public string ProgramDegree_EN { get; set; }

        private string _major;

        public string Major
        {
            get
            {

                if (_major == null)
                {
                    return SPFactory.GetLocalizedTitle(Dept_Title, Dept_Title_EN);
                }


                return _major;
            }
            set
            {

                _major = value;
            }
        }


        private string _major_EN;

        public string Major_EN
        {
            get
            {

                if (_major_EN == null)
                {
                    return SPFactory.GetLocalizedTitle(Dept_Title, Dept_Title_EN);
                }


                return _major_EN;
            }
            set
            {

                _major_EN = value;
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



  

        public string LinkUrl { get; set; }
        public string ItemOrder { get; set; }
        public string PublishingRollupImage { get; set; }

        public string Code { get; set; }

        public string Sec_Code { get; set; }
        public string Coll_Code { get; set; }
        public string Coll_Title { get; set; }
        public string Coll_Title_EN { get; set; }
        public string Dept_Code { get; set; }
        public string Dept_Title { get; set; }
        public string Dept_Title_EN { get; set; }
        public string Prog_Code { get; set; }
        public string Prog_Title { get; set; }
        public string ID { get; set; }
        public string ProgramNature { get; set; }
        public string ProgramFields { get; set; }
        public string SecondaryType { get; set; }

        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                    return "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg";

            }


        }

        

    }

    public class AllDepartmentPrograms
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


                return _description.Length > 600 ? _description.Substring(0, 600) : _description;
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


                return _description_EN.Length > 600 ? _description_EN.Substring(0, 600) : _description_EN;
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

        public string DEPT_CODE { get; set; }
        public string DEPT_DESC { get; set; }
        public string DEPT_DESC_EN { get; set; }


        public string SOBCURR_DEGC_CODE { get; set; }
        public string DEGC_DSC { get; set; }





    }

    public class AllProgramsRepeater
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Title_EN
        {
            get
            {
                string _Title = "";
                if (!string.IsNullOrEmpty(Title))
                {
                    if (Title == "البكالوريوس")
                        _Title = "Bachelor's degree";
                    else if (Title == "الماجستير")
                        _Title = "Master's degree";
                    else if (Title == "الدكتوراه")
                        _Title = "Ph.D.";
                    else 
                        _Title = Title;

                }

                return _Title;
            }
        }
        public List<AllDepartmentPrograms> Programs { get; set; }
    }
    public class AllPLANS_ELECMain
    {
        public string LevelCode { get; set; }
        public string LevelDesc { get; set; }
        
        public List<PLANS_ELEC_U> StudyPlan { get; set; }
        public List<PLANS_ELEC_C> StudyPlanC { get; set; }
        public List<PLANS_ELEC_P> StudyPlanP { get; set; }
    }

    public class ProgramsPageMain
    {
        public string ProgramDegree { get; set; }
        public string Major { get; set; }
        public string Major_EN { get; set; }
        public string ProgramDegree_EN { get; set; }
        public string ProgramDesc { get; set; }
        public string ProgramDesc_EN { get; set; }
        public string ProgramNature { get; set; }
        public string ProgramFields { get; set; }
        public string SecondaryType { get; set; }
        public string ProgramYears { get; set; }
        public string ProgramLanguage { get; set; }
        public string CollegeCode { get; set; }
        public string CollegeName { get; set; }
        public string CollegeName_EN { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName_EN { get; set; }
        public string ProgramCode { get; set; }        
        public string ProgramName { get; set; }
        public string ProgramName_EN { get; set; }
        public string ProgramNature_EN { get; set; }
        public string ProgramFields_EN { get; set; }
        public string SecondaryTypeBadgesHtml
        {
            get
            {
                return SPFactory.GetSecondaryTypeBadges(SecondaryType);
            }
        }
    }



    public class AllProgramsMain
    {
        public string LevelCode { get; set; }
        public string COLL_CODE { get; set; }
        public string LevelDesc
        {
            get
            {
                if (LevelCode != null && LevelCode != "")
                {
                    if(!string.IsNullOrEmpty(COLL_CODE))
                    {
                        if(COLL_CODE == "MD" || COLL_CODE == "DN")
                        {
                            switch (LevelCode)
                            {

                                case "-1":
                                    return SPFactory.GetPNUresResource("FirstYear-1");
                                case "-2":
                                    return SPFactory.GetPNUresResource("SecondYear-2");
                                case "1":    
                                    return SPFactory.GetPNUresResource("FirstYear");
                                case "2":
                                    return SPFactory.GetPNUresResource("SecondYear");
                                case "3":
                                    return SPFactory.GetPNUresResource("ThirdYear");

                                case "4":
                                    return SPFactory.GetPNUresResource("FourthYear");
                                case "5":
                                    return SPFactory.GetPNUresResource("FifthYear");

                                case "6":
                                    return SPFactory.GetPNUresResource("SixthYear");
                                case "7":
                                    return SPFactory.GetPNUresResource("SeventhYear");
                                case "8":
                                    return SPFactory.GetPNUresResource("Level8");
                                case "9":
                                    return SPFactory.GetPNUresResource("Level9");

                                case "10":
                                    return SPFactory.GetPNUresResource("Level10");
                                case "11":
                                    return SPFactory.GetPNUresResource("Level11");

                                default:
                                    return LevelCode.ToString();
                            }
                        }
                    }
                    switch (LevelCode)
                    {
                        case "-1":
                            return SPFactory.GetPNUresResource("FirstYear-1");
                        case "-2":
                            return SPFactory.GetPNUresResource("SecondYear-2");
                        case "1":
                            return SPFactory.GetPNUresResource("Level1");

                        case "2":
                            return SPFactory.GetPNUresResource("Level2");
                        case "3":
                            return SPFactory.GetPNUresResource("Level3");

                        case "4":
                            return SPFactory.GetPNUresResource("Level4");
                        case "5":
                            return SPFactory.GetPNUresResource("Level5");

                        case "6":
                            return SPFactory.GetPNUresResource("Level6");
                        case "7":
                            return SPFactory.GetPNUresResource("Level7");

                        case "8":
                            return SPFactory.GetPNUresResource("Level8");
                        case "9":
                            return SPFactory.GetPNUresResource("Level9");

                        case "10":
                            return SPFactory.GetPNUresResource("Level10");
                        case "11":
                            return SPFactory.GetPNUresResource("Level11");
                        default:
                            return LevelCode.ToString();
                    }
                }
                return "";
            }


        }
        public List<NewStudyPlanDto> StudyPlan { get; set; }
    }



        
    }

    public class busclsNewStudyPlan
    {
    public static List<NewStudyPlanDto> GetNewStudyPlanByProgramCode(string ProgramCode)
    {
        try
        {
            SPQuery query = new SPQuery();
            query.Query = string.Concat(
                             @"<Where>
                              <Eq>
                                 <FieldRef Name='PROGRAM' />
                                 <Value Type='Text'>" + ProgramCode + @"</Value>
                              </Eq>
                           </Where>");

            List<NewStudyPlanDto> _Data = SPFactory.GetAllItemsByQueryListName<NewStudyPlanDto>("Admin", Settings.NewStudyPlanListOnly, query);
            if (_Data != null && _Data.Count > 0)
                return _Data;



        }

        catch (Exception ex)
        {
            Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsNewStudyPlan - GetNewStudyPlanByProgramCode", ex.Message);
        }
        

        return null;



    }
}

