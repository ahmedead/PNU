using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    public partial class ucCollegeNews : UserControl
    {
        public int TabsCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string CollegeCode = "";
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        //string newsPath = SPContext.Current.Web.ServerRelativeUrl.Contains("/News") ? "" : SPContext.Current.Web.ServerRelativeUrl.TrimEnd('/') + "/News";

                        string newsPath = SPContext.Current.Web.ServerRelativeUrl.TrimEnd('/') + "/News";

                        using (SPWeb web = site.OpenWeb(newsPath))
                        {
                            if (web.Exists)   // OpenWeb doesn't always throw - verify before touching lists
                            {
                                SPList list = web.Lists.TryGetList("DepartmentDetails");
                                if (list != null)
                                {
                                    SPListItemCollection collitem = list.GetItems();
                                    if (collitem != null && collitem.Count > 0)
                                    {
                                        if (collitem[0]["DeptCode"] != null)
                                            CollegeCode = collitem[0]["DeptCode"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                    List<clsRequestsList> _allData = busclsRequestsList.GetCollegeNews(CollegeCode);

                    if (_allData != null && _allData.Count > 0)
                    {
                        rptNews.DataSource = _allData;
                    }

                    // Single DataBind on the control itself:
                    // binds rptNews AND evaluates the <%# %> expressions in the
                    // DGA search/filter header (placeholder, labels, button texts).
                    this.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

    }
}
