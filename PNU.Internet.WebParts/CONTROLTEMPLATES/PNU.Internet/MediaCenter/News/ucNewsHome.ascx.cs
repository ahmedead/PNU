using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucNewsHome : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int RowLimit { get; set; } = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindNews();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }



        }

        
        private void BindNews()
        {
            try
            {
                List<clsLookUP> MainCategory = busclsRequestsList.GetAllItemsLookup("MainCategory");
                List<clsAllRequestsList> _allCategories = new List<clsAllRequestsList>();
                int i = 1;

                if (MainCategory != null && MainCategory.Count > 0)
                {

                    foreach (clsLookUP c in MainCategory)
                    {
                        clsAllRequestsList objCat = new clsAllRequestsList();
                        objCat.ID = i.ToString();
                        i = i + 1;
                        objCat.MainCategory = c.Title;
                        objCat.MainCategory_EN = c.TitleEn;

                        List<clsRequestsList> _aalDataByLevel = new List<clsRequestsList>();
                        
                        _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategory(objCat.MainCategory, RowLimit);

                        if (_aalDataByLevel != null && _aalDataByLevel.Count > 0)
                        {
                            objCat.Requests = new List<clsRequestsList>();
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
                        _aalDataByLevel = busclsRequestsList.GetAllItemsByMediaCategoryNotMainCat(RowLimit);

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

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

    }
}
