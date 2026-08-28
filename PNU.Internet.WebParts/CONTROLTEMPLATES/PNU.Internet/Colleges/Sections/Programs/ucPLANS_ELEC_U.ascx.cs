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
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        public void BindDataU()
        {
            try
            {
                List<PLANS_ELEC_U> _AllData = new List<PLANS_ELEC_U>();
                string PLANS_ELEC_U_ListName = SPFactory.GetLocalizedTitle("PLANS_ELEC_U", "PLANS_ELEC_U_EN");
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList(PLANS_ELEC_U_ListName);
                            if (list != null)
                            {
                                SPListItemCollection items = list.GetItems();
                                if (items != null && items.Count > 0)
                                {
                                    _AllData = SPFactory.MapListItemsToClass<PLANS_ELEC_U>(items);
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
                    int i = 20;
                    foreach (PLANS_ELEC_U objLevel in distinctCategories)
                    {
                        AllPLANS_ELECMain obj = new AllPLANS_ELECMain();
                        obj.LevelCode = i.ToString();
                        i++;
                        obj.LevelDesc = objLevel.STVATTR_DESC;

                        var courses = _AllData.Where(d => d.STVATTR_DESC == objLevel.STVATTR_DESC).ToList();
                        if (courses != null && courses.Count > 0)
                        {
                            obj.StudyPlan = courses;
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
