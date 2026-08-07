using Microsoft.SharePoint;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Main.Helper;
using System.Net.Http;
using System.Net.Http.Headers;
using static Microsoft.SharePoint.WebPartPages.WebPartAdder;
using System.Web;
using System.Linq.Expressions;





namespace Pnu.Internet.CustomTimerJobs.OracleContext
{

    public static class OracleDBContext_Timer
    {

        public static string GetConnectionString(string ConnectionName)
        {
            string connection = "";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                using (SPSite site = new SPSite(GetSiteUrlConfigurationValue()))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList("ConnectionStrings");

                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         $@"<Where>
                                          <Eq>
                                             <FieldRef Name='Title' />
                                             <Value Type='Text'>{ConnectionName}</Value>
                                          </Eq>
                                       </Where>");

                        SPListItemCollection coll = list.GetItems(query);
                        if (coll != null && coll.Count > 0)
                            connection = coll[0]["ConnectionString"].ToString();
                    }
                }
            });

            return connection;
        }


        
        public static string GetSiteUrlConfigurationValue()
        {
            return ConfigurationManager.AppSettings["SiteUrl"];

        }

        public static List<CollegeDeptsDto> GetCollegeDeptsFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from CUSTAPP.MOBAPPL_COLL_PROG_DEPT";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<CollegeDeptsDto> collDepts = new List<CollegeDeptsDto>();

            foreach (DataRow row in dt.Rows)
            {
                var coldept = new CollegeDeptsDto()
                {
                    Coll_Code = Convert.ToString(row["coll_code"]),
                    Coll_Title = Convert.ToString(row["coll_desc"]),
                    Dept_Code = Convert.ToString(row["dept_code"]),
                    Dept_Title = Convert.ToString(row["dept_desc"]),
                    Prog_Code = Convert.ToString(row["prog_code"]),
                    Prog_Title = Convert.ToString(row["prog_desc"]),
                    Major_Code = Convert.ToString(row["majr_code"]),
                    Major_Title = Convert.ToString(row["majr_desc"])

                };
                collDepts.Add(coldept);
            }

            conn.Dispose();
            return collDepts;
        }

        public static List<CollegeCategoryDto> GetOnlyCollegeClassFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT COLL_CLASS_AR,COLL_CLASS_EN FROM CUSTAPP.MOBAPPL_COLL_PROG_DEPT
                                WHERE COLL_CLASS_AR IS NOT NULL ";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<CollegeCategoryDto> collDepts = new List<CollegeCategoryDto>();

            foreach (DataRow row in dt.Rows)
            {
                var coldept = new CollegeCategoryDto()
                {
                    COLL_CLASS_AR = Convert.ToString(row["COLL_CLASS_AR"]),
                    COLL_CLASS_EN = Convert.ToString(row["COLL_CLASS_EN"])

                };
                collDepts.Add(coldept);
            }

            conn.Dispose();
            return collDepts;
        }

        public static List<CollegesDto> GetOnlyCollegeFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT COLL_CLASS_AR,COLL_CLASS_EN,COLL_CODE , COLL_DESC,COLL_DESC_EN FROM CUSTAPP.MOBAPPL_COLL_PROG_DEPT
                                WHERE COLL_CODE != '00' ORDER BY COLL_CODE , COLL_DESC";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<CollegesDto> collDepts = new List<CollegesDto>();

            foreach (DataRow row in dt.Rows)
            {
                var coldept = new CollegesDto()
                {
                    COLL_CODE = Convert.ToString(row["COLL_CODE"]),
                    COLL_DESC = Convert.ToString(row["COLL_DESC"]),
                    COLL_DESC_EN = Convert.ToString(row["COLL_DESC_EN"]),
                    COLL_CLASS_AR = Convert.ToString(row["COLL_CLASS_AR"]),
                    COLL_CLASS_EN = Convert.ToString(row["COLL_CLASS_EN"])

                };
                collDepts.Add(coldept);
            }

            conn.Dispose();
            return collDepts;
        }
        public static List<DeptsDto> GetOnlyDeptFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = @" SELECT DISTINCT COLL_CLASS_AR,COLL_CLASS_EN,COLL_CODE , COLL_DESC,COLL_DESC_EN,DEPT_CODE , DEPT_DESC,DEPT_DESC_EN FROM CUSTAPP.MOBAPPL_COLL_PROG_DEPT
                                WHERE DEPT_CODE != '0000' ORDER BY COLL_CODE , COLL_DESC";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<DeptsDto> collDepts = new List<DeptsDto>();

            foreach (DataRow row in dt.Rows)
            {
                var coldept = new DeptsDto()
                {
                    COLL_CODE = Convert.ToString(row["COLL_CODE"]),
                    COLL_DESC = Convert.ToString(row["COLL_DESC"]),
                    COLL_DESC_EN = Convert.ToString(row["COLL_DESC_EN"]),
                    COLL_CLASS_AR = Convert.ToString(row["COLL_CLASS_AR"]),
                    COLL_CLASS_EN = Convert.ToString(row["COLL_CLASS_EN"]),
                    DEPT_CODE = Convert.ToString(row["DEPT_CODE"]),
                    DEPT_DESC = Convert.ToString(row["DEPT_DESC"]),
                    DEPT_DESC_EN = Convert.ToString(row["DEPT_DESC_EN"])


                };
                collDepts.Add(coldept);
            }

            conn.Dispose();
            return collDepts;
        }

        public static List<ProgramsDto> GetOnlyProgramsFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = @" SELECT DISTINCT COLL_CLASS_AR,COLL_CLASS_EN,COLL_CODE , COLL_DESC,COLL_DESC_EN,DEPT_CODE , DEPT_DESC,DEPT_DESC_EN,PROG_CODE , PROG_DESC,PROG_DESC_EN FROM CUSTAPP.MOBAPPL_COLL_PROG_DEPT
                                WHERE PROG_CODE != '0000' ORDER BY COLL_CODE , COLL_DESC";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<ProgramsDto> collDepts = new List<ProgramsDto>();

            foreach (DataRow row in dt.Rows)
            {
                var coldept = new ProgramsDto()
                {
                    COLL_CODE = Convert.ToString(row["COLL_CODE"]),
                    COLL_DESC = Convert.ToString(row["COLL_DESC"]),
                    COLL_DESC_EN = Convert.ToString(row["COLL_DESC_EN"]),
                    COLL_CLASS_AR = Convert.ToString(row["COLL_CLASS_AR"]),
                    COLL_CLASS_EN = Convert.ToString(row["COLL_CLASS_EN"]),
                    DEPT_CODE = Convert.ToString(row["DEPT_CODE"]),
                    DEPT_DESC = Convert.ToString(row["DEPT_DESC"]),
                    DEPT_DESC_EN = Convert.ToString(row["DEPT_DESC_EN"]),
                    PROG_CODE = Convert.ToString(row["PROG_CODE"]),
                    PROG_DESC = Convert.ToString(row["PROG_DESC"]),
                    PROG_DESC_EN = Convert.ToString(row["PROG_DESC_EN"])



                };
                collDepts.Add(coldept);
            }

            conn.Dispose();
            return collDepts;
        }

        public static List<CollegeMembersDto> GetCollegeMembersFromOracleDB()
        {
            //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            var oradb = GetGrpConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);  // C#
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();


            //cmd.CommandText = @"select title , full_name,english_name,employee_number,email_address,phone_number,extention_number ,profession 
            //        , college_id,college,specialization ,SPECIAL_CPECIALIZATION minor,qualification_name,nationality,section_id,section,employee_status,location_code,location_name,floor,office 
            //        from XXX_EMP_ACADEMIC_LOAD_V ";

            cmd.CommandText = @"select TITLE,FULL_NAME,ENGLISH_NAME,EMPLOYEE_NUMBER,LOWER(EMAIL_ADDRESS) EMAIL_ADDRESS,PHONE_NUMBER,
                                EXTENTION_NUMBER,PROFESSION,PROFESSION_EN,GRADE_NAME,GRADE_NAME_EN,COLLEGE_ID,     
                                COLLEGE, COLLEGE_EN, SPECIALIZATION, SPECIALIZATION_EN, SPECIAL_CPECIALIZATION,
                                SPECIAL_CPECIALIZATION_EN, QUALIFICATION_NAME, QUALIFICATION_NAME_EN, NATIONALITY, 
                                NATIONALITY_EN, SECTION_ID,  SECTION, SECTION_EN, SCHOLARSHIP, POSITION_DELEG,POSITION,
                                DELEG_DEPARTMENT, DELEG_START_DATE,DURATION, ORDERDATE,END_DATE,EMPLOYEE_STATUS,    
                                TERMINATION_REASON, LOCATION_CODE,  LOCATION_NAME, LOCATION_NAME_EN, FLOOR,  OFFICE 
            from XXX_EMP_ACADEMIC_LOAD_V";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<CollegeMembersDto> collMembers = SPFactory.DataTableMapToList<CollegeMembersDto>(dt);

            

            conn.Dispose();
            return collMembers;
        }
        public static List<StudyPlanDto> GetStudyPlanFromOracleDB()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();


            cmd.CommandText = @"select smrprle_program ,
                                smracaa_area,clss,smracaa_attr_code,
                                stvattr_desc,scrattr_subj_code,scrattr_crse_numb 
                                from CUSTAPP.MOBAPPL_PLANS_ELEC";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<StudyPlanDto> studyPlans = new List<StudyPlanDto>();

            foreach (DataRow row in dt.Rows)
            {
                var studyPlan = new StudyPlanDto()
                {


                    smrprle_program = !(row["smrprle_program"] is DBNull) ? Convert.ToString(row["smrprle_program"]) : "",
                    smracaa_area = !(row["smracaa_area"] is DBNull) ? Convert.ToString(row["smracaa_area"]) : "",
                    clss = !(row["clss"] is DBNull) ? Convert.ToString(row["clss"]) : "",
                    smracaa_attr_code = !(row["smracaa_attr_code"] is DBNull) ? Convert.ToString(row["smracaa_attr_code"]) : "",
                    stvattr_desc = !(row["stvattr_desc"] is DBNull) ? Convert.ToString(row["stvattr_desc"]) : "",
                    scrattr_subj_code = !(row["scrattr_subj_code"] is DBNull) ? Convert.ToString(row["scrattr_subj_code"]) : "",
                    scrattr_crse_numb = !(row["scrattr_crse_numb"] is DBNull) ? Convert.ToString(row["scrattr_crse_numb"]) : "",


                };
                studyPlans.Add(studyPlan);


            }

            conn.Dispose();
            return studyPlans;

        }

        public static List<PLANS_ELEC_U> GetPLANS_ELEC_U(string ViewName)
        {
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM {ViewName}";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();

            List<PLANS_ELEC_U> _PLANS_ELEC_U = new List<PLANS_ELEC_U>();
            _PLANS_ELEC_U = SPFactory.DataTableMapToList<PLANS_ELEC_U>(dt);

            conn.Dispose();
            return _PLANS_ELEC_U;
        }

        public static List<PLANS_ELEC_C> GetPLANS_ELEC_C(string ViewName)
        {
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM {ViewName}";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();

            List<PLANS_ELEC_C> _PLANS_ELEC_C = new List<PLANS_ELEC_C>();
            _PLANS_ELEC_C = SPFactory.DataTableMapToList<PLANS_ELEC_C>(dt);

            conn.Dispose();
            return _PLANS_ELEC_C;
        }

        public static List<PLANS_ELEC_P> GetPLANS_ELEC_P(string ViewName)
        {
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM {ViewName}";
            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();

            List<PLANS_ELEC_P> _PLANS_ELEC_P = new List<PLANS_ELEC_P>();
            _PLANS_ELEC_P = SPFactory.DataTableMapToList<PLANS_ELEC_P>(dt);

            conn.Dispose();
            return _PLANS_ELEC_P;
        }
        public static List<tblMemberCourses> GetNewInstCoursesFromOracleDB()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM CUSTAPP.MOBAPPL_INSTSCHD_M WHERE PNU_MAIL IS NOT NULL ORDER BY PNU_MAIL";


            // cmd.CommandText = @"SELECT * from custapp.MOBAPPL_ACADEMIC_LOAD";

            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<tblMemberCourses> courses = new List<tblMemberCourses>();

            courses = SPFactory.DataTableMapToList<tblMemberCourses>(dt);



            conn.Dispose();
            return courses;

        }


        public static List<tblMemberCourses_EN> GetNewInstCoursesFromOracleDB_EN()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM CUSTAPP.MOBAPPL_INSTSCHD_EN_M WHERE PNU_MAIL IS NOT NULL ORDER BY PNU_MAIL";


            // cmd.CommandText = @"SELECT * from custapp.MOBAPPL_ACADEMIC_LOAD";

            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<tblMemberCourses_EN> courses = new List<tblMemberCourses_EN>();

            courses = SPFactory.DataTableMapToList<tblMemberCourses_EN>(dt);


            conn.Dispose();
            return courses;

        }

        public static List<CourseDto> GetInstCoursesFromOracleDB()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT inst.scbcrse_title ,
                                inst.scbcrse_subj_code||inst.scbcrse_crse_numb as scbcrse_subj_code ,
                                inst.registered_count,inst.ssbsect_crn ,
                                inst.ssrmeet_begin_time ,inst.scrlevl_levl_code ,  inst.level_desc,
                                inst.PNU_MAIL instructor_pnu_email ,
                                inst.ssrmeet_end_time FROM custapp.MOBAPPL_ACADEMIC_LOAD_M inst";


            // cmd.CommandText = @"SELECT * from custapp.MOBAPPL_ACADEMIC_LOAD";

            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<CourseDto> courses = new List<CourseDto>();

            foreach (DataRow row in dt.Rows)
            {
                var course = new CourseDto()
                {
                    CourseCode = !(row["scbcrse_subj_code"] is DBNull) ? Convert.ToString(row["scbcrse_subj_code"]) : "",
                    CourseTitle = !(row["scbcrse_title"] is DBNull) ? Convert.ToString(row["scbcrse_title"]) : "",
                    CourseDays = "day",
                    StartTime = !(row["ssrmeet_begin_time"] is DBNull) ? Convert.ToString(row["ssrmeet_begin_time"]) : "",
                    EndTime = !(row["ssrmeet_end_time"] is DBNull) ? Convert.ToString(row["ssrmeet_end_time"]) : "",
                    InstructorEmail = !(row["instructor_pnu_email"] is DBNull) ? Convert.ToString(row["instructor_pnu_email"]).ToLower() : "",
                    LevelCode = !(row["scrlevl_levl_code"] is DBNull) ? Convert.ToString(row["scrlevl_levl_code"]) : "",
                    LevelDesc = !(row["level_desc"] is DBNull) ? Convert.ToString(row["level_desc"]) : "",
                    RegisteredCount = !(row["registered_count"] is DBNull) ? Convert.ToString(row["registered_count"]) : "",

                };
                courses.Add(course);


            }

            conn.Dispose();
            return courses;

        }
        
        public static List<CourseDto> GetInstCoursesFromSQLDB(string email)
        {
            email = email.ToLower();
            var oradb = GetConnectionString("New_PNU_Portal_Cust_DB");
            SqlConnection conn = new SqlConnection(oradb);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            var MemberCourseViewName = PortalHelper.IsArabic ? "tblMemberCourses" : "tblMemberCourses_EN";



            cmd.CommandText = $@"SELECT 
                    inst.SCBCRSE_TITLE ,
                    inst.SCBCRSE_SUBJ_CODE as code ,
                    inst.SCBCRSE_CRSE_NUMB as numb , 
                    inst.SCBCRSE_SUBJ_CODE + ' ' +inst.SCBCRSE_CRSE_NUMB as scbcrse_subj_code ,
                    inst.REGISTERED_COUNT,
                    inst.SSBSECT_CRN ,
                    inst.DAY,
                    inst.SSRMEET_BEGIN_TIME ,
                    inst.SCRLEVL_LEVL_CODE ,  
                    inst.LEVEL_DESC,                           
                    inst.PNU_MAIL instructor_pnu_email ,
                    inst.SSRMEET_END_TIME,inst.Description
                    FROM {MemberCourseViewName}  inst  WHERE LOWER(inst.PNU_MAIL) = N'{email}'";

            // cmd.CommandText = @"SELECT * from custapp.MOBAPPL_ACADEMIC_LOAD";
            SqlDataAdapter OA = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {

                OA.Fill(dt);
                cmd.Dispose();
            }
            catch
            {
                cmd.Dispose();
                return null;
            }




            List<CourseDto> courses = new List<CourseDto>();

            foreach (DataRow row in dt.Rows)
            {
                var course = new CourseDto()
                {
                    Code = !(row["code"] is DBNull) ? Convert.ToString(row["code"]) : "",
                    Numb = !(row["numb"] is DBNull) ? Convert.ToString(row["numb"]) : "",
                    CourseCode = !(row["SCBCRSE_SUBJ_CODE"] is DBNull) ? Convert.ToString(row["SCBCRSE_SUBJ_CODE"]) : "",
                    CourseTitle = !(row["SCBCRSE_TITLE"] is DBNull) ? Convert.ToString(row["SCBCRSE_TITLE"]) : "",
                    CourseDays = !(row["DAY"] is DBNull) ? Convert.ToString(row["DAY"]) : "",
                    StartTime = !(row["SSRMEET_BEGIN_TIME"] is DBNull) ? Convert.ToString(row["SSRMEET_BEGIN_TIME"]) : "",
                    EndTime = !(row["SSRMEET_END_TIME"] is DBNull) ? Convert.ToString(row["SSRMEET_END_TIME"]) : "",
                    InstructorEmail = !(row["instructor_pnu_email"] is DBNull) ? Convert.ToString(row["instructor_pnu_email"]).ToLower() : "",
                    LevelCode = !(row["SCRLEVL_LEVL_CODE"] is DBNull) ? Convert.ToString(row["SCRLEVL_LEVL_CODE"]) : "",
                    LevelDesc = !(row["LEVEL_DESC"] is DBNull) ? Convert.ToString(row["LEVEL_DESC"]) : "",
                    RegisteredCount = !(row["REGISTERED_COUNT"] is DBNull) ? Convert.ToString(row["REGISTERED_COUNT"]) : "",
                    SubjectSectionNo = !(row["SSBSECT_CRN"] is DBNull) ? Convert.ToString(row["SSBSECT_CRN"]) : "",
                    Description = !(row["Description"] is DBNull) ? Convert.ToString(row["Description"]) : ""

                };
                courses.Add(course);


            }

            conn.Dispose();
            return courses;

        }


        public static StatisticsDto GetStatisticsFromOracleDB()
        {
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"select * from CUSTAPP.MOBAPPL_COUNT_STD_COLL_PROG";

            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();

            var statistics = new StatisticsDto()
            {
                CNT_STD = !(dt.Rows[0]["CNT_STD"] is DBNull) ? Convert.ToString(dt.Rows[0]["CNT_STD"]) : "",
                CTN_COLL = !(dt.Rows[0]["CNT_COLL"] is DBNull) ? Convert.ToString(dt.Rows[0]["CNT_COLL"]) : "",
                CNT_PROG = !(dt.Rows[0]["CNT_PROG"] is DBNull) ? Convert.ToString(dt.Rows[0]["CNT_PROG"]) : "",
                CNT_ALL_DEPT = !(dt.Rows[0]["CNT_ALL_DEPT"] is DBNull) ? Convert.ToString(dt.Rows[0]["CNT_ALL_DEPT"]) : ""
            };



            conn.Dispose();
            return statistics;

        }
        public static List<NewStatisticsDto> GetNewStatisticsFromOracleDB()
        {
            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"select * from CUSTAPP.MOBAPPL_COUNT_STD_COLL_PROG";

            OracleDataAdapter OA = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();

            var statistics = new List<NewStatisticsDto>();

            statistics = SPFactory.DataTableMapToList<NewStatisticsDto>(dt);


            conn.Dispose();
            return statistics;

        }
        private static string GetCollegesMembersCount()
        {
            var result = "";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList("CollMembersListName");
                        var TotalCount = list.GetItems().Count;
                        result = TotalCount.ToString();

                    }
                }
            });

            return result;
        }
        private static void GetStatisticByName(StatisticsDto statistics, SPListItem item, string membersCount)
        {


            if (Convert.ToString(item["Title"]) == StatisticTypes.CollegesInstitues.Value)
            {
                item["Count"] = statistics.CTN_COLL;
            }
            else if (Convert.ToString(item["Title"]) == StatisticTypes.Programs.Value)
            {
                item["Count"] = statistics.CNT_PROG;
            }
            else if (Convert.ToString(item["Title"]) == StatisticTypes.Students.Value)
            {
                item["Count"] = statistics.CNT_STD;
            }
            else if (Convert.ToString(item["Title"]) == StatisticTypes.Members.Value)
            {
                item["Count"] = membersCount;
            }
            else if (Convert.ToString(item["Title"]) == StatisticTypes.AllDepartments.Value)
            {
                item["Count"] = statistics.CNT_ALL_DEPT;
            }

        }

        public static void SaveDataToList(string WebUrl, string ListName)
        {
            var collDeptList = GetCollegeDeptsFromOracleDB();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(WebUrl))
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(ListName);
                        list.Fields.Add("Coll_Code", SPFieldType.Text, false);
                        list.Fields.Add("Coll_Title", SPFieldType.Text, false);
                        list.Fields.Add("Dept_Code", SPFieldType.Text, false);
                        list.Fields.Add("Dept_Title", SPFieldType.Text, false);
                        list.Fields.Add("Prog_Code", SPFieldType.Text, false);
                        list.Fields.Add("Prog_Title", SPFieldType.Text, false);
                        list.Fields.Add("Major_Code", SPFieldType.Text, false);
                        list.Fields.Add("Major_Title", SPFieldType.Text, false);

                        SPView view = list.DefaultView;
                        view.ViewFields.Add("Coll_Code");
                        view.ViewFields.Add("Coll_Title");
                        view.ViewFields.Add("Dept_Code");
                        view.ViewFields.Add("Dept_Title");
                        view.ViewFields.Add("Prog_Code");
                        view.ViewFields.Add("Prog_Title");
                        view.ViewFields.Add("Major_Code");
                        view.ViewFields.Add("Major_Title");
                        view.Update();

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (var row in collDeptList)
                        {

                            SPListItem listItem = list.Items.Add();

                            listItem["Coll_Code"] = row.Coll_Code;
                            listItem["Coll_Title"] = row.Coll_Title;
                            listItem["Dept_Code"] = row.Dept_Code;
                            listItem["Dept_Title"] = row.Dept_Title;
                            listItem["Prog_Code"] = row.Prog_Code;
                            listItem["Prog_Title"] = row.Prog_Title;
                            listItem["Major_Code"] = row.Major_Code;
                            listItem["Major_Title"] = row.Major_Title;


                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });

        }

        public static void SaveCollMemeberDataToList(string ListName)
        {
            var collMemberList = GetCollegeMembersFromOracleDB();


            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(ListName);

                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<CollegeMembersDto>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (var member in collMemberList)
                        {

                            SPListItem listItem = list.Items.Add();
                            listItem = SPFactory.MapClassToSPListItem(listItem, member);

                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });

        }

        
        public static void SaveInstructorCoursesDataToDB()
        {
            var coursesList = GetNewInstCoursesFromOracleDB();

            var sqldb = GetConnectionString("New_PNU_Portal_Cust_DB");
            SQLFactory.SetConn(sqldb);
            SQLFactory.InsertIntoTable<tblMemberCourses>(coursesList, true);
            var coursesList_EN = GetNewInstCoursesFromOracleDB_EN();
            SQLFactory.SetConn(sqldb);
            SQLFactory.InsertIntoTable<tblMemberCourses_EN>(coursesList_EN, true);
        }


        public static void SaveGadeerCoursesDataToDB()
        {

            var sqldb = GetSQLDBConnection();
            SQLFactory.SetConn(sqldb);
            List<tblMemberCourses> _AllData = SQLFactory.ReadDataFromTable<tblMemberCourses>("tblMemberCourses", " APICALL = N'NO'", "", "");


            List<tblMemberCourses> _AllDataDistinct = _AllData.GroupBy(d => new { d.SCBCRSE_SUBJ_CODE, d.SCBCRSE_CRSE_NUMB }).Select(group => group.First()).ToList();








            while (_AllDataDistinct.Count > 0 && _AllDataDistinct != null)
            {
                string UpdateSQL = "";
                int batchSize = 50;
                int recordsProcessed = 0;

                foreach (tblMemberCourses obj in _AllDataDistinct.Take(batchSize))
                {
                    //obj.Description = GetCourseDescFromJadeer(obj.SCBCRSE_SUBJ_CODE, obj.SCBCRSE_CRSE_NUMB);
                    obj.APICALL = "YES";
                    string Description = "";
                    if (obj.Description != null && obj.Description != "")
                    {
                        obj.Description = obj.Description.Replace("\n", " ").Replace("'", "''");
                        Description = $",[Description] = N'{obj.Description}'";
                    }

                    UpdateSQL += $"UPDATE [dbo].[tblMemberCourses] SET [APICALL] = N'YES' {Description} WHERE SCBCRSE_SUBJ_CODE = N'{obj.SCBCRSE_SUBJ_CODE}' AND SCBCRSE_CRSE_NUMB = N'{obj.SCBCRSE_CRSE_NUMB}'";
                    UpdateSQL += Environment.NewLine;

                    recordsProcessed++;
                }
                try
                {
                    SQLFactory.Update(UpdateSQL);
                }
                catch (Exception ex)
                {

                }

                _AllDataDistinct = _AllDataDistinct.Skip(recordsProcessed).ToList();

            }


        }

        public static void SaveGadeerCoursesDataToDB_EN()
        {

            var sqldb = GetSQLDBConnection();
            SQLFactory.SetConn(sqldb);
            List<tblMemberCourses_EN> _AllData = SQLFactory.ReadDataFromTable<tblMemberCourses_EN>("tblMemberCourses_EN", " APICALL = N'NO'", "", "");
            List<tblMemberCourses_EN> _AllDataDistinct = _AllData.GroupBy(d => new { d.SCBCRSE_SUBJ_CODE, d.SCBCRSE_CRSE_NUMB }).Select(group => group.First()).ToList();

            while (_AllDataDistinct.Count > 0 && _AllDataDistinct != null)
            {
                string UpdateSQL = "";
                int batchSize = 50;
                int recordsProcessed = 0;

                foreach (tblMemberCourses_EN obj in _AllDataDistinct.Take(batchSize))
                {
                    //obj.Description = GetCourseDescFromJadeer(obj.SCBCRSE_SUBJ_CODE, obj.SCBCRSE_CRSE_NUMB);
                    obj.APICALL = "YES";
                    string Description = "";
                    if (obj.Description != null && obj.Description != "")
                    {
                        obj.Description = obj.Description.Replace("\n", " ").Replace("'", "''");
                        Description = $",[Description] = N'{obj.Description}'";
                    }

                    UpdateSQL += $"UPDATE [dbo].[tblMemberCourses_EN] SET [APICALL] = N'YES' {Description} WHERE SCBCRSE_SUBJ_CODE = N'{obj.SCBCRSE_SUBJ_CODE}' AND SCBCRSE_CRSE_NUMB = N'{obj.SCBCRSE_CRSE_NUMB}'";
                    UpdateSQL += Environment.NewLine;

                    recordsProcessed++;
                }
                try
                {
                    SQLFactory.Update(UpdateSQL);
                }
                catch (Exception ex)
                {

                }

                _AllDataDistinct = _AllDataDistinct.Skip(recordsProcessed).ToList();

            }


        }


        //public static string GetCourseDescFromJadeer(string code, string numb)
        //{
        //    HttpClient client = new HttpClient();
        //    string result = "";
        //    string apiUrl = "https://jadeer.pnu.edu.sa/api/CSPS/0/{0}/{1}";

        //    client.BaseAddress = new Uri(apiUrl);

        //    // var CourseNumber = GetCourseNumber(numb);

        //    client.DefaultRequestHeaders.Accept.Add(
        //    new MediaTypeWithQualityHeaderValue("application/json"));

        //    HttpResponseMessage response = client.GetAsync(string.Format(apiUrl, code, RemoveChar(numb))).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var dataObjects = response.Content.ReadAsAsync<clsCourses>().Result;

        //        result = dataObjects.course_specification;

        //    }



        //    return result;
        //}
        public static string RemoveChar(string code)
        {
            if (code.Length >= 4)
                return code.Substring(0, code.Length - 1);
            else
                return code;
        }
        public static int GetNumberPosition(string str)
        {
            int index = -1;
            foreach (char ch in str)
            {
                index++;
                if (!Char.IsDigit(ch))
                {
                    return index;
                }
            }
            return str.Length;
        }
        public static string GetCourseNumber(string numb)
        {
            int number = GetNumberPosition(numb);
            string Numberpart = numb.Substring(0, number);
            return Numberpart;
        }


        public static void SaveInstructorCoursesDataToList(string ListName)
        {
            var coursesList = GetInstCoursesFromOracleDB();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(ListName);
                        list.Fields.Add("scbcrse_subj_code", SPFieldType.Text, false);
                        list.Fields.Add("scbcrse_title", SPFieldType.Text, false);
                        list.Fields.Add("day", SPFieldType.Text, false);
                        list.Fields.Add("ssrmeet_begin_time", SPFieldType.Text, false);
                        list.Fields.Add("ssrmeet_end_time", SPFieldType.Text, false);
                        list.Fields.Add("instructor_pnu_email", SPFieldType.Text, false);
                        list.Fields.Add("scrlevl_levl_code", SPFieldType.Text, false);
                        list.Fields.Add("level_desc", SPFieldType.Text, false);
                        list.Fields.Add("registered_count", SPFieldType.Text, false);

                        SPView view = list.DefaultView;
                        view.ViewFields.Add("scbcrse_subj_code");
                        view.ViewFields.Add("scbcrse_title");
                        view.ViewFields.Add("day");
                        view.ViewFields.Add("ssrmeet_begin_time");
                        view.ViewFields.Add("ssrmeet_end_time");
                        view.ViewFields.Add("instructor_pnu_email");
                        view.ViewFields.Add("scrlevl_levl_code");
                        view.ViewFields.Add("level_desc");
                        view.ViewFields.Add("registered_count");

                        view.Update();

                        web.AllowUnsafeUpdates = false;

                    }
                }


                int batchSize = 1000; // Adjust the batch size based on your requirements

                for (int i = 0; i < coursesList.Count; i += batchSize)
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists[ListName]; // Replace with your actual list name

                            try
                            {
                                web.AllowUnsafeUpdates = true;

                                for (int j = i; j < Math.Min(i + batchSize, coursesList.Count); j++)
                                {
                                    var course = coursesList[j];

                                    SPListItem listItem = list.Items.Add();
                                    listItem["scbcrse_subj_code"] = course.CourseCode;
                                    listItem["scbcrse_title"] = course.CourseTitle;
                                    // ... (other fields)

                                    listItem.Update();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle the exception (log, display an error message, etc.)
                                Console.WriteLine($"Error adding courses: {ex.Message}");
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }

                    }
                }

            });
        }

        public static void UpdateStatisticsDataToList(string ListName)
        {
            var statistics = GetStatisticsFromOracleDB();
            List<NewStatisticsDto> Newstatistics = GetNewStatisticsFromOracleDB();
            var totlaMemberCount = GetCollegesMembersCount();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList(ListName);

                        SPListItemCollection coll = list.GetItems();
                        if (coll == null && coll.Count <= 0)
                            return;

                        for (int i = 1; i <= coll.Count; i++)
                        {
                            var item = coll.GetItemById(i);
                            web.AllowUnsafeUpdates = true;
                            GetStatisticByName(statistics, item, totlaMemberCount);
                            item.Update();
                            web.AllowUnsafeUpdates = false;
                        }

                        foreach (var obj in Newstatistics)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
                                          <Eq>
                                             <FieldRef Name='SMRPRLE_DEGC_CODE' />
                                             <Value Type='Text'>{obj.SMRPRLE_DEGC_CODE}</Value>
                                          </Eq>
                                       </Where>";
                            SPListItemCollection collNew = list.GetItems(query);
                            SPListItem NewItem;
                            web.AllowUnsafeUpdates = true;
                            if (collNew != null && collNew.Count > 0)
                            {
                                NewItem = collNew[0];
                            }
                            else
                                NewItem = list.Items.Add();

                            NewItem["Title"] = obj.STVDEGC_DESC;
                            NewItem["SMRPRLE_DEGC_CODE"] = obj.SMRPRLE_DEGC_CODE;
                            NewItem["Count"] = obj.CNT_DEGREE_PROG;
                            NewItem["TitleEn"] = obj.DESC_EN;

                            NewItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }



                    }
                }
            });

        }
        public static string GetBannerConfigurationValue()
        {
            return GetConnectionString("BannerConn");
            //return ConfigurationManager.AppSettings["BannerConn"];

        }

        public static string GetSQLDBConnection()
        {
            return GetConnectionString("New_PNU_Portal_Cust_DB");
            //return ConfigurationManager.AppSettings["BannerConn"];

        }
        public static string GetGrpConfigurationValue()
        {
            return GetConnectionString("GrpConn");

        }

        public static string GetWebUrlConfigurationValue()
        {
            return ConfigurationManager.AppSettings["WebUrl"];

        }
        public static string GetListNameConfigurationValue()
        {
            return ConfigurationManager.AppSettings["ListName"];

        }
        public static string GetCollMemeberListNameConfigurationValue()
        {
            return ConfigurationManager.AppSettings["CollMembersListName"];

        }
        public static string GetStudyPlanListNameConfigurationValue()
        {
            return ConfigurationManager.AppSettings["StudyPlanListName"];

        }
        public static string GetInstCourseListNameConfigurationValue()
        {
            return ConfigurationManager.AppSettings["InstCoursesListName"];

        }

        public static List<NewStudyPlanDto> GetNewStudyPlanFromOracleDB()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();


            cmd.CommandText = @"select *  from CUSTAPP.MOBAPPL_PLANS_M";
            OracleDataAdapter OA = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<NewStudyPlanDto> studyPlans = new List<NewStudyPlanDto>();
            studyPlans = SPFactory.DataTableMapToList<NewStudyPlanDto>(dt);



            conn.Dispose();
            return studyPlans;

        }

        public static List<NewStudyPlanDto> GetNewStudyPlanFromOracleDB_EN()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();


            cmd.CommandText = @"select *  from CUSTAPP.MOBAPPL_PLANS_EN_M";
            OracleDataAdapter OA = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<NewStudyPlanDto> studyPlans = new List<NewStudyPlanDto>();
            studyPlans = SPFactory.DataTableMapToList<NewStudyPlanDto>(dt);



            conn.Dispose();
            return studyPlans;

        }


        public static List<AcademicCreditsDto> GetAcademicCreditsFromOracleDB()
        {

            var oradb = GetBannerConfigurationValue();
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();


            cmd.CommandText = @"SELECT * FROM CUSTAPP.MOBAPPL_COLL_PROG_DEPT";
            OracleDataAdapter OA = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
            DataTable dt = new DataTable();
            OA.Fill(dt);
            cmd.Dispose();


            List<AcademicCreditsDto> studyPlans = SPFactory.DataTableMapToList<AcademicCreditsDto>(dt);


            conn.Dispose();
            return studyPlans;

        }

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



    

    
    public class StatisticsDto
    {
        public string CNT_STD { get; set; }
        public string CTN_COLL { get; set; }
        public string CNT_PROG { get; set; }
        public string CNT_ALL_DEPT { get; set; }

    }

    public class NewStatisticsDto
    {
        public string CNT_STD { get; set; }
        public string CNT_COLL { get; set; }
        public string CNT_PROG { get; set; }
        public string CNT_ALL_DEPT { get; set; }
        public string CNT_DEGREE_PROG { get; set; }
        public string SMRPRLE_DEGC_CODE { get; set; }
        public string STVDEGC_DESC { get; set; }
        public string DESC_EN { get; set; }
    }

}
