using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common
{
    public partial class ucGuidelines : UserControl
    {
        public string ListName { get; set; }
        public string WebPartTitle { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ListName == null || ListName == "")
                    ListName = "Guidelines";
                if (WebPartTitle == null || WebPartTitle == "")
                    WebPartTitle = "الأدلة الإرشادية";

                imgCoiledArrow.Attributes["src"] = SPFactory.GetPNUresResource("coiledarrow");

                if (!Page.IsPostBack)
                {
                    pnlData.Visible = false;
                    lblWPTitle.Text = WebPartTitle;
                    BindGuidelines();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        private void BindGuidelines()
        {
            try
            {
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

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {
                                List<clsGuidelines> _AllItems = new List<clsGuidelines>();
                                foreach (SPListItem item in collitem)
                                {
                                    clsGuidelines objGuideline = new clsGuidelines();
                                    objGuideline.ID = item.ID.ToString();
                                    objGuideline.Title = item["Title"] == null ? "" : item["Title"].ToString();


                                    if (item.Attachments != null && item.Attachments.Count > 0)
                                    {
                                        SPAttachmentCollection attachments = item.Attachments;
                                        SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);

                                        objGuideline.URL = SPContext.Current.Web.Url + "/" + file.Url;// item["URL"] == null ? "" : item["URL"].ToString();
                                        objGuideline.Extension = new FileInfo(file.Name).Extension.Replace('.', ' ');
                                        long FileSizeinKB = file.Length / 1024;
                                        long FileSizeinMB = FileSizeinKB / 1024;
                                        if (FileSizeinMB > 0)
                                            objGuideline.FileSize = FileSizeinMB.ToString() + " MB";
                                        else
                                            objGuideline.FileSize = FileSizeinKB.ToString() + " KB";

                                        //objGuideline.FileSize = FileUtils.byteCountToDisplaySize(fileObj.length());

                                    }

                                    _AllItems.Add(objGuideline);



                                }
                                if (_AllItems != null && _AllItems.Count > 0)
                                {
                                    rep.DataSource = _AllItems;
                                    rep.DataBind();
                                    pnlData.Visible = true;

                                }

                            }


                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }




    }


    public class clsGuidelines
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string URL { get; set; }
        public string FileSize { get; set; }
        public string Extension { get; set; }
        
    }

    
}
