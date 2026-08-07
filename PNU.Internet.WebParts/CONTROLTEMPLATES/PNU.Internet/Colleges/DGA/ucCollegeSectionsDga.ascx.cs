using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA
{
    /// <summary>
    /// Departments (الأقسام) tab - DGA design, EXISTING lists only:
    ///   Admin web / AllFacultyDepartments  (filtered by COLL_CODE)
    ///   Admin web / AllDepartmentPrograms  (filtered by DEPT_CODE)
    ///
    /// Renders one <section id="faculty-department-{index}"> per department -
    /// the SAME anchors and the SAME order that ucSideMenu generates for the
    /// Sections sub-menu, so sub-menu clicks land exactly on the department.
    /// </summary>
    public partial class ucCollegeSectionsDga : UserControl
    {
        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack || ltrSections.Text.Length == 0)
                    RenderDepartments();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void RenderDepartments()
        {
            string collegeCode = GetCollegeCode();
            if (string.IsNullOrEmpty(collegeCode)) return;

            StringBuilder sb = new StringBuilder();

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb("Admin"))
            {
                SPList deptList = web.Lists.TryGetList("AllFacultyDepartments");
                if (deptList == null) return;
                SPList progList = web.Lists.TryGetList("AllDepartmentPrograms");

                SPQuery query = new SPQuery
                {
                    Query = @"<Where>
                                 <Eq>
                                    <FieldRef Name='COLL_CODE' />
                                    <Value Type='Text'>" + collegeCode + @"</Value>
                                 </Eq>
                              </Where>"
                };

                int index = 0;
                SPListItemCollection depts = deptList.GetItems(query);
                int total = depts.Count;

                foreach (SPListItem dept in depts)
                {
                    string nameAr = ucSideMenu.GetFirstFieldValue(dept, "DEPT_NAME", "NAME", "Title");
                    string nameEn = ucSideMenu.GetFirstFieldValue(dept, "DEPT_NAME_EN", "ENG_NAME", "NAME_EN", "Title_EN");
                    string name = IsArabic
                        ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                        : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);
                    if (string.IsNullOrEmpty(name)) continue;

                    string about = IsArabic
                        ? ucSideMenu.GetFirstFieldValue(dept, "DEPT_ABOUT", "About", "Description")
                        : ucSideMenu.GetFirstFieldValue(dept, "DEPT_ABOUT_EN", "About_EN", "Description_EN",
                                                              "DEPT_ABOUT", "About", "Description");
                    string deptCode = ucSideMenu.GetFirstFieldValue(dept, "DEPT_CODE", "Code");

                    bool last = (index == total - 1);
                    sb.AppendFormat("<section id=\"faculty-department-{0}\" class=\"pnu-section-anchor{1}\">",
                        index, last ? "" : " mb-5");
                    sb.AppendFormat("<h2>{0}</h2>", HttpUtility.HtmlEncode(name));

                    if (!string.IsNullOrEmpty(about))
                    {
                        sb.AppendFormat("<h3 class=\"h5 mt-4\">{0}</h3>", IsArabic ? "عن القسم" : "About the Department");
                        sb.AppendFormat("<p>{0}</p>", HttpUtility.HtmlEncode(about));
                    }

                    // ---- programs of this department ----
                    if (progList != null && !string.IsNullOrEmpty(deptCode))
                    {
                        SPQuery pq = new SPQuery
                        {
                            Query = @"<Where>
                                         <Eq>
                                            <FieldRef Name='DEPT_CODE' />
                                            <Value Type='Text'>" + deptCode + @"</Value>
                                         </Eq>
                                      </Where>"
                        };

                        SPListItemCollection programs = progList.GetItems(pq);
                        if (programs != null && programs.Count > 0)
                        {
                            sb.AppendFormat("<h3 class=\"h5 mt-4\">{0}</h3>",
                                IsArabic ? "البرامج الأكاديمية للقسم" : "Academic Programs");
                            sb.Append("<div class=\"row g-3\">");

                            foreach (SPListItem prog in programs)
                            {
                                string pNameAr = ucSideMenu.GetFirstFieldValue(prog, "PROG_NAME", "NAME", "Title");
                                string pNameEn = ucSideMenu.GetFirstFieldValue(prog, "PROG_NAME_EN", "ENG_NAME", "NAME_EN", "Title_EN");
                                string pName = IsArabic
                                    ? (!string.IsNullOrEmpty(pNameAr) ? pNameAr : pNameEn)
                                    : (!string.IsNullOrEmpty(pNameEn) ? pNameEn : pNameAr);
                                if (string.IsNullOrEmpty(pName)) continue;

                                string pCode = ucSideMenu.GetFirstFieldValue(prog, "Code", "PROG_CODE");
                                string url = "ProgramDetails.aspx?ProgramCode=" + pCode;

                                sb.Append("<div class=\"col-12 col-md-6\">");
                                sb.AppendFormat(
                                    "<a class=\"card h-100 text-decoration-none\" href=\"{0}\">" +
                                    "<div class=\"card-body d-flex align-items-center gap-3\">" +
                                    "<span class=\"icon-container flex-shrink-0\" aria-hidden=\"true\">" +
                                    "<i class=\"hgi hgi-stroke hgi-diploma fs-3\"></i></span>" +
                                    "<h4 class=\"h6 mb-0\">{1}</h4></div></a>",
                                    url, HttpUtility.HtmlEncode(pName));
                                sb.Append("</div>");
                            }
                            sb.Append("</div>");
                        }
                    }

                    sb.Append("</section>");
                    index++;
                }
            }

            ltrSections.Text = sb.ToString();
        }

        private string GetCollegeCode()
        {
            try
            {
                if (Page.Request.QueryString["Source"] != null)
                    return Page.Request.QueryString["Source"].ToString().Trim();

                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList("AboutCollege");
                    if (list != null)
                    {
                        SPListItemCollection collitem = list.GetItems();
                        if (collitem != null && collitem.Count > 0 && collitem[0]["College_Code"] != null)
                            return collitem[0]["College_Code"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucCollegeSectionsDga.GetCollegeCode", ex.Message);
            }
            return "";
        }
    }

}
