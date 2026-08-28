using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public static class CenterProvisioner
    {
        private static readonly object _lock = new object();

        public static void EnsureAllLists(SPWeb web)
        {
            if (web == null && SPContext.Current != null) web = SPContext.Current.Web;
            if (web == null) return;

            lock (_lock)
            {
                try
                {
                    Guid siteId = web.Site.ID;
                    Guid webId = web.ID;

                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (var site = new SPSite(siteId))
                        using (var elevatedWeb = site.OpenWeb(webId))
                        {
                            elevatedWeb.AllowUnsafeUpdates = true;

                            EnsureDepartmentsList(elevatedWeb, "CenterDepartments");
                            EnsureBeneficiariesList(elevatedWeb, "CenterBeneficiaries");
                            EnsureRecordList(elevatedWeb, "CenterServiceRecord");
                            EnsureProgramsList(elevatedWeb, "CenterPrograms");
                            EnsureDigitalChannelsList(elevatedWeb, "CenterDigitalChannels");
                            EnsureHomeContentLists(elevatedWeb);
                            EnsureContactLists(elevatedWeb);
                            EnsureDocumentsList(elevatedWeb, "CenterDocuments");

                            elevatedWeb.AllowUnsafeUpdates = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureAllLists", ex.Message);
                }
            }
        }

        public static void EnsureDepartmentsList(SPWeb web, string listName = "CenterDepartments")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Departments List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }

                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Description", SPFieldType.Note);
                ListHelper.EnsureField(list, "Description_EN", SPFieldType.Note);
                ListHelper.EnsureField(list, "URL", SPFieldType.URL);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                ListHelper.EnsureField(list, "Visible", SPFieldType.Boolean);
                list.Update();

                // Seed initial items if empty
                if (list.ItemCount == 0)
                {
                    AddItem(list, "إدارة الإرشاد المهني", "Career Guidance Department",
                        "تُعنى الإدارة بتقديم خدمات إرشادية شاملة، فردية وجماعية، على مدار العام الدراسي لجميع الطالبات والخريجات، وتشمل قياس الميول المهنية، وتنمية القيم المهنية، والإعداد للمستقبل المهني، وصناعة السيرة الذاتية.",
                        "Responsible for comprehensive counseling services, career profiling, and professional development throughout the year.",
                        "", 1);

                    AddItem(list, "إدارة برنامج سموق", "Smoc Program Department",
                        "تمكّن الإدارة الطالبات من مهارات وظائف المستقبل عبر منهج علمي يقيس المهارات والقيم، ويقدم التوصيات المناسبة من البرامج التدريبية والأنشطة غير الصفية، بما يساعد الطالبة على بناء خارطة طريق للاستعداد لسوق العمل.",
                        "Enables students with future career competencies through structured methodologies.",
                        "https://smoc.pnu.edu.sa/login, منصة سموق", 2);

                    AddItem(list, "إدارة الخريجات", "Graduates Department",
                        "تعمل الإدارة على بناء جسور التواصل مع خريجات الجامعة وسوق العمل، وتوفير الفرص الوظيفية، وإبراز قصص النجاح، وتقديم الخدمات الموجهة للخريجات.",
                        "Builds strong engagement bridges with university alumni and the market.",
                        "https://graduates.pnu.edu.sa/ar/Pages/default.aspx, بوابة الخريجات", 3);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureDepartmentsList", ex.Message);
            }
        }

        public static void EnsureBeneficiariesList(SPWeb web, string listName = "CenterBeneficiaries")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Beneficiaries List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }

                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Description", SPFieldType.Note);
                ListHelper.EnsureField(list, "Description_EN", SPFieldType.Note);
                ListHelper.EnsureField(list, "IconClass", SPFieldType.Text);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                list.Update();

                if (list.ItemCount == 0)
                {
                    SPListItem itm1 = list.AddItem();
                    itm1["Title"] = "طالبات الجامعة";
                    itm1["Title_EN"] = "University Students";
                    itm1["Description"] = "دعم الاستعداد للمستقبل المهني، وتنمية المهارات والقيم المهنية، وقياس الميول، وبناء السيرة الذاتية خلال المرحلة الجامعية.";
                    itm1["Description_EN"] = "Supporting readiness for future careers, developing skills, and career counseling throughout the university journey.";
                    itm1["IconClass"] = "hgi-student";
                    itm1["ItemOrder"] = 1;
                    itm1.Update();

                    SPListItem itm2 = list.AddItem();
                    itm2["Title"] = "خريجات الجامعة";
                    itm2["Title_EN"] = "University Graduates";
                    itm2["Description"] = "رفع الجاهزية لسوق العمل، والوصول إلى الفرص الوظيفية، والاستفادة من الإرشاد والتدريب وبرامج المحاكاة المهنية.";
                    itm2["Description_EN"] = "Enhancing readiness for the job market, providing career opportunities, mentoring, and professional simulation programs.";
                    itm2["IconClass"] = "hgi-graduation-scroll";
                    itm2["ItemOrder"] = 2;
                    itm2.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureBeneficiariesList", ex.Message);
            }
        }

        public static void EnsureRecordList(SPWeb web, string listName = "CenterServiceRecord")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Service Record List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }

                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Body", SPFieldType.Note);
                ListHelper.EnsureField(list, "Body_EN", SPFieldType.Note);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                list.Update();

                if (list.ItemCount == 0)
                {
                    SPListItem itm1 = list.AddItem();
                    itm1["Title"] = "عن سجل سموق المهاري";
                    itm1["Title_EN"] = "About Smoc Skills Record";
                    itm1["Body"] = "<p class=\"mb-0\">وثيقة رسمية معتمدة من جامعة الأميرة نورة بنت عبد الرحمن تسجل المهارات الشخصية والمهنية التي اكتسبتها الطالبة خلال مرحلتها الجامعية، من خلال رفع شهادات الدورات والورش التدريبية.</p>";
                    itm1["ItemOrder"] = 1;
                    itm1.Update();

                    SPListItem itm2 = list.AddItem();
                    itm2["Title"] = "أهداف السجل";
                    itm2["Title_EN"] = "Record Objectives";
                    itm2["Body"] = "<ul class=\"mb-0\"><li>زيادة نسبة توظيف الخريجات.</li><li>توثيق المهارات المكتسبة خلال الحياة الجامعية.</li><li>تحفيز الطالبة على تنمية مهاراتها وإثراء سيرتها الذاتية.</li><li>تحسين المخرجات من خلال تطوير المهارات المطلوبة في سوق العمل.</li><li>توثيق المشاركة في الدورات والورش التدريبية.</li></ul>";
                    itm2["ItemOrder"] = 2;
                    itm2.Update();

                    SPListItem itm3 = list.AddItem();
                    itm3["Title"] = "الوصول إلى الخدمة";
                    itm3["Title_EN"] = "Accessing the Service";
                    itm3["Body"] = "<p>يمكن للطالبة استعراض تفاصيل سجل سموق المهاري والوصول إلى الخدمة من صفحة الخدمات الإلكترونية بالجامعة.</p><a class=\"btn btn-secondary\" href=\"https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205\" target=\"_blank\" rel=\"external noopener noreferrer\">خدمة سجل سموق المهاري</a>";
                    itm3["ItemOrder"] = 3;
                    itm3.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureRecordList", ex.Message);
            }
        }

        public static void EnsureProgramsList(SPWeb web, string listName = "CenterPrograms")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Programs List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }

                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Body", SPFieldType.Note);
                ListHelper.EnsureField(list, "Body_EN", SPFieldType.Note);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                list.Update();

                if (list.ItemCount == 0)
                {
                    SPListItem itm1 = list.AddItem();
                    itm1["Title"] = "برامج الدعم المهاري";
                    itm1["Title_EN"] = "Skills Support Programs";
                    itm1["Body"] = "<p class=\"mb-0\">برامج وفعاليات تدريبية موجهة إلى طالبات الجامعة وخريجاتها، تُبنى وفق مهارات وقيم سموق وكفاءات القرن الحادي والعشرين.</p>";
                    itm1["ItemOrder"] = 1;
                    itm1.Update();

                    SPListItem itm2 = list.AddItem();
                    itm2["Title"] = "الإرشاد المهني وبرنامج التلمذة";
                    itm2["Title_EN"] = "Career Guidance and Mentorship";
                    itm2["Body"] = "<p class=\"mb-3\">برنامج إرشادي يربط الطالبة أو الخريجة بخبير مهني من داخل الجامعة أو سوق العمل، لتطوير المعرفة والخبرة والاستعداد للمسار المهني.</p><p class=\"mb-0\"><a href=\"mailto:asss-cpsp@pnu.edu.sa\" dir=\"ltr\">asss-cpsp@pnu.edu.sa</a> · <a href=\"tel:+966118223119\" dir=\"ltr\">0118223119</a> · <a href=\"https://wa.me/966509262081\" target=\"_blank\" rel=\"external noopener noreferrer\" dir=\"ltr\">0509262081</a></p>";
                    itm2["ItemOrder"] = 2;
                    itm2.Update();

                    SPListItem itm3 = list.AddItem();
                    itm3["Title"] = "الدعم الوظيفي وبرنامج المحاكاة المهنية";
                    itm3["Title_EN"] = "Career Support and Work Simulation";
                    itm3["Body"] = "<p class=\"mb-0\">يهيئ البرنامج الخريجات لسوق العمل من خلال خبرة تطبيقية تقوم على مرافقة الخبراء والقيادات المهنية والتعرف على بيئات العمل الواقعية.</p>";
                    itm3["ItemOrder"] = 3;
                    itm3.Update();

                    SPListItem itm4 = list.AddItem();
                    itm4["Title"] = "التدريب المنتهي بالتوظيف";
                    itm4["Title_EN"] = "Training Ending in Employment";
                    itm4["Body"] = "<p class=\"mb-0\">تدريب مهني متخصص يُنفذ بالتعاون مع جهات التوظيف، ويتيح فرصة التوظيف بعد إتمام متطلبات البرنامج بنجاح.</p>";
                    itm4["ItemOrder"] = 4;
                    itm4.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureProgramsList", ex.Message);
            }
        }

        public static void EnsureDigitalChannelsList(SPWeb web, string listName = "CenterDigitalChannels")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Digital Channels List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }

                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Description", SPFieldType.Note);
                ListHelper.EnsureField(list, "Description_EN", SPFieldType.Note);
                ListHelper.EnsureField(list, "IconClass", SPFieldType.Text);
                ListHelper.EnsureField(list, "URL", SPFieldType.URL);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                list.Update();

                if (list.ItemCount == 0)
                {
                    SPListItem itm1 = list.AddItem();
                    itm1["Title"] = "منصة سموق";
                    itm1["Title_EN"] = "Smoc Platform";
                    itm1["Description"] = "منصة قياس المهارات والقيم المهنية وتوجيه الطالبة إلى البرامج والأنشطة الملائمة لاحتياجاتها.";
                    itm1["IconClass"] = "hgi-award-01";
                    itm1["URL"] = new SPFieldUrlValue { Url = "https://smoc.pnu.edu.sa/login", Description = "منصة سموق" };
                    itm1["ItemOrder"] = 1;
                    itm1.Update();

                    SPListItem itm2 = list.AddItem();
                    itm2["Title"] = "بوابة الخريجات";
                    itm2["Title_EN"] = "Graduates Portal";
                    itm2["Description"] = "بوابة التواصل مع خريجات الجامعة واستعراض الفرص الوظيفية والخدمات وقصص النجاح.";
                    itm2["IconClass"] = "hgi-graduation-scroll";
                    itm2["URL"] = new SPFieldUrlValue { Url = "https://graduates.pnu.edu.sa/ar/Pages/default.aspx", Description = "بوابة الخريجات" };
                    itm2["ItemOrder"] = 2;
                    itm2.Update();

                    SPListItem itm3 = list.AddItem();
                    itm3["Title"] = "خدمة سجل سموق المهاري";
                    itm3["Title_EN"] = "Smoc Skills Record Service";
                    itm3["Description"] = "الخدمة الإلكترونية لتوثيق المهارات والدورات والورش التي اكتسبتها الطالبة خلال حياتها الجامعية.";
                    itm3["IconClass"] = "hgi-license";
                    itm3["URL"] = new SPFieldUrlValue { Url = "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205", Description = "خدمة سجل سموق المهاري" };
                    itm3["ItemOrder"] = 3;
                    itm3.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureDigitalChannelsList", ex.Message);
            }
        }

        public static void EnsureHomeContentLists(SPWeb web)
        {
            if (web == null) return;
            try
            {
                // 1. CenterHomeContent
                SPList contentList = web.Lists.TryGetList("CenterHomeContent");
                if (contentList == null)
                {
                    Guid listId = web.Lists.Add("CenterHomeContent", "Center Home Content Key-Value Settings", SPListTemplateType.GenericList);
                    contentList = web.Lists[listId];
                }
                ListHelper.EnsureField(contentList, "Title", SPFieldType.Text);
                ListHelper.EnsureField(contentList, "Body", SPFieldType.Note);
                ListHelper.EnsureField(contentList, "Body_EN", SPFieldType.Note);
                contentList.Update();

                // 2. CenterObjectives
                SPList objList = web.Lists.TryGetList("CenterObjectives");
                if (objList == null)
                {
                    Guid listId = web.Lists.Add("CenterObjectives", "Center Objectives List", SPListTemplateType.GenericList);
                    objList = web.Lists[listId];
                }
                ListHelper.EnsureField(objList, "Title", SPFieldType.Text);
                ListHelper.EnsureField(objList, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(objList, "ItemOrder", SPFieldType.Number);
                objList.Update();

                // 3. CenterTasks
                SPList taskList = web.Lists.TryGetList("CenterTasks");
                if (taskList == null)
                {
                    Guid listId = web.Lists.Add("CenterTasks", "Center Tasks List", SPListTemplateType.GenericList);
                    taskList = web.Lists[listId];
                }
                ListHelper.EnsureField(taskList, "Title", SPFieldType.Text);
                ListHelper.EnsureField(taskList, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(taskList, "Description", SPFieldType.Note);
                ListHelper.EnsureField(taskList, "Description_EN", SPFieldType.Note);
                ListHelper.EnsureField(taskList, "ItemOrder", SPFieldType.Number);
                taskList.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureHomeContentLists", ex.Message);
            }
        }

        public static void EnsureContactLists(SPWeb web)
        {
            if (web == null) return;
            try
            {
                SPList dirList = web.Lists.TryGetList("CenterContactDirectory");
                if (dirList == null)
                {
                    Guid listId = web.Lists.Add("CenterContactDirectory", "Center Contact Directory List", SPListTemplateType.GenericList);
                    dirList = web.Lists[listId];
                }
                ListHelper.EnsureField(dirList, "Title", SPFieldType.Text);
                ListHelper.EnsureField(dirList, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(dirList, "Phone", SPFieldType.Text);
                ListHelper.EnsureField(dirList, "Email", SPFieldType.Text);
                ListHelper.EnsureField(dirList, "ItemOrder", SPFieldType.Number);
                dirList.Update();

                SPList infoList = web.Lists.TryGetList("CenterContactInfo");
                if (infoList == null)
                {
                    Guid listId = web.Lists.Add("CenterContactInfo", "Center Contact Info List", SPListTemplateType.GenericList);
                    infoList = web.Lists[listId];
                }
                ListHelper.EnsureField(infoList, "Title", SPFieldType.Text);
                ListHelper.EnsureField(infoList, "Value", SPFieldType.Note);
                ListHelper.EnsureField(infoList, "Value_EN", SPFieldType.Note);
                infoList.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureContactLists", ex.Message);
            }
        }

        public static void EnsureDocumentsList(SPWeb web, string listName = "CenterDocuments")
        {
            if (web == null) return;
            try
            {
                SPList list = web.Lists.TryGetList(listName);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(listName, "Center Documents List", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                }
                ListHelper.EnsureField(list, "Title", SPFieldType.Text);
                ListHelper.EnsureField(list, "Title_EN", SPFieldType.Text);
                ListHelper.EnsureField(list, "Category", SPFieldType.Text);
                ListHelper.EnsureField(list, "URL", SPFieldType.URL);
                ListHelper.EnsureField(list, "ItemOrder", SPFieldType.Number);
                list.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "CenterProvisioner.EnsureDocumentsList", ex.Message);
            }
        }

        private static void AddItem(SPList list, string titleAr, string titleEn, string descAr, string descEn, string url, int order)
        {
            SPListItem itm = list.AddItem();
            itm["Title"] = titleAr;
            itm["Title_EN"] = titleEn;
            itm["Description"] = descAr;
            itm["Description_EN"] = descEn;
            if (!string.IsNullOrEmpty(url))
            {
                string[] parts = url.Split(new[] { ',' }, 2);
                string u = parts[0].Trim();
                string d = parts.Length > 1 ? parts[1].Trim() : titleAr;
                itm["URL"] = new SPFieldUrlValue { Url = u, Description = d };
            }
            itm["ItemOrder"] = order;
            itm["Visible"] = true;
            itm.Update();
        }
    }
}
