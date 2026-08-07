using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.OracleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.CollegeMembers
{
    public class CollegeMembersTimerjob : SPJobDefinition
    {
        public const string Colleges_Members_JOB_NAME = "CollegesMembersTimerJob";

        public CollegeMembersTimerjob() : base() { }

        public CollegeMembersTimerjob(SPWebApplication webApp) : base(Colleges_Members_JOB_NAME, webApp, null, SPJobLockType.ContentDatabase)
        {

            this.Title = "Colleges Members Timer Job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {

                OracleDBContext_Timer.SaveCollMemeberDataToList("CollMembersListName");
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Colleges Members Timer Job", ex.Message);
            }

        }

    }
}
