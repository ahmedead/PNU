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
    public partial class ucStudyPlan : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //    BindData();
        }
        
        public void BindData(List<NewStudyPlanDto> _AllData, string COLL_CODE)
        {
            try
            { 
                pnlData.Visible = false;
                pnlAll.Visible = true;

                ucPLANS_ELEC_C ucPLANS_ELEC_C1 = FindControl("ucPLANS_ELEC_C") as ucPLANS_ELEC_C;
                if (ucPLANS_ELEC_C1 == null && pnlAll != null)
                {
                    ucPLANS_ELEC_C1 = pnlAll.FindControl("ucPLANS_ELEC_C") as ucPLANS_ELEC_C;
                }
                if (ucPLANS_ELEC_C1 != null)
                {
                    ucPLANS_ELEC_C1.BindDataU(COLL_CODE);
                }

                if (_AllData == null || _AllData.Count == 0)
                {
                    masterRepeater.DataSource = null;
                    masterRepeater.DataBind();
                    return;
                }
                _AllData = _AllData.OrderBy(p => Convert.ToInt32(p.CLSS)).ToList();
                List<NewStudyPlanDto> _allLevels = new List<NewStudyPlanDto>();
                List<AllProgramsMain> _MainData = new List<AllProgramsMain>();

                List<NewStudyPlanDto> _Data = new List<NewStudyPlanDto>();
                _Data = _AllData.Where(p => (p.CLSS == "1" || p.CLSS == "2") && (p.AREA == "جط-ستص01" || p.AREA == "جط-ستص02")).ToList();
                if (_Data != null && _Data.Count > 0)
                {
                    List<NewStudyPlanDto> _allLevels1 = new List<NewStudyPlanDto>();

                    _allLevels1 = _Data.Where(d => d.CLSS == "1" && d.AREA == "جط-ستص01").ToList();
                    if (_allLevels1 != null && _allLevels1.Count > 0)
                    {
                        AllProgramsMain obj1 = new AllProgramsMain();
                        obj1.LevelCode = "-1";
                        obj1.COLL_CODE = _Data[0].COLLEGE_CODE;

                        List<NewStudyPlanDto> _all = _allLevels1;
                        List<NewStudyPlanDto> _allNonData = _allLevels1.Where(g => g.STVATTR_DESC != null).ToList();
                        _allLevels1 = _allLevels1.Where(g => g.SUBJ_CODE != null && g.CRSE_NUMB != null).ToList();
                        List<NewStudyPlanDto> _allDataWithoutDistinct = _allLevels1;
                        _allDataWithoutDistinct = _allDataWithoutDistinct.OrderBy(p => Convert.ToDecimal(p.SCRRTST_SEQNO)).ToList();
                        _allLevels1 = _allLevels1.GroupBy(d => new { d.SUBJ_CODE, d.CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                        if (_allLevels1 != null && _allLevels1.Count > 0)
                        {
                            _allLevels1 = GetPreRequieites(_allLevels1, _allDataWithoutDistinct);
                            obj1.StudyPlan = _allLevels1;
                            if (_allNonData != null && _allNonData.Count > 0)
                                obj1.StudyPlan.AddRange(_allNonData);

                            _MainData.Add(obj1);
                        }
                    }

                    _allLevels1 = new List<NewStudyPlanDto>();
                    _allLevels1 = _Data.Where(d => d.CLSS == "2" && d.AREA == "جط-ستص02").ToList();
                    if (_allLevels1 != null && _allLevels1.Count > 0)
                    {
                        AllProgramsMain obj1 = new AllProgramsMain();
                        obj1.LevelCode = "-2";
                        obj1.COLL_CODE = _Data[0].COLLEGE_CODE;

                        List<NewStudyPlanDto> _all = _allLevels1;
                        List<NewStudyPlanDto> _allNonData = _allLevels1.Where(g => g.STVATTR_DESC != null).ToList();
                        _allLevels1 = _allLevels1.Where(g => g.SUBJ_CODE != null && g.CRSE_NUMB != null).ToList();
                        List<NewStudyPlanDto> _allDataWithoutDistinct = _allLevels1;
                        _allDataWithoutDistinct = _allDataWithoutDistinct.OrderBy(p => Convert.ToDecimal(p.SCRRTST_SEQNO)).ToList();
                        _allLevels1 = _allLevels1.GroupBy(d => new { d.SUBJ_CODE, d.CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                        if (_allLevels1 != null && _allLevels1.Count > 0)
                        {
                            _allLevels1 = GetPreRequieites(_allLevels1, _allDataWithoutDistinct);
                            obj1.StudyPlan = _allLevels1;
                            if (_allNonData != null && _allNonData.Count > 0)
                                obj1.StudyPlan.AddRange(_allNonData);

                            _MainData.Add(obj1);
                        }
                    }
                }

                _AllData = _AllData.Where(p => (p.AREA != "جط-ستص01" && p.AREA != "جط-ستص02")).ToList();

                _allLevels = _AllData.GroupBy(d => new { d.CLSS }).Select(group => group.First()).ToList();

                if (_allLevels != null && _allLevels.Count > 0)
                {
                    foreach (NewStudyPlanDto objLevel in _allLevels)
                    {
                        AllProgramsMain obj = new AllProgramsMain();
                        obj.LevelCode = objLevel.CLSS;
                        obj.COLL_CODE = objLevel.COLLEGE_CODE;

                        List<NewStudyPlanDto> _aalDataByLevel = new List<NewStudyPlanDto>();

                        var levelsData = _AllData.Where(d => d.CLSS == objLevel.CLSS).ToList();
                        if (levelsData != null && levelsData.Count > 0)
                        {
                            List<NewStudyPlanDto> _all = levelsData;
                            List<NewStudyPlanDto> _allNonData = levelsData.Where(g => g.STVATTR_DESC != null).ToList();
                            var courses = levelsData.Where(g => g.SUBJ_CODE != null && g.CRSE_NUMB != null).ToList();
                            List<NewStudyPlanDto> _allDataWithoutDistinct = courses;
                            _allDataWithoutDistinct = _allDataWithoutDistinct.OrderBy(p => Convert.ToDecimal(p.SCRRTST_SEQNO)).ToList();

                            courses = courses.GroupBy(d => new { d.SUBJ_CODE, d.CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                            if (courses != null && courses.Count > 0)
                            {
                                courses = GetPreRequieites(courses, _allDataWithoutDistinct);
                                obj.StudyPlan = courses;
                                if (_allNonData != null && _allNonData.Count > 0)
                                    obj.StudyPlan.AddRange(_allNonData);
                            }
                        }

                        _MainData.Add(obj);
                    }

                    masterRepeater.DataSource = _MainData;
                    masterRepeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        public void BindData1(List<NewStudyPlanDto> _AllData, string COLL_CODE)
        {
            try
            {
                pnlAll.Visible = false;
                pnlData.Visible = true;

                if (_AllData == null || _AllData.Count == 0)
                {
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                    return;
                }
                _AllData = _AllData.OrderBy(p => Convert.ToInt32(p.CLSS)).ToList();
                List<NewStudyPlanDto> _allLevels = new List<NewStudyPlanDto>();
                _allLevels = _AllData.GroupBy(d => new { d.CLSS }).Select(group => group.First()).ToList();
                List<AllProgramsMain> _MainData = new List<AllProgramsMain>();
                if (_allLevels != null && _allLevels.Count > 0)
                {
                    foreach (NewStudyPlanDto objLevel in _allLevels)
                    {
                        AllProgramsMain obj = new AllProgramsMain();
                        obj.LevelCode = objLevel.CLSS;
                        obj.COLL_CODE = objLevel.COLLEGE_CODE;

                        List<NewStudyPlanDto> _aalDataByLevel = new List<NewStudyPlanDto>();
                        var levelsData = _AllData.Where(d => d.CLSS == objLevel.CLSS).ToList();
                        if (levelsData != null && levelsData.Count > 0)
                        {
                            List<NewStudyPlanDto> _all = levelsData;
                            List<NewStudyPlanDto> _allNonData = levelsData.Where(g => g.STVATTR_DESC != null).ToList();
                            var courses = levelsData.Where(g => g.SUBJ_CODE != null && g.CRSE_NUMB != null).ToList();
                            List<NewStudyPlanDto> _allDataWithoutDistinct = courses;
                            _allDataWithoutDistinct = _allDataWithoutDistinct.OrderBy(p => Convert.ToDecimal(p.SCRRTST_SEQNO)).ToList();

                            courses = courses.GroupBy(d => new { d.SUBJ_CODE, d.CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                            if (courses != null && courses.Count > 0)
                            {
                                courses = GetPreRequieites(courses, _allDataWithoutDistinct);
                                obj.StudyPlan = courses;
                                if (_allNonData != null && _allNonData.Count > 0)
                                    obj.StudyPlan.AddRange(_allNonData);
                            }
                        }

                        _MainData.Add(obj);
                    }

                    Repeater1.DataSource = _MainData;
                    Repeater1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private List<NewStudyPlanDto> GetPreRequieites(List<NewStudyPlanDto> allLevels, List<NewStudyPlanDto> allDataWithoutDistinct)
        {
            try
            {
                for (int i = 0; i < allLevels.Count; i++)
                {
                    List<NewStudyPlanDto> allData = allDataWithoutDistinct.Where(e => e.SUBJ_CODE == allLevels[i].SUBJ_CODE && e.CRSE_NUMB == allLevels[i].CRSE_NUMB).ToList();
                    if (allData != null && allData.Count > 0)
                    {
                        allData = allData.OrderBy(p => Convert.ToDecimal(p.SCRRTST_SEQNO)).ToList();
                        if (allData != null && allData.Count > 0)
                        {
                            string PreRequ = "";
                            for (int j = 0; j < allData.Count; j++)
                            {
                                if (allData[j].S_COREQ1 != null && allData[j].S_COREQ1 != "")
                                {
                                    PreRequ += allData[j].S_COREQ1;
                                }
                            }
                            allLevels[i].S_COREQ1 = PreRequ;
                        }
                    }
                }

                return allLevels;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
            return null;    
        }
    }
}
