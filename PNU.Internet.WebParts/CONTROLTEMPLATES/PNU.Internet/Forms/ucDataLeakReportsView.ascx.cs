using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms
{
    public partial class ucDataLeakReportsView : UserControl
    {
        private const string LIST_NAME = "DataLeakReports";
        private const string ADMIN_LIST_NAME = "DataAdminUsers";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Step 1: If user is not logged in (anonymous), redirect to SharePoint's login page
            if (!IsUserAuthenticated())
            {
                RedirectToLogin();
                return;
            }

            // Step 2: User is logged in — check if they're authorized to view reports
            if (!IsCurrentUserAuthorized())
            {
                pnlUnauthorized.Visible = true;
                pnlAuthorized.Visible = false;
                return;
            }

            pnlUnauthorized.Visible = false;
            pnlAuthorized.Visible = true;

            if (!IsPostBack)
            {
                LoadReports(string.Empty);
            }
        }

        /// <summary>
        /// Checks if the current user is authenticated (not anonymous).
        /// </summary>
        private bool IsUserAuthenticated()
        {
            try
            {
                // HttpContext.Current.User.Identity.IsAuthenticated is the most reliable check
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                    return false;

                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return false;

                // SharePoint-specific check — anonymous users may still have a CurrentUser=null
                if (SPContext.Current == null || SPContext.Current.Web == null)
                    return false;

                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                return currentUser != null && !string.IsNullOrEmpty(currentUser.LoginName);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Redirects the user to the SharePoint login page, preserving the current URL as the return target.
        /// </summary>
        private void RedirectToLogin()
        {
            try
            {
                string returnUrl = HttpContext.Current.Request.RawUrl;
                string loginUrl = $"{SPContext.Current.Web.Url}/_layouts/15/Authenticate.aspx?Source={HttpUtility.UrlEncode(returnUrl)}";

                HttpContext.Current.Response.Redirect(loginUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                pnlAuthorized.Visible = true;
                pnlNoData.Visible = true;
                pnlNoData.Controls.Clear();
                pnlNoData.Controls.Add(new LiteralControl("<p style='color:red;'>تعذر إعادة التوجيه إلى صفحة تسجيل الدخول: " + ex.Message + "</p>"));
            }
        }
        #region Authorization

        /// <summary>
        /// Checks whether the current user is in the DataAdminUsers list.
        /// Matches against the AdminUser (SPFieldUser) field.
        /// </summary>
        private bool IsCurrentUserAuthorized()
        {
            bool authorized = false;

            try
            {
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                if (currentUser == null) return false;

                // Site collection administrators always allowed
                if (currentUser.IsSiteAdmin) return true;

                int currentUserId = currentUser.ID;
                string currentLogin = currentUser.LoginName;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList adminList = web.Lists.Cast<SPList>().FirstOrDefault(l => l.Title == ADMIN_LIST_NAME);
                            if (adminList == null) return;

                            SPQuery query = new SPQuery();
                            query.Query = $@"<Where>
                                                <Or>
                                                    <Eq>
                                                        <FieldRef Name='AdminUser' LookupId='TRUE' />
                                                        <Value Type='Integer'>{currentUserId}</Value>
                                                    </Eq>
                                                    <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{currentLogin}</Value>
                                                    </Eq>
                                                </Or>
                                             </Where>";
                            query.RowLimit = 1;

                            SPListItemCollection items = adminList.GetItems(query);
                            if (items != null && items.Count > 0)
                                authorized = true;
                        }
                    }
                });
            }
            catch
            {
                authorized = false;
            }

            return authorized;
        }

        #endregion

        #region Load Reports

        private void LoadReports(string searchTerm)
        {
            try
            {
                DataTable dt = BuildReportsTable();

                // Use the absolute URL from SPContext, but ensure fresh open
                string webUrl = SPContext.Current.Web.Url;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(webUrl))
                    {
                        // Force fresh read - bypass any cached web
                        using (SPWeb web = site.OpenWeb())
                        {
                            // Get list by title with a fresh reference
                            SPList list = web.Lists.TryGetList(LIST_NAME);
                            if (list == null) return;

                            // CRITICAL: This forces SharePoint to re-read from the content database
                            // instead of returning a cached snapshot
                            SPListItemCollectionPosition position = null;
                            SPQuery query = new SPQuery();
                            query.Query = "<OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";
                            query.ListItemCollectionPosition = position;

                            if (!string.IsNullOrWhiteSpace(searchTerm))
                            {
                                string esc = SecurityElement.Escape(searchTerm.Trim());
                                query.Query = $@"<Where>
                                            <Or>
                                                <Or>
                                                    <Contains><FieldRef Name='Title' /><Value Type='Text'>{esc}</Value></Contains>
                                                    <Contains><FieldRef Name='FullName' /><Value Type='Text'>{esc}</Value></Contains>
                                                </Or>
                                                <Contains><FieldRef Name='Email' /><Value Type='Text'>{esc}</Value></Contains>
                                            </Or>
                                         </Where>
                                         <OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";
                            }

                            SPListItemCollection items = list.GetItems(query);
                            foreach (SPListItem item in items)
                            {
                                DataRow row = dt.NewRow();
                                row["ID"] = item.ID;
                                row["Title"] = SafeStr(item["Title"]);
                                row["Created"] = item["Created"] != null ? Convert.ToDateTime(item["Created"]) : (object)DBNull.Value;
                                row["FullName"] = SafeStr(item["FullName"]);
                                row["Email"] = SafeStr(item["Email"]);
                                row["Mobile"] = SafeStr(item["Mobile"]);
                                row["Capacity"] = SafeStr(item["Capacity"]);
                                // FIX BUG 4: properly fetch the discovery date here
                                //row["DiscoveryDate"] = FormatDate(item["DiscoveryDate"]);
                                row["DiscoveryDate"] = SafeStr(item["DiscoveryDate"]);
                                dt.Rows.Add(row);
                            }
                        }
                    }
                });

                rptReports.DataSource = dt;
                rptReports.DataBind();
                litTotalCount.Text = dt.Rows.Count.ToString();
                pnlNoData.Visible = dt.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                pnlNoData.Visible = true;
                pnlNoData.Controls.Clear();
                pnlNoData.Controls.Add(new LiteralControl("<p style='color:red;'>خطأ: " + ex.Message + "</p>"));
            }
        }
        private DataTable BuildReportsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Created", typeof(DateTime));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Mobile", typeof(string));
            dt.Columns.Add("Capacity", typeof(string));
            dt.Columns.Add("DiscoveryDate", typeof(string));
            return dt;
        }

        private string SafeStr(object val)
        {
            return val == null ? "" : val.ToString();
        }

        private string FormatDate(object val)
        {
            if (val == null) return "";
            DateTime d;
            if (DateTime.TryParse(val.ToString(), out d))
                return d.ToString("yyyy-MM-dd");
            return val.ToString();
        }

        #endregion

        #region Search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadReports(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadReports(string.Empty);
        }

        #endregion

        #region Detail Modal

        protected void rptReports_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int itemId;
                if (int.TryParse(e.CommandArgument.ToString(), out itemId))
                {
                    LoadReportDetails(itemId);
                }
            }
        }

        private void LoadReportDetails(int itemId)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists[LIST_NAME];
                            SPListItem item = list.GetItemById(itemId);
                            if (item == null) return;

                            litRefNo.Text = SafeStr(item["Title"]);
                            litCreated.Text = item["Created"] != null
                                ? Convert.ToDateTime(item["Created"]).ToString("yyyy-MM-dd HH:mm")
                                : "";

                            litFullName.Text = SafeStr(item["FullName"]);
                            litEmail.Text = SafeStr(item["Email"]);
                            litMobile.Text = SafeStr(item["Mobile"]);

                            string capacity = SafeStr(item["Capacity"]);
                            string capacityOther = SafeStr(item["CapacityOther"]);
                            if (capacity == "أخرى" && !string.IsNullOrWhiteSpace(capacityOther))
                                capacity = "أخرى: " + capacityOther;
                            litCapacity.Text = capacity;

                            //litDiscoveryDate.Text = FormatDate(item["DiscoveryDate"]);
                            litDiscoveryDate.Text = SafeStr(item["DiscoveryDate"]);
                            litStillActive.Text = SafeStr(item["StillActive"]);

                            litSourceType.Text = AppendOther(SafeStr(item["SourceType"]), SafeStr(item["SourceTypeOther"]));
                            litSourceName.Text = SafeStr(item["SourceName"]);
                            litDataType.Text = AppendOther(SafeStr(item["DataType"]), SafeStr(item["DataTypeOther"]));
                            litDataFormat.Text = AppendOther(SafeStr(item["DataFormat"]), SafeStr(item["DataFormatOther"]));
                            litAccessMethod.Text = SafeStr(item["AccessMethod"]);
                            litAffectedCount.Text = SafeStr(item["AffectedCount"]);
                            litDataBelongsTo.Text = SafeStr(item["DataBelongsTo"]);
                            litDescription.Text = HtmlEncodeMultiline(SafeStr(item["IncidentDescription"]));

                            // Attachments
                            litAttachments.Text = BuildAttachmentsHtml(item);
                        }
                    }
                });

                pnlDetailsModal.Visible = true;
            }
            catch (Exception ex)
            {
                pnlDetailsModal.Visible = false;
                pnlNoData.Controls.Add(new LiteralControl("<p style='color:red;'>خطأ في تحميل التفاصيل: " + ex.Message + "</p>"));
            }
        }

        private string AppendOther(string main, string other)
        {
            if (!string.IsNullOrWhiteSpace(other))
                return string.IsNullOrWhiteSpace(main) ? other : main + " - " + other;
            return main;
        }

        private string HtmlEncodeMultiline(string text)
        {
            if (string.IsNullOrEmpty(text)) return "<em>لا يوجد</em>";
            return System.Web.HttpUtility.HtmlEncode(text).Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>");
        }

        private string BuildAttachmentsHtml(SPListItem item)
        {
            try
            {
                SPAttachmentCollection attachments = item.Attachments;
                if (attachments == null || attachments.Count == 0)
                    return "<em>لا توجد مرفقات</em>";

                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class='attachment-list'>");
                string urlPrefix = attachments.UrlPrefix;
                foreach (string fileName in attachments)
                {
                    string fullUrl = urlPrefix + fileName;
                    sb.Append($"<li><a href='{fullUrl}' target='_blank'>{System.Web.HttpUtility.HtmlEncode(fileName)}</a></li>");
                }
                sb.Append("</ul>");
                return sb.ToString();
            }
            catch
            {
                return "<em>تعذر تحميل قائمة المرفقات</em>";
            }
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            pnlDetailsModal.Visible = false;
        }

        #endregion
    }
}
