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



namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Advertisements
{

    public partial class ucAllCollegeAdvs : UserControl
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }



        }

        private List<dataEvent> GetAdvertisements()
        {
            try
            {



                
                SPListItemCollection collitem;
                List<dataEvent> EventList = new List<dataEvent>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string CollegeCode = "";

                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["DepartmentDetails"];
                            if (list != null)
                            {

                                SPListItemCollection collitem1 = list.GetItems();
                                if (collitem1 != null)
                                {
                                    if (collitem1.Count > 0)
                                        if (collitem1[0]["DeptCode"] != null)
                                            CollegeCode = collitem1[0]["DeptCode"].ToString().Trim();


                                }


                            }
                        }
                    }


                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/MediaCenterAdmin/"))
                        {

                            SPList reqList = web.Lists["AdvertisementsRequests"];

                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
      <And>
         <Eq>
            <FieldRef Name='RequestStatus' />
            <Value Type='Choice'>Approved</Value>
         </Eq>
         <Eq>
            <FieldRef Name='FacultyName_EN' />
            <Value Type='Text'>{CollegeCode}</Value>
         </Eq>
      </And>
   </Where>
   <OrderBy>
      <FieldRef Name='MediaDate' Ascending='False' />
   </OrderBy>";
                            //query.RowLimit = 3;
                            collitem = reqList.GetItems(query);

                            if (collitem != null)
                            {
                                for (int i = 0; i < collitem.Count; i++)
                                {
                                    SPListItem listItem = collitem[i];
                                    string strDate = "";
                                    string strDay = "";
                                    string strIso = "";

                                    if (listItem["MediaDate"] != null)
                                    {
                                        DateTime dtDate = Convert.ToDateTime(listItem["MediaDate"].ToString());
                                        System.Globalization.CultureInfo culture = PortalHelper.IsArabic
                                            ? new System.Globalization.CultureInfo("ar-AE")
                                            : new System.Globalization.CultureInfo("en-US");

                                        culture.DateTimeFormat.Calendar = new System.Globalization.GregorianCalendar();

                                        strDate = dtDate.ToString("dd MMMM yyyy", culture);
                                        strDay = dtDate.ToString("dddd", culture);
                                        strIso = dtDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                    }

                                    var eventitem = new dataEvent
                                    {
                                        FromDate = strDate,
                                        IsoDate = strIso,
                                        NavUrl = "AdvertisementDetails.aspx?RequestId=" + listItem["ID"].ToString(),
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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return null;
            }

        }

        private void BindNews()
        {
            try
            {
                List<dataEvent> Advertisements = GetAdvertisements();
                rptAdvs.DataSource = Advertisements;
                rptAdvs.DataBind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }


    }


}
