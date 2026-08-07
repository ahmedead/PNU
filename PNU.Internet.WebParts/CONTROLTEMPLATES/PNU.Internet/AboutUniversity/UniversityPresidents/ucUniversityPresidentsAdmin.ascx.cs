using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges;
using Portal.Main.WebApp.Controls.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint.Publishing;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System.Web;
using System.Xml.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidents
{
    public partial class ucUniversityPresidentsAdmin : UserControl
    {

        private List<ContactAdminItem> ContactItems
        {
            get
            {
                if (ViewState["ContactItems"] == null)
                    ViewState["ContactItems"] = new List<ContactAdminItem>();
                return (List<ContactAdminItem>)ViewState["ContactItems"];
            }
            set { ViewState["ContactItems"] = value; }
        }

        private List<MembershipAdminItem> MembershipItems
        {
            get
            {
                if (ViewState["MembershipItems"] == null)
                    ViewState["MembershipItems"] = new List<MembershipAdminItem>();
                return (List<MembershipAdminItem>)ViewState["MembershipItems"];
            }
            set { ViewState["MembershipItems"] = value; }
        }

        private List<AwardAdminItem> AwardItems
        {
            get
            {
                if (ViewState["AwardItems"] == null)
                    ViewState["AwardItems"] = new List<AwardAdminItem>();
                return (List<AwardAdminItem>)ViewState["AwardItems"];
            }
            set { ViewState["AwardItems"] = value; }
        }

        private List<ExperienceAdminItem> ExperienceItems
        {
            get
            {
                if (ViewState["ExperienceItems"] == null)
                    ViewState["ExperienceItems"] = new List<ExperienceAdminItem>();
                return (List<ExperienceAdminItem>)ViewState["ExperienceItems"];
            }
            set { ViewState["ExperienceItems"] = value; }
        }

        private string TargetWebUrl
        {
            get
            {
                return (ViewState["TargetWebUrl"] ?? string.Empty).ToString();
            }
            set
            {
                ViewState["TargetWebUrl"] = value;
                txtWebUrl.Text = value;
            }
        }

        public class AdminUsers
        {
            public int Id { get; set; } = 0;
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(!IsAllowedUser())
                {
                    dvForm.Attributes.Add("class", "d-none");
                    hMsg.Attributes.Add("class", "block");

                    return;
                }
                txtWebUrl.Text = SPContext.Current.Web.Url;
                TargetWebUrl = txtWebUrl.Text;
                EnsureLists();
                LoadExistingData();
                BindAllGrids();
            }
        }

        public  bool IsAllowedUser(string SiteURL = "/ar/ContentAdmin/")
        {
            try
            {
                var user = SPContext.Current.Web.CurrentUser;
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='UserAccount' />
                                 <Value Type='User'>" + user.Name + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists["UniversityPresidentsAdminUsers"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return false;
                if (objNew.Count == 0)
                    return false;

                return true;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);

            }


            return false;



        }



        protected void btnCreatePage_Click(object sender, EventArgs e)
        {
            try
            {
                string createdPageUrl = string.Empty;

                RunInTargetWeb(web =>
                {
                    web.AllowUnsafeUpdates = true;

                    string pageName = txtNewPageName.Text.Trim();
                    string pageTitle = txtNewPageTitle.Text.Trim();
                    string pageLayoutUrl = txtPageLayoutUrl.Text.Trim();

                    if (string.IsNullOrWhiteSpace(pageName))
                        throw new Exception("الرجاء إدخال اسم الصفحة");

                    if (string.IsNullOrWhiteSpace(pageTitle))
                        throw new Exception("الرجاء إدخال عنوان الصفحة");

                    if (string.IsNullOrWhiteSpace(pageLayoutUrl))
                        throw new Exception("الرجاء إدخال رابط الـ Page Layout");

                    if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                        pageName += ".aspx";

                    PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                    if (publishingWeb == null)
                        throw new Exception("الموقع المحدد ليس Publishing Web");

                    PageLayout pageLayout = GetPageLayoutFromUrl(web, pageLayoutUrl);
                    if (pageLayout == null)
                        throw new Exception("تعذر العثور على الـ Page Layout");

                    if (PageExists(web, pageName))
                        throw new Exception("الصفحة موجودة بالفعل");

                    PublishingPage newPage = publishingWeb.GetPublishingPages().Add(pageName, pageLayout);

                    if (newPage == null)
                        throw new Exception("فشل إنشاء الصفحة");

                    newPage.Title = pageTitle;
                    newPage.Update();

                    SPListItem item = newPage.ListItem;
                    item["Title"] = pageTitle;
                    item.Update();

                    if (item.File.CheckOutType != SPFile.SPCheckOutType.None)
                        item.File.CheckIn("Initial Check In");

                    if (item.File.Level == SPFileLevel.Draft)
                        item.File.Publish("Initial Publish");

                    createdPageUrl = web.Url.TrimEnd('/') + "/" + newPage.Url.TrimStart('/');

                    web.AllowUnsafeUpdates = false;
                });

                ShowSuccess("تم إنشاء الصفحة بنجاح: " + createdPageUrl);
            }
            catch (Exception ex)
            {
                ShowError("تعذر إنشاء الصفحة: " + ex.Message);
            }
        }


        private SPList GetPagesLibrary(SPWeb web)
        {
            // Try by URL first (best way)
            SPList list = null;

            try
            {
                string url = web.ServerRelativeUrl.TrimEnd('/') + "/Pages";
                SPFolder folder = web.GetFolder(url);

                if (folder != null && folder.Exists)
                    list = folder.DocumentLibrary;
            }
            catch { }

            // Fallback: try by title (Arabic/English)
            if (list == null)
            {
                list = web.Lists.TryGetList("Pages")
                    ?? web.Lists.TryGetList("الصفحات");
            }

            if (list == null)
                throw new Exception("لم يتم العثور على مكتبة Pages / الصفحات");

            return list;
        }

        private bool PageExists(SPWeb web, string pageName)
        {
            SPList pagesList = GetPagesLibrary(web);

            string pageUrl = pagesList.RootFolder.ServerRelativeUrl.TrimEnd('/') + "/" + pageName;

            SPFile file = web.GetFile(pageUrl);

            return file != null && file.Exists;
        }

        private PageLayout GetPageLayoutFromUrl(SPWeb web, string pageLayoutUrl)
        {
            if (web == null)
                throw new ArgumentNullException("web");

            if (string.IsNullOrWhiteSpace(pageLayoutUrl))
                return null;

            string normalizedUrl = pageLayoutUrl.Trim();

            if (!normalizedUrl.StartsWith("/"))
                normalizedUrl = "/" + normalizedUrl;

            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
            if (publishingWeb == null)
                return null;

            foreach (PageLayout layout in publishingWeb.GetAvailablePageLayouts())
            {
                if (layout == null || layout.ListItem == null || layout.ListItem.File == null)
                    continue;

                string serverRelativeUrl = layout.ListItem.File.ServerRelativeUrl;

                if (serverRelativeUrl.Equals(normalizedUrl, StringComparison.OrdinalIgnoreCase))
                    return layout;

                if (layout.Name.Equals(normalizedUrl, StringComparison.OrdinalIgnoreCase))
                    return layout;

                if (serverRelativeUrl.EndsWith("/" + normalizedUrl.TrimStart('/'), StringComparison.OrdinalIgnoreCase))
                    return layout;
            }

            return null;
        }
        protected void btnLoadWebData_Click(object sender, EventArgs e)
        {
            TargetWebUrl = txtWebUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(TargetWebUrl))
            {
                ShowError("الرجاء إدخال رابط الموقع");
                return;
            }

            try
            {
                EnsureLists();
                LoadExistingData();
                BindAllGrids();
                ShowSuccess("تم تحميل البيانات من الموقع بنجاح");
            }
            catch (Exception ex)
            {
                ShowError("تعذر تحميل البيانات: " + ex.Message);
            }
        }

        private void EnsureLists()
        {
            RunInTargetWeb(web =>
            {
                web.AllowUnsafeUpdates = true;
                EnsureAllLists(web);
                web.AllowUnsafeUpdates = false;
            });
        }

        private void LoadExistingData()
        {
            RunInTargetWeb(web =>
            {
                ClearForm();

                SPList profileList = web.Lists.TryGetList("PresidentProfile");
                if (profileList != null && profileList.ItemCount > 0)
                {
                    SPListItem item = profileList.Items[0];

                    txtPresidentNameAr.Text = Convert.ToString(item["PresidentNameAr"]);
                    txtPresidentTitleAr.Text = Convert.ToString(item["PresidentTitleAr"]);
                    txtUniversityNameAr.Text = Convert.ToString(item["UniversityNameAr"]);
                    txtParagraphTitleAr.Text = Convert.ToString(item["ParagraphTitleAr"]);
                    txtParagraphTextAr.Text = Convert.ToString(item["ParagraphTextAr"]);
                    txtBiographyTitleAr.Text = Convert.ToString(item["BiographyTitleAr"]);
                    txtBiographyTextAr.Text = Convert.ToString(item["BiographyTextAr"]);
                    txtImageUrl.Text = Convert.ToString(item["ImageUrl"]);
                    //txtExternalLink.Text = GetUrlFieldValue(item, "ExternalLink");
                }

                ContactItems = LoadContacts(web);
                MembershipItems = LoadMemberships(web);
                AwardItems = LoadAwards(web);
                ExperienceItems = LoadExperiences(web);
            });
        }

        private void ClearForm()
        {
            txtPresidentNameAr.Text = string.Empty;
            txtPresidentTitleAr.Text = string.Empty;
            txtUniversityNameAr.Text = string.Empty;
            txtParagraphTitleAr.Text = string.Empty;
            txtParagraphTextAr.Text = string.Empty;
            txtBiographyTitleAr.Text = string.Empty;
            txtBiographyTextAr.Text = string.Empty;
            txtImageUrl.Text = string.Empty;
            //txtExternalLink.Text = string.Empty;

            ContactItems = new List<ContactAdminItem>();
            MembershipItems = new List<MembershipAdminItem>();
            AwardItems = new List<AwardAdminItem>();
            ExperienceItems = new List<ExperienceAdminItem>();
        }

        private void BindAllGrids()
        {
            gvContacts.DataSource = ContactItems;
            gvContacts.DataBind();

            gvMemberships.DataSource = MembershipItems;
            gvMemberships.DataBind();

            gvAwards.DataSource = AwardItems;
            gvAwards.DataBind();

            gvExperiences.DataSource = ExperienceItems;
            gvExperiences.DataBind();
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                SaveProfileOnly();
                ShowSuccess("تم حفظ البيانات الرئيسية بنجاح");
            }
            catch (Exception ex)
            {
                ShowError("تعذر حفظ البيانات الرئيسية: " + ex.Message);
            }
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            var list = ContactItems;
            list.Add(new ContactAdminItem
            {
                ContactLabelAr = txtContactLabelAr.Text.Trim(),
                ContactValue = txtContactValue.Text.Trim(),
                ContactType = ddlContactType.SelectedValue,
                SortOrder = ParseInt(txtContactSortOrder.Text)
            });
            ContactItems = list;

            txtContactLabelAr.Text = "";
            txtContactValue.Text = "";
            txtContactSortOrder.Text = "";

            BindAllGrids();
        }

        protected void btnAddMembership_Click(object sender, EventArgs e)
        {
            var list = MembershipItems;
            list.Add(new MembershipAdminItem
            {
                MembershipTextAr = txtMembershipTextAr.Text.Trim(),
                SortOrder = ParseInt(txtMembershipSortOrder.Text)
            });
            MembershipItems = list;

            txtMembershipTextAr.Text = "";
            txtMembershipSortOrder.Text = "";

            BindAllGrids();
        }

        protected void btnAddAward_Click(object sender, EventArgs e)
        {
            var list = AwardItems;
            list.Add(new AwardAdminItem
            {
                AwardTextAr = txtAwardTextAr.Text.Trim(),
                SortOrder = ParseInt(txtAwardSortOrder.Text)
            });
            AwardItems = list;

            txtAwardTextAr.Text = "";
            txtAwardSortOrder.Text = "";

            BindAllGrids();
        }

        protected void btnAddExperience_Click(object sender, EventArgs e)
        {
            var list = ExperienceItems;
            list.Add(new ExperienceAdminItem
            {
                JobTitleAr = txtJobTitleAr.Text.Trim(),
                OrganizationAr = txtOrganizationAr.Text.Trim(),
                LocationAr = txtLocationAr.Text.Trim(),
                PeriodAr = txtPeriodAr.Text.Trim(),
                SortOrder = ParseInt(txtExperienceSortOrder.Text)
            });
            ExperienceItems = list;

            txtJobTitleAr.Text = "";
            txtOrganizationAr.Text = "";
            txtLocationAr.Text = "";
            txtPeriodAr.Text = "";
            txtExperienceSortOrder.Text = "";

            BindAllGrids();
        }

        protected void gvContacts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteContact")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var list = ContactItems;
                if (index >= 0 && index < list.Count)
                    list.RemoveAt(index);
                ContactItems = list;
                BindAllGrids();
            }
        }

        protected void gvMemberships_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteMembership")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var list = MembershipItems;
                if (index >= 0 && index < list.Count)
                    list.RemoveAt(index);
                MembershipItems = list;
                BindAllGrids();
            }
        }

        protected void gvAwards_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAward")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var list = AwardItems;
                if (index >= 0 && index < list.Count)
                    list.RemoveAt(index);
                AwardItems = list;
                BindAllGrids();
            }
        }

        protected void gvExperiences_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteExperience")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var list = ExperienceItems;
                if (index >= 0 && index < list.Count)
                    list.RemoveAt(index);
                ExperienceItems = list;
                BindAllGrids();
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                RunInTargetWeb(web =>
                {
                    web.AllowUnsafeUpdates = true;

                    EnsureAllLists(web);
                    SaveProfile(web);
                    ReplaceContacts(web);
                    ReplaceMemberships(web);
                    ReplaceAwards(web);
                    ReplaceExperiences(web);

                    web.AllowUnsafeUpdates = false;
                });

                ShowSuccess("تم حفظ جميع البيانات بنجاح");
            }
            catch (Exception ex)
            {
                ShowError("تعذر حفظ البيانات: " + ex.Message);
            }
        }

        private void SaveProfileOnly()
        {
            RunInTargetWeb(web =>
            {
                web.AllowUnsafeUpdates = true;
                EnsureAllLists(web);
                SaveProfile(web);
                web.AllowUnsafeUpdates = false;
            });
        }

        private void RunInTargetWeb(Action<SPWeb> action)
        {
            string webUrl = txtWebUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(webUrl))
                throw new Exception("رابط الموقع فارغ");

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(webUrl))
                using (SPWeb web = site.OpenWeb())
                {
                    TargetWebUrl = web.Url;
                    action(web);
                }
            });
        }

        private void SaveProfile(SPWeb web)
        {
            SPList list = web.Lists["PresidentProfile"];
            SPListItem item = list.ItemCount > 0 ? list.Items[0] : list.AddItem();

            item["Title"] = "Main";
            item["PresidentNameAr"] = txtPresidentNameAr.Text.Trim();
            item["PresidentTitleAr"] = txtPresidentTitleAr.Text.Trim();
            item["UniversityNameAr"] = txtUniversityNameAr.Text.Trim();
            item["ParagraphTitleAr"] = txtParagraphTitleAr.Text.Trim();
            item["ParagraphTextAr"] = txtParagraphTextAr.Text.Trim();
            item["BiographyTitleAr"] = txtBiographyTitleAr.Text.Trim();
            item["BiographyTextAr"] = txtBiographyTextAr.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtImageUrl.Text))
                item["ImageUrl"] = txtImageUrl.Text.Trim() ;
            else
                item["ImageUrl"] = null;

            //if (!string.IsNullOrWhiteSpace(txtExternalLink.Text))
            //    item["ExternalLink"] = txtExternalLink.Text.Trim() + ",Link";
            //else
            //    item["ExternalLink"] = null;

            item.Update();
        }

        private void ReplaceContacts(SPWeb web)
        {
            SPList list = web.Lists["PresidentContacts"];
            DeleteAllItems(list);

            foreach (var data in ContactItems)
            {
                SPListItem item = list.AddItem();
                item["Title"] = "Contact " + data.SortOrder;
                item["ContactLabelAr"] = data.ContactLabelAr;
                item["ContactValue"] = data.ContactValue;
                item["ContactType"] = data.ContactType;
                item["SortOrder"] = data.SortOrder;
                item.Update();
            }
        }

        private void ReplaceMemberships(SPWeb web)
        {
            SPList list = web.Lists["PresidentMemberships"];
            DeleteAllItems(list);

            foreach (var data in MembershipItems)
            {
                SPListItem item = list.AddItem();
                item["Title"] = "Membership " + data.SortOrder;
                item["MembershipTextAr"] = data.MembershipTextAr;
                item["SortOrder"] = data.SortOrder;
                item.Update();
            }
        }

        private void ReplaceAwards(SPWeb web)
        {
            SPList list = web.Lists["PresidentAwards"];
            DeleteAllItems(list);

            foreach (var data in AwardItems)
            {
                SPListItem item = list.AddItem();
                item["Title"] = "Award " + data.SortOrder;
                item["AwardTextAr"] = data.AwardTextAr;
                item["SortOrder"] = data.SortOrder;
                item.Update();
            }
        }

        private void ReplaceExperiences(SPWeb web)
        {
            SPList list = web.Lists["PresidentExperiences"];
            DeleteAllItems(list);

            foreach (var data in ExperienceItems)
            {
                SPListItem item = list.AddItem();
                item["Title"] = "Experience " + data.SortOrder;
                item["JobTitleAr"] = data.JobTitleAr;
                item["OrganizationAr"] = data.OrganizationAr;
                item["LocationAr"] = data.LocationAr;
                item["PeriodAr"] = data.PeriodAr;
                item["SortOrder"] = data.SortOrder;
                item.Update();
            }
        }

        private void DeleteAllItems(SPList list)
        {
            while (list.Items.Count > 0)
            {
                list.Items[0].Delete();
            }
        }

        private void EnsureAllLists(SPWeb web)
        {
            EnsureProfileList(web);
            EnsureContactsList(web);
            EnsureMembershipsList(web);
            EnsureAwardsList(web);
            EnsureExperiencesList(web);
        }

        private void EnsureProfileList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PresidentProfile");
            if (list == null)
            {
                Guid listId = web.Lists.Add("PresidentProfile", "Stores main president profile", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            EnsureField(list, "PresidentNameAr", SPFieldType.Text);
            EnsureField(list, "PresidentTitleAr", SPFieldType.Text);
            EnsureField(list, "UniversityNameAr", SPFieldType.Text);
            EnsureField(list, "ImageUrl", SPFieldType.Text);
            //EnsureField(list, "ExternalLink", SPFieldType.URL);
            EnsureField(list, "ParagraphTitleAr", SPFieldType.Text);
            EnsureField(list, "ParagraphTextAr", SPFieldType.Note);
            EnsureField(list, "BiographyTitleAr", SPFieldType.Text);
            EnsureField(list, "BiographyTextAr", SPFieldType.Note);
            list.Update();
        }

        private void EnsureContactsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PresidentContacts");
            if (list == null)
            {
                Guid listId = web.Lists.Add("PresidentContacts", "Stores contacts", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            EnsureField(list, "ContactLabelAr", SPFieldType.Text);
            EnsureField(list, "ContactValue", SPFieldType.Text);
            EnsureChoiceField(list, "ContactType", new[] { "Email", "Phone", "Link" });
            EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private void EnsureMembershipsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PresidentMemberships");
            if (list == null)
            {
                Guid listId = web.Lists.Add("PresidentMemberships", "Stores memberships", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            EnsureField(list, "MembershipTextAr", SPFieldType.Note);
            EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private void EnsureAwardsList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PresidentAwards");
            if (list == null)
            {
                Guid listId = web.Lists.Add("PresidentAwards", "Stores awards", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            EnsureField(list, "AwardTextAr", SPFieldType.Note);
            EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private void EnsureExperiencesList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList("PresidentExperiences");
            if (list == null)
            {
                Guid listId = web.Lists.Add("PresidentExperiences", "Stores experiences", SPListTemplateType.GenericList);
                list = web.Lists[listId];
            }

            EnsureField(list, "JobTitleAr", SPFieldType.Text);
            EnsureField(list, "OrganizationAr", SPFieldType.Text);
            EnsureField(list, "LocationAr", SPFieldType.Text);
            EnsureField(list, "PeriodAr", SPFieldType.Text);
            EnsureField(list, "SortOrder", SPFieldType.Number);
            list.Update();
        }

        private void EnsureField(SPList list, string internalName, SPFieldType type)
        {
            if (!list.Fields.ContainsField(internalName))
                list.Fields.Add(internalName, type, false);
            SPView view = list.DefaultView;
            if (!view.ViewFields.Exists(internalName))
            {
                view.ViewFields.Add(internalName);
                view.Update();
            }
        }

        private void EnsureChoiceField(SPList list, string internalName, string[] choices)
        {
            if (!list.Fields.ContainsField(internalName))
            {
                list.Fields.Add(internalName, SPFieldType.Choice, false);
                SPFieldChoice field = (SPFieldChoice)list.Fields[internalName];
                field.Choices.Clear();
                foreach (string choice in choices)
                    field.Choices.Add(choice);
                field.Update();
            }
        }

        private List<ContactAdminItem> LoadContacts(SPWeb web)
        {
            var result = new List<ContactAdminItem>();
            SPList list = web.Lists.TryGetList("PresidentContacts");
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new ContactAdminItem
                {
                    ContactLabelAr = Convert.ToString(item["ContactLabelAr"]),
                    ContactValue = Convert.ToString(item["ContactValue"]),
                    ContactType = Convert.ToString(item["ContactType"]),
                    SortOrder = ParseInt(Convert.ToString(item["SortOrder"]))
                });
            }

            return result;
        }

        private List<MembershipAdminItem> LoadMemberships(SPWeb web)
        {
            var result = new List<MembershipAdminItem>();
            SPList list = web.Lists.TryGetList("PresidentMemberships");
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new MembershipAdminItem
                {
                    MembershipTextAr = Convert.ToString(item["MembershipTextAr"]),
                    SortOrder = ParseInt(Convert.ToString(item["SortOrder"]))
                });
            }

            return result;
        }

        private List<AwardAdminItem> LoadAwards(SPWeb web)
        {
            var result = new List<AwardAdminItem>();
            SPList list = web.Lists.TryGetList("PresidentAwards");
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AwardAdminItem
                {
                    AwardTextAr = Convert.ToString(item["AwardTextAr"]),
                    SortOrder = ParseInt(Convert.ToString(item["SortOrder"]))
                });
            }

            return result;
        }

        private List<ExperienceAdminItem> LoadExperiences(SPWeb web)
        {
            var result = new List<ExperienceAdminItem>();
            SPList list = web.Lists.TryGetList("PresidentExperiences");
            if (list == null) return result;

            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>" };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new ExperienceAdminItem
                {
                    JobTitleAr = Convert.ToString(item["JobTitleAr"]),
                    OrganizationAr = Convert.ToString(item["OrganizationAr"]),
                    LocationAr = Convert.ToString(item["LocationAr"]),
                    PeriodAr = Convert.ToString(item["PeriodAr"]),
                    SortOrder = ParseInt(Convert.ToString(item["SortOrder"]))
                });
            }

            return result;
        }

        private string GetUrlFieldValue(SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return string.Empty;

            SPFieldUrlValue urlValue = new SPFieldUrlValue(item[fieldName].ToString());
            return urlValue.Url;
        }

        private int ParseInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }

        private void ShowSuccess(string message)
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = message;
        }

        private void ShowError(string message)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = message;
        }
    
    

    
    }

    [Serializable]
    public class ContactAdminItem
    {
        public string ContactLabelAr { get; set; }
        public string ContactValue { get; set; }
        public string ContactType { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class MembershipAdminItem
    {
        public string MembershipTextAr { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class AwardAdminItem
    {
        public string AwardTextAr { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class ExperienceAdminItem
    {
        public string JobTitleAr { get; set; }
        public string OrganizationAr { get; set; }
        public string LocationAr { get; set; }
        public string PeriodAr { get; set; }
        public int SortOrder { get; set; }
    }
}