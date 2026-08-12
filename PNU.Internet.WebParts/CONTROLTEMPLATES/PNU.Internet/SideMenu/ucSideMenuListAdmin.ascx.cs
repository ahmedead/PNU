using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu
{
    public partial class ucSideMenuListAdmin : UserControl
    {
        private struct ControlPreset
        {
            public string Name;
            public string PageName;
            public string TitleAr;
            public string TitleEn;
            public string ControlPath;
            public string Properties;
        }

        private readonly List<ControlPreset> _presets = new List<ControlPreset>
        {
            new ControlPreset { Name = "-- اختر من القوالب الجاهزة --", PageName = "", TitleAr = "", TitleEn = "", ControlPath = "", Properties = "" },
            new ControlPreset { Name = "البرامج (Programs)", PageName = "CollegePrograms.aspx", TitleAr = "البرامج", TitleEn = "Programs", ControlPath = "PNU.Internet/Colleges/DGA/ucCollegeProgramsDga.ascx", Properties = "" },
            new ControlPreset { Name = "المستندات والنماذج (Documents)", PageName = "CollegeDocuments.aspx", TitleAr = "المستندات والنماذج والأدلة", TitleEn = "Documents, Forms and Guides", ControlPath = "PNU.Internet/Colleges/DGA/ucCollegeDocuments.ascx", Properties = "" },
            new ControlPreset { Name = "تواصل مع الكلية (Contact)", PageName = "CollegeContacts.aspx", TitleAr = "تواصل مع الكلية", TitleEn = "Contact the College", ControlPath = "PNU.Internet/Colleges/DGA/ucCollegeContactDga.ascx", Properties = "" },
            new ControlPreset { Name = "الهيكل التنظيمي (Hierarchy)", PageName = "NewHierarchy.aspx", TitleAr = "الهيكل التنظيمي للكلية", TitleEn = "Hierarchy", ControlPath = "PNU.Internet/Faculties/DGA/OrgStructure/ucFacultyOrgStructureDga.ascx", Properties = "" },
            new ControlPreset { Name = "إنجازات الكلية (Achievements)", PageName = "NewAchievements.aspx", TitleAr = "إنجازات الكلية", TitleEn = "Achievements", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeAchievements.ascx", Properties = "" },
            new ControlPreset { Name = "وكالات الكلية (Agencies)", PageName = "NewAgancies.aspx", TitleAr = "وكالات الكلية", TitleEn = "Agancies", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeAgancies.ascx", Properties = "" },
            new ControlPreset { Name = "مرافق الكلية (Facilities)", PageName = "NewFacilities.aspx", TitleAr = "مرافق الكلية", TitleEn = "Facilities", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeFacilities.ascx", Properties = "" },
            new ControlPreset { Name = "البحث العلمي (Researches)", PageName = "NewResearches.aspx", TitleAr = "البحث العلمي في الكلية", TitleEn = "Researches", ControlPath = "PNU.Internet/Faculties/DGA/Research/ucFacultyResearchDga.ascx", Properties = "" },
            new ControlPreset { Name = "الطالبات (Students)", PageName = "NewStudents.aspx", TitleAr = "الطالبات في الكلية", TitleEn = "Students", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeStudents.ascx", Properties = "" },
            new ControlPreset { Name = "الخدمات الطلابية (Services)", PageName = "NewServices.aspx", TitleAr = "الخدمات الطلابية في الكلية", TitleEn = "Services", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeStudentServices.ascx", Properties = "" },
            new ControlPreset { Name = "الأندية الطلابية (Clubs)", PageName = "NewClubs.aspx", TitleAr = "الأندية الطلابية في الكلية", TitleEn = "Clubs", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeClubs.ascx", Properties = "" },
            new ControlPreset { Name = "المبادرات (Initiatives)", PageName = "NewInitiatives.aspx", TitleAr = "المبادرات", TitleEn = "Initiatives", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeInitiatives.ascx", Properties = "" },
            new ControlPreset { Name = "التدريب (Trainings)", PageName = "NewTrainings.aspx", TitleAr = "التدريب في الكلية", TitleEn = "Trainings", ControlPath = "PNU.Internet/Colleges/Details/ucCollegeTrainings.ascx", Properties = "" },
            new ControlPreset { Name = "الرئيسية العامة (Shared About)", PageName = "SharedAbout.aspx", TitleAr = "الرئيسية", TitleEn = "About", ControlPath = "PNU.Internet/Shared/About/ucSharedAboutDga.ascx", Properties = "" },
            new ControlPreset { Name = "تواصل معنا (Shared Contact)", PageName = "SharedContactUs.aspx", TitleAr = "تواصل معنا", TitleEn = "Contact Us", ControlPath = "PNU.Internet/Shared/ucContactUsDGA.ascx", Properties = "" },
            new ControlPreset { Name = "جوائز الوكالة (Agency Achievements)", PageName = "AgencyAchievements.aspx", TitleAr = "الشهادات والجوائز", TitleEn = "Agency Achievements", ControlPath = "PNU.Internet/GeneralDest/ucDestCertificates.ascx", Properties = "ListName#AgencyAchievements" },
            new ControlPreset { Name = "العمادات (Agency Deans)", PageName = "AgencyDeens.aspx", TitleAr = "العمادات", TitleEn = "Agency Deens", ControlPath = "PNU.Internet/GeneralDest/ucDestDepartments.ascx", Properties = "ListName#AgencyDeens" },
            new ControlPreset { Name = "الإدارات (Agency Departments)", PageName = "AgencyDepartments.aspx", TitleAr = "الإدارات", TitleEn = "Agency Departments", ControlPath = "PNU.Internet/GeneralDest/ucDestDepartments.ascx", Properties = "ListName#AgencyDepartments" },
            new ControlPreset { Name = "المراكز (Agency Centers)", PageName = "AgencyCenters.aspx", TitleAr = "المراكز", TitleEn = "Agency Centers", ControlPath = "PNU.Internet/GeneralDest/ucDestDepartments.ascx", Properties = "ListName#AgencyCenters" },
            new ControlPreset { Name = "الوحدات (Agency Units)", PageName = "AgencyUnits.aspx", TitleAr = "الوحدات", TitleEn = "Agency Units", ControlPath = "PNU.Internet/GeneralDest/ucDestDepartments.ascx", Properties = "ListName#AgencyUnits" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitSiteUrl();
                PopulateSubwebs();
                PopulatePresets();
                LoadSiteData();
            }
        }

        private string TargetSiteUrl
        {
            get
            {
                string url = txtWebSiteURL.Text.Trim();
                if (string.IsNullOrEmpty(url) && SPContext.Current != null && SPContext.Current.Web != null)
                {
                    url = SPContext.Current.Web.Url;
                    txtWebSiteURL.Text = url;
                }
                return url;
            }
        }

        private void InitSiteUrl()
        {
            if (SPContext.Current != null && SPContext.Current.Web != null)
            {
                txtWebSiteURL.Text = SPContext.Current.Web.Url;
            }
        }

        private void PopulateSubwebs()
        {
            string url = TargetSiteUrl;
            if (string.IsNullOrEmpty(url)) return;

            try
            {
                List<string> previouslyChecked = new List<string>();
                if (cblSubwebs != null)
                {
                    foreach (ListItem item in cblSubwebs.Items)
                    {
                        if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        {
                            previouslyChecked.Add(item.Value);
                        }
                    }
                }

                ddlSubwebs.Items.Clear();
                if (cblSubwebs != null) cblSubwebs.Items.Clear();

                ddlSubwebs.Items.Add(new ListItem("-- اختر موقع --", ""));

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(url))
                    using (SPWeb web = site.OpenWeb())
                    {
                        string currentTitle = web.Title;
                        string currentUrl = web.Url;
                        string currentRelUrl = web.ServerRelativeUrl;

                        ddlSubwebs.Items.Add(new ListItem("الموقع الحالي (" + currentTitle + ")", currentUrl));

                        if (cblSubwebs != null)
                        {
                            cblSubwebs.Items.Add(new ListItem(currentTitle + " (" + currentRelUrl + ") - [الموقع الرئيسي]", currentUrl));
                        }

                        foreach (SPWeb subweb in web.Webs)
                        {
                            try
                            {
                                string subTitle = subweb.Title;
                                string subUrl = subweb.Url;
                                string subRelUrl = subweb.ServerRelativeUrl;

                                ddlSubwebs.Items.Add(new ListItem("└─ " + subTitle + " (" + subRelUrl + ")", subUrl));
                                if (cblSubwebs != null)
                                {
                                    cblSubwebs.Items.Add(new ListItem(subTitle + " (" + subRelUrl + ")", subUrl));
                                }
                            }
                            finally
                            {
                                subweb.Dispose();
                            }
                        }
                    }
                });

                if (cblSubwebs != null && previouslyChecked.Count > 0)
                {
                    foreach (ListItem item in cblSubwebs.Items)
                    {
                        if (previouslyChecked.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(Request.Url.ToString(), "ucSideMenuListAdmin.PopulateSubwebs", ex.Message);
            }
        }

        private void PopulatePresets()
        {
            ddlControlPresets.Items.Clear();
            foreach (ControlPreset preset in _presets)
            {
                ddlControlPresets.Items.Add(new ListItem(preset.Name, preset.Name));
            }
        }

        protected void ddlSubwebs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSubwebs.SelectedValue))
            {
                txtWebSiteURL.Text = ddlSubwebs.SelectedValue;
                PopulateSubwebs();
                LoadSiteData();
            }
        }

        protected void btnLoadSite_Click(object sender, EventArgs e)
        {
            PopulateSubwebs();
            LoadSiteData();
        }

        protected void btnSelectAllSubwebs_Click(object sender, EventArgs e)
        {
            if (cblSubwebs != null)
            {
                foreach (ListItem item in cblSubwebs.Items)
                {
                    item.Selected = true;
                }
            }
        }

        protected void btnDeselectAllSubwebs_Click(object sender, EventArgs e)
        {
            if (cblSubwebs != null)
            {
                foreach (ListItem item in cblSubwebs.Items)
                {
                    item.Selected = false;
                }
            }
        }

        private void LoadSiteData()
        {
            string url = TargetSiteUrl;
            if (string.IsNullOrEmpty(url))
            {
                ShowAlert("يرجى إدخال رابط الموقع المستهدف", "warning");
                return;
            }

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(url))
                    using (SPWeb web = site.OpenWeb())
                    {
                        lblCurrentLoadedSite.Text = "الموقع المحمل حالياً: <strong>" + web.Title + "</strong> (" + web.Url + ")";

                        SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                        SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);

                        if (level1List == null || level2List == null)
                        {
                            lblListsStatus.Text = "<span class='text-danger font-weight-bold'>إحدى القائمتين أو كلاهما غير موجودة في الموقع المحدد. اضغط 'إنشاء القوائم' لإعدادهما.</span>";
                            pnlMainManagement.Visible = false;
                        }
                        else
                        {
                            lblListsStatus.Text = string.Format("<span class='text-success font-weight-bold'>القوائم موجودة وجاهزة! (عدد عناصر المستوى الأول: {0} | عدد عناصر المستوى الثاني: {1})</span>", level1List.ItemCount, level2List.ItemCount);
                            pnlMainManagement.Visible = true;
                            BindLevel1Data(web, level1List);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowAlert("خطأ أثناء تحميل بيانات الموقع: " + ex.Message, "danger");
                Publics.WriteToLog(Request.Url.ToString(), "ucSideMenuListAdmin.LoadSiteData", ex.Message);
            }
        }

        protected void btnCreateLists_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsureLists(web);
                ShowAlert("تمت عملية فحص/إنشاء القوائم بنجاح!", "success");
            });
        }

        protected void btnSeedCollege_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsureLists(web);
                SideMenuListProvisioner.SeedMenu(web);
                ShowAlert("تمت عملية تعبئة القائمة الافتراضية للكليات والصفحات بنجاح!", "success");
            });
        }

        protected void btnSeedAgency_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsureLists(web);
                SideMenuListProvisioner.SeedMenuForAgencies(web);
                ShowAlert("تمت عملية تعبئة القائمة الافتراضية للوكالات والصفحات بنجاح!", "success");
            });
        }

        protected void btnSeedDeenships_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsureLists(web);
                SideMenuListProvisioner.SeedMenuForDeenShips(web);
                ShowAlert("تمت عملية تعبئة القائمة الافتراضية للعمادة والصفحات بنجاح!", "success");
            });
        }
        protected void btnSeedDepartments_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsureLists(web);
                SideMenuListProvisioner.SeedMenuForDepartments(web);
                ShowAlert("تمت عملية تعبئة القائمة الافتراضية للإدارات والصفحات بنجاح!", "success");
            });
        }

        protected void btnClearLists_Click(object sender, EventArgs e)
        {
            ExecuteAction((web) =>
            {
                SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                if (level2List != null)
                {
                    SPListItemCollection items2 = level2List.GetItems();
                    for (int i = items2.Count - 1; i >= 0; i--)
                    {
                        items2[i].Delete();
                    }
                }

                SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                if (level1List != null)
                {
                    SPListItemCollection items1 = level1List.GetItems();
                    for (int i = items1.Count - 1; i >= 0; i--)
                    {
                        items1[i].Delete();
                    }
                }

                ShowAlert("تم مسح جميع عناصر القوائم بنجاح!", "info");
            });
        }

        private void BindLevel1Data(SPWeb web, SPList level1List)
        {
            string currentSelectedL1 = ddlSubMenuLevel1.SelectedValue;

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Title_EN", typeof(string));
            dt.Columns.Add("URL", typeof(string));
            dt.Columns.Add("ItemOrder", typeof(double));
            dt.Columns.Add("Visibility", typeof(bool));

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>"
            };

            SPListItemCollection items = level1List.GetItems(query);
            ddlSubMenuLevel1.Items.Clear();
            ddlL2Parent.Items.Clear();

            ddlSubMenuLevel1.Items.Add(new ListItem("-- اختر عنصر المستوى الأول --", "0"));
            ddlL2Parent.Items.Add(new ListItem("-- اختر عنصر المستوى الأول --", "0"));

            foreach (SPListItem item in items)
            {
                string titleAr = Convert.ToString(item["Title"]);
                string titleEn = Convert.ToString(item["Title_EN"]);
                string urlStr = "";
                if (item["URL"] != null)
                {
                    SPFieldUrlValue urlVal = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                    urlStr = urlVal.Url;
                }
                double order = item["ItemOrder"] != null ? Convert.ToDouble(item["ItemOrder"]) : 0;
                bool visibility = item["Visibility"] != null && Convert.ToBoolean(item["Visibility"]);

                dt.Rows.Add(item.ID, titleAr, titleEn, urlStr, order, visibility);

                string displayTitle = string.Format("{0} ({1})", titleAr, string.IsNullOrEmpty(titleEn) ? "No EN" : titleEn);
                ddlSubMenuLevel1.Items.Add(new ListItem(displayTitle, item.ID.ToString()));
                ddlL2Parent.Items.Add(new ListItem(displayTitle, item.ID.ToString()));
            }

            gvLevel1Items.DataSource = dt;
            gvLevel1Items.DataBind();

            if (!string.IsNullOrEmpty(currentSelectedL1) && ddlSubMenuLevel1.Items.FindByValue(currentSelectedL1) != null && currentSelectedL1 != "0")
            {
                ddlSubMenuLevel1.SelectedValue = currentSelectedL1;
                if (ddlL2Parent.Items.FindByValue(currentSelectedL1) != null)
                    ddlL2Parent.SelectedValue = currentSelectedL1;

                BindLevel2Data(web, Convert.ToInt32(currentSelectedL1));
            }
            else if (ddlSubMenuLevel1.Items.Count > 1)
            {
                ddlSubMenuLevel1.SelectedIndex = 1;
                if (ddlL2Parent.Items.Count > 1) ddlL2Parent.SelectedIndex = 1;
                BindLevel2Data(web, Convert.ToInt32(ddlSubMenuLevel1.SelectedValue));
            }
            else
            {
                gvLevel2Items.DataSource = null;
                gvLevel2Items.DataBind();
                litSelectedParentBadge.Text = "يرجى تحديد عنصر المستوى الأول";
            }
        }

        protected void ddlSubMenuLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parentId = Convert.ToInt32(ddlSubMenuLevel1.SelectedValue);
            if (ddlL2Parent.Items.FindByValue(parentId.ToString()) != null)
            {
                ddlL2Parent.SelectedValue = parentId.ToString();
            }

            ExecuteAction((web) =>
            {
                BindLevel2Data(web, parentId);
            }, reloadSiteData: false);
        }

        private void BindLevel2Data(SPWeb web, int parentId)
        {
            if (parentId <= 0)
            {
                gvLevel2Items.DataSource = null;
                gvLevel2Items.DataBind();
                litSelectedParentBadge.Text = "لم يتم تحديد عنصر أَب";
                return;
            }

            SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
            if (level2List == null) return;

            string parentTitle = "";
            SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
            if (level1List != null)
            {
                try
                {
                    SPListItem parentItem = level1List.GetItemById(parentId);
                    if (parentItem != null)
                    {
                        parentTitle = Convert.ToString(parentItem["Title"]);
                    }
                }
                catch { }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Title_EN", typeof(string));
            dt.Columns.Add("URL", typeof(string));
            dt.Columns.Add("ItemOrder", typeof(double));
            dt.Columns.Add("Visibility", typeof(bool));

            SPListItemCollection allItems = level2List.GetItems();
            int count = 0;

            foreach (SPListItem item in allItems)
            {
                bool isMatch = false;
                if (item["Parent"] != null)
                {
                    string rawParent = Convert.ToString(item["Parent"]);
                    SPFieldLookupValue lookupVal = new SPFieldLookupValue(rawParent);
                    if (lookupVal.LookupId == parentId)
                    {
                        isMatch = true;
                    }
                    else if (!string.IsNullOrEmpty(parentTitle) &&
                             string.Equals(lookupVal.LookupValue, parentTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        isMatch = true;
                    }
                    else if (!string.IsNullOrEmpty(parentTitle) &&
                             string.Equals(rawParent, parentTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        isMatch = true;
                    }
                }

                if (isMatch)
                {
                    string titleAr = Convert.ToString(item["Title"]);
                    string titleEn = Convert.ToString(item["Title_EN"]);
                    string urlStr = "";
                    if (item["URL"] != null)
                    {
                        SPFieldUrlValue urlVal = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                        urlStr = urlVal.Url;
                    }
                    double order = item["ItemOrder"] != null ? Convert.ToDouble(item["ItemOrder"]) : 0;
                    bool visibility = item["Visibility"] != null && Convert.ToBoolean(item["Visibility"]);

                    dt.Rows.Add(item.ID, titleAr, titleEn, urlStr, order, visibility);
                    count++;
                }
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "ItemOrder ASC";
            DataTable sortedDt = dv.ToTable();

            gvLevel2Items.DataSource = sortedDt;
            gvLevel2Items.DataBind();

            string displayParentText = (ddlSubMenuLevel1.SelectedItem != null && ddlSubMenuLevel1.SelectedIndex > 0)
                ? ddlSubMenuLevel1.SelectedItem.Text
                : (!string.IsNullOrEmpty(parentTitle) ? parentTitle : parentId.ToString());

            litSelectedParentBadge.Text = string.Format("الأب الحالي: {0} (العدد: {1})", displayParentText, count);
        }

        protected void ddlControlPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPresetName = ddlControlPresets.SelectedValue;
            foreach (ControlPreset preset in _presets)
            {
                if (preset.Name == selectedPresetName && !string.IsNullOrEmpty(preset.ControlPath))
                {
                    txtPageName.Text = preset.PageName;
                    txtUserControlPath.Text = preset.ControlPath;
                    txtUserControlProperties.Text = preset.Properties;

                    txtStandaloneTitleAr.Text = preset.TitleAr;
                    txtStandaloneTitleEn.Text = preset.TitleEn;

                    if (string.IsNullOrEmpty(txtL1TitleAr.Text)) txtL1TitleAr.Text = preset.TitleAr;
                    if (string.IsNullOrEmpty(txtL1TitleEn.Text)) txtL1TitleEn.Text = preset.TitleEn;
                    if (string.IsNullOrEmpty(txtL2TitleAr.Text)) txtL2TitleAr.Text = preset.TitleAr;
                    if (string.IsNullOrEmpty(txtL2TitleEn.Text)) txtL2TitleEn.Text = preset.TitleEn;
                    break;
                }
            }
        }

        protected void btnCreateStandalonePage_Click(object sender, EventArgs e)
        {
            string pageName = txtPageName.Text.Trim();
            if (string.IsNullOrEmpty(pageName))
            {
                ShowAlert("يرجى إدخال اسم ملف الصفحة (مثال: SharedAbout.aspx)", "warning");
                return;
            }

            string ctrlPath = txtUserControlPath.Text.Trim();
            if (string.IsNullOrEmpty(ctrlPath))
            {
                ShowAlert("يرجى إدخال مسار عنصر التحكم (User Control Path)", "warning");
                return;
            }

            string titleAr = txtStandaloneTitleAr.Text.Trim();
            if (string.IsNullOrEmpty(titleAr)) titleAr = Path.GetFileNameWithoutExtension(pageName);

            string titleEn = txtStandaloneTitleEn.Text.Trim();
            if (string.IsNullOrEmpty(titleEn)) titleEn = titleAr;

            string ctrlProps = txtUserControlProperties.Text.Trim();
            string layoutUrl = txtPageLayoutUrl.Text.Trim();

            ExecuteAction((web) =>
            {
                SideMenuListProvisioner.EnsurePage(web, pageName, titleAr, titleEn, ctrlPath, ctrlProps, layoutUrl);
                ShowAlert("تم إنشاء وتجهيز الصفحة بنجاح: " + pageName, "success");
            });
        }

        protected void btnAddLevel1_Click(object sender, EventArgs e)
        {
            string titleAr = txtL1TitleAr.Text.Trim();
            string titleEn = txtL1TitleEn.Text.Trim();
            string url = txtL1Url.Text.Trim();
            int order = 1;
            int.TryParse(txtL1Order.Text, out order);
            bool visibility = chkL1Visibility.Checked;

            if (string.IsNullOrEmpty(titleAr))
            {
                ShowAlert("يرجى إدخال العنوان بالعربية لعنصر المستوى الأول", "warning");
                return;
            }

            ExecuteAction((web) =>
            {
                if (chkCreatePage.Checked && !string.IsNullOrEmpty(txtPageName.Text.Trim()))
                {
                    string pageName = txtPageName.Text.Trim();
                    string ctrlPath = txtUserControlPath.Text.Trim();
                    string ctrlProps = txtUserControlProperties.Text.Trim();
                    string layoutUrl = txtPageLayoutUrl.Text.Trim();

                    SideMenuListProvisioner.EnsurePage(web, pageName, titleAr, titleEn, ctrlPath, ctrlProps, layoutUrl);

                    if (string.IsNullOrEmpty(url))
                    {
                        url = web.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + pageName;
                    }
                }

                SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                if (level1List != null)
                {
                    SideMenuListProvisioner.AddLevel1(level1List, titleAr, titleEn, url, order, visibility);
                    ShowAlert("تمت إضافة عنصر المستوى الأول بنجاح!", "success");
                    ClearL1Form();
                }
            });
        }

        protected void btnUpdateLevel1_Click(object sender, EventArgs e)
        {
            int editId = Convert.ToInt32(hfL1EditID.Value);
            if (editId <= 0) return;

            string titleAr = txtL1TitleAr.Text.Trim();
            string titleEn = txtL1TitleEn.Text.Trim();
            string url = txtL1Url.Text.Trim();
            int order = 1;
            int.TryParse(txtL1Order.Text, out order);
            bool visibility = chkL1Visibility.Checked;

            ExecuteAction((web) =>
            {
                SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                if (level1List != null)
                {
                    SideMenuListProvisioner.UpdateLevel1(level1List, editId, titleAr, titleEn, url, order, visibility);
                    ShowAlert("تم تحديث عنصر المستوى الأول بنجاح!", "success");
                    ClearL1Form();
                }
            });
        }

        protected void btnCancelL1Edit_Click(object sender, EventArgs e)
        {
            ClearL1Form();
        }

        private void ClearL1Form()
        {
            hfL1EditID.Value = "0";
            txtL1TitleAr.Text = "";
            txtL1TitleEn.Text = "";
            txtL1Url.Text = "";
            txtL1Order.Text = "1";
            chkL1Visibility.Checked = true;
            litL1FormTitle.Text = "إضافة عنصر مستوى أول جديد (Add SubMenu Level 1)";
            btnAddLevel1.Visible = true;
            btnUpdateLevel1.Visible = false;
            btnCancelL1Edit.Visible = false;
        }

        protected void btnAddLevel2_Click(object sender, EventArgs e)
        {
            int parentId = Convert.ToInt32(ddlL2Parent.SelectedValue);
            if (parentId <= 0)
            {
                ShowAlert("يرجى تحديد العنصر الأب في المستوى الأول", "warning");
                return;
            }

            string parentTitle = ddlL2Parent.SelectedItem.Text;
            int bracketIdx = parentTitle.IndexOf('(');
            if (bracketIdx > 0) parentTitle = parentTitle.Substring(0, bracketIdx).Trim();

            string titleAr = txtL2TitleAr.Text.Trim();
            string titleEn = txtL2TitleEn.Text.Trim();
            string url = txtL2Url.Text.Trim();
            int order = 1;
            int.TryParse(txtL2Order.Text, out order);
            bool visibility = chkL2Visibility.Checked;

            if (string.IsNullOrEmpty(titleAr))
            {
                ShowAlert("يرجى إدخال العنوان بالعربية لعنصر المستوى الثاني", "warning");
                return;
            }

            ExecuteAction((web) =>
            {
                if (chkCreatePage.Checked && !string.IsNullOrEmpty(txtPageName.Text.Trim()))
                {
                    string pageName = txtPageName.Text.Trim();
                    string ctrlPath = txtUserControlPath.Text.Trim();
                    string ctrlProps = txtUserControlProperties.Text.Trim();
                    string layoutUrl = txtPageLayoutUrl.Text.Trim();

                    SideMenuListProvisioner.EnsurePage(web, pageName, titleAr, titleEn, ctrlPath, ctrlProps, layoutUrl);

                    if (string.IsNullOrEmpty(url))
                    {
                        url = web.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + pageName;
                    }
                }

                SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                if (level2List != null)
                {
                    SideMenuListProvisioner.AddLevel2(level2List, parentId, parentTitle, titleAr, titleEn, url, order, visibility);
                    ShowAlert("تمت إضافة عنصر المستوى الثاني بنجاح!", "success");
                    ClearL2Form();
                }
            });
        }

        protected void btnUpdateLevel2_Click(object sender, EventArgs e)
        {
            int editId = Convert.ToInt32(hfL2EditID.Value);
            if (editId <= 0) return;

            int parentId = Convert.ToInt32(ddlL2Parent.SelectedValue);
            string parentTitle = ddlL2Parent.SelectedItem.Text;
            int bracketIdx = parentTitle.IndexOf('(');
            if (bracketIdx > 0) parentTitle = parentTitle.Substring(0, bracketIdx).Trim();

            string titleAr = txtL2TitleAr.Text.Trim();
            string titleEn = txtL2TitleEn.Text.Trim();
            string url = txtL2Url.Text.Trim();
            int order = 1;
            int.TryParse(txtL2Order.Text, out order);
            bool visibility = chkL2Visibility.Checked;

            ExecuteAction((web) =>
            {
                SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                if (level2List != null)
                {
                    SideMenuListProvisioner.UpdateLevel2(level2List, editId, parentId, parentTitle, titleAr, titleEn, url, order, visibility);
                    ShowAlert("تم تحديث عنصر المستوى الثاني بنجاح!", "success");
                    ClearL2Form();
                }
            });
        }

        protected void btnCancelL2Edit_Click(object sender, EventArgs e)
        {
            ClearL2Form();
        }

        private void ClearL2Form()
        {
            hfL2EditID.Value = "0";
            txtL2TitleAr.Text = "";
            txtL2TitleEn.Text = "";
            txtL2Url.Text = "";
            txtL2Order.Text = "1";
            chkL2Visibility.Checked = true;
            litL2FormTitle.Text = "إضافة عنصر مستوى ثاني جديد (Add SubMenu Level 2)";
            btnAddLevel2.Visible = true;
            btnUpdateLevel2.Visible = false;
            btnCancelL2Edit.Visible = false;
        }

        protected void gvLevel1Items_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (id <= 0) return;

            if (e.CommandName == "SelectParent")
            {
                if (ddlSubMenuLevel1.Items.FindByValue(id.ToString()) != null)
                {
                    ddlSubMenuLevel1.SelectedValue = id.ToString();
                    ddlL2Parent.SelectedValue = id.ToString();
                    ExecuteAction((web) => BindLevel2Data(web, id));
                }
            }
            else if (e.CommandName == "EditL1")
            {
                ExecuteAction((web) =>
                {
                    SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                    if (level1List != null)
                    {
                        SPListItem item = level1List.GetItemById(id);
                        if (item != null)
                        {
                            hfL1EditID.Value = item.ID.ToString();
                            txtL1TitleAr.Text = Convert.ToString(item["Title"]);
                            txtL1TitleEn.Text = Convert.ToString(item["Title_EN"]);
                            txtL1Url.Text = item["URL"] != null ? new SPFieldUrlValue(Convert.ToString(item["URL"])).Url : "";
                            txtL1Order.Text = Convert.ToString(item["ItemOrder"]);
                            chkL1Visibility.Checked = item["Visibility"] != null && Convert.ToBoolean(item["Visibility"]);

                            litL1FormTitle.Text = "تعديل عنصر مستوى أول ID: " + id;
                            btnAddLevel1.Visible = false;
                            btnUpdateLevel1.Visible = true;
                            btnCancelL1Edit.Visible = true;
                        }
                    }
                });
            }
            else if (e.CommandName == "DeleteL1")
            {
                ExecuteAction((web) =>
                {
                    SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                    if (level1List != null)
                    {
                        SideMenuListProvisioner.DeleteItem(level1List, id);
                        ShowAlert("تم حذف العنصر بنجاح!", "info");
                    }
                });
            }
        }

        protected void gvLevel2Items_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (id <= 0) return;

            if (e.CommandName == "EditL2")
            {
                ExecuteAction((web) =>
                {
                    SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                    if (level2List != null)
                    {
                        SPListItem item = level2List.GetItemById(id);
                        if (item != null)
                        {
                            hfL2EditID.Value = item.ID.ToString();
                            txtL2TitleAr.Text = Convert.ToString(item["Title"]);
                            txtL2TitleEn.Text = Convert.ToString(item["Title_EN"]);
                            txtL2Url.Text = item["URL"] != null ? new SPFieldUrlValue(Convert.ToString(item["URL"])).Url : "";
                            txtL2Order.Text = Convert.ToString(item["ItemOrder"]);
                            chkL2Visibility.Checked = item["Visibility"] != null && Convert.ToBoolean(item["Visibility"]);

                            if (item["Parent"] != null)
                            {
                                SPFieldLookupValue parentLookup = new SPFieldLookupValue(Convert.ToString(item["Parent"]));
                                if (ddlL2Parent.Items.FindByValue(parentLookup.LookupId.ToString()) != null)
                                {
                                    ddlL2Parent.SelectedValue = parentLookup.LookupId.ToString();
                                }
                            }

                            litL2FormTitle.Text = "تعديل عنصر مستوى ثاني ID: " + id;
                            btnAddLevel2.Visible = false;
                            btnUpdateLevel2.Visible = true;
                            btnCancelL2Edit.Visible = true;
                        }
                    }
                });
            }
            else if (e.CommandName == "DeleteL2")
            {
                ExecuteAction((web) =>
                {
                    SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                    if (level2List != null)
                    {
                        SideMenuListProvisioner.DeleteItem(level2List, id);
                        ShowAlert("تم حذف عنصر المستوى الثاني بنجاح!", "info");
                    }
                });
            }
            else if (e.CommandName == "CreatePageRow")
            {
                ExecuteAction((web) =>
                {
                    SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                    if (level2List != null)
                    {
                        SPListItem item = level2List.GetItemById(id);
                        if (item != null)
                        {
                            string titleAr = Convert.ToString(item["Title"]);
                            string titleEn = Convert.ToString(item["Title_EN"]);
                            string urlStr = item["URL"] != null ? new SPFieldUrlValue(Convert.ToString(item["URL"])).Url : "";
                            string pageName = txtPageName.Text.Trim();

                            if (string.IsNullOrEmpty(pageName) && !string.IsNullOrEmpty(urlStr))
                            {
                                pageName = Path.GetFileName(urlStr);
                            }
                            if (string.IsNullOrEmpty(pageName))
                            {
                                pageName = "Page_" + id + ".aspx";
                            }

                            string ctrlPath = txtUserControlPath.Text.Trim();
                            string ctrlProps = txtUserControlProperties.Text.Trim();
                            string layoutUrl = txtPageLayoutUrl.Text.Trim();

                            SideMenuListProvisioner.EnsurePage(web, pageName, titleAr, titleEn, ctrlPath, ctrlProps, layoutUrl);

                            if (string.IsNullOrEmpty(urlStr))
                            {
                                urlStr = web.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + pageName;
                                item["URL"] = new SPFieldUrlValue { Url = urlStr, Description = titleAr };
                                item.Update();
                            }

                            ShowAlert("تم إنشاء وتجهيز الصفحة: " + pageName, "success");
                        }
                    }
                });
            }
        }

        protected void gvLevel2Items_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = e.Row.DataItem as DataRowView;
                if (rowView != null)
                {
                    string urlStr = Convert.ToString(rowView["URL"]);
                    Literal litBadge = e.Row.FindControl("litPageStatusBadge") as Literal;

                    if (litBadge != null)
                    {
                        if (string.IsNullOrEmpty(urlStr))
                        {
                            litBadge.Text = "<span class='badge bg-secondary'>لا يوجد رابط</span>";
                        }
                        else if (urlStr.IndexOf("/Pages/", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string pageFileName = Path.GetFileName(urlStr);
                            bool exists = false;

                            try
                            {
                                string targetUrl = TargetSiteUrl;
                                using (SPSite site = new SPSite(targetUrl))
                                using (SPWeb web = site.OpenWeb())
                                {
                                    exists = SideMenuListProvisioner.CheckPageExists(web, pageFileName);
                                }
                            }
                            catch { }

                            if (exists)
                            {
                                litBadge.Text = "<span class='badge bg-success'>موجودة ✓</span>";
                            }
                            else
                            {
                                litBadge.Text = "<span class='badge bg-warning text-dark'>غير موجودة ⚠</span>";
                            }
                        }
                        else
                        {
                            litBadge.Text = "<span class='badge bg-info text-dark'>رابط آخر</span>";
                        }
                    }
                }
            }
        }

        private List<string> GetTargetWebUrls()
        {
            List<string> urls = new List<string>();
            if (cblSubwebs != null)
            {
                foreach (ListItem item in cblSubwebs.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        if (!urls.Contains(item.Value))
                        {
                            urls.Add(item.Value);
                        }
                    }
                }
            }

            if (urls.Count == 0)
            {
                string primaryUrl = TargetSiteUrl;
                if (!string.IsNullOrEmpty(primaryUrl))
                {
                    urls.Add(primaryUrl);
                }
            }

            return urls;
        }

        private void ExecuteAction(Action<SPWeb> action, bool reloadSiteData = true)
        {
            List<string> targetUrls = GetTargetWebUrls();
            if (targetUrls == null || targetUrls.Count == 0)
            {
                ShowAlert("يرجى اختيار أو إدخال موقع استهداف صحيح", "warning");
                return;
            }

            List<string> processedWebs = new List<string>();
            List<string> failedWebs = new List<string>();

            foreach (string url in targetUrls)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (SPSite site = new SPSite(url))
                        using (SPWeb web = site.OpenWeb())
                        {
                            bool origAllowUnsafe = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                action(web);
                                processedWebs.Add(web.Title + " (" + web.Url + ")");
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = origAllowUnsafe;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    failedWebs.Add(url + " (" + ex.Message + ")");
                    Publics.WriteToLog(Request.Url.ToString(), "ucSideMenuListAdmin.ExecuteAction [" + url + "]", ex.Message);
                }
            }

            if (processedWebs.Count > 0)
            {
                string statusMsg = string.Format("تم تنفيذ الإجراء بنجاح على {0} موقع/مواقع: <br/>- {1}",
                    processedWebs.Count,
                    string.Join("<br/>- ", processedWebs.ToArray()));

                if (failedWebs.Count > 0)
                {
                    statusMsg += string.Format("<br/><span class='text-danger font-weight-bold'>حدث خطأ في المواقع التالية: <br/>- {0}</span>",
                        string.Join("<br/>- ", failedWebs.ToArray()));
                    ShowAlert(statusMsg, "warning");
                }
                else
                {
                    ShowAlert(statusMsg, "success");
                }
            }
            else if (failedWebs.Count > 0)
            {
                ShowAlert("فشل تنفيذ الإجراء على جميع المواقع المحددة:<br/>- " + string.Join("<br/>- ", failedWebs.ToArray()), "danger");
            }

            if (reloadSiteData)
            {
                LoadSiteData();
            }
        }

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = "alert alert-" + alertType + " alert-dismissible fade show mb-4";
            litAlertMessage.Text = message;
        }
    }
}
