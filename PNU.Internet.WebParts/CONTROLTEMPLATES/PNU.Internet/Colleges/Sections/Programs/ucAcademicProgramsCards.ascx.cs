using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs
{
    public partial class ucAcademicProgramsCards : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucAcademicProgramsCards - Page_Load", ex.Message);
            }
        }

        public string GetSecondaryTypeBadges(object secondaryTypeObj)
        {
            return SPFactory.GetSecondaryTypeBadges(secondaryTypeObj);
        }

        private void BindData()
        {
            try
            {
                List<ProgramsPageMain> allProgramsList = new List<ProgramsPageMain>();
                List<NewStudyPlanDto> studyPlanData = new List<NewStudyPlanDto>();
                List<AllDepartmentPrograms> allDeptProgList = new List<AllDepartmentPrograms>();
                List<AllPrograms> allProgramsFromList = new List<AllPrograms>();

                try
                {
                    SPQuery q = new SPQuery();
                    allProgramsFromList = SPFactory.GetAllItemsByQuery<AllPrograms>("Admin", Settings.AllPrograms, q);
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucAcademicProgramsCards - GetAllItemsByQuery AllPrograms", ex.Message);
                }

                try
                {
                    SPQuery qDept = new SPQuery();
                    allDeptProgList = SPFactory.GetAllItemsByQuery<AllDepartmentPrograms>("Admin", Settings.AllDepartmentPrograms, qDept);
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucAcademicProgramsCards - GetAllItemsByQuery AllDepartmentPrograms", ex.Message);
                }

                try
                {
                    string studyPlanListName = SPFactory.GetLocalizedTitle("NewStudyPlan", "NewStudyPlan_EN");
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList list = null;
                                try
                                {
                                    list = web.Lists[studyPlanListName];
                                }
                                catch
                                {
                                    list = web.Lists.TryGetList(studyPlanListName);
                                }

                                if (list != null)
                                {
                                    SPListItemCollection spItems = list.GetItems(new SPQuery());
                                    if (spItems != null && spItems.Count > 0)
                                    {
                                        studyPlanData = SPFactory.MapListItemsToClass<NewStudyPlanDto>(spItems);
                                    }
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucAcademicProgramsCards - Get NewStudyPlan", ex.Message);
                }

                Dictionary<string, NewStudyPlanDto> studyPlanDict = new Dictionary<string, NewStudyPlanDto>(StringComparer.OrdinalIgnoreCase);
                if (studyPlanData != null && studyPlanData.Count > 0)
                {
                    studyPlanDict = studyPlanData
                        .Where(s => !string.IsNullOrEmpty(s.PROGRAM))
                        .GroupBy(s => s.PROGRAM.Trim())
                        .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
                }

                Dictionary<string, AllDepartmentPrograms> deptProgDict = new Dictionary<string, AllDepartmentPrograms>(StringComparer.OrdinalIgnoreCase);
                if (allDeptProgList != null && allDeptProgList.Count > 0)
                {
                    deptProgDict = allDeptProgList
                        .Where(d => !string.IsNullOrEmpty(d.Code))
                        .GroupBy(d => d.Code.Trim())
                        .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
                }

                if (allProgramsFromList != null && allProgramsFromList.Count > 0)
                {
                    foreach (var prog in allProgramsFromList)
                    {
                        string code = !string.IsNullOrEmpty(prog.Prog_Code) ? prog.Prog_Code.Trim() : (!string.IsNullOrEmpty(prog.Code) ? prog.Code.Trim() : string.Empty);
                        if (string.IsNullOrEmpty(code)) continue;

                        ProgramsPageMain item = new ProgramsPageMain();
                        item.ProgramCode = code;
                        item.ProgramName = !string.IsNullOrEmpty(prog.Prog_Title) ? prog.Prog_Title : prog.Title;
                        item.ProgramName_EN = prog.Title_EN;
                        item.ProgramDesc = prog.Description;
                        item.ProgramDesc_EN = prog.Description_EN;
                        item.CollegeCode = prog.Coll_Code;
                        item.CollegeName = prog.Coll_Title;
                        item.CollegeName_EN = prog.Coll_Title_EN;
                        item.DepartmentCode = prog.Dept_Code;
                        item.DepartmentName = prog.Dept_Title;
                        item.DepartmentName_EN = prog.Dept_Title_EN;
                        item.Major = prog.Major;
                        item.Major_EN = prog.Major_EN;
                        item.ProgramNature = prog.ProgramNature;
                        item.ProgramNature_EN = prog.ProgramNature_EN;
                        item.ProgramFields = prog.ProgramFields;
                        item.ProgramFields_EN = prog.ProgramFields_EN;
                        item.SecondaryType = prog.SecondaryType;

                        if (deptProgDict.ContainsKey(code))
                        {
                            var dp = deptProgDict[code];
                            if (string.IsNullOrEmpty(item.ProgramName_EN)) item.ProgramName_EN = dp.Title_EN;
                            if (string.IsNullOrEmpty(item.CollegeName_EN)) item.CollegeName_EN = dp.COLL_DESC_EN;
                            if (string.IsNullOrEmpty(item.DepartmentName_EN)) item.DepartmentName_EN = dp.DEPT_DESC_EN;
                        }

                        if (studyPlanDict.ContainsKey(code))
                        {
                            var sp = studyPlanDict[code];
                            item.ProgramYears = sp.YRS;
                            item.ProgramLanguage = sp.LANGDESC;
                            item.ProgramDegree = sp.PROG_DESC_L;
                        }

                        allProgramsList.Add(item);
                    }
                }
                else if (allDeptProgList != null && allDeptProgList.Count > 0)
                {
                    foreach (var dp in allDeptProgList)
                    {
                        string code = dp.Code != null ? dp.Code.Trim() : string.Empty;
                        if (string.IsNullOrEmpty(code)) continue;

                        ProgramsPageMain item = new ProgramsPageMain();
                        item.ProgramCode = code;
                        item.ProgramName = dp.Title;
                        item.ProgramName_EN = dp.Title_EN;
                        item.ProgramDesc = dp.Description;
                        item.ProgramDesc_EN = dp.Description_EN;
                        item.CollegeCode = dp.COLL_CODE;
                        item.CollegeName = dp.COLL_DESC;
                        item.CollegeName_EN = dp.COLL_DESC_EN;
                        item.DepartmentCode = dp.DEPT_CODE;
                        item.DepartmentName = dp.DEPT_DESC;
                        item.DepartmentName_EN = dp.DEPT_DESC_EN;

                        if (studyPlanDict.ContainsKey(code))
                        {
                            var sp = studyPlanDict[code];
                            item.ProgramYears = sp.YRS;
                            item.ProgramLanguage = sp.LANGDESC;
                            item.ProgramDegree = sp.PROG_DESC_L;
                        }

                        allProgramsList.Add(item);
                    }
                }

                var distinctColleges = allProgramsList
                    .Where(p => !string.IsNullOrEmpty(p.CollegeCode) && !string.IsNullOrEmpty(p.CollegeName))
                    .GroupBy(p => p.CollegeCode.Trim())
                    .Select(g => new { Code = g.Key, Title = g.First().CollegeName, Title_EN = g.First().CollegeName_EN })
                    .OrderBy(c => c.Title)
                    .ToList();

                rptCollegeFilter.DataSource = distinctColleges;
                rptCollegeFilter.DataBind();

                rptPrograms.DataSource = allProgramsList;
                rptPrograms.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.Url.ToString() : "", "ucAcademicProgramsCards - BindData", ex.Message);
            }
        }
    }
}
