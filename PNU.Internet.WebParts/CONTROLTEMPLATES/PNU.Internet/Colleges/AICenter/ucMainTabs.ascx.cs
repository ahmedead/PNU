using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers;
using Portal.Main.Helper;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucMainTabs : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Page.Request.QueryString["ResearchTrack"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab(2);", true);

                }

                if (Page.Request.QueryString["ChildResearchTrack"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab(2);", true);

                }

                if(Session["SelectedTab"] != null && Session["SelectedTab"].ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab(2);", true);
                    Session["SelectedTab"] = "";
                }
            }
        }


        protected void college2tab_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current URL
                string currentUrl = Request.Url.AbsoluteUri;

                // Get the URL without query string
                Uri uri = new Uri(currentUrl);
                string baseUrl = uri.GetLeftPart(UriPartial.Path);

                Session["SelectedTab"] = 2;
                // Redirect to the base URL
                Response.Redirect(baseUrl, false);


            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), sender.ToString(), ex.Message);
            }






        }
    }
}
