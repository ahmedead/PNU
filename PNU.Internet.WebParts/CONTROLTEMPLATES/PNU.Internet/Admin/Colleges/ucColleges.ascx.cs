using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges
{
    public partial class ucColleges : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNewItem_Click(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategory.SelectedValue != "-1")
                {
                    List<clsFaculties> _AllFaculties = new List<clsFaculties>();
                    _AllFaculties = busclsFaculties.GetAllItems();

                    if (_AllFaculties != null && _AllFaculties.Count > 0)
                    {
                        _AllFaculties = _AllFaculties.ToList().Where(item => item.Category == ddlCategory.SelectedValue).ToList();//.OrderByDescending(pet => pet.ItemOrder).ToList();
                        ddlFaculties.DataTextField = "FacultyName";
                        ddlFaculties.DataSource = _AllFaculties;
                        ddlFaculties.DataBind();
                        ddlFaculties.Items.Insert(0, new ListItem("--اختر--", "-1"));
                    }

                }
                else
                {
                    ddlFaculties.DataSource = null;
                    ddlFaculties.DataBind();

                }

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void ddlFaculties_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
