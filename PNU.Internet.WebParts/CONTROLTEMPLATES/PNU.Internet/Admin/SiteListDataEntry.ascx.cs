using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class SiteListDataEntry : UserControl
    {
        // -------------------------------------------------------
        // Constants
        // -------------------------------------------------------
        private const int SP_IMAGE_FIELD_TYPE = 23;
        private const int PAGE_SIZE = 15;

        // -------------------------------------------------------
        // Skip list (same as before)
        // -------------------------------------------------------
        private static readonly HashSet<string> SkipFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ID","ContentTypeId","ContentType","FileRef","FileDirRef","FileLeafRef","FSObjType",
            "SortBehavior","_Level","_IsCurrentVersion","Created","Author","Modified","Editor",
            "CheckoutUser","CheckedOutUserId","owshiddenversion","InstanceID","Order","GUID",
            "UniqueId","EncodedAbsUrl","EncodedAbsWebImgUrl","ServerUrl","BaseName",
            "LinkFilename","LinkFilenameNoMenu","LinkTitle","LinkTitleNoMenu","_UIVersionString",
            "MetaInfo","WorkflowVersion","Attachments","_ModerationStatus","_UIVersion",
            "_ModerationComments"
        };

        // -------------------------------------------------------
        // ViewState
        // -------------------------------------------------------
        private string SelectedWebUrl
        {
            get { return ViewState["SelectedWebUrl"] as string ?? string.Empty; }
            set { ViewState["SelectedWebUrl"] = value; }
        }

        private List<string> RenderedFields
        {
            get { return ViewState["RenderedFields"] as List<string> ?? new List<string>(); }
            set { ViewState["RenderedFields"] = value; }
        }

        private List<string> ImageFields
        {
            get { return ViewState["ImageFields"] as List<string> ?? new List<string>(); }
            set { ViewState["ImageFields"] = value; }
        }

        private int EditingItemId
        {
            get { return ViewState["EditingItemId"] as int? ?? 0; }
            set { ViewState["EditingItemId"] = value; }
        }

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] as int? ?? 0; }
            set { ViewState["CurrentPage"] = value; }
        }

        private bool FormInitialised
        {
            get { return ViewState["FormInitialised"] as bool? ?? false; }
            set { ViewState["FormInitialised"] = value; }
        }

        private Dictionary<int, string> PagePositions
        {
            get
            {
                if (ViewState["PagePositions"] == null)
                    ViewState["PagePositions"] = new Dictionary<int, string>();
                return (Dictionary<int, string>)ViewState["PagePositions"];
            }
        }

        // -------------------------------------------------------
        // Page_Load
        // CRITICAL: dynamic controls MUST be recreated here, before
        // any event fires, so ASP.NET can restore their state and
        // route the postback event correctly.
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSiteUrl.Text = SPContext.Current.Web.Url;
                return;
            }

            // Always rebuild dynamic controls on every postback when active
            if (FormInitialised
                && !string.IsNullOrEmpty(SelectedWebUrl)
                && !string.IsNullOrEmpty(ddlLists.SelectedValue))
            {
                RebuildFormControls();
                // Grid is rendered purely as HTML with plain <button onclick="sldGridAction(...)">
                // so we do NOT need to rebuild it here for event routing.
            }
        }

        // -------------------------------------------------------
        // Rebuild phFields from the cached RenderedFields list.
        // This MUST match exactly what BuildFormFromSharePoint() created
        // so that control IDs are identical and ViewState restores correctly.
        // -------------------------------------------------------
        private void RebuildFormControls()
        {
            if (string.IsNullOrEmpty(SelectedWebUrl) || RenderedFields.Count == 0) return;

            phFields.Controls.Clear();

            try
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                {
                    SPList list = web.Lists[ddlLists.SelectedValue];

                    foreach (string internalName in RenderedFields)
                    {
                        SPField field;
                        try { field = list.Fields.GetFieldByInternalName(internalName); }
                        catch { continue; }
                        RenderFieldRow(field);
                    }

                    bool hasImage = ImageFields.Count > 0;
                    pnlImageLibrary.Visible = hasImage;
                    if (hasImage) LoadImageLibraries(web);
                }
            }
            catch { /* ignore on postback */ }

            pnlForm.Visible = true;

            // Restore UI state
            if (EditingItemId > 0)
            {
                lblFormTitle.Text = "تعديل العنصر #" + EditingItemId;
                btnCancelEdit.Visible = true;
            }
            else
            {
                lblFormTitle.Text = "إضافة عنصر جديد";
                btnCancelEdit.Visible = false;
            }
        }

        // -------------------------------------------------------
        // STEP 1 — Load lists
        // -------------------------------------------------------
        protected void btnLoadLists_Click(object sender, EventArgs e)
        {
            lblSiteError.Visible = false;
            pnlLists.Visible = false;
            pnlForm.Visible = false;
            pnlGrid.Visible = false;
            FormInitialised = false;

            string url = txtSiteUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) { ShowError(lblSiteError, "الرجاء إدخال رابط الموقع."); return; }

            try
            {
                using (SPSite site = new SPSite(url))
                using (SPWeb web = site.OpenWeb(new Uri(url).PathAndQuery))
                {
                    if (!web.Exists) { ShowError(lblSiteError, "الموقع غير موجود."); return; }

                    SelectedWebUrl = web.Url;
                    ddlLists.Items.Clear();
                    ddlLists.Items.Add(new ListItem("-- اختر قائمة --", ""));
                    foreach (SPList list in web.Lists)
                        if (!list.Hidden)
                            ddlLists.Items.Add(new ListItem(list.Title, list.Title));
                }
                pnlLists.Visible = true;
            }
            catch (Exception ex) { ShowError(lblSiteError, "خطأ: " + ex.Message); }
        }

        // -------------------------------------------------------
        // STEP 2 — Load fields + grid
        // -------------------------------------------------------
        protected void btnLoadFields_Click(object sender, EventArgs e)
        {
            lblListError.Visible = false;

            if (string.IsNullOrEmpty(ddlLists.SelectedValue))
            {
                ShowError(lblListError, "الرجاء اختيار قائمة أولاً.");
                return;
            }

            EditingItemId = 0;
            CurrentPage = 0;
            PagePositions.Clear();

            BuildFormFromSharePoint();
            BindGrid();

            FormInitialised = true;
        }

        // -------------------------------------------------------
        // Grid action dispatcher (Edit / Delete)
        // This single stable server button receives ALL grid actions
        // via hfActionType + hfActionItemId set by JS.
        // -------------------------------------------------------
        protected void btnGridAction_Click(object sender, EventArgs e)
        {
            string action = hfActionType.Value;
            int itemId;
            if (!int.TryParse(hfActionItemId.Value, out itemId)) return;

            // Reset hidden fields
            hfActionType.Value = "";
            hfActionItemId.Value = "0";

            if (action == "edit")
            {
                EditingItemId = itemId;
                lblFormTitle.Text = "تعديل العنصر #" + itemId;
                btnCancelEdit.Visible = true;
                lblFormMsg.Visible = false;

                // Controls were already rebuilt in Page_Load — just fill values
                FillFormWithItem(itemId);

                pnlForm.Visible = true;
                pnlGrid.Visible = true;
                BindGrid();

                ScriptManager.RegisterStartupScript(this, GetType(), "scrollForm",
                    "setTimeout(function(){ document.getElementById('" + pnlForm.ClientID +
                    "').scrollIntoView({behavior:'smooth'}); }, 100);", true);
            }
            else if (action == "delete")
            {
                try
                {
                    using (SPSite site = new SPSite(SelectedWebUrl))
                    using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                    {
                        web.AllowUnsafeUpdates = true;
                        web.Lists[ddlLists.SelectedValue].GetItemById(itemId).Delete();
                        web.AllowUnsafeUpdates = false;
                    }

                    ShowSuccess(lblGridMsg, "تم حذف العنصر بنجاح.");

                    if (EditingItemId == itemId)
                    {
                        EditingItemId = 0;
                        lblFormTitle.Text = "إضافة عنصر جديد";
                        btnCancelEdit.Visible = false;
                        ClearFormControls();
                    }
                }
                catch (Exception ex) { ShowError(lblGridMsg, "خطأ في الحذف: " + ex.Message); }

                BindGrid();
            }
        }

        // -------------------------------------------------------
        // Cancel Edit
        // -------------------------------------------------------
        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            EditingItemId = 0;
            lblFormTitle.Text = "إضافة عنصر جديد";
            btnCancelEdit.Visible = false;
            lblFormMsg.Visible = false;
            ClearFormControls();
            BindGrid();
        }

        // -------------------------------------------------------
        // Save (Add or Update)
        // -------------------------------------------------------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblFormMsg.Visible = false;

            if (string.IsNullOrEmpty(ddlLists.SelectedValue))
            { ShowError(lblFormMsg, "الرجاء اختيار قائمة."); return; }

            if (ImageFields.Count > 0 && string.IsNullOrEmpty(ddlImageLibrary.SelectedValue))
            {
                bool needsPicker = false;
                foreach (string fn in ImageFields)
                    if (!string.Equals(fn, "PublishingRollupImage", StringComparison.OrdinalIgnoreCase))
                    { needsPicker = true; break; }

                if (needsPicker)
                { ShowError(lblFormMsg, "الرجاء اختيار مكتبة الصور."); return; }
            }

            var fieldErrors = new System.Text.StringBuilder();

            try
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                {
                    web.AllowUnsafeUpdates = true;
                    SPList list = web.Lists[ddlLists.SelectedValue];

                    SPListItem item = EditingItemId > 0
                        ? list.GetItemById(EditingItemId)
                        : list.Items.Add();

                    foreach (string internalName in RenderedFields)
                    {
                        SPField field;
                        try { field = list.Fields.GetFieldByInternalName(internalName); }
                        catch { continue; }

                        object value;
                        if (ImageFields.Contains(internalName))
                        {
                            try { value = UploadImageAndGetValue(web, field, internalName); }
                            catch (Exception ex)
                            {
                                fieldErrors.AppendFormat("خطأ في رفع الصورة ({0}): {1}\n", field.Title, ex.Message);
                                continue;
                            }
                            if (value == null && EditingItemId > 0) continue;
                        }
                        else
                        {
                            Control ctrl = phFields.FindControl("field_" + internalName);
                            if (ctrl == null) continue;
                            value = ReadControlValue(ctrl, field);
                        }

                        if (value == null) continue;

                        // Use SetFieldValue for sealed/readonly fields to bypass SP validation
                        try
                        {
                            if (field.Sealed || field.ReadOnlyField)
                                item[field.Id] = value;   // set by GUID, bypasses name-based checks
                            else
                                item[internalName] = value;
                        }
                        catch (Exception ex)
                        {
                            fieldErrors.AppendFormat("خطأ في حقل ({0}): {1}\n", field.Title, ex.Message);
                        }
                    }

                    // Use SystemUpdate to persist sealed/publishing fields without version bump issues
                    try
                    {
                        item.SystemUpdate(false);
                    }
                    catch
                    {
                        // Fallback to regular Update if SystemUpdate fails
                        item.Update();
                    }

                    web.AllowUnsafeUpdates = false;
                }

                if (fieldErrors.Length > 0)
                    ShowError(lblFormMsg, "تم الحفظ مع بعض الأخطاء:\n" + fieldErrors);
                else
                    ShowSuccess(lblFormMsg,
                        EditingItemId > 0 ? "تم تحديث البيانات بنجاح." : "تم حفظ البيانات بنجاح.");

                EditingItemId = 0;
                lblFormTitle.Text = "إضافة عنصر جديد";
                btnCancelEdit.Visible = false;
                ClearFormControls();
                BindGrid();
            }
            catch (Exception ex) { ShowError(lblFormMsg, "خطأ في الحفظ: " + ex.Message); }
        }

        // -------------------------------------------------------
        // Refresh / Paging
        // -------------------------------------------------------
        protected void btnRefreshGrid_Click(object sender, EventArgs e) { BindGrid(); }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        { if (CurrentPage > 0) { CurrentPage--; BindGrid(); } }

        protected void btnNextPage_Click(object sender, EventArgs e)
        { CurrentPage++; BindGrid(); }

        // -------------------------------------------------------
        // Build form from SharePoint (called once per list selection)
        // -------------------------------------------------------
        private void BuildFormFromSharePoint()
        {
            phFields.Controls.Clear();

            var fieldNames = new List<string>();
            var imageFields = new List<string>();
            bool hasImage = false;
            bool allRollup = true;   // true when every image field is PublishingRollupImage

            try
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                {
                    SPList list = web.Lists[ddlLists.SelectedValue];

                    foreach (SPField field in list.Fields)
                    {
                        if (ShouldSkipField(field)) continue;
                        fieldNames.Add(field.InternalName);

                        if (IsImageField(field))
                        {
                            imageFields.Add(field.InternalName);
                            hasImage = true;
                            if (!IsRollupImageField(field)) allRollup = false;
                        }

                        RenderFieldRow(field);
                    }

                    // Only load + show the library picker when there are
                    // non-rollup image fields that need a manual library choice.
                    if (hasImage && !allRollup) LoadImageLibraries(web);
                }
            }
            catch (Exception ex) { ShowError(lblListError, "خطأ في تحميل الحقول: " + ex.Message); return; }

            RenderedFields = fieldNames;
            ImageFields = imageFields;

            // Show picker only for non-rollup image fields
            pnlImageLibrary.Visible = hasImage && !allRollup;
            pnlForm.Visible = fieldNames.Count > 0;
            lblFormTitle.Text = "إضافة عنصر جديد";
            btnCancelEdit.Visible = false;
        }

        // -------------------------------------------------------
        // Render grid — uses plain HTML buttons + JS onclick,
        // NO dynamic server button wiring needed.
        // -------------------------------------------------------
        private void BindGrid()
        {
            phGrid.Controls.Clear();
            lblGridMsg.Visible = false;

            try
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                {
                    SPList list = web.Lists[ddlLists.SelectedValue];

                    // Collect display columns (max 8)
                    var displayFields = new List<SPField>();
                    foreach (SPField field in list.Fields)
                    {
                        if (ShouldSkipField(field)) continue;
                        displayFields.Add(field);
                        if (displayFields.Count >= 8) break;
                    }

                    // Paged SP query
                    SPQuery query = new SPQuery { RowLimit = (uint)PAGE_SIZE };
                    string pos = GetPagePosition(CurrentPage);
                    if (!string.IsNullOrEmpty(pos))
                        query.ListItemCollectionPosition = new SPListItemCollectionPosition(pos);

                    SPListItemCollection items = list.GetItems(query);
                    SavePagePosition(CurrentPage + 1, items.ListItemCollectionPosition != null
                        ? items.ListItemCollectionPosition.PagingInfo : null);

                    lblPageInfo.Text = string.Format("الصفحة {0}", CurrentPage + 1);
                    btnPrevPage.Enabled = CurrentPage > 0;
                    btnNextPage.Enabled = items.ListItemCollectionPosition != null;

                    // Build table
                    var tbl = new HtmlTable { CellSpacing = 0 };
                    tbl.Attributes["class"] = "sld-grid";

                    // Header row
                    var hRow = new HtmlTableRow();
                    AddHeaderCell(hRow, "#");
                    foreach (var f in displayFields) AddHeaderCell(hRow, f.Title);
                    AddHeaderCell(hRow, "إجراءات");
                    tbl.Rows.Add(hRow);

                    foreach (SPListItem item in items)
                    {
                        var row = new HtmlTableRow();
                        if (item.ID == EditingItemId)
                            row.Attributes["class"] = "sld-edit-row";

                        AddCell(row, item.ID.ToString());

                        foreach (SPField f in displayFields)
                        {
                            object val = null;
                            try { val = item[f.InternalName]; } catch { }

                            var td = new HtmlTableCell();
                            if (val != null && IsImageField(f))
                            {
                                string imgUrl = ExtractImageUrl(val.ToString());
                                if (!string.IsNullOrEmpty(imgUrl))
                                {
                                    var img = new HtmlImage { Src = imgUrl };
                                    img.Attributes["class"] = "sld-grid-img";
                                    td.Controls.Add(img);
                                }
                            }
                            else
                            {
                                td.InnerText = val != null ? SafeGetText(f, val) : string.Empty;
                            }
                            row.Cells.Add(td);
                        }

                        // Action cell — plain HTML buttons, no server wiring needed
                        var tdAct = new HtmlTableCell();
                        tdAct.Attributes["class"] = "sld-actions";
                        tdAct.InnerHtml = string.Format(
                            "<button type=\"button\" class=\"sld-btn-warning\" onclick=\"sldGridAction('edit',{0})\">تعديل</button>" +
                            "<button type=\"button\" class=\"sld-btn-danger\"  onclick=\"sldGridAction('delete',{0})\">حذف</button>",
                            item.ID);
                        row.Cells.Add(tdAct);

                        tbl.Rows.Add(row);
                    }

                    // Empty state
                    if (tbl.Rows.Count == 1)
                    {
                        var emptyRow = new HtmlTableRow();
                        var emptyTd = new HtmlTableCell { ColSpan = displayFields.Count + 2 };
                        emptyTd.InnerText = "لا توجد بيانات في هذه القائمة.";
                        emptyTd.Style["text-align"] = "center";
                        emptyTd.Style["color"] = "#888";
                        emptyRow.Cells.Add(emptyTd);
                        tbl.Rows.Add(emptyRow);
                    }

                    phGrid.Controls.Add(tbl);
                }

                pnlGrid.Visible = true;
            }
            catch (Exception ex)
            {
                ShowError(lblGridMsg, "خطأ في تحميل البيانات: " + ex.Message);
                pnlGrid.Visible = true;
            }
        }

        // -------------------------------------------------------
        // Fill form controls with existing item values (for Edit)
        // -------------------------------------------------------
        private void FillFormWithItem(int itemId)
        {
            try
            {
                using (SPSite site = new SPSite(SelectedWebUrl))
                using (SPWeb web = site.OpenWeb(new Uri(SelectedWebUrl).PathAndQuery))
                {
                    SPList list = web.Lists[ddlLists.SelectedValue];
                    SPListItem item = list.GetItemById(itemId);

                    foreach (string internalName in RenderedFields)
                    {
                        object raw;
                        try { raw = item[internalName]; } catch { continue; }
                        if (raw == null) continue;

                        SPField field;
                        try { field = list.Fields.GetFieldByInternalName(internalName); } catch { continue; }

                        if (ImageFields.Contains(internalName))
                        {
                            var preview = phFields.FindControl("field_" + internalName + "_preview") as HtmlImage;
                            if (preview != null)
                            {
                                string imgUrl = ExtractImageUrl(raw.ToString());
                                if (!string.IsNullOrEmpty(imgUrl))
                                { preview.Src = imgUrl; preview.Style["display"] = "block"; }
                            }
                            continue;
                        }

                        Control ctrl = phFields.FindControl("field_" + internalName);
                        if (ctrl != null) SetControlValue(ctrl, field, raw);
                    }
                }
            }
            catch { /* ignore */ }
        }

        // -------------------------------------------------------
        // Clear all form controls
        // -------------------------------------------------------
        private void ClearFormControls()
        {
            foreach (string internalName in RenderedFields)
            {
                if (ImageFields.Contains(internalName))
                {
                    var prev = phFields.FindControl("field_" + internalName + "_preview") as HtmlImage;
                    if (prev != null) { prev.Src = ""; prev.Style["display"] = "none"; }
                    continue;
                }

                Control ctrl = phFields.FindControl("field_" + internalName);
                if (ctrl == null) continue;

                if (ctrl is TextBox tb) { tb.Text = ""; continue; }
                if (ctrl is CheckBox chk) { chk.Checked = false; continue; }
                if (ctrl is DropDownList ddl && ddl.Items.Count > 0) { ddl.SelectedIndex = 0; continue; }
                if (ctrl is ListBox lb) { foreach (ListItem li in lb.Items) li.Selected = false; continue; }
                if (ctrl is Panel p && p.Controls.Count > 0 && p.Controls[0] is TextBox ptb) ptb.Text = "";
            }
        }

        // -------------------------------------------------------
        // Render one field row
        // -------------------------------------------------------
        private void RenderFieldRow(SPField field)
        {
            var row = new HtmlTableRow();
            var tdLbl = new HtmlTableCell { InnerText = field.Title };
            var tdInp = new HtmlTableCell();
            tdInp.Controls.Add(BuildInputControl(field));
            row.Cells.Add(tdLbl);
            row.Cells.Add(tdInp);
            phFields.Controls.Add(row);
        }

        private Control BuildInputControl(SPField field)
        {
            string id = "field_" + field.InternalName;

            if (IsImageField(field))
            {
                var wrapper = new Panel { ID = id + "_wrapper" };
                var fu = new HtmlInputFile { ID = id };
                fu.Attributes["accept"] = "image/*";
                var preview = new HtmlImage { ID = id + "_preview" };
                preview.Attributes["class"] = "sld-img-preview";
                preview.Alt = field.Title;
                wrapper.Controls.Add(fu);
                wrapper.Controls.Add(preview);
                return wrapper;
            }

            switch (field.Type)
            {
                case SPFieldType.Boolean:
                    return new CheckBox { ID = id };

                case SPFieldType.Choice:
                    var ddl = new DropDownList { ID = id, CssClass = "sld-select" };
                    var cf = field as SPFieldChoice;
                    if (cf != null) foreach (string c in cf.Choices) ddl.Items.Add(new ListItem(c, c));
                    return ddl;

                case SPFieldType.MultiChoice:
                    var lb = new ListBox { ID = id, SelectionMode = ListSelectionMode.Multiple, CssClass = "sld-select", Rows = 4 };
                    var mf = field as SPFieldMultiChoice;
                    if (mf != null) foreach (string c in mf.Choices) lb.Items.Add(new ListItem(c, c));
                    return lb;

                case SPFieldType.Note:
                    return new TextBox { ID = id, TextMode = TextBoxMode.MultiLine, CssClass = "sld-select", Rows = 4 };

                case SPFieldType.Number:
                case SPFieldType.Currency:
                case SPFieldType.Integer:
                    return new TextBox { ID = id, CssClass = "sld-select", TextMode = TextBoxMode.SingleLine };

                case SPFieldType.DateTime:
                    var dtBox = new TextBox { ID = id, CssClass = "sld-select" };
                    dtBox.Attributes.Add("placeholder", "yyyy-MM-dd");
                    return dtBox;

                case SPFieldType.URL:
                    var urlPanel = new Panel();
                    urlPanel.Controls.Add(new TextBox { ID = id, CssClass = "sld-select" });
                    return urlPanel;

                default:
                    return new TextBox { ID = id, CssClass = "sld-select" };
            }
        }

        // -------------------------------------------------------
        // Set control value from SP item (Edit mode)
        // -------------------------------------------------------
        private void SetControlValue(Control ctrl, SPField field, object raw)
        {
            string strVal = "";
            try { strVal = field.GetFieldValueAsText(raw) ?? raw.ToString(); }
            catch { strVal = raw.ToString(); }

            if (ctrl is CheckBox chk)
            { bool b; chk.Checked = bool.TryParse(raw.ToString(), out b) && b; return; }

            if (ctrl is DropDownList ddl)
            { var li = ddl.Items.FindByValue(strVal); if (li != null) ddl.SelectedValue = strVal; return; }

            if (ctrl is ListBox lb)
            {
                foreach (string part in strVal.Split(new[] { ";#" }, StringSplitOptions.RemoveEmptyEntries))
                { var li = lb.Items.FindByValue(part.Trim()); if (li != null) li.Selected = true; }
                return;
            }

            if (ctrl is TextBox tb)
            {
                tb.Text = (field.Type == SPFieldType.DateTime && raw is DateTime dt)
                    ? dt.ToString("yyyy-MM-dd") : strVal;
                return;
            }

            if (ctrl is Panel panel && panel.Controls.Count > 0 && panel.Controls[0] is TextBox ptb)
            { try { ptb.Text = new SPFieldUrlValue(raw.ToString()).Url; } catch { ptb.Text = raw.ToString(); } }
        }

        // -------------------------------------------------------
        // Read control value for Save
        // -------------------------------------------------------
        private object ReadControlValue(Control ctrl, SPField field)
        {
            if (ctrl is CheckBox chk) return chk.Checked;

            if (ctrl is DropDownList ddl)
                return string.IsNullOrEmpty(ddl.SelectedValue) ? null : (object)ddl.SelectedValue;

            if (ctrl is ListBox lb)
            {
                var sel = new List<string>();
                foreach (ListItem li in lb.Items) if (li.Selected) sel.Add(li.Value);
                return sel.Count > 0 ? string.Join(";#", sel) : null;
            }

            if (ctrl is TextBox tb)
            {
                string text = tb.Text.Trim();
                if (string.IsNullOrEmpty(text)) return null;
                if (field.Type == SPFieldType.DateTime)
                { DateTime dt; return DateTime.TryParse(text, out dt) ? (object)dt : null; }
                if (field.Type == SPFieldType.Number || field.Type == SPFieldType.Currency)
                { double d; return double.TryParse(text, out d) ? (object)d : null; }
                if (field.Type == SPFieldType.Integer)
                { int i; return int.TryParse(text, out i) ? (object)i : null; }
                return text;
            }

            if (ctrl is Panel panel && panel.Controls.Count > 0 && panel.Controls[0] is TextBox ptb)
                return string.IsNullOrWhiteSpace(ptb.Text) ? null
                    : (object)new SPFieldUrlValue { Url = ptb.Text.Trim(), Description = ptb.Text.Trim() };

            return null;
        }

        // -------------------------------------------------------
        // Image upload
        // For PublishingRollupImage: auto-uploads to "PublishingImages"
        // or "Images" library (whichever exists) — no picker needed.
        // For other image fields: uses ddlImageLibrary selection.
        // -------------------------------------------------------
        private object UploadImageAndGetValue(SPWeb web, SPField field, string internalName)
        {
            var wrapper = phFields.FindControl("field_" + internalName + "_wrapper") as Panel;
            if (wrapper == null) return null;
            var fu = wrapper.FindControl("field_" + internalName) as HtmlInputFile;
            if (fu == null || fu.PostedFile == null || fu.PostedFile.ContentLength == 0) return null;

            byte[] bytes;
            using (var ms = new MemoryStream()) { fu.PostedFile.InputStream.CopyTo(ms); bytes = ms.ToArray(); }

            string origName = Path.GetFileName(fu.PostedFile.FileName);
            string safeName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + origName;

            // Resolve the target library
            SPList targetLibrary = ResolveImageLibrary(web, field);
            if (targetLibrary == null)
                throw new InvalidOperationException("لم يتم العثور على مكتبة صور مناسبة. الرجاء اختيار مكتبة من القائمة.");

            SPFile uploaded = targetLibrary.RootFolder.Files.Add(safeName, bytes, true);

            // Build absolute URL using the host authority + server-relative path
            // so it works correctly regardless of which web the library is on.
            string serverRel = uploaded.ServerRelativeUrl;
            string hostUrl = new Uri(web.Url).GetLeftPart(UriPartial.Authority);
            string absUrl = hostUrl + serverRel;

            // Determine the correct value format for this field type
            // PublishingRollupImage and Publishing Image fields → <img> HTML tag
            if (IsRollupImageField(field)
                || string.Equals(field.TypeAsString, "PublishingImage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(field.TypeAsString, "PublishingRollupImage", StringComparison.OrdinalIgnoreCase)
                || field.Sealed)   // sealed image fields on publishing lists store <img> tag
                return string.Format("<img alt=\"{0}\" src=\"{1}\" />", origName, absUrl);

            // Modern SP Image field (type int 23) with JSON format
            if ((int)field.Type == SP_IMAGE_FIELD_TYPE)
                return string.Format("{{\"serverRelativeUrl\":\"{0}\"}}", serverRel);

            // Fallback: plain absolute URL
            return absUrl;
        }

        // Resolve the library to upload to:
        //   - For PublishingRollupImage: look for "PublishingImages" then "Images" on the root web,
        //     then fall back to the current web, then finally to the picker selection.
        //   - For all other image fields: use the picker selection.
        private SPList ResolveImageLibrary(SPWeb web, SPField field)
        {
            var candidates = new[] { "PublishingImages", "Images", "صور", "مكتبة الصور" };

            bool autoResolve = IsRollupImageField(field) || field.Sealed;

            if (autoResolve)
            {
                // Priority 1: current web (same subsite as the list)
                foreach (string candidate in candidates)
                {
                    try
                    {
                        SPList lib = web.Lists.TryGetList(candidate);
                        if (lib != null) return lib;
                    }
                    catch { }
                }

                // Priority 2: first picture library in current web
                foreach (SPList list in web.Lists)
                {
                    if (!list.Hidden && list.BaseTemplate == SPListTemplateType.PictureLibrary)
                        return list;
                }

                // Priority 3: first document library in current web
                foreach (SPList list in web.Lists)
                {
                    if (!list.Hidden && list.BaseTemplate == SPListTemplateType.DocumentLibrary)
                        return list;
                }

                // Priority 4: root web fallback
                foreach (string candidate in candidates)
                {
                    try
                    {
                        SPList lib = web.Site.RootWeb.Lists.TryGetList(candidate);
                        if (lib != null) return lib;
                    }
                    catch { }
                }
            }

            // Fall back to picker selection
            if (!string.IsNullOrEmpty(ddlImageLibrary.SelectedValue))
            {
                try { return web.Lists[ddlImageLibrary.SelectedValue]; }
                catch { }
            }

            return null;
        }

        // -------------------------------------------------------
        // Image libraries loader
        // -------------------------------------------------------
        private void LoadImageLibraries(SPWeb web)
        {
            ddlImageLibrary.Items.Clear();
            ddlImageLibrary.Items.Add(new ListItem("-- اختر مكتبة الصور --", ""));

            string autoSelect = null;

            foreach (SPList list in web.Lists)
            {
                if (list.Hidden) continue;
                if (list.BaseTemplate == SPListTemplateType.PictureLibrary ||
                    list.BaseTemplate == SPListTemplateType.DocumentLibrary)
                {
                    ddlImageLibrary.Items.Add(new ListItem(list.Title, list.Title));

                    // Auto-select PublishingImages / Images / Arabic equivalents if found
                    if (autoSelect == null &&
                        (string.Equals(list.Title, "PublishingImages", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(list.Title, "Images", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(list.Title, "صور", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(list.Title, "مكتبة الصور", StringComparison.OrdinalIgnoreCase)))
                        autoSelect = list.Title;
                }
            }

            if (autoSelect != null)
            {
                ListItem li = ddlImageLibrary.Items.FindByValue(autoSelect);
                if (li != null) ddlImageLibrary.SelectedValue = autoSelect;
            }
        }

        // -------------------------------------------------------
        // Paging helpers
        // -------------------------------------------------------
        private string GetPagePosition(int page)
        { string v; return PagePositions.TryGetValue(page, out v) ? v : null; }

        private void SavePagePosition(int page, string position)
        { if (position != null) PagePositions[page] = position; }

        // -------------------------------------------------------
        // Field / type helpers
        // -------------------------------------------------------
        private bool ShouldSkipField(SPField field)
        {
            if (field.InternalName.Equals("ID", StringComparison.OrdinalIgnoreCase)) return true;
            if (SkipFields.Contains(field.InternalName)) return true;
            if (field.Hidden) return true;

            // Always show image fields regardless of Sealed / ReadOnly / FromBaseType —
            // publishing image fields (e.g. صورة الالتفاف) are sealed by SP but must be editable.
            if (IsImageField(field)) return false;

            if (field.Sealed) return true;
            if (field.ReadOnlyField) return true;
            if (field.FromBaseType && field.InternalName != "Title") return true;
            return false;
        }

        private bool IsImageField(SPField field)
        {
            // 1. Match by SP type integer (23)
            if ((int)field.Type == SP_IMAGE_FIELD_TYPE) return true;

            // 2. Match by TypeAsString — covers all known Publishing image variants
            string ts = field.TypeAsString ?? string.Empty;
            switch (ts.ToLowerInvariant())
            {
                case "image":
                case "publishingimage":
                case "publishingrollupimage":
                case "thumbnailimage":
                case "rollupimage":
                    return true;
            }

            // 3. Match by internal name for the standard publishing rollup image field
            if (field.InternalName.Equals("PublishingRollupImage", StringComparison.OrdinalIgnoreCase))
                return true;

            // 4. Last resort: field description or title contains image-related keywords
            //    This catches custom image fields with non-standard type strings.
            string title = (field.Title ?? string.Empty).Trim();
            if ((title == "صورة الالتفاف" || title.StartsWith("صورة", StringComparison.Ordinal))
                && (field.Type == SPFieldType.Note || field.Type == SPFieldType.Text
                    || (int)field.Type == SP_IMAGE_FIELD_TYPE))
                return true;

            return false;
        }

        // Is this the specific PublishingRollupImage field (auto-resolves its own library)?
        private bool IsRollupImageField(SPField field)
        {
            return field.InternalName.Equals("PublishingRollupImage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(field.TypeAsString, "PublishingRollupImage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(field.TypeAsString, "RollupImage", StringComparison.OrdinalIgnoreCase);
        }

        private string ExtractImageUrl(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return null;
            if (raw.Contains("serverRelativeUrl"))
            { int s = raw.IndexOf(':') + 2, end = raw.IndexOf('"', s); if (s > 1 && end > s) return raw.Substring(s, end - s); }
            if (raw.Contains("<img"))
            { int s = raw.IndexOf("src=\"") + 5, end = raw.IndexOf('"', s); if (s > 4 && end > s) return raw.Substring(s, end - s); }
            if (raw.StartsWith("http", StringComparison.OrdinalIgnoreCase) || raw.StartsWith("/")) return raw;
            return null;
        }

        private string SafeGetText(SPField field, object val)
        { try { return field.GetFieldValueAsText(val) ?? val.ToString(); } catch { return val.ToString(); } }

        // -------------------------------------------------------
        // Table helpers
        // -------------------------------------------------------
        private void AddHeaderCell(HtmlTableRow row, string text)
        { row.Cells.Add(new HtmlTableCell("th") { InnerText = text }); }

        private void AddCell(HtmlTableRow row, string text)
        { row.Cells.Add(new HtmlTableCell { InnerText = text }); }

        // -------------------------------------------------------
        // Message helpers
        // -------------------------------------------------------
        private void ShowError(Label lbl, string msg)
        { lbl.Text = msg; lbl.CssClass = "sld-msg-error"; lbl.Visible = true; }

        private void ShowSuccess(Label lbl, string msg)
        { lbl.Text = msg; lbl.CssClass = "sld-msg-success"; lbl.Visible = true; }
    }
}