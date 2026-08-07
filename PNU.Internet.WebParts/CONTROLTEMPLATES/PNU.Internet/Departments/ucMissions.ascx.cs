using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Departments
{
    public partial class ucMissions : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string TitleRes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                pnlData.Visible = false;
                LoadData();

                if (TitleRes == null || TitleRes.Length == 0)
                    TitleRes = SPFactory.GetPNUresResource("DepartmentMission");
                else
                    TitleRes = SPFactory.GetPNUresResource(TitleRes);

                literalDepartmentName.Text= TitleRes;

            }
            
        }
        

        private void LoadData()
        {
            try
            {
                
            List<clsMissions> _StrategicPlan = new List<clsMissions>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {



                        SPList listStrategicPlan = web.Lists["Missions"];
                        if (listStrategicPlan != null)
                        {
                            

                            SPListItemCollection collitem = listStrategicPlan.GetItems();

                            if (collitem != null && collitem.Count > 0)
                            {
                                _StrategicPlan = SPFactory.MapListItemsToClass<clsMissions>(collitem);
                                pnlData.Visible = true;
                                //if (_StrategicPlan != null && _StrategicPlan.Count > 0)
                                //{
                                //    for (int i = 0; i < collitem.Count; i++)
                                //    {
                                //        string Desc = _StrategicPlan[i].Desc;
                                //        if (Desc != null && Desc != "")
                                //        {
                                //            string[] s = Desc.Split('\n');
                                //            string AllDesc = "";
                                //            foreach (string s2 in s)
                                //            {
                                //                AllDesc += $@"<div class=""d-flex align-items-start mb-1""> 
                                //                         <svg xmlns=""http://www.w3.org/2000/svg"" class=""bi me-1 mt-1 align-self-center"" width=""14"" height=""14""><use xmlns:xlink=""http://www.w3.org/1999/xlink"" xlink:href=""#circle-dots""/> </svg> 
                                //                         <p class=""fs-5 mb-0 px-3 text-muted text-start"">{s2} </p> 
                                //                      </div> ";
                                //            }

                                //            _StrategicPlan[i].Desc = AllDesc;
                                //            //_StrategicPlan[i].Desc_EN = AllDesc;
                                //        }
                                //    }
                                //}
                            }
                            else
                                pnlData.Visible = false;


                        }

                    }
                }
            });

            
            rptStrategicPlan.DataSource = _StrategicPlan;
            rptStrategicPlan.DataBind();


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
    }


    public class clsMissions
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string DisplayNo { get; set; }

    }

}
