using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.MediaCenter
{
    public partial class ucAllDigitalMedia : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<clsRequestsList> allData = busclsRequestsList.GetAllItems();
                    if (allData == null) allData = new List<clsRequestsList>();

                    // DIGITAL MEDIA ONLY — CatID == "2"
                    List<clsRequestsList> mediaItems = allData
                        .Where(d => d.CatID == "2")
                        .OrderByDescending(d =>
                        {
                            DateTime dt;
                            return DateTime.TryParse(d.Created, out dt) ? dt : DateTime.MinValue;
                        })
                        .ToList();

                    rptMedia.DataSource = mediaItems;
                    rptMedia.DataBind();

                    // ----- Faculty filter -----
                    // Build filter options from distinct FacultyName / FacultyName_EN
                    // values present in the media items.
                    var facultiesWithMedia = mediaItems
                        .Where(m => !string.IsNullOrEmpty(m.FacultyName))
                        .GroupBy(m => new { m.FacultyName, m.FacultyName_EN })
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

                    bool hasFacultyFilter = facultiesWithMedia.Count > 0;
                    pnlFacultyFilter.Visible = hasFacultyFilter;
                    if (hasFacultyFilter)
                    {
                        rptFacultyFilter.DataSource = facultiesWithMedia;
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
