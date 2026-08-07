using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucAcademicCredits : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string Code = "";
                    List<AcademicCreditsDto> data = new List<AcademicCreditsDto>();
                    if (Request.QueryString["Source"] != null)
                    {
                        Code = Request.QueryString["Source"].ToString();
                        data = busclsAcademicCredits.GetAllAcademicCreditsByCollegeCode(Code);
                    }
                    if (Request.QueryString["SecCode"] != null)
                    {
                        Code = Request.QueryString["SecCode"].ToString();
                        data = busclsAcademicCredits.GetAllAcademicCreditsByDepartmentCode(Code);
                    }
                    if (Request.QueryString["ProgramCode"] != null)
                    {
                        Code = Request.QueryString["ProgramCode"].ToString();
                        data = busclsAcademicCredits.GetAllAcademicCreditsByProgCode(Code);
                    }

                    if (data == null || data.Count == 0)
                        pnlData.Visible = false;

                    rptData.DataSource = data;
                    rptData.DataBind();

                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
    }
}
