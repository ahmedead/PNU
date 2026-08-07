using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    public partial class ucMemberDegrees : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();

                if (Page.Request.QueryString["view"] == null)
                    return;
                string email = Request.QueryString["view"].ToString();
                email = email.ToLower().Trim();
                GetData(email);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            try
            {
                bool isArabic = SPContext.Current.Web.Language == 1025;
                ltrBachelorTitle.Text = isArabic ? "البكالوريوس" : "Bachelor";
                ltrMasterTitle.Text = isArabic ? "الماجستير" : "Master";
                ltrDoctorateTitle.Text = isArabic ? "الدكتوراه" : "Doctorate";
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucMemberDegrees - SetTitles", ex.Message);
            }
        }

        public void GetData(string email)
        {
            try
            {
                // Ensure new columns exist (runs once per app-domain)
                MembersResumesFieldProvisioner.EnsureFields();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList requestsList = web.Lists["MembersResumes"];

                            SPQuery query = new SPQuery();
                            query.Query = @"<Where>
                                              <Eq>
                                                 <FieldRef Name='Email' />
                                                 <Value Type='Text'>" + email + @"</Value>
                                              </Eq>
                                           </Where>";

                            SPListItemCollection items = requestsList.GetItems(query);

                            if (items != null && items.Count > 0)
                            {
                                SPListItem item = items[0];

                                if (requestsList.Fields.ContainsField("Bachelor") && item["Bachelor"] != null)
                                    ltrBachelor.Text = item["Bachelor"].ToString();

                                if (requestsList.Fields.ContainsField("Master") && item["Master"] != null)
                                    ltrMaster.Text = item["Master"].ToString();

                                if (requestsList.Fields.ContainsField("Doctorate") && item["Doctorate"] != null)
                                    ltrDoctorate.Text = item["Doctorate"].ToString();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }
    }

}
