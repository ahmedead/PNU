using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class UserControl2 : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["ID"] != null)
                    {
                        var ID = Page.Request.QueryString["ID"].ToString();

                        clsRequestsList _CurrentNew = busclsRequestsList.GetItemByID(ID);
                        //clsRequestsList _CurrentNew = new clsRequestsList();

                        //SPListItem objNew = null;
                        //SPSecurity.RunWithElevatedPrivileges(delegate ()
                        //{
                        //    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        //    {
                        //        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        //        {
                        //            SPList reqList = web.Lists["RequestsList"];
                        //            objNew = reqList.GetItemById(Convert.ToInt32(ID));
                        //        }
                        //    }
                        //});
                        //if (objNew != null)

                        //{
                        //    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(objNew);
                        //}


                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
                            _AllData.Add(_CurrentNew);
                            rptMainData.DataSource = _AllData;
                            rptMainData.DataBind();
                        }
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
