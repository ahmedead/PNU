using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterProgramsDga : UserControl
    {
        public string ListName { get; set; } = "CenterPrograms";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class ProgramItem
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterProgramsDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litSectionTitle.Text = IsArabic ? "البرامج" : "Programs";

            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterPrograms" : ListName.Trim();

            var items = new List<ProgramItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureProgramsList(web, targetListName);

                Guid siteId = web.Site.ID;
                Guid webId = web.ID;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var elevatedWeb = site.OpenWeb(webId))
                    {
                        SPList list = elevatedWeb.Lists.TryGetList(targetListName);
                        if (list != null && list.ItemCount > 0)
                        {
                            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                            foreach (SPListItem item in list.GetItems(query))
                            {
                                string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                                string html = Convert.ToString(IsArabic ? (item["Body"] ?? item["Description"]) : (item["Body_EN"] ?? item["Description_EN"] ?? item["Body"] ?? item["Description"]));
                                if (!string.IsNullOrEmpty(title))
                                {
                                    items.Add(new ProgramItem { Title = title, HtmlContent = html });
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterProgramsDga.BindData", ex.Message);
            }

            if (items.Count == 0)
            {
                if (IsArabic)
                {
                    items.Add(new ProgramItem
                    {
                        Title = "برامج الدعم المهاري",
                        HtmlContent = "<p class=\"mb-0\">برامج وفعاليات تدريبية موجهة إلى طالبات الجامعة وخريجاتها، تُبنى وفق مهارات وقيم سموق وكفاءات القرن الحادي والعشرين.</p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "الإرشاد المهني وبرنامج التلمذة",
                        HtmlContent = "<p class=\"mb-3\">برنامج إرشادي يربط الطالبة أو الخريجة بخبير مهني من داخل الجامعة أو سوق العمل، لتطوير المعرفة والخبرة والاستعداد للمسار المهني.</p><p class=\"mb-0\"><a href=\"mailto:asss-cpsp@pnu.edu.sa\" dir=\"ltr\">asss-cpsp@pnu.edu.sa</a> · <a href=\"tel:+966118223119\" dir=\"ltr\">0118223119</a> · <a href=\"https://wa.me/966509262081\" target=\"_blank\" rel=\"external noopener noreferrer\" dir=\"ltr\">0509262081</a></p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "الدعم الوظيفي وبرنامج المحاكاة المهنية",
                        HtmlContent = "<p class=\"mb-0\">يهيئ البرنامج الخريجات لسوق العمل من خلال خبرة تطبيقية تقوم على مرافقة الخبراء والقيادات المهنية والتعرف على بيئات العمل الواقعية.</p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "التدريب المنتهي بالتوظيف",
                        HtmlContent = "<p class=\"mb-0\">تدريب مهني متخصص يُنفذ بالتعاون مع جهات التوظيف، ويتيح فرصة التوظيف بعد إتمام متطلبات البرنامج بنجاح.</p>"
                    });
                }
                else
                {
                    items.Add(new ProgramItem
                    {
                        Title = "Skills Support Programs",
                        HtmlContent = "<p class=\"mb-0\">Training programs and events designed for female university students and graduates aligned with 21st century competencies.</p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "Career Guidance and Mentorship",
                        HtmlContent = "<p class=\"mb-0\">Guidance program connecting students and graduates with professional industry mentors.</p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "Career Support and Work Simulation",
                        HtmlContent = "<p class=\"mb-0\">Hands-on experience allowing graduates to shadow leaders and gain workplace readiness.</p>"
                    });
                    items.Add(new ProgramItem
                    {
                        Title = "Training Ending in Employment",
                        HtmlContent = "<p class=\"mb-0\">Specialized vocational training delivered in collaboration with hiring partners.</p>"
                    });
                }
            }

            rptPrograms.DataSource = items;
            rptPrograms.DataBind();
        }
    }
}
