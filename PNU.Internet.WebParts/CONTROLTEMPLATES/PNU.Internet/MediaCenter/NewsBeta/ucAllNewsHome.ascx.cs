using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.BusinessClassess;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;



namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta
{
    public partial class ucAllNewsHome : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string WebUrl { get; set; } = "/ar/MediaCenter/NewsBeta/";

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; } = "RequestsList";

        public int TabsCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var Id = 1;
                if (Page.Request.QueryString["Id"] != null)
                {
                    Id = Convert.ToInt32(Page.Request.QueryString["Id"]);

                }
                else
                { Id = TabsCount; }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('{Id}');", true);

                //AllMCNews.HRef = "https://" + HttpContext.Current.Request.Url.Host.ToString() + "/ar/MediaCenter/Pages/AllNews.aspx";
                if (!IsPostBack)
                {
                    //List<clsLookUPNewsBeta> MediaTypes = busclsRequestsListNewsBeta.GetAllItemsLookup("MediaTypes");
                    List<clsLookUPNewsBeta> MainCategory = busclsRequestsListNewsBeta.GetAllItemsLookup(WebUrl,"MainCategory");

                    List<clsRequestsListNewsBeta> _allData = new List<clsRequestsListNewsBeta>();
                    _allData = busclsRequestsListNewsBeta.GetAllItemsIsHome(WebUrl, ListName);
                    //_allData = busclsRequestsListNewsBeta.GetAllItemsIsHomePending();
                    if (_allData != null && _allData.Count > 0)
                    {
                        List<clsAllRequestsListNewsBeta> _allCategories = new List<clsAllRequestsListNewsBeta>();
                        List<clsRequestsListNewsBeta> Categories = new List<clsRequestsListNewsBeta>();
                        Categories = _allData.GroupBy(d => new { d.MainCategory }).Select(group => group.First()).ToList();
                        int i = 1;

                        if (MainCategory != null && MainCategory.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('new1');", true);

                            foreach (clsLookUPNewsBeta c in MainCategory)
                            {
                                clsAllRequestsListNewsBeta objCat = new clsAllRequestsListNewsBeta();
                                objCat.ID = i.ToString();
                                i = i + 1;
                                objCat.MainCategory = c.Title;
                                objCat.MainCategory_EN = c.TitleEn;

                                List<clsRequestsListNewsBeta> _aalDataByLevel = new List<clsRequestsListNewsBeta>();
                                _aalDataByLevel = _allData.Where(d => d.MainCategory == objCat.MainCategory && d.IsHome == true).ToList();

                                if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                                {
                                    objCat.Requests = new List<clsRequestsListNewsBeta>();

                                    objCat.Requests.AddRange(_aalDataByLevel);
                                }

                                _allCategories.Add(objCat);


                                // الوسائط الرقمية

                                objCat = new clsAllRequestsListNewsBeta();
                                objCat.ID = (i).ToString();
                                i = i + 1;
                                objCat.MainCategory = SPFactory.GetPNUresResource("DigitalMedia", "AR");
                                objCat.MainCategory_EN = SPFactory.GetPNUresResource("DigitalMedia", "EN");

                                _aalDataByLevel = new List<clsRequestsListNewsBeta>();
                                _aalDataByLevel = busclsRequestsListNewsBeta.GetAllItemsByMediaCategoryNotMainCat(WebUrl, "DigitalMedia");

                                if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                                {

                                    objCat.Requests = new List<clsRequestsListNewsBeta>();
                                    objCat.Requests.AddRange(_aalDataByLevel);

                                }



                                _allCategories.Add(objCat);

                                break;
                            }
                        }


                        masterRepeater.DataSource = _allCategories;
                        masterRepeater.DataBind();

                        detailsRepeater.DataSource = _allCategories;
                        detailsRepeater.DataBind();
                    }

                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

    }
}
