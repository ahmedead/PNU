using Microsoft.SharePoint;
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
    public partial class ucNewsEvents : UserControl
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
        
        private List<dataEvent> Getevents()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "MediaCenter/Events/"))
                    {
                        SPList list = web.Lists["ActivitiesAndEvents"];
                        if (list != null)
                        {
                            DateTime dtToday = DateTime.Now.Date;
                            SPQuery query = new SPQuery();
                            string dateToday = dtToday.ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("en-US"));


                            DateTime today = DateTime.Now;
                            string formattedToday = today.ToString("yyyy-MM-ddTHH:mm:ssZ", new System.Globalization.CultureInfo("en-US"));

                            query.Query = $@"<Where>
                              <Leq>
                                 <FieldRef Name='FromDate' />
                                 <Value IncludeTimeValue='TRUE' Type='DateTime'>{formattedToday}</Value>
                              </Leq>
                           </Where>
                           <OrderBy>
                              <FieldRef Name='FromDate' Ascending='False' />
                           </OrderBy>";
                            query.RowLimit = 3;
                            SPListItemCollection collitem = list.GetItems(query);
                            List<dataEvent> EventList = new List<dataEvent>();
                            if (collitem != null)
                            {
                                for (int i = 0; i < collitem.Count; i++)
                                {
                                    if (i == 3)
                                        break;
                                    SPListItem listItem = collitem[i];
                                    string strDate = "";
                                    string strDay = "";
                                    if (listItem["FromDate"] != null)
                                    {
                                        DateTime dtDate = Convert.ToDateTime(listItem["FromDate"].ToString());
                                        if (PortalHelper.IsArabic)
                                            strDate = dtDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("ar-AE"));
                                        else
                                            strDate = dtDate.ToString("MMM yyyy", new System.Globalization.CultureInfo("en-US"));
                                        strDay = dtDate.Day.ToString();
                                    }

                                    var eventitem = new dataEvent
                                    {
                                        Date = listItem["Date"].ToString(),
                                        ShortDesc = listItem["ShortDesc"].ToString(),
                                        Faculty = listItem["Faculty"].ToString(),
                                        From = listItem["From"].ToString(),
                                        NavUrl = PortalHelper.ParentLangSite + "MediaCenter/Events/Pages/EventDetails.aspx?view=" + listItem["ID"].ToString(),
                                        //SubCategory = listItem["SubCategory"].ToString(),
                                        //Tag = listItem["Tag"].ToString(),
                                        Title = listItem["Title"].ToString(),
                                        To = listItem["To"].ToString(),
                                        FromDate = strDate,
                                        ToDate = listItem["ToDate"].ToString(),
                                        day = strDay,

                                    };
                                    EventList.Add(eventitem);
                                }
                                return EventList;

                            }
                        }
                    }
                }


                return null;

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
            {//List<clsLookUP> MediaTypes = busclsRequestsList.GetAllItemsLookup("MediaTypes");
                List<clsLookUP> MainCategory = busclsRequestsList.GetAllItemsLookup("MainCategory");
                List<dataEvent> Events = Getevents();
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
                            //        if (r.MediaTypes_EN.Contains("Video"))
                            //            r.IsVideo = true;
                            //        else
                            //            r.IsVideo = false;
                            //    }
                            //}

                            objCat.Requests.AddRange(_aalDataByLevel);



                        }

                        if (Events != null && Events.Count > 0)
                        {
                            objCat.Events = new List<dataEvent>();
                            objCat.Events.AddRange(Events);

                        }

                        _allCategories.Add(objCat);
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
