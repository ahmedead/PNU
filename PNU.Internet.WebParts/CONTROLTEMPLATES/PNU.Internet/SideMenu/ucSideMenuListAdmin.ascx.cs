using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
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

        private class SPWebSummary
        {
            public string SiteTitle { get; set; }
            public string SiteUrl { get; set; }
            public string ServerRelativeUrl { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }

            public List<SPListSummary> Lists { get; set; }
            public List<SPPageSummary> Pages { get; set; }
            public List<SPMenuItemSummary> MenuLevel1Items { get; set; }
            public List<SPMenuItemSummary> MenuLevel2Items { get; set; }

            public SPWebSummary()
            {
                Lists = new List<SPListSummary>();
                Pages = new List<SPPageSummary>();
                MenuLevel1Items = new List<SPMenuItemSummary>();
                MenuLevel2Items = new List<SPMenuItemSummary>();
            }
        }

        private class SPListSummary
        {
            public string ListNameAr { get; set; }
            public string ListNameEn { get; set; }
            public string InternalName { get; set; }
            public int ItemCount { get; set; }
            public bool Exists { get; set; }
        }

        private class SPPageSummary
        {
            public string PageName { get; set; }
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string PageUrl { get; set; }
            public bool Exists { get; set; }
        }

        private class SPMenuItemSummary
        {
            public int ItemId { get; set; }
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string Url { get; set; }
            public int Order { get; set; }
            public bool Visible { get; set; }
            public string ParentTitleAr { get; set; }
            public string ParentTitleEn { get; set; }
        }

        private SPWebSummary CollectWebSummary(SPWeb web)
        {
            var summary = new SPWebSummary
            {
                SiteTitle = web.Title,
                SiteUrl = web.Url,
                ServerRelativeUrl = web.ServerRelativeUrl,
                Success = true
            };

            Dictionary<string, Tuple<string, string>> listNamesMap = new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { SideMenuListProvisioner.LIST_LEVEL1, new Tuple<string, string>("القائمة الجانبية (المستوى 1)", "Side Menu Level 1") },
                { SideMenuListProvisioner.LIST_LEVEL2, new Tuple<string, string>("القائمة الجانبية (المستوى 2)", "Side Menu Level 2") },

                // Deanship Lists (Dn...)
                { "DnDocuments", new Tuple<string, string>("مستندات ونماذج العمادة", "Deanship Documents") },
                { "DnBeneficiaryPathwaysTracks", new Tuple<string, string>("مسارات المستفيدين (المسارات)", "Beneficiary Pathways Tracks") },
                { "DnBeneficiaryPathwaysBullets", new Tuple<string, string>("مسارات المستفيدين (النقاط)", "Beneficiary Pathways Bullets") },
                { "DnServicesTracks", new Tuple<string, string>("خدمات العمادة (المسارات)", "Deanship Services Tracks") },
                { "DnServicesBullets", new Tuple<string, string>("خدمات العمادة (النقاط)", "Deanship Services Bullets") },
                { "DnInitiativesTracks", new Tuple<string, string>("مبادرات العمادة (المسارات)", "Deanship Initiatives Tracks") },
                { "DnInitiativesBullets", new Tuple<string, string>("مبادرات العمادة (النقاط)", "Deanship Initiatives Bullets") },
                { "DnAgencies", new Tuple<string, string>("وكالات العمادة", "Deanship Agencies") },
                { "DnDepartments", new Tuple<string, string>("إدارات العمادة", "Deanship Departments") },
                { "DnCenters", new Tuple<string, string>("مراكز العمادة", "Deanship Centers") },
                { "DnUnits", new Tuple<string, string>("وحدات العمادة", "Deanship Units") },

                // Agency Lists
                { "AgencyAchievements", new Tuple<string, string>("جوائز الوكالة والشهادات", "Agency Achievements") },
                { "AgencyDeens", new Tuple<string, string>("العمادات التابعة للوكالة", "Agency Deens") },
                { "AgencyDepartments", new Tuple<string, string>("الإدارات التابعة للوكالة", "Agency Departments") },
                { "AgencyCenters", new Tuple<string, string>("المراكز التابعة للوكالة", "Agency Centers") },
                { "AgencyUnits", new Tuple<string, string>("الوحدات التابعة للوكالة", "Agency Units") },

                // Faculty Lists
                { "AllFacultyDepartments", new Tuple<string, string>("أقسام الكلية", "Faculty Departments") }
            };

            // 1. Scan Known & Existing Custom Lists
            HashSet<string> processedLists = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kvp in listNamesMap)
            {
                string internalName = kvp.Key;
                SPList spList = web.Lists.TryGetList(internalName);
                if (spList != null)
                {
                    processedLists.Add(internalName);
                    summary.Lists.Add(new SPListSummary
                    {
                        InternalName = internalName,
                        ListNameAr = kvp.Value.Item1,
                        ListNameEn = kvp.Value.Item2,
                        Exists = true,
                        ItemCount = spList.ItemCount
                    });
                }
            }

            // Also check any other non-hidden custom list in web.Lists
            try
            {
                foreach (SPList list in web.Lists)
                {
                    try
                    {
                        if (list.Hidden) continue;
                        string title = list.Title;
                        if (processedLists.Contains(title)) continue;
                        if (title.Equals("Pages", StringComparison.OrdinalIgnoreCase) ||
                            title.Equals("Documents", StringComparison.OrdinalIgnoreCase) ||
                            title.Equals("Images", StringComparison.OrdinalIgnoreCase) ||
                            title.Equals("Site Assets", StringComparison.OrdinalIgnoreCase) ||
                            title.Equals("Microfeed", StringComparison.OrdinalIgnoreCase))
                            continue;

                        processedLists.Add(title);
                        summary.Lists.Add(new SPListSummary
                        {
                            InternalName = title,
                            ListNameAr = title,
                            ListNameEn = title,
                            Exists = true,
                            ItemCount = list.ItemCount
                        });
                    }
                    catch { }
                }
            }
            catch { }

            // 2. Read actual items from SideMenuLevel1 and SideMenuLevel2
            try
            {
                SPList level1List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL1);
                if (level1List != null && level1List.ItemCount > 0)
                {
                    SPQuery q1 = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in level1List.GetItems(q1))
                    {
                        try
                        {
                            string titleAr = item["Title"] != null ? Convert.ToString(item["Title"]) : "";
                            string titleEn = item.Fields.ContainsField("Title_EN") && item["Title_EN"] != null ? Convert.ToString(item["Title_EN"]) : "";
                            string url = "";
                            if (item.Fields.ContainsField("URL") && item["URL"] != null)
                            {
                                SPFieldUrlValue urlVal = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                url = urlVal.Url ?? "";
                            }
                            int order = 0;
                            if (item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null)
                            {
                                double dOrder = 0;
                                double.TryParse(Convert.ToString(item["ItemOrder"]), out dOrder);
                                order = (int)dOrder;
                            }
                            bool visible = true;
                            if (item.Fields.ContainsField("Visibility") && item["Visibility"] != null)
                                visible = Convert.ToBoolean(item["Visibility"]);

                            summary.MenuLevel1Items.Add(new SPMenuItemSummary
                            {
                                ItemId = item.ID,
                                TitleAr = titleAr,
                                TitleEn = string.IsNullOrEmpty(titleEn) ? titleAr : titleEn,
                                Url = url,
                                Order = order,
                                Visible = visible
                            });
                        }
                        catch { }
                    }
                }

                SPList level2List = web.Lists.TryGetList(SideMenuListProvisioner.LIST_LEVEL2);
                if (level2List != null && level2List.ItemCount > 0)
                {
                    SPQuery q2 = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in level2List.GetItems(q2))
                    {
                        try
                        {
                            string titleAr = item["Title"] != null ? Convert.ToString(item["Title"]) : "";
                            string titleEn = item.Fields.ContainsField("Title_EN") && item["Title_EN"] != null ? Convert.ToString(item["Title_EN"]) : "";
                            string url = "";
                            if (item.Fields.ContainsField("URL") && item["URL"] != null)
                            {
                                SPFieldUrlValue urlVal = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                url = urlVal.Url ?? "";
                            }
                            int order = 0;
                            if (item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null)
                            {
                                double dOrder = 0;
                                double.TryParse(Convert.ToString(item["ItemOrder"]), out dOrder);
                                order = (int)dOrder;
                            }
                            bool visible = true;
                            if (item.Fields.ContainsField("Visibility") && item["Visibility"] != null)
                                visible = Convert.ToBoolean(item["Visibility"]);

                            string parentTitleAr = "";
                            string parentTitleEn = "";
                            if (item.Fields.ContainsField("Parent") && item["Parent"] != null)
                            {
                                SPFieldLookupValue lkp = new SPFieldLookupValue(Convert.ToString(item["Parent"]));
                                parentTitleAr = lkp.LookupValue ?? "";
                                // Try to match parent title EN from Level1 items already collected
                                var parentMatch = summary.MenuLevel1Items.Find(m => m.ItemId == lkp.LookupId);
                                parentTitleEn = parentMatch != null ? parentMatch.TitleEn : parentTitleAr;
                            }

                            summary.MenuLevel2Items.Add(new SPMenuItemSummary
                            {
                                ItemId = item.ID,
                                TitleAr = titleAr,
                                TitleEn = string.IsNullOrEmpty(titleEn) ? titleAr : titleEn,
                                Url = url,
                                Order = order,
                                Visible = visible,
                                ParentTitleAr = parentTitleAr,
                                ParentTitleEn = parentTitleEn
                            });
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "CollectWebSummary.MenuItems", ex.Message);
            }

            // 3. Scan Pages Library
            try
            {
                SPList pagesList = web.Lists.TryGetList("Pages");
                if (pagesList != null)
                {
                    foreach (SPListItem item in pagesList.GetItems())
                    {
                        try
                        {
                            string fileName = item.Name;
                            if (string.IsNullOrEmpty(fileName) && item.File != null)
                            {
                                fileName = item.File.Name;
                            }
                            if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                                continue;

                            string titleAr = "";
                            if (item.Fields.ContainsField("Title") && item["Title"] != null)
                            {
                                titleAr = Convert.ToString(item["Title"]);
                            }
                            if (string.IsNullOrEmpty(titleAr)) titleAr = fileName;

                            string titleEn = "";
                            if (item.Fields.ContainsField("Title_EN") && item["Title_EN"] != null)
                            {
                                titleEn = Convert.ToString(item["Title_EN"]);
                            }
                            if (string.IsNullOrEmpty(titleEn)) titleEn = titleAr;

                            string pageUrl = "";
                            if (item.File != null)
                            {
                                pageUrl = item.File.ServerRelativeUrl;
                            }
                            else
                            {
                                pageUrl = web.ServerRelativeUrl.TrimEnd('/') + "/Pages/" + fileName;
                            }

                            summary.Pages.Add(new SPPageSummary
                            {
                                PageName = fileName,
                                TitleAr = titleAr,
                                TitleEn = titleEn,
                                PageUrl = pageUrl,
                                Exists = true
                            });
                        }
                        catch (Exception pageEx)
                        {
                            Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "CollectWebSummary.PageItem", pageEx.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "CollectWebSummary.PagesList", ex.Message);
            }

            return summary;
        }

        private string GetAbsoluteUrl(string siteUrl, string pageUrl)
        {
            if (string.IsNullOrEmpty(pageUrl)) return "";
            pageUrl = pageUrl.Trim();
            if (pageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                pageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return pageUrl;
            }

            try
            {
                if (string.IsNullOrEmpty(siteUrl)) return pageUrl;
                string baseUrl = siteUrl.TrimEnd('/') + "/";
                Uri baseUri = new Uri(baseUrl);
                if (pageUrl.StartsWith("/"))
                {
                    string authority = baseUri.GetLeftPart(UriPartial.Authority);
                    return authority + pageUrl;
                }
                else
                {
                    return new Uri(baseUri, pageUrl).ToString();
                }
            }
            catch
            {
                return pageUrl;
            }
        }

        private void RenderBilingualExecutionSummary(List<SPWebSummary> webSummaries)
        {
            if (webSummaries == null || webSummaries.Count == 0) return;

            // Populate the multiline TextBox with the page titles and full absolute URLs
            StringBuilder textSummary = new StringBuilder();
            foreach (var webSum in webSummaries)
            {
                if (!webSum.Success) continue;

                HashSet<string> addedUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                int pageCounter = 1;

                // 1. Gather Level 1 items with page links
                foreach (var m in webSum.MenuLevel1Items)
                {
                    if (string.IsNullOrEmpty(m.Url)) continue;
                    string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                    if (addedUrls.Contains(absUrl)) continue;
                    addedUrls.Add(absUrl);

                    string title = string.IsNullOrEmpty(m.TitleAr) ? m.TitleEn : m.TitleAr;
                    textSummary.AppendLine(string.Format("{0}- {1} :\t{2}", pageCounter++, title, absUrl));
                }

                // 2. Gather Level 2 items with page links
                foreach (var m in webSum.MenuLevel2Items)
                {
                    if (string.IsNullOrEmpty(m.Url)) continue;
                    string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                    if (addedUrls.Contains(absUrl)) continue;
                    addedUrls.Add(absUrl);

                    string title = string.IsNullOrEmpty(m.TitleAr) ? m.TitleEn : m.TitleAr;
                    textSummary.AppendLine(string.Format("{0}- {1} :\t{2}", pageCounter++, title, absUrl));
                }

                // 3. Gather Pages library items
                foreach (var p in webSum.Pages)
                {
                    if (string.IsNullOrEmpty(p.PageUrl)) continue;
                    string absUrl = GetAbsoluteUrl(webSum.SiteUrl, p.PageUrl);
                    if (addedUrls.Contains(absUrl)) continue;
                    addedUrls.Add(absUrl);

                    string title = string.IsNullOrEmpty(p.TitleAr) ? (string.IsNullOrEmpty(p.TitleEn) ? p.PageName : p.TitleEn) : p.TitleAr;
                    textSummary.AppendLine(string.Format("{0}- {1} :\t{2}", pageCounter++, title, absUrl));
                }
            }

            if (txtPagesSummary != null)
            {
                txtPagesSummary.Text = textSummary.ToString().TrimEnd();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='summary-bilingual-wrapper'>");

            // Navigation pill buttons for switching view
            sb.Append("<div class='d-flex justify-content-start mb-3 border-bottom pb-2'>");
            sb.Append("<button type='button' onclick='switchSummaryTab(\"ar\")' class='btn btn-outline-primary active font-weight-bold me-2' id='btnTabAr'>🇸🇦 التقرير باللغة العربية (Arabic)</button>");
            sb.Append("<button type='button' onclick='switchSummaryTab(\"en\")' class='btn btn-outline-primary font-weight-bold' id='btnTabEn'>🇬🇧 Report in English (الإنجليزية)</button>");
            sb.Append("</div>");

            sb.Append("<script type='text/javascript'>");
            sb.Append("function switchSummaryTab(lang) {");
            sb.Append("  var arContent = document.getElementById('ar-summary-view');");
            sb.Append("  var enContent = document.getElementById('en-summary-view');");
            sb.Append("  var btnAr = document.getElementById('btnTabAr');");
            sb.Append("  var btnEn = document.getElementById('btnTabEn');");
            sb.Append("  if (lang === 'ar') {");
            sb.Append("    if(arContent) arContent.style.display = 'block';");
            sb.Append("    if(enContent) enContent.style.display = 'none';");
            sb.Append("    if(btnAr) { btnAr.classList.add('active', 'btn-primary'); btnAr.classList.remove('btn-outline-primary'); }");
            sb.Append("    if(btnEn) { btnEn.classList.remove('active', 'btn-primary'); btnEn.classList.add('btn-outline-primary'); }");
            sb.Append("  } else {");
            sb.Append("    if(arContent) arContent.style.display = 'none';");
            sb.Append("    if(enContent) enContent.style.display = 'block';");
            sb.Append("    if(btnEn) { btnEn.classList.add('active', 'btn-primary'); btnEn.classList.remove('btn-outline-primary'); }");
            sb.Append("    if(btnAr) { btnAr.classList.remove('active', 'btn-primary'); btnAr.classList.add('btn-outline-primary'); }");
            sb.Append("  }");
            sb.Append("}");
            sb.Append("</script>");

            // -------------------------------------------------------------
            // ARABIC VIEW
            // -------------------------------------------------------------
            sb.Append("<div id='ar-summary-view' dir='rtl'>");
            foreach (var webSum in webSummaries)
            {
                sb.Append("<div class='card mb-3 border-primary'>");
                sb.AppendFormat("<div class='card-header bg-primary text-white font-weight-bold'>📍 الموقع المستهدف: {0} ({1})</div>",
                    HttpUtility.HtmlEncode(webSum.SiteTitle ?? "موقع غير معروف"),
                    HttpUtility.HtmlEncode(webSum.SiteUrl ?? ""));

                sb.Append("<div class='card-body'>");

                if (!webSum.Success)
                {
                    sb.AppendFormat("<div class='alert alert-danger'>فشلت العملية على هذا الموقع: {0}</div>", HttpUtility.HtmlEncode(webSum.ErrorMessage));
                }
                else
                {
                    // --- SubMenu Level 1 Items (Arabic) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 عناصر القائمة الجانبية - المستوى الأول (عدد العناصر: {0}):</h6>", webSum.MenuLevel1Items.Count);
                    if (webSum.MenuLevel1Items.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>لا توجد عناصر في القائمة الجانبية - المستوى الأول.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive mb-4'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>العنوان بالعربية</th><th>العنوان بالإنجليزية</th><th>الرابط (URL)</th><th>الترتيب</th><th>الحالة</th></tr></thead>");
                        sb.Append("<tbody>");
                        int idx = 1;
                        foreach (var m in webSum.MenuLevel1Items)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                            string urlCell = string.IsNullOrEmpty(m.Url)
                                ? "<span class='text-muted'>—</span>"
                                : string.Format("<a href='{0}' target='_blank' class='text-primary'>{0}</a>", HttpUtility.HtmlEncode(absUrl));
                            string visBadge = m.Visible
                                ? "<span class='badge bg-success'>ظاهر ✓</span>"
                                : "<span class='badge bg-secondary'>مخفي ✗</span>";

                            sb.AppendFormat("<tr><td>{0}</td><td><strong>{1}</strong></td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                                idx++,
                                HttpUtility.HtmlEncode(m.TitleAr),
                                HttpUtility.HtmlEncode(m.TitleEn),
                                urlCell,
                                m.Order,
                                visBadge);
                        }
                        sb.Append("</tbody></table></div>");
                    }

                    // --- SubMenu Level 2 Items (Arabic) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 عناصر القائمة الجانبية - المستوى الثاني (عدد العناصر: {0}):</h6>", webSum.MenuLevel2Items.Count);
                    if (webSum.MenuLevel2Items.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>لا توجد عناصر في القائمة الجانبية - المستوى الثاني.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive mb-4'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>العنصر الأب</th><th>العنوان بالعربية</th><th>العنوان بالإنجليزية</th><th>الرابط (URL)</th><th>الترتيب</th><th>الحالة</th></tr></thead>");
                        sb.Append("<tbody>");
                        int idx2 = 1;
                        foreach (var m in webSum.MenuLevel2Items)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                            string urlCell = string.IsNullOrEmpty(m.Url)
                                ? "<span class='text-muted'>—</span>"
                                : string.Format("<a href='{0}' target='_blank' class='text-primary'>{0}</a>", HttpUtility.HtmlEncode(absUrl));
                            string visBadge = m.Visible
                                ? "<span class='badge bg-success'>ظاهر ✓</span>"
                                : "<span class='badge bg-secondary'>مخفي ✗</span>";

                            sb.AppendFormat("<tr><td>{0}</td><td><span class='badge bg-info text-dark'>{1}</span></td><td><strong>{2}</strong></td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                                idx2++,
                                HttpUtility.HtmlEncode(m.ParentTitleAr),
                                HttpUtility.HtmlEncode(m.TitleAr),
                                HttpUtility.HtmlEncode(m.TitleEn),
                                urlCell,
                                m.Order,
                                visBadge);
                        }
                        sb.Append("</tbody></table></div>");
                    }

                    // --- Lists Table (Arabic) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 القوائم المنشأة والمفحوصة تحت الموقع (عدد القوائم المفحوصة: {0}):</h6>", webSum.Lists.Count);
                    sb.Append("<div class='table-responsive mb-4'>");
                    sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                    sb.Append("<thead class='table-dark'><tr><th>اسم القائمة بالعربية</th><th>الاسم الداخلي (Internal Name)</th><th>حالة القائمة</th><th>عدد العناصر</th></tr></thead>");
                    sb.Append("<tbody>");
                    foreach (var l in webSum.Lists)
                    {
                        string badge = l.Exists
                            ? "<span class='badge bg-success'>موجودة وجاهزة ✓</span>"
                            : "<span class='badge bg-secondary'>غير منشأة ✗</span>";

                        sb.AppendFormat("<tr><td><strong>{0}</strong></td><td><code>{1}</code></td><td>{2}</td><td><span class='badge bg-info text-dark'>{3} عنصر</span></td></tr>",
                            HttpUtility.HtmlEncode(l.ListNameAr),
                            HttpUtility.HtmlEncode(l.InternalName),
                            badge,
                            l.ItemCount);
                    }
                    sb.Append("</tbody></table></div>");

                    // --- Pages Table (Arabic) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📄 الصفحات المنشأة والمجهزة تحت الموقع (إجمالي عدد الصفحات: {0}):</h6>", webSum.Pages.Count);
                    if (webSum.Pages.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>لا توجد صفحات منشأة حالياً في مكتبة الصفحات.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>اسم ملف الصفحة</th><th>العنوان بالعربية</th><th>الرابط المباشر</th><th>حالة الصفحة</th></tr></thead>");
                        sb.Append("<tbody>");
                        int pIdx = 1;
                        foreach (var p in webSum.Pages)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, p.PageUrl);
                            sb.AppendFormat("<tr><td>{0}</td><td><code>{1}</code></td><td>{2}</td><td><a href='{3}' target='_blank' class='text-primary'>{3}</a></td><td><span class='badge bg-success'>مجهزة وموجودة ✓</span></td></tr>",
                                pIdx++,
                                HttpUtility.HtmlEncode(p.PageName),
                                HttpUtility.HtmlEncode(p.TitleAr),
                                HttpUtility.HtmlEncode(absUrl));
                        }
                        sb.Append("</tbody></table></div>");
                    }
                }

                sb.Append("</div></div>");
            }
            sb.Append("</div>"); // End Arabic View

            // -------------------------------------------------------------
            // ENGLISH VIEW
            // -------------------------------------------------------------
            sb.Append("<div id='en-summary-view' dir='ltr' style='display:none;'>");
            foreach (var webSum in webSummaries)
            {
                sb.Append("<div class='card mb-3 border-primary'>");
                sb.AppendFormat("<div class='card-header bg-primary text-white font-weight-bold'>📍 Target Site: {0} ({1})</div>",
                    HttpUtility.HtmlEncode(webSum.SiteTitle ?? "Unknown Site"),
                    HttpUtility.HtmlEncode(webSum.SiteUrl ?? ""));

                sb.Append("<div class='card-body'>");

                if (!webSum.Success)
                {
                    sb.AppendFormat("<div class='alert alert-danger'>Execution failed on this site: {0}</div>", HttpUtility.HtmlEncode(webSum.ErrorMessage));
                }
                else
                {
                    // --- SubMenu Level 1 Items (English) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 SubMenu Level 1 Items (Total: {0}):</h6>", webSum.MenuLevel1Items.Count);
                    if (webSum.MenuLevel1Items.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>No SubMenu Level 1 items found.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive mb-4'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>Title (AR)</th><th>Title (EN)</th><th>URL</th><th>Order</th><th>Status</th></tr></thead>");
                        sb.Append("<tbody>");
                        int idx = 1;
                        foreach (var m in webSum.MenuLevel1Items)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                            string urlCell = string.IsNullOrEmpty(m.Url)
                                ? "<span class='text-muted'>—</span>"
                                : string.Format("<a href='{0}' target='_blank' class='text-primary'>{0}</a>", HttpUtility.HtmlEncode(absUrl));
                            string visBadge = m.Visible
                                ? "<span class='badge bg-success'>Visible ✓</span>"
                                : "<span class='badge bg-secondary'>Hidden ✗</span>";

                            sb.AppendFormat("<tr><td>{0}</td><td><strong>{1}</strong></td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                                idx++,
                                HttpUtility.HtmlEncode(m.TitleAr),
                                HttpUtility.HtmlEncode(m.TitleEn),
                                urlCell,
                                m.Order,
                                visBadge);
                        }
                        sb.Append("</tbody></table></div>");
                    }

                    // --- SubMenu Level 2 Items (English) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 SubMenu Level 2 Items (Total: {0}):</h6>", webSum.MenuLevel2Items.Count);
                    if (webSum.MenuLevel2Items.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>No SubMenu Level 2 items found.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive mb-4'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>Parent</th><th>Title (AR)</th><th>Title (EN)</th><th>URL</th><th>Order</th><th>Status</th></tr></thead>");
                        sb.Append("<tbody>");
                        int idx2 = 1;
                        foreach (var m in webSum.MenuLevel2Items)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, m.Url);
                            string urlCell = string.IsNullOrEmpty(m.Url)
                                ? "<span class='text-muted'>—</span>"
                                : string.Format("<a href='{0}' target='_blank' class='text-primary'>{0}</a>", HttpUtility.HtmlEncode(absUrl));
                            string visBadge = m.Visible
                                ? "<span class='badge bg-success'>Visible ✓</span>"
                                : "<span class='badge bg-secondary'>Hidden ✗</span>";

                            sb.AppendFormat("<tr><td>{0}</td><td><span class='badge bg-info text-dark'>{1}</span></td><td><strong>{2}</strong></td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                                idx2++,
                                HttpUtility.HtmlEncode(m.ParentTitleEn),
                                HttpUtility.HtmlEncode(m.TitleAr),
                                HttpUtility.HtmlEncode(m.TitleEn),
                                urlCell,
                                m.Order,
                                visBadge);
                        }
                        sb.Append("</tbody></table></div>");
                    }

                    // --- Lists Table (English) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📋 Lists Created & Verified Under Site (Total Verified Lists: {0}):</h6>", webSum.Lists.Count);
                    sb.Append("<div class='table-responsive mb-4'>");
                    sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                    sb.Append("<thead class='table-dark'><tr><th>List Name (EN)</th><th>Internal Name</th><th>List Status</th><th>Item Count</th></tr></thead>");
                    sb.Append("<tbody>");
                    foreach (var l in webSum.Lists)
                    {
                        string badge = l.Exists
                            ? "<span class='badge bg-success'>Exists & Ready ✓</span>"
                            : "<span class='badge bg-secondary'>Not Created ✗</span>";

                        sb.AppendFormat("<tr><td><strong>{0}</strong></td><td><code>{1}</code></td><td>{2}</td><td><span class='badge bg-info text-dark'>{3} items</span></td></tr>",
                            HttpUtility.HtmlEncode(l.ListNameEn),
                            HttpUtility.HtmlEncode(l.InternalName),
                            badge,
                            l.ItemCount);
                    }
                    sb.Append("</tbody></table></div>");

                    // --- Pages Table (English) ---
                    sb.AppendFormat("<h6 class='font-weight-bold text-dark border-bottom pb-2 mb-3'>📄 Pages Created & Provisioned Under Site (Total Created Pages: {0}):</h6>", webSum.Pages.Count);
                    if (webSum.Pages.Count == 0)
                    {
                        sb.Append("<div class='text-muted mb-3'>No pages found in Pages library.</div>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive'>");
                        sb.Append("<table class='table table-bordered table-striped table-sm align-middle'>");
                        sb.Append("<thead class='table-dark'><tr><th>#</th><th>Page File Name</th><th>English Title</th><th>Absolute URL</th><th>Status</th></tr></thead>");
                        sb.Append("<tbody>");
                        int pIdx = 1;
                        foreach (var p in webSum.Pages)
                        {
                            string absUrl = GetAbsoluteUrl(webSum.SiteUrl, p.PageUrl);
                            sb.AppendFormat("<tr><td>{0}</td><td><code>{1}</code></td><td>{2}</td><td><a href='{3}' target='_blank' class='text-primary'>{3}</a></td><td><span class='badge bg-success'>Provisioned & Ready ✓</span></td></tr>",
                                pIdx++,
                                HttpUtility.HtmlEncode(p.PageName),
                                HttpUtility.HtmlEncode(p.TitleEn),
                                HttpUtility.HtmlEncode(absUrl));
                        }
                        sb.Append("</tbody></table></div>");
                    }
                }

                sb.Append("</div></div>");
            }
            sb.Append("</div>"); // End English View

            sb.Append("</div>"); // End Wrapper

            if (litExecutionSummaryContent != null)
            {
                litExecutionSummaryContent.Text = sb.ToString();
            }
            if (pnlExecutionSummary != null)
            {
                pnlExecutionSummary.Visible = true;
            }
        }

        protected void btnCloseSummary_Click(object sender, EventArgs e)
        {
            if (pnlExecutionSummary != null)
            {
                pnlExecutionSummary.Visible = false;
            }
        }

        protected void btnExportSummary_Click(object sender, EventArgs e)
        {
            List<string> targetUrls = GetTargetWebUrls();
            if (targetUrls == null || targetUrls.Count == 0)
            {
                ShowAlert("يرجى اختيار أو إدخال موقع استهداف صحيح لتصدير الملخص. (No target site selected)", "warning");
                return;
            }

            List<SPWebSummary> webSummaries = new List<SPWebSummary>();
            foreach (string url in targetUrls)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (SPSite site = new SPSite(url))
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPWebSummary summary = CollectWebSummary(web);
                            webSummaries.Add(summary);
                        }
                    });
                }
                catch (Exception ex)
                {
                    webSummaries.Add(new SPWebSummary
                    {
                        SiteUrl = url,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                }
            }

            if (webSummaries.Count == 0)
            {
                ShowAlert("لا يوجد ملخص تنفيذ لتصديره. (No summary to export)", "warning");
                return;
            }

            StringBuilder csv = new StringBuilder();
            string sep = "\t";

            foreach (var webSum in webSummaries)
            {
                csv.AppendLine("==========================================================");
                csv.AppendLine("Site / الموقع" + sep + (webSum.SiteTitle ?? "") + sep + (webSum.SiteUrl ?? ""));
                csv.AppendLine("==========================================================");

                if (!webSum.Success)
                {
                    csv.AppendLine("Error / خطأ" + sep + (webSum.ErrorMessage ?? ""));
                    csv.AppendLine();
                    continue;
                }

                // --- SubMenu Level 1 ---
                csv.AppendLine();
                csv.AppendLine("--- SubMenu Level 1 / عناصر المستوى الأول ---");
                csv.AppendLine("#" + sep + "Title AR / العنوان بالعربية" + sep + "Title EN / العنوان بالإنجليزية" + sep + "URL / الرابط" + sep + "Order / الترتيب" + sep + "Status / الحالة");
                int idx = 1;
                foreach (var m in webSum.MenuLevel1Items)
                {
                    csv.AppendLine(string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}",
                        idx++, sep,
                        m.TitleAr ?? "", m.TitleEn ?? "",
                        m.Url ?? "", m.Order,
                        m.Visible ? "Visible / ظاهر" : "Hidden / مخفي"));
                }

                // --- SubMenu Level 2 ---
                csv.AppendLine();
                csv.AppendLine("--- SubMenu Level 2 / عناصر المستوى الثاني ---");
                csv.AppendLine("#" + sep + "Parent AR / العنصر الأب" + sep + "Title AR / العنوان بالعربية" + sep + "Title EN / العنوان بالإنجليزية" + sep + "URL / الرابط" + sep + "Order / الترتيب" + sep + "Status / الحالة");
                int idx2 = 1;
                foreach (var m in webSum.MenuLevel2Items)
                {
                    csv.AppendLine(string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}{1}{7}",
                        idx2++, sep,
                        m.ParentTitleAr ?? "", m.TitleAr ?? "",
                        m.TitleEn ?? "", m.Url ?? "",
                        m.Order,
                        m.Visible ? "Visible / ظاهر" : "Hidden / مخفي"));
                }

                // --- Pages ---
                csv.AppendLine();
                csv.AppendLine("--- Pages / الصفحات المنشأة ---");
                csv.AppendLine("#" + sep + "Page Name / اسم الصفحة" + sep + "Title AR / العنوان بالعربية" + sep + "Title EN / العنوان بالإنجليزية" + sep + "URL / الرابط");
                int pIdx = 1;
                foreach (var p in webSum.Pages)
                {
                    csv.AppendLine(string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}",
                        pIdx++, sep,
                        p.PageName ?? "", p.TitleAr ?? "",
                        p.TitleEn ?? "", p.PageUrl ?? ""));
                }

                // --- Lists ---
                csv.AppendLine();
                csv.AppendLine("--- Lists / القوائم ---");
                csv.AppendLine("List Name AR / اسم القائمة بالعربية" + sep + "List Name EN / الاسم بالإنجليزية" + sep + "Internal Name / الاسم الداخلي" + sep + "Status / الحالة" + sep + "Item Count / عدد العناصر");
                foreach (var l in webSum.Lists)
                {
                    csv.AppendLine(string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}",
                        l.ListNameAr ?? "", sep,
                        l.ListNameEn ?? "", l.InternalName ?? "",
                        l.Exists ? "Exists / موجودة" : "Not Created / غير منشأة",
                        l.ItemCount));
                }

                csv.AppendLine();
            }

            // Write as Excel-compatible tab-separated CSV with UTF-8 BOM
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            string fileName = "SideMenu_Summary_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            // Write UTF-8 BOM for proper Arabic display in Excel
            Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
            Response.Write(csv.ToString());
            Response.Flush();
            Response.End();
        }

        private void ExecuteAction(Action<SPWeb> action, bool reloadSiteData = true)
        {
            List<string> targetUrls = GetTargetWebUrls();
            if (targetUrls == null || targetUrls.Count == 0)
            {
                ShowAlert("يرجى اختيار أو إدخال موقع استهداف صحيح", "warning");
                return;
            }

            List<SPWebSummary> webSummaries = new List<SPWebSummary>();
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

                                SPWebSummary summary = CollectWebSummary(web);
                                webSummaries.Add(summary);
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
                    webSummaries.Add(new SPWebSummary
                    {
                        SiteUrl = url,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
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

                RenderBilingualExecutionSummary(webSummaries);
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
            if (pnlAlert != null)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "alert alert-" + alertType + " alert-dismissible fade show mb-4";
            }
            if (litAlertMessage != null)
            {
                litAlertMessage.Text = message;
            }
        }
    }
}
