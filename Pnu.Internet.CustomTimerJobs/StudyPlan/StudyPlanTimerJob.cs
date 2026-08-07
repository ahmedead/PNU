using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pnu.Internet.CustomTimerJobs.OracleContext;
namespace Pnu.Internet.CustomTimerJobs.StudyPlan
{
    public class StudyPlanTimerJob : SPJobDefinition
    {
        public const string StudyPlan_JOB_NAME = "StudyPlanTimerJob";


        public StudyPlanTimerJob() : base() { }

        public StudyPlanTimerJob(SPWebApplication webApp) : base(StudyPlan_JOB_NAME, webApp, null, SPJobLockType.ContentDatabase)
        {

            this.Title = "Study Plan Timer Job";
        }


        public override void Execute(Guid targetInstanceId)
        {
            try
            {

                List<PLANS_ELEC_U> PLANS_ELEC_UList = OracleDBContext_Timer.GetPLANS_ELEC_U("CUSTAPP.MOBAPPL_PLANS_ELEC_U_M");
                List<PLANS_ELEC_C> PLANS_ELEC_CList = OracleDBContext_Timer.GetPLANS_ELEC_C("CUSTAPP.MOBAPPL_PLANS_ELEC_C_M");
                List<PLANS_ELEC_P> PLANS_ELEC_PList = OracleDBContext_Timer.GetPLANS_ELEC_P("CUSTAPP.MOBAPPL_PLANS_ELEC_P_M");
                //List<ProgramsDto> ProgramsList = OracleDBContext_Timer.GetOnlyProgramsFromOracleDB();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPFactory.CreatePLANS_ELEC_UList(PLANS_ELEC_UList, "PLANS_ELEC_U");
                    SPFactory.CreatePLANS_ELEC_CList(PLANS_ELEC_CList, "PLANS_ELEC_C");
                    SPFactory.CreatePLANS_ELEC_PList(PLANS_ELEC_PList, "PLANS_ELEC_P");

                });

                //EN

                List<PLANS_ELEC_U> PLANS_ELEC_UList_EN = OracleDBContext_Timer.GetPLANS_ELEC_U("CUSTAPP.MOBAPPL_PLANS_ELEC_UEN_M");
                List<PLANS_ELEC_C> PLANS_ELEC_CList_EN = OracleDBContext_Timer.GetPLANS_ELEC_C("CUSTAPP.MOBAPPL_PLANS_ELEC_CEN_M");
                List<PLANS_ELEC_P> PLANS_ELEC_PList_EN = OracleDBContext_Timer.GetPLANS_ELEC_P("CUSTAPP.MOBAPPL_PLANS_ELEC_PEN_M");
                //List<ProgramsDto> ProgramsList = OracleDBContext_Timer.GetOnlyProgramsFromOracleDB();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPFactory.CreatePLANS_ELEC_UList(PLANS_ELEC_UList_EN, "PLANS_ELEC_U_EN");
                    SPFactory.CreatePLANS_ELEC_CList(PLANS_ELEC_CList_EN, "PLANS_ELEC_C_EN");
                    SPFactory.CreatePLANS_ELEC_PList(PLANS_ELEC_PList_EN, "PLANS_ELEC_P_EN");

                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("StudyPlan Timer Job", ex.Message);
            }

        }



    }
}
