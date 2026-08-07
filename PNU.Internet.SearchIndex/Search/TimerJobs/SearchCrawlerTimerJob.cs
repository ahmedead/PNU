using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using PNU.Internet.SearchIndex.Indexer;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.TimerJobs
{
    /// <summary>
    /// Nightly safety net. Even if event receivers fire correctly during
    /// the day, this job runs a full crawl every night to keep the index
    /// healthy.
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
                    site.Dispose();
                    SearchIndexer.FullCrawl(siteId);
                }
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("TimerJob",
                    JobName, ex.Message);
            }
        }

        public static void Install(SPWebApplication webApp)
        {
            Uninstall(webApp);

            var job = new SearchCrawlerTimerJob(webApp);
            var schedule = new SPDailySchedule
            {
                BeginHour = 2,
                BeginMinute = 30,
                EndHour = 2,
                EndMinute = 45
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
