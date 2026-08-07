using Microsoft.SharePoint;
using Portal.Main.Helper.Utils;
using System;
using System.Web;

namespace PNU.Internet.WebParts.Helpers
{
    /// <summary>
    /// Provisions the 3 Council lists under the /ar/AboutUniversity/ subsite if they do not already exist.
    /// Call <see cref="EnsureCouncilLists"/> once from Page_Load; it is guarded so it runs at most once
    /// per AppDomain and is safe against concurrent first-hit requests.
    ///
    /// Lists provisioned under /ar/AboutUniversity/:
    ///   - CouncilLeadership  (Chair + Vice Chair cards)
    ///   - CouncilMembers     (member grid cards)
    ///   - CouncilDocuments   (downloadable document cards)
    ///
    /// Note: a single set of lists serves BOTH the Arabic and English pages.
    /// Each list stores *Ar and *En fields; the code-behind chooses which to render
    /// based on the current web's language.
    /// </summary>
    public static class CouncilListProvisioner
    {
        public const string LIST_LEADERSHIP = "CouncilLeadership";
        public const string LIST_MEMBERS = "CouncilMembers";
        public const string LIST_DOCUMENTS = "CouncilDocuments";

        /// <summary>
        /// Server-relative URL of the subsite that hosts the Council lists.
        /// Code-behind MUST use this same value when reading.
        /// </summary>
        public const string SUBSITE_URL = "/ar/AboutUniversity";

        private static volatile bool _alreadyChecked = false;
        private static readonly object _syncRoot = new object();

        public static void EnsureCouncilLists()
        {
            if (_alreadyChecked) return;

            lock (_syncRoot)
            {
                if (_alreadyChecked) return;

                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            // Open the target subsite by server-relative URL.
                            using (SPWeb targetWeb = site.OpenWeb(SUBSITE_URL))
                            {
                                if (targetWeb == null || !targetWeb.Exists) return;

                                targetWeb.AllowUnsafeUpdates = true;
                                try
                                {
                                    EnsureLeadershipList(targetWeb);
                                    EnsureMembersList(targetWeb);
                                    EnsureDocumentsList(targetWeb);
                                }
                                finally
                                {
                                    targetWeb.AllowUnsafeUpdates = false;
                                }
                            }
                        }
                    });

                    _alreadyChecked = true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        string urlStr = "CouncilListProvisioner";
                        try
                        {
                            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                                urlStr = HttpContext.Current.Request.Url.ToString();
                        }
                        catch { /* ignore */ }

