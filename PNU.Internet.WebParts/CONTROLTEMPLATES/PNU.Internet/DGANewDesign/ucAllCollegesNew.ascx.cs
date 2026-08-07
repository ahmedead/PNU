using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucAllCollegesNew : UserControl
    {
        public string ListName { get; set; }
        public int TabsCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindColleges();
                }

                var Id = 0; // default to "All Colleges" tab
                if (Page.Request.QueryString["Id"] != null)
                {
                    Id = Convert.ToInt32(Page.Request.QueryString["Id"]);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('{Id}');", true);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }


        private void BindColleges()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        ListName = "CollegeCategory";
                        SPList list = web.Lists[ListName];
                        if (list != null)
                        {
                            SPListItemCollection listItems = list.Items;
                            List<AllCollegesFromBanner> _AllData = SPFactory.MapListItemsToClass<AllCollegesFromBanner>(listItems);

                            if (_AllData != null)
                            {
                                // Build each category tab with its colleges
                                for (int i = 0; i < _AllData.Count; i++)
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = string.Concat(
                                                     @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='COLL_CLASS_AR' />
                                                         <Value Type='Text'>" + _AllData[i].COLL_CLASS_AR + @"</Value>
                                                      </Eq>
                                                   </Where>");

                                    List<AllFaculties> _Data = SPFactory.GetAllItemsByQuery<AllFaculties>("Admin", Settings.AllFaculties, query);

                                    // IDs for category tabs start at 1
                                    _AllData[i].ID = (i + 1).ToString();
                                    if (_Data != null && _Data.Count > 0)
                                    {
                                        _AllData[i].Colleges = new List<AllFaculties>();
                                        _AllData[i].Colleges.AddRange(_Data);
                                    }
                                }

                                // Order categories as before
                                _AllData = _AllData.OrderByDescending(o => o.ID).ToList();

                                // Build the "All Colleges" aggregated tab (ID = 0, always first)
                                AllCollegesFromBanner allTab = new AllCollegesFromBanner
                                {
                                    ID = "0",
                                    COLL_CLASS_AR = "كل الكليات",
                                    COLL_CLASS_EN = "All Colleges",
                                    Colleges = new List<AllFaculties>()
                                };

                                foreach (var cat in _AllData)
                                {
                                    if (cat.Colleges != null && cat.Colleges.Count > 0)
                                    {
                                        allTab.Colleges.AddRange(cat.Colleges);
                                    }
                                }

                                // Deduplicate by Code (same college may appear in multiple categories)
                                allTab.Colleges = allTab.Colleges
                                    .GroupBy(c => c.Code)
                                    .Select(g => g.First())
                                    .ToList();

                                // Insert "All Colleges" as the first tab
                                List<AllCollegesFromBanner> _FinalData = new List<AllCollegesFromBanner>();
                                _FinalData.Add(allTab);
                                _FinalData.AddRange(_AllData);

                                TabsCount = _FinalData.Count;

                                masterRepeater.DataSource = _FinalData;
                                masterRepeater.DataBind();

                                masterRepeater1.DataSource = _FinalData;
                                masterRepeater1.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindRepeater(Repeater rptColleges1, string Category)
        {
            try
            {
                List<AllFaculties> collitem = LoadData(Category);
                if (collitem != null && collitem.Count > 0)
                {
                    rptColleges1.DataSource = collitem;
                    rptColleges1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        protected List<AllFaculties> LoadData(string Category)
        {
            try
            {
                ArrayList qryParam = new ArrayList();
                qryParam.Add("<Eq><FieldRef Name='Category'  /><Value Type='Choice'>" + Category + "</Value></Eq>");
                SPListItemCollection allItems = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, "Admin", ListName, qryParam, "ItemOrder", "TRUE");
                if (allItems == null || allItems.Count == 0)
                    return null;
                return SPFactory.MapListItemsToClass<AllFaculties>(allItems);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return null;
            }
        }
    }


}
