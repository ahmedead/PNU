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
    public partial class ucPLANS_ELEC_U : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    BindDataU();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        public void BindDataU()
        {
            try
            {            //List<NewStudyPlanDto> _AllData = new List<NewStudyPlanDto>();
                List<PLANS_ELEC_U> _AllData = new List<PLANS_ELEC_U>();
                string PLANS_ELEC_U_ListName = SPFactory.GetLocalizedTitle("PLANS_ELEC_U", "PLANS_ELEC_U_EN");
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(PLANS_ELEC_U_ListName);

                        SPListItemCollection items = list.GetItems();

                        if (items == null || items.Count == 0)
                        {

                            List<AllPLANS_ELECMain> _MainEmptyData = new List<AllPLANS_ELECMain>();
                            AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                            obj.LevelCode = "5000";
                            obj.LevelDesc = SPFactory.GetPNUresResource("NoProgramRequirements");
                            obj.StudyPlanP = new List<PLANS_ELEC_P>();
                            obj.StudyPlanC = new List<PLANS_ELEC_C>();
                            obj.StudyPlan = new List<PLANS_ELEC_U>();

                            _MainEmptyData.Add(obj);
                            masterRepeaterU.DataSource = _MainEmptyData;
                            masterRepeaterU.DataBind();
                            return;
                        }


                        _AllData = SPFactory.MapListItemsToClass<PLANS_ELEC_U>(items);



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
                List<PLANS_ELEC_U> _allLevels = new List<PLANS_ELEC_U>();
                _allLevels = _AllData.GroupBy(d => new { d.STVATTR_DESC }).Select(group => group.First()).ToList();
                List<AllPLANS_ELECMain> _MainData = new List<AllPLANS_ELECMain>();
                if (_allLevels != null && _allLevels.Count > 0)
                {
                    int i = 20;
                    foreach (PLANS_ELEC_U objLevel in _allLevels)
                    {
                        AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                        obj.LevelCode = i.ToString();
                        i = i + 1;
                        obj.LevelDesc = objLevel.STVATTR_DESC.ToString();

                        List<PLANS_ELEC_U> _aalDataByLevel = new List<PLANS_ELEC_U>();
                        _allLevels = _AllData.Where(d => d.STVATTR_DESC == objLevel.STVATTR_DESC).ToList();
                        if (_allLevels != null && _allLevels.Count > 0)
                        {
                            //_allLevels = _allLevels.Where(g => g.SUBJ_CODE != null && g.CRSE_NUMB != null).ToList();
                            //_allLevels = _allLevels.GroupBy(d => new { d.SUBJ_CODE, d.CRSE_NUMB, d.COURSE_TITLE, d.CREDIT, d.SMRACAA_SEQNO }).Select(group => group.First()).ToList();
                            if (_allLevels != null && _allLevels.Count > 0)
                            {
                                obj.StudyPlan = _allLevels;
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
