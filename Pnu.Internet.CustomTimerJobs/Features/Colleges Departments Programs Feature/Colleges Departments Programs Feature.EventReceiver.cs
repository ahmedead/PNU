using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.CollegeDepartments;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Pnu.Internet.CustomTimerJobs.Features.Colleges_Departments_Programs_Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("66da5184-8a99-4cec-a063-f170e1bccc84")]
    public class Colleges_Departments_Programs_FeatureEventReceiver : SPFeatureReceiver
    {
        const string JOB_NAME = "CollegesDeptsTimerJob";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            // create the timer job when the feature is activated
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication; // get the web application reference

            // Delete the job, If its already created
            foreach (SPJobDefinition job in webApp.JobDefinitions)
                if (job.Name == JOB_NAME)
                {
                    job.Delete();
                }

            //Create the timer job by creating the instance of the class we have created
            CollegeDeptsTimerjob customTimerJob = new CollegeDeptsTimerjob(webApp);

            // Create a schedule interval for timer job
            // You can create Hourly, Daily,  Weekly schedules as well
            SPDailySchedule customSchedule = new SPDailySchedule();
            customSchedule.BeginHour = 12;
            customSchedule.EndHour = 12;

            // assign the schedule to the timer job
            customTimerJob.Schedule = customSchedule;
            customTimerJob.Update();

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;

            foreach (SPJobDefinition job in webApp.JobDefinitions)
            {
                if (job.Name == JOB_NAME)
                {
                    job.Delete();
                }
            }
        }
    }
}
