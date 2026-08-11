using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class ucAllAdvertisementsDGA : UserControl
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
                string advertisementWebUrl = currentWebUrl.IndexOf("/Advertisement", StringComparison.OrdinalIgnoreCase) >= 0
                    ? currentWebUrl
                    : currentWebUrl.TrimEnd('/') + "/Advertisement/";

                var itemsColl = Helper.LoadListDynamicByCML(advertisementWebUrl, ListName, qryParam, "Created");
                if (itemsColl == null) return;

                List<ArticlesList> articlesList = new List<ArticlesList>();

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

                    string title = item["Title"] != null ? item["Title"].ToString() : "";
                    string articleEditor = item.Properties["ArticleEditor"] != null ? item.Properties["ArticleEditor"].ToString() : "";
                    string pageName = item.Name ?? "";

                    string navUrl = !string.IsNullOrEmpty(pageName)
                        ? advertisementWebUrl.TrimEnd('/') + "/Pages/" + pageName
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
                        Date = articleDate.ToString("dd/MMMM/yyyy"),
                        ImageUrl = url,
                        Title = title,
                        Link = navUrl,
                        ArticleEditor = articleEditor,
                        Comments = comments
                    };

                    articlesList.Add(articleItem);
                }

                rptAdvertisements.DataSource = articlesList;
                rptAdvertisements.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucAllAdvertisementsDGA.LoadArticles", ex.Message);
            }
        }
    }
}

