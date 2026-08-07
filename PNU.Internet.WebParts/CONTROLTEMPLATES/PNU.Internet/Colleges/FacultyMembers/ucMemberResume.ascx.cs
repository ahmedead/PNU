using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    public partial class ucMemberResume : UserControl
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
                ltrInterestsTitle.Text = isArabic ? "الاهتمامات البحثية" : "Research Interests";
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucMemberResume - SetTitles", ex.Message);
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
                            query.Query = string.Concat(
                                 @"<Where>
                                      <Eq>
                                         <FieldRef Name='Email' />
                                         <Value Type='Text'>" + email + @"</Value>
                                      </Eq>
                                   </Where>");

                            SPListItemCollection items = requestsList.GetItems(query);

                            if (items != null && items.Count > 0)
                            {
                                SPListItem item = items[0];

                                // نبذة مختصرة
                                ltrBrief.Text = item["BriefAbout"] != null ? item["BriefAbout"].ToString() : "";

                                // الاهتمامات البحثية (new column)
                                if (requestsList.Fields.ContainsField("ResearchInterests") && item["ResearchInterests"] != null)
                                    ltrInterests.Text = item["ResearchInterests"].ToString();

                                // المنصب الحالي — take the first item by Order
                                var json = !String.IsNullOrEmpty(Convert.ToString(item["CurrentPositions"])) ? Convert.ToString(item["CurrentPositions"]) : "";
                                if (!String.IsNullOrEmpty(json))
                                {
                                    var curPosObj = JsonConvert.DeserializeObject<List<CurrentPosition>>(json);
                                    if (curPosObj != null && curPosObj.Count > 0)
                                    {
                                        var topPosition = curPosObj.OrderBy(p => p.Order).First();
                                        ltrPosition.Text = topPosition.Title;
                                    }
                                }
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

    [Serializable]
    public class CurrentPosition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}
