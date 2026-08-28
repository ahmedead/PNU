using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterBeneficiariesDga : UserControl
    {
        public string ListName { get; set; } = "CenterBeneficiaries";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class BeneficiaryItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string IconClass { get; set; }
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterBeneficiariesDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litSectionTitle.Text = IsArabic ? "الفئات المستفيدة" : "Beneficiaries";

            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterBeneficiaries" : ListName.Trim();

            var items = new List<BeneficiaryItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureBeneficiariesList(web, targetListName);

                SPList list = web.Lists.TryGetList(targetListName);
                if (list != null && list.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in list.GetItems(query))
                    {
                        string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        string desc = Convert.ToString(IsArabic ? item["Description"] : (item["Description_EN"] ?? item["Description"]));
                        string icon = Convert.ToString(item["IconClass"] ?? "hgi-user");
                        if (!string.IsNullOrEmpty(title))
                        {
                            items.Add(new BeneficiaryItem { Title = title, Description = desc, IconClass = icon });
                        }
                    }
                }
            }
            catch { }

            if (items.Count == 0)
            {
                if (IsArabic)
                {
                    items.Add(new BeneficiaryItem
                    {
                        Title = "طالبات الجامعة",
                        Description = "دعم الاستعداد للمستقبل المهني، وتنمية المهارات والقيم المهنية، وقياس الميول، وبناء السيرة الذاتية خلال المرحلة الجامعية.",
                        IconClass = "hgi-student"
                    });
                    items.Add(new BeneficiaryItem
                    {
                        Title = "خريجات الجامعة",
                        Description = "رفع الجاهزية لسوق العمل، والوصول إلى الفرص الوظيفية، والاستفادة من الإرشاد والتدريب وبرامج المحاكاة المهنية.",
                        IconClass = "hgi-graduation-scroll"
                    });
                }
                else
                {
                    items.Add(new BeneficiaryItem
                    {
                        Title = "University Students",
                        Description = "Supporting readiness for future careers, developing skills, and career counseling throughout the university journey.",
                        IconClass = "hgi-student"
                    });
                    items.Add(new BeneficiaryItem
                    {
                        Title = "University Graduates",
                        Description = "Enhancing readiness for the job market, providing career opportunities, mentoring, and professional simulation programs.",
                        IconClass = "hgi-graduation-scroll"
                    });
                }
            }

            rptBeneficiaries.DataSource = items;
            rptBeneficiaries.DataBind();
        }
    }
}
