using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm;
using System.Web;

namespace PNU.Internet.WebParts
{
    public static class busclsCourses
    {
        public static List<lstCourses> GetCoursesByEmail(string email)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='instructor_pnu_email' />
                                 <Value Type='Text'>" + email + @"</Value>
                              </Eq>
                           </Where>");

                List<lstCourses> _Data = SPFactory.GetAllItemsByQuery<lstCourses>("Admin", Settings.Courses, query);
                if (_Data != null && _Data.Count > 0)
                    return _Data;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsCourses - GetCoursesByEmail", ex.Message);
                return null;
            }


            return null;




        }


        public static List<CourseDto> GetCoursesFromSQLByEmail(string email)
        {
            try
            {
                var coursesList = OracleDBContext.GetInstCoursesFromSQLDB(email);
                if (coursesList != null && coursesList.Count > 0)
                    return coursesList;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsCourses - GetCoursesFromOracleByEmail", ex.Message);
                return null;
            }



            return null;



        }

        public static List<CourseDto> GetCoursesFromList(string email)
        {
            try
            {
                List<CourseDto> coursesList = new List<CourseDto>();

                List<tblMemberCourses> courses = new List<tblMemberCourses>();
                string ListName = PortalHelper.IsArabic ? "tblMemberCourses" : "tblMemberCourses_EN";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);

                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
                                  <Eq>
                                     <FieldRef Name='PNU_MAIL' />
                                     <Value Type='Text'>{email}</Value>
                                  </Eq>
                               </Where>";
                            SPListItemCollection coll = list.GetItems(query);

                            if (coll == null && coll.Count <= 0)
                                coursesList = null;


                            courses = SPFactory.MapListItemsToClass<tblMemberCourses>(coll);

                            foreach (tblMemberCourses row in courses)
                            {
                                var course = new CourseDto()
                                {
                                    Code = !(row.SCBCRSE_SUBJ_CODE == null) ? Convert.ToString(row.SCBCRSE_SUBJ_CODE) : "",
                                    Numb = !(row.SCBCRSE_CRSE_NUMB == null) ? Convert.ToString(row.SCBCRSE_CRSE_NUMB) : "",
                                    CourseTitle = !(row.SCBCRSE_TITLE == null) ? Convert.ToString(row.SCBCRSE_TITLE) : "",
                                    CourseDays = !(row.DAY == null) ? Convert.ToString(row.DAY) : "",
                                    StartTime = !(row.SSRMEET_BEGIN_TIME == null) ? Convert.ToString(row.SSRMEET_BEGIN_TIME) : "",
                                    EndTime = !(row.SSRMEET_END_TIME == null) ? Convert.ToString(row.SSRMEET_END_TIME) : "",
                                    InstructorEmail = !(row.PNU_MAIL == null) ? Convert.ToString(row.PNU_MAIL).ToLower() : "",
                                    LevelCode = !(row.SCRLEVL_LEVL_CODE == null) ? Convert.ToString(row.SCRLEVL_LEVL_CODE) : "",
                                    LevelDesc = !(row.LEVEL_DESC == null) ? Convert.ToString(row.LEVEL_DESC) : "",
                                    RegisteredCount = !(row.REGISTERED_COUNT == null) ? Convert.ToString(row.REGISTERED_COUNT) : "",
                                    SubjectSectionNo = !(row.SSBSECT_CRN == null) ? Convert.ToString(row.SSBSECT_CRN) : "",
                                    Description = !(row.Description == null) ? Convert.ToString(row.Description) : ""

                                };
                                course.CourseCode = course.Code + " " + course.Numb;

                                coursesList.Add(course);


                            }





                        }
                    }
                });


                if (coursesList != null && coursesList.Count > 0)
                    return coursesList;

                return null;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsCourses - GetCoursesFromOracleByEmail", ex.Message);
                return null;
            }


            


        }

        public static List<CourseDto> GetCoursesFromOracleByEmail(string email)
        {
            try
            {
                var coursesList = OracleDBContext.GetInstCoursesFromOracleDB(email);
                if (coursesList != null && coursesList.Count > 0)
                    return coursesList;

                return null;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsCourses - GetCoursesFromOracleByEmail", ex.Message);
                return null;
            }

            



        }
    }

    public class lstCourses
    {
        public string Title { get; set; }
        public string scbcrse_subj_code { get; set; }
        public string scbcrse_title { get; set; }
        public string day { get; set; }
        public string ssrmeet_begin_time { get; set; }
        public string ssrmeet_end_time { get; set; }
        public string instructor_pnu_email { get; set; }
        public string scrlevl_levl_code { get; set; }
        public string level_desc { get; set; }
        public string registered_count { get; set; }
        public string ID { get; set; }


    }



    public class CourseLevel
    {
        public string LevelCode { get; set; }
        public string LevelDesc { get; set; }
        public List<Days> Days { get; set; }

       

    }
    public class Days
    {

        public string ID { get; set; }
        public string LevelDesc { get; set; }
        public string DayName { get; set; }
        public string DayNumber { get; set; }

        public List<Course> Courses { get; set; }

    }
    public class Course
    {

        public string ID { get; set; }
        public string LevelDesc { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string SubjectSectionNo { get; set; }
        public List<CourseTime> CoursesTime { get; set; }
        
        public string DayName { get; set; }
        public string DayNumber { get; set; }
        public string Decription { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

    }

    public class CourseTime
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Numb { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string RegisteredCount { get; set; }
        public string SubjectSectionNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Decription { get; set; }
    }

}
