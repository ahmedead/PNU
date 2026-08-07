using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Http.Formatting;
using iTextSharp.text;
using System.Net.Security;
using System.Web;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    public partial class ucMemberCourses : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindData();

            }



        }


        public void BindData()
        {
            string email = "";
            int index = 1;
            try
            {
                email = Request.QueryString["view"].ToString();
                email.ToLower().Trim();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    List<CourseLevel> courseLevels = new List<CourseLevel>();

                    // Assuming busclsCourses.GetCoursesFromList(email) returns List<CourseDto>
                    List<CourseDto> _AllCourses = busclsCourses.GetCoursesFromList(email);

                    if (_AllCourses != null && _AllCourses.Count > 0)
                    {
                        // Group courses by level
                        var distinctLevels = _AllCourses.GroupBy(c => new { c.LevelCode, c.LevelDesc }).Select(g => g.First());

                        foreach (var level in distinctLevels)
                        {
                            CourseLevel courseLevel = new CourseLevel
                            {
                                LevelCode = level.LevelCode,
                                LevelDesc = level.LevelDesc,
                                Days = new List<Days>()
                            };

                            // Group courses by days within the current level
                            var distinctDays = _AllCourses.Where(c => c.LevelCode == level.LevelCode)
                                                          .GroupBy(c => new { c.CourseDays })
                                                          .Select(g => g.First());

                            foreach (var day in distinctDays)
                            {
                                Days courseDay = new Days
                                {
                                    ID = index.ToString(),
                                    LevelDesc = level.LevelDesc,
                                    DayName = day.CourseDays == null ? "" : SPFactory.GetEquivalantDayName(day.CourseDays.ToString()),
                                    DayNumber = day.CourseDays.ToString(),
                                    Courses = new List<Course>()
                                };

                                index = index + 1;
                                // Fetch courses for the current day
                                var dayCourses = _AllCourses.Where(c => c.LevelCode == level.LevelCode && c.CourseDays == day.CourseDays).ToList();

                                foreach (var course in dayCourses)
                                {
                                    string StartTime = "";
                                    string EndTime = "";
                                    if (course.StartTime.Length == 3)
                                        StartTime = "0" + course.StartTime;
                                    else
                                        StartTime = course.StartTime;
                                    if (course.EndTime.Length == 3)
                                        EndTime = "0" + course.EndTime;
                                    else
                                        EndTime = course.EndTime;


                                    Course courseData = new Course
                                    {
                                        ID = index.ToString(),
                                        LevelDesc = course.LevelDesc,
                                        DayName = day.CourseDays == null ? "" : SPFactory.GetEquivalantDayName(day.CourseDays.ToString()),
                                        DayNumber = day.CourseDays.ToString(),
                                        Decription = course.Description,
                                        CourseCode = course.CourseCode,
                                        CourseTitle = course.CourseTitle,
                                        SubjectSectionNo = course.SubjectSectionNo,
                                        StartTime = StartTime == "" ? "" : DateTime.ParseExact(StartTime, "HHmm", null).ToString("HH:mm"), // _time.StartTime.ToString("HH:mm"),
                                        EndTime = EndTime == "" ? "" : DateTime.ParseExact(EndTime, "HHmm", null).ToString("HH:mm"), //_time.EndTime.ToString("HH:mm")
                                        CoursesTime = new List<CourseTime>()
                                    };
                                    index = index + 1;
                                    // Fetch course times for the current course
                                    var courseTimes = _AllCourses.Where(c => c.LevelCode == level.LevelCode && c.CourseDays == day.CourseDays && c.CourseCode == course.CourseCode).ToList();

                                    foreach (var time in courseTimes)
                                    {
                                        courseData.CoursesTime.Add(new CourseTime
                                        {
                                            ID = index.ToString(),
                                            Code = time.Code,
                                            Numb = time.Numb,
                                            CourseCode = time.CourseCode,
                                            CourseTitle = time.CourseTitle,
                                            RegisteredCount = time.RegisteredCount,
                                            SubjectSectionNo = time.SubjectSectionNo,
                                            StartTime = time.StartTime.Length == 3 ? "0" + time.StartTime : time.StartTime,
                                            EndTime = time.EndTime.Length == 3 ? "0" + time.EndTime : time.EndTime,
                                            Decription = time.Description
                                        });

                                        index = index + 1;
                                    }

                                    courseDay.Courses.Add(courseData);
                                }

                                courseLevel.Days.Add(courseDay);
                            }

                            courseLevels.Add(courseLevel);
                        }

                        // Bind the data to ASP.NET controls here
                        masterRepeater.DataSource = courseLevels;
                        masterRepeater.DataBind();
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected string GetCourseDescFromJadeer(string code, string numb)
        {
            string result = "";
            try
            {
                HttpClient client = new HttpClient();

                string apiUrl = "https://jadeer.pnu.edu.sa/api/CSPS/0/{0}/{1}";

                client.BaseAddress = new Uri(apiUrl);

                // var CourseNumber = GetCourseNumber(numb);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Format(apiUrl, code, RemoveChar(numb))).Result;
                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsAsync<clsCourses>().Result;

                    result = dataObjects.course_specification;

                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            


            return result;
        }
        private string RemoveChar(string code)
        {
            try
            {
                if (code.Length >= 4)
                    return code.Substring(0, code.Length - 1);
                else
                    return code;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return "";
            }

           
        }
        private int GetNumberPosition(string str)
        {
            int index = -1;
            try
            {
                foreach (char ch in str)
                {
                    index++;
                    if (!Char.IsDigit(ch))
                    {
                        return index;
                    }
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
            
            return str.Length;
        }
        private string GetCourseNumber(string numb)
        {
            try
            {
                int number = GetNumberPosition(numb);
                string Numberpart = numb.Substring(0, number);
                return Numberpart;


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return "0";
            }

            
        }

    }
}
