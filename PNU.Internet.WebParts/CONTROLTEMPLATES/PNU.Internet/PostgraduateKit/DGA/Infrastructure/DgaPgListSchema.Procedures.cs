using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public static partial class DgaPgListSchema
    {
        private static void RegisterProcedureLists()
        {
            var procedures = new DgaPgListDef(DgaPgListNames.Procedures, "الإجراءات الأكاديمية التفاعلية (DGA)", "الإجراءات والشروط والمواد والنماذج ومسار الإجراء");
            procedures.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            procedures.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.Stations);
            procedures.AddField("ShortDescription", "Short Description (AR)", SPFieldType.Note, false);
            procedures.AddField("ShortDescription_EN", "Short Description (EN)", SPFieldType.Note, false);
            procedures.AddField("Requirements", "Requirements / Conditions (AR)", SPFieldType.Note, false);
            procedures.AddField("Requirements_EN", "Requirements / Conditions (EN)", SPFieldType.Note, false);
            procedures.AddField("ArticleText", "Article Text (AR)", SPFieldType.Note, false);
            procedures.AddField("ArticleText_EN", "Article Text (EN)", SPFieldType.Note, false);
            procedures.AddField("Forms", "Forms (AR)", SPFieldType.Note, false);
            procedures.AddField("Forms_EN", "Forms (EN)", SPFieldType.Note, false);
            procedures.AddField("FlowText", "Workflow / Path (AR)", SPFieldType.Note, false);
            procedures.AddField("FlowText_EN", "Workflow / Path (EN)", SPFieldType.Note, false);
            procedures.AddField("DirectServiceUrl", "Direct Service URL", SPFieldType.Note, false);
            procedures.AddField("IconUrl", "Accordion Icon URL", SPFieldType.Note, false);
            procedures.AddField("BadgeText", "Badge Text (AR)", SPFieldType.Text, false);
            procedures.AddField("BadgeText_EN", "Badge Text (EN)", SPFieldType.Text, false);
            procedures.AddField("SubGroupKey", "Sub Group", SPFieldType.Choice, false, new[] { "College", "Student" });
            procedures.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            procedures.SeedDataFactory = GetProceduresSeed;
            Register(procedures);
        }

        private static List<Dictionary<string, object>> GetProceduresSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Enrolled students: the 15 interactive academic procedures, transcribed from the v4 design.
            // SubGroupKey splits them into the design 3.1 (College-initiated) and 3.2 (Student-initiated) groups.
            list.Add(new Dictionary<string, object> {
                { "Title", "معادلة المقررات الدراسية" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة التاسعة عشر" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-equivalency.png" },
                { "ShortDescription", "معادلة مقرراتٍ دراسيةٍ سبق للطالب/ـة اجتيازُها في جامعةٍ أو مؤسسةٍ تعليميةٍ مرخّصةٍ من جهة الاختصاص، وإثباتها في السجل الأكاديمي." },
                { "ShortDescription_EN", "" },
                { "Requirements", "ألا تقلّ المقررات المُعادَلة عمّا تسمح به الخطة الدراسية للبرنامج.\nأن تكون الجهة المانحة للمقرر مرخّصةً من جهة الاختصاص في بلد الدراسة.\nأن يصدر بالمعادلة قرارٌ من مجلس الكلية بناءً على توصية القسم، وتُثبَت في السجل الأكاديمي." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة التاسعة عشرة|يجوز معادلة مقررات دراسية سبق للطالب اجتيازها في جامعة أو مؤسسة تعليمية داخل المملكة، أو من خارج المملكة على أن تكون مرخصة من جهة الاختصاص في بلد الدراسة، بناءً على توصية مجلس القسم، وموافقة من مجلس الكلية، وتُثبت المقررات الدراسية التي تم معادلتها في السجل الأكاديمي للطالب، ويحدد مجلس الجامعة الضوابط المنظمة لذلك، بما فيها إمكانية احتسابها ضمن المعدل التراكمي للطالب في سجله الأكاديمي." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج استمارة معادلة المقررات الدراسية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a7%d8%b3%d8%aa%d9%85%d8%a7%d8%b1%d8%a9%20%d9%85%d8%b9%d8%a7%d8%af%d9%84%d8%a9%20%d9%85%d9%82%d8%b1%d8%b1%d8%a7%d8%aa%20%d8%af%d8%b1%d8%a7%d8%b3%d9%8a%d8%a9.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم ← موافقة مجلس الكلية ← إثبات المقررات في السجل الأكاديمي." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تسجيل خطة البحث للرسالة العلمية" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة السادسة والثلاثون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-research-plan.png" },
                { "ShortDescription", "اعتماد موضوع الرسالة العلمية وخطتها البحثية، وتحديد المشرف عليها، وفق القواعد المنظِّمة التي يقرّها مجلس الجامعة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "ألا يقلّ المنجَز من المقررات الدراسية عن 50% قبل التقدّم بتسجيل الخطة.\nألا يقلّ المعدل التراكمي عن تقدير «جيد جداً».\nسلامة الخطة فكرياً وخلوّها من المؤلفات المحظورة، وتقدّمها المرشدة الأكاديمية للقسم المختص." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة السادسة والثلاثون|يضع مجلس الجامعة القواعد المنظمة لتسجيل مشروع بحث التخرج، والرسالة العلمية لطالب الدراسات العليا — إن وجدت — وآلية تحديد المشرف على الرسالة العلمية، والمشرف المساعد — إن وجد — وذلك بناءً على توصية اللجنة الدائمة.\nالمادة الثامنة والثلاثون (الإشراف)|يشرف على الرسائل العلمية الأساتذة والأساتذة المشاركون من أعضاء هيئة التدريس بالجامعة، ويجوز للأستاذ المساعد أن يشرف على أبحاث التخرج ورسائل الماجستير، والمساعدة في الإشراف على رسائل الدكتوراه إذا كان لديه بحثان منشوران أو مقبولان للنشر في مجلة علمية محكّمة." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج تسجيل خطة البحث للرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b3%d8%ac%d9%8a%d9%84%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab%20%d9%84%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج إفادة عن محتوى ومراجع ومصادر خطة البحث / الرسالة / بحث التخرج|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d9%81%d8%a7%d8%af%d8%a9%20%d8%b9%d9%86%20%d9%85%d8%ad%d8%aa%d9%88%d9%89%20%d9%88%d9%85%d8%b1%d8%a7%d8%ac%d8%b9%20%d9%88%d9%85%d8%b5%d8%a7%d8%af%d8%b1%20%d8%ae%d8%b7%d8%a9%20%d8%a7%d9%84%d8%a8%d8%ad%d8%ab/gradpro.docx\nنموذج تقرير عن سير دراسة طالبة دراسات عليا|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d8%b9%d9%86%20%d8%b3%d9%8a%d8%b1%20%d8%af%d8%b1%d8%a7%d8%b3%d8%a9%20%d8%b7%d8%a7%d9%84%d8%a8%d8%a9%20%d8%af%d8%b1%d8%a7%d8%b3%d8%a7%d8%aa%20%d8%b9%d9%84%d9%8a%d8%a7.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة والمشرف ← توصية مجلس القسم ← مجلس الكلية ← اعتماد الخطة وتحديد المشرف." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إلغاء القيد" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة السادسة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-cancel.png" },
                { "ShortDescription", "إنهاء الجامعة علاقتَها بالطالب/ـة قبل إكمال برنامجها الدراسي لأسبابٍ محدّدةٍ نصّت عليها اللائحة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "يقع إلغاء القيد عند انطباق إحدى الحالات النظامية (الانقطاع، أو انخفاض المعدل عن «جيد جداً» في فصلين متتاليين، أو تجاوز المدة، أو عدم اجتياز التكميلية، أو الإخلال بالأمانة العلمية).\nأن يصدر بقرارٍ من اللجنة الدائمة بناءً على توصية القسم وتأييد مجلس الكلية في الحالات التي تستوجب ذلك." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة السادسة والعشرون|يُلغى قيد الطالب في الحالات الآتية: إذا انقطع عن الدراسة وفقاً للمادة الخامسة والعشرين؛ إذا لم يجتز المقررات التكميلية وفق الضوابط المقرّة؛ إذا انخفض معدله التراكمي عن تقدير (جيد جداً) في فصلين دراسيين متتاليين؛ إذا اتضح أن قبوله مبني على معلومات أو وثائق غير صحيحة كلياً أو جزئياً؛ إذا لم يحصل على الدرجة العلمية خلال المدة المحددة للبرنامج. ويُلغى القيد بقرار من اللجنة الدائمة بناءً على توصية مجلس القسم وتأييد من مجلس الكلية في حالات أخرى منها: ثبوت عدم الجدية في مرحلة الرسالة العلمية، وعدم اجتياز الاختبار الشامل بعد إعادته، وعدم صلاحية الرسالة للمناقشة، والإخلال بالأمانة العلمية." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج إلغاء قيد|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d9%84%d8%ba%d8%a7%d8%a1%20%d9%82%d9%8a%d8%af.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: توصية مجلس القسم ← تأييد مجلس الكلية ← قرار اللجنة الدائمة ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الإنذار الأكاديمي" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة الرابعة والأربعون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-warning.png" },
                { "ShortDescription", "إشعارٌ نظاميٌّ يوجَّه إلى الطالب/ـة عند تعثّر مسارها الأكاديمي؛ سواء بانخفاض المعدل أو بثبوت عدم الجدّية في مرحلة الرسالة العلمية." },
                { "ShortDescription_EN", "" },
                { "Requirements", "يُوجَّه الإنذار عند ثبوت عدم الجدية في مرحلة الرسالة بناءً على تقرير المشرف، أو عند انخفاض المعدل.\nإذا أُنذرت الطالبة مرتين ولم تتلافَ أسباب الإنذار يُلغى قيدها بتوصية مجلسي القسم والكلية." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الرابعة والأربعون|إذا ثبت عدم جدية الطالب في مرحلة الرسالة العلمية، بناءً على تقرير المشرف على الرسالة العلمية، يتم إنذار الطالب بخطاب من القسم المختص، وإذا أُنذر الطالب مرتين ولم يتلافَ أسباب الإنذار يُلغى قيده، بناءً على توصية مجلسي القسم والكلية." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج إنذار طالبة|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%86%d8%b0%d8%a7%d8%b1.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: القسم المختص ← مجلس الكلية ← إشعار الطالب/ـة." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إعادة القيد" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة السابعة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-reinstate.png" },
                { "ShortDescription", "إعادة الجامعة علاقتَها بالطالب/ـة التي أُلغي قيدها، عند توافر ظروفٍ قهريةٍ حالت دون مواصلة الدراسة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يكون الحائل دون مواصلة الدراسة ظرفاً قهرياً يقبله مجلسا القسم والكلية.\nمَن مضى على إلغاء قيدها أكثر من ثلاثة أعوام تُعامَل معاملة المستجدّة؛ ومن مضى ثلاثة أعوام أو أقل تُعيد بعض المقررات.\nصدور قرار إعادة القيد من اللجنة الدائمة." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة السابعة والعشرون|يجوز إعادة قيد الطالب الذي أُلغي قيده إذا كان الحائل دون مواصلة دراسته ظروفاً يقبلها مجلسا القسم والكلية، وتكون إعادة القيد بقرار من اللجنة الدائمة، مع مراعاة: مَن مضى على إلغاء قيده أكثر من ثلاثة أعوام دراسية يُعامل معاملة الطالب المستجد من حيث إعادة دراسة المقررات، ومَن مضى ثلاثة أعوام أو أقل يعيد دراسة بعض المقررات التي يحددها مجلس القسم ويوافق عليها مجلس الكلية." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج إعادة قيد|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%b9%d8%a7%d8%af%d8%a9%20%d9%82%d9%8a%d8%af.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: قبول مجلسي القسم والكلية للظروف ← قرار اللجنة الدائمة ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تشكيل لجنة مناقشة للرسالة العلمية" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة الثامنة والأربعون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-committee.png" },
                { "ShortDescription", "تكوين لجنةٍ لمناقشة الرسالة العلمية بعد تقديم المشرف تقريراً باكتمالها إلى رئيس القسم." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يتقدّم المشرف بتقرير اكتمال الرسالة إلى رئيس القسم، مرفقاً به نسخةٌ منها.\nأن تُكوَّن اللجنة بقرارٍ من مجلس الكلية بناءً على توصية مجلس القسم.\nأن تُعدّ اللجنة تقريرها موقَّعاً من جميع أعضائها خلال أسبوعٍ من المناقشة." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة السابعة والأربعون|يُقدِّم المشرف على الرسالة العلمية، بعد انتهاء الطالب من إعدادها، تقريراً عن اكتمالها إلى رئيس القسم، مرفقاً به نسخة من الرسالة العلمية، تمهيداً لاستكمال إجراءات المناقشة التي يحددها مجلس الكلية.\nالمادة الثامنة والأربعون|تُكوَّن لجنة لمناقشة الرسالة العلمية بقرار من مجلس الكلية، بناءً على توصية مجلس القسم.\nالمادة الخمسون|تُعِد لجنة المناقشة تقريراً يُوقَّع من جميع أعضائها، يُقدَّم إلى رئيس القسم خلال أسبوع من تاريخ المناقشة، متضمناً إحدى التوصيات: قبول الرسالة والتوصية بمنح الدرجة؛ أو قبولها مع إجراء تعديلات؛ أو استكمال أوجه النقص وإعادة المناقشة؛ أو عدم قبول الرسالة." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج ترشيح لجنة مناقشة الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b1%d8%b4%d9%8a%d8%ad%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج اعتماد لجنة مناقشة الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/1437-1438%d9%87/%d8%a7%d8%b9%d8%aa%d9%85%d8%a7%d8%af%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a91.docx\nنموذج تقرير لجنة مناقشة الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج الإقرار بإجراء التعديلات المطلوبة على الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a5%d9%82%d8%b1%d8%a7%d8%b1%20%d8%a8%d8%a5%d8%ac%d8%b1%d8%a7%d8%a1%20%d8%a7%d9%84%d8%aa%d8%b9%d8%af%d9%8a%d9%84%d8%a7%d8%aa%20%d8%a7%d9%84%d9%85%d8%b7%d9%84%d9%88%d8%a8%d8%a9%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: تقرير المشرف باكتمال الرسالة ← توصية مجلس القسم ← قرار مجلس الكلية بتكوين اللجنة ← المناقشة وإعداد تقرير اللجنة." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الفرصة الإستثنائية لرفع المعدل" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة الثامنة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-gpa.png" },
                { "ShortDescription", "منح الطالب/ـة فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسيٍّ كحدٍّ أعلى؛ لرفع معدلها التراكمي، استثناءً من حالة انخفاض المعدل في فصلين متتاليين." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن تكون فرصةً استثنائيةً واحدةً لا تزيد على عامٍ دراسيٍّ كحدٍّ أعلى.\nتُمنَح استثناءً من حالة انخفاض المعدل التراكمي عن تقدير «جيد جداً» في فصلين متتاليين.\nصدور قرار مجلس الكلية بالاستثناء بناءً على توصية القسم." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الثامنة والعشرون|لمجلس الكلية، استثناءً من الفقرة (ج) من البند (١) من المادة السادسة والعشرين من هذه اللائحة، منح الطالب فرصة استثنائية واحدة لا تزيد على عام دراسي كحد أعلى، بناءً على توصية مجلس القسم." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج طلب فرصة استثنائية لرفع المعدل التراكمي|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%b7%d9%84%d8%a8%20%d9%81%d8%b1%d8%b5%d8%a9%20%d8%a7%d8%b3%d8%aa%d8%ab%d9%86%d8%a7%d9%8a%d9%94%d9%8a%d8%a9%20%d9%84%d8%b1%d9%81%d8%b9%20%d8%a7%d9%84%d9%85%d8%b9%d8%af%d9%84%20%d8%a7%d9%84%d8%aa%d8%b1%d8%a7%d9%83%d9%85%d9%8a.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← قرار مجلس الكلية بالاستثناء." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "منح الدرجة العلمية" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المواد ٥٠ و ٥٣ و ٥٦" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-degree.png" },
                { "ShortDescription", "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار." },
                { "ShortDescription_EN", "" },
                { "Requirements", "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».\nاستكمال نماذج اللجنة والإقرار بالتعديلات وإخلاء الطرف، وتسليم نسخ الرسالة المعتمدة.\nرفع الوكيل المختص التوصية بمنح الدرجة إلى مجلس الجامعة (المفوَّض لمعالي رئيسة الجامعة)." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الثالثة والخمسون|يرفع رئيس القسم المختص تقرير لجنة المناقشة إلى عميد الكلية في مدة لا تتجاوز أسبوعين من تاريخ المناقشة؛ لرفعه إلى الإدارة التنفيذية.\nالمادة الرابعة والخمسون|يتخرج الطالب بعد إنهاء متطلبات التخرج بنجاح حسب خطة البرنامج، على ألا يقل معدله التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، على ألا يقل في كل الأحوال عن تقدير (جيد جداً).\nالمادة السادسة والخمسون|يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج تقرير لجنة مناقشة الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج الإقرار بإجراء التعديلات المطلوبة|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a5%d9%82%d8%b1%d8%a7%d8%b1%20%d8%a8%d8%a5%d8%ac%d8%b1%d8%a7%d8%a1%20%d8%a7%d9%84%d8%aa%d8%b9%d8%af%d9%8a%d9%84%d8%a7%d8%aa%20%d8%a7%d9%84%d9%85%d8%b7%d9%84%d9%88%d8%a8%d8%a9%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج إخلاء طرف|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: تقرير لجنة المناقشة ← رئيس القسم ← عميد الكلية ← الإدارة التنفيذية ← الوكيل المختص ← مجلس الجامعة." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الفرصة الاستثنائية للحصول على الدرجة" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "المادة التاسعة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-degree-opportunity.png" },
                { "ShortDescription", "منح الطالب/ـة فرصةً استثنائيةً لا تزيد على عامٍ دراسي؛ لاستكمال متطلبات الحصول على الدرجة، استثناءً من حالة عدم الحصول عليها خلال المدة المحددة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن تُقدَّم قبل نهاية المدة النظامية للبرنامج (بمدةٍ لا تتجاوز أربعة أسابيع قبل بدء آخر فصل دراسي من المدة المحددة).\nأن تُبنى على تقريرٍ من المشرف وتوصية مجلسي القسم والكلية.\nألا تزيد الفرصة على عامٍ دراسي، وتصدر بقرار اللجنة الدائمة." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة التاسعة والعشرون|للجنة الدائمة، استثناءً من الفقرة (هـ) من البند (١) من المادة السادسة والعشرين من هذه اللائحة، منح الطالب فرصة استثنائية لا تزيد على عام دراسي، بناءً على تقرير من المشرف وتوصية مجلسي القسم والكلية." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج طلب فرصة استثنائية للحصول على الدرجة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%b7%d9%84%d8%a8%20%d9%81%d8%b1%d8%b5%d8%a9%20%d8%a7%d8%b3%d8%aa%d8%ab%d9%86%d8%a7%d9%8a%d9%94%d9%8a%d8%a9%20%d9%84%d9%84%d8%ad%d8%b5%d9%88%d9%84%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%af%d8%b1%d8%ac%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9%20%281%29.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: تقرير المشرف ← توصية مجلسي القسم والكلية ← قرار اللجنة الدائمة بالاستثناء." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 9 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تعديل النتيجة" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "قواعد تنفيذية" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-result-edit.png" },
                { "ShortDescription", "تصحيح نتيجة مقررٍ بعد رصدها وفق الضوابط المقرّة وبالمسوّغات المعتمدة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يكون التعديل بمسوّغٍ معتمدٍ ووفق القواعد التنفيذية لرصد الدرجات.\nاعتماد التعديل عبر مجلسي القسم والكلية قبل تنفيذه." },
                { "Requirements_EN", "" },
                { "ArticleText", "الأساس النظامي|يستند تعديل النتيجة إلى القواعد التنفيذية المعتمدة لرصد الدرجات وتعديلها، وفقاً لما تقرّه الجامعة استناداً إلى لائحة الدراسة والاختبارات وأحكام اللائحة المنظمة." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج تعديل نتيجة طالبة|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d8%b9%d8%af%d9%8a%d9%84%20%d9%86%d8%aa%d9%8a%d8%ac%d8%a9%20%d8%b7%d8%a7%d9%84%d8%a8%d8%a9.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: عضو هيئة التدريس ← مجلس القسم ← مجلس الكلية ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 10 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إخلاء الطرف" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "College" },
                { "BadgeText", "إجراءات معتمدة" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-clearance.png" },
                { "ShortDescription", "استكمال الطالب/ـة براءةَ ذمّتها تجاه وحدات الجامعة عند الانسحاب أو التخرج أو إلغاء القيد." },
                { "ShortDescription_EN", "" },
                { "Requirements", "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.\nيُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب أو التخرج أو إلغاء القيد ومنح الدرجة." },
                { "Requirements_EN", "" },
                { "ArticleText", "الأساس الإجرائي|يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة، ويُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب والتخرج وإلغاء القيد ومنح الدرجة." },
                { "ArticleText_EN", "" },
                { "Forms", "نموذج إخلاء طرف|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← الوحدات ذات العلاقة ← اعتماد إخلاء الطرف." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "" },
                { "ItemOrder", 11 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تأجيل القبول" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "Student" },
                { "BadgeText", "المادة العشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-admission-delay.png" },
                { "ShortDescription", "إرجاء تاريخ بدء دراسة الطالب/ـة بعد صدور رقمها الجامعي وقبل تسجيل أي مقرر دراسي، ويكون مرةً واحدة، ولا يُقبل بعد بدء الدراسة." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يكون التأجيل قبل تسجيل أي مقرر دراسي، ولمرّةٍ واحدةٍ فقط.\nألا يُقبل الطلب بعد بدء الطالبة دراستها.\nصدور موافقة مجلس القسم المختص وعميد الكلية." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة العشرون|يجوز بموافقة مجلس القسم المختص وعميد الكلية تأجيل الطالب قبوله، ويكون لمرة واحدة، ولا يمكن قبول التأجيل بعد بدء الطالب دراسته، وفق ضوابط يقرها مجلس الجامعة، بناءً على اقتراح اللجنة الدائمة." },
                { "ArticleText_EN", "" },
                { "Forms", "" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← موافقة مجلس القسم المختص ← موافقة عميد الكلية ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج تأجيل القبول.docx" },
                { "ItemOrder", 12 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الاعتذار عن دراسة مقرر أو فصل" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "Student" },
                { "BadgeText", "المادة الثالثة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-course-apology.png" },
                { "ShortDescription", "إنهاء الطالب/ـة دراستَها لمقرر أو أكثر، أو لجميع مقررات الفصل الدراسي، بعد انتهاء فترة التسجيل." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يكون الاعتذار بعد انتهاء فترة التسجيل وضمن المدة المحددة في تقويم الإجراءات الأكاديمية.\nصدور توصية مجلس القسم المختص وموافقة عميد الكلية." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الثالثة والعشرون|يجوز للطالب الاعتذار عن الاستمرار في دراسة مقرر أو أكثر، أو جميع مقررات الفصل الدراسي، وذلك بعد انتهاء فترة التسجيل، بتوصية من مجلس القسم المختص، وموافقة من عميد الكلية، وفق ضوابط يقرها مجلس الجامعة بناءً على اقتراح اللجنة الدائمة." },
                { "ArticleText_EN", "" },
                { "Forms", "" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← موافقة عميد الكلية ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج الاعتذار  عن الاستمرار  في دراسة مقرر أو أكثر أو جميع مقررات الفصل الدراسي.docx" },
                { "ItemOrder", 13 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "تأجيل الدراسة" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "Student" },
                { "BadgeText", "المادة الثانية والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-study-delay.png" },
                { "ShortDescription", "إيقاف الطالب/ـة سريانَ المدة المقررة للحصول على الدرجة العلمية بعد بدء دراستها." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يصل الطلب مكتملاً للإدارة التنفيذية في موعدٍ أقصاه نهاية الأسبوع الثالث من بدء الفصل.\nألا يتجاوز التأجيل فصلين دراسيين متتاليين، ويُقدَّم لكل فصلٍ طلبٌ مستقل.\nألا يتجاوز مجموع مدة التأجيل نصف المدة المقررة للبرنامج، ولا تُحتسب ضمن مدة الحصول على الدرجة." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الثانية والعشرون|يجوز للطالب تأجيل الدراسة بتوصية من مجلس القسم المختص وموافقة مجلس الكلية، وفق ضوابط يقرها مجلس الجامعة، بناءً على اقتراح اللجنة الدائمة.\nالقواعد التنفيذية|لا تُحتسب مدة التأجيل ضمن المدة المقررة للحصول على الدرجة، وألا يتجاوز مجموع مدة التأجيل نصف المدة المقررة للبرنامج." },
                { "ArticleText_EN", "" },
                { "Forms", "" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← توصية مجلس القسم المختص ← موافقة مجلس الكلية ← الإدارة التنفيذية للتنفيذ." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/نموذج تأجيل الدراسة.docx" },
                { "ItemOrder", 14 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "الانسحاب" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "SubGroupKey", "Student" },
                { "BadgeText", "المادة الرابعة والعشرون" },
                { "BadgeText_EN", "" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/enrolled-procedure-withdrawal.png" },
                { "ShortDescription", "إنهاء الطالب/ـة علاقتَها بالجامعة المقيَّدة بها بشكلٍ نهائي، قبل إكمال برنامجها الدراسي." },
                { "ShortDescription_EN", "" },
                { "Requirements", "أن يكون الانسحاب قبل إكمال البرنامج الدراسي.\nاستكمال إجراءات إخلاء الطرف المرتبطة بالانسحاب.\nعند الرغبة في العودة لاحقاً تُطبَّق شروط القبول وقت التقديم الجديد." },
                { "Requirements_EN", "" },
                { "ArticleText", "المادة الرابعة والعشرون|إذا انسحب طالب الدراسات العليا من الجامعة ثم أراد الالتحاق بها مجدداً يجوز للجامعة أن تطبق عليه شروط القبول وقت التقديم الجديد.\nتعريف الانسحاب (المادة الأولى)|إنهاء الطالب علاقته بالجامعة المقيد بها بشكل نهائي، قبل إكماله برنامجه الدراسي." },
                { "ArticleText_EN", "" },
                { "Forms", "" },
                { "Forms_EN", "" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← القسم المختص ← الكلية ← الإدارة التنفيذية، مع إخلاء الطرف." },
                { "FlowText_EN", "" },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/2نموذج أنسحاب.docx" },
                { "ItemOrder", 15 }
            });

            // 7. Degree Conferral (Graduates)
            list.Add(new Dictionary<string, object> {
                { "Title", "منح الدرجة العلمية" },
                { "Title_EN", "Conferral of Academic Degree" },
                { "Phase", DgaPgPhases.Graduates },
                { "BadgeText", "المواد ٥٣ و٥٤ و٥٦" },
                { "BadgeText_EN", "Articles 53, 54 & 56" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/graduate-procedure-degree.png" },
                { "ShortDescription", "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار." },
                { "ShortDescription_EN", "Graduation occurs upon successfully fulfilling all program requirements and recommending degree conferral to the University Council." },
                { "Requirements", "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».\nاستكمال نماذج اللجنة والإقرار بالتعديلات وإخلاء الطرف، وتسليم نسخ الرسالة المعتمدة.\nرفع الوكيل المختص التوصية بمنح الدرجة إلى مجلس الجامعة (المفوَّض لمعالي رئيسة الجامعة)." },
                { "Requirements_EN", "Complete graduation requirements with minimum GPA of Very Good.\nSubmit committee report, revision acknowledgment, clearance, and approved thesis copies.\nVice Rector submits conferral recommendation to University Council." },
                { "ArticleText", "المادة الثالثة والخمسون|يرفع رئيس القسم المختص تقرير لجنة المناقشة إلى عميد الكلية في مدة لا تتجاوز أسبوعين من تاريخ المناقشة؛ لرفعه إلى الإدارة التنفيذية.\n\nالمادة الرابعة والخمسون|يتخرج الطالب بعد إنهاء متطلبات التخرج بنجاح حسب خطة البرنامج، على ألا يقل معدله التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، على ألا يقل في كل الأحوال عن تقدير (جيد جداً).\n\nالمادة السادسة والخمسون|يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار." },
                { "ArticleText_EN", "Article 53|Department head submits defense report to dean within two weeks.\n\nArticle 54|Graduation requires completing study plan with minimum GPA of Very Good.\n\nArticle 56|Vice Rector submits degree conferral recommendation to University Council." },
                { "Forms", "نموذج تقرير لجنة مناقشة الرسالة العلمية|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج الإقرار بإجراء التعديلات المطلوبة|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a5%d9%82%d8%b1%d8%a7%d8%b1%20%d8%a8%d8%a5%d8%ac%d8%b1%d8%a7%d8%a1%20%d8%a7%d9%84%d8%aa%d8%b9%d8%af%d9%8a%d9%84%d8%a7%d8%aa%20%d8%a7%d9%84%d9%85%d8%b7%d9%84%d9%88%d8%a8%d8%a9%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx\nنموذج إخلاء طرف|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" },
                { "Forms_EN", "Defense Committee Report Form|https://pnu.edu.sa/en/Deanship/postgraduate/Documents/defense_report.docx\nRevisions Acknowledgment Form|https://pnu.edu.sa/en/Deanship/postgraduate/Documents/amendment_acknowledgement.docx\nClearance Form|https://pnu.edu.sa/en/Deanship/postgraduate/Documents/clearance.docx" },
                { "FlowText", "مسار الإجراء الأكاديمي: تقرير لجنة المناقشة ← رئيس القسم ← عميد الكلية ← الإدارة التنفيذية ← الوكيل المختص ← مجلس الجامعة." },
                { "FlowText_EN", "Procedure Workflow: Defense Committee Report ← Department Head ← College Dean ← Executive Admin ← Vice Rector ← University Council." },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Pages/e-services.aspx" },
                { "ItemOrder", 1 }
            });

            // 8. Clearance (Graduates)
            list.Add(new Dictionary<string, object> {
                { "Title", "إخلاء الطرف" },
                { "Title_EN", "Clearance" },
                { "Phase", DgaPgPhases.Graduates },
                { "BadgeText", "إجراءات معتمدة" },
                { "BadgeText_EN", "Approved Procedures" },
                { "IconUrl", "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/graduate-procedure-exit.png" },
                { "ShortDescription", "استكمال الطالب/ـة براءةَ ذمّتها تجاه وحدات الجامعة عند الانسحاب أو التخرج أو إلغاء القيد." },
                { "ShortDescription_EN", "Student completes clearance from all university departments upon graduation, withdrawal, or cancellation of enrollment." },
                { "Requirements", "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.\nيُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب أو التخرج أو إلغاء القيد ومنح الدرجة." },
                { "Requirements_EN", "Obtain approvals from Central Library, Student Housing, Financial Affairs, and Graduate Studies Deanship.\nFinal prerequisite before receiving official graduation certificate." },
                { "ArticleText", "الأساس الإجرائي|يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة، ويُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب والتخرج وإلغاء القيد ومنح الدرجة." },
                { "ArticleText_EN", "Procedural Basis|Clearance is processed under approved deanship protocols as a final step for degree issuance." },
                { "Forms", "نموذج إخلاء طرف|https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" },
                { "Forms_EN", "Clearance Form|https://pnu.edu.sa/en/Deanship/postgraduate/Documents/clearance.docx" },
                { "FlowText", "مسار الإجراء الأكاديمي: الطالب/ـة ← الوحدات ذات العلاقة ← اعتماد إخلاء الطرف." },
                { "FlowText_EN", "Procedure Workflow: Student ← Relevant Departments ← Clearance Approval." },
                { "DirectServiceUrl", "https://pnu.edu.sa/ar/Pages/e-services.aspx" },
                { "ItemOrder", 2 }
            });

            return list;
        }
    }
}
