using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class ucAllAdvertisements : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadArticles();
        }

        void LoadArticles()
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                var ListName = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PagesList");
                if (string.IsNullOrEmpty(ListName)) ListName = "Pages";

                string currentWebUrl = SPContext.Current.Web.Url;
                var itemsColl = Helper.LoadListDynamicByCML(currentWebUrl, ListName, qryParam, "Created");
                if (itemsColl == null) return;

                List<ArticlesList> articlesList = new List<ArticlesList>();
                CultureInfo arCi = new CultureInfo("ar-SA");

                foreach (SPListItem item in itemsColl)
                {
                    if (item == null || string.IsNullOrEmpty(item.Name) || item.Name.Contains("default.aspx"))
                        continue;

                    string url = @"\Style Library\pnu.jpg";
                    if (item["PublishingPageImage"] != null)
                    {
                        try
                        {
                            string imgStr = item["PublishingPageImage"].ToString();
                            int srcIdx = imgStr.IndexOf("src=", StringComparison.OrdinalIgnoreCase);
                            if (srcIdx >= 0)
                            {
                                int styleIdx = imgStr.IndexOf("style=", StringComparison.OrdinalIgnoreCase);
                                if (styleIdx > srcIdx + 5)
                                {
                                    url = imgStr.Substring(srcIdx + 5, styleIdx - (srcIdx + 7)).Trim('"', '\'', ' ');
                                }
                                else
                                {
                                    int quoteEnd = imgStr.IndexOf('"', srcIdx + 5);
                                    if (quoteEnd > srcIdx + 5)
                                    {
                                        url = imgStr.Substring(srcIdx + 5, quoteEnd - (srcIdx + 5)).Trim('"', '\'', ' ');
                                    }
                                }
                            }
                        }
                        catch { }
                    }

                    DateTime articleDate = DateTime.Now;
                    if (item.Properties["ArticleStartDate"] != null)
                    {
                        DateTime.TryParse(Convert.ToString(item.Properties["ArticleStartDate"]), out articleDate);
                    }
                    else if (item["Created"] != null)
                    {
                        DateTime.TryParse(Convert.ToString(item["Created"]), out articleDate);
                    }

                    string title = item["Title"] != null ? item["Title"].ToString() : "";
                    string articleEditor = item.Properties["ArticleEditor"] != null ? item.Properties["ArticleEditor"].ToString() : "";
                    string pageName = item.Name ?? "";

                    string navUrl = !string.IsNullOrEmpty(pageName)
                        ? currentWebUrl.TrimEnd('/') + "/Pages/" + pageName
                        : "";

                    string comments = "";
                    try
                    {
                        if (item["Comments"] != null)
                        {
                            comments = item["Comments"].ToString();
                        }
                        else if (item.Properties["Comments"] != null)
                        {
                            comments = item.Properties["Comments"].ToString();
                        }
                    }
                    catch { }

                    var articleItem = new ArticlesList
                    {
                        Date = articleDate.ToString("dd MMMM yyyy", arCi),
                        DateISO = articleDate.ToString("yyyy-MM-dd"),
                        DayName = articleDate.ToString("dddd", arCi),
                        ImageUrl = url,
                        Title = title,
                        Link = navUrl,
                        ArticleEditor = articleEditor,
                        Comments = comments
                    };

                    articlesList.Add(articleItem);
                }

                // Bind Articles
                rptNews.DataSource = articlesList;
                rptNews.DataBind();

                // Bind Category Filter
                var categories = articlesList
                    .Where(a => !string.IsNullOrEmpty(a.ArticleEditor))
                    .Select(a => a.ArticleEditor)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                if (pnlCategoryFilter != null && rptCategoryFilter != null)
                {
                    if (categories.Count > 0)
                    {
                        pnlCategoryFilter.Visible = true;
                        rptCategoryFilter.DataSource = categories;
                        rptCategoryFilter.DataBind();
                    }
                    else
                    {
                        pnlCategoryFilter.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucAllAdvertisements.LoadArticles", ex.Message);
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
            LoadArticles();
        }
    }

    public class ArticlesList
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ArticleEditor { get; set; }
        public string Date { get; set; }
        public string DateISO { get; set; }
        public string DayName { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
    }
}
