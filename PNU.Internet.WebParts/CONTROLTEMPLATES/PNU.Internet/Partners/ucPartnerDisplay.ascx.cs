using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Partners
{
    public partial class ucPartnerDisplay : UserControl
    {
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "PnuPartners";

    protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
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
        void LoadLocalPartners()
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                List<clsPartner> Localpartners = new List<clsPartner>();
                List<clsPartner> InternationPartners = new List<clsPartner>();

                clsPartner partner = null;
                qryParam.Add("<Eq><FieldRef Name='IsHome'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PartnerTypes") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Local") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, 10, "ItemOrder", "True");
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

                rptData.DataSource = Localpartners;
                rptData.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }

        void LoadInternationalPartners()
        {
            try
            {
                ArrayList qryParam = new ArrayList();

                List<clsPartner> InternationPartners = new List<clsPartner>();

                clsPartner partner = null;
                qryParam.Add("<Eq><FieldRef Name='IsHome'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Contains><FieldRef Name='" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PartnerTypes") + "'/><Value Type='Choice'>" + Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_International") + "</Value></Contains>");

                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, 10, "ItemOrder", "True");
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

                rptInternational.DataSource = InternationPartners;
                rptInternational.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }

    }
   
}
