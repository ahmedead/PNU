using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.OracleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.InstructorCourses
{
    public class InstructorCoursesTimerjob : SPJobDefinition
    {
        public const string Instructor_Courses_JOB_NAME = "InstructorCoursesTimerJob";
        public InstructorCoursesTimerjob() : base() { }
        public InstructorCoursesTimerjob(SPWebApplication webApp) : base(Instructor_Courses_JOB_NAME, webApp, null, SPJobLockType.ContentDatabase)
        {

            this.Title = "Instructor Courses Timer Job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {

                OracleDBContext_Timer.SaveInstructorCoursesDataToList(OracleDBContext_Timer.GetInstCourseListNameConfigurationValue());
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Colleges Members Timer Job", ex.Message);
            }

        }

    }
}
