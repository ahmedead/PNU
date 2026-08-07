using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Main.Helper;
using Newtonsoft.Json;
using System.Data;
using Microsoft.SharePoint;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Web.Caching;
using Microsoft.SharePoint.Publishing;
using System.Text;

namespace Portal.Main.WebApp.Handlers
{
    /// <summary>
    /// Summary description for PortalHandler
    /// </summary>
    public class PortalHandler : IHttpHandler
    {
        #region Private Fields

        private const string XML_START_TAG = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        private HttpContext currContext;

        #endregion

        #region Public Methods

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                currContext = context;
                if (context.Request["op"] != null)
                {
                    switch (context.Request["op"])
                    {
                        case "LoadItems":
                            GetListItems();
                            break;
                        case "loadRss":
                            LoadRSS(context);
                            break;
                        case "getMenu":
                            GetMenu();
                            break;
						case "getTodayDate":
                            GetTodayDate();
                            break;
					}
                }
            }
            catch (Exception ex)
            {
                PortalHelper.LogException(ex);
                var error = new { Message = "Unexpected error has occurred. Please contact system administrator." };
                string data = JsonConvert.SerializeObject(error);
                // write the result items as json 
                currContext.Response.ClearContent();
                currContext.Response.ContentType = "application/json";
                currContext.Response.Write(data);
                currContext.ApplicationInstance.CompleteRequest();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

		/// <summary>
		/// Check if the publishing page is published in the other language site (ar / en)
		/// </summary>  
		private void IsItemPublished()
        {
            if (currContext.Request["itemUrl"] != null)
            {
                string itemUrl = currContext.Request["itemUrl"].ToString();
                var splitted = itemUrl.Split('/');
                var isPublished = false;
                string fileName = splitted[splitted.Length - 1];
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = site.OpenWeb(itemUrl.Substring(0, itemUrl.ToLower().LastIndexOf("/pages/"))))
                        {
                            PublishingWeb pWeb = PublishingWeb.GetPublishingWeb(web);
                            var page = pWeb.GetPublishingPages().ToList().Find(x => x.Name.Equals(fileName));
                            if (page != null)
                            {
                                isPublished = (page.ListItem.Level == SPFileLevel.Published);
                            }
                        }
                    }
                });

                string data = JsonConvert.SerializeObject(isPublished);
                // write the result items as json 
                currContext.Response.ClearContent();
                currContext.Response.ContentType = "application/json";
                currContext.Response.Write(data);
                currContext.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Get Today date for both Hijri and Gregorian
        /// </summary>       
        private void GetTodayDate()
        {
            var arCulture = new CultureInfo("ar-SA");
            var enCulture = new CultureInfo("en-US");

            var dateStringar =  DateTime.Today.Date.ToString("dd/MM/yyyy", arCulture);
            var dateStringen =  DateTime.Today.Date.ToString("dd/MM/yyyy", enCulture);

            // write the result items as json 
            var data = JsonConvert.SerializeObject(new { HijriDate = dateStringar, MiladiDate = dateStringen });
            currContext.Response.ClearContent();
            currContext.Response.ContentType = "application/json";
            currContext.Response.Write(data);
            currContext.ApplicationInstance.CompleteRequest(); 
        }

        /// <summary>
        /// Get List Items from list by viewName
        /// </summary>       
        private void GetListItems()
        {
            if (currContext.Request["listUrl"] != null && currContext.Request["viewName"] != null)
            {
                string listUrl = currContext.Request["listUrl"].ToString();
                string viewName = currContext.Request["viewName"].ToString();
				string calType = string.IsNullOrEmpty(currContext.Request["calType"]) ? "" : currContext.Request["calType"].ToString();
				string[] skipFields = (currContext.Request["skip"] != null) ? currContext.Request["skip"].Split(',') : null;
                DataTable dt = null;
                string cachKey = string.Format("listItem_{0}_{1}", listUrl, viewName);
				if (GetObjectFromCache(cachKey) == null)
				{
					SPWeb web = SPContext.Current.Site.OpenWeb();
					SPList list = web.GetList(listUrl);
					dt = list.GetItems(list.Views[viewName]).GetDataTable();
					if (dt != null)
					{
						RemoveMetadataColumns(dt, skipFields);
					}
					AddObjectToCahce(cachKey, dt);

				}
				else
					dt = (DataTable)GetObjectFromCache(cachKey);

                IsoDateTimeConverter isoDate = new IsoDateTimeConverter();
                isoDate.DateTimeFormat = currContext.Request["df"] == null ? "dd/MM/yyyy" : currContext.Request["df"];
                isoDate.Culture = new System.Globalization.CultureInfo("en-US");

				if (currContext.Request["lang"] == "ar")
				{
					isoDate.Culture = (calType == "miladi") ? new CultureInfo("ar-EG") : new CultureInfo("ar-SA");
				}

				string data = JsonConvert.SerializeObject(dt, isoDate);
                // write the result items as json 
                currContext.Response.ClearContent();
                currContext.Response.ContentType = "application/json";
                currContext.Response.Write(data);
                currContext.ApplicationInstance.CompleteRequest();
            }
        }

		/// <summary>
		/// Gets the home menu.
		/// </summary>
		private void GetMenu()
        {
            if (currContext.Request["listUrl"] != null && currContext.Request["viewName"] != null)
            {
                string listUrl = currContext.Request["listUrl"].ToString();
                string viewName = currContext.Request["viewName"].ToString();
                string subList = currContext.Request["subList"].ToString();

                DataTable dt = null;
                List<MenuItem> items = new List<MenuItem>();
                string cachKey = string.Format("listItem_{0}_{1}", listUrl, viewName);

                if (GetObjectFromCache(cachKey) == null)
                {
                    SPWeb web = SPContext.Current.Site.OpenWeb();

                    SPList list = web.GetList(listUrl);
                    dt = list.GetItems(list.Views[viewName]).GetDataTable();
                    if (dt != null)
                    {
                        RemoveMetadataColumns(dt);
                    }
                    // to get the sub menu for each item 
                    foreach (DataRow item in dt.Rows)
                    {
                        MenuItem topItem = new MenuItem();
                        topItem.Item = new {
							ID = item["ID"],
							Title = item["Title"],
							URL = dt.Columns.Contains("URL") ? item["URL"] : string.Empty,
							OpenInNewTab = dt.Columns.Contains("OpenInNewTab") ? item["OpenInNewTab"] : string.Empty
						};
                        
                        string queryString = string.Format("<Where><And><Eq><FieldRef Name='Parent' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='Visibility' /><Value Type='Boolean'>1</Value></Eq></And></Where>" +
                         "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>", viewName, item["Title"]);
                        SPQuery query = new SPQuery();
                        query.Query = queryString;
                        var dtSub = web.GetList(subList).GetItems(query).GetDataTable();
                        if (dtSub != null)
                        {
                            RemoveMetadataColumns(dtSub);
                            topItem.SubItem = dtSub;
                        }
                        items.Add(topItem);
                    }
                    AddObjectToCahce(cachKey, items);
                }
                else
                    items = GetObjectFromCache(cachKey) as List<MenuItem>;

                string data = JsonConvert.SerializeObject(items);
                // write the result items as json 
                currContext.Response.ClearContent();
                currContext.Response.ContentType = "application/json";
                currContext.Response.Write(data);
                currContext.ApplicationInstance.CompleteRequest();
            }
        }

		/// <summary>
		/// LoadRSS
		/// </summary>       
		private void LoadRSS(HttpContext context)
		{

			if (context.Request["siteUrl"] != null && context.Request["viewName"] != null)
			{
				//initialize the xml 
				StringBuilder sbXml = new StringBuilder();
				sbXml.Append(XML_START_TAG);

				string siteUrl = context.Request["siteUrl"];
				string listName = string.Empty;
				string viewName = context.Request["viewName"];
				string rssTitle = context.Request["RssTitle"];
				string imageUrl = context.Request["ImageUrl"];
				string rssDescription = context.Request["RssDescription"];

				using (SPSite site = new SPSite(string.Format("{0}{1}", SPContext.Current.Site.Url, siteUrl)))
				{
					using (SPWeb web = site.OpenWeb())
					{
						listName = web.UICulture.LCID == 1025 ? "الصفحات" : "Pages";

						sbXml.Append(@"<rss version=""2.0"">");

						sbXml.AppendFormat(@"<channel>
                        <title>{0}</title>
                        <link>{1}</link>
                        <description>{4}</description>
                        <language>ar-SA</language>
                        <lastBuildDate>{2}</lastBuildDate>
                        <copyright></copyright>
                        <ttl>15</ttl>
                        <image>
                          <url>{3}</url>
                        </image>", !string.IsNullOrEmpty(rssTitle) ? rssTitle : site.RootWeb.Title, web.Url, DateTime.Now.ToString("R"), imageUrl, !string.IsNullOrEmpty(rssDescription) ? rssDescription : site.RootWeb.Description);
						DataTable dt = null;
						dt = web.Lists[listName].GetItems(web.Lists[listName].Views[viewName]).GetDataTable();

						if (dt != null)
						{
							foreach (DataRow row in dt.Rows)
							{
								string date = string.Empty;
								try
								{
									date = Convert.ToDateTime(row["ArticleStartDate"]).ToString("R");
								}
								catch { }

								string title = row["Title"].ToString().Replace("&", " ");
								string desc = row["Comments"].ToString().Replace("&", " ");
								sbXml.AppendFormat(@"<item>
                                    <title>{0}</title>
                                    <description>{1}</description>
                                    <link>{2}</link>
                                    <guid isPermaLink=""true"">{2}</guid>
                                    <pubDate>{3}</pubDate>
                                  </item>", title, desc,
											string.Format("{0}/Pages/{1}", web.Url, row["FileLeafRef"]),
										   date);
							}
						}
						sbXml.Append("</channel></rss>");

					}
				}

				// write the result items as xml 
				context.Response.ClearContent();
				context.Response.ContentType = "text/xml";
				context.Response.Write(sbXml.ToString());
				context.ApplicationInstance.CompleteRequest();
			}
		}

		/// <summary>
		/// Add Object To Cache
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="value">value</param>
		/// <param name="minutes">minutes</param>
		private void AddObjectToCahce(string key, object value)
        {
            if (SPContext.Current.Web.CurrentUser == null)
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMinutes(0), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Get Object From Cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private object GetObjectFromCache(string key)
        {
            if (SPContext.Current.Web.CurrentUser != null)
                return null;

            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Remove metadata columns from retrieve datatables
        /// </summary>
        private void RemoveMetadataColumns(DataTable dt, string[] skipFields = default(string[]))
        {
            dt.RemoveColumn("Editor", skipFields);
            dt.RemoveColumn("Author", skipFields);
            dt.RemoveColumn("CheckoutUser", skipFields);
            dt.RemoveColumn("Created", skipFields);
            dt.RemoveColumn("Modified", skipFields);
            dt.RemoveColumn("PeopleInMedia", skipFields);
            dt.RemoveColumn("_ModerationStatus", skipFields);
            dt.RemoveColumn("_ModerationComments", skipFields);
            dt.RemoveColumn("AppAuthor", skipFields);
            dt.RemoveColumn("AppEditor", skipFields);
            dt.RemoveColumn("_UIVersionString", skipFields);
            dt.RemoveColumn("ContentType", skipFields);
            dt.RemoveColumn("FolderChildCount", skipFields);
            dt.RemoveColumn("ItemChildCount", skipFields);
        }

        #endregion

        private class MenuItem
        {
            public object Item { get; set; }
            public object SubItem { get; set; }
        }
	}

}