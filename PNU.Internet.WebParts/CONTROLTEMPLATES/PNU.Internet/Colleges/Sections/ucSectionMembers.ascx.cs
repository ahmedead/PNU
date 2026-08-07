using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections
{
    public partial class ucSectionMembers : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadSectionMembers();
                    //LoadFacultyMembers();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
            


        }

        //private void LoadFacultyMembers()
        //{
        //    if (Request.QueryString["SecCode"] == null)
        //        return;
        //    string SecCode = Request.QueryString["SecCode"].ToString();


        //    GRPBannerMapping _FacultyMember = null;
        //    List<clsCollMembersListName> _AllMembers = new List<clsCollMembersListName>();

        //    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
        //    {
        //        using (SPWeb web = site.OpenWeb("Admin"))
        //        {
        //            SPList _CollegesList = web.Lists["GRPBannerMapping"];
        //            SPQuery query = new SPQuery();
        //            query.Query = string.Concat(
        //                             @"<Where>
        //                                              <Eq>
        //                                                 <FieldRef Name='BannerDeptCode' />
        //                                                 <Value Type='Text'>" + SecCode + @"</Value>
        //                                              </Eq>
        //                                           </Where>");

        //            SPListItemCollection objSection = null;
        //            objSection = _CollegesList.GetItems(query);
        //            if (objSection == null || objSection.Count == 0)
        //                return;


        //            List<GRPBannerMapping> _Data = SPFactory.MapListItemsToClass<GRPBannerMapping>(objSection);//SPFactory.GetAllItemsByQuery<AllFacultyDepartments>("Admin", Settings.AllFacultyDepartments, query);
        //            if (_Data != null && _Data.Count > 0)
        //            {
        //                _FacultyMember = new GRPBannerMapping();
        //                _FacultyMember = _Data[0];

        //                SPQuery query1 = new SPQuery();
        //                if (_Data.Count == 1)
        //                {
        //                    query1.Query = string.Concat(
        //                                 @"<Where>
        //                                  <Contains>
        //                                     <FieldRef Name='SECTION_ID' />
        //                                     <Value Type='Text'>" + _FacultyMember.GRPDeptCode + @"</Value>
        //                                  </Contains>
        //                               </Where>");
        //                    SPList reqList = web.Lists["CollMembersListName"];
        //                    SPListItemCollection objNew = reqList.GetItems(query1);


        //                    if (objNew != null && objNew.Count > 0)
        //                        _AllMembers = SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);
        //                }
        //                else
        //                {
        //                    List<clsCollMembersListName> _AllConcatinatedData = new List<clsCollMembersListName>();
        //                    foreach (GRPBannerMapping obj in _Data)
        //                    {
        //                        query1 = new SPQuery();
        //                        query1.Query = string.Concat(
        //                                 @"<Where>
        //                                  <Contains>
        //                                     <FieldRef Name='SECTION_ID' />
        //                                     <Value Type='Text'>" + obj.GRPDeptCode + @"</Value>
        //                                  </Contains>
        //                               </Where>");
        //                        SPList reqList = web.Lists["CollMembersListName"];
        //                        SPListItemCollection objNew = reqList.GetItems(query1);

        //                        List<clsCollMembersListName> objNew2 = new List<clsCollMembersListName>();
        //                        if (objNew != null && objNew.Count > 0)
        //                        {
        //                            List<clsCollMembersListName> _AllMembers1 = SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);
        //                            _AllConcatinatedData.AddRange(_AllMembers1);
        //                        }
        //                    }

        //                    if (_AllConcatinatedData != null && _AllConcatinatedData.Count > 0)
        //                    {
        //                        _AllMembers.AddRange(_AllConcatinatedData);
        //                    }

        //                }




        //            }

        //            else
        //                return;



        //        }
        //    }


        //    //if (_FacultyMember == null)
        //    //    return;

        //    //List<clsCollMembersListName> _AllMembers = busclsFacultyMembers.GetAllSectionFacultyMembers(_FacultyMember.Title);

        //    if (_AllMembers != null && _AllMembers.Count > 0)
        //    {

        //        List<clsCollMembersListName> _Categories = new List<clsCollMembersListName>();
        //        _Categories = _AllMembers.Where(e => e.PROFESSION != null && e.PROFESSION != "").ToList();
        //        _Categories= _Categories.GroupBy(d => new { d.PROFESSION }).Select(group => group.First()).ToList();
        //        if(_Categories != null && _Categories.Count > 0)
        //        {
        //            List<clsFacultyMembersRepeaterData> _RepeaterData = new List<clsFacultyMembersRepeaterData>();
        //            int indexer = 1;
        //            foreach (clsCollMembersListName cat in _Categories)
        //            {
        //                clsFacultyMembersRepeaterData obj = new clsFacultyMembersRepeaterData();
        //                obj.ID = indexer.ToString();
        //                indexer = indexer + 1;
        //                obj.MainCategory = cat.PROFESSION;
        //                obj.MainCategory_EN = cat.PROFESSION_EN;
        //                List<clsCollMembersListName> _FilteredData = new List<clsCollMembersListName>();
        //                _FilteredData = _AllMembers.Where(e => e.PROFESSION == cat.PROFESSION).ToList();
        //                obj.FacultyMembers = new List<clsCollMembersListName>();
        //                if(_FilteredData != null && _FilteredData.Count > 0)
        //                {
        //                    obj.FacultyMembers.AddRange(_FilteredData);
        //                }

        //                _RepeaterData.Add(obj);

        //            }

        //            masterRepeater.DataSource= _RepeaterData;
        //            masterRepeater.DataBind();

        //            detailsRepeater.DataSource = _RepeaterData;
        //            detailsRepeater.DataBind();
        //        }


        //        //List<clsCollMembersListName> _FilteredItems = _AllMembers.Where(e => e.GRADE_NAME == Category).ToList();

        //        //BindRepeater(_AllMembers, Repeater1, "أستاذ", tab1);
        //        //BindRepeater(_AllMembers, Repeater2, "أستاذ مشارك", tab2);
        //        //BindRepeater(_AllMembers, Repeater3, "أستاذ مساعد", tab3);
        //        //BindRepeater(_AllMembers, Repeater4, "محاضر", tab4);
        //        //BindRepeater(_AllMembers, Repeater5, "معيد", tab5);
        //    }
        //}


        private void LoadSectionMembers()
        {
            try
            {
                if (Request.QueryString["SecCode"] == null)
                    return;
                string SecCode = Request.QueryString["SecCode"].ToString();


                GRPBannerMapping _FacultyMember = null;
                List<clsCollMembersListName> _AllMembers = new List<clsCollMembersListName>();

                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList _CollegesList = web.Lists["GRPBannerMapping"];
                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='BannerDeptCode' />
                                                         <Value Type='Text'>" + SecCode + @"</Value>
                                                      </Eq>
                                                   </Where>");

                        SPListItemCollection objSection = null;
                        objSection = _CollegesList.GetItems(query);
                        if (objSection == null || objSection.Count == 0)
                            return;

                        List<clsCollMembersListName> _AllMembersData = new List<clsCollMembersListName>();
                        List<GRPBannerMapping> _Data = SPFactory.MapListItemsToClass<GRPBannerMapping>(objSection);//SPFactory.GetAllItemsByQuery<AllFacultyDepartments>("Admin", Settings.AllFacultyDepartments, query);
                        if (_Data != null && _Data.Count > 0)
                        {
                            _FacultyMember = new GRPBannerMapping();
                            _FacultyMember = _Data[0];

                            SPQuery query1 = new SPQuery();
                            if (_Data.Count == 1)
                            {
                                query1.Query = string.Concat(
                                             @"<Where>
                                          <Eq>
                                             <FieldRef Name='SECTION_ID' />
                                             <Value Type='Text'>" + _FacultyMember.GRPDeptCode + @"</Value>
                                          </Eq>
                                       </Where>");
                                SPList reqList = web.Lists["CollMembersListName"];
                                SPListItemCollection objNew = reqList.GetItems(query1);


                                if (objNew != null && objNew.Count > 0)
                                    _AllMembers = SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);
                            }
                            else
                            {
                                List<clsCollMembersListName> _AllConcatinatedData = new List<clsCollMembersListName>();
                                foreach (GRPBannerMapping obj in _Data)
                                {
                                    query1 = new SPQuery();
                                    query1.Query = string.Concat(
                                             @"<Where>
                                          <Eq>
                                             <FieldRef Name='SECTION_ID' />
                                             <Value Type='Text'>" + obj.GRPDeptCode + @"</Value>
                                          </Eq>
                                       </Where>");
                                    SPList reqList = web.Lists["CollMembersListName"];
                                    SPListItemCollection objNew = reqList.GetItems(query1);

                                    List<clsCollMembersListName> objNew2 = new List<clsCollMembersListName>();
                                    if (objNew != null && objNew.Count > 0)
                                    {
                                        _AllMembers = SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);
                                        _AllConcatinatedData.AddRange(_AllMembers);
                                    }
                                }

                                if (_AllConcatinatedData != null && _AllConcatinatedData.Count > 0)
                                {
                                    _AllMembers = new List<clsCollMembersListName>();
                                    _AllMembers.AddRange(_AllConcatinatedData);
                                }

                            }




                        }

                        else
                            return;



                    }
                }


                //if (_FacultyMember == null)
                //    return;

                //List<clsCollMembersListName> _AllMembers = busclsFacultyMembers.GetAllSectionFacultyMembers(_FacultyMember.Title);

                if (_AllMembers != null && _AllMembers.Count > 0)
                {

                    BindRepeater(_AllMembers, Repeater1, "أستاذ", tab1);
                    BindRepeater(_AllMembers, Repeater2, "أستاذ مشارك", tab2);
                    BindRepeater(_AllMembers, Repeater3, "أستاذ مساعد", tab3);
                    BindRepeater(_AllMembers, Repeater4, "محاضر", tab4);
                    BindRepeater(_AllMembers, Repeater5, "معيد", tab5);
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        private void BindRepeater(List<clsCollMembersListName> _AllMembers, Repeater rptColleges1, string Category, HtmlGenericControl tab)
        {
            try
            {
                List<clsCollMembersListName> _FilteredItems = _AllMembers.Where(e => e.GRADE_NAME == Category).ToList();

                if (_FilteredItems != null && _FilteredItems.Count > 0)
                {
                    rptColleges1.DataSource = _FilteredItems;
                    rptColleges1.DataBind();
                }
                else
                {
                    //tab.Visible = false;
                    rptColleges1.DataSource = null;
                    rptColleges1.DataBind();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }



    }
}
