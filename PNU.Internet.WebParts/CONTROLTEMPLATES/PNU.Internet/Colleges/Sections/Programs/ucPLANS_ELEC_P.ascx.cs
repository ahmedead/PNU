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
    public partial class ucPLANS_ELEC_P : UserControl
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
            {
                if (Request.QueryString["ProgramCode"] == null)
                    return;
                string ProgramCode = Request.QueryString["ProgramCode"].ToString();
                List<PLANS_ELEC_P> _AllData = new List<PLANS_ELEC_P>();
                string PLANS_ELEC_P_ListName = SPFactory.GetLocalizedTitle("PLANS_ELEC_P", "PLANS_ELEC_P_EN");
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(PLANS_ELEC_P_ListName);

                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         @"<Where>
                              <Eq>
                                 <FieldRef Name='SMRPRLE_PROGRAM' />
                                 <Value Type='Text'>" + ProgramCode + @"</Value>
                              </Eq>
                           </Where>");

                        SPListItemCollection items = list.GetItems(query);

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


                        _AllData = SPFactory.MapListItemsToClass<PLANS_ELEC_P>(items);



                    }
                }



                List<AllPLANS_ELECMain> _MainData = new List<AllPLANS_ELECMain>();



                List<PLANS_ELEC_P> _allLevels = new List<PLANS_ELEC_P>();
                _allLevels = _AllData.GroupBy(d => new { d.STVATTR_DESC }).Select(group => group.First()).ToList();
                if (_allLevels != null && _allLevels.Count > 0)
                {
                    int i = 2020;
                    foreach (PLANS_ELEC_P objLevel in _allLevels)
                    {
                        AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                        obj.LevelCode = i.ToString();
                        i = i + 1;
                        obj.LevelDesc = objLevel.STVATTR_DESC.ToString();

                        List<PLANS_ELEC_P> _aalDataByLevel = new List<PLANS_ELEC_P>();
                        _allLevels = _AllData.Where(d => d.STVATTR_DESC == objLevel.STVATTR_DESC).ToList();

                        if (_allLevels != null && _allLevels.Count > 0)
                        {
                            
                            _allLevels = _allLevels.GroupBy(d => new { d.SCRATTR_SUBJ_CODE, d.SCRATTR_CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                            if (_allLevels != null && _allLevels.Count > 0)
                            {
                                obj.StudyPlanP = _allLevels;
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
