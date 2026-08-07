using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Admin
{
    /// <summary>
    /// Generic add / edit / delete editor for ONE content list on ONE subweb.
    /// The hosting control sets WebUrl and ListName before this control loads.
    ///
    /// SECURITY - every data path re-checks all three of:
    ///   1. AdminSecurity.IsAuthorized()      - is the caller an approved admin
    ///   2. AdminSecurity.IsWebInScope(url)   - is the target web in this site collection
    ///   3. ContentSchema.FindList(name)      - is the list on the whitelist
    /// The checks are repeated on every postback, never cached in the form, because
    /// form values can be tampered with.
    ///
    /// ViewState is often disabled inside SharePoint page zones, so state lives in
    /// HiddenFields and the grid is re-bound on every Page_Load.
    /// </summary>
    public partial class ucListEditor : UserControl
    {
        public string WebUrl { get; set; }
        public string ListName { get; set; }

        private const string ModeGrid = "grid";
        private const string ModeEdit = "edit";

        public class RowDto
        {
            public int Id { get; set; }
            public List<string> Values { get; set; }
        }

        public class FieldValueDto
        {
            public string Name { get; set; }
            public string Label { get; set; }
            public string Hint { get; set; }
            public bool HasHint { get; set; }
            public string Value { get; set; }
            public bool IsNote { get; set; }
            public bool IsSingleLine { get; set; }
        }

        private ListDef Definition
        {
            get { return ContentSchema.FindList(ListName); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Guard()) { phEditor.Visible = false; return; }

                phEditor.Visible = true;
                ltrListTitle.Text = HttpUtility.HtmlEncode(Definition.Display + "  —  " + Definition.ListName);

                if (string.IsNullOrEmpty(hfMode.Value)) hfMode.Value = ModeGrid;

                Render();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucListEditor - Page_Load", ex.Message);
                ShowMessage("تعذر تحميل البيانات. تم تسجيل الخطأ.", true);
            }
        }

        /// <summary>All three security checks. Returns false and hides the UI when any fails.</summary>
        private bool Guard()
        {
            if (!AdminSecurity.IsAuthorized()) return false;
            if (!AdminSecurity.IsWebInScope(WebUrl)) return false;
            if (Definition == null) return false;
            return true;
        }

        private void Render()
        {
            bool editing = hfMode.Value == ModeEdit;
            phGrid.Visible = !editing;
            phForm.Visible = editing;

            if (editing) BindForm();
            else BindGrid();
        }

        // ---------------- grid ----------------

        private void BindGrid()
        {
            var cols = Definition.Fields.Where(f => f.ShowInGrid).ToList();
            if (cols.Count == 0) cols = Definition.Fields.Take(3).ToList();

            rptHeaders.DataSource = cols.Select(c => c.Label).ToList();
            rptHeaders.DataBind();

            var rows = new List<RowDto>();
            WithList(false, delegate (SPList list)
            {
                var items = new List<SPListItem>();
                foreach (SPListItem it in list.Items) items.Add(it);

                foreach (SPListItem it in items.OrderBy(x => SafeDouble(x, "SortOrder")))
                {
                    var vals = new List<string>();
                    foreach (FieldDef f in cols) vals.Add(Shorten(ReadField(it, f)));
                    rows.Add(new RowDto { Id = it.ID, Values = vals });
                }
            });

            ltrCount.Text = rows.Count.ToString();
            phEmpty.Visible = rows.Count == 0;
            phTable.Visible = rows.Count > 0;

            rptRows.DataSource = rows;
            rptRows.DataBind();
        }

        private static string Shorten(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("\r", " ").Replace("\n", " ").Trim();
            return s.Length <= 70 ? s : s.Substring(0, 70) + "…";
        }

        /// <summary>Binds the per-row cells (a row has a variable number of columns).</summary>
        protected void rptRows_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            RowDto row = e.Item.DataItem as RowDto;
            if (row == null) return;
            Repeater rptCells = e.Item.FindControl("rptCells") as Repeater;
            if (rptCells != null) { rptCells.DataSource = row.Values; rptCells.DataBind(); }
        }

        protected void rptRows_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (!Guard()) return;

                int id;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

                if (e.CommandName == "EditItem")
                {
                    hfMode.Value = ModeEdit;
                    hfItemId.Value = id.ToString();
                    Render();
                }
                else if (e.CommandName == "DeleteItem")
                {
                    DeleteItem(id);
                    hfMode.Value = ModeGrid;
                    hfItemId.Value = "";
                    Render();
                    ShowMessage("تم حذف السجل بنجاح.", false);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucListEditor - ItemCommand", ex.Message);
                ShowMessage("تعذر تنفيذ العملية. تم تسجيل الخطأ.", true);
            }
        }

        // ---------------- form ----------------

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            if (!Guard()) return;
            hfMode.Value = ModeEdit;
            hfItemId.Value = "";        // empty = new record
            Render();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            hfMode.Value = ModeGrid;
            hfItemId.Value = "";
            if (Guard()) Render();
        }

        private void BindForm()
        {
            int id = ParseId();
            var values = new Dictionary<string, string>();

            if (id > 0)
            {
                WithList(false, delegate (SPList list)
                {
                    SPListItem item = TryGetItem(list, id);
                    if (item != null)
                        foreach (FieldDef f in Definition.Fields)
                            values[f.Name] = ReadField(item, f);
                });
            }

            ltrFormMode.Text = id > 0 ? "تعديل سجل" : "إضافة سجل جديد";

            var dtos = Definition.Fields.Select(f => new FieldValueDto
            {
                Name = f.Name,
                Label = f.Label,
                Hint = f.Hint,
                HasHint = !string.IsNullOrEmpty(f.Hint),
                Value = values.ContainsKey(f.Name) ? values[f.Name] : "",
                IsNote = f.Kind == FieldKind.Note,
                IsSingleLine = f.Kind != FieldKind.Note
            }).ToList();

            rptFields.DataSource = dtos;
            rptFields.DataBind();
        }

        /// <summary>
        /// Configures each field's textbox. TextMode is an enum, so it is set here
        /// rather than databound in the .ascx: databinding an enum property emits a
        /// direct cast and fails to compile (CS0030).
        /// </summary>
        protected void rptFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            FieldValueDto row = e.Item.DataItem as FieldValueDto;
            if (row == null) return;

            TextBox txt = e.Item.FindControl("txtValue") as TextBox;
            if (txt == null) return;

            if (row.IsNote)
            {
                txt.TextMode = TextBoxMode.MultiLine;
                txt.Rows = 8;
            }
            else
            {
                txt.TextMode = TextBoxMode.SingleLine;
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Guard()) { ShowMessage("ليس لديك صلاحية لتنفيذ هذه العملية.", true); return; }

                // Read what the editor typed, keyed by the hidden field name on each row.
                var posted = new Dictionary<string, string>();
                foreach (RepeaterItem ri in rptFields.Items)
                {
                    HiddenField hfName = ri.FindControl("hfFieldName") as HiddenField;
                    TextBox txt = ri.FindControl("txtValue") as TextBox;
                    if (hfName == null || txt == null) continue;

                    // Only accept names that exist in the schema - ignore anything injected.
                    FieldDef def = Definition.Fields.FirstOrDefault(f => f.Name == hfName.Value);
                    if (def == null) continue;

                    posted[def.Name] = txt.Text;
                }

                if (posted.Count == 0) { ShowMessage("لا توجد بيانات للحفظ.", true); return; }

                string titleValue = posted.ContainsKey("Title") ? (posted["Title"] ?? "").Trim() : "x";
                if (titleValue.Length == 0) { ShowMessage("حقل العنوان مطلوب.", true); return; }

                int id = ParseId();
                SaveItem(id, posted);

                hfMode.Value = ModeGrid;
                hfItemId.Value = "";
                Render();
                ShowMessage(id > 0 ? "تم حفظ التعديلات بنجاح." : "تمت إضافة السجل بنجاح.", false);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucListEditor - Save", ex.Message);
                ShowMessage("تعذر حفظ البيانات. تم تسجيل الخطأ.", true);
            }
        }

        // ---------------- data access ----------------

        /// <summary>
        /// Opens the target list on the target web. Elevation happens ONLY inside
        /// here, and only after Guard() has already passed in the caller.
        /// </summary>
        private void WithList(bool forWrite, Action<SPList> action)
        {
            if (!Guard()) return;

            string webUrl = WebUrl;
            string listName = Definition.ListName;
            Guid siteId = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = site.OpenWeb(webUrl))
                {
                    if (web == null || !web.Exists) return;

                    SPList list = web.Lists.TryGetList(listName);
                    if (list == null) return;

                    bool allowUnsafe = web.AllowUnsafeUpdates;
                    try
                    {
                        if (forWrite) web.AllowUnsafeUpdates = true;
                        action(list);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = allowUnsafe;
                    }
                }
            });
        }

        private void SaveItem(int id, Dictionary<string, string> posted)
        {
            WithList(true, delegate (SPList list)
            {
                SPListItem item = id > 0 ? TryGetItem(list, id) : list.AddItem();
                if (item == null) return;

                foreach (FieldDef f in Definition.Fields)
                {
                    if (!posted.ContainsKey(f.Name)) continue;
                    string raw = posted[f.Name] ?? "";

                    if (!list.Fields.ContainsField(f.Name) && f.Name != "Title") continue;

                    switch (f.Kind)
                    {
                        case FieldKind.Number:
                            double d;
                            item[f.Name] = double.TryParse(raw.Trim(), out d) ? (object)d : null;
                            break;
                        case FieldKind.Url:
                            string u = raw.Trim();
                            item[f.Name] = string.IsNullOrEmpty(u)
                                ? null
                                : (object)new SPFieldUrlValue { Url = u, Description = u };
                            break;
                        default:
                            item[f.Name] = raw;
                            break;
                    }
                }
                item.Update();
            });
        }

        private void DeleteItem(int id)
        {
            if (id <= 0) return;
            WithList(true, delegate (SPList list)
            {
                SPListItem item = TryGetItem(list, id);
                if (item != null) item.Delete();
            });
        }

        private static SPListItem TryGetItem(SPList list, int id)
        {
            try { return list.GetItemById(id); }
            catch { return null; }
        }

        private static string ReadField(SPListItem item, FieldDef f)
        {
            try
            {
                if (!item.Fields.ContainsField(f.Name)) return "";
                object v = item[f.Name];
                if (v == null) return "";
                if (f.Kind == FieldKind.Url)
                {
                    var uv = new SPFieldUrlValue(v.ToString());
                    return uv.Url ?? "";
                }
                return v.ToString();
            }
            catch { return ""; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null
                    && double.TryParse(item[field].ToString(), out v) ? v : 0;
            }
            catch { return 0; }
        }

        private int ParseId()
        {
            int id;
            return int.TryParse(hfItemId.Value, out id) ? id : 0;
        }

        private void ShowMessage(string text, bool isError)
        {
            phMessage.Visible = true;
            ltrMessage.Text = HttpUtility.HtmlEncode(text);
            pnlMessage.CssClass = isError ? "alert alert-danger mt-3" : "alert alert-success mt-3";
        }
    }

}
