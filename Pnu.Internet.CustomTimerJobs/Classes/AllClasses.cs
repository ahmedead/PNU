using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs
{
    public class AllFaculties
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string FacultyName { get; set; }
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


                return _description.Length > 600 ? _description.Substring(0, 600) : _description;
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


                return _description_EN;
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


                return _description;
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


                return _description_EN;
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


    public class StatisticTypes
    {
        private StatisticTypes(string value) { Value = value; }

        public string Value { get; private set; }

        public static StatisticTypes CollegesInstitues { get { return new StatisticTypes("الكليات والمعاهد"); } }
        public static StatisticTypes Programs { get { return new StatisticTypes("البرامج الاكاديمية"); } }
        public static StatisticTypes Students { get { return new StatisticTypes("طالبة"); } }
        public static StatisticTypes Members { get { return new StatisticTypes("أعضاء هيئة التدريس"); } }

        public static StatisticTypes AllDepartments { get { return new StatisticTypes("قسم"); } }


        public override string ToString()
        {
            return Value;
        }

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

    public class CollegeDeptsDto
    {
        public string Coll_Code { get; set; }
        public string Coll_Title { get; set; }
        public string Dept_Code { get; set; }
        public string Dept_Title { get; set; }
        public string Prog_Code { get; set; }
        public string Prog_Title { get; set; }
        public string Major_Code { get; set; }
        public string Major_Title { get; set; }
    }

    public class AllCollegesFromBanner
    {
        public string ID { get; internal set; }
        public string COLL_CLASS_AR { get; internal set; }
        public string COLL_CLASS_EN { get; internal set; }

        public List<AllFaculties> Colleges { get; internal set; }

    }
    public class CollegeCategoryDto
    {

        public string COLL_CLASS_AR { get; internal set; }
        public string COLL_CLASS_EN { get; internal set; }
    }
    public class CollegesDto
    {
        public string COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string COLL_DESC_EN { get; set; }
        public string COLL_CLASS_AR { get; internal set; }
        public string COLL_CLASS_EN { get; internal set; }
    }

    public class DeptsDto
    {
        public string COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string COLL_DESC_EN { get; set; }
        public string COLL_CLASS_AR { get; internal set; }
        public string COLL_CLASS_EN { get; internal set; }
        public string DEPT_CODE { get; internal set; }
        public string DEPT_DESC { get; internal set; }
        public string DEPT_DESC_EN { get; internal set; }
    }


    public class ProgramsDto
    {
        //public string SOBCURR_DEGC_CODE { get; set; }
        //public string DEGC_DSC { get; set; }
        public string COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string COLL_DESC_EN { get; set; }
        public string COLL_CLASS_AR { get; internal set; }
        public string COLL_CLASS_EN { get; internal set; }
        public string DEPT_CODE { get; internal set; }
        public string DEPT_DESC { get; internal set; }
        public string DEPT_DESC_EN { get; internal set; }
        public string PROG_CODE { get; internal set; }
        public string PROG_DESC { get; internal set; }
        public string PROG_DESC_EN { get; internal set; }



    }

    public class CollegeMembersDto
    {
        //public string Title { get; set; }
        //public string Full_Name { get; set; }
        //public string English_Name { get; set; }
        //public string Employee_number { get; set; }
        //public string Email_Address { get; set; }
        //public string Phone_Number { get; set; }
        //public string Extention_Number { get; set; }
        //public string Profession { get; set; }
        //public string College { get; set; }
        //public string Specialization { get; set; }
        //public string Qualification_Name { get; set; }
        //public string Minor { get; set; }
        //public string Nationality { get; set; }
        //public string Section { get; set; }
        //public string Employee_Status { get; set; }
        //public string location_name { get; set; }
        //public string location_code { get; set; }
        //public string floor { get; set; }
        //public string office { get; set; }
        //public int College_Id { get; set; }
        //public int Section_Id { get; set; }



        public string TITLE { get; set; }
        public string FULL_NAME { get; set; }
        public string ENGLISH_NAME { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string EXTENTION_NUMBER { get; set; }
        public string PROFESSION { get; set; }
        public string PROFESSION_EN { get; set; }
        public string GRADE_NAME { get; set; }
        public string GRADE_NAME_EN { get; set; }
        public string COLLEGE_ID { get; set; }
        public string COLLEGE { get; set; }
        public string COLLEGE_EN { get; set; }
        public string SPECIALIZATION { get; set; }
        public string SPECIALIZATION_EN { get; set; }
        public string SPECIAL_CPECIALIZATION { get; set; }
        public string SPECIAL_CPECIALIZATION_EN { get; set; }
        public string QUALIFICATION_NAME { get; set; }
        public string QUALIFICATION_NAME_EN { get; set; }
        public string NATIONALITY { get; set; }
        public string NATIONALITY_EN { get; set; }
        public string SECTION_ID { get; set; }
        public string SECTION { get; set; }
        public string SECTION_EN { get; set; }
        public string SCHOLARSHIP { get; set; }
        public string POSITION_DELEG { get; set; }
        public string POSITION { get; set; }
        public string DELEG_DEPARTMENT { get; set; }
        public string DELEG_START_DATE { get; set; }
        public string DURATION { get; set; }
        public string ORDERDATE { get; set; }
        public string END_DATE { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string TERMINATION_REASON { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LOCATION_NAME_EN { get; set; }
        public string FLOOR { get; set; }
        public string OFFICE { get; set; }
    }


    public class CourseDto
    {
        [Column("code")]
        public string Code { get; set; }
        [Column("numb")]
        public string Numb { get; set; }
        [Column("SCBCRSE_SUBJ_CODE")]
        public string CourseCode { get; set; }

        [Column("SCBCRSE_TITLE")]
        public string CourseTitle { get; set; }
        [Column("REGISTERED_COUNT")]
        public string RegisteredCount { get; set; }
        [Column("SSBSECT_CRN")]
        public string SubjectSectionNo { get; set; }
        [Column("day")]
        public string CourseDays { get; set; }
        [Column("SSRMEET_BEGIN_TIME")]
        public string StartTime { get; set; }
        [Column("SSRMEET_END_TIME")]
        public string EndTime { get; set; }
        [Column("SCBCRSE_CRSE_NUMB")]
        public string CourseNumber { get; set; }
        [Column("SCBCRSE_COLL_CODE")]
        public string CollegeCode { get; set; }
        [Column("COLL_DESC")]
        public string CollegeDesc { get; set; }
        [Column("SCBCRSE_DEPT_CODE")]
        public string DepartmentCode { get; set; }
        [Column("DEPT_DESC")]
        public string DepartmentDesc { get; set; }
        [Column("SCRLEVL_LEVL_CODE")]
        public string LevelCode { get; set; }
        [Column("LEVEL_DESC")]
        public string LevelDesc { get; set; }
        [Column("COURSE_CLASS")]
        public string CourseClass { get; set; }
        [Column("SCBCRSE_CREDIT_HR_LOW")]
        public int CourseCreditHours { get; set; }
        [Column("SCBCRSE_LEC_HR_LOW")]
        public int LectureHours { get; set; }
        [Column("SCBCRSE_LAB_HR_LOW")]
        public int LabHours { get; set; }
        [Column("SSBSECT_TERM_CODE")]
        public string TermCode { get; set; }
        [Column("SSBSECT_SEQ_NUMB")]
        public string SubjectSequenceNumber { get; set; }
        [Column("SECTION_STATUS_CODE")]
        public string SectionStatusCode { get; set; }
        [Column("SECTION_STATUS_DESC")]
        public string SectionStatusDesc { get; set; }
        [Column("SSBSECT_SCHD_CODE")]
        public string SubjectCategoryCode { get; set; }
        [Column("SCHD_DESC")]
        public string SubjectCategoryDesc { get; set; }
        [Column("SSBSECT_INSM_CODE")]
        public string SubjectINSMCode { get; set; }
        [Column("INSM_DESC")]
        public string SubjectINSMDesc { get; set; }
        [Column("INST_PIDM")]
        public int PIDM { get; set; }
        [Column("INST_ID")]
        public string InstructorId { get; set; }
        [Column("INST_NAME")]
        public string InstructorName { get; set; }
        [Column("INSTRUCTOR_PNU_EMAIL")]
        public string InstructorEmail { get; set; }
        [Column("SIRASGN_PERCENT_RESPONSE")]
        public int PercentageResponse { get; set; }

        [Column("SIRASGN_PRIMARY_IND")]
        public string PrimaryIND { get; set; }
        public string Description { get; set; }
    }


    public class tblMemberCourses
    {
        public string ID { get; set; }
        public string SCBCRSE_SUBJ_CODE { get; set; }
        public string SCBCRSE_CRSE_NUMB { get; set; }
        public string SCBCRSE_COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string SCBCRSE_DEPT_CODE { get; set; }
        public string DEPT_DESC { get; set; }
        public string SCBCRSE_TITLE { get; set; }
        public string SCRLEVL_LEVL_CODE { get; set; }
        public string LEVEL_DESC { get; set; }
        public string SCBCRSE_CREDIT_HR_LOW { get; set; }
        public string SCBCRSE_LEC_HR_LOW { get; set; }
        public string SCBCRSE_LAB_HR_LOW { get; set; }
        public string SSBSECT_TERM_CODE { get; set; }
        public string STVTERM_DESC { get; set; }
        public string STVTERM_ACYR_CODE { get; set; }
        public string SSBSECT_CRN { get; set; }
        public string SSBSECT_SEQ_NUMB { get; set; }
        public string SECTION_STATUS_CODE { get; set; }
        public string SECTION_STATUS_DESC { get; set; }
        public string SSBSECT_SCHD_CODE { get; set; }
        public string SCHD_DESC { get; set; }
        public string SSBSECT_INSM_CODE { get; set; }
        public string INSM_DESC { get; set; }
        public string REGISTERED_COUNT { get; set; }
        public string INST_PIDM { get; set; }
        public string SPBPERS_SSN { get; set; }
        public string INST_ID { get; set; }
        public string INST_NAME { get; set; }
        public string SIRASGN_PERCENT_RESPONSE { get; set; }
        public string SIRASGN_PRIMARY_IND { get; set; }
        public string SSRMEET_BEGIN_TIME { get; set; }
        public string SSRMEET_END_TIME { get; set; }
        public string DAY { get; set; }
        public string CRN_CNT { get; set; }
        public string PNU_MAIL { get; set; }

        public string Description { get; set; }
        public string APICALL { get; set; }

    }


    public class tblMemberCourses_EN
    {
        public string ID { get; set; }
        public string SCBCRSE_SUBJ_CODE { get; set; }
        public string SCBCRSE_CRSE_NUMB { get; set; }
        public string SCBCRSE_COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string SCBCRSE_DEPT_CODE { get; set; }
        public string DEPT_DESC { get; set; }
        public string SCBCRSE_TITLE { get; set; }
        public string SCRLEVL_LEVL_CODE { get; set; }
        public string LEVEL_DESC { get; set; }
        public string SCBCRSE_CREDIT_HR_LOW { get; set; }
        public string SCBCRSE_LEC_HR_LOW { get; set; }
        public string SCBCRSE_LAB_HR_LOW { get; set; }
        public string SSBSECT_TERM_CODE { get; set; }
        public string STVTERM_DESC { get; set; }
        public string STVTERM_ACYR_CODE { get; set; }
        public string SSBSECT_CRN { get; set; }
        public string SSBSECT_SEQ_NUMB { get; set; }
        public string SECTION_STATUS_CODE { get; set; }
        public string SECTION_STATUS_DESC { get; set; }
        public string SSBSECT_SCHD_CODE { get; set; }
        public string SCHD_DESC { get; set; }
        public string SSBSECT_INSM_CODE { get; set; }
        public string INSM_DESC { get; set; }
        public string REGISTERED_COUNT { get; set; }
        public string INST_PIDM { get; set; }
        public string SPBPERS_SSN { get; set; }
        public string INST_ID { get; set; }
        public string INST_NAME { get; set; }
        public string SIRASGN_PERCENT_RESPONSE { get; set; }
        public string SIRASGN_PRIMARY_IND { get; set; }
        public string SSRMEET_BEGIN_TIME { get; set; }
        public string SSRMEET_END_TIME { get; set; }
        public string DAY { get; set; }
        public string CRN_CNT { get; set; }
        public string PNU_MAIL { get; set; }
        public string Description { get; set; }

        public string APICALL { get; set; }
    }


    public class CourseLevel
    {
        public string LevelCode { get; set; }
        public string LevelDesc { get; set; }
        public List<Course> Courses { get; set; }
    }
    public class Course
    {


        public string CourseCode { get; set; }


        public string CourseTitle { get; set; }


        public string RegisteredCount { get; set; }


        public string SubjectSectionNo { get; set; }
        public string LevelDesc { get; set; }

        public List<CourseTime> CoursesTime { get; set; }

    }

    public class CourseTime
    {
        public string DayName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }


    public class StudyPlanDto
    {
        public string smrprle_program { get; set; }
        public string smracaa_area { get; set; }
        public string clss { get; set; }
        public string smracaa_attr_code { get; set; }
        public string stvattr_desc { get; set; }
        public string scrattr_subj_code { get; set; }
        public string scrattr_crse_numb { get; set; }


    }


    public class NewStudyPlanDto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string SOBCURR_DEGC_CODE { get; set; }
        public string DEGC_DSC { get; set; }
        public string SMRPRLE_PROGRAM_DESC { get; set; }
        public string PROG_DESC_L { get; set; }
        public string COLLEGE_CODE { get; set; }
        public string COLLEGE_DSC { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DSC { get; set; }
        public string PROGRAM { get; set; }
        public string LANGCODE { get; set; }
        public string LANGDESC { get; set; }
        public string TERM_CTLG { get; set; }
        public string PROG_REQ_CREDITS { get; set; }
        public string PROG_CONNECTOR { get; set; }
        public string PROG_REQ_COURSES { get; set; }
        public string CLSS { get; set; }
        public string AREA { get; set; }
        public string AREA_DESC { get; set; }
        public string YRS { get; set; }
        //public string COURSE_TITLE { get; set; }

        public string SCRRTST_SEQNO { get; internal set; }
        private string _COURSE_TITLE;

        public string COURSE_TITLE
        {
            get
            {

                if (_COURSE_TITLE == null)
                {
                    return STVATTR_DESC;
                }


                return _COURSE_TITLE;
            }
            set
            {

                _COURSE_TITLE = value;
            }
        }
        public string SUBJ_CODE { get; set; }
        public string CRSE_NUMB { get; set; }
        public string LEC { get; set; }
        public string LAB { get; set; }
        public string OTH { get; set; }
        public string CREDIT { get; set; }


        private string _S_COREQ1;

        public string S_COREQ1
        {
            get
            {
                //if (SUBJ_CODE == null)
                //{
                //    return "";
                //}

                //if (SUBJ_CODE.Trim() == "انج")
                //{
                //    if (CRSE_NUMB.Trim() == "101")
                //    {
                //        return "";
                //    }
                //    else if (CRSE_NUMB.Trim() == "101-1")
                //    {
                //        return "انج 101";
                //    }
                //    else if (CRSE_NUMB.Trim() == "102")
                //    {
                //        return "انج 101 أو انج 101-1";
                //    }
                //    else if (CRSE_NUMB.Trim() == "102-2")
                //    {
                //        return "انج 101-1 - انج 101-1 - انج 102";
                //    }
                //    return _S_COREQ1;
                //}

                if (_S_COREQ1 == null)
                {
                    return "";
                }

                return _S_COREQ1;
            }
            set
            {

                _S_COREQ1 = value;
            }
        }
        public string STVATTR_DESC { get; set; }

    }


    public class AcademicCreditsDto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string COLL_CODE { get; set; }
        public string COLL_DESC { get; set; }
        public string PROG_DESC { get; set; }
        public string PROG_CODE { get; set; }
        public string DEPT_CODE { get; set; }
        public string DEPT_DESC { get; set; }
        public string MAJR_CODE { get; set; }
        public string MAJR_DESC { get; set; }
        public string PRG_CRED_CLASS { get; set; }
        public string PRG_CRED_SPN { get; set; }
    }



    public class PLANS_ELEC_U
    {
        public string ID { get; set; }
        public string SMRACAA_ATTR_CODE { get; set; }
        public string CAT { get; set; }
        public string STVATTR_DESC { get; set; }
        public string SCRATTR_SUBJ_CODE { get; set; }
        public string SCRATTR_CRSE_NUMB { get; set; }
        public string LEC { get; set; }
        public string LAB { get; set; }
        public string OTH { get; set; }
        public string CREDIT { get; set; }
        public string COURSE_TITLE { get; set; }
    }

    public class PLANS_ELEC_C
    {
        public string ID { get; set; }

        public string SMRPRLE_COLL_CODE { get; set; }
        public string SMRPRLE_PROGRAM { get; set; }
        public string SMRACAA_AREA { get; set; }
        public string CLSS { get; set; }
        public string SMRACAA_ATTR_CODE { get; set; }
        public string STVATTR_DESC2 { get; set; }
        public string STVATTR_DESC { get; set; }
        public string SCRATTR_SUBJ_CODE { get; set; }
        public string SCRATTR_CRSE_NUMB { get; set; }

        public string LEC { get; set; }
        public string LAB { get; set; }
        public string OTH { get; set; }
        public string CREDIT { get; set; }
        public string COURSE_TITLE { get; set; }

    }
    public class PLANS_ELEC_P
    {
        public string ID { get; set; }

        public string SMRPRLE_COLL_CODE { get; set; }
        public string SMRPRLE_PROGRAM { get; set; }
        public string SMRACAA_AREA { get; set; }
        public string CLSS { get; set; }
        public string SMRACAA_ATTR_CODE { get; set; }
        public string STVATTR_DESC2 { get; set; }
        public string STVATTR_DESC { get; set; }
        public string SCRATTR_SUBJ_CODE { get; set; }
        public string SCRATTR_CRSE_NUMB { get; set; }
        public string LEC { get; set; }
        public string LAB { get; set; }
        public string OTH { get; set; }
        public string CREDIT { get; set; }
        public string COURSE_TITLE { get; set; }


    }

}
