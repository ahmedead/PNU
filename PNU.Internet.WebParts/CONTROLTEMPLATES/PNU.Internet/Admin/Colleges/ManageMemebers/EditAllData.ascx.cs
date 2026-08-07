using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers
{
    public partial class EditAllData : UserControl
    {
        protected string GetUserEmail()
        {
            try
            {
                
                if (Request.QueryString["IsAdmin"] != null)
                {
                    string IsAdmin = Request.QueryString["IsAdmin"].ToString();
                    if(IsAdmin == "YesIsAdmin")
                    {
                        string email = Request.QueryString["email"].ToString();
                        if(email != null)
                        {
                            return email;
                        }
                    }
                }
                if (SPContext.Current.Web.CurrentUser == null)
                    return "";
                if (SPContext.Current.Web.CurrentUser.Email == null)
                    return "";
                if (SPContext.Current.Web.CurrentUser.Email == "")
                    return "";

                return SPContext.Current.Web.CurrentUser.Email.ToLower().Trim();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
                return "";
            }
            

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadData();

            }



        }

        private void LoadData()
        {
            try
            {
                string email = GetUserEmail();


                if (email == "")
                {
                    pnlData.Visible = false;
                    return;
                }
                clsCollMembersListName _FacultyMember = busclsFacultyMembers.GetFacultyMemberByEmail(email);
                List<clsCollMembersListName> _AllMainData = new List<clsCollMembersListName>();


                if (_FacultyMember != null)
                {
                    _AllMainData.Add(_FacultyMember);
                    rptMainData.DataSource = _AllMainData;
                    rptMainData.DataBind();
                }
                else
                {
                    pnlData.Visible = false;
                    return;
                }



            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
        }
    }                                 
}
