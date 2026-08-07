using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.OracleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.CollegeDepartments
{
    public class CollegeDeptsTimerjob : SPJobDefinition
    {
        public const string COLL_DEPTS_JOB_NAME = "CollegesDeptsTimerJob";

        public CollegeDeptsTimerjob() : base() { }
        public CollegeDeptsTimerjob(SPWebApplication webApp)
            : base(COLL_DEPTS_JOB_NAME, webApp, null, SPJobLockType.ContentDatabase)
        {

            this.Title = "Colleges depts programs Timer job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {

                OracleDBContext_Timer.SaveDataToList(OracleDBContext_Timer.GetWebUrlConfigurationValue(), OracleDBContext_Timer.GetListNameConfigurationValue());
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("COLL_DEPT_PROG Timer Job", ex.Message);
            }

        }
    }
}
