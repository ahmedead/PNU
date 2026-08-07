using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers
{
    public partial class ucFacultyActivationReport : UserControl
    {
        // ── Configuration ────────────────────────────────────────────────
        // Adjust these to your actual list / internal field names.


        private const string MembersListName = "CollMembersListName";
        private const string ResumesListName = "MembersResumes";
        private const string TweetsListName = "MemberTweets";
        private const string LibraryHoursListName = "LibraryHours";
        private const string ScholarArticlesListName = "GoogleScholarArticles";
        private const string OrcidArticlesListName = "ORCIDArticles";

        private const string CollegeFieldName = "COLLEGE";          // <== college field on the members list
        private const string NameFieldAr = "FULL_NAME";
        private const string NameFieldEn = "ENGLISH_NAME";
        private const string SectionField = "SECTION";
        private const string EmailField = "EMAIL_ADDRESS";

        private const int PageSize = 2000;                          // paged CAML to stay under the list view threshold

        private bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        // ── Data model ───────────────────────────────────────────────────
        [Serializable]
        public class MemberRow
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string College { get; set; }
            public string Section { get; set; }
            public bool HasResume { get; set; }
            public string ScholarId { get; set; }
            public string Orcid { get; set; }

            public string HasResumeText { get; set; }
            public string ScholarText { get { return String.IsNullOrEmpty(ScholarId) ? "-" : ScholarId; } }
            public string OrcidText { get { return String.IsNullOrEmpty(Orcid) ? "-" : Orcid; } }
        }

        public class CollegeSummary
        {
            public string College { get; set; }
            public int Total { get; set; }
            public int Activated { get; set; }
            public int ScholarCount { get; set; }
            public int OrcidCount { get; set; }
            public string Pct { get { return Total == 0 ? "0" : Math.Round(Activated * 100.0 / Total, 1).ToString(); } }
        }

        /// <summary>Describes one list that should become its own worksheet in the full export.</summary>
        private class ExportListDef
        {
            public string ListName;
            public string SheetNameAr;
            public string SheetNameEn;
            /// <summary>Preferred internal name of the field that holds the member e-mail. Null = auto-detect.</summary>
            public string LinkField;

            public ExportListDef(string listName, string sheetAr, string sheetEn, string linkField)
            {
                ListName = listName; SheetNameAr = sheetAr; SheetNameEn = sheetEn; LinkField = linkField;
            }
        }

        /// <summary>Internal names we try, in order, when a list does not declare its link field explicitly.</summary>
        private static readonly string[] LinkFieldCandidates = new string[]
        {
            "EMAIL_ADDRESS", "Email", "EmailAddress", "Email_x0020_Address",
            "MemberEmail", "Member_x0020_Email", "UserEmail", "User_x0020_Email",
            "OwnerEmail", "Mail", "EMAIL"
        };

        /// <summary>Columns that carry no business value in an export.</summary>
        private static readonly HashSet<string> SkippedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ContentType", "ContentTypeId", "Attachments", "Edit", "DocIcon", "LinkTitle", "LinkTitleNoMenu",
            "LinkTitle2", "_UIVersionString", "_UIVersion", "ItemChildCount", "FolderChildCount",
            "AppAuthor", "AppEditor", "ComplianceAssetId", "_ComplianceFlags", "_ComplianceTag",
            "_ComplianceTagWrittenTime", "_ComplianceTagUserId", "SelectTitle", "InstanceID", "Order",
            "GUID", "WorkflowVersion", "WorkflowInstanceID", "FileRef", "FileDirRef", "ServerUrl",
            "EncodedAbsUrl", "BaseName", "MetaInfo", "_Level", "_IsCurrentVersion", "ScopeId",
            "UniqueId", "ProgId", "SyncClientId", "FileLeafRef", "SortBehavior", "PermMask"
        };

        private List<ExportListDef> GetExportLists()
        {
            return new List<ExportListDef>
            {
                new ExportListDef(MembersListName,          "بيانات الأعضاء",      "Members",            EmailField),
                new ExportListDef(ResumesListName,          "السير الذاتية",       "Resumes",            "Email"),
                new ExportListDef(TweetsListName,           "التغريدات",           "Tweets",             null),
                new ExportListDef(LibraryHoursListName,     "ساعات المكتبة",       "Library Hours",      null),
                new ExportListDef(ScholarArticlesListName,  "أبحاث Google Scholar","Scholar Articles",   null),
                new ExportListDef(OrcidArticlesListName,    "أبحاث ORCID",         "ORCID Articles",     null)
            };
        }

        // Drill-down selection kept across postbacks via hidden fields
        // (works even when ViewState is disabled on the page/zone)
        private string SelectedCollege
        {
            get { return hfCollege.Value ?? ""; }
            set { hfCollege.Value = value; }
        }
        private string SelectedMetric
        {
            get { return hfMetric.Value ?? ""; }
            set { hfMetric.Value = value; }
        }

        // ── Page lifecycle ───────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();

                // Bind on EVERY load (not just !IsPostBack): if ViewState is disabled
                // on the page or web part zone, the repeater would otherwise be empty
                // on postback and LinkButton clicks would be silently dropped.
                BindSummary();

                // Restore details after a postback (e.g. before export click fires)
                if (IsPostBack && !String.IsNullOrEmpty(SelectedCollege))
                    BindDetails();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            bool ar = IsArabic;
            ltrSummaryTitle.Text = ar ? "نسبة تفعيل مواقع أعضاء هيئة التدريس" : "Faculty Member Sites Activation";
            ltrHdrCollege.Text = ar ? "الكلية" : "College";
            ltrHdrTotal.Text = ar ? "إجمالي الأعضاء" : "Total Members";
            ltrHdrActivated.Text = ar ? "المفعّلون" : "Activated";
            ltrHdrPct.Text = ar ? "نسبة التفعيل" : "Activation %";
            ltrHdrGrandTotal.Text = ar ? "الإجمالي" : "Grand Total";
            ltrHdrName.Text = ar ? "الاسم" : "Name";
            ltrHdrEmail.Text = ar ? "البريد الإلكتروني" : "Email";
            ltrHdrSection.Text = ar ? "القسم" : "Section";
            ltrHdrHasResume.Text = ar ? "مفعّل" : "Activated";
            btnExportSummary.Text = ar ? "تصدير إلى Excel" : "Export to Excel";
            btnExportDetails.Text = ar ? "تصدير الجدول" : "Export Table";
            btnExportAll.Text = ar ? "تصدير كل البيانات" : "Export All Data";
            btnExportAll.ToolTip = ar
                ? "تصدير جميع بيانات الأعضاء المحددين من كافة القوائم، كل قائمة في ورقة مستقلة"
                : "Export every list for the selected members, one worksheet per list";
        }

        // ── Data loading ─────────────────────────────────────────────────
        private List<MemberRow> _dataCache;

        private List<MemberRow> LoadAllData()
        {
            if (_dataCache != null) return _dataCache;
            List<MemberRow> rows = new List<MemberRow>();
            try
            {
                bool ar = IsArabic;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            // 1) Resumes indexed by email
                            Dictionary<string, SPListItem> resumes = new Dictionary<string, SPListItem>();
                            SPList resumeList = web.Lists.TryGetList(ResumesListName);
                            if (resumeList != null)
                            {
                                SPQuery rq = new SPQuery();
                                rq.ViewFields = "<FieldRef Name='Email'/><FieldRef Name='GoogleScolarID'/><FieldRef Name='ORCID'/>";
                                rq.ViewFieldsOnly = true;
                                rq.RowLimit = 5000;
                                foreach (SPListItem it in resumeList.GetItems(rq))
                                {
                                    string em = Convert.ToString(it["Email"]);
                                    if (String.IsNullOrEmpty(em)) continue;
                                    em = em.ToLower().Trim();
                                    if (!resumes.ContainsKey(em))
                                        resumes.Add(em, it);
                                }
                            }

                            // 2) All members
                            SPList membersList = web.Lists.TryGetList(MembersListName);
                            if (membersList == null) return;

                            SPQuery mq = new SPQuery();
                            mq.RowLimit = 10000;
                            foreach (SPListItem it in membersList.GetItems(mq))
                            {
                                MemberRow row = new MemberRow();

                                string nameAr = membersList.Fields.ContainsField(NameFieldAr) ? Convert.ToString(it[NameFieldAr]) : "";
                                string nameEn = membersList.Fields.ContainsField(NameFieldEn) ? Convert.ToString(it[NameFieldEn]) : "";
                                row.Name = ar ? (!String.IsNullOrEmpty(nameAr) ? nameAr : nameEn)
                                              : (!String.IsNullOrEmpty(nameEn) ? nameEn : nameAr);

                                row.Email = membersList.Fields.ContainsField(EmailField) ? Convert.ToString(it[EmailField]) : "";
                                row.Email = row.Email == null ? "" : row.Email.ToLower().Trim();

                                row.College = membersList.Fields.ContainsField(CollegeFieldName) ? Convert.ToString(it[CollegeFieldName]) : "";
                                if (String.IsNullOrEmpty(row.College))
                                    row.College = ar ? "غير محدد" : "Not specified";

                                row.Section = membersList.Fields.ContainsField(SectionField) ? Convert.ToString(it[SectionField]) : "";

                                if (!String.IsNullOrEmpty(row.Email) && resumes.ContainsKey(row.Email))
                                {
                                    SPListItem res = resumes[row.Email];
                                    row.HasResume = true;
                                    row.ScholarId = resumeList.Fields.ContainsField("GoogleScolarID") ? Convert.ToString(res["GoogleScolarID"]) : "";
                                    row.Orcid = resumeList.Fields.ContainsField("ORCID") ? Convert.ToString(res["ORCID"]) : "";
                                    if (row.ScholarId != null) row.ScholarId = row.ScholarId.Trim();
                                    if (row.Orcid != null) row.Orcid = row.Orcid.Trim();
                                }

                                row.HasResumeText = row.HasResume ? (ar ? "مفعّل" : "Yes") : (ar ? "غير مفعّل" : "No");

                                rows.Add(row);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucFacultyActivationReport - LoadAllData", ex.Message);
            }
            _dataCache = rows;
            return rows;
        }

        private List<CollegeSummary> BuildSummary(List<MemberRow> rows)
        {
            return rows.GroupBy(r => r.College)
                       .Select(g => new CollegeSummary
                       {
                           College = g.Key,
                           Total = g.Count(),
                           Activated = g.Count(r => r.HasResume),
                           ScholarCount = g.Count(r => !String.IsNullOrEmpty(r.ScholarId)),
                           OrcidCount = g.Count(r => !String.IsNullOrEmpty(r.Orcid))
                       })
                       .OrderBy(s => s.College)
                       .ToList();
        }

        private void BindSummary()
        {
            List<MemberRow> rows = LoadAllData();
            List<CollegeSummary> summary = BuildSummary(rows);

            rptSummary.DataSource = summary;
            rptSummary.DataBind();

            int total = summary.Sum(s => s.Total);
            int activated = summary.Sum(s => s.Activated);
            ltrGrandTotal.Text = total.ToString();
            ltrGrandActivated.Text = activated.ToString();
            ltrGrandPct.Text = total == 0 ? "0" : Math.Round(activated * 100.0 / total, 1).ToString();
            ltrGrandScholar.Text = summary.Sum(s => s.ScholarCount).ToString();
            ltrGrandOrcid.Text = summary.Sum(s => s.OrcidCount).ToString();
        }

        // ── Drill-down ───────────────────────────────────────────────────
        protected void rptSummary_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                SelectedMetric = e.CommandName;
                SelectedCollege = Convert.ToString(e.CommandArgument);
                BindDetails();

                // Scroll down to the details section after the postback
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollDetails",
                    "var p=document.getElementById('" + pnlDetails.ClientID + "'); if(p){p.scrollIntoView({behavior:'smooth'});}", true);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private List<MemberRow> GetDetailRows()
        {
            List<MemberRow> rows = LoadAllData()
                .Where(r => r.College == SelectedCollege).ToList();

            switch (SelectedMetric)
            {
                case "Activated": return rows.Where(r => r.HasResume).ToList();
                case "Scholar": return rows.Where(r => !String.IsNullOrEmpty(r.ScholarId)).ToList();
                case "Orcid": return rows.Where(r => !String.IsNullOrEmpty(r.Orcid)).ToList();
                default: return rows; // Total
            }
        }

        private void BindDetails()
        {
            bool ar = IsArabic;
            List<MemberRow> rows = GetDetailRows();

            string metricTitle;
            switch (SelectedMetric)
            {
                case "Activated": metricTitle = ar ? "الأعضاء المفعّلون" : "Activated members"; break;
                case "Scholar": metricTitle = ar ? "الأعضاء المسجلون في Google Scholar" : "Members with Google Scholar"; break;
                case "Orcid": metricTitle = ar ? "الأعضاء المسجلون في ORCID" : "Members with ORCID"; break;
                default: metricTitle = ar ? "جميع الأعضاء" : "All members"; break;
            }
            ltrDetailsTitle.Text = metricTitle + " - " + SelectedCollege + " (" + rows.Count + ")";

            rptDetails.DataSource = rows;
            rptDetails.DataBind();
            pnlDetails.Visible = true;
        }

        // ── Excel export: summary ────────────────────────────────────────
        protected void btnExportSummary_Click(object sender, EventArgs e)
        {
            try
            {
                bool ar = IsArabic;
                List<CollegeSummary> summary = BuildSummary(LoadAllData());

                DataTable dt = new DataTable();
                dt.Columns.Add(ar ? "الكلية" : "College");
                dt.Columns.Add(ar ? "إجمالي الأعضاء" : "Total Members");
                dt.Columns.Add(ar ? "المفعّلون" : "Activated");
                dt.Columns.Add(ar ? "نسبة التفعيل %" : "Activation %");
                dt.Columns.Add("Google Scholar");
                dt.Columns.Add("ORCID");

                foreach (CollegeSummary s in summary)
                    dt.Rows.Add(s.College, s.Total, s.Activated, s.Pct, s.ScholarCount, s.OrcidCount);

                dt.Rows.Add(ar ? "الإجمالي" : "Grand Total",
                    summary.Sum(x => x.Total), summary.Sum(x => x.Activated),
                    summary.Sum(x => x.Total) == 0 ? "0" : Math.Round(summary.Sum(x => x.Activated) * 100.0 / summary.Sum(x => x.Total), 1).ToString(),
                    summary.Sum(x => x.ScholarCount), summary.Sum(x => x.OrcidCount));

                ExportDataTable(dt, ar ? "نسبة_تفعيل_مواقع_الأعضاء" : "FacultyActivationSummary");
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ── Excel export: the on-screen details grid (single sheet) ──────
        protected void btnExportDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(SelectedCollege)) return;
                bool ar = IsArabic;
                List<MemberRow> rows = GetDetailRows();

                DataTable dt = new DataTable();
                dt.Columns.Add(ar ? "الاسم" : "Name");
                dt.Columns.Add(ar ? "البريد الإلكتروني" : "Email");
                dt.Columns.Add(ar ? "الكلية" : "College");
                dt.Columns.Add(ar ? "القسم" : "Section");
                dt.Columns.Add(ar ? "مفعّل" : "Activated");
                dt.Columns.Add("Google Scholar");
                dt.Columns.Add("ORCID");

                foreach (MemberRow r in rows)
                    dt.Rows.Add(r.Name, r.Email, r.College, r.Section, r.HasResumeText, r.ScholarText, r.OrcidText);

                ExportDataTable(dt, (ar ? "تفاصيل_" : "Details_") + SelectedMetric);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ── Excel export: EVERYTHING, one worksheet per list ─────────────
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(SelectedCollege)) return;
                bool ar = IsArabic;

                // The members currently shown in the drill-down grid drive the whole export.
                HashSet<string> emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (MemberRow r in GetDetailRows())
                    if (!String.IsNullOrEmpty(r.Email)) emails.Add(r.Email.ToLower().Trim());

                List<DataTable> sheets = new List<DataTable>();

                // Sheet 0: the report view itself, so the workbook is readable on its own.
                sheets.Add(BuildOverviewSheet());

                if (emails.Count > 0)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                foreach (ExportListDef def in GetExportLists())
                                {
                                    DataTable dt = BuildListSheet(web, def, emails, ar);
                                    if (dt != null) sheets.Add(dt);
                                }
                            }
                        }
                    });
                }

                string file = (ar ? "بيانات_الأعضاء_" : "MembersFullData_") + CleanFileName(SelectedCollege);
                ExportWorkbook(sheets, file);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "ucFacultyActivationReport - btnExportAll_Click", ex.Message);
            }
        }

        /// <summary>The drill-down grid, reproduced as the first worksheet.</summary>
        private DataTable BuildOverviewSheet()
        {
            bool ar = IsArabic;
            DataTable dt = new DataTable(ar ? "ملخص" : "Overview");
            dt.Columns.Add(ar ? "الاسم" : "Name");
            dt.Columns.Add(ar ? "البريد الإلكتروني" : "Email");
            dt.Columns.Add(ar ? "الكلية" : "College");
            dt.Columns.Add(ar ? "القسم" : "Section");
            dt.Columns.Add(ar ? "مفعّل" : "Activated");
            dt.Columns.Add("Google Scholar");
            dt.Columns.Add("ORCID");

            foreach (MemberRow r in GetDetailRows())
                dt.Rows.Add(r.Name, r.Email, r.College, r.Section, r.HasResumeText, r.ScholarText, r.OrcidText);

            return dt;
        }

        /// <summary>
        /// Reads every visible column of <paramref name="def"/>'s list, keeping only the rows whose
        /// link field matches one of the selected members' e-mails.
        /// </summary>
        private DataTable BuildListSheet(SPWeb web, ExportListDef def, HashSet<string> emails, bool ar)
        {
            string sheetName = ar ? def.SheetNameAr : def.SheetNameEn;
            DataTable dt = new DataTable(sheetName);

            SPList list = web.Lists.TryGetList(def.ListName);
            if (list == null)
            {
                dt.Columns.Add(ar ? "ملاحظة" : "Note");
                dt.Rows.Add((ar ? "القائمة غير موجودة: " : "List not found: ") + def.ListName);
                return dt;
            }

            string linkField = ResolveLinkField(list, def.LinkField);
            if (linkField == null)
            {
                dt.Columns.Add(ar ? "ملاحظة" : "Note");
                dt.Rows.Add((ar ? "تعذّر تحديد حقل البريد الإلكتروني في القائمة: " : "No e-mail link field found on list: ") + def.ListName);
                return dt;
            }

            // Columns = every meaningful field on the list, in list order.
            List<SPField> fields = new List<SPField>();
            foreach (SPField f in list.Fields)
            {
                if (!IncludeField(f)) continue;
                fields.Add(f);
                string col = f.Title;
                if (String.IsNullOrEmpty(col)) col = f.InternalName;
                // DataTable rejects duplicate column names; SharePoint does not.
                string unique = col; int n = 2;
                while (dt.Columns.Contains(unique)) { unique = col + " (" + n + ")"; n++; }
                dt.Columns.Add(unique);
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add(ar ? "ملاحظة" : "Note");
                dt.Rows.Add(ar ? "لا توجد أعمدة قابلة للتصدير" : "No exportable columns");
                return dt;
            }

            // Paged read — avoids the 5000-item list view threshold on large lists.
            SPQuery q = new SPQuery();
            q.RowLimit = PageSize;
            do
            {
                SPListItemCollection items = list.GetItems(q);
                foreach (SPListItem it in items)
                {
                    string key = SafeText(it, linkField);
                    if (String.IsNullOrEmpty(key)) continue;
                    if (!emails.Contains(key.ToLower().Trim())) continue;

                    object[] values = new object[fields.Count];
                    for (int i = 0; i < fields.Count; i++)
                        values[i] = GetFieldText(it, fields[i]);
                    dt.Rows.Add(values);
                }
                q.ListItemCollectionPosition = items.ListItemCollectionPosition;
            }
            while (q.ListItemCollectionPosition != null);

            return dt;
        }

        /// <summary>Returns the internal name of the field linking a list row to a member, or null.</summary>
        private string ResolveLinkField(SPList list, string preferred)
        {
            if (!String.IsNullOrEmpty(preferred) && list.Fields.ContainsField(preferred))
                return preferred;

            foreach (string candidate in LinkFieldCandidates)
                if (list.Fields.ContainsField(candidate))
                    return candidate;

            // Last resort: any field whose internal or display name mentions "mail".
            foreach (SPField f in list.Fields)
            {
                if (f.Hidden) continue;
                if (f.InternalName.IndexOf("mail", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (f.Title != null && f.Title.IndexOf("mail", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (f.Title != null && f.Title.IndexOf("بريد", StringComparison.OrdinalIgnoreCase) >= 0))
                    return f.InternalName;
            }
            return null;
        }

        private bool IncludeField(SPField f)
        {
            if (f == null || f.Hidden) return false;
            if (SkippedFields.Contains(f.InternalName)) return false;
            if (f.Type == SPFieldType.Attachments) return false;
            if (f.Type == SPFieldType.Computed) return false;
            if (f.Type == SPFieldType.Invalid) return false;
            return true;
        }

        private string SafeText(SPListItem item, string internalName)
        {
            try
            {
                object v = item[internalName];
                return v == null ? "" : Convert.ToString(v);
            }
            catch { return ""; }
        }

        /// <summary>Renders any SharePoint field value as clean, Excel-friendly text.</summary>
        private string GetFieldText(SPListItem item, SPField f)
        {
            try
            {
                object v = item[f.Id];
                if (v == null) return "";

                if (v is DateTime)
                    return ((DateTime)v).ToString("yyyy-MM-dd HH:mm");

                if (f.Type == SPFieldType.User || f.Type == SPFieldType.Lookup)
                {
                    string raw = Convert.ToString(v);
                    // "1;#Ahmed" or "1;#Ahmed;#2;#Sara" -> "Ahmed, Sara"
                    string[] parts = raw.Split(new string[] { ";#" }, StringSplitOptions.None);
                    List<string> names = new List<string>();
                    for (int i = 1; i < parts.Length; i += 2)
                        if (!String.IsNullOrEmpty(parts[i])) names.Add(parts[i]);
                    if (names.Count > 0) return String.Join(", ", names.ToArray());
                    return raw;
                }

                if (f.Type == SPFieldType.URL)
                {
                    SPFieldUrlValue url = new SPFieldUrlValue(Convert.ToString(v));
                    return String.IsNullOrEmpty(url.Description) ? url.Url : url.Url + " (" + url.Description + ")";
                }

                if (f.Type == SPFieldType.Boolean)
                    return Convert.ToBoolean(v) ? (IsArabic ? "نعم" : "Yes") : (IsArabic ? "لا" : "No");

                string text = f.GetFieldValueAsText(v);
                if (text == null) text = Convert.ToString(v);

                if (f.Type == SPFieldType.Note)
                    text = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", " ");

                return HttpUtility.HtmlDecode(text).Replace("\r", " ").Trim();
            }
            catch
            {
                try { return Convert.ToString(item[f.Id]); }
                catch { return ""; }
            }
        }

        // ── Writers ──────────────────────────────────────────────────────

        /// <summary>Single-sheet export (kept as HTML-table .xls for backwards compatibility).</summary>
        private void ExportDataTable(DataTable dt, string fileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            response.BinaryWrite(Encoding.UTF8.GetPreamble()); // BOM so Arabic renders correctly in Excel

            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/></head>");
            sb.Append(IsArabic ? "<body dir=\"rtl\">" : "<body>");
            sb.Append("<table border=\"1\"><tr>");
            foreach (DataColumn col in dt.Columns)
                sb.Append("<th style=\"background:#0D6E4F;color:#fff;\">" + HttpUtility.HtmlEncode(col.ColumnName) + "</th>");
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (object cell in row.ItemArray)
                    sb.Append("<td>" + HttpUtility.HtmlEncode(Convert.ToString(cell)) + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table></body></html>");

            response.Write(sb.ToString());
            try { response.End(); }
            catch (System.Threading.ThreadAbortException) { /* expected on Response.End */ }
        }

        /// <summary>
        /// Multi-sheet export using the SpreadsheetML 2003 format — no third-party
        /// assembly required, opens natively in Excel with one tab per DataTable.
        /// </summary>
        private void ExportWorkbook(List<DataTable> tables, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
            sb.Append("<?mso-application progid=\"Excel.Sheet\"?>\r\n");
            sb.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
            sb.Append("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
            sb.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
            sb.Append("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
            sb.Append("xmlns:html=\"http://www.w3.org/TR/REC-html40\">");

            sb.Append("<Styles>");
            sb.Append("<Style ss:ID=\"hdr\">");
            sb.Append("<Font ss:Bold=\"1\" ss:Color=\"#FFFFFF\"/>");
            sb.Append("<Interior ss:Color=\"#0D6E4F\" ss:Pattern=\"Solid\"/>");
            sb.Append("<Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"" + (IsArabic ? " ss:ReadingOrder=\"RightToLeft\"" : "") + "/>");
            sb.Append("<Borders>");
            sb.Append("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
            sb.Append("</Borders>");
            sb.Append("</Style>");
            sb.Append("<Style ss:ID=\"cell\">");
            sb.Append("<Alignment ss:Vertical=\"Top\" ss:WrapText=\"1\"" + (IsArabic ? " ss:ReadingOrder=\"RightToLeft\"" : "") + "/>");
            sb.Append("</Style>");
            sb.Append("</Styles>");

            HashSet<string> usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataTable dt in tables)
            {
                if (dt == null) continue;
                string name = MakeSheetName(dt.TableName, usedNames);

                sb.Append("<Worksheet ss:Name=\"" + XmlEscape(name) + "\">");
                sb.Append("<Table>");

                foreach (DataColumn c in dt.Columns)
                    sb.Append("<Column ss:AutoFitWidth=\"1\" ss:Width=\"130\"/>");

                // Header
                sb.Append("<Row ss:Height=\"22\">");
                foreach (DataColumn c in dt.Columns)
                    sb.Append("<Cell ss:StyleID=\"hdr\"><Data ss:Type=\"String\">" + XmlEscape(c.ColumnName) + "</Data></Cell>");
                sb.Append("</Row>");

                // Body
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("<Row>");
                    foreach (DataColumn c in dt.Columns)
                    {
                        string val = Convert.ToString(r[c]);
                        if (val == null) val = "";
                        if (IsExcelNumber(val))
                            sb.Append("<Cell ss:StyleID=\"cell\"><Data ss:Type=\"Number\">" + val + "</Data></Cell>");
                        else
                            sb.Append("<Cell ss:StyleID=\"cell\"><Data ss:Type=\"String\">" + XmlEscape(val) + "</Data></Cell>");
                    }
                    sb.Append("</Row>");
                }

                sb.Append("</Table>");
                sb.Append("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                if (IsArabic) sb.Append("<DisplayRightToLeft/>");
                sb.Append("<FreezePanes/><FrozenNoSplit/><SplitHorizontal>1</SplitHorizontal><TopRowBottomPane>1</TopRowBottomPane><ActivePane>2</ActivePane>");
                sb.Append("</WorksheetOptions>");
                sb.Append("</Worksheet>");
            }

            sb.Append("</Workbook>");

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            response.Write(sb.ToString());
            try { response.End(); }
            catch (System.Threading.ThreadAbortException) { /* expected on Response.End */ }
        }

        private static bool IsExcelNumber(string s)
        {
            if (String.IsNullOrEmpty(s) || s.Length > 15) return false;
            if (s.Length > 1 && s[0] == '0') return false;          // keep leading zeros (IDs, codes)
            double d;
            return Double.TryParse(s, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out d);
        }

        private static string MakeSheetName(string raw, HashSet<string> used)
        {
            if (String.IsNullOrEmpty(raw)) raw = "Sheet";
            foreach (char bad in new char[] { '\\', '/', '?', '*', '[', ']', ':' })
                raw = raw.Replace(bad, ' ');
            raw = raw.Trim();
            if (raw.Length > 31) raw = raw.Substring(0, 31);

            string name = raw; int n = 2;
            while (used.Contains(name))
            {
                string suffix = " " + n;
                name = (raw.Length + suffix.Length > 31 ? raw.Substring(0, 31 - suffix.Length) : raw) + suffix;
                n++;
            }
            used.Add(name);
            return name;
        }

        private static string CleanFileName(string s)
        {
            if (String.IsNullOrEmpty(s)) return "Export";
            foreach (char bad in System.IO.Path.GetInvalidFileNameChars())
                s = s.Replace(bad, '_');
            return s.Replace(' ', '_');
        }

        private static string XmlEscape(string s)
        {
            if (String.IsNullOrEmpty(s)) return "";
            StringBuilder sb = new StringBuilder(s.Length + 16);
            foreach (char c in s)
            {
                switch (c)
                {
                    case '&': sb.Append("&amp;"); break;
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '"': sb.Append("&quot;"); break;
                    case '\'': sb.Append("&apos;"); break;
                    case '\n': sb.Append("&#10;"); break;
                    default:
                        // strip control characters Excel's XML parser rejects
                        if (c < 0x20 && c != '\t') break;
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}