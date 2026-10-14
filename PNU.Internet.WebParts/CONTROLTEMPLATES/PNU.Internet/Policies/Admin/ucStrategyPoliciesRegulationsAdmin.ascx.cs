using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Policies.Admin
{
    public partial class ucStrategyPoliciesRegulationsAdmin : UserControl
    {
        private bool _isAdmin;
        private string _initError;

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
                    StrategyPoliciesProvisioner.EnsureLists();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.OnInit", ex.Message);
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
                BindSectionsGrid();
            }

            if (!string.IsNullOrEmpty(_initError))
            {
                ShowError(_initError);
            }
        }

        #region Tab Switching

        protected void btnTabCards_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            phTabCards.Visible = true;
            phTabSections.Visible = false;
            btnTabCards.CssClass = "nav-link active px-4 py-2 rounded-3";
            btnTabSections.CssClass = "nav-link px-4 py-2 rounded-3";
            ClearAlerts();
            BindCardsGrid();
        }

        protected void btnTabSections_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            phTabCards.Visible = false;
            phTabSections.Visible = true;
            btnTabCards.CssClass = "nav-link px-4 py-2 rounded-3";
            btnTabSections.CssClass = "nav-link active px-4 py-2 rounded-3";
            ClearAlerts();
            BindSectionsGrid();
        }

        #endregion

        #region Cards Management

        protected void ddlFilterSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            BindCardsGrid();
        }

        private void BindCardsGrid()
        {
            try
            {
                List<StrategyPoliciesCard> cards = new List<StrategyPoliciesCard>();
                string filter = ddlFilterSection.SelectedValue;
                string secKey = filter == "ALL" ? null : filter;

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                        if (list != null)
                        {
                            cards = StrategyPoliciesProvisioner.ReadCards(list, isArabic: true, sectionKey: secKey, onlyVisible: false);
                        }
                    }
                });

                rptCardsAdmin.DataSource = cards;
                rptCardsAdmin.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تحميل البطاقات: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.BindCardsGrid", ex.Message);
            }
        }

        protected void btnAddNewCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            hfCardId.Value = "0";
            ltCardFormTitle.Text = "إضافة بطاقة جديدة";

            if (ddlFilterSection.SelectedValue != "ALL")
            {
                ddlCardSection.SelectedValue = ddlFilterSection.SelectedValue;
            }
            else
            {
                ddlCardSection.SelectedIndex = 0;
            }

            txtCardTitleAr.Text = "";
            txtCardTitleEn.Text = "";
            txtCardUrlAr.Text = "";
            txtCardUrlEn.Text = "";
            txtCardBtnAr.Text = "عرض التفاصيل";
            txtCardBtnEn.Text = "View Details";
            txtCardBadgeAr.Text = "";
            txtCardBadgeEn.Text = "";
            txtCardBadgeClass.Text = "badge-warning";
            txtCardIcon.Text = "hgi hgi-stroke hgi-file-02";
            ddlCardTarget.SelectedValue = "_blank";
            txtCardOrder.Text = "1";
            chkCardVisibility.Checked = true;

            pnlCardForm.Visible = true;
        }

        protected void btnCancelCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            pnlCardForm.Visible = false;
            hfCardId.Value = "0";
            ClearAlerts();
        }

        protected void btnSaveCard_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();

            string titleAr = txtCardTitleAr.Text.Trim();
            string titleEn = txtCardTitleEn.Text.Trim();
            string sectionKey = ddlCardSection.SelectedValue;
            string urlAr = txtCardUrlAr.Text.Trim();
            string urlEn = txtCardUrlEn.Text.Trim();
            string btnAr = txtCardBtnAr.Text.Trim();
            string btnEn = txtCardBtnEn.Text.Trim();
            string badgeAr = txtCardBadgeAr.Text.Trim();
            string badgeEn = txtCardBadgeEn.Text.Trim();
            string badgeClass = txtCardBadgeClass.Text.Trim();
            string icon = txtCardIcon.Text.Trim();
            string target = SafeHtml.Target(ddlCardTarget.SelectedValue);
            bool visible = chkCardVisibility.Checked;

            int cardId = 0;
            int.TryParse(hfCardId.Value, out cardId);

            double order;
            if (!double.TryParse(txtCardOrder.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out order))
                order = 1;

            try
            {
                if (string.IsNullOrEmpty(titleAr))
                    throw new Exception("الرجاء إدخال عنوان البطاقة بالعربية.");
                if (string.IsNullOrEmpty(urlAr))
                    throw new Exception("الرجاء إدخال رابط البطاقة بالعربية.");
                if (!SafeHtml.IsSafeUrl(urlAr))
                    throw new Exception("رابط البطاقة بالعربية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");
                if (!string.IsNullOrEmpty(urlEn) && !SafeHtml.IsSafeUrl(urlEn))
                    throw new Exception("رابط البطاقة بالإنجليزية غير صالح. يجب أن يبدأ بـ / أو http:// أو https://");
                if (string.IsNullOrEmpty(icon))
                    throw new Exception("الرجاء تحديد فئة الأيقونة.");

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new Exception("تعذر الوصول إلى الموقع المستهدف.");

                        SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                        if (list == null)
                        {
                            StrategyPoliciesProvisioner.EnsureListsOnWeb(web);
                            list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                        }

                        if (list == null) throw new Exception("تعذر العثور على قائمة البطاقات.");

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item = cardId > 0 ? list.GetItemById(cardId) : list.AddItem();
                            item["Title"] = titleAr;
                            item["TitleEn"] = titleEn;
                            item["SectionKey"] = sectionKey;
                            item["Url"] = urlAr;
                            item["UrlEn"] = urlEn;
                            item["ButtonText"] = btnAr;
                            item["ButtonTextEn"] = btnEn;
                            item["Badge"] = badgeAr;
                            item["BadgeEn"] = badgeEn;
                            item["BadgeClass"] = badgeClass;
                            item["Icon"] = icon;
                            item["Target"] = target;
                            item["SortOrder"] = order;
                            item["Visibility"] = visible;
                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                pnlCardForm.Visible = false;
                hfCardId.Value = "0";
                ShowSuccess(cardId > 0 ? "تم تحديث البطاقة بنجاح." : "تمت إضافة البطاقة بنجاح.");
                BindCardsGrid();
            }
            catch (Exception ex)
            {
                ShowError("تعذر حفظ البطاقة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.btnSaveCard_Click", ex.Message);
            }
        }

        protected void rptCardsAdmin_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            int id = 0;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

            try
            {
                if (e.CommandName == "EditCard")
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                            if (list == null) return;

                            SPListItem item = list.GetItemById(id);
                            var card = StrategyPoliciesProvisioner.MapCardItem(item, true);

                            hfCardId.Value = card.ID.ToString();
                            ltCardFormTitle.Text = "تعديل البطاقة: " + SafeHtml.Text(card.Title);
                            SelectIfExists(ddlCardSection, (card.SectionKey ?? "").Trim().ToLowerInvariant());
                            txtCardTitleAr.Text = card.Title;
                            txtCardTitleEn.Text = card.TitleEn;
                            txtCardUrlAr.Text = card.Url;
                            txtCardUrlEn.Text = card.UrlEn;
                            txtCardBtnAr.Text = card.ButtonText;
                            txtCardBtnEn.Text = card.ButtonTextEn;
                            txtCardBadgeAr.Text = card.Badge;
                            txtCardBadgeEn.Text = card.BadgeEn;
                            txtCardBadgeClass.Text = card.BadgeClass;
                            txtCardIcon.Text = card.Icon;
                            SelectIfExists(ddlCardTarget, SafeHtml.Target(card.Target));
                            txtCardOrder.Text = card.SortOrder.ToString(CultureInfo.InvariantCulture);
                            chkCardVisibility.Checked = card.Visibility;

                            pnlCardForm.Visible = true;
                        }
                    });
                }
                else if (e.CommandName == "ToggleCard")
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                            if (list == null) return;

                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPListItem item = list.GetItemById(id);
                                bool current = item.Fields.ContainsField("Visibility") && item["Visibility"] != null
                                    ? Convert.ToBoolean(item["Visibility"])
                                    : true;
                                item["Visibility"] = !current;
                                item.Update();
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });
                    ShowSuccess("تم تغيير حالة البطاقة بنجاح.");
                    BindCardsGrid();
                }
                else if (e.CommandName == "DeleteCard")
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                            if (list == null) return;

                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPListItem item = list.GetItemById(id);
                                item.Delete();
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });
                    ShowSuccess("تم حذف البطاقة بنجاح.");
                    BindCardsGrid();
                }
                else if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown")
                {
                    SwapCardOrder(id, e.CommandName == "MoveUp");
                    BindCardsGrid();
                }
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تنفيذ الإجراء: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.rptCardsAdmin_ItemCommand", ex.Message);
            }
        }

        private void SwapCardOrder(int cardId, bool moveUp)
        {
            Guid siteId = SPContext.Current.Site.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                {
                    if (web == null) return;
                    SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListCards);
                    if (list == null) return;

                    SPListItem currentItem = list.GetItemById(cardId);
                    string sectionKey = Convert.ToString(currentItem["SectionKey"]);
                    var allInSec = StrategyPoliciesProvisioner.ReadCards(list, true, sectionKey, false);

                    int idx = allInSec.FindIndex(c => c.ID == cardId);
                    if (idx < 0) return;

                    int targetIdx = moveUp ? idx - 1 : idx + 1;
                    if (targetIdx < 0 || targetIdx >= allInSec.Count) return;

                    // Swap positions, then renumber the section 1..n so duplicate orders cannot collide
                    var moved = allInSec[idx];
                    allInSec[idx] = allInSec[targetIdx];
                    allInSec[targetIdx] = moved;

                    web.AllowUnsafeUpdates = true;
                    try
                    {
                        for (int i = 0; i < allInSec.Count; i++)
                        {
                            double newOrder = i + 1;
                            if (Math.Abs(allInSec[i].SortOrder - newOrder) < 0.001) continue;

                            SPListItem item = list.GetItemById(allInSec[i].ID);
                            item["SortOrder"] = newOrder;
                            item.Update();
                        }
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #endregion

        #region Sections Management

        private void BindSectionsGrid()
        {
            try
            {
                List<StrategyPoliciesSection> sections = new List<StrategyPoliciesSection>();

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) return;
                        SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListSections);
                        if (list != null)
                        {
                            sections = StrategyPoliciesProvisioner.ReadSections(list, isArabic: true, onlyVisible: false);
                        }
                    }
                });

                rptSectionsAdmin.DataSource = sections;
                rptSectionsAdmin.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تحميل الأقسام: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.BindSectionsGrid", ex.Message);
            }
        }

        protected void btnCancelSec_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            pnlSectionForm.Visible = false;
            hfSectionId.Value = "0";
            ClearAlerts();
        }

        protected void btnSaveSec_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();

            int secId = 0;
            int.TryParse(hfSectionId.Value, out secId);
            if (secId <= 0) return;

            string titleAr = txtSecTitleAr.Text.Trim();
            string titleEn = txtSecTitleEn.Text.Trim();
            string descAr = txtSecDescAr.Text.Trim();
            string descEn = txtSecDescEn.Text.Trim();
            bool visible = chkSecVisibility.Checked;

            double order;
            if (!double.TryParse(txtSecOrder.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out order))
                order = 1;

            try
            {
                if (string.IsNullOrEmpty(titleAr))
                    throw new Exception("الرجاء إدخال عنوان القسم بالعربية.");

                Guid siteId = SPContext.Current.Site.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                    {
                        if (web == null) throw new Exception("تعذر الوصول إلى الموقع المستهدف.");
                        SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListSections);
                        if (list == null) throw new Exception("تعذر العثور على قائمة الأقسام.");

                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem item = list.GetItemById(secId);
                            item["Title"] = titleAr;
                            item["TitleEn"] = titleEn;
                            item["Description"] = descAr;
                            item["DescriptionEn"] = descEn;
                            item["ItemOrder"] = order;
                            item["Visibility"] = visible;
                            item.Update();
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                pnlSectionForm.Visible = false;
                hfSectionId.Value = "0";
                ShowSuccess("تم تحديث بيانات القسم بنجاح.");
                BindSectionsGrid();
            }
            catch (Exception ex)
            {
                ShowError("تعذر حفظ بيانات القسم: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.btnSaveSec_Click", ex.Message);
            }
        }

        protected void rptSectionsAdmin_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            int id = 0;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;

            try
            {
                if (e.CommandName == "EditSection")
                {
                    Guid siteId = SPContext.Current.Site.ID;
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(siteId))
                        using (SPWeb web = StrategyPoliciesProvisioner.OpenTargetWeb(site))
                        {
                            if (web == null) return;
                            SPList list = web.Lists.TryGetList(StrategyPoliciesProvisioner.ListSections);
                            if (list == null) return;

                            SPListItem item = list.GetItemById(id);
                            var sec = StrategyPoliciesProvisioner.MapSectionItem(item, true);

                            hfSectionId.Value = sec.ID.ToString();
                            txtSecKey.Text = sec.SectionKey;
                            txtSecTitleAr.Text = sec.Title;
                            txtSecTitleEn.Text = sec.TitleEn;
                            txtSecDescAr.Text = sec.Description;
                            txtSecDescEn.Text = sec.DescriptionEn;
                            txtSecOrder.Text = sec.ItemOrder.ToString(CultureInfo.InvariantCulture);
                            chkSecVisibility.Checked = sec.Visibility;

                            pnlSectionForm.Visible = true;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء تحميل بيانات القسم: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.rptSectionsAdmin_ItemCommand", ex.Message);
            }
        }

        #endregion

        #region Actions & Helpers

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            BindCardsGrid();
            BindSectionsGrid();
            ShowSuccess("تم تحديث البيانات.");
        }

        protected void btnProvision_Click(object sender, EventArgs e)
        {
            if (!_isAdmin) return;
            ClearAlerts();
            try
            {
                bool ok = StrategyPoliciesProvisioner.ForceProvision();
                if (ok)
                {
                    ShowSuccess("تم التحقق من وجود القوائم وتهيئتها بنجاح.");
                    BindCardsGrid();
                    BindSectionsGrid();
                }
                else
                {
                    ShowError("تعذر إتمام عملية التهيئة. يرجى مراجعة سجل النظام.");
                }
            }
            catch (Exception ex)
            {
                ShowError("حدث خطأ أثناء التهيئة: " + SafeHtml.Text(ex.Message));
                Publics.WriteToLog(CurrentUrl(), "ucStrategyPoliciesRegulationsAdmin.btnProvision_Click", ex.Message);
            }
        }

        private void ShowSuccess(string msg)
        {
            ltSuccessMsg.Text = msg;
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string msg)
        {
            ltErrorMsg.Text = msg;
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
        }

        private void ClearAlerts()
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
            ltSuccessMsg.Text = "";
            ltErrorMsg.Text = "";
        }

        private string CurrentUrl()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    return HttpContext.Current.Request.Url.ToString();
            }
            catch { }
            return "ucStrategyPoliciesRegulationsAdmin";
        }

        private static void SelectIfExists(DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            ListItem li = ddl.Items.FindByValue(value ?? "");
            if (li != null) li.Selected = true;
            else if (ddl.Items.Count > 0) ddl.SelectedIndex = 0;
        }

        public static string RenderBadge(object badgeText, object badgeClass)
        {
            return ucStrategyPoliciesRegulations.RenderBadge(badgeText, badgeClass);
        }

        public static string Text(object value) { return SafeHtml.Text(value); }
        public static string Attr(object value) { return SafeHtml.Attr(value); }
        public static string Href(object value) { return SafeHtml.Href(value); }

        public static string GetSectionDisplayName(object sectionKey)
        {
            string key = Convert.ToString(sectionKey) ?? "";
            switch (key.ToLower())
            {
                case "strategies": return "الاستراتيجيات";
                case "policies": return "السياسات";
                case "regulations": return "الأنظمة واللوائح";
                default: return key;
            }
        }

        public static string GetSectionBadgeClass(object sectionKey)
        {
            string key = Convert.ToString(sectionKey) ?? "";
            switch (key.ToLower())
            {
                case "strategies": return "bg-primary-subtle text-primary border";
                case "policies": return "bg-success-subtle text-success border";
                case "regulations": return "bg-info-subtle text-info border";
                default: return "bg-light text-muted border";
            }
        }

        #endregion
    }
}
