using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System.Linq;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter
{
    public partial class ucAllNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<clsRequestsList> allData = busclsRequestsList.GetAllItems();
                    if (allData == null) allData = new List<clsRequestsList>();

                    // NEWS ONLY — Digital Media has CatID == "2"
                    List<clsRequestsList> newsItems = allData
                        .Where(d => d.CatID != "2")
                        .OrderByDescending(d =>
                        {
                            DateTime dt;
                            return DateTime.TryParse(d.Created, out dt) ? dt : DateTime.MinValue;
                        })
                        .ToList();

                    rptNews.DataSource = newsItems;
                    rptNews.DataBind();

                    // ----- Faculty filter -----
                    // Build filter options from distinct FacultyName / FacultyName_EN
                    // values present in the news items.
                    var facultiesWithNews = newsItems
                        .Where(n => !string.IsNullOrEmpty(n.FacultyName))
                        .GroupBy(n => new { n.FacultyName, n.FacultyName_EN })
                        .Select((g, idx) => new clsRequestsList
                        {
                            ID = (idx + 1).ToString(),
                            FacultyName = g.Key.FacultyName,
                            FacultyName_EN = g.Key.FacultyName_EN
                        })
                        .OrderBy(f =>
                            PortalHelper.IsArabic ? f.FacultyName : f.FacultyName_EN,
                            StringComparer.CurrentCulture)
                        .ToList();

                    bool hasFacultyFilter = facultiesWithNews.Count > 0;
                    pnlFacultyFilter.Visible = hasFacultyFilter;
                    if (hasFacultyFilter)
                    {
                        rptFacultyFilter.DataSource = facultiesWithNews;
                        rptFacultyFilter.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }
    }

}
