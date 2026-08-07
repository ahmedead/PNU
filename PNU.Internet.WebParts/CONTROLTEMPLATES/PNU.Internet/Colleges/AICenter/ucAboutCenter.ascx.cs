using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucAboutCenter : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadData();

            }
        }

        private void LoadData()
        {
            try
            {
                List<clsAbout> _AllItems = new List<clsAbout>();
                List<clsStrategicPlan> _StrategicPlan = new List<clsStrategicPlan>();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {

                            SPList list = web.Lists["About"];
                            if (list != null)
                            {

                                SPListItemCollection collitem = list.GetItems();

                                if (collitem != null && collitem.Count > 0)
                                {
                                    clsAbout obj = SPFactory.MapListItemsToClass<clsAbout>(collitem[0]);

                                    if (obj != null) { _AllItems.Add(obj); }

                                }


                            }

                            


                            SPList listStrategicPlan = web.Lists["StrategicPlan"];
                            if (listStrategicPlan != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = string.Concat(@"<OrderBy>
                                                  <FieldRef Name='ItemOrder' Ascending='True' />
                                               </OrderBy>"
                                );

                                SPListItemCollection collitem = listStrategicPlan.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {
                                    _StrategicPlan = SPFactory.MapListItemsToClass<clsStrategicPlan>(collitem);
                                    if (_StrategicPlan != null && _StrategicPlan.Count > 0)
                                    {
                                        for (int i = 0; i < collitem.Count; i++)
                                        {
                                            string Desc = SPFactory.GetLocalizedTitle(_StrategicPlan[i].Desc, _StrategicPlan[i].Desc_EN);
                                            if (Desc != null && Desc != "")
                                            {
                                                string[] s = Desc.Split('\n');
                                                string AllDesc = "";
                                                foreach (string s2 in s)
                                                {
                                                    AllDesc += $@"<div class=""d-flex align-items-start mb-1""> 
                                                         <svg xmlns=""http://www.w3.org/2000/svg"" class=""bi me-1 mt-1 align-self-center"" width=""14"" height=""14""><use xmlns:xlink=""http://www.w3.org/1999/xlink"" xlink:href=""#circle-dots""/> </svg> 
                                                         <p class=""fs-5 mb-0 px-3 text-muted text-start"">{s2} </p> 
                                                      </div> ";
                                                }

                                                _StrategicPlan[i].Desc = AllDesc;
                                                //_StrategicPlan[i].Desc_EN = AllDesc;
                                            }
                                        }
                                    }
                                }


                            }

                        }
                    }
                });

                rptMainData.DataSource = _AllItems;
                rptMainData.DataBind();
                Repeater1.DataSource = _AllItems;
                Repeater1.DataBind();



                rptStrategicPlan.DataSource = _StrategicPlan;
                rptStrategicPlan.DataBind();


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

    }
}
