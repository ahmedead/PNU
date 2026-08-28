using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterRecordDga : UserControl
    {
        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("CenterServiceRecord"),
         Description("Name of the SharePoint list containing Center Record Items.")]
        public string ListName { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class RecordItem
        {
            public string Title { get; set; }
            public string HtmlContent { get; set; }
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterRecordDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litSmocTitle.Text = IsArabic ? "سجل سموق المهاري" : "Smoc Skills Record";

            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterServiceRecord" : ListName.Trim();

            var recordItems = new List<RecordItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureRecordList(web, targetListName);

                SPList list = web.Lists.TryGetList(targetListName);
                if (list != null && list.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in list.GetItems(query))
                    {
                        string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        string html = Convert.ToString(IsArabic ? (item["Body"] ?? item["Description"]) : (item["Body_EN"] ?? item["Description_EN"] ?? item["Body"] ?? item["Description"]));
                        if (!string.IsNullOrEmpty(title))
                            recordItems.Add(new RecordItem { Title = title, HtmlContent = html });
                    }
                }
            }
            catch { }

            if (recordItems.Count == 0)
            {
                if (IsArabic)
                {
                    recordItems.Add(new RecordItem
                    {
                        Title = "عن سجل سموق المهاري",
                        HtmlContent = "<p class=\"mb-0\">وثيقة رسمية معتمدة من جامعة الأميرة نورة بنت عبد الرحمن تسجل المهارات الشخصية والمهنية التي اكتسبتها الطالبة خلال مرحلتها الجامعية، من خلال رفع شهادات الدورات والورش التدريبية.</p>"
                    });
                    recordItems.Add(new RecordItem
                    {
                        Title = "أهداف السجل",
                        HtmlContent = "<ul class=\"mb-0\"><li>زيادة نسبة توظيف الخريجات.</li><li>توثيق المهارات المكتسبة خلال الحياة الجامعية.</li><li>تحفيز الطالبة على تنمية مهاراتها وإثراء سيرتها الذاتية.</li><li>تحسين المخرجات من خلال تطوير المهارات المطلوبة في سوق العمل.</li><li>توثيق المشاركة في الدورات والورش التدريبية.</li></ul>"
                    });
                    recordItems.Add(new RecordItem
                    {
                        Title = "الوصول إلى الخدمة",
                        HtmlContent = "<p>يمكن للطالبة استعراض تفاصيل سجل سموق المهاري والوصول إلى الخدمة من صفحة الخدمات الإلكترونية بالجامعة.</p><a class=\"btn btn-secondary\" href=\"https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205\" target=\"_blank\" rel=\"external noopener noreferrer\">خدمة سجل سموق المهاري</a>"
                    });
                }
                else
                {
                    recordItems.Add(new RecordItem
                    {
                        Title = "About Smoc Skills Record",
                        HtmlContent = "<p class=\"mb-0\">An official certified document from Princess Nourah University recording personal and professional skills acquired by students during their academic journey.</p>"
                    });
                    recordItems.Add(new RecordItem
                    {
                        Title = "Record Objectives",
                        HtmlContent = "<ul class=\"mb-0\"><li>Increasing graduate employability.</li><li>Documenting acquired skills.</li><li>Encouraging continuous self-development.</li></ul>"
                    });
                    recordItems.Add(new RecordItem
                    {
                        Title = "Accessing the Service",
                        HtmlContent = "<p>Students can access the Smoc Record service through the PNU electronic portal.</p><a class=\"btn btn-secondary\" href=\"https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205\" target=\"_blank\" rel=\"external noopener noreferrer\">Smoc Record Service</a>"
                    });
                }
            }

            rptSmocRecord.DataSource = recordItems;
            rptSmocRecord.DataBind();
        }
    }
}
