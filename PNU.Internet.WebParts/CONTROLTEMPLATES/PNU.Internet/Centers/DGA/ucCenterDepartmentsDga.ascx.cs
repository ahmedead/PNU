using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterDepartmentsDga : UserControl
    {
        public string ListName { get; set; } = "CenterDepartments";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class DepartmentItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string UrlTitle { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterDepartmentsDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litSectionTitle.Text = IsArabic ? "إدارات المركز" : "Center Departments";

            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterDepartments" : ListName.Trim();

            var items = new List<DepartmentItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureDepartmentsList(web, targetListName);

                SPList list = web.Lists.TryGetList(targetListName);
                if (list != null && list.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in list.GetItems(query))
                    {
                        string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        string desc = Convert.ToString(IsArabic ? (item["Description"] ?? item["Body"]) : (item["Description_EN"] ?? item["Body_EN"] ?? item["Description"] ?? item["Body"]));
                        string url = "";
                        string urlTitle = "";
                        if (item["URL"] != null)
                        {
                            var uv = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                            url = uv.Url;
                            urlTitle = !string.IsNullOrEmpty(uv.Description) ? uv.Description : title;
                        }
                        if (!string.IsNullOrEmpty(title))
                        {
                            items.Add(new DepartmentItem { Title = title, Description = desc, Url = url, UrlTitle = urlTitle });
                        }
                    }
                }
            }
            catch { }

            if (items.Count == 0)
            {
                if (IsArabic)
                {
                    items.Add(new DepartmentItem
                    {
                        Title = "إدارة الإرشاد المهني",
                        Description = "تُعنى الإدارة بتقديم خدمات إرشادية شاملة، فردية وجماعية، على مدار العام الدراسي لجميع الطالبات والخريجات، وتشمل قياس الميول المهنية، وتنمية القيم المهنية، والإعداد للمستقبل المهني، وصناعة السيرة الذاتية.",
                        Url = "",
                        UrlTitle = ""
                    });
                    items.Add(new DepartmentItem
                    {
                        Title = "إدارة برنامج سموق",
                        Description = "تمكّن الإدارة الطالبات من مهارات وظائف المستقبل عبر منهج علمي يقيس المهارات والقيم، ويقدم التوصيات المناسبة من البرامج التدريبية والأنشطة غير الصفية، بما يساعد الطالبة على بناء خارطة طريق للاستعداد لسوق العمل.",
                        Url = "https://smoc.pnu.edu.sa/login",
                        UrlTitle = "منصة سموق"
                    });
                    items.Add(new DepartmentItem
                    {
                        Title = "إدارة الخريجات",
                        Description = "تعمل الإدارة على بناء جسور التواصل مع خريجات الجامعة وسوق العمل، وتوفير الفرص الوظيفية، وإبراز قصص النجاح، وتقديم الخدمات الموجهة للخريجات.",
                        Url = "https://graduates.pnu.edu.sa/ar/Pages/default.aspx",
                        UrlTitle = "بوابة الخريجات"
                    });
                }
                else
                {
                    items.Add(new DepartmentItem
                    {
                        Title = "Career Guidance Department",
                        Description = "Responsible for comprehensive counseling services, career profiling, and professional development throughout the year.",
                        Url = "",
                        UrlTitle = ""
                    });
                    items.Add(new DepartmentItem
                    {
                        Title = "Smoc Program Department",
                        Description = "Enables students with future career competencies through structured methodologies.",
                        Url = "https://smoc.pnu.edu.sa/login",
                        UrlTitle = "Smoc Platform"
                    });
                    items.Add(new DepartmentItem
                    {
                        Title = "Graduates Department",
                        Description = "Builds strong engagement bridges with university alumni and the market.",
                        Url = "https://graduates.pnu.edu.sa/ar/Pages/default.aspx",
                        UrlTitle = "Graduates Portal"
                    });
                }
            }

            rptDepartments.DataSource = items;
            rptDepartments.DataBind();
        }
    }
}
