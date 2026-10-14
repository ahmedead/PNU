using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.Admin
{
    public partial class ucNavCardsAdmin : UserControl
    {
        private const int MaxTextFieldLength = 255;
        private const string TargetWebMissing = "تعذر الوصول إلى موقع الأمن السيبراني /ar/SECURITYCYBER.";

        private bool _isAdmin;
        private string _initError; // already HTML-safe; shown at the end of Page_Load

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _isAdmin = ContentAdm.IsAdmin();
                pnlDenied.Visible = !_isAdmin;
                pnlMain.Visible = _isAdmin;
                phHeaderActions.Visible = _isAdmin;

                if (!_isAdmin)
                {
                    if (!ContentAdm.IsAuthenticated())
                    {
                        ltDenied.Text = "يجب تسجيل الدخول أولاً للوصول إلى لوحة التحكم. <a href=\"javascript:void(0)\" onclick=\"openAdminLoginPopup(); return false;\" class=\"btn btn-sm btn-primary ms-2 me-2\">تسجيل الدخول / Login</a>";
                    }
                    return;
                }

                if (!Page.IsPostBack)
                {
                    NavCardsProvisioner.EnsureList();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.OnInit", ex.Message);
                _initError = SafeHtml.Text(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            if (!IsPostBack)
            {
                BindCardsGrid();
            }

            if (!string.IsNullOrEmpty(_initError))
            {
                ShowError(_initError);
            }
        }

        private void BindCardsGrid()
        {
            try
            {
                List<NavCardsList> cards = new List<NavCardsList>();
                bool webFound = false;

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) return;
                        webFound = true;

                        SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                        if (list != null)
                        {
                            // Hand-mapped: SPFactory.MapListItemsToClass leaves ID/SortOrder at 0
                            cards = NavCardsProvisioner.ReadCards(list, true);
                        }
                    }
                });

                if (!webFound) ShowError(TargetWebMissing);

                ltCount.Text = cards.Count.ToString(CultureInfo.InvariantCulture);
                rptCardsGrid.DataSource = cards;
                rptCardsGrid.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تحميل بطاقات السياسات: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.BindCardsGrid", ex.Message);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            hfCardId.Value = "0";
            ltFormTitle.Text = "إضافة بطاقة جديدة";
            txtTitleAr.Text = "";
            txtTitleEn.Text = "";
            txtUrlAr.Text = "";
            txtUrlEn.Text = "";
            txtButtonTextAr.Text = "عرض التفاصيل";
            txtButtonTextEn.Text = "View Details";
            ddlTarget.SelectedValue = "_blank";
            txtIcon.Text = "hgi hgi-stroke hgi-shield-01";

            // Next order
            int count = 0;
            int.TryParse(ltCount.Text, out count);
            txtSortOrder.Text = (count + 1).ToString(CultureInfo.InvariantCulture);

            pnlEditForm.Visible = true;
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            pnlEditForm.Visible = false;
            hfCardId.Value = "0";
            ClearAlerts();
        }

        protected void btnSaveCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            string titleAr = txtTitleAr.Text.Trim();
            string titleEn = txtTitleEn.Text.Trim();
            string urlAr = txtUrlAr.Text.Trim();
            string urlEn = txtUrlEn.Text.Trim();
            string buttonAr = txtButtonTextAr.Text.Trim();
            string buttonEn = txtButtonTextEn.Text.Trim();
            string icon = txtIcon.Text.Trim();
            string target = SafeHtml.Target(ddlTarget.SelectedValue);

            int cardId;
            int.TryParse(hfCardId.Value, out cardId);

            try
            {
                if (string.IsNullOrEmpty(titleAr))
                    throw new ValidationException("الرجاء إدخال عنوان البطاقة بالعربية.");
                if (string.IsNullOrEmpty(urlAr))
                    throw new ValidationException("الرجاء إدخال رابط البطاقة بالعربية.");
                if (!SafeHtml.IsSafeUrl(urlAr))
                    throw new ValidationException("رابط البطاقة بالعربية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");
                if (!string.IsNullOrEmpty(urlEn) && !SafeHtml.IsSafeUrl(urlEn))
                    throw new ValidationException("رابط البطاقة بالإنجليزية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");
                if (string.IsNullOrEmpty(icon))
                    throw new ValidationException("الرجاء إدخال فئة الأيقونة.");

                if (string.IsNullOrEmpty(buttonAr)) buttonAr = "عرض التفاصيل";

                RequireMaxLength(titleAr, "عنوان البطاقة بالعربية");
                RequireMaxLength(titleEn, "عنوان البطاقة بالإنجليزية");
                RequireMaxLength(buttonAr, "نص الزر بالعربية");
                RequireMaxLength(buttonEn, "نص الزر بالإنجليزية");
                RequireMaxLength(icon, "فئة الأيقونة");

                double order = ParseOrder(txtSortOrder.Text, 1);

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new ValidationException(TargetWebMissing);

                        // Creates/patches the list on this same web if it is missing
                        SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName) ?? NavCardsProvisioner.EnsureListOnWeb(web);
                        if (list == null) throw new ValidationException("تعذر إنشاء قائمة " + NavCardsProvisioner.ListName + ".");

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item = cardId > 0 ? list.GetItemById(cardId) : list.AddItem();

                            item["Title"] = titleAr;
                            item["TitleEn"] = titleEn;
                            item["Url"] = urlAr;
                            item["UrlEn"] = urlEn;
                            item["ButtonText"] = buttonAr;
                            item["ButtonTextEn"] = buttonEn;
                            item["Target"] = target;
                            item["Icon"] = icon;
                            item["SortOrder"] = order;

                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                pnlEditForm.Visible = false;
                hfCardId.Value = "0";
                BindCardsGrid();
                ShowMessage(cardId > 0 ? "تم تحديث بيانات البطاقة بنجاح." : "تمت إضافة البطاقة الجديدة بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(SafeHtml.Text(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء حفظ البطاقة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.btnSaveCard_Click", ex.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            int id = GetCommandId(sender);
            if (id <= 0) return;

            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new ValidationException(TargetWebMissing);
                        SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                        if (list == null) throw new ValidationException("قائمة البطاقات غير موجودة.");

                        NavCardsList card = NavCardsProvisioner.MapItem(list.GetItemById(id), true);

                        hfCardId.Value = card.ID.ToString(CultureInfo.InvariantCulture);
                        ltFormTitle.Text = "تعديل البطاقة: " + SafeHtml.Text(card.Title);
                        txtTitleAr.Text = card.Title;
                        txtTitleEn.Text = card.TitleEn;
                        txtUrlAr.Text = card.Url;
                        txtUrlEn.Text = card.UrlEn;
                        txtButtonTextAr.Text = !string.IsNullOrEmpty(card.ButtonText) ? card.ButtonText : "عرض التفاصيل";
                        txtButtonTextEn.Text = !string.IsNullOrEmpty(card.ButtonTextEn) ? card.ButtonTextEn : "View Details";
                        ddlTarget.SelectedValue = SafeHtml.Target(card.Target);
                        txtIcon.Text = card.Icon;
                        txtSortOrder.Text = card.SortOrder.ToString(CultureInfo.InvariantCulture);

                        pnlEditForm.Visible = true;
                    }
                });
            }
            catch (ValidationException vex)
            {
                ShowError(SafeHtml.Text(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تحميل بيانات البطاقة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.btnEdit_Click", ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            int id = GetCommandId(sender);
            if (id <= 0) return;

            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new ValidationException(TargetWebMissing);
                        SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                        if (list == null) throw new ValidationException("قائمة البطاقات غير موجودة.");

                        SPListItem item = list.GetItemById(id);
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
                });

                // Close the form if it was editing the card that was just deleted
                if (hfCardId.Value == id.ToString(CultureInfo.InvariantCulture))
                {
                    pnlEditForm.Visible = false;
                    hfCardId.Value = "0";
                }

                BindCardsGrid();
                ShowMessage("تم حذف البطاقة بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(SafeHtml.Text(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء حذف البطاقة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.btnDelete_Click", ex.Message);
            }
        }

        protected void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SwapOrder(GetCommandId(sender), moveUp: true);
        }

        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            SwapOrder(GetCommandId(sender), moveUp: false);
        }

        private void SwapOrder(int id, bool moveUp)
        {
            ClearAlerts();
            if (id <= 0) return;

            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = NavCardsProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new ValidationException(TargetWebMissing);
                        SPList list = web.Lists.TryGetList(NavCardsProvisioner.ListName);
                        if (list == null) return;

                        SPListItemCollection items = list.GetItems(NavCardsProvisioner.BuildOrderedQuery());
                        if (items == null || items.Count <= 1) return;

                        int currentIndex = -1;
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].ID == id)
                            {
                                currentIndex = i;
                                break;
                            }
                        }

                        if (currentIndex == -1) return;
                        int targetIndex = moveUp ? currentIndex - 1 : currentIndex + 1;
                        if (targetIndex < 0 || targetIndex >= items.Count) return;

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem currentItem = items[currentIndex];
                            SPListItem targetItem = items[targetIndex];

                            double currentOrder = NavCardsProvisioner.MapItem(currentItem, true).SortOrder;
                            double targetOrder = NavCardsProvisioner.MapItem(targetItem, true).SortOrder;

                            if (currentOrder == targetOrder)
                            {
                                currentOrder = currentIndex + 1;
                                targetOrder = targetIndex + 1;
                            }

                            currentItem["SortOrder"] = targetOrder;
                            targetItem["SortOrder"] = currentOrder;

                            currentItem.Update();
                            targetItem.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                BindCardsGrid();
            }
            catch (ValidationException vex)
            {
                ShowError(SafeHtml.Text(vex.Message));
            }
            catch (Exception ex)
            {
                ShowError("تعذر تغيير ترتيب البطاقة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucNavCardsAdmin.SwapOrder", ex.Message);
            }
        }

        protected void btnProvision_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            try
            {
                bool ok = NavCardsProvisioner.ForceProvision();
                BindCardsGrid();

                if (ok)
                    ShowMessage("تم التحقق من القائمة والحقول وتحديث الروابط الناقصة في الموقع /ar/SECURITYCYBER بنجاح.");
                else
                    ShowError("تعذرت تهيئة القائمة. تأكد من وجود الموقع /ar/SECURITYCYBER وراجع سجل الأخطاء.");
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تهيئة القوائم: " + SafeHtml.Text(ex.Message));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();
            BindCardsGrid();
        }

        #region Helpers

        public static string Text(object value) { return SafeHtml.Text(value); }
        public static string Attr(object value) { return SafeHtml.Attr(value); }

        private static int GetCommandId(object sender)
        {
            LinkButton btn = sender as LinkButton;
            int id;
            return btn != null && int.TryParse(btn.CommandArgument, out id) ? id : 0;
        }

        // Messages are rendered as HTML — callers encode any list/user/exception text they include
        private void ShowMessage(string msg)
        {
            ltMessage.Text = msg;
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string err)
        {
            ltError.Text = err;
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
            ltMessage.Text = "";
            ltError.Text = "";
        }

        private static void RequireMaxLength(string value, string label)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > MaxTextFieldLength)
                throw new ValidationException(string.Format("{0} يجب ألا يتجاوز {1} حرفًا.", label, MaxTextFieldLength));
        }

        private static double ParseOrder(string text, double fallback)
        {
            double value;
            string s = (text ?? "").Trim();
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) return value;
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out value)) return value;
            return fallback;
        }

        private static string CurrentUrl()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    return HttpContext.Current.Request.Url.ToString();
            }
            catch { }
            return "ucNavCardsAdmin";
        }

        /// <summary>User-facing validation error (shown as-is, not logged).</summary>
        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        #endregion
    }
}
