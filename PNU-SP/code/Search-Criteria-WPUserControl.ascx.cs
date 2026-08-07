using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.Search_Criteria_WP
{
    public partial class Search_Criteria_WPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               
            }

        }
        

        protected void btn_Click(object sender, EventArgs e)
        {
            var result = LoadData();
            foreach (SPListItem item in result)
            {
                lbl1.Text = Convert.ToString(item["Title"]);
                lbl2.Text = Convert.ToString(item["Created"]);
            }
        }

        protected SPListItemCollection LoadData()
        {
            var listname = "ActivitiesAndEvents";
            ArrayList qryParam = new ArrayList();
            if(txt.Text !="")
                qryParam.Add("<Eq><FieldRef Name='Title' /><Value Type='Text'>" + txt.Text + "</Value></Eq>");

            if(ddl1.SelectedIndex !=0)
                qryParam.Add("<Eq><FieldRef Name='SubCategory' /><Value Type='Choice'>" + ddl1.SelectedValue + "</Value></Eq>");
            
            if(ddl2.SelectedIndex !=0)
                qryParam.Add("<Eq><FieldRef Name='Faculty' /><Value Type='Choice'>" + ddl2.SelectedValue + "</Value></Eq>");

            if(cal.SelectedDate != null)
                qryParam.Add("<Eq><FieldRef Name='FromDate' /><Value IncludeTimeValue='FALSE' Type='DateTime'>" + cal.SelectedDate + "</Value></Eq>");


            return Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, listname, qryParam);
        }

        
    }
}
