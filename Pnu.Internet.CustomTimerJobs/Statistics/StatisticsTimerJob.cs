using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.OracleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.Statistics
{
   public class StatisticsTimerJob : SPJobDefinition
    {
        public const string Statistics_JOB_NAME = "StatisticsTimerJob";
        public StatisticsTimerJob() : base() { }
        public StatisticsTimerJob(SPWebApplication webApp) : base(Statistics_JOB_NAME, webApp, null, SPJobLockType.ContentDatabase)
        {

            this.Title = "Statistics Timer Job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {

                //OracleDBContext_Timer.UpdateStatisticsDataToList(OracleDBContext_Timer.GetStatisticsListNameConfigurationValue());
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Statistics Timer Job", ex.Message);
            }

        }
    }
}
