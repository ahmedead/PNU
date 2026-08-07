using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs
{
    public partial class ucProgramMainData : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    BindData();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        private void SetBrowserTitle(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title)) return;
                // make the dynamic title available to ucHomeHeader
                HttpContext.Current.Items["PNU_BrowserTitle"] = title;
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(title)));
                }
                else
                {
                    // fallback if the master exposes a normal <head runat="server">
                    Page.Title = title;
                }
            }
            catch (Exception ex)
            {
                //Publics.WriteToLog("ucMediaDetails.SetBrowserTitle", ex);
            }
        }

        // Handles nested master pages (e.g. DGA_Internal.master under a root master)
        private ContentPlaceHolder FindPlaceHolder(System.Web.UI.MasterPage master, string id)
        {
            while (master != null)
            {
                var ph = master.FindControl(id) as ContentPlaceHolder;
                if (ph != null) return ph;
                master = master.Master;
            }
            return null;
        }
        private void BindData()
        {
            try
            {
                if (Request.QueryString["ProgramCode"] == null)
                    return;
                string ProgramCode = Request.QueryString["ProgramCode"].ToString();

                List<ProgramsPageMain> _AllData = new List<ProgramsPageMain>();


                //SPListItemCollection objNew = null;
                List<NewStudyPlanDto> data = new List<NewStudyPlanDto>();
                AllDepartmentPrograms tmp = new AllDepartmentPrograms();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList Proglist = web.Lists["AllDepartmentPrograms"];
                            if (Proglist != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = $@"<Where>
                                          
                                             <Eq>
                                                <FieldRef Name='Code' />
                                                <Value Type='Text'>{ProgramCode}</Value>
                                             </Eq>
                                       </Where>";

                                SPListItemCollection objcollitem = Proglist.GetItems(query);

                                if (objcollitem != null && objcollitem.Count > 0)
                                {
                                    tmp = SPFactory.MapListItemsToClass<AllDepartmentPrograms>(objcollitem[0]);
                                }
                            }
                        }
                    }
                });


                //data = busclsNewStudyPlan.GetNewStudyPlanByProgramCode(ProgramCode);

                string ListName = SPFactory.GetLocalizedTitle("NewStudyPlan", "NewStudyPlan_EN");
                SPListItemCollection objNew1 = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                              <Eq>
                                 <FieldRef Name='PROGRAM' />
                                 <Value Type='Text'>" + ProgramCode + @"</Value>
                              </Eq>
                           </Where>");
                            SPList reqList = web.Lists[ListName];
                            objNew1 = reqList.GetItems(query);
                            if (objNew1 != null && objNew1.Count > 0)
                            {
                                data = SPFactory.MapListItemsToClass<NewStudyPlanDto>(objNew1);
                            }
                        }
                    }
                });



                AllPrograms _MainData = busclsPrograms.GetNAllProgramMainDataByProgramCode(ProgramCode);
                if (_MainData != null)
                {
                    ProgramsPageMain _CurrentItem = new ProgramsPageMain();
                    _CurrentItem.ProgramCode = _MainData.Prog_Code;
                    _CurrentItem.CollegeCode = _MainData.Coll_Code;
                    _CurrentItem.CollegeName = _MainData.Coll_Title;
                    _CurrentItem.DepartmentCode = _MainData.Dept_Code;
                    _CurrentItem.DepartmentName = _MainData.Dept_Title;

                    _CurrentItem.Major = _MainData.Major;
                    _CurrentItem.Major_EN = _MainData.Major_EN;




                    _CurrentItem.ProgramName_EN = tmp.Title_EN;
                    _CurrentItem.CollegeName_EN = tmp.COLL_DESC_EN;
                    _CurrentItem.DepartmentName_EN = tmp.DEPT_DESC_EN;

                    _CurrentItem.ProgramDesc = _MainData.Description;
                    _CurrentItem.ProgramDesc_EN = _MainData.Description_EN;
                    _CurrentItem.ProgramName = _MainData.Prog_Title;
                    _CurrentItem.ProgramNature = _MainData.ProgramNature;
                    _CurrentItem.ProgramFields = _MainData.ProgramFields;

                    _CurrentItem.ProgramNature_EN = _MainData.ProgramNature_EN;
                    _CurrentItem.ProgramFields_EN = _MainData.ProgramFields_EN;

                    _CurrentItem.SecondaryType = _MainData.SecondaryType;
                    _CurrentItem.ProgramDegree_EN = _MainData.ProgramDegree_EN;


                    //string localizedTitle = SPFactory.GetLocalizedTitle(_CurrentItem.CollegeName + " - " + _CurrentItem.DepartmentName + " - " + _CurrentItem.ProgramName, _CurrentItem.CollegeName_EN + " - " + _CurrentItem.DepartmentName_EN + " - " + _CurrentItem.ProgramName_EN);
                    string localizedTitle = SPFactory.GetLocalizedTitle( _CurrentItem.ProgramName,  _CurrentItem.ProgramName_EN);
                    SetBrowserTitle(localizedTitle);
                    //var head = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                    //if (head != null)
                    //{
                    //    string desc = HttpUtility.HtmlEncode(
                    //        Publics.TruncateText(SPFactory.GetLocalizedTitle(_CurrentItem.ProgramDesc, _CurrentItem.ProgramDesc_EN), 160));
                    //    head.Controls.Add(new LiteralControl(
                    //        $"<meta name=\"description\" content=\"{desc}\" />"));
                    //}


                    if (data != null && data.Count > 0)
                    {
                        List<NewStudyPlanDto> _ProgData = new List<NewStudyPlanDto>();
                        _ProgData = data.GroupBy(d => new { d.COLLEGE_CODE, d.COLLEGE_DSC, d.DEPARTMENT_CODE, d.DEPARTMENT_DSC, d.PROGRAM }).Select(group => group.First()).ToList();

                        if (_ProgData != null && _ProgData.Count > 0)
                        {
                            _CurrentItem.ProgramYears = _ProgData[0].YRS;
                            _CurrentItem.ProgramLanguage = _ProgData[0].LANGDESC;
                            _CurrentItem.ProgramDegree = _ProgData[0].PROG_DESC_L;

                            ucStudyPlan ucStudyPlan1 = FindControl("ucStudyPlan") as ucStudyPlan;

                            if (ucStudyPlan1 != null)
                            {
                                if (_ProgData[0].SOBCURR_DEGC_CODE.Trim() == "MAD")
                                    ucStudyPlan1.BindData1(data, _CurrentItem.CollegeCode);
                                else
                                    ucStudyPlan1.BindData(data, _CurrentItem.CollegeCode);
                            }

                        }

                    }


                    _AllData.Add(_CurrentItem);

                    rptMainData.DataSource = _AllData;
                    rptMainData.DataBind();
                    return;
                }






            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }



        }
    }
}
