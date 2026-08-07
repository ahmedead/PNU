using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucAICenterNews : UserControl
    {
        public int TabsCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                
                if (!IsPostBack)
                {
                    string CollegeCode = "";

                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["DepartmentDetails"];
                            if (list != null)
                            {

                                SPListItemCollection collitem = list.GetItems();
                                if (collitem != null)
                                {
                                    if (collitem.Count > 0)
                                        if (collitem[0]["DeptCode"] != null)
                                            CollegeCode = collitem[0]["DeptCode"].ToString().Trim();
                                }


                            }
                        }
                    }

                    List<clsRequestsList> _allData = new List<clsRequestsList>();
                    _allData = busclsRequestsList.GetCollegeNews(CollegeCode);

                    //_allData = busclsRequestsList.GetAllItemsIsHome();
                    if (_allData != null && _allData.Count > 0)
                    {
                        

                        rptNews.DataSource = _allData;
                        rptNews.DataBind();

                       

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
