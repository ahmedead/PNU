using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Pnu.Internet.CustomTimerJobs.Statistics;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Pnu.Internet.CustomTimerJobs.Features.Statistics_Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
  
    [Guid("b26ed63c-0c8d-4ebd-a6ac-e3cfb67acbeb")]
    public class Statistics_FeatureEventReceiver : SPFeatureReceiver
    {
        const string JOB_NAME = "StatisticsTimerJob";

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
            StatisticsTimerJob customTimerJob = new StatisticsTimerJob(webApp);

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



        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

    }
}
