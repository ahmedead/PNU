using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.WithoutVideoUpload
{
    public partial class ucNewAllNewsNew : UserControl
    {
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
                    //List<clsLookUP> MediaTypes = busclsRequestsList.GetAllItemsLookup("MediaTypes");
                    List<clsLookUP> MainCategory = busclsRequestsList.GetAllItemsLookup("MainCategory");

                    List<clsRequestsList> _allData = new List<clsRequestsList>();
                    //_allData = busclsRequestsList.GetAllItemsIsHome();
                    _allData = busclsRequestsList.GetAllItemsIsHomePending();
                    if (_allData != null && _allData.Count > 0)
                    {
                        List<clsAllRequestsList> _allCategories = new List<clsAllRequestsList>();
                        List<clsRequestsList> Categories = new List<clsRequestsList>();
                        Categories = _allData.GroupBy(d => new { d.MainCategory }).Select(group => group.First()).ToList();
                        int i = 1;

                        if (MainCategory != null && MainCategory.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab('new1');", true);

                            foreach (clsLookUP c in MainCategory)
                            {
                                clsAllRequestsList objCat = new clsAllRequestsList();
                                objCat.ID = i.ToString();
                                i = i + 1;
                                objCat.MainCategory = c.Title;
                                objCat.MainCategory_EN = c.TitleEn;

                                List<clsRequestsList> _aalDataByLevel = new List<clsRequestsList>();
                                _aalDataByLevel = _allData.Where(d => d.MainCategory == objCat.MainCategory && d.IsHome == true).ToList();

                                if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                                {
                                    objCat.Requests = new List<clsRequestsList>();

                                    objCat.Requests.AddRange(_aalDataByLevel);
                                }

                                _allCategories.Add(objCat);


                                // الوسائط الرقمية

                                objCat = new clsAllRequestsList();
                                objCat.ID = (i).ToString();
                                i = i + 1;
                                objCat.MainCategory = SPFactory.GetPNUresResource("DigitalMedia", "AR");
                                objCat.MainCategory_EN = SPFactory.GetPNUresResource("DigitalMedia", "EN");

                                _aalDataByLevel = new List<clsRequestsList>();
                                _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategoryNotMainCat();

                                if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                                {

                                    objCat.Requests = new List<clsRequestsList>();
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
