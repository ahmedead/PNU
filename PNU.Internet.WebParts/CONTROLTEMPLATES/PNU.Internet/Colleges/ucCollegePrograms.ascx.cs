using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucCollegePrograms : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadCollegePrograms();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
            


        }

        private void LoadCollegePrograms()
        {
            try
            {
                string CollegeCode = "";
                if (Page.Request.QueryString["Source"] != null)
                {
                    CollegeCode = Page.Request.QueryString["Source"].ToString();
                }
                else
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["AboutCollege"];
                            if (list != null)
                            {

                                SPListItemCollection collitem = list.GetItems();
                                if (collitem != null)
                                {
                                    if (collitem.Count > 0)
                                        if (collitem[0]["College_Code"] != null)
                                            CollegeCode = collitem[0]["College_Code"].ToString().Trim();


                                }


                            }
                        }
                    }


                }
                if (CollegeCode == "")
                    return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {

                            //string ListName = PortalHelper.IsArabic ? "NewStudyPlan" : "NewStudyPlan_EN";
                            string ListName = "NewStudyPlan";
                            SPList list = web.Lists[ListName];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                //query.Query = $@"<Where>
                                //          <And>
                                //             <Eq>
                                //                <FieldRef Name='DEPARTMENT_CODE' />
                                //                <Value Type='Text'>0000</Value>
                                //             </Eq>
                                //             <Eq>
                                //                <FieldRef Name='COLLEGE_CODE' />
                                //                <Value Type='Text'>{CollegeCode}</Value>
                                //             </Eq>
                                //          </And>
                                //       </Where>";

                                query.Query = $@"<Where>
                                             <Eq>
                                                <FieldRef Name='COLLEGE_CODE' />
                                                <Value Type='Text'>{CollegeCode}</Value>
                                             </Eq>
                                       </Where>";

                                SPListItemCollection collitem = list.GetItems(query);

                                if (collitem != null && collitem.Count > 0)
                                {

                                    List<NewStudyPlanDto> _AllData = new List<NewStudyPlanDto>();
                                    _AllData = SPFactory.MapListItemsToClass<NewStudyPlanDto>(collitem);
                                    int i = 1;
                                    List<NewStudyPlanDto> _GroupedData = _AllData.GroupBy(d => new { d.SOBCURR_DEGC_CODE }).Select(group => group.First()).ToList();
                                    List<AllProgramsRepeater> _RepeaterData = new List<AllProgramsRepeater>();
                                    foreach (NewStudyPlanDto obj in _GroupedData)
                                    {
                                        AllProgramsRepeater _item = new AllProgramsRepeater();
                                        _item.ID = i.ToString();
                                        _item.Title = obj.DEGC_DSC;
                                        //_item.Title_EN = obj.DEGC_DSC;

                                        List<NewStudyPlanDto> _FilteredData = _AllData.Where(e => e.SOBCURR_DEGC_CODE == obj.SOBCURR_DEGC_CODE).ToList();
                                        List<NewStudyPlanDto> _GroupedFilteredData = _FilteredData.GroupBy(d => new { d.SOBCURR_DEGC_CODE, d.PROGRAM }).Select(group => group.First()).ToList();


                                        _item.Programs = new List<AllDepartmentPrograms>();
                                        List<AllDepartmentPrograms> AllPrograms = new List<AllDepartmentPrograms>();
                                        foreach (NewStudyPlanDto _Study in _GroupedFilteredData)
                                        {
                                            SPList Proglist = web.Lists["AllDepartmentPrograms"];
                                            if (list != null)
                                            {
                                                query = new SPQuery();
                                       //         query.Query = $@"<Where>
                                       //   <And>
                                       //      <Eq>
                                       //         <FieldRef Name='DEPT_CODE' />
                                       //         <Value Type='Text'>0000</Value>
                                       //      </Eq>
                                       //      <Eq>
                                       //         <FieldRef Name='Code' />
                                       //         <Value Type='Text'>{_Study.PROGRAM}</Value>
                                       //      </Eq>
                                       //   </And>
                                       //</Where>";


                                                query.Query = $@"<Where>
                                             <Eq>
                                                <FieldRef Name='Code' />
                                                <Value Type='Text'>{_Study.PROGRAM}</Value>
                                             </Eq>
                                       </Where>";

                                                SPListItemCollection objcollitem = Proglist.GetItems(query);

                                                if (objcollitem != null && objcollitem.Count > 0)
                                                {
                                                    AllDepartmentPrograms tmp = SPFactory.MapListItemsToClass<AllDepartmentPrograms>(objcollitem[0]);
                                                    AllPrograms.Add(tmp);
                                                }
                                            }
                                        }
                                        _item.Programs = AllPrograms;
                                        i = i + 1;
                                        _RepeaterData.Add(_item);

                                    }


                                    masterRepeater.DataSource = _RepeaterData;
                                    masterRepeater.DataBind();
                                    detailsRepeater.DataSource = _RepeaterData;
                                    detailsRepeater.DataBind();

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('new1');", true);

                                }
                                else
                                {
                                    pnlData.Visible = false;
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
