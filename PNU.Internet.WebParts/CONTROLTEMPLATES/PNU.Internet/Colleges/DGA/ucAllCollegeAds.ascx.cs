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
    /// <summary>
    /// Advertisements tab - mirrors the provided ucAllCollegeNews pattern:
    /// DepartmentDetails -> DeptCode, then the business layer, then one DataBind()
    /// that binds rptAds AND evaluates the <%# %> resource expressions in the header.
    ///
    /// NOTE: adjust the business-layer call if the method name differs
    /// (busclsRequestsList.GetCollegeAdvertisements is assumed - same family
    /// as GetCollegeNews).
    /// </summary>
    public partial class ucAllCollegeAds : UserControl
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
                        rptAds.DataSource = _allData;
                    }

                    // Single DataBind on the control itself:
                    // binds rptAds AND evaluates the <%# %> header expressions.
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
