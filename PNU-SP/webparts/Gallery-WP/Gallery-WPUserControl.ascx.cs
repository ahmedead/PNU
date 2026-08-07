using Microsoft.SharePoint;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.Gallery_WP
{
    public partial class Gallery_WPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetSliderImages();
        }


        private void GetSliderImages()
        {
            try
            {
                var _web = SPContext.Current.Web;
                string webUrl = _web.Url;
                using (SPSite site = new SPSite(webUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Images"];
                        if (list != null)
                        {
                            SPListItemCollection spListItemCollection = null;
                            foreach (SPFolder subFolder in list.RootFolder.SubFolders)
                            {
                                if (subFolder.Name.ToLower() == "1") {
                                    SPQuery spQuery = new SPQuery();
                                    spQuery.Folder = subFolder;
                                    spQuery.Query = @"<OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";
                                    spQuery.RowLimit = 4;
                                    spListItemCollection = list.GetItems(spQuery);
                                    StringBuilder stringCollection = new StringBuilder();
                                    string carouselIndString = string.Empty;
                                    int count = 0;
                                    if (spListItemCollection.Count > 0)
                                    {
                                        foreach (SPListItem item in spListItemCollection)
                                        {
                                            string ImageUrl = Convert.ToString(item["EncodedAbsUrl"]);
                                            string ImageName = Convert.ToString(item["Name"]);
                                            if (count == 0)
                                            {
                                                stringCollection.AppendFormat("<div class='item active'><img src = '" + ImageUrl + "' alt = '" + ImageName + "' /></div>");
                                                carouselIndString += "<li class='active' data-slide-to='" + count + "' data-target='#myCarousel'></li>";
                                            }
                                            else
                                            {
                                                carouselIndString += "<li data-slide-to='" + count + "' data-target='#myCarousel'></li>";
                                                stringCollection.AppendFormat("<div class='item'><img src = '" + ImageUrl + "' alt = '" + ImageName + "' /></div >");
                                            }
                                            count++;
                                        }
                                        carouselIndicatordiv.InnerHtml = carouselIndString;
                                        carousaldiv.InnerHtml = stringCollection.ToString();
                                        leftIcon.InnerHtml = "<span class='glyphicon glyphicon-chevron-left'></span><span class='sr-only'>Previous</span>";
                                        rightIcon.InnerHtml = "<span class='glyphicon glyphicon-chevron-right'></span><span class='sr-only'>Next</span>";
                                    }
                                }
                            }
                        }
                        else
                        {
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
