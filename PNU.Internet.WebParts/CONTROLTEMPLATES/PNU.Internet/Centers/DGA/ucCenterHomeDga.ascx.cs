using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterHomeDga : UserControl
    {
        public const string LIST_CONTENT = "CenterHomeContent";
        public const string LIST_TASKS = "CenterTasks";
        public const string LIST_OBJECTIVES = "CenterObjectives";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class TaskItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterHomeDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureHomeContentLists(web);

                Dictionary<string, string> content = LoadContent();
                List<string> objectives = LoadObjectives(content);
                List<TaskItem> tasks = LoadTasks();

                // 1. Overview
                string overviewTitle = Get(content, "OverviewTitle", IsArabic ? "نظرة عامة عن المركز" : "Center Overview");
                string overviewText = Get(content, "Overview", IsArabic ?
                    "<p class=\"mb-3 text-justify\">يسعى المركز إلى إكساب طالبات الجامعة وخريجاتها المهارات اللازمة لوظائف المستقبل، وتهيئتهن للمنافسة في سوق العمل من خلال خدمات الدعم المهاري والإرشاد المهني والدعم الوظيفي.</p>" +
                    "<p class=\"mb-0 text-justify\">يدعم المركز توجه جامعة الأميرة نورة بنت عبد الرحمن نحو تحقيق التميز المهاري والسلوكي والمعرفي لطالباتها، وتعزيز مشاركتهن الفاعلة في التنمية الوطنية.</p>"
                    : "<p class=\"mb-3 text-justify\">The Center strives to equip female university students and graduates with the skills required for future careers and prepare them to compete effectively in the job market.</p>");

                litOverviewTitle.Text = overviewTitle;
                litOverviewText.Text = overviewText;

                string imgUrl = Get(content, "OverviewImage", "");
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    imgOverview.Src = imgUrl;
                    imgOverview.Alt = overviewTitle;
                    pnlOverviewImage.Visible = true;
                    divOverviewText.Attributes["class"] = "col-12 col-md-6 col-lg-8";
                }
                else
                {
                    pnlOverviewImage.Visible = false;
                    divOverviewText.Attributes["class"] = "col-12";
                }

                // 2. Vision & Mission
                litVisionTitle.Text = Get(content, "VisionTitle", IsArabic ? "الرؤية" : "Vision");
                litVisionText.Text = Get(content, "Vision", IsArabic ? "الريادة في إعداد طالبات وخريجات جامعة الأميرة نورة لوظائف المستقبل، وتعزيز مشاركتهن النوعية في سوق العمل." : "Leadership in preparing Princess Nourah University students and graduates for future careers.");

                litMissionTitle.Text = Get(content, "MissionTitle", IsArabic ? "الرسالة" : "Mission");
                litMissionText.Text = Get(content, "Mission", IsArabic ? "تقديم دعم طلابي ومهني متكامل ينمّي المهارات والقيم المهنية، ويربط المعرفة الأكاديمية بمتطلبات سوق العمل المتغيرة." : "Providing integrated student and professional support to develop skills and values.");

                // 3. Objectives
                litObjectivesTitle.Text = Get(content, "ObjectivesTitle", IsArabic ? "الأهداف" : "Objectives");
                rptObjectives.DataSource = objectives;
                rptObjectives.DataBind();

                // 4. Director Word
                litDirectorTitle.Text = Get(content, "DirectorTitle", IsArabic ? "كلمة مديرة المركز" : "Center Director's Word");
                litDirectorWord.Text = Get(content, "DirectorWord", IsArabic ?
                    "نؤمن في المركز بأن تمكين الطالبة مهاريًا ومهنيًا يبدأ مبكرًا خلال رحلتها الجامعية. ومن هذا المنطلق، نعمل على تقديم خدمات وبرامج متكاملة تساعد طالبات الجامعة وخريجاتها على اكتشاف قدراتهن، وتنمية مهاراتهن، والاستعداد بثقة لوظائف المستقبل."
                    : "We believe at the Center that student empowerment begins early during the academic journey.");
                litDirectorName.Text = Get(content, "DirectorName", IsArabic ? "مديرة المركز" : "Center Director");

                // 5. Tasks
                litTasksHeader.Text = Get(content, "TasksTitle", IsArabic ? "مهام المركز" : "Center Tasks");
                rptTasks.DataSource = tasks;
                rptTasks.DataBind();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterHomeDga.BindData", ex.Message);
            }
        }

        private Dictionary<string, string> LoadContent()
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(LIST_CONTENT);
                if (list != null)
                {
                    foreach (SPListItem item in list.Items)
                    {
                        string key = Convert.ToString(item["Title"] ?? item["ContentKey"] ?? "").Trim();
                        string val = Convert.ToString(IsArabic ? (item["Body"] ?? item["Value"]) : (item["Body_EN"] ?? item["Value_EN"] ?? item["Body"] ?? item["Value"]));
                        if (!string.IsNullOrEmpty(key))
                            dict[key] = val;
                    }
                }
            }
            catch { }
            return dict;
        }

        private List<string> LoadObjectives(Dictionary<string, string> content)
        {
            var list = new List<string>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList spList = web.Lists.TryGetList(LIST_OBJECTIVES);
                if (spList != null && spList.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in spList.GetItems(query))
                    {
                        string text = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        if (!string.IsNullOrEmpty(text))
                            list.Add(text);
                    }
                }
            }
            catch { }

            if (list.Count == 0)
            {
                if (IsArabic)
                {
                    list.Add("إكساب الطالبات المهارات اللازمة لوظائف المستقبل.");
                    list.Add("تحقيق التميز المهاري والسلوكي والمعرفي للطالبات والخريجات.");
                    list.Add("رفع جاهزية الخريجات للمنافسة والمشاركة الفاعلة في سوق العمل.");
                    list.Add("دعم تمكين المرأة ومشاركتها في المسيرة الاقتصادية والتنموية.");
                    list.Add("إثراء البرامج الأكاديمية بالمعرفة والمهارات المهنية والتطبيقية.");
                }
                else
                {
                    list.Add("Equipping students with essential future skills.");
                    list.Add("Achieving behavioral, cognitive, and professional excellence.");
                    list.Add("Enhancing graduate readiness for the competitive labor market.");
                    list.Add("Supporting women's empowerment and economic participation.");
                }
            }
            return list;
        }

        private List<TaskItem> LoadTasks()
        {
            var list = new List<TaskItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList spList = web.Lists.TryGetList(LIST_TASKS);
                if (spList != null && spList.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in spList.GetItems(query))
                    {
                        string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        string desc = Convert.ToString(IsArabic ? item["Description"] : (item["Description_EN"] ?? item["Description"]));
                        if (!string.IsNullOrEmpty(title))
                            list.Add(new TaskItem { Title = title, Description = desc });
                    }
                }
            }
            catch { }

            if (list.Count == 0)
            {
                if (IsArabic)
                {
                    list.Add(new TaskItem { Title = "التهيئة لوظائف المستقبل", Description = "يسعى المركز إلى إكساب الطالبة المهارات اللازمة لوظائف المستقبل المتغيرة بمؤثرات تقنية واقتصادية وثقافية واجتماعية، بما يمكّنها من المنافسة في سوق العمل." });
                    list.Add(new TaskItem { Title = "التميز المهاري والسلوكي والمعرفي", Description = "يترجم المركز توجه الجامعة نحو تحقيق التميز لطالباتها مهاريًا وسلوكيًا ومعرفيًا، بما يعزز دورهن الفاعل في بناء الوطن." });
                    list.Add(new TaskItem { Title = "دعم تمكين المرأة", Description = "يعزز المركز أهدافه وخدماته استجابةً للتوجهات الوطنية الداعمة لفرص تمكين المرأة ومشاركتها في المسيرة الاقتصادية والتنموية." });
                    list.Add(new TaskItem { Title = "الإثراء المعرفي والمهاري", Description = "يدعم المركز رؤية الجامعة من خلال الإثراء المعرفي والمهاري للبرامج الأكاديمية، وتعظيم استثمار التراكم المعرفي لدى المتعلمات وتطويره." });
                }
                else
                {
                    list.Add(new TaskItem { Title = "Preparation for Future Careers", Description = "Preparing students with essential technical and soft skills." });
                    list.Add(new TaskItem { Title = "Skill & Behavioral Excellence", Description = "Translating university goals into tangible excellence." });
                    list.Add(new TaskItem { Title = "Supporting Women Empowerment", Description = "Aligning with national goals for women's development." });
                    list.Add(new TaskItem { Title = "Cognitive & Practical Enrichment", Description = "Enriching academic programs with practical know-how." });
                }
            }
            return list;
        }

        private static string Get(Dictionary<string, string> dict, string key, string fallback = "")
        {
            if (dict != null && dict.TryGetValue(key, out string val) && !string.IsNullOrWhiteSpace(val))
                return val;
            return fallback;
        }
    }
}
