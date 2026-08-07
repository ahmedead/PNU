using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles
{
    public partial class ucMediaFiles : UserControl
    {
        private int iPageSize =10;
        public string WebUrl { get; set; } = "/MediaCenter/";
        public string ListName { get; set; } = "MediaFiles";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadWeeklyFiles();
                    LoadMonthlyFiles();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
                LoadWeeklyFiles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "activeTab(1)", true);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber2"] = Convert.ToInt32(e.CommandArgument);
                LoadMonthlyFiles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "activeTab(2)", true);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        private void LoadWeeklyFiles()
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                List<clsMediaFiles> weeklyFiles = new List<clsMediaFiles>();


                clsMediaFiles weekly = null;
                
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_MediaType") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Weekly") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_MediaFileWeb"), ListName, qryParam, 0, "ItemOrder", "True");
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


                PagedDataSource pdsData = new PagedDataSource();

                pdsData.DataSource = weeklyFiles;
                pdsData.AllowPaging = true;
                pdsData.PageSize = iPageSize;
                if (ViewState["PageNumber"] != null)
                    pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber"]) - 1;
                else
                    pdsData.CurrentPageIndex = 0;
                if (pdsData.PageCount > 1)
                {
                    Repeater1.Visible = true;
                    ArrayList alPages = new ArrayList();
                    for (int i = 1; i <= pdsData.PageCount; i++)
                        alPages.Add((i).ToString());
                    Repeater1.DataSource = alPages;
                    Repeater1.DataBind();
                }
                else
                {
                    Repeater1.Visible = false;
                }
                rptWeekly.DataSource = pdsData;
                rptWeekly.DataBind();


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }



        }
        private void LoadMonthlyFiles()
        {
            try
            {
                ArrayList qryParam = new ArrayList();

                List<clsMediaFiles> MonthlyFiles= new List<clsMediaFiles>();

                clsMediaFiles monthly = null;
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_MediaType") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Monthly") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_MediaFileWeb"), ListName, qryParam, 0, "ItemOrder", "True");
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

                        

                        monthly = new clsMediaFiles()
                        {
                            FileName = Convert.ToString(item["Title"]),
                            FilePath = filePath


                        };

                        MonthlyFiles.Add(monthly);


                    }
                }

                PagedDataSource pdsData = new PagedDataSource();

                pdsData.DataSource = MonthlyFiles;
                pdsData.AllowPaging = true;
                pdsData.PageSize = iPageSize;
                if (ViewState["PageNumber2"] != null)
                    pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber2"]) - 1;
                else
                    pdsData.CurrentPageIndex = 0;
                if (pdsData.PageCount > 1)
                {
                    Repeater1.Visible = true;
                    ArrayList alPages = new ArrayList();
                    for (int i = 1; i <= pdsData.PageCount; i++)
                        alPages.Add((i).ToString());
                    Repeater2.DataSource = alPages;
                    Repeater2.DataBind();
                }
                else
                {
                    Repeater2.Visible = false;
                }
                rptMonthly.DataSource = pdsData;
                rptMonthly.DataBind();


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }
    
    }
}
