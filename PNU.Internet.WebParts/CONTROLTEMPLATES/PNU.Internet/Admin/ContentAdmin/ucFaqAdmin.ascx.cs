using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign;
using System;
using System.Data;
using System.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ContentAdmin
{
    public partial class ucFaqAdmin : UserControl
    {
        private bool _isAdmin;
        private string _initError; // shown after Page_Load, since BindGrid clears alerts
        private const string TargetWebUrl = "/ar/FAQs/";
        private const int MaxTextFieldLength = 255;

        private string CurrentList
        {
            get
            {
                if (ddlList != null && !string.IsNullOrEmpty(ddlList.SelectedValue))
                    return ddlList.SelectedValue;
                if (hfCurrentList != null && !string.IsNullOrEmpty(hfCurrentList.Value))
                    return hfCurrentList.Value;
                return FAQListNames.FAQQuestions;
            }
            set
            {
                if (hfCurrentList != null) hfCurrentList.Value = value;
                if (ddlList != null && ddlList.Items.FindByValue(value) != null)
                    ddlList.SelectedValue = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _isAdmin = IsUserAuthorized();
                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;
                btnRefreshHeader.Visible = _isAdmin;

                if (!_isAdmin)
                {
                    if (!ContentAdm.IsAuthenticated())
                    {
                        ltDenied.Text = "يجب تسجيل الدخول أولاً للوصول إلى لوحة تحكم الأسئلة الشائعة. <a href=\"javascript:void(0)\" onclick=\"openAdminLoginPopup(); return false;\" class=\"btn btn-sm btn-primary ms-2\">تسجيل الدخول / Login</a>";
                    }
                    return;
                }

                // Ensure FAQ lists and EN fields are provisioned on /ar/FAQs/ (first load only,
                // and never on a fallback web when /ar/FAQs/ is missing)
                if (!Page.IsPostBack)
                {
                    if (TargetWebExists())
                        FAQListInitializer.EnsureAllListsExist(TargetWebUrl);
                    else
                        _initError = "الموقع " + TargetWebUrl + " غير موجود. لا يمكن إدارة قوائم الأسئلة الشائعة.";
                }
            }
            catch (Exception ex)
            {
                _initError = "خطأ في التهيئة: " + Server.HtmlEncode(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            if (!IsPostBack)
            {
                PopulateCategoryDropdowns();
                PopulateQuestionDropdowns();
                BindGrid();
                UpdateFormVisibility();
            }

            if (!string.IsNullOrEmpty(_initError))
                ShowError(_initError);
        }

        private bool IsUserAuthorized()
        {
            if (!ContentAdm.IsAuthenticated())
            {
                ContentAdm.PromptLogin(Page);
                return false;
            }

            try
            {
                if (ContentAdm.IsAdmin()) return true;

                if (SPContext.Current != null && SPContext.Current.Web != null)
                {
                    if (SPContext.Current.Web.UserIsSiteAdmin) return true;
                }
            }
            catch { }

            return false;
        }

        #region Data Binding & UI Updates

        private void BindGrid()
        {
            ClearAlerts();
            string listName = CurrentList;
            ltCurrentListNameDisplay.Text = Server.HtmlEncode(GetListDisplayName(listName));

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("ColTitle", typeof(string));
            dt.Columns.Add("ColCategory", typeof(string));
            dt.Columns.Add("ColExtra", typeof(string));
            dt.Columns.Add("SortOrder", typeof(int));
            dt.Columns.Add("TitleRaw", typeof(string));

            string searchFilter = txtSearch != null ? txtSearch.Text.Trim().ToLowerInvariant() : "";
            string categoryFilter = ddlFilterCategory != null && ddlFilterCategory.SelectedIndex > 0 ? ddlFilterCategory.SelectedValue : "";
            string questionFilter = ddlFilterQuestion != null && ddlFilterQuestion.SelectedIndex > 0 ? ddlFilterQuestion.SelectedValue : "";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = OpenFaqWeb(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(listName);
                        if (list == null) return;

                        SPQuery query = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /><FieldRef Name='Title' Ascending='TRUE' /></OrderBy>"
                        };

                        SPListItemCollection items = list.GetItems(query);
                        if (items == null) return;

                        foreach (SPListItem item in items)
                        {
                            string titleAr = item.Title ?? "";
                            string titleEn = SafeString(item, "TitleEn");
                            string displayTitle = !string.IsNullOrEmpty(titleEn)
                                ? string.Format("{0} <span class='text-muted small'>({1})</span>", Server.HtmlEncode(titleAr), Server.HtmlEncode(titleEn))
                                : Server.HtmlEncode(titleAr);

                            int sortOrder = ParseInt(SafeString(item, "SortOrder", "0"), 0);

                            string categoryCol = "";
                            string extraCol = "";
                            string searchExtra = "";

                            if (listName == FAQListNames.FAQCategories)
                            {
                                string icon = SafeString(item, "IconClass");
                                extraCol = "الأيقونة: " + Server.HtmlEncode(icon);
                                searchExtra = icon;
                            }
                            else if (listName == FAQListNames.FAQQuestions)
                            {
                                categoryCol = SafeString(item, "CategoryName");
                                string detailAr = SafeString(item, "QuestionTextAr");
                                string detailEn = SafeString(item, "QuestionTextEn");
                                string textDisp = !string.IsNullOrEmpty(detailEn) ? detailAr + " | EN: " + detailEn : detailAr;
                                extraCol = !string.IsNullOrEmpty(textDisp) ? Server.HtmlEncode(Truncate(textDisp, 80)) : "-";
                                searchExtra = detailAr + " " + detailEn;

                                if (!string.IsNullOrEmpty(categoryFilter) && !string.Equals(categoryCol, categoryFilter, StringComparison.OrdinalIgnoreCase))
                                    continue;
                            }
                            else if (listName == FAQListNames.FAQAnswerItems)
                            {
                                categoryCol = SafeString(item, "QuestionTitle");
                                string type = SafeString(item, "ItemType");
                                string contentAr = SafeString(item, "ContentTextAr");
                                string contentEn = SafeString(item, "ContentTextEn");
                                string contentDisp = !string.IsNullOrEmpty(contentEn) ? contentAr + " [EN: " + contentEn + "]" : contentAr;

                                extraCol = string.Format("النوع: <span class='badge bg-info text-dark'>{0}</span> | المحتوى: {1}",
                                    Server.HtmlEncode(type), !string.IsNullOrEmpty(contentDisp) ? Server.HtmlEncode(Truncate(contentDisp, 70)) : "-");
                                searchExtra = type + " " + contentAr + " " + contentEn;

                                if (!string.IsNullOrEmpty(questionFilter) && !string.Equals(categoryCol, questionFilter, StringComparison.OrdinalIgnoreCase))
                                    continue;
                            }

                            if (!string.IsNullOrEmpty(searchFilter))
                            {
                                string searchable = (titleAr + " " + titleEn + " " + categoryCol + " " + searchExtra).ToLowerInvariant();
                                if (!searchable.Contains(searchFilter)) continue;
                            }

                            DataRow row = dt.NewRow();
                            row["ID"] = item.ID.ToString();
                            row["ColTitle"] = displayTitle;
                            row["ColCategory"] = !string.IsNullOrEmpty(categoryCol) ? Server.HtmlEncode(categoryCol) : "-";
                            row["ColExtra"] = extraCol;
                            row["SortOrder"] = sortOrder;
                            row["TitleRaw"] = titleAr;
                            dt.Rows.Add(row);
                        }
                    }
                }
            });

            // SortOrder may be stored as a Text column, so the CAML order is lexical (1, 10, 2...).
            // Re-sort numerically in memory.
            DataView view = dt.DefaultView;
            view.Sort = "SortOrder ASC, TitleRaw ASC";

            rptItems.DataSource = view;
            rptItems.DataBind();

            ltItemCount.Text = dt.Rows.Count.ToString();
            pnlEmpty.Visible = (dt.Rows.Count == 0);

            UpdateFilterBarVisibility();
        }

        private void PopulateCategoryDropdowns()
        {
            try
            {
                ddlQuestionCategory.Items.Clear();
                ddlFilterCategory.Items.Clear();
                ddlFilterCategory.Items.Add(new ListItem("-- جميع التصنيفات --", ""));

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = OpenFaqWeb(site))
                        {
                            if (web == null) return;
                            var repo = new FAQRepository();
                            var categories = repo.GetCategories(web);
                            foreach (var c in categories)
                            {
                                string label = !string.IsNullOrEmpty(c.TitleEn) ? string.Format("{0} ({1})", c.Title, c.TitleEn) : c.Title;
                                ddlQuestionCategory.Items.Add(new ListItem(label, c.Title));
                                ddlFilterCategory.Items.Add(new ListItem(label, c.Title));
                            }
                        }
                    }
                });
            }
            catch { }
        }

        private void PopulateQuestionDropdowns()
        {
            try
            {
                ddlAnswerQuestion.Items.Clear();
                ddlFilterQuestion.Items.Clear();
                ddlFilterQuestion.Items.Add(new ListItem("-- جميع الأسئلة --", ""));

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = OpenFaqWeb(site))
                        {
                            if (web == null) return;
                            var repo = new FAQRepository();
                            var questions = repo.GetQuestions(web);
                            foreach (var q in questions)
                            {
                                string label = !string.IsNullOrEmpty(q.TitleEn) ? string.Format("{0} ({1})", q.Title, q.TitleEn) : q.Title;
                                ddlAnswerQuestion.Items.Add(new ListItem(label, q.Title));
                                ddlFilterQuestion.Items.Add(new ListItem(label, q.Title));
                            }
                        }
                    }
                });
            }
            catch { }
        }

        private void UpdateFormVisibility()
        {
            string list = CurrentList;
            phCategoryFields.Visible = (list == FAQListNames.FAQCategories);
            phQuestionFields.Visible = (list == FAQListNames.FAQQuestions);
            phAnswerFields.Visible = (list == FAQListNames.FAQAnswerItems);
        }

        private void UpdateFilterBarVisibility()
        {
            string list = CurrentList;
            phCategoryFilter.Visible = (list == FAQListNames.FAQQuestions);
            phQuestionFilter.Visible = (list == FAQListNames.FAQAnswerItems);
        }

        private string GetListDisplayName(string listName)
        {
            if (listName == FAQListNames.FAQCategories) return "تصنيفات الأسئلة (FAQCategories)";
            if (listName == FAQListNames.FAQQuestions) return "أسئلة FAQ (FAQQuestions)";
            if (listName == FAQListNames.FAQAnswerItems) return "فقرات إجابات FAQ (FAQAnswerItems)";
            return listName;
        }

        #endregion

        #region Event Handlers

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            CurrentList = ddlList.SelectedValue;
            pnlForm.Visible = false;
            hfEditingId.Value = "";
            UpdateFormVisibility();
            BindGrid();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            hfEditingId.Value = "";
            ltFormTitle.Text = Server.HtmlEncode("إضافة عنصر جديد - " + GetListDisplayName(CurrentList));
            ClearFormInputs();
            UpdateFormVisibility();
            pnlForm.Visible = true;
        }

        protected void btnSeed_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            try
            {
                if (!TargetWebExists())
                    throw new Exception("الموقع " + TargetWebUrl + " غير موجود.");

                FAQListInitializer.EnsureAllListsExist(TargetWebUrl);
                PopulateCategoryDropdowns();
                PopulateQuestionDropdowns();
                BindGrid();
                ShowSuccess("تم التحقق من قوائم الأسئلة الشائعة على " + TargetWebUrl + "، وإنشاء القوائم والحقول الناقصة. لا تُعبأ البيانات الافتراضية إلا في القوائم المنشأة حديثًا، ولم تتغير البيانات الحالية.");
            }
            catch (Exception ex)
            {
                ShowError("تعذر تهيئة القوائم: " + Server.HtmlEncode(ex.Message));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            PopulateCategoryDropdowns();
            PopulateQuestionDropdowns();
            BindGrid();
            ShowSuccess("تم تحديث البيانات.");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            string listName = CurrentList;
            string editingId = hfEditingId.Value;
            bool isEdit = !string.IsNullOrEmpty(editingId);

            try
            {
                int editId = 0;
                if (isEdit && !int.TryParse(editingId, out editId))
                    throw new Exception("معرّف العنصر غير صالح.");

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = OpenFaqWeb(site))
                        {
                            if (web == null)
                                throw new Exception("الموقع " + TargetWebUrl + " غير موجود.");

                            SPList list = web.Lists.TryGetList(listName);
                            if (list == null)
                                throw new Exception("القائمة المطلوب التعديل عليها غير موجودة في " + TargetWebUrl);

                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPListItem item = isEdit ? list.GetItemById(editId) : list.Items.Add();
                                string oldTitle = isEdit ? (item.Title ?? "") : "";

                                if (listName == FAQListNames.FAQCategories)
                                {
                                    string catTitle = txtCategoryTitle.Text.Trim();
                                    string catTitleEn = txtCategoryTitleEn.Text.Trim();
                                    if (string.IsNullOrEmpty(catTitle))
                                        throw new Exception("اسم التصنيف بالعربية مطلوب.");
                                    RequireMaxLength(catTitle, "اسم التصنيف بالعربية");
                                    RequireMaxLength(catTitleEn, "اسم التصنيف بالإنجليزية");
                                    RequireMaxLength(txtCategoryIconClass.Text.Trim(), "فئة الأيقونة");
                                    RequireUniqueTitle(list, catTitle, isEdit ? editId : 0, "يوجد تصنيف آخر بنفس الاسم.");

                                    SetFieldValue(item, "Title", catTitle);
                                    SetFieldValue(item, "TitleEn", catTitleEn);
                                    SetFieldValue(item, "IconClass", txtCategoryIconClass.Text.Trim());
                                    SetFieldValue(item, "SortOrder", ParseInt(txtCategorySortOrder.Text, 1));
                                    item.Update();

                                    // Questions reference categories by title — keep them linked after a rename
                                    if (isEdit)
                                        CascadeParentRename(web, FAQListNames.FAQQuestions, "CategoryName", "CategoryNameEn", oldTitle, catTitle, catTitleEn);
                                }
                                else if (listName == FAQListNames.FAQQuestions)
                                {
                                    string qTitle = txtQuestionTitle.Text.Trim();
                                    string qTitleEn = txtQuestionTitleEn.Text.Trim();
                                    string category = ddlQuestionCategory.SelectedValue;
                                    if (string.IsNullOrEmpty(qTitle))
                                        throw new Exception("عنوان السؤال بالعربية مطلوب.");
                                    if (string.IsNullOrEmpty(category))
                                        throw new Exception("يجب اختيار التصنيف التابع له السؤال.");
                                    RequireMaxLength(qTitle, "عنوان السؤال بالعربية");
                                    RequireMaxLength(qTitleEn, "عنوان السؤال بالإنجليزية");
                                    RequireUniqueTitle(list, qTitle, isEdit ? editId : 0, "يوجد سؤال آخر بنفس العنوان.");

                                    SPListItem parent = FindItemByTitle(web.Lists.TryGetList(FAQListNames.FAQCategories), category);
                                    if (parent == null)
                                        throw new Exception("التصنيف المختار غير موجود. يرجى تحديث الصفحة.");

                                    SetFieldValue(item, "Title", qTitle);
                                    SetFieldValue(item, "TitleEn", qTitleEn);
                                    SetFieldValue(item, "CategoryName", category);
                                    SetFieldValue(item, "CategoryNameEn", SafeString(parent, "TitleEn"));
                                    SetFieldValue(item, "QuestionTextAr", txtQuestionTextAr.Text.Trim());
                                    SetFieldValue(item, "QuestionTextEn", txtQuestionTextEn.Text.Trim());
                                    SetFieldValue(item, "SortOrder", ParseInt(txtQuestionSortOrder.Text, 1));
                                    item.Update();

                                    // Answer items reference questions by title — keep them linked after a rename
                                    if (isEdit)
                                        CascadeParentRename(web, FAQListNames.FAQAnswerItems, "QuestionTitle", "QuestionTitleEn", oldTitle, qTitle, qTitleEn);
                                }
                                else if (listName == FAQListNames.FAQAnswerItems)
                                {
                                    string contentAr = txtAnswerContentTextAr.Text.Trim();
                                    string question = ddlAnswerQuestion.SelectedValue;
                                    if (string.IsNullOrEmpty(contentAr))
                                        throw new Exception("نص محتوى الفقرة بالعربية مطلوب.");
                                    if (string.IsNullOrEmpty(question))
                                        throw new Exception("يجب اختيار السؤال التابع له الإجابة.");

                                    string labelAr = txtAnswerTitle.Text.Trim();
                                    if (string.IsNullOrEmpty(labelAr)) labelAr = Truncate(contentAr, 50, false);
                                    RequireMaxLength(labelAr, "العنوان المختصر بالعربية");
                                    RequireMaxLength(txtAnswerTitleEn.Text.Trim(), "العنوان المختصر بالإنجليزية");

                                    SPListItem parent = FindItemByTitle(web.Lists.TryGetList(FAQListNames.FAQQuestions), question);
                                    if (parent == null)
                                        throw new Exception("السؤال المختار غير موجود. يرجى تحديث الصفحة.");

                                    SetFieldValue(item, "Title", labelAr);
                                    SetFieldValue(item, "TitleEn", txtAnswerTitleEn.Text.Trim());
                                    SetFieldValue(item, "QuestionTitle", question);
                                    SetFieldValue(item, "QuestionTitleEn", SafeString(parent, "TitleEn"));
                                    SetFieldValue(item, "ItemType", ddlAnswerItemType.SelectedValue);
                                    SetFieldValue(item, "ContentTextAr", contentAr);
                                    SetFieldValue(item, "ContentTextEn", txtAnswerContentTextEn.Text.Trim());
                                    SetFieldValue(item, "SortOrder", ParseInt(txtAnswerSortOrder.Text, 1));
                                    item.Update();
                                }
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });

                pnlForm.Visible = false;
                hfEditingId.Value = "";
                PopulateCategoryDropdowns();
                PopulateQuestionDropdowns();
                BindGrid();
                ShowSuccess(isEdit ? "تم حفظ التعديلات بنجاح." : "تمت إضافة العنصر الجديد بنجاح.");
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء الحفظ: " + Server.HtmlEncode(ex.Message));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            pnlForm.Visible = false;
            hfEditingId.Value = "";
            ClearAlerts();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            string idStr = Convert.ToString(e.CommandArgument);
            int itemId;
            if (!int.TryParse(idStr, out itemId))
            {
                ShowError("معرّف العنصر غير صالح.");
                return;
            }
            string listName = CurrentList;

            if (e.CommandName == "EditItem")
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = OpenFaqWeb(site))
                            {
                                if (web == null) return;
                                SPList list = web.Lists.TryGetList(listName);
                                if (list == null) return;

                                SPListItem item = list.GetItemById(itemId);

                                hfEditingId.Value = idStr;
                                ltFormTitle.Text = Server.HtmlEncode(string.Format("تعديل عنصر #{0} - {1}", idStr, GetListDisplayName(listName)));

                                if (listName == FAQListNames.FAQCategories)
                                {
                                    txtCategoryTitle.Text = item.Title ?? "";
                                    txtCategoryTitleEn.Text = SafeString(item, "TitleEn");
                                    txtCategoryIconClass.Text = SafeString(item, "IconClass");
                                    txtCategorySortOrder.Text = ParseInt(SafeString(item, "SortOrder", "1"), 1).ToString();
                                }
                                else if (listName == FAQListNames.FAQQuestions)
                                {
                                    txtQuestionTitle.Text = item.Title ?? "";
                                    txtQuestionTitleEn.Text = SafeString(item, "TitleEn");
                                    string cat = SafeString(item, "CategoryName");
                                    if (ddlQuestionCategory.Items.FindByValue(cat) != null)
                                        ddlQuestionCategory.SelectedValue = cat;
                                    txtQuestionTextAr.Text = SafeString(item, "QuestionTextAr");
                                    txtQuestionTextEn.Text = SafeString(item, "QuestionTextEn");
                                    txtQuestionSortOrder.Text = ParseInt(SafeString(item, "SortOrder", "1"), 1).ToString();
                                }
                                else if (listName == FAQListNames.FAQAnswerItems)
                                {
                                    txtAnswerTitle.Text = item.Title ?? "";
                                    txtAnswerTitleEn.Text = SafeString(item, "TitleEn");
                                    string qParent = SafeString(item, "QuestionTitle");
                                    if (ddlAnswerQuestion.Items.FindByValue(qParent) != null)
                                        ddlAnswerQuestion.SelectedValue = qParent;

                                    string type = SafeString(item, "ItemType");
                                    if (ddlAnswerItemType.Items.FindByValue(type) != null)
                                        ddlAnswerItemType.SelectedValue = type;

                                    txtAnswerContentTextAr.Text = SafeString(item, "ContentTextAr");
                                    txtAnswerContentTextEn.Text = SafeString(item, "ContentTextEn");
                                    txtAnswerSortOrder.Text = ParseInt(SafeString(item, "SortOrder", "1"), 1).ToString();
                                }

                                UpdateFormVisibility();
                                pnlForm.Visible = true;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    ShowError("تعذر تحميل بيانات العنصر للتعديل: " + Server.HtmlEncode(ex.Message));
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = OpenFaqWeb(site))
                            {
                                if (web == null) return;
                                SPList list = web.Lists.TryGetList(listName);
                                if (list == null) return;

                                SPListItem item = list.GetItemById(itemId);

                                // Block deletes that would orphan child rows (they link by parent title)
                                if (listName == FAQListNames.FAQCategories)
                                {
                                    int children = CountChildren(web, FAQListNames.FAQQuestions, "CategoryName", item.Title);
                                    if (children > 0)
                                        throw new Exception(string.Format("لا يمكن حذف التصنيف لارتباط {0} سؤال/أسئلة به. احذف الأسئلة أو انقلها إلى تصنيف آخر أولاً.", children));
                                }
                                else if (listName == FAQListNames.FAQQuestions)
                                {
                                    int children = CountChildren(web, FAQListNames.FAQAnswerItems, "QuestionTitle", item.Title);
                                    if (children > 0)
                                        throw new Exception(string.Format("لا يمكن حذف السؤال لارتباط {0} فقرة إجابة به. احذف فقرات الإجابة أولاً.", children));
                                }

                                web.AllowUnsafeUpdates = true;
                                try
                                {
                                    item.Delete();
                                }
                                finally
                                {
                                    web.AllowUnsafeUpdates = false;
                                }
                            }
                        }
                    });

                    // Close the form if it was editing the item that was just deleted
                    if (hfEditingId.Value == idStr)
                    {
                        pnlForm.Visible = false;
                        hfEditingId.Value = "";
                    }

                    PopulateCategoryDropdowns();
                    PopulateQuestionDropdowns();
                    BindGrid();
                    ShowSuccess("تم حذف العنصر بنجاح.");
                }
                catch (Exception ex)
                {
                    ShowError("خطأ أثناء الحذف: " + Server.HtmlEncode(ex.Message));
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindGrid();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            if (txtSearch != null) txtSearch.Text = "";
            if (ddlFilterCategory != null && ddlFilterCategory.Items.Count > 0) ddlFilterCategory.SelectedIndex = 0;
            if (ddlFilterQuestion != null && ddlFilterQuestion.Items.Count > 0) ddlFilterQuestion.SelectedIndex = 0;
            BindGrid();
        }

        protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindGrid();
        }

        protected void ddlFilterQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindGrid();
        }

        #endregion

        #region SharePoint Helpers

        /// <summary>
        /// Opens /ar/FAQs/ only. FAQListInitializer.OpenTargetWeb falls back to the current web
        /// when the target is missing; the admin must never read or write FAQ lists there.
        /// </summary>
        private static SPWeb OpenFaqWeb(SPSite site)
        {
            SPWeb web = FAQListInitializer.OpenTargetWeb(site, TargetWebUrl);
            if (web == null) return null;

            string expected = TargetWebUrl.TrimEnd('/');
            string actual = (web.ServerRelativeUrl ?? "").TrimEnd('/');
            if (web.Exists && actual.EndsWith(expected, StringComparison.OrdinalIgnoreCase))
                return web;

            web.Dispose();
            return null;
        }

        private static bool TargetWebExists()
        {
            bool exists = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = OpenFaqWeb(site))
                    {
                        exists = web != null;
                    }
                }
            });
            return exists;
        }

        private static SPListItemCollection GetItemsByField(SPList list, string fieldName, string value)
        {
            if (list == null || string.IsNullOrEmpty(value) || !list.Fields.ContainsField(fieldName)) return null;

            SPQuery query = new SPQuery
            {
                Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>",
                    fieldName, SecurityElement.Escape(value))
            };
            return list.GetItems(query);
        }

        private static SPListItem FindItemByTitle(SPList list, string title)
        {
            SPListItemCollection items = GetItemsByField(list, "Title", title);
            return items != null && items.Count > 0 ? items[0] : null;
        }

        private static void RequireUniqueTitle(SPList list, string title, int currentId, string message)
        {
            SPListItemCollection items = GetItemsByField(list, "Title", title);
            if (items == null) return;

            foreach (SPListItem existing in items)
            {
                if (existing.ID != currentId)
                    throw new Exception(message);
            }
        }

        private static int CountChildren(SPWeb web, string childListName, string fkField, string parentTitle)
        {
            SPListItemCollection items = GetItemsByField(web.Lists.TryGetList(childListName), fkField, parentTitle);
            return items != null ? items.Count : 0;
        }

        private static void CascadeParentRename(SPWeb web, string childListName, string fkField, string fkEnField,
            string oldTitle, string newTitle, string newTitleEn)
        {
            SPListItemCollection children = GetItemsByField(web.Lists.TryGetList(childListName), fkField, oldTitle);
            if (children == null) return;

            foreach (SPListItem child in children)
            {
                SetFieldValue(child, fkField, newTitle);
                SetFieldValue(child, fkEnField, newTitleEn);
                child.Update();
            }
        }

        #endregion

        #region Helpers

        private void ClearFormInputs()
        {
            txtCategoryTitle.Text = "";
            txtCategoryTitleEn.Text = "";
            txtCategoryIconClass.Text = "";
            txtCategorySortOrder.Text = "1";

            txtQuestionTitle.Text = "";
            txtQuestionTitleEn.Text = "";
            txtQuestionTextAr.Text = "";
            txtQuestionTextEn.Text = "";
            txtQuestionSortOrder.Text = "1";

            txtAnswerTitle.Text = "";
            txtAnswerTitleEn.Text = "";
            txtAnswerContentTextAr.Text = "";
            txtAnswerContentTextEn.Text = "";
            txtAnswerSortOrder.Text = "1";
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
        }

        // msg is rendered as HTML — callers must encode any user/list/exception text they include
        private void ShowSuccess(string msg)
        {
            pnlMessage.Visible = true;
            ltMessage.Text = msg;
        }

        private void ShowError(string msg)
        {
            pnlError.Visible = true;
            ltError.Text = msg;
        }

        private static void RequireMaxLength(string value, string label)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > MaxTextFieldLength)
                throw new Exception(string.Format("{0} يجب ألا يتجاوز {1} حرفًا.", label, MaxTextFieldLength));
        }

        private static string Truncate(string s, int max, bool ellipsis = true)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s;
            return s.Substring(0, max) + (ellipsis ? "..." : "");
        }

        private static string SafeString(SPListItem item, string fieldName, string fallback = "")
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return fallback;
            if (!item.Fields.ContainsField(fieldName)) return fallback;
            object o = item[fieldName];
            return o != null ? o.ToString() : fallback;
        }

        private static void SetFieldValue(SPListItem item, string fieldName, object val)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return;
            if (item.Fields.ContainsField(fieldName))
            {
                item[fieldName] = val;
            }
        }

        private static int ParseInt(string str, int fallback = 1)
        {
            int val;
            if (int.TryParse(str, out val)) return val;

            // Number fields come back as "3" but older values may be "3.00000000000000"
            double d;
            return double.TryParse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d) ? (int)d : fallback;
        }

        #endregion
    }
}
