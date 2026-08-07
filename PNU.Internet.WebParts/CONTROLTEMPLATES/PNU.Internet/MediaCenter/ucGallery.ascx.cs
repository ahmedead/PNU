using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter
{
    public class dataExhibtion
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string imageUrl { get; set; }
        public string NavUrl { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ShortDesc { get; set; }
        public string Faculty { get; set; }
        public string Tag { get; set; }
    }
    public partial class ucGallery : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindGallery();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        protected SPListItemCollection LoadData(int i)
        {
            try
            {
                var listname = "Gallery";
                ArrayList qryParam = new ArrayList();
                string SiteURL = SPContext.Current.Site.Url;
                if (!SiteURL.Contains(PortalHelper.ParentLangSite + "MediaCenter/Gallery"))
                    SiteURL = SPContext.Current.Site.Url + PortalHelper.ParentLangSite + "MediaCenter/Gallery/";
                return Helper.LoadListDynamicByCML(SiteURL, listname, qryParam);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return null;
            }
            

        }
        private void BindGallery()
        {
            try
            {//string newPortalURL = "https://newportal.pnu.edu.sa/ar/MediaCenter/versions/";
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                // using (SPSite site = new SPSite(newPortalURL))
                {
                    using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "MediaCenter/Gallery/"))
                    {
                        SPList list = web.Lists["Gallery"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            SPQuery query2 = new SPQuery();

                            query.Query = "<Query> <OrderBy> <FieldRef Name='Created' Ascending='False' /> </OrderBy> </Query> <ViewFields> <FieldRef Name='Title' /> <FieldRef Name='Date' /> <FieldRef Name='Desc' /> <FieldRef Name='Faculty' /> <FieldRef Name='Tag' /> <FieldRef Name='ImageUrl' /> </ViewFields> <QueryOptions />";

                            SPListItemCollection collitem = LoadData(1);
                            List<dataExhibtion> exhibtionList = new List<dataExhibtion>();

                            if (collitem != null)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    SPListItem listItem = collitem[i];
                                    DateTime FromdateValue = Convert.ToDateTime(listItem["FromDate"]);
                                    var FromDate = FromdateValue.ToString("dd MMMM yyyy");
                                    var exhibtion = new dataExhibtion
                                    {
                                        Title = listItem["Title"].ToString(),
                                        Faculty = listItem["Faculty"].ToString(),
                                        Tag = listItem["Tag"].ToString(),
                                        FromDate = FromDate,
                                        ShortDesc = listItem["ShortDesc"].ToString(),
                                        imageUrl = listItem["ImageUrl"].ToString(),
                                        NavUrl = PortalHelper.ParentLangSite + "MediaCenter/Gallery/Pages/exhibition-details.aspx?view=" + listItem["ID"].ToString(),
                                    };

                                    exhibtionList.Add(exhibtion);

                                }
                                rptGallery.DataSource = exhibtionList;
                                rptGallery.DataBind();

                            }

                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

    }
}
