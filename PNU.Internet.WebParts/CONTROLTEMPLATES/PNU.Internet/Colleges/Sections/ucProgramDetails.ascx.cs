using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections
{

    public class clsProgram
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string ImageUrl { get; set; }
        public string Overview { get; set; }
        public string Outputs { get; set; }
        
    }
    public partial class ucProgramDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["ItemId"] == null)
                    return;
                if (!IsPostBack)
                {

                    GetItemDetailsById();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }


        private void GetItemDetailsById()
        {
            try
            {
                if (Page.Request.QueryString["ItemId"] == null)
                    return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["Programs"];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                SPQuery query2 = new SPQuery();


                                SPListItem item = list.GetItemById(Convert.ToInt32(Page.Request.QueryString["ItemId"].ToString()));
                                if (item != null)
                                {
                                    clsProgram _Program = new clsProgram();
                                    _Program.Title = item["Title"] != null ? item["Title"].ToString() : "";
                                    _Program.ImageUrl = item["ImageUrl"] != null ? item["ImageUrl"].ToString() : "";
                                    _Program.Overview = item["Overview"] != null ? item["Overview"].ToString() : "";
                                    _Program.Outputs = item["Outputs"] != null ? item["Outputs"].ToString() : "";

                                    List<clsProgram> _AllItems = new List<clsProgram>();
                                    _AllItems.Add(_Program);
                                    Repeater1.DataSource = _AllItems;
                                    Repeater1.DataBind();

                                    Repeater2.DataSource = _AllItems;
                                    Repeater2.DataBind();


                                    rptPrograms.DataSource = _AllItems;
                                    rptPrograms.DataBind();
                                }
                            }


                            SPList ProgramGoals = web.Lists["ProgramGoals"];
                            if (ProgramGoals != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = @"<Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='Program_x003a__x0627__x0644__x06' />
                                            <Value Type='Lookup'>" + Page.Request.QueryString["ItemId"].ToString() + @"</Value>
                                         </Eq>
                                         <Eq>
                                            <FieldRef Name='Visibility' />
                                            <Value Type='Boolean'>1</Value>
                                         </Eq>
                                      </And>
                                   </Where>
                                   <OrderBy>
                                      <FieldRef Name='ItemOrder' Ascending='True' />
                                   </OrderBy>";
                                SPListItemCollection items = ProgramGoals.GetItems(query);
                                if (items != null)
                                {
                                    rptProgramGoals.DataSource = items.GetDataTable();
                                    rptProgramGoals.DataBind();
                                }
                            }
                        }
                    }
                });





            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            

        }
    }
}
