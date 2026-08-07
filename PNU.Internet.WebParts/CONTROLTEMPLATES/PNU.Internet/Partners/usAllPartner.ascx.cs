using iTextSharp.text;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using Microsoft.SharePoint.BusinessData.Runtime;
using System.Windows.Forms;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Partners
{
    public partial class usAllPartner : System.Web.UI.UserControl
    {

        private int iPageSize = 20;
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "PnuPartners";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadLocalPartners();
                    LoadInternationalPartners();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument);
                LoadLocalPartners();
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "activeTab(1)", true);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                ViewState["PageNumber2"] = Convert.ToInt32(e.CommandArgument);
                LoadInternationalPartners();
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "activeTab(2)", true);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        private void LoadLocalPartners()
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                List<clsPartner> Localpartners = new List<clsPartner>();


                clsPartner partner = null;
                qryParam.Add("<Eq><FieldRef Name='IsHome'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PartnerTypes") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Local") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, 0, "ItemOrder", "True");
                if (CollITems != null && CollITems.Count > 0)
                {
                    foreach (SPListItem item in CollITems)
                    {

                        partner = new clsPartner()
                        {
                            Partner = Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Partner")]),
                            Logo = Convert.ToString(item["LogoUrl"]).Split(',')[0]

                        };

                        Localpartners.Add(partner);


                    }
                }


                PagedDataSource pdsData = new PagedDataSource();

                pdsData.DataSource = Localpartners;
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
                rptLocal.DataSource = pdsData;
                rptLocal.DataBind();


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            


        }
        private void  LoadInternationalPartners()
        {
            try
            {
                ArrayList qryParam = new ArrayList();

                List<clsPartner> InternationPartners = new List<clsPartner>();

                clsPartner partner = null;
                qryParam.Add("<Eq><FieldRef Name='IsHome'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PartnerTypes") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_International") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, 0, "ItemOrder", "True");
                if (CollITems != null && CollITems.Count > 0)
                {
                    foreach (SPListItem item in CollITems)
                    {

                        partner = new clsPartner()
                        {
                            Partner = Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Partner")]),
                            Logo = Convert.ToString(item["LogoUrl"]).Split(',')[0]

                        };

                        InternationPartners.Add(partner);


                    }
                }

                PagedDataSource pdsData = new PagedDataSource();

                pdsData.DataSource = InternationPartners;
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
                rptInternational.DataSource = pdsData;
                rptInternational.DataBind();


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
       

    }
}
