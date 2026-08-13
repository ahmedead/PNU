using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucDetails : UserControl
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



                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
                            _AllData.Add(_CurrentNew);
                            //rptMainData.DataSource = _AllData;
                            //rptMainData.DataBind();
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
