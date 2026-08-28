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
            try
            {
                if (!Page.IsPostBack && !string.IsNullOrEmpty(Request.QueryString["COLL_CODE"]))
                {
                    BindDataU(Request.QueryString["COLL_CODE"].ToString());
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        public void BindDataU(string COLL_CODE)
        {
            try
            {
                if (string.IsNullOrEmpty(COLL_CODE))
                    return;

                List<PLANS_ELEC_C> _AllData = new List<PLANS_ELEC_C>();
                string PLANS_ELEC_C_ListName = SPFactory.GetLocalizedTitle("PLANS_ELEC_C", "PLANS_ELEC_C_EN");
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(PLANS_ELEC_C_ListName);
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = string.Concat(
                                                 @"<Where>
                                      <Eq>
                                         <FieldRef Name='SMRPRLE_COLL_CODE' />
                                         <Value Type='Text'>" + COLL_CODE + @"</Value>
                                      </Eq>
                                   </Where>");

                                SPListItemCollection items = list.GetItems(query);
                                if (items != null && items.Count > 0)
                                {
                                    _AllData = SPFactory.MapListItemsToClass<PLANS_ELEC_C>(items);
                                }
                            }
                        }
                    }
                });

                if (_AllData == null || _AllData.Count == 0)
                {
                    masterRepeaterU.DataSource = null;
                    masterRepeaterU.DataBind();
                    return;
                }

                var distinctCategories = _AllData.Where(d => d.STVATTR_DESC != null).GroupBy(d => d.STVATTR_DESC).Select(group => group.First()).ToList();
                List<AllPLANS_ELECMain> _MainData = new List<AllPLANS_ELECMain>();
                if (distinctCategories != null && distinctCategories.Count > 0)
                {
                    int i = 1020;
                    foreach (PLANS_ELEC_C objLevel in distinctCategories)
                    {
                        AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                        obj.LevelCode = i.ToString();
                        i++;
                        obj.LevelDesc = objLevel.STVATTR_DESC;

                        var courses = _AllData.Where(d => d.STVATTR_DESC == objLevel.STVATTR_DESC && d.SCRATTR_SUBJ_CODE != null && d.SCRATTR_CRSE_NUMB != null).ToList();
                        if (courses != null && courses.Count > 0)
                        {
                            obj.StudyPlanC = courses.GroupBy(d => new { d.SCRATTR_SUBJ_CODE, d.SCRATTR_CRSE_NUMB, d.COURSE_TITLE, d.CREDIT }).Select(group => group.First()).ToList();
                        }

                        _MainData.Add(obj);
                    }

                    masterRepeaterU.DataSource = _MainData;
                    masterRepeaterU.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }
    }
}
