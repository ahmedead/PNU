using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeAds : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string RowsCount { get; set; } = "0";
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
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
                            if(Convert.ToInt32(RowsCount) != 0)
                                query.RowLimit =Convert.ToUInt32(RowsCount);
                            collitem = reqList.GetItems(query);

                            if (collitem != null)
                            {
                                for (int i = 0; i < collitem.Count; i++)
                                {
                                    //if (i == 3)
                                    //    break;
                                    SPListItem listItem = collitem[i];

                                    string strDate = "";
                                    string strDay = "";
                                    string dayEn = "";
                                    string dayAr = "";
                                    if (listItem["MediaDate"] != null)
                                    {
                                        DateTime dtDate = Convert.ToDateTime(listItem["MediaDate"].ToString());
                                        if (PortalHelper.IsArabic)
                                            strDate = dtDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                                        else
                                            strDate = dtDate.ToString("MMM yyyy", new System.Globalization.CultureInfo("en-US"));
                                        strDay = dtDate.Day.ToString();

                                        // English day name
                                         dayEn = dtDate.ToString("dddd", new CultureInfo("en-US"));

                                        // Arabic day name
                                         dayAr = dtDate.ToString("dddd", new CultureInfo("ar-SA"));
                                    }



                                    var eventitem = new dataEvent
                                    {
                                        FromDate = strDate,
                                        NavUrl = PortalHelper.ParentLangSite + "MediaCenter/Pages/AdvertisementDetails.aspx?RequestID=" + listItem["ID"].ToString(),

                                        Title = listItem["Title"].ToString(),
                                        Title_EN = listItem["Title_EN"] == null ? "" : listItem["Title_EN"].ToString(),

                                        day = strDay,
                                        dayEn = dayEn,
                                        dayAr = dayAr,

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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return null;
            }

        }

        private void BindNews()
        {
            try
            {
                
                List<dataEvent> Advertisements = GetAdvertisements();

                if (Advertisements != null && Advertisements.Count > 0)
                {
                    rptEvents.DataSource = Advertisements;
                    rptEvents.DataBind();

                }

                

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

    }
}
