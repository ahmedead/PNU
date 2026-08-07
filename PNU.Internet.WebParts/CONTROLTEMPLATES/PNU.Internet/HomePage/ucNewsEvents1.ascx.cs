using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucNewsEvents1 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindNews();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            

        }
        
        private List<dataEvent> GetAdvertisements()
        {
            try
            {

                SPQuery query = new SPQuery();
                query.Query = @"<Where>
                          <And>
                             <Eq>
                                <FieldRef Name='RequestStatus' />
                                <Value Type='Choice'>Approved</Value>
                             </Eq>
                             <Eq>
                                <FieldRef Name='IsHome' />
                                <Value Type='Boolean'>1</Value>
                             </Eq>
                          </And>
                       </Where>
                       <OrderBy>
                          <FieldRef Name='MediaDate' Ascending='False' />
                       </OrderBy>";
                SPListItemCollection collitem;
                //List<clsRequestsList> _returnList = new List<clsRequestsList>();
                List<dataEvent> EventList = new List<dataEvent>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                        {
                            SPList reqList = web.Lists["AdvertisementsRequests"];
                            query.RowLimit = 3;
                            collitem = reqList.GetItems(query);

                            if (collitem != null)
                            {
                                for (int i = 0; i < collitem.Count; i++)
                                {
                                    if (i == 3)
                                        break;
                                    SPListItem listItem = collitem[i];
                                    string strDate = "";
                                    string strDay = "";
                                    if (listItem["MediaDate"] != null)
                                    {
                                        DateTime dtDate = Convert.ToDateTime(listItem["MediaDate"].ToString());
                                        if (PortalHelper.IsArabic)
                                            strDate = dtDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                                        else
                                            strDate = dtDate.ToString("MMM yyyy", new System.Globalization.CultureInfo("en-US"));
                                        strDay = dtDate.Day.ToString();
                                    }

                                    var eventitem = new dataEvent
                                    {
                                        FromDate = strDate,
                                        NavUrl = PortalHelper.ParentLangSite + "MediaCenter/Pages/AdvertisementDetails.aspx?RequestID=" + listItem["ID"].ToString(),

                                        Title = listItem["Title"].ToString(),
                                        Title_EN = listItem["Title_EN"] == null ? "" : listItem["Title_EN"].ToString(),

                                        day = strDay,

                                    };
                                    EventList.Add(eventitem);
                                }
                                

                            }


                        }
                    }
                });

                return EventList;
               

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return null;
            }
            
        }

        private void BindNews()
        {
            try
            {
                List<clsLookUP> MainCategory = busclsRequestsList.GetAllItemsLookup("MainCategory");
                List<dataEvent> Advertisements = GetAdvertisements();
                List<clsAllRequestsList> _allCategories = new List<clsAllRequestsList>();
                int i = 1;

                if (MainCategory != null && MainCategory.Count > 0)
                {

                    foreach (clsLookUP c in MainCategory)
                    {
                        clsAllRequestsList objCat = new clsAllRequestsList();
                        objCat.ID = i.ToString();
                        i = i + 1;
                        objCat.MainCategory = c.Title;
                        objCat.MainCategory_EN = c.TitleEn;

                        List<clsRequestsList> _aalDataByLevel = new List<clsRequestsList>();
                        _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategory(objCat.MainCategory, 2);

                        if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                        {
                            //_aalDataByLevel[0].ID = "1";
                            objCat.Requests = new List<clsRequestsList>();
                            //objCat.Requests.AddRange(_aalDataByLevel);


                            objCat.Requests = new List<clsRequestsList>();
                            //if (!c.Title.Contains("الأخبار الرئيسية"))

                            //{
                            //    foreach (var r in _aalDataByLevel)
                            //    {
                            //        if(r.MediaTypes_EN != null)
                            //        {
                            //            if (r.MediaTypes_EN.Contains("Video"))
                            //                r.IsVideo = true;
                            //            else
                            //                r.IsVideo = false;
                            //        }
                                    
                            //    }
                            //}

                            objCat.Requests.AddRange(_aalDataByLevel);



                        }

                        if (Advertisements != null && Advertisements.Count > 0)
                        {
                            objCat.Events = new List<dataEvent>();
                            objCat.Events.AddRange(Advertisements);

                        }

                        _allCategories.Add(objCat);



                        // الوسائط الرقمية

                        objCat = new clsAllRequestsList();
                        objCat.ID = (i).ToString();
                        i = i + 1;
                        objCat.MainCategory = SPFactory.GetPNUresResource("DigitalMedia", "AR");
                        objCat.MainCategory_EN = SPFactory.GetPNUresResource("DigitalMedia","EN");

                         _aalDataByLevel = new List<clsRequestsList>();
                        _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategoryNotMainCat( 2);

                        if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                        {
                            objCat.Requests = new List<clsRequestsList>();

                            objCat.Requests = new List<clsRequestsList>();
                            //if (!c.Title.Contains("الأخبار الرئيسية"))

                            //{
                            //    foreach (var r in _aalDataByLevel)
                            //    {
                            //        if (r.MediaTypes_EN != null)
                            //        {
                            //            if (r.MediaTypes_EN.Contains("Video"))
                            //                r.IsVideo = true;
                            //            else
                            //                r.IsVideo = false;
                            //        }
                            //    }
                            //}

                            objCat.Requests.AddRange(_aalDataByLevel);



                        }

                        if (Advertisements != null && Advertisements.Count > 0)
                        {
                            objCat.Events = new List<dataEvent>();
                            objCat.Events.AddRange(Advertisements);

                        }

                        _allCategories.Add(objCat);

                        break;

                    }
                }


                masterRepeater.DataSource = _allCategories;
                masterRepeater.DataBind();

                detailsRepeater.DataSource = _allCategories;
                detailsRepeater.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }

    }
}
