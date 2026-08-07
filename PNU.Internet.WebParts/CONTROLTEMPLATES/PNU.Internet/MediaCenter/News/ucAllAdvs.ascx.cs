using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucAllAdvs : UserControl
    {
        public int TabsCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack)
                {
                    
                    List<clsAdvertisements> _allData = new List<clsAdvertisements>();
                    

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
                    SPListItemCollection collitem;
                    
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                            {
                                SPList reqList = web.Lists["AdvertisementsRequests"];
                                
                                collitem = reqList.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {
                                    _allData = SPFactory.MapListItemsToClass<clsAdvertisements>(collitem); 


                                }


                            }
                        }
                    });

                    if (_allData != null && _allData.Count > 0)
                    {
                        List<clsAllRequestsList> _allCategories = new List<clsAllRequestsList>();
                        List<clsAdvertisements> Categories = new List<clsAdvertisements>();
                        Categories = _allData.GroupBy(d => new { d.MainCategory }).Select(group => group.First()).ToList();
                        int i = 1;

                        clsAllRequestsList objCat = new clsAllRequestsList();
                        objCat.ID = i.ToString();
                        i = i + 1;
                        objCat.MainCategory = "اخر الإعلانات";
                        objCat.MainCategory_EN ="Latest Advertisements";

                        List<clsAdvertisements> _aalDataByLevel = new List<clsAdvertisements>();
                        _aalDataByLevel = _allData;

                        if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                        {
                            objCat.Advertisements = new List<clsAdvertisements>();

                            objCat.Advertisements.AddRange(_aalDataByLevel);
                        }

                        _allCategories.Add(objCat);

                        masterRepeater.DataSource = _allCategories;
                        masterRepeater.DataBind();

                        detailsRepeater.DataSource = _allCategories;
                        detailsRepeater.DataBind();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('1');", true);
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
