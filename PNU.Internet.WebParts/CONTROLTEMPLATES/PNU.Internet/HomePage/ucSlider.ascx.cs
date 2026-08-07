using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucSlider : UserControl
    {
        public string ListName { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string RowsCount { get; set; } = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<clsSlider> _AllItems = busclsSlider.GetAllItems();

                    if (_AllItems != null && _AllItems.Count > 0)
                        _AllItems[0].ClassName = " active";
                    int limit;
                    if (int.TryParse(RowsCount, out limit) && limit > 0)
                    {
                        // 2. Take only the specified number of rows
                        _AllItems = _AllItems.Take(limit).ToList();
                    }


                    // Inside your Page_Load or Controller action
                    foreach (var slider in _AllItems)
                    {

                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {

                                using (SPWeb web = site.OpenWeb("ar"))
                                {
                                    string targetLib = "SliderImages"; // The name of your target document library

                                    // Check if files exist in the library before running
                                    string checkUrl = $"{targetLib}/{slider.ID}/hero-lg.avif";
                                    SPFile checkFile = web.GetFile(checkUrl);

                                    if (!checkFile.Exists)
                                    {
                                        ImageAutomation.ConvertAndUploadToLibrary(web, slider.PublishingRollupImage, targetLib, slider.ID);
                                    }
                                }
                            }
                        });


                        using (SPWeb web = SPContext.Current.Site.OpenWeb())
                        {
                            
                        }


                    }


                    rptSlider.DataSource = _AllItems;
                    rptSlider.DataBind();

                    rptIndicators.DataSource = _AllItems;
                    rptIndicators.DataBind();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        
        
    }
}
