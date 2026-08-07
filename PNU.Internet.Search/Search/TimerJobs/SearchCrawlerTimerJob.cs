using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using PNU.Internet.Search.Indexer;
using Portal.Main.Helper;

namespace PNU.Internet.Search.TimerJobs
{
    /// <summary>
    /// Nightly safety net. Even if event receivers fire correctly during
    /// the day, this job runs a full crawl every night to keep the index
    /// healthy (catches deletes that bypassed events, manual SQL changes,
    /// content-deployment imports, etc).
    ///
    /// Install via a Feature receiver - see install snippet at the bottom
    /// of this file.
    /// </summary>
    public class SearchCrawlerTimerJob : SPJobDefinition
    {
        public const string JobName = "PNU_SearchCrawler";

        public SearchCrawlerTimerJob() : base() { }

        public SearchCrawlerTimerJob(SPWebApplication webApp)
            : base(JobName, webApp, null, SPJobLockType.Job)
        {
            this.Title = "PNU Custom Search - nightly crawl";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                SPWebApplication webApp = this.WebApplication;
                if (webApp == null) return;

                foreach (SPSite site in webApp.Sites)
                {
                    Guid siteId = site.ID;
                    site.Dispose();          // dispose enumerator handle
                    SearchIndexer.FullCrawl(siteId);
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        // ----------------------------------------------------------------
        //  Install / Uninstall - call these from a Feature receiver.
        // ----------------------------------------------------------------
        public static void Install(SPWebApplication webApp)
        {
            // remove any existing job with the same name first
            Uninstall(webApp);

            var job = new SearchCrawlerTimerJob(webApp);

            // Run every night at 02:30
            var schedule = new SPDailySchedule
            {
                BeginHour   = 2,
                BeginMinute = 30,
                EndHour     = 2,
                EndMinute   = 45
            };
            job.Schedule = schedule;
            job.Update();
        }

        public static void Uninstall(SPWebApplication webApp)
        {
            foreach (SPJobDefinition def in webApp.JobDefinitions)
            {
                if (def.Name == JobName)
                {
                    def.Delete();
                    break;
                }
            }
        }
    }
}
