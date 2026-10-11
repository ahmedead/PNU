using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.Admin
{
    public partial class ucHomeNoraFutureAdmin : UserControl
    {
        private const int MaxTextFieldLength = 255;
        private const string AdminWebMissing = "تعذر الوصول إلى موقع إدارة المحتوى /ar/ContentAdmin.";

        private bool _isAdmin;
        private string _initError; // shown at the end of Page_Load (ClearAlerts runs there first)

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // asp:CheckBox puts CssClass on a wrapper <span>; Bootstrap switches need it on the <input>
            chkShowSection.InputAttributes["class"] = "form-check-input";
            chkCardIsActive.InputAttributes["class"] = "form-check-input";

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
                    HomeNoraFutureProvisioner.EnsureAllListsExist();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.OnInit", ex.Message);
                _initError = Enc(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            ClearAlerts();

            if (!IsPostBack)
            {
                LoadTitleSettings();
                BindCardsGrid();
            }

            if (!string.IsNullOrEmpty(_initError))
                ShowError(_initError);
        }

        #region Section Title Logic

        private void LoadTitleSettings()
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) return;

                        SPListItem item = HomeNoraFutureRepository.GetTitleItem(web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameTitle));
                        if (item != null)
                        {
                            txtTitleAr.Text = HomeNoraFutureRepository.SafeString(item, "Title");
                            txtTitleEn.Text = HomeNoraFutureRepository.SafeString(item, "Title_EN");
                            txtAriaLabelAr.Text = HomeNoraFutureRepository.SafeString(item, "AriaLabel");
                            txtAriaLabelEn.Text = HomeNoraFutureRepository.SafeString(item, "AriaLabel_EN");
                            chkShowSection.Checked = HomeNoraFutureRepository.SafeBool(item, "ShowSection", true);
                        }
                        else
                        {
                            // Defaults
                            txtTitleAr.Text = "الدراسة في جامعة الأميرة نورة";
                            txtTitleEn.Text = "Study at Princess Nourah University";
                            txtAriaLabelAr.Text = "برامج ومستقبل نورة";
                            txtAriaLabelEn.Text = "PNU Programs & Future";
                            chkShowSection.Checked = true;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.LoadTitleSettings", ex.Message);
                ShowError("خطأ أثناء تحميل إعدادات العنوان: " + Enc(ex.Message));
            }
        }

        protected void btnSaveTitle_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string titleAr = txtTitleAr.Text.Trim();
            string titleEn = txtTitleEn.Text.Trim();
            string ariaAr = txtAriaLabelAr.Text.Trim();
            string ariaEn = txtAriaLabelEn.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(titleAr))
                    throw new ValidationException("الرجاء إدخال عنوان القسم بالعربية.");
                RequireMaxLength(titleAr, "عنوان القسم بالعربية");
                RequireMaxLength(titleEn, "عنوان القسم بالإنجليزية");
                RequireMaxLength(ariaAr, "تسمية إمكانية الوصول بالعربية");
                RequireMaxLength(ariaEn, "تسمية إمكانية الوصول بالإنجليزية");

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) throw new ValidationException(AdminWebMissing);

                        SPList list = GetOrCreateList(web, HomeNoraFutureProvisioner.ListNameTitle);

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item = HomeNoraFutureRepository.GetTitleItem(list) ?? list.AddItem();
                            item["Title"] = titleAr;
                            item["Title_EN"] = titleEn;
                            item["AriaLabel"] = ariaAr;
                            item["AriaLabel_EN"] = ariaEn;
                            item["ShowSection"] = chkShowSection.Checked;
                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                HomeNoraFutureRepository.InvalidateCache();
                ShowMessage("تم حفظ إعدادات عنوان القسم بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.btnSaveTitle_Click", ex.Message);
                ShowError("فشل حفظ إعدادات العنوان: " + Enc(ex.Message));
            }
        }

        #endregion

        #region Program Cards CRUD

        private void BindCardsGrid()
        {
            try
            {
                List<HomeNoraFutureItem> items = new List<HomeNoraFutureItem>();
                string filter = txtSearchCards.Text.Trim().ToLowerInvariant();

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) return;

                        SPList list = web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameItems);
                        if (list == null) return;

                        foreach (SPListItem row in list.GetItems(HomeNoraFutureRepository.BuildOrderedQuery()))
                        {
                            HomeNoraFutureItem itm = HomeNoraFutureRepository.MapItem(row);
                            if (string.IsNullOrEmpty(filter) || Matches(itm, filter))
                                items.Add(itm);
                        }
                    }
                });

                gvCards.DataSource = items;
                gvCards.DataBind();

                lblCardCount.Text = string.Format("{0} بطاقة مسجلة", items.Count);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.BindCardsGrid", ex.Message);
                ShowError("خطأ أثناء استرجاع بطاقات البرامج: " + Enc(ex.Message));
            }
        }

        private static bool Matches(HomeNoraFutureItem itm, string filter)
        {
            string searchable = string.Join(" ", new[]
            {
                itm.Title, itm.Title_EN, itm.Description, itm.Description_EN, itm.LinkUrl, itm.LinkUrl_EN, itm.IconClass
            }).ToLowerInvariant();
            return searchable.Contains(filter);
        }

        protected void btnNewCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            hfCardId.Value = "0";
            ltCardFormTitle.Text = "إضافة بطاقة برنامج جديدة";
            txtCardTitleAr.Text = "";
            txtCardTitleEn.Text = "";
            txtCardDescAr.Text = "";
            txtCardDescEn.Text = "";
            txtCardLinkAr.Text = "";
            txtCardLinkEn.Text = "";
            txtCardIconClass.Text = "hgi-book-01";
            txtCardOrder.Text = (GetMaxOrder() + 1).ToString(CultureInfo.InvariantCulture);
            chkCardIsActive.Checked = true;

            pnlCardForm.Visible = true;
        }

        protected void btnCancelCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            pnlCardForm.Visible = false;
            hfCardId.Value = "0";
        }

        protected void btnSaveCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            string titleAr = txtCardTitleAr.Text.Trim();
            string titleEn = txtCardTitleEn.Text.Trim();
            string descAr = txtCardDescAr.Text.Trim();
            string descEn = txtCardDescEn.Text.Trim();
            string linkAr = txtCardLinkAr.Text.Trim();
            string linkEn = txtCardLinkEn.Text.Trim();
            string icon = txtCardIconClass.Text.Trim();

            int cardId;
            int.TryParse(hfCardId.Value, out cardId);

            try
            {
                if (string.IsNullOrEmpty(titleAr))
                    throw new ValidationException("الرجاء إدخال عنوان البطاقة بالعربية.");
                if (string.IsNullOrEmpty(titleEn))
                    throw new ValidationException("الرجاء إدخال عنوان البطاقة بالإنجليزية.");
                if (string.IsNullOrEmpty(linkAr))
                    throw new ValidationException("الرجاء إدخال رابط البطاقة.");
                if (string.IsNullOrEmpty(icon))
                    throw new ValidationException("الرجاء إدخال فئة الأيقونة.");
                if (!HomeNoraFutureRepository.IsSafeUrl(linkAr))
                    throw new ValidationException("رابط البطاقة بالعربية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");
                if (!string.IsNullOrEmpty(linkEn) && !HomeNoraFutureRepository.IsSafeUrl(linkEn))
                    throw new ValidationException("رابط البطاقة بالإنجليزية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");

                RequireMaxLength(titleAr, "عنوان البطاقة بالعربية");
                RequireMaxLength(titleEn, "عنوان البطاقة بالإنجليزية");
                RequireMaxLength(descAr, "الوصف بالعربية");
                RequireMaxLength(descEn, "الوصف بالإنجليزية");
                RequireMaxLength(linkAr, "رابط البطاقة بالعربية");
                RequireMaxLength(linkEn, "رابط البطاقة بالإنجليزية");
                RequireMaxLength(icon, "فئة الأيقونة");

                double order = ParseOrder(txtCardOrder.Text, 1);

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) throw new ValidationException(AdminWebMissing);

                        SPList list = GetOrCreateList(web, HomeNoraFutureProvisioner.ListNameItems);

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item = cardId > 0 ? list.GetItemById(cardId) : list.AddItem();

                            item["Title"] = titleAr;
                            item["Title_EN"] = titleEn;
                            item["Description"] = descAr;
                            item["Description_EN"] = descEn;
                            item["LinkUrl"] = linkAr;
                            item["LinkUrl_EN"] = linkEn;
                            item["IconClass"] = icon;
                            item["ItemOrder"] = order;
                            item["IsActive"] = chkCardIsActive.Checked;
                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                HomeNoraFutureRepository.InvalidateCache();
                pnlCardForm.Visible = false;
                hfCardId.Value = "0";
                BindCardsGrid();
                ShowMessage(cardId > 0 ? "تم تحديث البطاقة بنجاح." : "تمت إضافة البطاقة بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.btnSaveCard_Click", ex.Message);
                ShowError("فشل حفظ البطاقة: " + Enc(ex.Message));
            }
        }

        protected void gvCards_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!_isAdmin) return;

            int itemId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out itemId)) return;

            if (e.CommandName == "EditCard")
            {
                LoadCardForEdit(itemId);
            }
            else if (e.CommandName == "DeleteCard")
            {
                DeleteCard(itemId);
            }
            else if (e.CommandName == "ToggleActive")
            {
                ToggleCardStatus(itemId);
            }
        }

        protected void gvCards_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Placeholder for row customization if needed
        }

        private void LoadCardForEdit(int itemId)
        {
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) throw new ValidationException(AdminWebMissing);

                        SPList list = web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameItems);
                        if (list == null) throw new ValidationException("قائمة البطاقات غير موجودة.");

                        HomeNoraFutureItem card = HomeNoraFutureRepository.MapItem(list.GetItemById(itemId));

                        hfCardId.Value = card.ID.ToString(CultureInfo.InvariantCulture);
                        ltCardFormTitle.Text = "تعديل البطاقة: " + Enc(card.Title);
                        txtCardTitleAr.Text = card.Title;
                        txtCardTitleEn.Text = card.Title_EN;
                        txtCardDescAr.Text = card.Description;
                        txtCardDescEn.Text = card.Description_EN;
                        txtCardLinkAr.Text = card.LinkUrl;
                        txtCardLinkEn.Text = card.LinkUrl_EN;
                        txtCardIconClass.Text = card.IconClass;
                        txtCardOrder.Text = card.ItemOrder.ToString(CultureInfo.InvariantCulture);
                        chkCardIsActive.Checked = card.IsActive;

                        pnlCardForm.Visible = true;
                    }
                });
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.LoadCardForEdit", ex.Message);
                ShowError("فشل تحميل بيانات البطاقة: " + Enc(ex.Message));
            }
        }

        private void DeleteCard(int itemId)
        {
            try
            {
                UpdateCard(itemId, item => item.Delete());

                // Close the form if it was editing the card that was just deleted
                if (hfCardId.Value == itemId.ToString(CultureInfo.InvariantCulture))
                {
                    pnlCardForm.Visible = false;
                    hfCardId.Value = "0";
                }

                BindCardsGrid();
                ShowMessage("تم حذف البطاقة بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.DeleteCard", ex.Message);
                ShowError("فشل حذف البطاقة: " + Enc(ex.Message));
            }
        }

        private void ToggleCardStatus(int itemId)
        {
            try
            {
                UpdateCard(itemId, item =>
                {
                    item["IsActive"] = !HomeNoraFutureRepository.SafeBool(item, "IsActive", true);
                    item.Update();
                });

                BindCardsGrid();
                ShowMessage("تم تغيير حالة البطاقة بنجاح.");
            }
            catch (ValidationException vex)
            {
                ShowError(Enc(vex.Message));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "ucHomeNoraFutureAdmin.ToggleCardStatus", ex.Message);
                ShowError("فشل تغيير حالة البطاقة: " + Enc(ex.Message));
            }
        }

        /// <summary>Runs a write against one card (elevated) and refreshes the home page cache.</summary>
        private static void UpdateCard(int itemId, Action<SPListItem> change)
        {
            Guid siteId = SPContext.Current.Site.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                {
                    if (web == null) throw new ValidationException(AdminWebMissing);

                    SPList list = web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameItems);
                    if (list == null) throw new ValidationException("قائمة البطاقات غير موجودة.");

                    SPListItem item = list.GetItemById(itemId);

                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        change(item);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

            HomeNoraFutureRepository.InvalidateCache();
        }

        private int GetMaxOrder()
        {
            int max = 0;
            try
            {
                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameItems);
                        if (list != null && list.ItemCount > 0)
                        {
                            SPQuery query = new SPQuery
                            {
                                RowLimit = 1,
                                Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='False' /></OrderBy>"
                            };
                            SPListItemCollection items = list.GetItems(query);
                            if (items.Count > 0)
                            {
                                max = (int)HomeNoraFutureRepository.SafeDouble(items[0], "ItemOrder", 0);
                            }
                        }
                    }
                });
            }
            catch { }
            return max;
        }

        /// <summary>Returns the list, creating missing lists/fields on this same web if needed.</summary>
        private static SPList GetOrCreateList(SPWeb web, string listName)
        {
            SPList list = web.Lists.TryGetList(listName);
            if (list != null) return list;

            HomeNoraFutureProvisioner.EnsureListsOnWeb(web);
            list = web.Lists.TryGetList(listName);
            if (list == null)
                throw new ValidationException("تعذر إنشاء القائمة " + listName + " في /ar/ContentAdmin.");
            return list;
        }

        #endregion

        #region Actions & Filters

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            HomeNoraFutureRepository.InvalidateCache();
            LoadTitleSettings();
            BindCardsGrid();
            ShowMessage("تم تحديث البيانات بنجاح.");
        }

        protected void btnProvision_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            try
            {
                bool ok = HomeNoraFutureProvisioner.ForceProvision();
                LoadTitleSettings();
                BindCardsGrid();

                if (ok)
                    ShowMessage("تم التحقق من القوائم والحقول وإنشاء الناقص منها بنجاح.");
                else
                    ShowError("تعذرت تهيئة القوائم. تأكد من وجود موقع /ar/ContentAdmin وراجع سجل الأخطاء.");
            }
            catch (Exception ex)
            {
                ShowError("فشلت التهيئة: " + Enc(ex.Message));
            }
        }

        protected void btnSearchCards_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindCardsGrid();
        }

        protected void btnResetSearch_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;

            txtSearchCards.Text = "";
            BindCardsGrid();
        }

        #endregion

        #region Rendering Helpers

        public string FormatIconClass(object iconObj)
        {
            string icon = iconObj != null ? iconObj.ToString().Trim() : "";
            string css;
            if (string.IsNullOrEmpty(icon)) css = "hgi hgi-stroke fs-5 hgi-book-01";
            else if (icon.Contains("hgi-") && !icon.Contains("hgi ")) css = "hgi hgi-stroke fs-5 " + icon;
            else css = icon;
            return HttpUtility.HtmlAttributeEncode(css);
        }

        public static string Enc(object value)
        {
            return HttpUtility.HtmlEncode(value != null ? value.ToString() : "");
        }

        public static string SafeHref(object value)
        {
            string url = value != null ? value.ToString().Trim() : "";
            return HttpUtility.HtmlAttributeEncode(HomeNoraFutureRepository.IsSafeUrl(url) ? url : "#");
        }

        #endregion

        #region UI Message Helpers

        // Messages are rendered as HTML — encode any list/user/exception text before passing it in
        private void ShowMessage(string message)
        {
            ltMessage.Text = message;
            pnlMessage.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string error)
        {
            ltError.Text = error;
            pnlError.Visible = true;
            pnlMessage.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlMessage.Visible = false;
            pnlError.Visible = false;
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

        /// <summary>User-facing validation error (message is shown as-is, not logged).</summary>
        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        #endregion
    }
}
