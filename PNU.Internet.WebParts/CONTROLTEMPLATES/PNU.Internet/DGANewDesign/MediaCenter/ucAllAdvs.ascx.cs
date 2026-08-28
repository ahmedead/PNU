using Microsoft.SharePoint;
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
    public partial class ucAllAdvs : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<clsAdvertisements> _allData = new List<clsAdvertisements>();

                    // Load approved advertisements from the "Main" faculty (preserved from
                    // the original logic). The in-page filter is by SearchCategory, NOT faculty.
                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                       <And>
                                          <Eq>
                                             <FieldRef Name='RequestStatus' />
                                             <Value Type='Choice'>Approved</Value>
                                          </Eq>
                                          <Eq>
                                             <FieldRef Name='FacultyName' />
                                             <Value Type='Text'>Main</Value>
                                          </Eq>
                                       </And>
                                    </Where>
                                    <OrderBy>
                                       <FieldRef Name='MediaDate' Ascending='False' />
                                    </OrderBy>";

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                            {
                                SPList reqList = web.Lists["AdvertisementsRequests"];
                                SPListItemCollection collitem = reqList.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {
                                    _allData = SPFactory.MapListItemsToClass<clsAdvertisements>(collitem);
                                }
                            }
                        }
                    });

                    if (_allData == null) _allData = new List<clsAdvertisements>();

                    // ----- Bind the cards repeater -----
                    rptAdvs.DataSource = _allData;
                    rptAdvs.DataBind();

                    // ----- SearchCategory filter -----
                    // Build options from distinct SearchCategory values present in the data.
                    var searchCatsWithAdvs = _allData
                        .Where(a => !string.IsNullOrEmpty(a.SearchCategory))
                        .GroupBy(a => new { a.SearchCategory, a.SearchCategory_EN })
                        .Select((g, idx) => new clsAdvertisements
                        {
                            ID = (idx + 1).ToString(),
                            SearchCategory = g.Key.SearchCategory,
                            SearchCategory_EN = g.Key.SearchCategory_EN
                        })
                        .OrderBy(c =>
                            PortalHelper.IsArabic ? c.SearchCategory : c.SearchCategory_EN,
                            StringComparer.CurrentCulture)
                        .ToList();

                    bool hasSearchCategoryFilter = searchCatsWithAdvs.Count > 0;
                    pnlSearchCategoryFilter.Visible = hasSearchCategoryFilter;
                    if (hasSearchCategoryFilter)
                    {
                        rptSearchCategoryFilter.DataSource = searchCatsWithAdvs;
                        rptSearchCategoryFilter.DataBind();
                    }

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