                        Publics.WriteToLog(
                            urlStr,
                            "CouncilListProvisioner.EnsureCouncilLists",
                            ex.Message + " | " + ex.StackTrace);
                    }
                    catch { /* swallow */ }
                }
            }
        }

        // ------------------------------------------------------------------
        //  CouncilLeadership (Chair + Vice Chair)
        // ------------------------------------------------------------------

        private static void EnsureLeadershipList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_LEADERSHIP) != null) return;

            Guid id = web.Lists.Add(LIST_LEADERSHIP, "University Council leadership (Chair + Vice Chair)", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='Text' DisplayName='RoleAr' Name='RoleAr' />");
            AddField(list, "<Field Type='Text' DisplayName='RoleEn' Name='RoleEn' />");
            AddField(list, "<Field Type='Text' DisplayName='IconClass' Name='IconClass' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed Chair + Vice Chair
            AddLeadershipRow(list,
                "معالي الأستاذ/ يوسف بن عبد الله البنيان",
                "H.E. Eng. Yousef bin Abdullah Al-Benyan",
                "رئيس مجلس الجامعة",
                "Chairman of the University Council",
                "hgi hgi-stroke hgi-manager",
                1);

            AddLeadershipRow(list,
                "د. فوزية بنت سليمان العمرو",
                "Dr. Fawziah bint Suleiman Al-Amro",
                "نائبة رئيس مجلس الجامعة",
                "Vice Chair of the University Council",
                "hgi hgi-stroke hgi-user-star-01",
                2);
        }

        private static void AddLeadershipRow(SPList list, string nameAr, string nameEn, string roleAr, string roleEn, string iconClass, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = nameAr;
            row["TitleEn"] = nameEn;
            row["RoleAr"] = roleAr;
            row["RoleEn"] = roleEn;
            row["IconClass"] = iconClass;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  CouncilMembers (grid cards)
        // ------------------------------------------------------------------

        private static void EnsureMembersList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_MEMBERS) != null) return;

            Guid id = web.Lists.Add(LIST_MEMBERS, "University Council members", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='Text' DisplayName='RoleAr' Name='RoleAr' />");
            AddField(list, "<Field Type='Text' DisplayName='RoleEn' Name='RoleEn' />");
            // MemberNumber: the badge number shown on each card (1..32).
            AddField(list, "<Field Type='Text' DisplayName='MemberNumber' Name='MemberNumber' />");
            // MemberType: Arabic "عضواً" / "أمين مجلس" (displayed as a small label under the role).
            AddField(list, "<Field Type='Text' DisplayName='MemberTypeAr' Name='MemberTypeAr' />");
            AddField(list, "<Field Type='Text' DisplayName='MemberTypeEn' Name='MemberTypeEn' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed: the 32 University Council members in display order.
            // English fields are populated with the Arabic text as a placeholder —
            // edit each row through Site Contents to add the proper English name/role later.
            AddMemberRow(list, "1", "أ.د. ناصر بن محمد العقيلي", "الأمين العام لمجلس شؤون الجامعات", "عضواً", "Member", 1);
            AddMemberRow(list, "2", "د. نجلاء بنت عبدالله التويجري", "وكيلة الجامعة", "عضواً", "Member", 2);
            AddMemberRow(list, "3", "د. فوزية بنت سليمان العمرو", "وكيلة الجامعة للدراسات العليا والبحث العلمي", "عضواً", "Member", 3);
            AddMemberRow(list, "4", "د. فرح بنت منصور العسكر", "وكيلة الجامعة للشؤون الأكاديمية", "عضواً", "Member", 4);
            AddMemberRow(list, "5", "د. مشاعل بنت عويض المطيري", "وكيلة الجامعة للأصول والاستثمار", "عضواً", "Member", 5);
            AddMemberRow(list, "6", "د. دينا بنت عبدالعزيز الحمادي", "عميدة معهد التنمية والخدمات الاستشارية", "عضواً", "Member", 6);
            AddMemberRow(list, "7", "د. غادة بنت عبدالرحمن العماني", "عميدة عمادة القبول والتسجيل", "عضواً", "Member", 7);
            AddMemberRow(list, "8", "د. دنيا بنت عبدالعزيز الفرّاج", "عميدة عمادة شؤون الطالبات", "عضواً", "Member", 8);
            AddMemberRow(list, "9", "د. معالي بنت إبراهيم العبد الحافظ", "عميدة عمادة البحث العلمي والمكتبات", "عضواً", "Member", 9);
            AddMemberRow(list, "10", "د. مها بنت مشعل الرشيد", "عميدة عمادة الدراسات العليا", "عضواً", "Member", 10);
            AddMemberRow(list, "11", "د. آلاء بنت صالح اللحيدان", "عميدة عمادة التطوير والجودة", "عضواً", "Member", 11);
            AddMemberRow(list, "12", "أ.د. هند بنت أحمد الصعيدي", "عميدة كلية التربية والتنمية البشرية", "عضواً", "Member", 12);
            AddMemberRow(list, "13", "د. فاطمة بنت صالح القبيسي", "عميدة كلية العلوم الإنسانية والاجتماعية", "عضواً", "Member", 13);
            AddMemberRow(list, "14", "د. فلوه بنت عبدالله ثقفان", "عميدة كلية العلوم", "عضواً", "Member", 14);
            AddMemberRow(list, "15", "د. مها بنت أمين خياط", "عميدة كلية التصاميم والفنون", "عضواً", "Member", 15);
            AddMemberRow(list, "16", "د. هوازن بنت زامل المقرن", "عميدة كلية إدارة الأعمال", "عضواً", "Member", 16);
            AddMemberRow(list, "17", "د. عبير بنت عبدالعزيز العيسى", "عميدة كلية القانون", "عضواً", "Member", 17);
            AddMemberRow(list, "18", "أ.د. منال بنت عبدالله العوهلي", "عميدة كلية علوم الحاسب والمعلومات", "عضواً", "Member", 18);
            AddMemberRow(list, "19", "د. حمدة بنت عبدالله الغامدي", "عميدة كلية اللغات", "عضواً", "Member", 19);
            AddMemberRow(list, "20", "د. ريم بنت عوض الحربي", "عميدة كلية الطب البشري", "عضواً", "Member", 20);
            AddMemberRow(list, "21", "أ.د. رهف بنت عبدالله المحارب", "عميدة كلية طب الأسنان", "عضواً", "Member", 21);
            AddMemberRow(list, "22", "د. هديل بنت فؤاد الصالح", "عميدة كلية الصحة وعلوم التأهيل", "عضواً", "Member", 22);
            AddMemberRow(list, "23", "د. حنان بنت فهد الحربي", "عميدة كلية التمريض", "عضواً", "Member", 23);
            AddMemberRow(list, "24", "د. ريم بنت علي عمر سويدان", "عميدة كلية الصيدلة", "عضواً", "Member", 24);
            AddMemberRow(list, "25", "د. رنا بنت سليمان العقيلي", "الرئيس التنفيذي للكلية التطبيقية", "عضواً", "Member", 25);
            AddMemberRow(list, "26", "أ.د. حنان بنت عبدالله بن منقاش", "عميدة كلية الهندسة", "عضواً", "Member", 26);
            AddMemberRow(list, "27", "د. فاطمة بنت عبدالإله المؤيد", "عميدة كلية علوم الرياضة والنشاط البدني", "عضواً", "Member", 27);
            AddMemberRow(list, "28", "د. بدرية بنت براك العنزي", "عميدة معهد تعليم اللغة العربية للناطقات بغيرها", "عضواً", "Member", 28);
            AddMemberRow(list, "29", "د. دانيا بنت عبدالله بن شعلان", "عميدة معهد اللغة الإنجليزية", "عضواً", "Member", 29);
            AddMemberRow(list, "30", "م. عبدالله بن عبدالرحمن العبيكان", "الرئيس التنفيذي لمجموعة العبيكان للاستثمار", "عضواً", "Member", 30);
            AddMemberRow(list, "31", "أ.د. ناصر بن عبدالله الصانع", "استشاري جراحة القولون والمستقيم", "عضواً", "Member", 31);
            AddMemberRow(list, "32", "د. فاطمة بنت علي الشهري", "أمينة مجلس الجامعة", "أمين مجلس", "Council Secretary", 32);
        }

        private static void AddMemberRow(SPList list, string memberNumber, string nameAr, string roleAr, string memberTypeAr, string memberTypeEn, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = nameAr;
            // English fields default to Arabic (placeholder) — edit manually after provisioning.
            row["TitleEn"] = nameAr;
            row["RoleAr"] = roleAr;
            row["RoleEn"] = roleAr;
            row["MemberNumber"] = memberNumber;
            row["MemberTypeAr"] = memberTypeAr;
            row["MemberTypeEn"] = memberTypeEn;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  CouncilDocuments (downloadable PDFs)
        // ------------------------------------------------------------------

        private static void EnsureDocumentsList(SPWeb web)
        {
            if (web.Lists.TryGetList(LIST_DOCUMENTS) != null) return;

            Guid id = web.Lists.Add(LIST_DOCUMENTS, "University Council reference documents", SPListTemplateType.GenericList);
            SPList list = web.Lists[id];

            AddField(list, "<Field Type='Text' DisplayName='TitleEn' Name='TitleEn' />");
            AddField(list, "<Field Type='URL' DisplayName='FileURL' Name='FileURL' Format='Hyperlink' />");
            AddField(list, "<Field Type='Text' DisplayName='IconClass' Name='IconClass' />");
            AddField(list, "<Field Type='Text' DisplayName='ButtonLabelAr' Name='ButtonLabelAr' />");
            AddField(list, "<Field Type='Text' DisplayName='ButtonLabelEn' Name='ButtonLabelEn' />");
            AddField(list, "<Field Type='Boolean' DisplayName='Visibility' Name='Visibility'><Default>1</Default></Field>");
            AddField(list, "<Field Type='Number' DisplayName='ItemOrder' Name='ItemOrder' Decimals='0' />");

            HideFromUI(list);
            list.Update();

            // Seed the 3 documents shown in the original design.
            AddDocumentRow(list,
                "نظام مجلس التعليم العالي والجامعات ولوائحه",
                "Higher Education and Universities Council System and Regulations",
                "https://pnu.edu.sa/ar/AboutUniversity/Documents/%d9%86%d8%b8%d8%a7%d9%85%20%d9%85%d8%ac%d9%84%d8%b3%20%d8%a7%d9%84%d8%aa%d8%b9%d9%84%d9%8a%d9%85%20%d8%a7%d9%84%d8%b9%d8%a7%d9%84%d9%8a%20%d9%88%d8%a7%d9%84%d8%ac%d8%a7%d9%85%d8%b9%d8%a7%d8%aa%20%d9%88%d9%84%d9%88%d8%a7%d8%a6%d8%ad%d9%87.pdf",
                "hgi hgi-stroke hgi-file-02",
                "تحميل الملف",
                "Download File",
                1);

            AddDocumentRow(list,
                "جدول الجلسات للعام الجامعي 1447هـ",
                "Council Sessions Schedule for Academic Year 1447 AH",
                "https://pnu.edu.sa/ar/AboutUniversity/Documents/mmjales1447.pdf",
                "hgi hgi-stroke hgi-calendar-03",
                "تحميل الملف",
                "Download File",
                2);

            AddDocumentRow(list,
                "أبرز قرارات مجلس جامعة الأميرة نورة لعام 1447هـ",
                "Key Decisions of the PNU Council for 1447 AH",
                "https://pnu.edu.sa/ar/AboutUniversity/Documents/%d8%a3%d8%a8%d8%b1%d8%b2%20%d9%82%d8%b1%d8%a7%d8%b1%d8%a7%d8%aa%20%d9%85%d8%ac%d9%84%d8%b3%20%d8%ac%d8%a7%d9%85%d8%b9%d8%a9%20%d8%a7%d9%84%d8%a3%d9%85%d9%8a%d8%b1%d8%a9%20%d9%86%d9%88%d8%b1%d8%a9%20%d8%a8%d9%86%d8%aa%20%d8%b9%d8%a8%d8%af%d8%a7%d9%84%d8%b1%d8%ad%d9%85%d9%86%20%d9%84%d8%b9%d8%a7%d9%85%201447%d9%87%d9%80_2.pdf",
                "hgi hgi-stroke hgi-validation-approval",
                "تحميل الملف",
                "Download File",
                3);
        }

        private static void AddDocumentRow(SPList list, string titleAr, string titleEn, string url, string iconClass, string buttonAr, string buttonEn, int order)
        {
            SPListItem row = list.Items.Add();
            row["Title"] = titleAr;
            row["TitleEn"] = titleEn;
            if (!string.IsNullOrEmpty(url))
            {
                row["FileURL"] = new SPFieldUrlValue { Url = url, Description = titleAr };
            }
            row["IconClass"] = iconClass;
            row["ButtonLabelAr"] = buttonAr;
            row["ButtonLabelEn"] = buttonEn;
            row["Visibility"] = true;
            row["ItemOrder"] = order;
            row.Update();
        }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        private static void AddField(SPList list, string fieldXml)
        {
            list.Fields.AddFieldAsXml(fieldXml, true, SPAddFieldOptions.AddFieldInternalNameHint);
        }

        private static void HideFromUI(SPList list)
        {
            try
            {
                list.Hidden = false;
                list.OnQuickLaunch = false;
                list.NoCrawl = true;
            }
            catch { /* cosmetic */ }
        }
    }
}
