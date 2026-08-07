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
    /// Renders ONE department (قسم) - DGA design, EXISTING lists only.
    ///
    /// ucSideMenu loads one instance per department and passes the department code:
    ///     ucCollegeSectionDetails.ascx?SectionKey={DEPT_CODE}
    /// (SectionKey is set via the same reflection mechanism used by ucCollegeContentSection.)
    ///
    /// Data:
    ///   Admin web / AllFacultyDepartments  (DEPT_CODE = SectionKey) -> name + about
    ///   Admin web / AllDepartmentPrograms  (DEPT_CODE = SectionKey) -> program cards
    /// </summary>
    public partial class ucCollegeSectionDetails : UserControl
    {
        /// <summary>Department code, set by ucSideMenu from the ControlPath query.</summary>
        public string SectionKey { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack || ltrSectionDetails.Text.Length == 0)
                    RenderDepartment();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void RenderDepartment()
        {
            if (string.IsNullOrEmpty(SectionKey)) return;

            StringBuilder sb = new StringBuilder();

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb web = site.OpenWeb("Admin"))
            {
                SPList deptList = web.Lists.TryGetList("AllFacultyDepartments");
                if (deptList == null) return;
                SPList progList = web.Lists.TryGetList("AllDepartmentPrograms");

                // ---- the department itself ----
                SPQuery dq = new SPQuery
                {
                    Query = @"<Where>
                                 <Eq>
                                    <FieldRef Name='Code' />
                                    <Value Type='Text'>" + SectionKey + @"</Value>
                                 </Eq>
                              </Where>",
                    RowLimit = 1
                };

                SPListItemCollection depts = deptList.GetItems(dq);
                if (depts == null || depts.Count == 0) return;
                SPListItem dept = depts[0];

                string nameAr = ucSideMenu.GetFirstFieldValue(dept, "DEPT_NAME", "NAME", "Title");
                string nameEn = ucSideMenu.GetFirstFieldValue(dept, "DEPT_NAME_EN", "ENG_NAME", "NAME_EN", "Title_EN");
                string name = IsArabic
                    ? (!string.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                    : (!string.IsNullOrEmpty(nameEn) ? nameEn : nameAr);

                string about = IsArabic
                    ? ucSideMenu.GetFirstFieldValue(dept, "DEPT_ABOUT", "About", "Description")
                    : ucSideMenu.GetFirstFieldValue(dept, "DEPT_ABOUT_EN", "About_EN", "Description_EN",
                                                          "DEPT_ABOUT", "About", "Description");

                sb.AppendFormat("<section id=\"faculty-department-{0}\" class=\"pnu-section-anchor\">",
                    HttpUtility.HtmlAttributeEncode(SectionKey));
                sb.AppendFormat("<h2>{0}</h2>", HttpUtility.HtmlEncode(name));

                if (!string.IsNullOrEmpty(about))
                {
                    sb.AppendFormat("<h3 class=\"h5 mt-4\">{0}</h3>", IsArabic ? "عن القسم" : "About the Department");
                    sb.AppendFormat("<p>{0}</p>", HttpUtility.HtmlEncode(about));
                }

                // ---- the department programs ----
                if (progList != null)
                {
                    SPQuery pq = new SPQuery
                    {
                        Query = @"<Where>
                                     <Eq>
                                        <FieldRef Name='DEPT_CODE' />
                                        <Value Type='Text'>" + SectionKey + @"</Value>
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
                            //string url = PortalHelper.ParentLangSite +
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
            }

            ltrSectionDetails.Text = sb.ToString();
        }
    }

}
