using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.OpenData
{
    public partial class ucOpenData : UserControl
    {
        // ------------------------------------------------------------------
        //  List name provisioned + read by this control
        // ------------------------------------------------------------------
        private const string ListName = "OpenDataSections";

        protected bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private List<OpenDataTab> _tabs = new List<OpenDataTab>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    litSectionTitle.Text = IsArabic ? "البيانات المفتوحة" : "Open Data";
                    litEmpty.Text = IsArabic ? "لا توجد بيانات لعرضها حالياً." : "No data available at this time.";

                    EnsureOpenDataList();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  Provisioning (self-contained in the control, no shell access needed)
        // ------------------------------------------------------------------
        private void EnsureOpenDataList()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb("admin"))
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists.TryGetList(ListName);
                    if (list == null)
                    {
                        Guid listId = web.Lists.Add(ListName, "Open Data page tabs (BiLingual)", SPListTemplateType.GenericList);
                        list = web.Lists[listId];
                        list.OnQuickLaunch = false;
                        list.Update();
                    }

                    EnsureField(list, "Title_EN", SPFieldType.Text);
                    EnsureField(list, "IconClass", SPFieldType.Text);
                    EnsureField(list, "Description", SPFieldType.Note);
                    EnsureField(list, "Description_EN", SPFieldType.Note);
                    EnsureField(list, "Bullets", SPFieldType.Note);
                    EnsureField(list, "Bullets_EN", SPFieldType.Note);
                    EnsureField(list, "ItemOrder", SPFieldType.Number);
                    EnsureField(list, "Visibility", SPFieldType.Boolean);

                    EnsureDefaultViewFields(list, new[] { "Title_EN", "IconClass", "ItemOrder", "Visibility" });

                    if (list.Items.Count == 0)
                    {
                        SeedDefaultData(list);
                    }

                    web.AllowUnsafeUpdates = false;
                }
            });
        }

        private static void EnsureField(SPList list, string fieldName, SPFieldType fieldType)
        {
            if (!list.Fields.ContainsField(fieldName))
            {
                list.Fields.Add(fieldName, fieldType, false);
                if (fieldType == SPFieldType.Note)
                {
                    SPFieldMultiLineText note = (SPFieldMultiLineText)list.Fields.GetFieldByInternalName(fieldName);
                    note.NumberOfLines = 6;
                    note.Update();
                }
                list.Update();
            }
        }

        private static void EnsureDefaultViewFields(SPList list, string[] fieldNames)
        {
            SPView view = list.DefaultView;
            foreach (string f in fieldNames)
            {
                if (!view.ViewFields.Exists(f))
                    view.ViewFields.Add(f);
            }
            view.Update();
        }

        private static void SeedDefaultData(SPList list)
        {
            AddRow(list, 1,
                "البيانات المفتوحة", "Open Data",
                "hgi-database",
                "وصف واضح ومختصر عن البيانات المفتوحة، وأهميتها في تعزيز الشفافية وإتاحة المعلومات العامة بصيغ قابلة للاستخدام وإعادة الاستخدام.",
                "A clear, concise description of open data and its importance in enhancing transparency and providing public information in reusable, machine-readable formats.",
                new[] {
                    "توفير بيانات عامة قابلة للوصول والاستخدام.",
                    "رفع مستوى الشفافية ودعم البحث والتحليل.",
                    "تمكين المستفيدين من بناء حلول ومعارف قائمة على البيانات."
                },
                new[] {
                    "Providing accessible and reusable public data.",
                    "Raising the level of transparency and supporting research and analysis.",
                    "Enabling beneficiaries to build data-driven solutions and knowledge."
                });

            AddRow(list, 2,
                "سياسة البيانات المفتوحة", "Open Data Policy",
                "hgi-agreement-01",
                "توضح السياسة المبادئ العامة لنشر البيانات المفتوحة واستخدامها، بما يشمل الالتزام بجودة البيانات، وتحديثها، وبيان شروط إعادة استخدامها.",
                "The policy outlines the general principles for publishing and using open data, including commitment to data quality, regular updates, and clear terms of reuse.",
                new[] {
                    "الالتزام بذكر مصدر البيانات عند إعادة استخدامها.",
                    "عدم استخدام البيانات لأغراض مخالفة للأنظمة.",
                    "مراعاة خصوصية البيانات المصنفة أو المحمية."
                },
                new[] {
                    "Attributing the data source when it is reused.",
                    "Not using the data for purposes that violate applicable regulations.",
                    "Respecting the privacy of classified or protected data."
                });

            AddRow(list, 3,
                "مكتبة البيانات المفتوحة", "Open Data Library",
                "hgi-folder-library",
                "تضم مكتبة البيانات المفتوحة مجموعات البيانات المنشورة أو الروابط المؤدية إلى مصادرها الرسمية.",
                "The open data library includes published datasets or links to their official sources.",
                new[] {
                    "يمكن الوصول إلى مجموعات البيانات عبر بوابة البيانات المفتوحة السعودية.",
                    "تُنظم البيانات حسب التصنيف والجهة والموضوع."
                },
                new[] {
                    "Datasets can be accessed through the Saudi Open Data Portal.",
                    "Data is organized by classification, entity, and topic."
                });

            AddRow(list, 4,
                "حالات الاستخدام للبيانات المفتوحة", "Open Data Use Cases",
                "hgi-task-done-01",
                "تستعرض حالات الاستخدام أمثلة لكيفية الاستفادة من البيانات المفتوحة في الدراسات، والتحليل، وتطوير الخدمات.",
                "Use cases showcase examples of how open data can be leveraged in studies, analysis, and service development.",
                new[] {
                    "تحليل احتياجات المستفيدين.",
                    "دعم الدراسات والبحوث.",
                    "تحسين تجربة الخدمات الرقمية."
                },
                new[] {
                    "Analyzing beneficiary needs.",
                    "Supporting studies and research.",
                    "Improving the digital services experience."
                });

            AddRow(list, 5,
                "البيانات الجيومكانية", "Geospatial Data",
                "hgi-location-01",
                "تعرض البيانات الجيومكانية عند توفرها معلومات مرتبطة بالموقع والنطاق الجغرافي للخدمات والمرافق.",
                "When available, geospatial data presents information related to the location and geographic scope of services and facilities.",
                new[] {
                    "خرائط المرافق.",
                    "توزيع الخدمات حسب الموقع.",
                    "مؤشرات مكانية داعمة لاتخاذ القرار."
                },
                new[] {
                    "Facility maps.",
                    "Distribution of services by location.",
                    "Spatial indicators supporting decision-making."
                });

            AddRow(list, 6,
                "البيانات اللحظية", "Live Data",
                "hgi-activity-01",
                "توضح البيانات اللحظية أو شبه اللحظية المعلومات التي يمكن تحديثها بشكل دوري حسب توفر مصادرها.",
                "Live or near-live data reflects information that can be updated periodically depending on the availability of its sources.",
                new[] {
                    "مؤشرات الاستخدام.",
                    "بيانات تشغيلية قابلة للتحديث.",
                    "لوحات متابعة عند توفرها."
                },
                new[] {
                    "Usage indicators.",
                    "Updatable operational data.",
                    "Monitoring dashboards where available."
                });
        }

        private static void AddRow(SPList list, int order, string titleAr, string titleEn, string iconClass,
            string descAr, string descEn, string[] bulletsAr, string[] bulletsEn)
        {
            SPListItem item = list.AddItem();
            item["Title"] = titleAr;
            item["Title_EN"] = titleEn;
            item["IconClass"] = iconClass;
            item["Description"] = descAr;
            item["Description_EN"] = descEn;
            item["Bullets"] = string.Join(Environment.NewLine, bulletsAr);
            item["Bullets_EN"] = string.Join(Environment.NewLine, bulletsEn);
            item["ItemOrder"] = order;
            item["Visibility"] = true;
            item.Update();
        }

        // ------------------------------------------------------------------
        //  Data loading + binding
        // ------------------------------------------------------------------
        private void BindData()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list == null) return;

                        SPQuery query = new SPQuery
                        {
                            Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                                     <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
                        };

                        SPListItemCollection items = list.GetItems(query);
                        int i = 0;
                        foreach (SPListItem item in items)
                        {
                            bool isFirst = (i == 0);
                            string anchorId = "open-data-pane-" + i;

                            string bulletsRaw = IsArabic ? SafeString(item["Bullets"]) : SafeString(item["Bullets_EN"]);

                            _tabs.Add(new OpenDataTab
                            {
                                TabAnchorId = anchorId,
                                Title = IsArabic ? SafeString(item["Title"]) : SafeString(item["Title_EN"]),
                                IconClass = SafeString(item["IconClass"]),
                                Description = IsArabic ? SafeString(item["Description"]) : SafeString(item["Description_EN"]),
                                Bullets = bulletsRaw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                                NavLinkClass = "nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2"
                                               + (isFirst ? " active" : ""),
                                PaneClass = "tab-pane fade" + (isFirst ? " show active" : ""),
                                AriaSelected = isFirst ? "true" : "false",
                                LinkIntro = IsArabic
                                    ? "يمكنكم الوصول إلى البيانات والمنصات ذات العلاقة عبر "
                                    : "You can access related data and platforms via the ",
                                LinkText = IsArabic ? "بوابة البيانات المفتوحة السعودية" : "Saudi Open Data Portal"
                            });
                            i++;
                        }
                    }
                });

                if (_tabs.Count == 0)
                {
                    phData.Visible = false;
                    phEmpty.Visible = true;
                    return;
                }

                rptTabsNav.DataSource = _tabs;
                rptTabsNav.DataBind();

                rptTabsContent.DataSource = _tabs;
                rptTabsContent.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                phData.Visible = false;
                phEmpty.Visible = true;
            }
        }

        protected void rptTabsContent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            OpenDataTab tab = (OpenDataTab)e.Item.DataItem;
            Repeater rptBullets = (Repeater)e.Item.FindControl("rptBullets");
            if (rptBullets != null)
            {
                rptBullets.DataSource = tab.Bullets;
                rptBullets.DataBind();
            }
        }

        private static string SafeString(object val)
        {
            return val == null ? string.Empty : val.ToString().Trim();
        }
    }

    // ------------------------------------------------------------------
    //  DTO
    // ------------------------------------------------------------------
    internal class OpenDataTab
    {
        public string TabAnchorId { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Description { get; set; }
        public List<string> Bullets { get; set; }
        public string NavLinkClass { get; set; }
        public string PaneClass { get; set; }
        public string AriaSelected { get; set; }
        public string LinkIntro { get; set; }
        public string LinkText { get; set; }
    }
}
