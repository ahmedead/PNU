
using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices
{
    public partial class PnuEServiceHome : UserControl
    {
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "PnuEservices";
       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadHomeServices();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        void LoadHomeServices()
        {
            try
            {
                ArrayList qryParam = new ArrayList();


                List<HomeService> services = new List<HomeService>();
                var RowLimt = Convert.ToInt32(Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_PnuServiceHomeCount"));

                qryParam.Add("<Eq><FieldRef Name='IsHome'/><Value Type='Boolean'>1</Value></Eq>");
                qryParam.Add("<Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>");


                var CollITems = Helper.LoadListDynamicByCML(SPContext.Current.Site.ID, WebUrl, ListName, qryParam, RowLimt, "ItemOrder", "True");
                if (CollITems != null && CollITems.Count > 0)
                {
                    foreach (SPListItem item in CollITems)
                    {

                        SPAttachmentCollection attachments = item.Attachments;

                        HomeService service = new HomeService();

                        service.Id = Convert.ToInt32(item["ID"]);
                        service.Title = Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Title")]);
                        service.Desc = Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Desc")]);
                        SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Beneficiaries")].ToString());
                        service.Beneficiaries = new List<Beneficiary>();
                        for (int i = 0; i < itemValue.Count; i++)
                        {
                            var benef = new Beneficiary()
                            {
                                BeneficiaryTitle = itemValue[i].ToString()
                            };


                            service.Beneficiaries.Add(benef);
                        }

                        service.ImageUrl = attachments.UrlPrefix + attachments[0];

                        services.Add(service);

                    }
                }


                rpt.DataSource = services;
                rpt.DataBind();

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }


        }

        
    }
}
