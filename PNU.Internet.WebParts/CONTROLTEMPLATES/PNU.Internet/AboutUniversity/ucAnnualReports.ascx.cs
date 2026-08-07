using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System.Web;
using Org.BouncyCastle.Ocsp;
using System.IO;
using System.ComponentModel;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucAnnualReports : UserControl
    {
        private int iPageSize = 10;
        public string WebUrl { get; set; } = "/AboutUniversity/";
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; } = "AnnualReports";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadAnnualReports();
                    
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        
        

        private void LoadAnnualReports()
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                List<clsMediaFiles> weeklyFiles = new List<clsMediaFiles>();


                clsMediaFiles weekly = null;

                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())

                    {
                        SPList list = web.Lists[ListName];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = @"<Where>
                                     <Eq>
                                        <FieldRef Name='Visibility' />
                                        <Value Type='Boolean'>1</Value>
                                     </Eq>
                               </Where>
                               <OrderBy>
                                  <FieldRef Name='ItemOrder' Ascending='True' />
                               </OrderBy>";

                            var CollITems = list.GetItems(query);
                            if (CollITems != null && CollITems.Count > 0)
                            {
                                foreach (SPListItem item in CollITems)
                                {

                                    string filePath = "";
                                    if (item.Attachments != null || item.Attachments.Count > 0)
                                    {
                                        SPAttachmentCollection attachments = item.Attachments;

                                        SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                        
                                        filePath = file.ServerRelativeUrl;
                                    }


                                    weekly = new clsMediaFiles()
                                    {
                                        FileName = Convert.ToString(item["Title"]),
                                        FilePath = filePath

                                    };

                                    weeklyFiles.Add(weekly);


                                }
                            }


                            rptWeekly.DataSource = weeklyFiles;
                            rptWeekly.DataBind();


                        }
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
