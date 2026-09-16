using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definition and seed data for PgProcedures: the interactive academic and graduation
    /// procedures with their requirements, regulation article and approved forms.
    /// </summary>
    public static partial class PgListSchema
    {
        private const string Docs = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/";

        internal const string FormCourseEquivalence = Docs + "%d8%a7%d8%b3%d8%aa%d9%85%d8%a7%d8%b1%d8%a9%20%d9%85%d8%b9%d8%a7%d8%af%d9%84%d8%a9%20%d9%85%d9%82%d8%b1%d8%b1%d8%a7%d8%aa%20%d8%af%d8%b1%d8%a7%d8%b3%d9%8a%d8%a9.docx";
        internal const string FormResearchPlan = Docs + "%d8%aa%d8%b3%d8%ac%d9%8a%d9%84%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%84%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx";
        internal const string FormPlanContents = Docs + "%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d9%81%d8%a7%d8%af%d8%a9%20%d8%b9%d9%86%20%d9%85%d8%ad%d8%aa%d9%88%d9%89%20%d9%88%d9%85%d8%b1%d8%a7%d8%ac%d8%b9%20%d9%88%d9%85%d8%b5%d8%a7%d8%af%d8%b1%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab/gradpro.docx";
        internal const string FormCancelRegistration = Docs + "%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d9%84%d8%ba%d8%a7%d8%a1%20%d9%82%d9%8a%d8%af.docx";
        internal const string FormWarning = Docs + "%D9%86%D9%85%D9%88%D8%B0%D8%AC%20%D8%A7%D9%86%D8%B0%D8%A7%D8%B1.docx";
        internal const string FormReRegistration = Docs + "%D9%86%D9%85%D9%88%D8%B0%D8%AC%20%D8%A5%D8%B9%D8%A7%D8%AF%D8%A9%20%D9%82%D9%8A%D8%AF.docx";
        internal const string FormCommitteeNomination = Docs + "%D8%AA%D8%B1%D8%B4%D9%8A%D8%AD%20%D9%84%D8%AC%D9%86%D8%A9%20%D9%85%D9%86%D8%A7%D9%82%D8%B4%D8%A9%20%D8%A7%D9%84%D8%B1%D8%B3%D8%A7%D9%84%D8%A9%20%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A%D8%A9.docx";
        internal const string FormCommitteeApproval = Docs + "1437-1438%D9%87/%D8%A7%D8%B9%D8%AA%D9%85%D8%A7%D8%AF%20%D9%84%D8%AC%D9%86%D8%A9%20%D9%85%D9%86%D8%A7%D9%82%D8%B4%D8%A9%20%D8%A7%D9%84%D8%B1%D8%B3%D8%A7%D9%84%D8%A9%20%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A%D8%A91.docx";
        internal const string FormCommitteeReport = Docs + "%D8%AA%D9%82%D8%B1%D9%8A%D8%B1%20%D9%84%D8%AC%D9%86%D8%A9%20%D9%85%D9%86%D8%A7%D9%82%D8%B4%D8%A9%20%D8%A7%D9%84%D8%B1%D8%B3%D8%A7%D9%84%D8%A9%20%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A%D8%A9.docx";
        internal const string FormAmendmentAck = Docs + "%D8%A5%D9%82%D8%B1%D8%A7%D8%B1%20%D8%A8%D8%A5%D8%AC%D8%B1%D8%A7%D8%A1%20%D8%A7%D9%84%D8%AA%D8%B9%D8%AF%D9%8A%D9%84%D8%A7%D8%AA%20%D8%A7%D9%84%D9%85%D8%B7%D9%84%D9%88%D8%A8%D8%A9%20%D8%B9%D9%84%D9%89%20%D8%A7%D9%84%D8%B1%D8%B3%D8%A7%D9%84%D8%A9%20%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A%D8%A9.docx";
        internal const string FormClearance = Docs + "%D9%86%D9%85%D9%88%D8%B0%D8%AC%20%D8%A5%D8%AE%D9%84%D8%A7%D8%A1%20%D8%B7%D8%B1%D9%81.docx";
        internal const string FormExceptionalDegree = Docs + "%D9%86%D9%85%D9%88%D8%B0%D8%AC%20%D8%B7%D9%84%D8%A8%20%D9%81%D8%B1%D8%B5%D8%A9%20%D8%A7%D8%B3%D8%AA%D8%AB%D9%86%D8%A7%D8%A6%D9%8A%D8%A9%20%D9%84%D9%84%D8%AD%D8%B5%D9%88%D9%84%20%D8%B9%D9%84%D9%89%20%D8%A7%D9%84%D8%AF%D8%B1%D8%AC%D8%A9%20%D8%A7%D9%84%D8%B9%D9%84%D9%85%D9%8A%D8%A9%20%281%29.docx";
        internal const string FormGradeAmendment = Docs + "%D8%AA%D8%B9%D8%AF%D9%8A%D9%84%20%D9%86%D8%AA%D9%8A%D8%AC%D8%A9%20%D8%B7%D8%A7%D9%84%D8%A8%D8%A9.docx";

        private const string GroupCollegeAr = "٣.١ إجراءات أكاديمية تبدأ من الكلية:";
        private const string GroupCollegeEn = "3.1 Academic procedures initiated by the college:";
        private const string GroupStudentAr = "٣.٢ إجراءات أكاديمية تبدأ من الطالبة:";
        private const string GroupStudentEn = "3.2 Academic procedures initiated by the student:";

        private const string FormsHeadAr = "النماذج المرتبطة";
        private const string FormsHeadEn = "Related forms";
        private const string ApplyHeadAr = "للتقديم على الإجراء";
        private const string ApplyHeadEn = "To apply for this procedure";
        private const string ServiceLinkAr = "رابط الخدمة|#";
        private const string ServiceLinkEn = "Service link|#";

        private static void BuildProcedureList(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Procedures] = new PgListDef
            {
                Name = PgListNames.Procedures,
                TitleAr = "الإجراءات الأكاديمية", TitleEn = "Academic procedures",
                Description = "Expandable procedure cards with requirements, regulation article and related forms.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(),
                    new PgFieldDef("Badge", "رقم المادة (عربي)", "Article badge (AR)", SPFieldType.Text, true),
                    new PgFieldDef("Badge_EN", "رقم المادة (إنجليزي)", "Article badge (EN)", SPFieldType.Text),
                    DescAr(4), DescEn(4),
                    new PgFieldDef("GroupLabel", "عنوان المجموعة (عربي)", "Group label (AR)", SPFieldType.Text, true),
                    new PgFieldDef("GroupLabel_EN", "عنوان المجموعة (إنجليزي)", "Group label (EN)", SPFieldType.Text),
                    new PgFieldDef("Conditions", "الشروط والمتطلبات (عربي)", "Requirements (AR)", SPFieldType.Note, false, 6, "شرط في كل سطر."),
                    new PgFieldDef("Conditions_EN", "الشروط والمتطلبات (إنجليزي)", "Requirements (EN)", SPFieldType.Note, false, 6),
                    new PgFieldDef("ArticleTitle", "عنوان المادة (عربي)", "Article title (AR)", SPFieldType.Text),
                    new PgFieldDef("ArticleTitle_EN", "عنوان المادة (إنجليزي)", "Article title (EN)", SPFieldType.Text),
                    new PgFieldDef("ArticleText", "نص المادة (عربي)", "Article text (AR)", SPFieldType.Note, false, 8, "فقرة في كل سطر."),
                    new PgFieldDef("ArticleText_EN", "نص المادة (إنجليزي)", "Article text (EN)", SPFieldType.Note, false, 8),
                    new PgFieldDef("FormsHeading", "عنوان النماذج (عربي)", "Forms heading (AR)", SPFieldType.Text),
                    new PgFieldDef("FormsHeading_EN", "عنوان النماذج (إنجليزي)", "Forms heading (EN)", SPFieldType.Text),
                    new PgFieldDef("Forms", "النماذج (عربي)", "Forms (AR)", SPFieldType.Note, false, 6, "اسم النموذج|الرابط — سطر لكل نموذج."),
                    new PgFieldDef("Forms_EN", "النماذج (إنجليزي)", "Forms (EN)", SPFieldType.Note, false, 6),
                    Phase(), Order()
                },
                Seed = ProceduresSeed()
            };
        }

        private static Dictionary<string, string> Procedure(
            string phase, int order, string groupAr, string groupEn,
            string titleAr, string titleEn, string badgeAr, string badgeEn,
            string descAr, string descEn,
            string condAr, string condEn,
            string articleTitleAr, string articleTitleEn,
            string articleAr, string articleEn,
            string formsHeadAr, string formsHeadEn,
            string formsAr, string formsEn)
        {
            return Row(
                "Title", titleAr, "Title_EN", titleEn,
                "Badge", badgeAr, "Badge_EN", badgeEn,
                "Description", descAr, "Description_EN", descEn,
                "GroupLabel", groupAr, "GroupLabel_EN", groupEn,
                "Conditions", condAr, "Conditions_EN", condEn,
                "ArticleTitle", articleTitleAr, "ArticleTitle_EN", articleTitleEn,
                "ArticleText", articleAr, "ArticleText_EN", articleEn,
                "FormsHeading", formsHeadAr, "FormsHeading_EN", formsHeadEn,
                "Forms", formsAr, "Forms_EN", formsEn,
                "Phase", phase,
                "ItemOrder", order.ToString(CultureInfo.InvariantCulture));
        }

        private static List<Dictionary<string, string>> ProceduresSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // ===================== المقيدون / Enrolled =====================

            seed.Add(Procedure(PgPhases.Enrolled, 1, GroupCollegeAr, GroupCollegeEn,
                "معادلة المقررات الدراسية", "Course equivalence",
                "المادة التاسعة عشر", "Article 19",
                "معادلة مقرراتٍ دراسيةٍ سبق للطالب/ـة اجتيازُها في جامعةٍ أو مؤسسةٍ تعليميةٍ مرخّصةٍ من جهة الاختصاص، وإثباتها في السجل الأكاديمي.",
                "Recognising courses the student has already passed at a university or educational institution licensed by the competent authority, and recording them in her academic transcript.",
                "ألا تقلّ المقررات المُعادَلة عمّا تسمح به الخطة الدراسية للبرنامج.\nأن تكون الجهة المانحة للمقرر مرخّصةً من جهة الاختصاص في بلد الدراسة.\nأن يصدر بالمعادلة قرارٌ من مجلس الكلية بناءً على توصية القسم، وتُثبَت في السجل الأكاديمي.",
                "The recognised courses must not fall below what the program study plan allows.\nThe institution that awarded the course must be licensed by the competent authority in the country of study.\nThe equivalence must be issued by a college council decision on the recommendation of the department, and recorded in the academic transcript.",
                "المادة التاسعة عشر", "Article 19",
                "يجوز معادلة مقررات دراسية سبق للطالب اجتيازها في جامعة أو مؤسسة تعليمية داخل المملكة، أو من خارج المملكة على أن تكون مرخصة من جهة الاختصاص في بلد الدراسة، بناءً على توصية مجلس القسم، وموافقة من مجلس الكلية، وتُثبت المقررات الدراسية التي تم معادلتها في السجل الأكاديمي للطالب، ويحدد مجلس الجامعة الضوابط المنظمة لذلك، بما فيها إمكانية احتسابها ضمن المعدل التراكمي للطالب في سجله الأكاديمي.",
                "Courses the student has already passed at a university or educational institution inside the Kingdom, or outside it provided the institution is licensed by the competent authority in the country of study, may be recognised on the recommendation of the department council and with the approval of the college council. The recognised courses are recorded in the student's academic transcript, and the university council sets the governing rules, including whether they may count towards the student's grade point average.",
                FormsHeadAr, FormsHeadEn,
                "نموذج استمارة معادلة المقررات الدراسية|" + FormCourseEquivalence,
                "Course equivalence application form|" + FormCourseEquivalence));

            seed.Add(Procedure(PgPhases.Enrolled, 2, GroupCollegeAr, GroupCollegeEn,
                "تسجيل خطة البحث للرسالة العلمية", "Registering the thesis research plan",
                "المادة السادسة والثلاثون", "Article 36",
                "تسجيل خطة بحث الرسالة العلمية بعد إنجاز نصف المقررات الدراسية، بتقدّم المرشدة الأكاديمية بالطلب إلى القسم المختص.",
                "Registering the research plan of the thesis after completing half of the coursework, with the academic advisor submitting the request to the relevant department.",
                "ألا يقلّ المنجَز من المقررات الدراسية عن 50% قبل التقدّم بتسجيل الخطة.\nألا يقلّ المعدل التراكمي عن تقدير «جيد جداً».\nسلامة الخطة فكرياً وخلوّها من المؤلفات المحظورة، وتقدّمها المرشدة الأكاديمية للقسم المختص.",
                "At least 50% of the coursework must be completed before the plan is submitted for registration.\nThe grade point average must not be below \"very good\".\nThe plan must be intellectually sound and free of prohibited works, and it is submitted by the academic advisor to the relevant department.",
                "المادة السادسة والثلاثون", "Article 36",
                "يضع مجلس الجامعة القواعد المنظمة لتسجيل مشروع بحث التخرج، والرسالة العلمية لطالب الدراسات العليا — إن وجدت — وآلية تحديد المشرف على الرسالة العلمية، والمشرف المساعد — إن وجد — وذلك بناءً على توصية اللجنة الدائمة.\nالمادة الثامنة والثلاثون (الإشراف): يشرف على الرسائل العلمية الأساتذة والأساتذة المشاركون من أعضاء هيئة التدريس بالجامعة، ويجوز للأستاذ المساعد أن يشرف على أبحاث التخرج ورسائل الماجستير، والمساعدة في الإشراف على رسائل الدكتوراه إذا كان لديه بحثان منشوران أو مقبولان للنشر في مجلة علمية محكّمة.",
                "The university council, on the recommendation of the standing committee, sets the rules governing the registration of the graduation research project and of the postgraduate student's thesis, where one exists, and the mechanism for appointing the thesis supervisor and the co-supervisor, where one exists.\nArticle 38 (supervision): theses are supervised by professors and associate professors among the university's faculty members. An assistant professor may supervise graduation research and master's theses, and may co-supervise doctoral theses if he or she has two papers published or accepted for publication in a peer-reviewed academic journal.",
                FormsHeadAr, FormsHeadEn,
                "نموذج تسجيل خطة البحث للرسالة العلمية|" + FormResearchPlan
                + "\nنموذج إفادة عن محتوى ومراجع ومصادر خطة البحث / الرسالة / بحث التخرج|" + FormPlanContents
                + "\nنموذج تقرير عن سير دراسة طالبة دراسات عليا|" + FormPlanContents,
                "Thesis research plan registration form|" + FormResearchPlan
                + "\nStatement on the content, references and sources of the research plan, thesis or graduation research|" + FormPlanContents
                + "\nProgress report on a postgraduate student's studies|" + FormPlanContents));

            seed.Add(Procedure(PgPhases.Enrolled, 3, GroupCollegeAr, GroupCollegeEn,
                "إلغاء القيد", "Cancellation of registration",
                "المادة السادسة والعشرون", "Article 26",
                "إنهاء قيد الطالب/ـة عند انطباق إحدى الحالات النظامية المنصوص عليها في اللائحة.",
                "Ending the student's registration when one of the statutory cases set out in the regulation applies.",
                "يقع إلغاء القيد عند انطباق إحدى الحالات النظامية (الانقطاع، أو انخفاض المعدل عن «جيد جداً» في فصلين متتاليين، أو تجاوز المدة، أو عدم اجتياز التكميلية، أو الإخلال بالأمانة العلمية).\nأن يصدر بقرارٍ من اللجنة الدائمة بناءً على توصية القسم وتأييد مجلس الكلية في الحالات التي تستوجب ذلك.",
                "Registration is cancelled when one of the statutory cases applies (interruption of study, a grade point average below \"very good\" for two consecutive terms, exceeding the permitted period, failing the supplementary courses, or a breach of academic integrity).\nIt is issued by a decision of the standing committee on the recommendation of the department and with the endorsement of the college council in the cases that require it.",
                "المادة السادسة والعشرون", "Article 26",
                "يُلغى قيد الطالب في الحالات الآتية: إذا انقطع عن الدراسة وفقاً للمادة الخامسة والعشرين؛ إذا لم يجتز المقررات التكميلية وفق الضوابط المقرّة؛ إذا انخفض معدله التراكمي عن تقدير (جيد جداً) في فصلين دراسيين متتاليين؛ إذا اتضح أن قبوله مبني على معلومات أو وثائق غير صحيحة كلياً أو جزئياً؛ إذا لم يحصل على الدرجة العلمية خلال المدة المحددة للبرنامج.\nويُلغى القيد بقرار من اللجنة الدائمة بناءً على توصية مجلس القسم وتأييد من مجلس الكلية في حالات أخرى منها: ثبوت عدم الجدية في مرحلة الرسالة العلمية، وعدم اجتياز الاختبار الشامل بعد إعادته، وعدم صلاحية الرسالة للمناقشة، والإخلال بالأمانة العلمية.",
                "A student's registration is cancelled in the following cases: if she interrupts her studies under Article 25; if she does not pass the supplementary courses under the approved rules; if her grade point average falls below \"very good\" for two consecutive terms; if it emerges that her admission was based wholly or partly on incorrect information or documents; if she does not obtain the degree within the period set for the program.\nRegistration is also cancelled by a decision of the standing committee on the recommendation of the department council and with the endorsement of the college council in other cases, including: proven lack of seriousness during the thesis stage, failing the comprehensive examination after resitting it, the thesis being unfit for defence, and a breach of academic integrity.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إلغاء قيد|" + FormCancelRegistration,
                "Registration cancellation form|" + FormCancelRegistration));

            seed.Add(Procedure(PgPhases.Enrolled, 4, GroupCollegeAr, GroupCollegeEn,
                "الإنذار الأكاديمي", "Academic warning",
                "المادة الرابعة والأربعون", "Article 44",
                "توجيه إنذارٍ خطيٍّ من القسم عند ثبوت عدم الجدية في مرحلة الرسالة العلمية بناءً على تقرير المشرف.",
                "A written warning from the department when a lack of seriousness during the thesis stage is established on the basis of the supervisor's report.",
                "يُوجَّه الإنذار عند ثبوت عدم الجدية في مرحلة الرسالة بناءً على تقرير المشرف، أو عند انخفاض المعدل.\nإذا أُنذرت الطالبة مرتين ولم تتلافَ أسباب الإنذار يُلغى قيدها بتوصية مجلسي القسم والكلية.",
                "The warning is issued when a lack of seriousness during the thesis stage is established on the basis of the supervisor's report, or when the grade point average falls.\nIf the student is warned twice and does not remedy the reasons for the warning, her registration is cancelled on the recommendation of the department and college councils.",
                "المادة الرابعة والأربعون", "Article 44",
                "إذا ثبت عدم جدية الطالب في مرحلة الرسالة العلمية، بناءً على تقرير المشرف على الرسالة العلمية، يتم إنذار الطالب بخطاب من القسم المختص، وإذا أُنذر الطالب مرتين ولم يتلافَ أسباب الإنذار يُلغى قيده، بناءً على توصية مجلسي القسم والكلية.",
                "If a lack of seriousness on the student's part during the thesis stage is established on the basis of the thesis supervisor's report, the student is warned by a letter from the relevant department. If the student is warned twice and does not remedy the reasons for the warning, her registration is cancelled on the recommendation of the department and college councils.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إنذار طالبة|" + FormWarning,
                "Student warning form|" + FormWarning));

            seed.Add(Procedure(PgPhases.Enrolled, 5, GroupCollegeAr, GroupCollegeEn,
                "إعادة القيد", "Reinstatement of registration",
                "المادة السابعة والعشرون", "Article 27",
                "إعادة الجامعة علاقتَها بالطالب/ـة التي أُلغي قيدها، عند توافر ظروفٍ قهريةٍ حالت دون مواصلة الدراسة.",
                "The university restores its relationship with a student whose registration was cancelled, where compelling circumstances prevented her from continuing her studies.",
                "أن يكون الحائل دون مواصلة الدراسة ظرفاً قهرياً يقبله مجلسا القسم والكلية.\nمَن مضى على إلغاء قيدها أكثر من ثلاثة أعوام تُعامَل معاملة المستجدّة؛ ومن مضى ثلاثة أعوام أو أقل تُعيد بعض المقررات.\nصدور قرار إعادة القيد من اللجنة الدائمة.",
                "The obstacle to continuing study must be a compelling circumstance accepted by the department and college councils.\nA student whose registration was cancelled more than three years ago is treated as a new student; one for whom three years or less have passed repeats some courses.\nThe reinstatement decision is issued by the standing committee.",
                "المادة السابعة والعشرون", "Article 27",
                "يجوز إعادة قيد الطالب الذي أُلغي قيده إذا كان الحائل دون مواصلة دراسته ظروفاً يقبلها مجلسا القسم والكلية، وتكون إعادة القيد بقرار من اللجنة الدائمة، مع مراعاة: مَن مضى على إلغاء قيده أكثر من ثلاثة أعوام دراسية يُعامل معاملة الطالب المستجد من حيث إعادة دراسة المقررات، ومَن مضى ثلاثة أعوام أو أقل يعيد دراسة بعض المقررات التي يحددها مجلس القسم ويوافق عليها مجلس الكلية.",
                "A student whose registration was cancelled may be reinstated if the obstacle to continuing her studies consisted of circumstances accepted by the department and college councils. Reinstatement is by decision of the standing committee, taking into account that a student for whom more than three academic years have passed since cancellation is treated as a new student as regards repeating courses, while one for whom three years or less have passed repeats some courses determined by the department council and approved by the college council.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إعادة قيد|" + FormReRegistration,
                "Reinstatement form|" + FormReRegistration));

            seed.Add(Procedure(PgPhases.Enrolled, 6, GroupCollegeAr, GroupCollegeEn,
                "تشكيل لجنة مناقشة للرسالة العلمية", "Forming the thesis examination committee",
                "المادة الثامنة والأربعون", "Article 48",
                "تشكيل لجنة مناقشة الرسالة العلمية بقرارٍ من مجلس الكلية بعد تقرير المشرف باكتمال الرسالة.",
                "Forming the thesis examination committee by a decision of the college council after the supervisor reports that the thesis is complete.",
                "أن يتقدّم المشرف بتقرير اكتمال الرسالة إلى رئيس القسم، مرفقاً به نسخةٌ منها.\nأن تُكوَّن اللجنة بقرارٍ من مجلس الكلية بناءً على توصية مجلس القسم.\nأن تُعدّ اللجنة تقريرها موقَّعاً من جميع أعضائها خلال أسبوعٍ من المناقشة.",
                "The supervisor submits a report on the completion of the thesis to the head of department, together with a copy of it.\nThe committee is formed by a decision of the college council on the recommendation of the department council.\nThe committee prepares its report, signed by all its members, within one week of the defence.",
                "المادة السابعة والأربعون", "Article 47",
                "يُقدِّم المشرف على الرسالة العلمية، بعد انتهاء الطالب من إعدادها، تقريراً عن اكتمالها إلى رئيس القسم، مرفقاً به نسخة من الرسالة العلمية، تمهيداً لاستكمال إجراءات المناقشة التي يحددها مجلس الكلية.\nالمادة الثامنة والأربعون: تُكوَّن لجنة لمناقشة الرسالة العلمية بقرار من مجلس الكلية، بناءً على توصية مجلس القسم.\nالمادة الخمسون: تُعِد لجنة المناقشة تقريراً يُوقَّع من جميع أعضائها، يُقدَّم إلى رئيس القسم خلال أسبوع من تاريخ المناقشة، متضمناً إحدى التوصيات: قبول الرسالة والتوصية بمنح الدرجة؛ أو قبولها مع إجراء تعديلات؛ أو استكمال أوجه النقص وإعادة المناقشة؛ أو عدم قبول الرسالة.",
                "After the student has finished preparing the thesis, the supervisor submits a report on its completion to the head of department, together with a copy of the thesis, in preparation for the defence procedures determined by the college council.\nArticle 48: a committee to examine the thesis is formed by a decision of the college council on the recommendation of the department council.\nArticle 50: the examination committee prepares a report signed by all its members and submitted to the head of department within one week of the defence, containing one of the following recommendations: accepting the thesis and recommending the award of the degree; accepting it subject to amendments; making good the deficiencies and repeating the defence; or not accepting the thesis.",
                FormsHeadAr, FormsHeadEn,
                "نموذج ترشيح لجنة مناقشة الرسالة العلمية|" + FormCommitteeNomination
                + "\nنموذج اعتماد لجنة مناقشة الرسالة العلمية|" + FormCommitteeApproval
                + "\nنموذج تقرير لجنة مناقشة الرسالة العلمية|" + FormCommitteeReport
                + "\nنموذج الإقرار بإجراء التعديلات المطلوبة على الرسالة العلمية|" + FormAmendmentAck,
                "Thesis examination committee nomination form|" + FormCommitteeNomination
                + "\nThesis examination committee approval form|" + FormCommitteeApproval
                + "\nThesis examination committee report form|" + FormCommitteeReport
                + "\nAcknowledgement of the required amendments to the thesis|" + FormAmendmentAck));

            seed.Add(Procedure(PgPhases.Enrolled, 7, GroupCollegeAr, GroupCollegeEn,
                "الفرصة الاستثنائية لرفع المعدل", "Exceptional opportunity to raise the grade point average",
                "المادة الثامنة والعشرون", "Article 28",
                "منح الطالب/ـة فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسي لمعالجة انخفاض المعدل التراكمي.",
                "Granting the student a single exceptional opportunity of no more than one academic year to remedy a fall in her grade point average.",
                "أن تكون فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسيٍّ كحدٍّ أعلى.\nتُمنَح استثناءً من حالة انخفاض المعدل التراكمي عن تقدير «جيد جداً» في فصلين متتاليين.\nصدور قرار مجلس الكلية بالاستثناء بناءً على توصية القسم.",
                "It is a single exceptional opportunity of no more than one academic year.\nIt is granted by way of exception from the case of a grade point average below \"very good\" for two consecutive terms.\nThe college council issues the exception decision on the recommendation of the department.",
                "المادة الثامنة والعشرون", "Article 28",
                "لمجلس الكلية، استثناءً من الفقرة (ج) من البند (١) من المادة السادسة والعشرين من هذه اللائحة، منح الطالب فرصة استثنائية واحدة لا تزيد على عام دراسي كحد أعلى، بناءً على توصية مجلس القسم.",
                "The college council may, by way of exception from paragraph (c) of clause (1) of Article 26 of this regulation, grant the student a single exceptional opportunity of no more than one academic year, on the recommendation of the department council.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إلغاء قيد|" + FormCancelRegistration,
                "Registration cancellation form|" + FormCancelRegistration));

            seed.Add(Procedure(PgPhases.Enrolled, 8, GroupCollegeAr, GroupCollegeEn,
                "منح الدرجة العلمية", "Award of the degree",
                "المواد ٥٠ و٥٣ و٥٦", "Articles 50, 53 and 56",
                "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.",
                "The student graduates after successfully completing the program requirements, and the recommendation to award the degree is forwarded to the university council for a decision.",
                "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».\nاستكمال نماذج اللجنة والإقرار بالتعديلات وإخلاء الطرف، وتسليم نسخ الرسالة المعتمدة.\nرفع الوكيل المختص التوصية بمنح الدرجة إلى مجلس الجامعة (المفوَّض لمعالي رئيسة الجامعة).",
                "The graduation requirements are successfully completed in accordance with the program plan, and the grade point average is not below \"very good\".\nThe committee forms, the acknowledgement of amendments and the clearance form are completed, and the approved copies of the thesis are delivered.\nThe relevant vice-president forwards the recommendation to award the degree to the university council, whose authority is delegated to the university president.",
                "المادة الثالثة والخمسون", "Article 53",
                "يرفع رئيس القسم المختص تقرير لجنة المناقشة إلى عميد الكلية في مدة لا تتجاوز أسبوعين من تاريخ المناقشة؛ لرفعه إلى الإدارة التنفيذية.\nالمادة الرابعة والخمسون: يتخرج الطالب بعد إنهاء متطلبات التخرج بنجاح حسب خطة البرنامج، على ألا يقل معدله التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، على ألا يقل في كل الأحوال عن تقدير (جيد جداً).\nالمادة السادسة والخمسون: يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.",
                "The head of the relevant department forwards the examination committee report to the dean of the college within no more than two weeks of the date of the defence, to be passed on to the executive administration.\nArticle 54: the student graduates after successfully completing the graduation requirements in accordance with the program plan, provided her grade point average is not below the average set by the university council for each program and in all cases not below \"very good\".\nArticle 56: the relevant deputy or vice-president forwards the recommendation to award the degree to the university council for a decision.",
                FormsHeadAr, FormsHeadEn,
                "نموذج تقرير لجنة مناقشة الرسالة العلمية|" + FormCommitteeReport
                + "\nنموذج الإقرار بإجراء التعديلات المطلوبة|" + FormAmendmentAck
                + "\nنموذج إخلاء طرف|" + FormClearance,
                "Thesis examination committee report form|" + FormCommitteeReport
                + "\nAcknowledgement of the required amendments|" + FormAmendmentAck
                + "\nClearance form|" + FormClearance));

            seed.Add(Procedure(PgPhases.Enrolled, 9, GroupCollegeAr, GroupCollegeEn,
                "الفرصة الاستثنائية للحصول على الدرجة", "Exceptional opportunity to obtain the degree",
                "المادة التاسعة والعشرون", "Article 29",
                "منح الطالب/ـة فرصةً استثنائيةً لا تزيد على عامٍ دراسي؛ لاستكمال متطلبات الحصول على الدرجة، استثناءً من حالة عدم الحصول عليها خلال المدة المحددة.",
                "Granting the student an exceptional opportunity of no more than one academic year to complete the requirements for the degree, by way of exception from the case of not obtaining it within the set period.",
                "أن تُقدَّم قبل نهاية المدة النظامية للبرنامج (بمدةٍ لا تتجاوز أربعة أسابيع قبل بدء آخر فصل دراسي من المدة المحددة).\nأن تُبنى على تقريرٍ من المشرف وتوصية مجلسي القسم والكلية.\nألا تزيد الفرصة على عامٍ دراسي، وتصدر بقرار اللجنة الدائمة.",
                "It must be submitted before the end of the statutory period of the program, no later than four weeks before the start of the final term of that period.\nIt must be based on a report from the supervisor and the recommendation of the department and college councils.\nThe opportunity must not exceed one academic year, and it is issued by a decision of the standing committee.",
                "المادة التاسعة والعشرون", "Article 29",
                "للجنة الدائمة، استثناءً من الفقرة (هـ) من البند (١) من المادة السادسة والعشرين من هذه اللائحة، منح الطالب فرصة استثنائية لا تزيد على عام دراسي، بناءً على تقرير من المشرف وتوصية مجلسي القسم والكلية.",
                "The standing committee may, by way of exception from paragraph (e) of clause (1) of Article 26 of this regulation, grant the student an exceptional opportunity of no more than one academic year, on the basis of a report from the supervisor and the recommendation of the department and college councils.",
                FormsHeadAr, FormsHeadEn,
                "نموذج طلب فرصة استثنائية للحصول على الدرجة العلمية|" + FormExceptionalDegree,
                "Application for an exceptional opportunity to obtain the degree|" + FormExceptionalDegree));

            seed.Add(Procedure(PgPhases.Enrolled, 10, GroupCollegeAr, GroupCollegeEn,
                "تعديل النتيجة", "Amending a result",
                "قواعد تنفيذية", "Executive rules",
                "تعديل نتيجة مقررٍ دراسيٍّ بمسوّغٍ معتمدٍ ووفق القواعد التنفيذية لرصد الدرجات.",
                "Amending the result of a course on approved grounds and in accordance with the executive rules for recording grades.",
                "أن يكون التعديل بمسوّغٍ معتمدٍ ووفق القواعد التنفيذية لرصد الدرجات.\nاعتماد التعديل عبر مجلسي القسم والكلية قبل تنفيذه.",
                "The amendment must rest on approved grounds and follow the executive rules for recording grades.\nThe amendment must be approved by the department and college councils before it is applied.",
                "الأساس النظامي", "Regulatory basis",
                "يستند تعديل النتيجة إلى القواعد التنفيذية المعتمدة لرصد الدرجات وتعديلها، وفقاً لما تقرّه الجامعة استناداً إلى لائحة الدراسة والاختبارات وأحكام اللائحة المنظمة.",
                "The amendment of a result rests on the approved executive rules for recording and amending grades, as decided by the university on the basis of the study and examinations regulation and the provisions of the governing regulation.",
                FormsHeadAr, FormsHeadEn,
                "نموذج تعديل نتيجة طالبة|" + FormGradeAmendment,
                "Student result amendment form|" + FormGradeAmendment));

            seed.Add(Procedure(PgPhases.Enrolled, 11, GroupCollegeAr, GroupCollegeEn,
                "إخلاء الطرف", "Clearance",
                "إجراءات معتمدة", "Approved procedures",
                "استكمال براءة الذمّة تجاه وحدات الجامعة عبر النموذج المعتمد، بوصفه خطوةً ختاميةً للانسحاب أو التخرج أو إلغاء القيد.",
                "Settling all obligations towards the university's units through the approved form, as the closing step of withdrawal, graduation or cancellation of registration.",
                "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.\nيُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب أو التخرج أو إلغاء القيد ومنح الدرجة.",
                "All obligations towards the relevant university units must be settled.\nIt is a closing step tied to withdrawal, graduation, or cancellation of registration and the award of the degree.",
                "الأساس الإجرائي", "Procedural basis",
                "يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة، ويُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب والتخرج وإلغاء القيد ومنح الدرجة.",
                "Clearance is completed in accordance with the procedures approved by the deanship, and is a closing step tied to withdrawal, graduation, cancellation of registration and the award of the degree.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إخلاء الطرف|" + FormClearance,
                "Clearance form|" + FormClearance));

            seed.Add(Procedure(PgPhases.Enrolled, 12, GroupStudentAr, GroupStudentEn,
                "تأجيل القبول", "Deferring admission",
                "المادة العشرون", "Article 20",
                "إرجاء تاريخ بدء دراسة الطالب/ـة بعد صدور رقمها الجامعي وقبل تسجيل أي مقرر دراسي، ويكون مرةً واحدة، ولا يُقبل بعد بدء الدراسة.",
                "Postponing the start of the student's studies after her university number is issued and before she registers any course. It is granted once only and is not accepted after study has begun.",
                "أن يكون التأجيل قبل تسجيل أي مقرر دراسي، ولمرّةٍ واحدةٍ فقط.\nألا يُقبل الطلب بعد بدء الطالبة دراستها.\nصدور موافقة مجلس القسم المختص وعميد الكلية.",
                "The deferral must be before any course is registered, and once only.\nThe request is not accepted after the student has begun her studies.\nThe relevant department council and the dean of the college must approve it.",
                "المادة العشرون", "Article 20",
                "يجوز بموافقة مجلس القسم المختص وعميد الكلية تأجيل الطالب قبوله، ويكون لمرة واحدة، ولا يمكن قبول التأجيل بعد بدء الطالب دراسته، وفق ضوابط يقرها مجلس الجامعة، بناءً على اقتراح اللجنة الدائمة.",
                "With the approval of the relevant department council and the dean of the college, a student may defer her admission. This is granted once only and cannot be accepted after the student has begun her studies, in accordance with rules approved by the university council on the proposal of the standing committee.",
                ApplyHeadAr, ApplyHeadEn, ServiceLinkAr, ServiceLinkEn));

            seed.Add(Procedure(PgPhases.Enrolled, 13, GroupStudentAr, GroupStudentEn,
                "الاعتذار عن دراسة مقرر أو فصل", "Apologising for a course or a term",
                "المادة الثالثة والعشرون", "Article 23",
                "الاعتذار عن الاستمرار في دراسة مقرر أو أكثر، أو جميع مقررات الفصل الدراسي، بعد انتهاء فترة التسجيل.",
                "Apologising for continuing one or more courses, or all the courses of a term, after the registration period has ended.",
                "أن يكون الاعتذار بعد انتهاء فترة التسجيل وضمن المدة المحددة في تقويم الإجراءات الأكاديمية.\nصدور توصية مجلس القسم المختص وموافقة عميد الكلية.",
                "The apology must be made after the registration period ends and within the period set in the academic procedures calendar.\nThe relevant department council must recommend it and the dean of the college must approve it.",
                "المادة الثالثة والعشرون", "Article 23",
                "يجوز للطالب الاعتذار عن الاستمرار في دراسة مقرر أو أكثر، أو جميع مقررات الفصل الدراسي، وذلك بعد انتهاء فترة التسجيل، بتوصية من مجلس القسم المختص، وموافقة من عميد الكلية، وفق ضوابط يقرها مجلس الجامعة بناءً على اقتراح اللجنة الدائمة.",
                "A student may apologise for continuing one or more courses, or all the courses of the term, after the registration period has ended, on the recommendation of the relevant department council and with the approval of the dean of the college, in accordance with rules approved by the university council on the proposal of the standing committee.",
                ApplyHeadAr, ApplyHeadEn, ServiceLinkAr, ServiceLinkEn));

            seed.Add(Procedure(PgPhases.Enrolled, 14, GroupStudentAr, GroupStudentEn,
                "تأجيل الدراسة", "Deferring study",
                "المادة الثانية والعشرون", "Article 22",
                "إيقاف سريان المدة النظامية بعد بدء الدراسة، بتوصية مجلس القسم وموافقة مجلس الكلية.",
                "Suspending the statutory period after study has begun, on the recommendation of the department council and with the approval of the college council.",
                "أن يصل الطلب مكتملاً للإدارة التنفيذية في موعدٍ أقصاه نهاية الأسبوع الثالث من بدء الفصل.\nألا يتجاوز التأجيل فصلين دراسيين متتاليين، ويُقدَّم لكل فصلٍ طلبٌ مستقل.\nألا يتجاوز مجموع مدة التأجيل نصف المدة المقررة للبرنامج، ولا تُحتسب ضمن مدة الحصول على الدرجة.",
                "The completed request must reach the executive administration by the end of the third week of the term at the latest.\nThe deferral must not exceed two consecutive terms, and a separate request is submitted for each term.\nThe total deferral must not exceed half the period set for the program, and it does not count towards the period for obtaining the degree.",
                "المادة الثانية والعشرون", "Article 22",
                "يجوز للطالب تأجيل الدراسة بتوصية من مجلس القسم المختص وموافقة مجلس الكلية، وفق ضوابط يقرها مجلس الجامعة، بناءً على اقتراح اللجنة الدائمة.\nالقواعد التنفيذية: لا تُحتسب مدة التأجيل ضمن المدة المقررة للحصول على الدرجة، وألا يتجاوز مجموع مدة التأجيل نصف المدة المقررة للبرنامج.",
                "A student may defer her studies on the recommendation of the relevant department council and with the approval of the college council, in accordance with rules approved by the university council on the proposal of the standing committee.\nExecutive rules: the deferral period does not count towards the period set for obtaining the degree, and the total deferral must not exceed half the period set for the program.",
                ApplyHeadAr, ApplyHeadEn, ServiceLinkAr, ServiceLinkEn));

            seed.Add(Procedure(PgPhases.Enrolled, 15, GroupStudentAr, GroupStudentEn,
                "الانسحاب", "Withdrawal",
                "المادة الرابعة والعشرون", "Article 24",
                "إنهاء الطالب/ـة علاقتها بالجامعة المقيدة بها بشكلٍ نهائي، قبل إكمال برنامجها الدراسي.",
                "The student ends her relationship with the university at which she is registered on a final basis, before completing her program of study.",
                "أن يكون الانسحاب قبل إكمال البرنامج الدراسي.\nاستكمال إجراءات إخلاء الطرف المرتبطة بالانسحاب.\nعند الرغبة في العودة لاحقاً تُطبَّق شروط القبول وقت التقديم الجديد.",
                "The withdrawal must be before the program of study is completed.\nThe clearance procedures associated with withdrawal must be completed.\nIf the student wishes to return later, the admission requirements in force at the time of the new application apply.",
                "المادة الرابعة والعشرون", "Article 24",
                "إذا انسحب طالب الدراسات العليا من الجامعة ثم أراد الالتحاق بها مجدداً يجوز للجامعة أن تطبق عليه شروط القبول وقت التقديم الجديد.\nتعريف الانسحاب (المادة الأولى): إنهاء الطالب علاقته بالجامعة المقيد بها بشكل نهائي، قبل إكماله برنامجه الدراسي.",
                "If a postgraduate student withdraws from the university and then wishes to enrol again, the university may apply to her the admission requirements in force at the time of the new application.\nDefinition of withdrawal (Article 1): the student ends her relationship with the university at which she is registered on a final basis, before completing her program of study.",
                ApplyHeadAr, ApplyHeadEn, ServiceLinkAr, ServiceLinkEn));

            // ===================== الخريجون / Graduates =====================

            seed.Add(Procedure(PgPhases.Graduates, 1, "", "",
                "منح الدرجة العلمية", "Award of the degree",
                "المواد ٥٠ و٥٣ و٥٦", "Articles 50, 53 and 56",
                "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.",
                "The student graduates after successfully completing the program requirements, and the recommendation to award the degree is forwarded to the university council for a decision.",
                "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».\nاستكمال نماذج اللجنة والإقرار بالتعديلات وإخلاء الطرف، وتسليم نسخ الرسالة المعتمدة.\nرفع الوكيل المختص التوصية بمنح الدرجة إلى مجلس الجامعة (المفوَّض لمعالي رئيسة الجامعة).",
                "The graduation requirements are successfully completed in accordance with the program plan, and the grade point average is not below \"very good\".\nThe committee forms, the acknowledgement of amendments and the clearance form are completed, and the approved copies of the thesis are delivered.\nThe relevant vice-president forwards the recommendation to award the degree to the university council, whose authority is delegated to the university president.",
                "المادة الثالثة والخمسون", "Article 53",
                "يرفع رئيس القسم المختص تقرير لجنة المناقشة إلى عميد الكلية في مدة لا تتجاوز أسبوعين من تاريخ المناقشة؛ لرفعه إلى الإدارة التنفيذية.\nالمادة الرابعة والخمسون: يتخرج الطالب بعد إنهاء متطلبات التخرج بنجاح حسب خطة البرنامج، على ألا يقل معدله التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، على ألا يقل في كل الأحوال عن تقدير (جيد جداً).\nالمادة السادسة والخمسون: يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.",
                "The head of the relevant department forwards the examination committee report to the dean of the college within no more than two weeks of the date of the defence, to be passed on to the executive administration.\nArticle 54: the student graduates after successfully completing the graduation requirements in accordance with the program plan, provided her grade point average is not below the average set by the university council for each program and in all cases not below \"very good\".\nArticle 56: the relevant deputy or vice-president forwards the recommendation to award the degree to the university council for a decision.",
                FormsHeadAr, FormsHeadEn,
                "نموذج تقرير لجنة مناقشة الرسالة العلمية|" + FormCommitteeReport
                + "\nنموذج الإقرار بإجراء التعديلات المطلوبة|" + FormAmendmentAck
                + "\nنموذج إخلاء طرف|" + FormClearance,
                "Thesis examination committee report form|" + FormCommitteeReport
                + "\nAcknowledgement of the required amendments|" + FormAmendmentAck
                + "\nClearance form|" + FormClearance));

            seed.Add(Procedure(PgPhases.Graduates, 2, "", "",
                "إخلاء الطرف", "Clearance",
                "إجراءات معتمدة", "Approved procedures",
                "استكمال براءة الذمّة تجاه وحدات الجامعة عبر النموذج المعتمد، تمهيداً لاستلام وثيقة التخرّج.",
                "Settling all obligations towards the university's units through the approved form, in preparation for receiving the graduation document.",
                "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.\nيُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب أو التخرج أو إلغاء القيد ومنح الدرجة.",
                "All obligations towards the relevant university units must be settled.\nIt is a closing step tied to withdrawal, graduation, or cancellation of registration and the award of the degree.",
                "الأساس الإجرائي", "Procedural basis",
                "يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة، ويُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب والتخرج وإلغاء القيد ومنح الدرجة.",
                "Clearance is completed in accordance with the procedures approved by the deanship, and is a closing step tied to withdrawal, graduation, cancellation of registration and the award of the degree.",
                FormsHeadAr, FormsHeadEn,
                "نموذج إخلاء طرف|" + FormClearance,
                "Clearance form|" + FormClearance));

            return seed;
        }
    }
}
