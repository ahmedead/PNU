using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs
{
    public partial class ucPLANS_ELEC_C : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //    BindDataU();

        }
        public void BindDataU(string COLL_CODE)
        {
            try
            {
                List<PLANS_ELEC_C> _AllData = new List<PLANS_ELEC_C>();
                string PLANS_ELEC_C_ListName = SPFactory.GetLocalizedTitle("PLANS_ELEC_C", "PLANS_ELEC_C_EN");
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(PLANS_ELEC_C_ListName);

                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         @"<Where>
                              <Eq>
                                 <FieldRef Name='SMRPRLE_COLL_CODE' />
                                 <Value Type='Text'>" + COLL_CODE + @"</Value>
                              </Eq>
                           </Where>");

                        SPListItemCollection items = list.GetItems(query);

                        if (items == null || items.Count == 0)
                        {

                            List<AllPLANS_ELECMain> _MainEmptyData = new List<AllPLANS_ELECMain>();
                            AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                            obj.LevelCode = "5000";
                            obj.LevelDesc = SPFactory.GetPNUresResource("NoCollegeRequirements");
                            obj.StudyPlanP = new List<PLANS_ELEC_P>();
                            obj.StudyPlanC = new List<PLANS_ELEC_C>();
                            obj.StudyPlan = new List<PLANS_ELEC_U>();

                            _MainEmptyData.Add(obj);
                            masterRepeaterU.DataSource = _MainEmptyData;
                            masterRepeaterU.DataBind();
                            return;
                        }


                        _AllData = SPFactory.MapListItemsToClass<PLANS_ELEC_C>(items);



                    }
                }






                //if (Request.QueryString["ProgramCode"] == null)
                //    return;
                //string ProgramCode = Request.QueryString["ProgramCode"].ToString();

                //List<NewStudyPlanDto> _AllData = busclsNewStudyPlan.GetNewStudyPlanByProgramCode(ProgramCode);
                if (_AllData == null || _AllData.Count == 0)
                {
                    masterRepeaterU.DataSource = null;
                    masterRepeaterU.DataBind();
                    return;
                }
                List<PLANS_ELEC_C> _allLevels = new List<PLANS_ELEC_C>();
                _allLevels = _AllData.GroupBy(d => new { d.STVATTR_DESC }).Select(group => group.First()).ToList();
                List<AllPLANS_ELECMain> _MainData = new List<AllPLANS_ELECMain>();
                if (_allLevels != null && _allLevels.Count > 0)
                {
                    int i = 1020;
                    foreach (PLANS_ELEC_C objLevel in _allLevels)
                    {
                        AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                        obj.LevelCode = i.ToString();
                        i = i + 1;
                        obj.LevelDesc = objLevel.STVATTR_DESC.ToString();

                        List<PLANS_ELEC_C> _aalDataByLevel = new List<PLANS_ELEC_C>();
                        _allLevels = _AllData.Where(d => d.STVATTR_DESC == objLevel.STVATTR_DESC).ToList();
                        if (_allLevels != null && _allLevels.Count > 0)
                        {
                            _allLevels = _allLevels.Where(g => g.SCRATTR_SUBJ_CODE != null && g.SCRATTR_CRSE_NUMB != null).ToList();
                            _allLevels = _allLevels.GroupBy(d => new { d.SCRATTR_SUBJ_CODE, d.SCRATTR_CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                            if (_allLevels != null && _allLevels.Count > 0)
                            {
                                obj.StudyPlanC = _allLevels;
                            }

                        }


                        _MainData.Add(obj);
                    }

                    masterRepeaterU.DataSource = _MainData;
                    masterRepeaterU.DataBind();

                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }


        }

    }
}
