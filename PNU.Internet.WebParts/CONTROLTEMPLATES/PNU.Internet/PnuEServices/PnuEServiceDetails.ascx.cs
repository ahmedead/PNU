using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices
{
    public partial class PnuEServiceDetails : UserControl
    {
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "PnuEservices";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var qry = Request.Params.Keys[0] + "=" + Request.Params[0];
                    int ServiceId = 0;
                    if (!ValidateQueryString(qry, out ServiceId))
                    {
                        Response.Redirect("AllServices.aspx");
                    }


                    GetServiceById(ServiceId);
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        void GetServiceById(int serviceId)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(WebUrl))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            SPQuery qry = new SPQuery();
                            qry.Query = @"<Where>
                                    <And>
                                     <Eq><FieldRef Name='IsActive'/><Value Type='Boolean'>1</Value></Eq>
                                     <Eq><FieldRef Name='ID'/><Value Type='Number'>" + serviceId + "</Value></Eq></And></Where>";

                            SPListItemCollection coll = list.GetItems(qry);
                            if (coll == null && coll.Count <= 0)
                            {
                                return;
                            }
                            SPListItem item = coll[0];
                            if (item == null) return;
                            lit_ServiceDescription.Text = Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Desc")]);
                            lnkApply.PostBackUrl = Convert.ToString(item["ServiceUrl"]);
                            SPAttachmentCollection attachments = item.Attachments;
                            ServiceImageUrl.ImageUrl = attachments.UrlPrefix + attachments[0];

                            BindBeneficiaries(item);
                            BindConditions(item);
                            BindDocuments(item);
                            BindSchedule(item);



                        }
                    }
                });

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        void BindBeneficiaries(SPListItem item)
        {
            try
            {
                SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Beneficiaries")].ToString());
                List<Beneficiary> BeneficiariesList = new List<Beneficiary>();
                for (int i = 0; i < itemValue.Count; i++)
                {
                    var benef = new Beneficiary()
                    {
                        BeneficiaryTitle = itemValue[i].ToString()
                    };


                    BeneficiariesList.Add(benef);
                }
                RptBenef.DataSource = BeneficiariesList;
                RptBenef.DataBind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        void BindConditions(SPListItem item)
        {
            try
            {
                var json = !String.IsNullOrEmpty(Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Conditions")])) ?
                Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Conditions")]) : "";
                var Conditions = JsonConvert.DeserializeObject<List<Condition>>(json);

                rptConditions.DataSource = Conditions;
                rptConditions.DataBind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        void BindDocuments(SPListItem item)
        {
            try
            {
                var json = !String.IsNullOrEmpty(Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Documents")])) ?
                Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Documents")]) : "";
                var Documents = JsonConvert.DeserializeObject<List<Document>>(json);

                rptDocuments.DataSource = Documents;
                rptDocuments.DataBind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }


        }
        void BindSchedule(SPListItem item)
        {
            try
            {
                var json = !String.IsNullOrEmpty(Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Schedule")])) ?
                    Convert.ToString(item[Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_Schedule")]) : "";
                var Schedule = JsonConvert.DeserializeObject<List<Schedule>>(json);

                rptSchedule.DataSource = Schedule;
                rptSchedule.DataBind();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

        }


        static bool ValidateQueryString(string queryString,out int ServiceId)
        {
            ServiceId = 0;
            try
            {
                var queryParams = HttpUtility.ParseQueryString(queryString);

                if (queryParams["ServiceId"] != null && int.TryParse(queryParams["ServiceId"], out ServiceId))
                {

                    if (ServiceId < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"PnuEServiceDetails - ValidateQueryString", ex.Message);
                return false;
            }
            
            
        }
    }


}
