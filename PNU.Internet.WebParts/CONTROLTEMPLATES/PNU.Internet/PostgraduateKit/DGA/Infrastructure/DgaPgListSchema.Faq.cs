using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public static partial class DgaPgListSchema
    {
        private static void RegisterFaqLists()
        {
            var faq = new DgaPgListDef(DgaPgListNames.Faq, "الأسئلة الشائعة (DGA)", "الأسئلة الأكثر شيوعاً وإجاباتها لكل فئة");
            faq.AddField("Title_EN", "Question (EN)", SPFieldType.Text, false);
            faq.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.Stations);
            faq.AddField("Answer", "Answer (AR)", SPFieldType.Note, true);
            faq.AddField("Answer_EN", "Answer (EN)", SPFieldType.Note, false);
            faq.AddField("Category", "Category", SPFieldType.Text, false);
            faq.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            faq.SeedDataFactory = GetFaqSeed;
            Register(faq);
        }

        private static List<Dictionary<string, object>> GetFaqSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Applicants FAQ - 39 questions transcribed from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "متى يُفتح باب القبول لبرامج الدراسات العليا؟" },
                { "Title_EN", "When does admission open for postgraduate programs?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يُعلَن عن فتح القبول عبر موقع عمادة الدراسات العليا وقنواتها الرسمية في مواعيد محدّدة لكل عام؛ ويمكن متابعة المواعيد عبر صفحة مواعيد وبوابة القبول." },
                { "Answer_EN", "Admission openings are announced on the Deanship of Graduate Studies website and official channels on scheduled dates each year." },
                { "Category", "القبول والتسجيل" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما شروط القبول؟" },
                { "Title_EN", "What are the admission requirements?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تختلف الشروط بحسب البرنامج والمرحلة، وتشمل شروطاً عامةً (كالمؤهّل الجامعي والتقدير) وأخرى خاصةً بكل برنامج. ولأنها قد تتغيّر، يُرجى الرجوع إلى صفحة شروط القبول الرسمية للاطّلاع على المحدّث منها." },
                { "Answer_EN", "Requirements vary by program and stage, comprising general criteria and program-specific conditions." },
                { "Category", "القبول والتسجيل" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المستندات المطلوبة للتقديم؟" },
                { "Title_EN", "What documents are required for application?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُحدَّد المستندات وفق شروط كل برنامج، وتشمل غالباً المؤهّل الجامعي والسجل الأكاديمي والتزكيات والهوية؛ وتُرفع إلكترونياً عبر بوابة القبول." },
                { "Answer_EN", "Documents generally include graduation certificate, academic transcript, recommendation letters, and national ID/passport." },
                { "Category", "القبول والتسجيل" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "لم أحقّق شرط اللغة الإنجليزية، ما الحلّ؟" },
                { "Title_EN", "I didn't meet the English requirement, what is the solution?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يمكنكِ الالتحاق بالدورة التأهيلية للغة الإنجليزية بمعهد اللغة الإنجليزية، فهي تُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول." },
                { "Answer_EN", "You may enroll in the English Qualifying Course offered by the English Language Institute, which satisfies the requirement." },
                { "Category", "اللغة الإنجليزية" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف تتمّ المفاضلة بين المتقدّمات؟" },
                { "Title_EN", "How is candidate evaluation and selection conducted?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تتمّ المفاضلة وفق معايير كل برنامج (كالتقدير الجامعي والاختبارات والمقابلات إن وُجدت)، وتُعلَن النتائج عبر البوابة." },
                { "Answer_EN", "Evaluation is based on program criteria (GPA, standardized tests, interviews) and results are published via portal." },
                { "Category", "المفاضلة والقبول" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل يمكنني التقديم على أكثر من برنامج؟" },
                { "Title_EN", "Can I apply for more than one program?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تتيح بوابة القبول التقديم وفق ضوابط كل فصل؛ ويُنصح بمراجعة دليل المستخدم وصفحة البرامج لمعرفة الخيارات المتاحة وآليّة الترشيح." },
                { "Answer_EN", "The admission portal specifies options per term; refer to the user guide for allowable choices." },
                { "Category", "التقديم" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل الدراسة في برامج الدراسات العليا بمقابلٍ مادي؟" },
                { "Title_EN", "Are postgraduate programs tuition-based?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تتفاوت البرامج بين مجانيةٍ ومدفوعة، وتُوضَّح الرسوم في صفحة كل برنامج؛ لذا يُرجى الرجوع إلى صفحة البرامج والرسوم للاطّلاع على المحدّث." },
                { "Answer_EN", "Programs vary between funded and tuition-based; tuition details are specified on each program's page." },
                { "Category", "الرسوم الدراسية" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الدورة التأهيلية للغة الإنجليزية، ولمن تُوجَّه؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "هي دورةٌ يقدّمها معهد اللغة الإنجليزية للطالبات الحاصلات على قبولٍ مشروط، تُعِدّهنّ بالمهارات اللغوية اللازمة، وتُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أتابع حالة طلب قبولي؟" },
                { "Title_EN", "Where can I track my application status?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يمكنكِ متابعة حالة طلبكِ عبر بوابة القبول الإلكترونية التي قدّمتِ من خلالها، ومتابعة إعلانات النتائج عبر قنوات العمادة الرسمية." },
                { "Answer_EN", "You can track your application status directly via the online admissions portal." },
                { "Category", "متابعة الطلب" },
                { "ItemOrder", 9 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أجد جدولي الدراسي؟" },
                { "Title_EN", "Where do I find my class schedule?" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تجدين جدولكِ الدراسي في نظام إدارة التعلم الإلكتروني «Blackboard» بعد الدخول إلى صفحتكِ باسم المستخدم وكلمة المرور الجامعية. وعند عدم ظهور الجدول أو وجود تعارض، تواصلي مع القسم أو البرنامج الأكاديمي للتحقق من حالة التسجيل." },
                { "Answer_EN", "Your schedule is accessible on Blackboard and the Student Information System using university credentials." },
                { "Category", "الجداول والأنظمة" },
                { "ItemOrder", 10 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أدخل على البلاك بورد؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تدخلين إلى نظام إدارة التعلم الإلكتروني «Blackboard» باستخدام حسابكِ الجامعي، عبر اسم المستخدم وكلمة المرور المعتمدة للأنظمة الجامعية. وعند وجود مشكلةٍ في الدخول أو كلمة المرور، تواصلي مع الدعم الفني المختص." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 11 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل إذا لم يظهر جدولي الدراسي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "راجعي القسم أو البرنامج الأكاديمي أولاً للتأكد من حالة تسجيلكِ والمقررات المطروحة، ثم تابعي مع الجهة المختصة بحسب الإجراء المعتمد." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 12 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الأنظمة الإلكترونية التي أحتاج استخدامها خلال دراستي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تحتاجين إلى متابعة البريد الجامعي، ونظام إدارة التعلم الإلكتروني «Blackboard»، والنظام الأكاديمي، وأي روابط أو أنظمةٍ تعلنها الجامعة أو العمادة لاستكمال التسجيل ومتابعة الإجراءات الأكاديمية. وتُدرِج جامعة الأميرة نورة نظام إدارة التعلم الإلكتروني ضمن أنظمتها بوصفه منصة تعلّمٍ إلكترونيٍّ متكاملة، كما تُدرِج النظام الأكاديمي للطالبات لإدارة عملياتهنّ الأكاديمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 13 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أتابع حالة طلب القبول؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تتابعين حالة طلب القبول عبر بوابة القبول الإلكترونية أو القناة الرسمية المعلنة عند التقديم، مع أهمية متابعة البريد الإلكتروني والجوال المسجّلين في الطلب؛ فهما من قنوات إشعار المتقدمة بأي تحديثاتٍ أو مواعيدَ مهمة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 14 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا تعني أوضاع طلب القبول؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "توضّح أوضاع طلب القبول المرحلة التي بلغها طلبكِ، مثل: مكتمل، غير مكتمل، مرشحة للمقابلة، مقبولة مبدئياً، مقبولة قبولاً مشروطاً، مقبولة نهائياً، أو غير مقبولة. وتُتابَع الحالة باستمرارٍ عبر بوابة القبول والقنوات الرسمية المعلنة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 15 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أعرف أنني مرشحة للمقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُشعَر المرشحات للمقابلة الشخصية عبر بوابة القبول أو القنوات الرسمية المعلنة، وقد يتضمّن الإشعار موعد المقابلة، وطريقتها، والجهة المنفّذة لها، وأي تعليماتٍ يجب الالتزام بها." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 16 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى أعرف نتيجة المقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُعلَن نتيجة المقابلة الشخصية وفق الجدول الزمني المعتمد للقبول، وتظهر عادةً عبر بوابة القبول أو القنوات الرسمية المعلنة؛ فاحرصي على متابعة حالة طلبكِ والبريد الإلكتروني والجوال المسجّلين فيه." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 17 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا فاتتني المقابلة الشخصية، ماذا أفعل؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "بادري فوراً بالتواصل مع الجهة الموضّحة في إعلان المقابلات أو مع البرنامج الأكاديمي المختص، مع إرفاق بياناتكِ وسبب عدم الحضور. وإعادة جدولة المقابلة ليست مضمونة، وتخضع لتقدير الجهة المختصة والمدة الزمنية المتاحة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 18 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل يمكن إعادة جدولة المقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يُنظَر في إعادة جدولة المقابلة إذا وُجد مبرّرٌ مقبولٌ وسمحت المدة الزمنية والإجراءات بذلك، غير أن الموافقة ليست تلقائية، وتخضع لما تقرّره الجهة الأكاديمية المختصة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 19 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل أستطيع القبول إذا لم أحقق شرط اللغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "لا يُستكمل القبول دون تحقيق شرط اللغة الإنجليزية المعتمد للبرنامج. ويكون تحقيقه إمّا بالحصول على الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ مثل IELTS أو TOEFL، أو باجتياز الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا متى كانت متاحةً ومعتمدةً ضمن شروط القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 20 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل إذا لم أحقق شرط اللغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "اختاري أحد المسارين: التقدّم لاختبار لغةٍ إنجليزيةٍ معتمدٍ وتحقيق الدرجة المطلوبة، أو التسجيل في الدورة التأهيلية للغة الإنجليزية واجتيازها وفق الضوابط المعلنة. ولا يُعدّ الطلب مستوفياً لشرط اللغة إلا بعد تحقيق الدرجة المطلوبة أو اجتياز الدورة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 21 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "برنامجٌ مخصّصٌ للمقبلات على برامج الدراسات العليا بجامعة الأميرة نورة بنت عبد الرحمن، يهدف إلى تأهيلهنّ لغوياً وأكاديمياً لاستيفاء متطلبات القبول في برامج الدراسات العليا." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 22 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أسجّل في الدورة التأهيلية للغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تسجّلين في الدورة عبر رابط التسجيل المعلن من معهد اللغة الإنجليزية. وتابعي الإعلان الرسمي لمعرفة آخر موعدٍ للتسجيل، ومقر الدورة، وتاريخ بداية البرنامج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 23 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل التسجيل في الدورة يعني أنني حققت شرط اللغة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "التسجيل في الدورة لا يعني تحقيق شرط اللغة. التزمي بمتطلبات الدورة واجتيازها وفق الآلية المعتمدة، ثم زوّدي الجهة المختصة بما يثبت الاجتياز لاستكمال إجراءات القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 24 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل بعد اجتياز الدورة التأهيلية للغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "بعد الاجتياز، احتفظي بما يثبت تحقيق شرط اللغة، ثم ارفعيه أو أرسليه عبر القناة الرسمية المحددة في إعلان القبول أو تعليمات العمادة، حتى تُحدَّث حالة طلبكِ وتُستكمل إجراءات القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 25 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا اختبرت IELTS وحققت الدرجة المطلوبة، هل أحتاج الدورة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "إذا حققتِ الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ وفق شروط البرنامج، فلا تحتاجين غالباً إلى الالتحاق بالدورة التأهيلية؛ والمهمّ رفع ما يثبت تحقيق شرط اللغة عبر القناة الرسمية المحددة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 26 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا لم أجتز الدورة التأهيلية للغة الإنجليزية، ماذا يحدث؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "عدم اجتياز شرط اللغة الإنجليزية خلال المدة المحددة قد يؤثّر على استكمال القبول أو الانتظام في البرنامج؛ فتواصلي مع الجهة المختصة لمعرفة الإجراء النظامي المتاح وفق تعليمات القبول. وقد أعلن معهد اللغة الإنجليزية بجامعة الأميرة نورة بدء التسجيل في الدورة التأهيلية للغة الإنجليزية للمتقدمات إلى برامج الدراسات العليا، وأن البرنامج يبدأ بتاريخ 28 يونيو 2026م." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 27 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل أستطيع إكمال الدكتوراه بعد الماجستير؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يمكن إكمال الدكتوراه بعد الماجستير متى استوفيتِ شروط القبول في البرنامج. ويختلف المسار بحسب نوع الماجستير؛ فإن كان أكاديمياً أمكن التقديم على الدكتوراه الأكاديمية، وإن كان مهنياً فالمسار المناسب بعده الدكتوراه المهنية، ولا يؤهّل عادةً للتقديم على الدكتوراه الأكاديمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 28 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الفرق بين الماجستير الأكاديمي والماجستير المهني؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "الماجستير الأكاديمي يركّز على التعمّق العلمي والبحثي في التخصص، ويهيّئ الطالبة لمسارات البحث العلمي والدكتوراه الأكاديمية. والماجستير المهني يركّز على التطبيق المتقدّم للمعرفة والمهارات في مجالٍ مهنيٍّ محدد، ويهيّئ الطالبة للممارسة المهنية المتقدمة أو الدكتوراه المهنية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 29 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل الماجستير المهني يؤهل للدكتوراه الأكاديمية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "الماجستير المهني ليس المسار المناسب للدكتوراه الأكاديمية، وإنما يكون مساره غالباً نحو الدكتوراه المهنية أو المسارات التطبيقية المتقدمة، وفق شروط القبول المعتمدة لكل برنامج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 30 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل الماجستير الأكاديمي يؤهل للدكتوراه الأكاديمية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "الماجستير الأكاديمي هو المسار المناسب للتقديم على الدكتوراه الأكاديمية، مع استيفاء شروط القبول الأخرى التي يحددها البرنامج، كالتخصص، والمعدل، ومتطلبات اللغة، وأي شروطٍ إضافية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 31 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المقصود بالدكتوراه المهنية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "مؤهّلٌ متقدّمٌ يركّز على توظيف المعرفة والمهارات البحثية والتطبيقية لمعالجة قضايا مهنية وتطوير الممارسة في مجالٍ محدد، ويختلف عن الدكتوراه الأكاديمية التي تركّز غالباً على البحث العلمي وإنتاج المعرفة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 32 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا كنت أرغب في الدكتوراه الأكاديمية، أي مسار ماجستير أختار؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "إذا كان هدفكِ إكمال الدكتوراه الأكاديمية، فالمسار الأنسب الماجستير الأكاديمي؛ لارتباطه بالبحث العلمي والتعمّق النظري والمنهجي في التخصص. والماجستير المهني يناسب غالباً من ترغب في تطوير ممارستها المهنية أو التوجّه إلى الدكتوراه المهنية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 33 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أتقدّم للسكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تتقدّمين عبر خدمة التسجيل الإلكتروني في سكن الطالبات بعمادة شؤون الطالبات، من خلال الرابط الرسمي للخدمة: خدمة التسجيل الإلكتروني في سكن الطالبات ، مع استيفاء الشروط والمستندات المطلوبة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 34 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل يحقّ لطالبة الدراسات العليا السكن في السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يتيح السكن الجامعي خدماته لطالبات الجامعة وفق الشروط والفئات المعتمدة، وتُدرَج طالبة الدراسات العليا ضمن الفئات المستفيدة وفق الضوابط المعلنة في صفحة الخدمة الرسمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 35 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما رسوم السكن لطالبة الدراسات العليا؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُحدَّد رسوم السكن لطالبة الدراسات العليا وفق ما تعلنه عمادة شؤون الطالبات في صفحة الخدمة الرسمية؛ فراجعي الصفحة للاطّلاع على الرسوم المحدّثة وآلية سدادها." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 36 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المستندات المطلوبة للتقديم على السكن؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُحدَّد المستندات في صفحة خدمة التسجيل الإلكتروني في سكن الطالبات، وتشمل غالباً إثبات القيد والهوية والبيانات التي تطلبها الخدمة؛ فاتّبعي ما هو مبيَّنٌ في الصفحة الرسمية عند التقديم." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 37 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى أسجّل في السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "يُفتح التسجيل في السكن وفق المواعيد التي تعلنها عمادة شؤون الطالبات؛ فتابعي صفحة الخدمة الرسمية وقنوات العمادة لمعرفة مواعيد فتح التسجيل والتعليمات المصاحبة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 38 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "بمن أتواصل بشأن السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Applicants },
                { "Answer", "تُدار خدمات السكن عبر عمادة شؤون الطالبات، ويمكنكِ التواصل معها ومتابعة مستجدّات الخدمة من خلال صفحة خدمة التسجيل الإلكتروني في سكن الطالبات وقنوات العمادة الرسمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 39 }
            });

            // Enrolled students FAQ - 41 questions transcribed from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "كم مرّةً يمكنني تأجيل القبول؟ ومتى؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تأجيل القبول يكون لمرّةٍ واحدةٍ فقط، بعد صدور الرقم الجامعي وقبل تسجيل أي مقرر، ولا يُقبل بعد بدء الدراسة، بموافقة مجلس القسم وعميد الكلية (المادة العشرون)." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الفرق بين تأجيل القبول وتأجيل الدراسة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تأجيل القبول يكون قبل بدء الدراسة وقبل تسجيل أي مقرر (المادة العشرون)؛ أمّا تأجيل الدراسة فيكون بعد بدء الدراسة بإيقاف سريان المدة النظامية (المادة الثانية والعشرون)، بموافقة مجلس الكلية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل تُحتسب مدة التأجيل ضمن المدة النظامية للحصول على الدرجة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "لا تُحتسب مدة تأجيل الدراسة ضمن المدة المقررة للحصول على الدرجة، على ألا يتجاوز مجموع التأجيل نصف المدة المقررة للبرنامج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى يُلغى قيد الطالب/ـة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يُلغى القيد عند انطباق إحدى الحالات النظامية، ومنها: الانقطاع عن الدراسة، أو انخفاض المعدل التراكمي عن تقدير «جيد جداً» في فصلين متتاليين، أو عدم اجتياز المقررات التكميلية، أو تجاوز المدة المحددة للبرنامج، أو الإخلال بالأمانة العلمية (المادة السادسة والعشرون)." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما شروط التقدّم بتسجيل خطة البحث للرسالة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يُشترط إنجاز ما لا يقل عن 50% من المقررات الدراسية، وألا يقلّ المعدل التراكمي عن تقدير «جيد جداً»، وتتقدّم المرشدة الأكاديمية بالطلب للقسم المختص (المادة السادسة والثلاثون)." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الحدّ الأدنى للمعدل التراكمي للتخرّج ومنح الدرجة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "ألا يقلّ المعدل التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، وفي كل الأحوال لا يقلّ عن تقدير «جيد جداً» (المادة الرابعة والخمسون)." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الفرق بين الفرصة الاستثنائية لرفع المعدل والفرصة للحصول على الدرجة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "الأولى (المادة الثامنة والعشرون) تُمنَح بقرار مجلس الكلية لمعالجة انخفاض المعدل؛ والثانية (المادة التاسعة والعشرون) تُمنَح بقرار اللجنة الدائمة بناءً على تقرير المشرف عند انتهاء المدة دون الحصول على الدرجة، وكلتاهما لا تزيد على عامٍ دراسي." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى أحتاج إخلاء الطرف؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يُستكمل إخلاء الطرف عند الانسحاب أو التخرج أو إلغاء القيد، بوصفه خطوةً ختاميةً لبراءة الذمّة تجاه وحدات الجامعة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أعرف الإجراء الأكاديمي المناسب لحالتي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يعرض محور «الإجراءات الأكاديمية التفاعلية» الإجراءات كاملةً؛ وبالضغط على أيّ إجراءٍ تظهر شروطه ونصّ مادته ونماذجه المعتمدة ومساره، لتختاري ما يناسب حالتكِ." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 9 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أجد النماذج المعتمدة للإجراءات؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "ترتبط نماذج كل إجراءٍ مباشرةً بموقع العمادة داخل بطاقة الإجراء نفسه، وتُحدَّث تلقائياً بتحديث المصدر الرسمي." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 10 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المقصود بحافز النشر العلمي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "هو حافزٌ يشجّع طالبات الدراسات العليا على النشر العلمي المحكّم؛ وتجدين تفاصيله وضوابطه في المحور المخصّص له ضمن هذه المحطّة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 11 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أجد جدولي الدراسي؟" },
                { "Title_EN", "Where do I find my class schedule?" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تجدين جدولكِ الدراسي في نظام إدارة التعلم الإلكتروني «Blackboard» بعد الدخول إلى صفحتكِ باسم المستخدم وكلمة المرور الجامعية. وعند عدم ظهور الجدول أو وجود تعارض، تواصلي مع القسم أو البرنامج الأكاديمي للتحقق من حالة التسجيل." },
                { "Answer_EN", "Your schedule is accessible on Blackboard and the Student Information System using university credentials." },
                { "Category", "الجداول والأنظمة" },
                { "ItemOrder", 12 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أدخل على البلاك بورد؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تدخلين إلى نظام إدارة التعلم الإلكتروني «Blackboard» باستخدام حسابكِ الجامعي، عبر اسم المستخدم وكلمة المرور المعتمدة للأنظمة الجامعية. وعند وجود مشكلةٍ في الدخول أو كلمة المرور، تواصلي مع الدعم الفني المختص." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 13 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل إذا لم يظهر جدولي الدراسي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "راجعي القسم أو البرنامج الأكاديمي أولاً للتأكد من حالة تسجيلكِ والمقررات المطروحة، ثم تابعي مع الجهة المختصة بحسب الإجراء المعتمد." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 14 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الأنظمة الإلكترونية التي أحتاج استخدامها خلال دراستي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تحتاجين إلى متابعة البريد الجامعي، ونظام إدارة التعلم الإلكتروني «Blackboard»، والنظام الأكاديمي، وأي روابط أو أنظمةٍ تعلنها الجامعة أو العمادة لاستكمال التسجيل ومتابعة الإجراءات الأكاديمية. وتُدرِج جامعة الأميرة نورة نظام إدارة التعلم الإلكتروني ضمن أنظمتها بوصفه منصة تعلّمٍ إلكترونيٍّ متكاملة، كما تُدرِج النظام الأكاديمي للطالبات لإدارة عملياتهنّ الأكاديمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 15 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "أين أتابع حالة طلب القبول؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تتابعين حالة طلب القبول عبر بوابة القبول الإلكترونية أو القناة الرسمية المعلنة عند التقديم، مع أهمية متابعة البريد الإلكتروني والجوال المسجّلين في الطلب؛ فهما من قنوات إشعار المتقدمة بأي تحديثاتٍ أو مواعيدَ مهمة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 16 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا تعني أوضاع طلب القبول؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "توضّح أوضاع طلب القبول المرحلة التي بلغها طلبكِ، مثل: مكتمل، غير مكتمل، مرشحة للمقابلة، مقبولة مبدئياً، مقبولة قبولاً مشروطاً، مقبولة نهائياً، أو غير مقبولة. وتُتابَع الحالة باستمرارٍ عبر بوابة القبول والقنوات الرسمية المعلنة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 17 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أعرف أنني مرشحة للمقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تُشعَر المرشحات للمقابلة الشخصية عبر بوابة القبول أو القنوات الرسمية المعلنة، وقد يتضمّن الإشعار موعد المقابلة، وطريقتها، والجهة المنفّذة لها، وأي تعليماتٍ يجب الالتزام بها." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 18 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى أعرف نتيجة المقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تُعلَن نتيجة المقابلة الشخصية وفق الجدول الزمني المعتمد للقبول، وتظهر عادةً عبر بوابة القبول أو القنوات الرسمية المعلنة؛ فاحرصي على متابعة حالة طلبكِ والبريد الإلكتروني والجوال المسجّلين فيه." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 19 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا فاتتني المقابلة الشخصية، ماذا أفعل؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "بادري فوراً بالتواصل مع الجهة الموضّحة في إعلان المقابلات أو مع البرنامج الأكاديمي المختص، مع إرفاق بياناتكِ وسبب عدم الحضور. وإعادة جدولة المقابلة ليست مضمونة، وتخضع لتقدير الجهة المختصة والمدة الزمنية المتاحة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 20 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل يمكن إعادة جدولة المقابلة الشخصية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يُنظَر في إعادة جدولة المقابلة إذا وُجد مبرّرٌ مقبولٌ وسمحت المدة الزمنية والإجراءات بذلك، غير أن الموافقة ليست تلقائية، وتخضع لما تقرّره الجهة الأكاديمية المختصة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 21 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل أستطيع القبول إذا لم أحقق شرط اللغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "لا يُستكمل القبول دون تحقيق شرط اللغة الإنجليزية المعتمد للبرنامج. ويكون تحقيقه إمّا بالحصول على الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ مثل IELTS أو TOEFL، أو باجتياز الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا متى كانت متاحةً ومعتمدةً ضمن شروط القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 22 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل إذا لم أحقق شرط اللغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "اختاري أحد المسارين: التقدّم لاختبار لغةٍ إنجليزيةٍ معتمدٍ وتحقيق الدرجة المطلوبة، أو التسجيل في الدورة التأهيلية للغة الإنجليزية واجتيازها وفق الضوابط المعلنة. ولا يُعدّ الطلب مستوفياً لشرط اللغة إلا بعد تحقيق الدرجة المطلوبة أو اجتياز الدورة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 23 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "برنامجٌ مخصّصٌ للمقبلات على برامج الدراسات العليا بجامعة الأميرة نورة بنت عبد الرحمن، يهدف إلى تأهيلهنّ لغوياً وأكاديمياً لاستيفاء متطلبات القبول في برامج الدراسات العليا." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 24 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أسجّل في الدورة التأهيلية للغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تسجّلين في الدورة عبر رابط التسجيل المعلن من معهد اللغة الإنجليزية. وتابعي الإعلان الرسمي لمعرفة آخر موعدٍ للتسجيل، ومقر الدورة، وتاريخ بداية البرنامج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 25 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل التسجيل في الدورة يعني أنني حققت شرط اللغة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "التسجيل في الدورة لا يعني تحقيق شرط اللغة. التزمي بمتطلبات الدورة واجتيازها وفق الآلية المعتمدة، ثم زوّدي الجهة المختصة بما يثبت الاجتياز لاستكمال إجراءات القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 26 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ماذا أفعل بعد اجتياز الدورة التأهيلية للغة الإنجليزية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "بعد الاجتياز، احتفظي بما يثبت تحقيق شرط اللغة، ثم ارفعيه أو أرسليه عبر القناة الرسمية المحددة في إعلان القبول أو تعليمات العمادة، حتى تُحدَّث حالة طلبكِ وتُستكمل إجراءات القبول." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 27 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا اختبرت IELTS وحققت الدرجة المطلوبة، هل أحتاج الدورة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "إذا حققتِ الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ وفق شروط البرنامج، فلا تحتاجين غالباً إلى الالتحاق بالدورة التأهيلية؛ والمهمّ رفع ما يثبت تحقيق شرط اللغة عبر القناة الرسمية المحددة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 28 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا لم أجتز الدورة التأهيلية للغة الإنجليزية، ماذا يحدث؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "عدم اجتياز شرط اللغة الإنجليزية خلال المدة المحددة قد يؤثّر على استكمال القبول أو الانتظام في البرنامج؛ فتواصلي مع الجهة المختصة لمعرفة الإجراء النظامي المتاح وفق تعليمات القبول. وقد أعلن معهد اللغة الإنجليزية بجامعة الأميرة نورة بدء التسجيل في الدورة التأهيلية للغة الإنجليزية للمتقدمات إلى برامج الدراسات العليا، وأن البرنامج يبدأ بتاريخ 28 يونيو 2026م." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 29 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل أستطيع إكمال الدكتوراه بعد الماجستير؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يمكن إكمال الدكتوراه بعد الماجستير متى استوفيتِ شروط القبول في البرنامج. ويختلف المسار بحسب نوع الماجستير؛ فإن كان أكاديمياً أمكن التقديم على الدكتوراه الأكاديمية، وإن كان مهنياً فالمسار المناسب بعده الدكتوراه المهنية، ولا يؤهّل عادةً للتقديم على الدكتوراه الأكاديمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 30 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الفرق بين الماجستير الأكاديمي والماجستير المهني؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "الماجستير الأكاديمي يركّز على التعمّق العلمي والبحثي في التخصص، ويهيّئ الطالبة لمسارات البحث العلمي والدكتوراه الأكاديمية. والماجستير المهني يركّز على التطبيق المتقدّم للمعرفة والمهارات في مجالٍ مهنيٍّ محدد، ويهيّئ الطالبة للممارسة المهنية المتقدمة أو الدكتوراه المهنية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 31 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل الماجستير المهني يؤهل للدكتوراه الأكاديمية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "الماجستير المهني ليس المسار المناسب للدكتوراه الأكاديمية، وإنما يكون مساره غالباً نحو الدكتوراه المهنية أو المسارات التطبيقية المتقدمة، وفق شروط القبول المعتمدة لكل برنامج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 32 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل الماجستير الأكاديمي يؤهل للدكتوراه الأكاديمية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "الماجستير الأكاديمي هو المسار المناسب للتقديم على الدكتوراه الأكاديمية، مع استيفاء شروط القبول الأخرى التي يحددها البرنامج، كالتخصص، والمعدل، ومتطلبات اللغة، وأي شروطٍ إضافية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 33 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المقصود بالدكتوراه المهنية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "مؤهّلٌ متقدّمٌ يركّز على توظيف المعرفة والمهارات البحثية والتطبيقية لمعالجة قضايا مهنية وتطوير الممارسة في مجالٍ محدد، ويختلف عن الدكتوراه الأكاديمية التي تركّز غالباً على البحث العلمي وإنتاج المعرفة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 34 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "إذا كنت أرغب في الدكتوراه الأكاديمية، أي مسار ماجستير أختار؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "إذا كان هدفكِ إكمال الدكتوراه الأكاديمية، فالمسار الأنسب الماجستير الأكاديمي؛ لارتباطه بالبحث العلمي والتعمّق النظري والمنهجي في التخصص. والماجستير المهني يناسب غالباً من ترغب في تطوير ممارستها المهنية أو التوجّه إلى الدكتوراه المهنية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 35 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أتقدّم للسكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تتقدّمين عبر خدمة التسجيل الإلكتروني في سكن الطالبات بعمادة شؤون الطالبات، من خلال الرابط الرسمي للخدمة: خدمة التسجيل الإلكتروني في سكن الطالبات ، مع استيفاء الشروط والمستندات المطلوبة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 36 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "هل يحقّ لطالبة الدراسات العليا السكن في السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يتيح السكن الجامعي خدماته لطالبات الجامعة وفق الشروط والفئات المعتمدة، وتُدرَج طالبة الدراسات العليا ضمن الفئات المستفيدة وفق الضوابط المعلنة في صفحة الخدمة الرسمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 37 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما رسوم السكن لطالبة الدراسات العليا؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تُحدَّد رسوم السكن لطالبة الدراسات العليا وفق ما تعلنه عمادة شؤون الطالبات في صفحة الخدمة الرسمية؛ فراجعي الصفحة للاطّلاع على الرسوم المحدّثة وآلية سدادها." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 38 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المستندات المطلوبة للتقديم على السكن؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تُحدَّد المستندات في صفحة خدمة التسجيل الإلكتروني في سكن الطالبات، وتشمل غالباً إثبات القيد والهوية والبيانات التي تطلبها الخدمة؛ فاتّبعي ما هو مبيَّنٌ في الصفحة الرسمية عند التقديم." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 39 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "متى أسجّل في السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "يُفتح التسجيل في السكن وفق المواعيد التي تعلنها عمادة شؤون الطالبات؛ فتابعي صفحة الخدمة الرسمية وقنوات العمادة لمعرفة مواعيد فتح التسجيل والتعليمات المصاحبة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 40 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "بمن أتواصل بشأن السكن الجامعي؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Answer", "تُدار خدمات السكن عبر عمادة شؤون الطالبات، ويمكنكِ التواصل معها ومتابعة مستجدّات الخدمة من خلال صفحة خدمة التسجيل الإلكتروني في سكن الطالبات وقنوات العمادة الرسمية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 41 }
            });

            // Graduates FAQ - 9 questions transcribed from the v4 design
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المتطلّبات اللازمة لإتمام تخرّجي؟" },
                { "Title_EN", "What are the requirements to finalize graduation?" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "تسليم الرسالة العلمية بصورتها النهائية إلى القسم والكلية لرفعها إلى العمادة، وتسليم نسختين ورقيتين من الرسالة لعمادة الدراسات العليا، واستكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه." },
                { "Answer_EN", "Electronic submission of final thesis, delivery of two bound copies to deanship, and completing university clearance." },
                { "Category", "متطلبات التخرج" },
                { "ItemOrder", 1 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف تُرفع التوصية بمنح الدرجة العلمية؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "يرفع القسم تقرير لجنة المناقشة إلى الكلية، ثم تضمّ وحدة الخريجين بالعمادة اسمكِ إلى مذكرة الخريجين، وتُرفع إلى مجلس الجامعة، وتُعتمد من رئيس الجامعة المكلّفة وفق الصلاحيات النظامية." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 2 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أستلم وثيقة التخرّج؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "بعد اعتماد منح الدرجة، تطبعين نموذج إخلاء الطرف وتستكملينه، ثم تراجعين عمادة القبول والتسجيل لاستلام وثيقة التخرّج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 3 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما المقصود بإخلاء الطرف؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "هو استكمال إنهاء التزاماتكِ تجاه وحدات الجامعة عبر النموذج المعتمد، وهو خطوةٌ ختاميةٌ لازمةٌ قبل استلام وثيقة التخرّج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 4 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أتابع تغيّر حالتي إلى متخرّجة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "يمكنكِ متابعة وضعكِ على بوابة النظام الأكاديمي حتى تتأكّدي من تغيّر حالتكِ من منتظمة إلى متخرّجة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 5 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "كيف أسلّم رسالتي العلمية للعمادة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "تُسلَّم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة، مع تسليم نسختين ورقيتين للعمادة." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 6 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما الجهات التي يلزم اعتمادها في نموذج إخلاء الطرف؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "يلزم اعتماد النموذج من: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 7 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "ما دور رئيسة الجامعة المكلّفة في منح الدرجة؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "بعد رفع مذكرة الخريجين إلى مجلس الجامعة، تُعتمد الدرجة العلمية وفق الصلاحيات النظامية المقرّرة، تمهيداً لإصدار وثيقة التخرّج." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 8 }
            });
            list.Add(new Dictionary<string, object> {
                { "Title", "من الجهة التي تُصدر وثيقة التخرّج؟" },
                { "Title_EN", "" },
                { "Phase", DgaPgPhases.Graduates },
                { "Answer", "بعد اعتماد منح الدرجة، يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج، وتراجعينها لاستلامها بعد استكمال إخلاء الطرف." },
                { "Answer_EN", "" },
                { "Category", "" },
                { "ItemOrder", 9 }
            });

            return list;
        }
    }
}
