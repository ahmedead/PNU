using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.InstructorCourses
{
    public class CourseDto
    {
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
}
