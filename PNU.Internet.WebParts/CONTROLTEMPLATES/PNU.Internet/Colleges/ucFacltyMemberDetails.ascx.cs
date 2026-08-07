using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Web;
using Microsoft.SharePoint;
using System.Collections.Generic;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public partial class ucFacltyMemberDetails : UserControl
    {

        public int id { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindFacultyMemberDetails();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        private void BindFacultyMemberDetails()
        {
            try
            { //string newPortalURL = "https://newportal.pnu.edu.sa/ar/Faculties";
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                //using (SPSite site = new SPSite(newPortalURL))
                {
                    using (SPWeb web = site.OpenWeb("/ar/Faculties/HumanitiesColleges/FacultyOfEducation"))
                    {
                        SPList list = web.Lists["FacultyMembers"];
                        if (list != null)
                        {
                            Uri myUri = new Uri(Request.Url.AbsoluteUri);
                            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("view");
                            id = Convert.ToInt32(param1);

                            SPQuery query = new SPQuery();
                            query.Query = "<Where> <Eq> <FieldRef Name='ID' /> <Value Type='Counter'>" + id + "</Value> </Eq> </Where>";
                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {
                                List<clsFacultyMembers> AllFacultyMembersList = new List<clsFacultyMembers>();

                                if (collitem != null)
                                {
                                    if (collitem[0] != null)
                                    {
                                        SPListItem item = collitem[0];
                                        if (item["Title"] != null)
                                        {
                                            var member = new clsFacultyMembers
                                            {

                                                Name = item["Name"] == null ? "" : item["Name"].ToString(),
                                                Title = item["Title"] == null ? "" : item["Title"].ToString(),
                                                Email = item["Email"] == null ? "" : item["Email"].ToString(),
                                                Phone = item["Phone"] == null ? "" : item["Phone"].ToString(),
                                                LinkedInUrl = item["LinkedInUrl"] == null ? "" : item["LinkedInUrl"].ToString(),
                                                Bio = item["Bio"] == null ? "" : item["Bio"].ToString(),
                                                SubjectTitle1 = item["SubjectTitle1"] == null ? "" : item["SubjectTitle1"].ToString(),
                                                SubjectDesc1 = item["SubjectDesc1"] == null ? "" : item["SubjectDesc1"].ToString(),
                                            };
                                            AllFacultyMembersList.Add(member);
                                        }
                                    }








                                    rptFacultyMembers.DataSource = AllFacultyMembersList;
                                    rptFacultyMembers.DataBind();
                                }
                            }
                        }
                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

           
        }

    }
}
