using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucAllColleges : UserControl
    {
        public string ListName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindColleges();
                }

                if (Page.Request.QueryString["Id"] == null)
                    return;

                var Id = Convert.ToInt32(Page.Request.QueryString["Id"]);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('{Id}');", true);

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "activeTab("+Id.ToString() +")", true);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }


        }


        private void BindColleges()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        ListName = "Faculties";
                        SPList list = web.Lists[ListName];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = @"<OrderBy>
                                  <FieldRef Name='ItemOrder' Ascending='True' />
                               </OrderBy>";


                            //string Category = "الكليات الانسانية";

                            BindRepeater(rptColleges1, "الكليات الانسانية");
                            BindRepeater(rptColleges2, "الكليات العلمية");
                            BindRepeater(rptColleges3, "الكليات الصحية");
                            BindRepeater(rptColleges4, "العمادات والمعاهد");
                            BindRepeater(rptColleges5, "الكليات التطبيقية");


                        }
                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
           
        }

        private void BindRepeater(Repeater rptColleges1, string Category)
        {
            try
            {
                SPListItemCollection collitem = LoadData(Category);
                if (collitem != null)
                {
                    rptColleges1.DataSource = collitem.GetDataTable();
                    rptColleges1.DataBind();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        protected SPListItemCollection LoadData(string  Category)
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                qryParam.Add("<Eq><FieldRef Name='Category'  /><Value Type='Choice'>" + Category + "</Value></Eq>");
                return Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, ListName, qryParam, "ItemOrder", "TRUE");



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return null;
            }
            

        }

    }
}
