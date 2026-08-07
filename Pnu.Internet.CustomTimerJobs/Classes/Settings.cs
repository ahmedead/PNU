using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs
{
    public static class Settings
    {


        #region HomePage
        public static string SliderList
        { //TimeLineList/";
            get
            {
                return PortalHelper.ParentLangSite + "Lists/Slider";
            }
        }


        public static string FacultyMembersList
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/CollMembersListName/";
            }
        }
        public static string RequestsList
        { //TimeLineList/";
            get
            {
                return "/ar/MediaCenter/Lists/RequestsList/";
            }
        }

        public static string Courses
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/Instcourses/";
            }
        }

        public static string NewStudyPlan
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/NewStudyPlan/";
            }
        }

        public static string NewStudyPlanListOnly
        { //TimeLineList/";
            get
            {
                return "NewStudyPlan";
            }
        }
        public static string AllPrograms
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllPrograms/";
            }
        }

        public static string AcademicCredits
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AcademicCredits/";
            }
        }

        public static string AllFaculties
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllFaculties/";
            }
        }

        public static string AllFacultyDepartments
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllFacultyDepartments/";
            }
        }
        public static string AllDepartmentPrograms
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllDepartmentPrograms/";
            }
        }

        public static string Faculties
        {
            get
            {
                return "Faculties";
            }
        }

        public static string AllTamOutSourcingList
        {
            get
            {
                return PortalHelper.ParentLangSite + "Lists/AllTamOutSourcingList/";
            }
        }
        #endregion


    }


}
