using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucSliderNew : UserControl
    {
        public string ListName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<clsSlider> _AllItems = busclsSlider.GetAllItems();

                    if (_AllItems != null && _AllItems.Count > 0)
                        _AllItems[0].ClassName = " active";
                    rptSlider.DataSource = _AllItems;
                    rptSlider.DataBind();

                    rptIndicators.DataSource = _AllItems;
                    rptIndicators.DataBind();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        
        
    }
}
