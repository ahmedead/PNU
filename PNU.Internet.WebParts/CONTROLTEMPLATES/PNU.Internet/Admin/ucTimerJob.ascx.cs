using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using iTextSharp.text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using PNU.Internet.WebParts.Layouts.PNU.Internet;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucTimerJob : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRunCources_Click(object sender, EventArgs e)
        {
            try
            {
                OracleDBContext.SaveInstructorCoursesDataToDB();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnRunCources_Click", ex.Message);
            }
        }

        protected void btnGadeerAPICall_Click(object sender, EventArgs e)
        {
            try
            {

                OracleDBContext.SaveGadeerCoursesDataToDB();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnGadeerAPICall_Click", ex.Message);

            }
        }

        protected void btnRunCources_EN_Click(object sender, EventArgs e)
        {
            try
            {
                OracleDBContext.SaveInstructorCourses_ENDataToDB();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnRunCources_Click", ex.Message);
            }
        }
        protected void btnGadeerAPICall_EN_Click(object sender, EventArgs e)
        {
            try
            {

                OracleDBContext.SaveGadeerCoursesDataToDB_EN();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnGadeerAPICall_EN_Click", ex.Message);

            }
        }


        protected void btnRunCourcesToList_Click(object sender, EventArgs e)
        {
            try
            {




                var sqldb = OracleDBContext.GetSQLDBConnection();
                SQLFactory.SetConn(sqldb);

                List<tblMemberCourses> coursesList = SQLFactory.ReadDataFromTable<tblMemberCourses>("tblMemberCourses", "", "", "");



                SaveDataIntoLists_tblMemberCourses(coursesList);


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnRunCourcesToList_Click", ex.Message);

            }
        }
        protected void btnRunCourcesToList_EN_Click(object sender, EventArgs e)
        {
            try
            {




                var sqldb = OracleDBContext.GetSQLDBConnection();
                SQLFactory.SetConn(sqldb);

                List<tblMemberCourses_EN> coursesList = SQLFactory.ReadDataFromTable<tblMemberCourses_EN>("tblMemberCourses_EN", "", "", "");



                SaveDataIntoLists_tblMemberCourses_EN(coursesList);


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnRunCourcesToList_EN_Click", ex.Message);

            }
        }
















        protected void btnStatistics_Click(object sender, EventArgs e)
        {
            try
            {

                //OracleDBContext.UpdateStatisticsDataToList("Statistics");

                //new method get statistics from powerpi view
                OracleDBContext.UpdatePowerPiStatisticsDataToList("Statistics");

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnStatistics_Click", ex.Message);

            }
        }



        protected void btnFacultyMembers_Click(object sender, EventArgs e)
        {
            try
            {

                OracleDBContext.SaveCollMemeberDataToList("CollMembersListName");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnFacultyMembers_Click", ex.Message);
            }
        }

        protected void btnAddProgramsForFirstTime_Click(object sender, EventArgs e)
        {
            try
            {
                var collDeptList = OracleDBContext.GetCollegeDeptsFromOracleDB();

                List<CollegeDeptsDto> distinctCategories = collDeptList.GroupBy(d => new { d.Coll_Code, d.Coll_Title, d.Dept_Code, d.Dept_Title, d.Prog_Code, d.Prog_Title }).Select(group => group.First()).ToList();
                string ListName = "AllPrograms";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list == null)
                            {
                                web.AllowUnsafeUpdates = true;
                                web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            list = web.Lists.TryGetList(ListName);


                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in distinctCategories)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem["Coll_Code"] = row.Coll_Code;
                                listItem["Coll_Title"] = row.Coll_Title;
                                listItem["Dept_Code"] = row.Dept_Code;
                                listItem["Dept_Title"] = row.Dept_Title;
                                listItem["Prog_Code"] = row.Prog_Code;
                                listItem["Prog_Title"] = row.Prog_Title;



                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual -btnAddProgramsForFirstTime_Click", ex.Message);
            }
        }

        protected void btnAddColleges_Click(object sender, EventArgs e)
        {
            try
            {
                var collDeptList = OracleDBContext.GetCollegeDeptsFromOracleDB();
                string ListName = "CollDeptListName";
                //var facultyList = collDeptList.Select(faculty => new { faculty.Coll_Code, faculty.Coll_Title }).Distinct();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list != null)
                            {
                                web.AllowUnsafeUpdates = true;
                                list.Delete();
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                            list = web.Lists.TryGetList(ListName);
                            list.Fields.Add("Coll_Code", SPFieldType.Text, false);
                            list.Fields.Add("Coll_Title", SPFieldType.Text, false);
                            list.Fields.Add("Dept_Code", SPFieldType.Text, false);
                            list.Fields.Add("Dept_Title", SPFieldType.Text, false);
                            list.Fields.Add("Prog_Code", SPFieldType.Text, false);
                            list.Fields.Add("Prog_Title", SPFieldType.Text, false);
                            list.Fields.Add("Major_Code", SPFieldType.Text, false);
                            list.Fields.Add("Major_Title", SPFieldType.Text, false);

                            SPView view = list.DefaultView;
                            view.ViewFields.Add("Coll_Code");
                            view.ViewFields.Add("Coll_Title");
                            view.ViewFields.Add("Dept_Code");
                            view.ViewFields.Add("Dept_Title");
                            view.ViewFields.Add("Prog_Code");
                            view.ViewFields.Add("Prog_Title");
                            view.ViewFields.Add("Major_Code");
                            view.ViewFields.Add("Major_Title");
                            view.Update();

                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in collDeptList)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem["Coll_Code"] = row.Coll_Code;
                                listItem["Coll_Title"] = row.Coll_Title;
                                listItem["Dept_Code"] = row.Dept_Code;
                                listItem["Dept_Title"] = row.Dept_Title;
                                listItem["Prog_Code"] = row.Prog_Code;
                                listItem["Prog_Title"] = row.Prog_Title;
                                listItem["Major_Code"] = row.Major_Code;
                                listItem["Major_Title"] = row.Major_Title;


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnAddColleges_Click", ex.Message);
            }
        }


        protected void btnAddCollegesNew_Click(object sender, EventArgs e)
        {
            try
            {
                List<CollegeCategoryDto> collCategoryList = OracleDBContext.GetOnlyCollegeClassFromOracleDB();
                List<CollegesDto> collList = OracleDBContext.GetOnlyCollegeFromOracleDB();
                List<DeptsDto> DeptsList = OracleDBContext.GetOnlyDeptFromOracleDB();
                List<ProgramsDto> ProgramsList = OracleDBContext.GetOnlyProgramsFromOracleDB();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPFactory.CreateCollegesCategoryList(collCategoryList);
                    SPFactory.CreateCollegesList(collList);
                    SPFactory.CreateDeptsList(DeptsList);
                    SPFactory.CreateProgramsList(ProgramsList);
                });



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnAddCollegesNew_Click", ex.Message);
            }
        }

        protected void btnAddNewStudyPlanOld_Click(object sender, EventArgs e)
        {
            try
            {
                var collDeptList = OracleDBContext.GetNewStudyPlanFromOracleDB();
                var collDeptList_EN = OracleDBContext.GetNewStudyPlanFromOracleDB_EN();
                string ListName = "NewStudyPlan";
                string ListName_EN = "NewStudyPlan_EN";

                GetDataIntoLists(ListName, collDeptList);

                GetDataIntoLists(ListName_EN, collDeptList_EN);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnAddNewStudyPlanOld_Click", ex.Message);
            }
        }

        private void GetDataIntoLists(string ListName, List<NewStudyPlanDto> collDeptList)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list != null)
                            {
                                web.AllowUnsafeUpdates = true;
                                list.Delete();
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                            list = web.Lists.TryGetList(ListName);

                            if (list != null)
                            {
                                list = SPFactory.MapListFieldsFromClass<NewStudyPlanDto>(list);
                            }


                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in collDeptList)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem = SPFactory.MapClassToSPListItem(listItem, row, true);


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - GetDataIntoLists", ex.Message);
            }
        }


        private void SaveDataIntoLists_tblMemberCourses(List<tblMemberCourses> MemberCourses)
        {
            try
            {
                string ListName = "tblMemberCourses";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list != null)
                            {
                                web.AllowUnsafeUpdates = true;
                                list.Delete();
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                            list = web.Lists.TryGetList(ListName);

                            if (list != null)
                            {
                                list = SPFactory.MapListFieldsFromClass<tblMemberCourses>(list);
                            }


                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in MemberCourses)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem = SPFactory.MapClassToSPListItem(listItem, row, true);


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - SaveDataIntoLists_tblMemberCourses", ex.Message);
            }
        }


        private void SaveDataIntoLists_tblMemberCourses_EN(List<tblMemberCourses_EN> MemberCourses)
        {
            try
            {
                string ListName = "tblMemberCourses_EN";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list != null)
                            {
                                web.AllowUnsafeUpdates = true;
                                list.Delete();
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                            list = web.Lists.TryGetList(ListName);

                            if (list != null)
                            {
                                list = SPFactory.MapListFieldsFromClass<tblMemberCourses_EN>(list);
                            }


                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in MemberCourses)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem = SPFactory.MapClassToSPListItem(listItem, row, true);


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - SaveDataIntoLists_tblMemberCourses_EN", ex.Message);
            }
        }

        protected void btnAcademicCredits_Click(object sender, EventArgs e)
        {
            try
            {
                var collDeptList = OracleDBContext.GetAcademicCreditsFromOracleDB();
                string ListName = "AcademicCredits";
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(ListName);
                            if (list != null)
                            {
                                web.AllowUnsafeUpdates = true;
                                list.Delete();
                                web.AllowUnsafeUpdates = false;
                            }

                            web.AllowUnsafeUpdates = true;
                            web.Lists.Add(ListName, "", SPListTemplateType.GenericList);
                            list = web.Lists.TryGetList(ListName);





                            list.Fields.Add("COLL_CODE", SPFieldType.Text, false);
                            list.Fields.Add("COLL_DESC", SPFieldType.Text, false);
                            list.Fields.Add("PROG_DESC", SPFieldType.Text, false);
                            list.Fields.Add("PROG_CODE", SPFieldType.Text, false);
                            list.Fields.Add("DEPT_CODE", SPFieldType.Text, false);
                            list.Fields.Add("DEPT_DESC", SPFieldType.Text, false);
                            list.Fields.Add("MAJR_CODE", SPFieldType.Text, false);
                            list.Fields.Add("MAJR_DESC", SPFieldType.Text, false);
                            list.Fields.Add("PRG_CRED_CLASS", SPFieldType.Text, false);
                            list.Fields.Add("PRG_CRED_SPN", SPFieldType.Text, false);


                            SPView view = list.DefaultView;
                            view.ViewFields.Add("COLL_CODE");
                            view.ViewFields.Add("COLL_DESC");
                            view.ViewFields.Add("PROG_DESC");
                            view.ViewFields.Add("PROG_CODE");
                            view.ViewFields.Add("DEPT_CODE");
                            view.ViewFields.Add("DEPT_DESC");
                            view.ViewFields.Add("MAJR_CODE");
                            view.ViewFields.Add("MAJR_DESC");
                            view.ViewFields.Add("PRG_CRED_CLASS");
                            view.ViewFields.Add("PRG_CRED_SPN");


                            view.Update();

                            web.AllowUnsafeUpdates = false;

                            if (list == null) return;

                            foreach (var row in collDeptList)
                            {

                                SPListItem listItem = list.Items.Add();

                                listItem = SPFactory.MapClassToSPListItem(listItem, row, true);


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnAcademicCredits_Click", ex.Message);
            }
        }

        protected void btnAddNewStudyPlan_Click(object sender, EventArgs e)
        {
            try
            {
                List<PLANS_ELEC_U> PLANS_ELEC_UList = OracleDBContext.GetPLANS_ELEC_U("CUSTAPP.MOBAPPL_PLANS_ELEC_U_M");
                List<PLANS_ELEC_C> PLANS_ELEC_CList = OracleDBContext.GetPLANS_ELEC_C("CUSTAPP.MOBAPPL_PLANS_ELEC_C_M");
                List<PLANS_ELEC_P> PLANS_ELEC_PList = OracleDBContext.GetPLANS_ELEC_P("CUSTAPP.MOBAPPL_PLANS_ELEC_P_M");
                //List<ProgramsDto> ProgramsList = OracleDBContext.GetOnlyProgramsFromOracleDB();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPFactory.CreatePLANS_ELEC_UList(PLANS_ELEC_UList, "PLANS_ELEC_U");
                    SPFactory.CreatePLANS_ELEC_CList(PLANS_ELEC_CList, "PLANS_ELEC_C");
                    SPFactory.CreatePLANS_ELEC_PList(PLANS_ELEC_PList, "PLANS_ELEC_P");

                });

                //EN

                List<PLANS_ELEC_U> PLANS_ELEC_UList_EN = OracleDBContext.GetPLANS_ELEC_U("CUSTAPP.MOBAPPL_PLANS_ELEC_UEN_M");
                List<PLANS_ELEC_C> PLANS_ELEC_CList_EN = OracleDBContext.GetPLANS_ELEC_C("CUSTAPP.MOBAPPL_PLANS_ELEC_CEN_M");
                List<PLANS_ELEC_P> PLANS_ELEC_PList_EN = OracleDBContext.GetPLANS_ELEC_P("CUSTAPP.MOBAPPL_PLANS_ELEC_PEN_M");
                //List<ProgramsDto> ProgramsList = OracleDBContext.GetOnlyProgramsFromOracleDB();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPFactory.CreatePLANS_ELEC_UList(PLANS_ELEC_UList_EN, "PLANS_ELEC_U_EN");
                    SPFactory.CreatePLANS_ELEC_CList(PLANS_ELEC_CList_EN, "PLANS_ELEC_C_EN");
                    SPFactory.CreatePLANS_ELEC_PList(PLANS_ELEC_PList_EN, "PLANS_ELEC_P_EN");

                });



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "TimerJobsManual - btnAddNewStudyPlan_Click", ex.Message);



            }
        }

    }
}
