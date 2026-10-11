using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ContentManagement
{
    public partial class ucGenericPageContentEditor : UserControl
    {
        /// <summary>
        /// Fields probed when reading page content, in order of preference. Mirrors the list
        /// used by ucAutoDgaPageConverter so both tools see the same content on PNU pages.
        /// </summary>
        private static readonly string[] ReadableContentFields = new string[]
        {
            "PublishingPageContent",
            "PublishingPageContent_EN",
            "PublishingPageContentHtml",
            "PageContent",
            "Content",
            "Content_EN",
            "Body",
            "ArticleBody",
            "Description",
            "Description_EN",
            "Comments"
        };

        /// <summary>Fields we may write back to. "Comments" is intentionally read-only.</summary>
        private static readonly string[] WritableContentFields = new string[]
        {
            "PublishingPageContent",
            "PublishingPageContent_EN",
            "PublishingPageContentHtml",
            "PageContent",
            "Content",
            "Content_EN",
            "Body",
            "ArticleBody"
        };

        /// <summary>Marks content that lives in a page field: "field:&lt;InternalName&gt;".</summary>
        private const string SourceFieldPrefix = "field:";

        /// <summary>Marks content that lives in a Content Editor Web Part: "cewp:&lt;WebPartId&gt;".</summary>
        private const string SourceCewpPrefix = "cewp:";

        /// <summary>Minimum rights required on the target page before we touch it.</summary>
        private const SPBasePermissions RequiredPageRights = SPBasePermissions.EditListItems;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ContentAdm.IsAuthenticated())
            {
                ContentAdm.PromptLogin(Page);
                pnlEditorArea.Visible = false;
                ShowMessage("يرجى تسجيل الدخول للوصول إلى محرر محتوى الصفحات.", "alert-warning-pnu");
                return;
            }

            if (!IsPostBack)
            {
                string qPage = Request.QueryString["targetPage"];
                if (!string.IsNullOrEmpty(qPage))
                {
                    txtPageUrl.Text = qPage;
                    // GET request: never mutate. SharePoint blocks updates on GET, and an
                    // auto check-out here would be triggerable by a crafted link.
                    LoadPageContent(qPage, null, false);
                }
            }
        }

        protected void btnLoadPage_Click(object sender, EventArgs e)
        {
            string pageUrl = txtPageUrl.Text.Trim();
            if (string.IsNullOrEmpty(pageUrl))
            {
                ShowMessage("يرجى إدخال رابط الصفحة أولاً.", "alert-warning-pnu");
                return;
            }

            LoadPageContent(pageUrl, null, true);
        }

        protected void btnLoadVersion_Click(object sender, EventArgs e)
        {
            string pageUrl = txtPageUrl.Text.Trim();
            if (string.IsNullOrEmpty(pageUrl)) return;

            LoadPageContent(pageUrl, ddlPageVersions.SelectedValue, true);
        }

        private void LoadPageContent(string inputUrl, string targetVersionLabel, bool allowCheckOut)
        {
            SPWeb targetWeb = null;
            try
            {
                if (allowCheckOut)
                {
                    SPUtility.ValidateFormDigest();
                }

                string serverRelativeUrl = NormalizeServerRelativeUrl(inputUrl);
                SPSite site = SPContext.Current.Site;

                SPFile file = GetTargetFile(site, serverRelativeUrl, out targetWeb);

                if (file == null || !file.Exists || targetWeb == null || !targetWeb.Exists)
                {
                    ShowMessage("الصفحة المطلوبة غير موجودة على الموقع أو لا تملك صلاحية الوصول إليها: " + serverRelativeUrl, "alert-danger-pnu");
                    pnlEditorArea.Visible = false;
                    return;
                }

                SPUser currentUser = targetWeb.CurrentUser;

                // 1. Verify the caller may actually edit this specific page
                if (!UserCanEditPage(file))
                {
                    ShowMessage("عذراً، لا تملك صلاحية تعديل محتوى هذه الصفحة.", "alert-danger-pnu");
                    pnlEditorArea.Visible = false;
                    return;
                }

                // 2. Verify check-out status
                bool isCheckedOutToMe = false;
                if (file.CheckOutType != SPFile.SPCheckOutType.None)
                {
                    if (file.CheckedOutByUser != null && currentUser != null && file.CheckedOutByUser.ID != currentUser.ID)
                    {
                        ShowMessage(string.Format("عذراً! الصفحة مسحوبة حالياً بواسطة ({0}) ولا يمكنك التعديل عليها حتى يقوم بإلغاء السحب أو تسجيل التغييرات.", file.CheckedOutByUser.Name), "alert-warning-pnu");
                        pnlEditorArea.Visible = false;
                        return;
                    }
                    isCheckedOutToMe = true;
                }
                else if (allowCheckOut)
                {
                    // 3. Check the page out to the current editor (postback only)
                    SPFile toCheckOut = file;
                    WithUnsafeUpdates(targetWeb, delegate { toCheckOut.CheckOut(); });
                    isCheckedOutToMe = true;
                }

                // 4. Extract content and populate version history
                SPListItem item = file.Item;
                string detectedField;
                string resolvedVersionLabel;
                bool versionRequestSatisfied;
                string contentSource;
                string pageContent = ExtractContentFromItem(item, file, targetVersionLabel,
                    out resolvedVersionLabel, out detectedField, out versionRequestSatisfied, out contentSource);

                hfContentSource.Value = contentSource ?? string.Empty;

                PopulateVersionDropdown(item, file, targetVersionLabel);

                lblCurrentPageTitle.InnerText = (item != null && item.Title != null) ? item.Title : file.Name;
                lblCurrentWebInfo.InnerText = string.Format("{0} ({1})", targetWeb.Title, targetWeb.ServerRelativeUrl);
                lblDetectedField.InnerText = detectedField;
                lblCurrentVersionBadge.InnerText = resolvedVersionLabel;
                hfPageRawContent.Value = pageContent ?? string.Empty;
                txtSourceCode.Text = pageContent ?? string.Empty;

                RenderCheckOutBadge(isCheckedOutToMe);
                btnDiscardCheckOut.Visible = isCheckedOutToMe;
                pnlEditorArea.Visible = true;

                string message;
                string alertClass = "alert-success-pnu";

                if (isCheckedOutToMe)
                {
                    message = string.Format("تم جلب محتوى [{0}] والصفحة مسحوبة باسمك ({1}). المحتوى جاهز للتعديل.",
                        resolvedVersionLabel, currentUser != null ? currentUser.Name : string.Empty);
                }
                else
                {
                    message = string.Format("تم عرض محتوى [{0}] للاطلاع فقط. سيتم سحب الصفحة باسمك تلقائياً عند أول عملية حفظ.", resolvedVersionLabel);
                    alertClass = "alert-warning-pnu";
                }

                // Warn when the editor is not showing the newest working copy: saving would overwrite it.
                if (string.IsNullOrEmpty(targetVersionLabel) && !string.IsNullOrEmpty(file.UIVersionLabel)
                    && resolvedVersionLabel.IndexOf(file.UIVersionLabel, StringComparison.Ordinal) < 0)
                {
                    message += string.Format(" تنبيه: توجد نسخة أحدث ({0}) لم يتم جلبها، والحفظ الآن سيحل محلها.", file.UIVersionLabel);
                    alertClass = "alert-warning-pnu";
                }

                if (!string.IsNullOrEmpty(targetVersionLabel) && !versionRequestSatisfied)
                {
                    message = string.Format("تعذر العثور على محتوى للإصدار المطلوب ({0})، تم عرض [{1}] بدلاً منه.", targetVersionLabel, resolvedVersionLabel);
                    alertClass = "alert-warning-pnu";
                }

                // An empty editor is almost always a content-location problem, not an empty page.
                if (string.IsNullOrWhiteSpace(pageContent))
                {
                    message = "لم يتم العثور على محتوى قابل للتحرير في هذه الصفحة. قد يكون محتوى الصفحة مبنياً داخل تخطيط الصفحة (Page Layout) أو عبر أدوات ويب مخصصة لا يمكن تحريرها من هنا.";
                    alertClass = "alert-warning-pnu";
                }

                // Pages built from a ControlLoaderWebPart render from list data, not from the
                // page field. Without this warning the editor shows stale field HTML that looks
                // editable but has no effect on what visitors see.
                string renderingControls = DetectRenderingControls(file);
                if (!string.IsNullOrEmpty(renderingControls))
                {
                    message += string.Format(
                        " || تنبيه هام: هذه الصفحة تعرض محتواها عبر وحدة تحكم مخصصة ({0}) تقرأ بياناتها من قوائم SharePoint، وليس من حقل محتوى الصفحة. المحتوى المعروض هنا قد يكون قديماً وغير ظاهر للزوار، وأي تعديل يتم حفظه من هذه الشاشة لن ينعكس على الصفحة. يجب تعديل البيانات من شاشة الإدارة الخاصة بالوحدة.",
                        renderingControls);
                    alertClass = "alert-warning-pnu";
                }

                ShowMessage(message, alertClass);
                RegisterEditorInitScript();
            }
            catch (Exception ex)
            {
                LogError("LoadPageContent", ex);
                ShowMessage("حدث خطأ أثناء جلب الصفحة: " + ex.Message, "alert-danger-pnu");
                pnlEditorArea.Visible = false;
            }
            finally
            {
                DisposeIfOwned(targetWeb);
            }
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SPWeb targetWeb = null;
            try
            {
                SPUtility.ValidateFormDigest();

                string serverRelativeUrl = NormalizeServerRelativeUrl(txtPageUrl.Text.Trim());
                if (string.IsNullOrEmpty(serverRelativeUrl))
                {
                    ShowMessage("يرجى إدخال رابط الصفحة أولاً.", "alert-warning-pnu");
                    return;
                }

                SPSite site = SPContext.Current.Site;
                SPFile file = GetTargetFile(site, serverRelativeUrl, out targetWeb);

                if (file == null || !file.Exists || targetWeb == null || !targetWeb.Exists)
                {
                    ShowMessage("تعذر الوصول إلى الصفحة المطلوبة للحفظ: " + serverRelativeUrl, "alert-danger-pnu");
                    return;
                }

                if (!UserCanEditPage(file))
                {
                    ShowMessage("عذراً، لا تملك صلاحية تعديل محتوى هذه الصفحة.", "alert-danger-pnu");
                    return;
                }

                SPUser currentUser = targetWeb.CurrentUser;

                // The page must be checked out to the current user before we write anything.
                if (file.CheckOutType != SPFile.SPCheckOutType.None)
                {
                    if (file.CheckedOutByUser != null && currentUser != null && file.CheckedOutByUser.ID != currentUser.ID)
                    {
                        ShowMessage(string.Format("عذراً! الصفحة مسحوبة حالياً بواسطة ({0}) ولا يمكن حفظ تعديلاتك.", file.CheckedOutByUser.Name), "alert-danger-pnu");
                        return;
                    }
                }
                else
                {
                    SPFile toCheckOut = file;
                    WithUnsafeUpdates(targetWeb, delegate { toCheckOut.CheckOut(); });
                }

                string content = GetSubmittedContent();
                string contentSource = hfContentSource.Value;
                SPFile savedFile = file;
                bool minorVersionsEnabled = false;
                string writtenField = null;

                WithUnsafeUpdates(targetWeb, delegate
                {
                    SPListItem item = savedFile.Item;
                    writtenField = SaveContentToItem(item, savedFile, content, contentSource);
                    if (writtenField == null) return;

                    SPList parentList = (item != null) ? item.ParentList : null;
                    minorVersionsEnabled = parentList != null && parentList.EnableMinorVersions;

                    if (minorVersionsEnabled)
                    {
                        // A minor check-in creates a real draft version that stays invisible to public readers.
                        savedFile.CheckIn("حفظ مسودة عبر محرر محتوى صفحات البوابة", SPCheckinType.MinorCheckIn);

                        // Re-open it so the editor can keep working without reloading. Re-fetch
                        // first: the SPFile instance is stale immediately after a check-in.
                        SPFile reopened = targetWeb.GetFile(savedFile.ServerRelativeUrl);
                        if (reopened != null && reopened.Exists && reopened.CheckOutType == SPFile.SPCheckOutType.None)
                        {
                            reopened.CheckOut();
                        }
                    }
                });

                if (writtenField == null)
                {
                    ShowMessage("تعذر تحديد حقل المحتوى المناسب في هذه الصفحة، لم يتم حفظ أي تغييرات.", "alert-danger-pnu");
                    return;
                }

                // Refresh the editor from what was actually persisted.
                SPFile refreshed = targetWeb.GetFile(savedFile.ServerRelativeUrl);
                if (refreshed != null && refreshed.Exists && refreshed.Item != null)
                {
                    PopulateVersionDropdown(refreshed.Item, refreshed, null);
                    lblCurrentVersionBadge.InnerText = "الإصدار " + refreshed.UIVersionLabel;
                }

                lblDetectedField.InnerText = writtenField;
                hfPageRawContent.Value = content;
                txtSourceCode.Text = content;

                RenderCheckOutBadge(true);
                btnDiscardCheckOut.Visible = true;
                pnlEditorArea.Visible = true;

                string savedMessage = minorVersionsEnabled
                    ? string.Format("تم حفظ التعديلات كمسودة (الإصدار {0}) في الحقل [{1}] دون نشرها للعامة، والصفحة ما زالت مسحوبة باسمك.",
                        refreshed != null ? refreshed.UIVersionLabel : string.Empty, writtenField)
                    : string.Format("تم حفظ التعديلات في الحقل [{0}]. المكتبة لا تدعم الإصدارات الفرعية، لذا بقيت الصفحة مسحوبة باسمك ولن تظهر التغييرات للعامة حتى يتم تسجيل دخول الصفحة (Check-In).", writtenField);

                ShowMessage(savedMessage, "alert-success-pnu");
                RegisterEditorInitScript();
            }
            catch (Exception ex)
            {
                LogError("btnSaveDraft_Click", ex);
                ShowMessage("حدث خطأ أثناء حفظ التعديلات: " + ex.Message, "alert-danger-pnu");
            }
            finally
            {
                DisposeIfOwned(targetWeb);
            }
        }

        protected void btnDiscardCheckOut_Click(object sender, EventArgs e)
        {
            SPWeb targetWeb = null;
            try
            {
                SPUtility.ValidateFormDigest();

                string serverRelativeUrl = NormalizeServerRelativeUrl(txtPageUrl.Text.Trim());
                SPSite site = SPContext.Current.Site;

                SPFile file = GetTargetFile(site, serverRelativeUrl, out targetWeb);

                if (file == null || !file.Exists || targetWeb == null)
                {
                    ShowMessage("تعذر الوصول إلى الصفحة المطلوبة.", "alert-danger-pnu");
                    return;
                }

                if (!UserCanEditPage(file))
                {
                    ShowMessage("عذراً، لا تملك صلاحية التعديل على هذه الصفحة.", "alert-danger-pnu");
                    return;
                }

                if (file.CheckOutType == SPFile.SPCheckOutType.None)
                {
                    ShowMessage("الصفحة ليست مسحوبة للتراجع عنها.", "alert-warning-pnu");
                    return;
                }

                // Never discard somebody else's work in progress.
                SPUser currentUser = targetWeb.CurrentUser;
                if (file.CheckedOutByUser != null && currentUser != null && file.CheckedOutByUser.ID != currentUser.ID)
                {
                    ShowMessage(string.Format("لا يمكنك إلغاء سحب الصفحة لأنها مسحوبة بواسطة ({0}).", file.CheckedOutByUser.Name), "alert-danger-pnu");
                    return;
                }

                SPFile toUndo = file;
                WithUnsafeUpdates(targetWeb, delegate { toUndo.UndoCheckOut(); });

                pnlEditorArea.Visible = false;
                ShowMessage("تم إلغاء سحب الصفحة والتراجع عن التعديلات بنجاح.", "alert-success-pnu");
            }
            catch (Exception ex)
            {
                LogError("btnDiscardCheckOut_Click", ex);
                ShowMessage("حدث خطأ أثناء إلغاء السحب: " + ex.Message, "alert-danger-pnu");
            }
            finally
            {
                DisposeIfOwned(targetWeb);
            }
        }

        /// <summary>
        /// Reads the HTML the browser posted back. The hidden field is the canonical carrier;
        /// the source textarea is the fallback when scripting did not run.
        /// </summary>
        private string GetSubmittedContent()
        {
            string content = hfPageRawContent.Value;
            if (string.IsNullOrEmpty(content))
            {
                content = txtSourceCode.Text;
            }
            return content ?? string.Empty;
        }

        private void RenderCheckOutBadge(bool isCheckedOutToMe)
        {
            if (spCheckOutBadge == null) return;

            if (isCheckedOutToMe)
            {
                spCheckOutBadge.Attributes["class"] = "badge bg-success-subtle text-success border border-success-subtle ms-1";
                spCheckOutBadge.InnerHtml = "<i class=\"hgi hgi-stroke hgi-lock-unlocked me-1\"></i>مسحوبة باسمك (جاهزة للتعديل)";
            }
            else
            {
                spCheckOutBadge.Attributes["class"] = "badge bg-warning-subtle text-warning-emphasis border border-warning-subtle ms-1";
                spCheckOutBadge.InnerHtml = "<i class=\"hgi hgi-stroke hgi-lock me-1\"></i>غير مسحوبة (للاطلاع فقط حتى الحفظ)";
            }
        }

        private void RegisterEditorInitScript()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "InitTinyMce",
                "if(typeof initTinyMceEditor === 'function') { initTinyMceEditor(); }", true);
        }

        private void PopulateVersionDropdown(SPListItem item, SPFile file, string activeVersionLabel)
        {
            try
            {
                ddlPageVersions.Items.Clear();
                if (item == null) return;

                bool anySelected = false;

                if (item.Versions != null && item.Versions.Count > 0)
                {
                    foreach (SPListItemVersion v in item.Versions)
                    {
                        bool isMajor = IsMajorVersion(v.VersionLabel);
                        string badge = isMajor ? "[إصدار رئيسي]" : "[مسودة]";
                        string author = (v.CreatedBy != null && v.CreatedBy.User != null) ? v.CreatedBy.User.Name : "";
                        string text = string.Format("الإصدار {0} {1} — {2:yyyy/MM/dd HH:mm} — {3}", v.VersionLabel, badge, v.Created, author);

                        var listItem = new System.Web.UI.WebControls.ListItem(text, v.VersionLabel);

                        if (!anySelected)
                        {
                            if (!string.IsNullOrEmpty(activeVersionLabel))
                            {
                                if (string.Equals(v.VersionLabel, activeVersionLabel, StringComparison.OrdinalIgnoreCase))
                                {
                                    listItem.Selected = true;
                                    anySelected = true;
                                }
                            }
                            else if (isMajor)
                            {
                                listItem.Selected = true;
                                anySelected = true;
                            }
                        }

                        ddlPageVersions.Items.Add(listItem);
                    }
                }

                if (!anySelected && ddlPageVersions.Items.Count > 0)
                {
                    ddlPageVersions.SelectedIndex = 0;
                }

                if (ddlPageVersions.Items.Count == 0 && file != null)
                {
                    string text = string.Format("الإصدار الحالي ({0})", file.UIVersionLabel);
                    ddlPageVersions.Items.Add(new System.Web.UI.WebControls.ListItem(text, file.UIVersionLabel));
                }
            }
            catch (Exception ex)
            {
                LogError("PopulateVersionDropdown", ex);
            }
        }

        private string ExtractContentFromItem(SPListItem item, SPFile file, string targetVersionLabel,
            out string resolvedVersionLabel, out string detectedFieldName, out bool versionRequestSatisfied,
            out string contentSource)
        {
            resolvedVersionLabel = "الإصدار الحالي";
            detectedFieldName = "غير محدد";
            contentSource = string.Empty;
            versionRequestSatisfied = string.IsNullOrEmpty(targetVersionLabel);

            if (item == null) return string.Empty;

            // Case A: a specific version was requested. Exact label match only.
            if (!string.IsNullOrEmpty(targetVersionLabel) && item.Versions != null)
            {
                try
                {
                    foreach (SPListItemVersion v in item.Versions)
                    {
                        if (!string.Equals(v.VersionLabel, targetVersionLabel, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        foreach (string fieldName in ReadableContentFields)
                        {
                            if (v.Fields.ContainsField(fieldName) && v[fieldName] != null)
                            {
                                string val = v[fieldName].ToString();
                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    resolvedVersionLabel = "الإصدار " + v.VersionLabel + (IsMajorVersion(v.VersionLabel) ? " (رئيسي)" : " (مسودة)");
                                    detectedFieldName = fieldName;
                                    contentSource = SourceFieldPrefix + fieldName;
                                    versionRequestSatisfied = true;
                                    return val;
                                }
                            }
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    LogError("ExtractContentFromItem/version", ex);
                }
            }
            // Case B: default load. Prefer the latest major version.
            else if (item.Versions != null && item.Versions.Count > 0)
            {
                foreach (SPListItemVersion v in item.Versions)
                {
                    if (!IsMajorVersion(v.VersionLabel)) continue;

                    foreach (string fieldName in ReadableContentFields)
                    {
                        if (v.Fields.ContainsField(fieldName) && v[fieldName] != null)
                        {
                            string val = v[fieldName].ToString();
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                resolvedVersionLabel = "الإصدار " + v.VersionLabel + " (رئيسي)";
                                detectedFieldName = fieldName;
                                contentSource = SourceFieldPrefix + fieldName;
                                return val;
                            }
                        }
                    }
                }
            }

            // Case C: fall back to the current working item.
            foreach (string fieldName in ReadableContentFields)
            {
                if (item.Fields.ContainsField(fieldName) && item[fieldName] != null)
                {
                    string val = item[fieldName].ToString();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        resolvedVersionLabel = (file != null) ? "الإصدار " + file.UIVersionLabel : "النسخة الحالية";
                        detectedFieldName = fieldName;
                        contentSource = SourceFieldPrefix + fieldName;
                        return val;
                    }
                }
            }

            // Case D: go through the publishing API.
            try
            {
                PublishingPage pubPage = PublishingPage.GetPublishingPage(item);
                if (pubPage != null && pubPage.ListItem != null)
                {
                    foreach (string fieldName in ReadableContentFields)
                    {
                        if (pubPage.ListItem.Fields.ContainsField(fieldName) && pubPage.ListItem[fieldName] != null)
                        {
                            string val = pubPage.ListItem[fieldName].ToString();
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                resolvedVersionLabel = "PublishingPage (النسخة الحالية)";
                                detectedFieldName = "PublishingPage." + fieldName;
                                contentSource = SourceFieldPrefix + fieldName;
                                return val;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ExtractContentFromItem/publishing", ex);
            }

            // Case E: many PNU pages carry their body in a Content Editor Web Part rather than
            // in a page field. Read it the same way ucAutoDgaPageConverter does.
            string cewpId;
            string cewpHtml = ReadContentEditorWebPart(file, out cewpId);
            if (!string.IsNullOrWhiteSpace(cewpHtml))
            {
                resolvedVersionLabel = (file != null) ? "الإصدار " + file.UIVersionLabel : "النسخة الحالية";
                detectedFieldName = "Content Editor Web Part";
                contentSource = SourceCewpPrefix + cewpId;
                return cewpHtml;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns a comma-separated list of the user controls the page renders through a
        /// ControlLoaderWebPart, or null when there are none. Those controls build their output
        /// from SharePoint lists, so the page field this editor shows is not what visitors see.
        /// </summary>
        private string DetectRenderingControls(SPFile file)
        {
            if (file == null || !file.Exists) return null;

            try
            {
                System.Collections.Generic.List<string> paths = new System.Collections.Generic.List<string>();

                using (SPLimitedWebPartManager wpManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                    {
                        string typeName = wp.GetType().FullName;
                        if (typeName == null ||
                            typeName.IndexOf("ControlLoaderWebPart", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            continue;
                        }

                        // The web part type lives in another assembly, so read the path reflectively
                        // (same approach as ucBilingualPageCreator).
                        string path = null;
                        System.Reflection.PropertyInfo pathProp = wp.GetType().GetProperty("UserControlPath");
                        if (pathProp != null)
                        {
                            path = Convert.ToString(pathProp.GetValue(wp, null));
                        }

                        if (string.IsNullOrWhiteSpace(path)) path = wp.Title;
                        if (!string.IsNullOrWhiteSpace(path) && !paths.Contains(path))
                        {
                            paths.Add(path);
                        }
                    }
                }

                return paths.Count > 0 ? string.Join("، ", paths.ToArray()) : null;
            }
            catch (Exception ex)
            {
                LogError("DetectRenderingControls", ex);
                return null;
            }
        }

        /// <summary>
        /// Returns the HTML of the first Content Editor Web Part on the page, or null.
        /// <paramref name="webPartId"/> receives the part id so we can write back to the same part.
        /// </summary>
        private string ReadContentEditorWebPart(SPFile file, out string webPartId)
        {
            webPartId = null;
            if (file == null || !file.Exists) return null;

            try
            {
                using (SPLimitedWebPartManager wpManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                    {
                        ContentEditorWebPart cewp = wp as ContentEditorWebPart;
                        if (cewp == null || cewp.Content == null) continue;

                        string xml = cewp.Content.InnerXml;
                        string html = !string.IsNullOrWhiteSpace(xml) ? xml : cewp.Content.InnerText;

                        if (!string.IsNullOrWhiteSpace(html))
                        {
                            // Content stored via InnerText comes back HTML-escaped.
                            if (html.IndexOf('<') < 0 && html.IndexOf("&lt;", StringComparison.Ordinal) >= 0)
                            {
                                html = HttpUtility.HtmlDecode(html);
                            }

                            webPartId = wp.ID;
                            return html;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ReadContentEditorWebPart", ex);
            }

            return null;
        }

        /// <summary>
        /// Writes HTML back into the Content Editor Web Part identified by <paramref name="webPartId"/>.
        /// Falls back to the first CEWP on the page when the id no longer matches.
        /// </summary>
        private bool WriteContentEditorWebPart(SPFile file, string webPartId, string html)
        {
            if (file == null || !file.Exists) return false;

            try
            {
                using (SPLimitedWebPartManager wpManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    ContentEditorWebPart target = null;
                    ContentEditorWebPart firstCewp = null;

                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpManager.WebParts)
                    {
                        ContentEditorWebPart cewp = wp as ContentEditorWebPart;
                        if (cewp == null) continue;

                        if (firstCewp == null) firstCewp = cewp;

                        if (!string.IsNullOrEmpty(webPartId) &&
                            string.Equals(wp.ID, webPartId, StringComparison.OrdinalIgnoreCase))
                        {
                            target = cewp;
                            break;
                        }
                    }

                    if (target == null) target = firstCewp;
                    if (target == null) return false;

                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement elem = xmlDoc.CreateElement("div");
                    try
                    {
                        elem.InnerXml = html;
                    }
                    catch (XmlException)
                    {
                        // TinyMCE emits HTML5 (for example a bare <br>) that is not valid XML.
                        elem = xmlDoc.CreateElement("div");
                        elem.InnerText = html;
                    }

                    target.Content = elem;
                    wpManager.SaveChanges(target);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogError("WriteContentEditorWebPart", ex);
                return false;
            }
        }

        /// <summary>
        /// Writes <paramref name="content"/> back to wherever the editor originally read it from,
        /// so a page whose body lives in a Content Editor Web Part is not silently rewritten into
        /// an unused page field. Returns a human-readable description of the target, or null when
        /// nothing writable was found.
        /// </summary>
        private string SaveContentToItem(SPListItem item, SPFile file, string content, string contentSource)
        {
            // 1. Content came from a Content Editor Web Part: write it straight back there.
            if (!string.IsNullOrEmpty(contentSource) && contentSource.StartsWith(SourceCewpPrefix, StringComparison.Ordinal))
            {
                string webPartId = contentSource.Substring(SourceCewpPrefix.Length);
                if (WriteContentEditorWebPart(file, webPartId, content))
                {
                    return "Content Editor Web Part";
                }
                // Fall through and try the page fields rather than losing the edit.
            }

            if (item == null) return null;

            // 2. Prefer the exact field the content was read from.
            if (!string.IsNullOrEmpty(contentSource) && contentSource.StartsWith(SourceFieldPrefix, StringComparison.Ordinal))
            {
                string sourceField = contentSource.Substring(SourceFieldPrefix.Length);
                if (Array.IndexOf(WritableContentFields, sourceField) >= 0 && item.Fields.ContainsField(sourceField))
                {
                    item[sourceField] = content;
                    item.Update();
                    return sourceField;
                }
            }

            // 3. Otherwise fall back to the first writable field on the item.
            foreach (string fieldName in WritableContentFields)
            {
                if (item.Fields.ContainsField(fieldName))
                {
                    item[fieldName] = content;
                    item.Update();
                    return fieldName;
                }
            }

            // No direct field: go through the publishing API, and commit via pubPage.Update()
            // because pubPage.ListItem is a different instance than `item`.
            try
            {
                PublishingPage pubPage = PublishingPage.GetPublishingPage(item);
                if (pubPage != null && pubPage.ListItem != null)
                {
                    foreach (string fieldName in WritableContentFields)
                    {
                        if (pubPage.ListItem.Fields.ContainsField(fieldName))
                        {
                            pubPage.ListItem[fieldName] = content;
                            pubPage.Update();
                            return "PublishingPage." + fieldName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("SaveContentToItem/publishing", ex);
            }

            return null;
        }

        /// <summary>
        /// True for labels whose minor part is zero (1.0, 10.0). A plain EndsWith(".0")
        /// test also matches nothing useful, but this keeps 1.10 out of the major bucket.
        /// </summary>
        private static bool IsMajorVersion(string versionLabel)
        {
            if (string.IsNullOrEmpty(versionLabel)) return false;

            int dot = versionLabel.IndexOf('.');
            if (dot < 0) return false;

            return versionLabel.Substring(dot + 1) == "0";
        }

        private bool UserCanEditPage(SPFile file)
        {
            try
            {
                SPListItem item = file.Item;
                if (item != null)
                {
                    return item.DoesUserHavePermissions(RequiredPageRights);
                }

                return file.Web != null && file.Web.DoesUserHavePermissions(RequiredPageRights);
            }
            catch (Exception ex)
            {
                LogError("UserCanEditPage", ex);
                return false;
            }
        }

        /// <summary>
        /// Runs a mutation with AllowUnsafeUpdates temporarily enabled, always restoring the
        /// previous value. Needed because the target web may differ from the context web.
        /// </summary>
        private static void WithUnsafeUpdates(SPWeb web, Action action)
        {
            if (web == null)
            {
                action();
                return;
            }

            bool previous = web.AllowUnsafeUpdates;
            try
            {
                web.AllowUnsafeUpdates = true;
                action();
            }
            finally
            {
                web.AllowUnsafeUpdates = previous;
            }
        }

        private static void DisposeIfOwned(SPWeb web)
        {
            if (web == null) return;

            try
            {
                SPContext ctx = SPContext.Current;
                if (ctx != null && (web == ctx.Web || (ctx.Site != null && web == ctx.Site.RootWeb)))
                {
                    return;
                }

                web.Dispose();
            }
            catch { }
        }

        private SPFile GetTargetFile(SPSite site, string serverRelativeUrl, out SPWeb targetWeb)
        {
            targetWeb = null;
            if (site == null || string.IsNullOrWhiteSpace(serverRelativeUrl)) return null;

            // 1. Direct check on current web first (in case the editor is running in the same subsite)
            try
            {
                SPWeb currentWeb = SPContext.Current.Web;
                if (currentWeb != null && currentWeb.Exists)
                {
                    SPFile file = TryGetFileFromWeb(currentWeb, serverRelativeUrl);
                    if (file != null && file.Exists)
                    {
                        targetWeb = currentWeb;
                        return file;
                    }
                }
            }
            catch { }

            // 2. Build candidate subweb paths from the URL (from longest prefix to shortest)
            // Example: /ar/RegAdm/PGD/Pages/HumanitarianPror.aspx
            // Candidates:
            // - /ar/RegAdm/PGD
            // - /ar/RegAdm
            // - /ar
            // - / (RootWeb)
            string urlWithoutFile = serverRelativeUrl;
            int lastSlash = serverRelativeUrl.LastIndexOf('/');
            if (lastSlash > 0)
            {
                urlWithoutFile = serverRelativeUrl.Substring(0, lastSlash);
            }

            int pagesIdx = urlWithoutFile.IndexOf("/Pages", StringComparison.OrdinalIgnoreCase);
            if (pagesIdx > 0)
            {
                urlWithoutFile = urlWithoutFile.Substring(0, pagesIdx);
            }

            string[] segments = urlWithoutFile.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // Try candidate subweb paths in reverse order (longest first)
            for (int len = segments.Length; len >= 1; len--)
            {
                string candidatePath = "/" + string.Join("/", segments, 0, len);

                SPWeb candidateWeb = TryOpenWeb(site, candidatePath);
                if (candidateWeb != null && candidateWeb.Exists)
                {
                    SPFile file = TryGetFileFromWeb(candidateWeb, serverRelativeUrl);
                    if (file != null && file.Exists)
                    {
                        targetWeb = candidateWeb;
                        return file;
                    }

                    // Do not leak an unmanaged SPWeb when it is not the match
                    DisposeIfOwned(candidateWeb);
                }
            }

            // 3. Fallback to RootWeb
            try
            {
                SPWeb rootWeb = site.RootWeb;
                if (rootWeb != null && rootWeb.Exists)
                {
                    SPFile rootFile = TryGetFileFromWeb(rootWeb, serverRelativeUrl);
                    if (rootFile != null && rootFile.Exists)
                    {
                        targetWeb = rootWeb;
                        return rootFile;
                    }
                }
            }
            catch { }

            return null;
        }

        private SPWeb TryOpenWeb(SPSite site, string webPath)
        {
            if (site == null || string.IsNullOrWhiteSpace(webPath)) return null;

            try
            {
                SPWeb web = site.OpenWeb(webPath);
                if (web != null && web.Exists) return web;
            }
            catch { }

            try
            {
                string trimmed = webPath.TrimStart('/');
                SPWeb web = site.OpenWeb(trimmed);
                if (web != null && web.Exists) return web;
            }
            catch { }

            return null;
        }

        private SPFile TryGetFileFromWeb(SPWeb web, string serverRelativeUrl)
        {
            if (web == null || !web.Exists || string.IsNullOrWhiteSpace(serverRelativeUrl)) return null;

            // Try 1: By full server-relative URL
            try
            {
                SPFile file = web.GetFile(serverRelativeUrl);
                if (file != null && file.Exists) return file;
            }
            catch { }

            // Try 2: Relative to web url
            try
            {
                string webUrl = web.ServerRelativeUrl;
                if (!string.IsNullOrEmpty(webUrl) && webUrl != "/" && serverRelativeUrl.StartsWith(webUrl, StringComparison.OrdinalIgnoreCase))
                {
                    string relativeToWeb = serverRelativeUrl.Substring(webUrl.Length).TrimStart('/');
                    SPFile file = web.GetFile(relativeToWeb);
                    if (file != null && file.Exists) return file;
                }
            }
            catch { }

            // Try 3: Direct page name in Pages library if applicable
            try
            {
                int pagesIdx = serverRelativeUrl.IndexOf("/Pages/", StringComparison.OrdinalIgnoreCase);
                if (pagesIdx >= 0)
                {
                    string pageSubPath = serverRelativeUrl.Substring(pagesIdx + 1); // "Pages/filename.aspx"
                    SPFile file = web.GetFile(pageSubPath);
                    if (file != null && file.Exists) return file;
                }
            }
            catch { }

            return null;
        }

        private string NormalizeServerRelativeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            string path = url.Trim();
            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                Uri uri;
                if (!Uri.TryCreate(path, UriKind.Absolute, out uri)) return string.Empty;
                path = uri.AbsolutePath;
            }

            // Drop any query string or fragment so "/Pages/x.aspx?a=1" still resolves
            int cut = path.IndexOfAny(new char[] { '?', '#' });
            if (cut >= 0)
            {
                path = path.Substring(0, cut);
            }

            try
            {
                path = Uri.UnescapeDataString(path);
            }
            catch { }

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            // Remove duplicate slashes
            path = Regex.Replace(path, @"(?<!:)/{2,}", "/");

            return path;
        }

        private static void LogError(string operation, Exception ex)
        {
            try
            {
                SPDiagnosticsService.Local.WriteTrace(0,
                    new SPDiagnosticsCategory("PNU.Internet.ContentEditor", TraceSeverity.Unexpected, EventSeverity.Error),
                    TraceSeverity.Unexpected,
                    "ucGenericPageContentEditor.{0} failed: {1}",
                    new object[] { operation, ex });
            }
            catch { }
        }

        private void ShowMessage(string message, string alertCssClass)
        {
            pnlStatus.Visible = true;
            divStatusAlert.Attributes["class"] = "pnu-status-alert " + alertCssClass;
            // Messages embed user-supplied values (URLs, user names, exception text), so encode them.
            litStatusMessage.Text = HttpUtility.HtmlEncode(message);
        }
    }
}
