using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{
    public partial class ucAllAdvertisements : UserControl
    {
        private int iPageSize = 6;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                LoadArticles();
        }

        void LoadArticles()
        {
            ArrayList qryParam = new ArrayList();
            var ListName = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PagesList");
            var itemsColl = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, ListName, qryParam, "Created");
            List<ArticlesList> articlesList = new List<ArticlesList>();
            

           

            foreach (SPListItem item in itemsColl)
            {
                string url = "";

                if (item.Name.Contains("default.aspx"))
                    continue;


                if (item["PublishingPageImage"] != null)
                {
                    url = item["PublishingPageImage"].ToString().Substring(item["PublishingPageImage"].ToString().IndexOf("src=", StringComparison.Ordinal) + 5,
                                       item["PublishingPageImage"].ToString().IndexOf("style=", StringComparison.Ordinal) - (
                                         item["PublishingPageImage"].ToString().IndexOf("src=", StringComparison.Ordinal) + 7));
                }
                else
                {
                   url = @"\Style Library\pnu.jpg";
                }



                var ArticleDate = Convert.ToDateTime(item.Properties["ArticleStartDate"]);

                var Title = item["Title"] != null ? item["Title"].ToString() : "";
                var ArticleEditor = item.Properties["ArticleEditor"] != null ? item.Properties["ArticleEditor"].ToString() : "";
                string Navurl = item.Name != null ? item.Name.ToString() : "";
                // link.NavigateUrl = "/ar/MediaCenter/News/Pages/" + Navurl

                var articleItem = new ArticlesList
                {
                    Date = ArticleDate.ToString("dd/MMMM/yyyy"),
                    ImageUrl = url,
                    Title = Title,
                    Link = Navurl,
                    ArticleEditor= ArticleEditor


                };

                articlesList.Add(articleItem);
            }


            PagedDataSource pdsData = new PagedDataSource();
            //DataView dv = new DataView(EventList);
            pdsData.DataSource = articlesList;
            pdsData.AllowPaging = true;
            pdsData.PageSize = iPageSize;
            if (ViewState["PageNumber"] != null)
                pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber"]) - 1;
            else
                pdsData.CurrentPageIndex = 0;
            if (pdsData.PageCount > 1)
            {
                Repeater1.Visible = true;
                ArrayList alPages = new ArrayList();
                for (int i = 1; i <= pdsData.PageCount; i++)
                    alPages.Add((i).ToString());
                Repeater1.DataSource = alPages;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.Visible = false;
            }

            rptNews.DataSource = pdsData;
            rptNews.DataBind();
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
        public string Link { get; set; }
        public string Comments { get; set; }

}
}
