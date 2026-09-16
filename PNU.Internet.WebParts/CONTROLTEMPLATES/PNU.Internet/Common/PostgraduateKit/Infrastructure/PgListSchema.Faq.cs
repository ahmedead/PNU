using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Globalization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definition and seed data for PgFaq: the frequently asked questions of each audience.
    /// </summary>
    public static partial class PgListSchema
    {
        private static void BuildFaqList(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Faq] = new PgListDef
            {
                Name = PgListNames.Faq,
                TitleAr = "الأسئلة الأكثر شيوعاً", TitleEn = "Frequently asked questions",
                Description = "Question and answer pairs, one set per audience.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(), DescAr(6), DescEn(6), Phase(), Order()
                },
                Seed = FaqSeed()
            };
        }

        private static int _faqOrder;

        private static Dictionary<string, string> Faq(string phase, string qAr, string qEn, string aAr, string aEn)
        {
            return Row("Title", qAr, "Title_EN", qEn,
                       "Description", aAr, "Description_EN", aEn,
                       "Phase", phase,
                       "ItemOrder", (++_faqOrder).ToString(CultureInfo.InvariantCulture));
        }

        private static List<Dictionary<string, string>> FaqSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // ===================== المتقدمون / Applicants =====================
            _faqOrder = 0;

            seed.Add(Faq(PgPhases.Applicants,
                "متى يُفتح باب القبول لبرامج الدراسات العليا؟",
                "When does admission to postgraduate programs open?",
                "يُعلَن عن فتح القبول عبر موقع عمادة الدراسات العليا وقنواتها الرسمية في مواعيد محدّدة لكل عام؛ ويمكن متابعة المواعيد عبر صفحة مواعيد وبوابة القبول.",
                "The opening of admission is announced on the Deanship of Graduate Studies website and its official channels at set times each year; the dates can be followed on the admission dates and portal page."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما شروط القبول؟",
                "What are the admission requirements?",
                "تختلف الشروط بحسب البرنامج والمرحلة، وتشمل شروطاً عامةً (كالمؤهّل الجامعي والتقدير) وأخرى خاصةً بكل برنامج. ولأنها قد تتغيّر، يُرجى الرجوع إلى صفحة شروط القبول الرسمية للاطّلاع على المحدّث منها.",
                "The requirements differ by program and by level, and include general requirements such as the university qualification and the grade, as well as requirements specific to each program. Because they may change, please refer to the official admission requirements page for the current version."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما المستندات المطلوبة للتقديم؟",
                "Which documents are required to apply?",
                "تُحدَّد المستندات وفق شروط كل برنامج، وتشمل غالباً المؤهّل الجامعي والسجل الأكاديمي والتزكيات والهوية؛ وتُرفع إلكترونياً عبر بوابة القبول.",
                "The documents are set by the requirements of each program and usually include the university qualification, the academic transcript, recommendations and an identity document; they are uploaded online through the admission portal."));

            seed.Add(Faq(PgPhases.Applicants,
                "لم أحقّق شرط اللغة الإنجليزية، ما الحلّ؟",
                "I have not met the English language requirement. What can I do?",
                "يمكنكِ الالتحاق بالدورة التأهيلية للغة الإنجليزية بمعهد اللغة الإنجليزية، فهي تُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول.",
                "You can join the English language preparatory course at the English Language Institute; it is treated as equivalent to the score required in the approved language test for admission."));

            seed.Add(Faq(PgPhases.Applicants,
                "كيف تتمّ المفاضلة بين المتقدّمات؟",
                "How are applicants ranked against one another?",
                "تتمّ المفاضلة وفق معايير كل برنامج (كالتقدير الجامعي والاختبارات والمقابلات إن وُجدت)، وتُعلَن النتائج عبر البوابة.",
                "Ranking follows the criteria of each program, such as the university grade, tests and interviews where these exist, and the results are announced through the portal."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل يمكنني التقديم على أكثر من برنامج؟",
                "May I apply to more than one program?",
                "تتيح بوابة القبول التقديم وفق ضوابط كل فصل؛ ويُنصح بمراجعة دليل المستخدم وصفحة البرامج لمعرفة الخيارات المتاحة وآليّة الترشيح.",
                "The admission portal allows applications under the rules of each term; review the user guide and the programs page to see the available options and the nomination mechanism."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل الدراسة في برامج الدراسات العليا بمقابلٍ مادي؟",
                "Do postgraduate programs charge tuition fees?",
                "تتفاوت البرامج بين مجانيةٍ ومدفوعة، وتُوضَّح الرسوم في صفحة كل برنامج؛ لذا يُرجى الرجوع إلى صفحة البرامج والرسوم للاطّلاع على المحدّث.",
                "Programs vary between free and fee-paying, and the fees are stated on each program page, so please refer to the programs and fees page for the current information."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما الدورة التأهيلية للغة الإنجليزية ولمن تُوجّه؟",
                "What is the English language preparatory course and who is it for?",
                "هي دورة يقدّمها معهد اللغة الإنجليزية للحاصلين على قبول مشروط، تؤهّلهم باكتساب المهارات اللغوية اللازمة، وتُعادل الدرجة المطلوبة في اختبار اللغة المعتمد للقبول.",
                "It is a course offered by the English Language Institute to students with conditional admission. It equips them with the language skills they need and is treated as equivalent to the score required in the approved language test for admission."));

            seed.Add(Faq(PgPhases.Applicants,
                "أين أتابع حالة طلب قبولي؟",
                "Where do I follow the status of my admission application?",
                "يمكنك متابعة حالة طلبك عبر بوابة القبول الإلكترونية التي تقدّمت من خلالها، مع أهمية متابعة إشعارات التذكير عبر القنوات الرسمية.",
                "You can follow the status of your application through the electronic admission portal you applied on, and it is important to watch for reminder notifications on the official channels."));

            seed.Add(Faq(PgPhases.Applicants,
                "أين أجد جدولي الدراسي؟",
                "Where do I find my study schedule?",
                "تُحدَّث جداول المقررات في نظام إدارة التعلم الإلكتروني (Blackboard)؛ ويمكنك الدخول إلى صفحتك باسم المستخدم وكلمة المرور الجامعية، وعند عدم ظهور الجدول تواصل مع القسم أو البرنامج الأكاديمي للتحقق من حالة تسجيلك.",
                "Course schedules are updated in the Blackboard learning management system. Sign in to your page with your university username and password, and if the schedule does not appear, contact the department or the academic program to check your registration status."));

            seed.Add(Faq(PgPhases.Applicants,
                "كيف أدخل على البلاكبورد؟",
                "How do I sign in to Blackboard?",
                "تدخل إلى نظام إدارة التعلم الإلكتروني (Blackboard) باستخدام حسابك الجامعي عبر اسم المستخدم وكلمة المرور المعتمدة، وعند وجود مشكلة في الدخول تواصل مع الدعم الفني المختص.",
                "Sign in to the Blackboard learning management system with your university account using the approved username and password. If you have trouble signing in, contact the relevant technical support team."));

            seed.Add(Faq(PgPhases.Applicants,
                "ماذا أفعل إذا لم يظهر جدولي الدراسي؟",
                "What should I do if my study schedule does not appear?",
                "راجع القسم أو البرنامج الأكاديمي للتأكد من حالة تسجيلك والمقررات المطروحة، ثم تابع مع الجهة المختصة بحسب إجراء الفصل.",
                "Check with the department or the academic program to confirm your registration status and the courses on offer, then follow up with the relevant office according to the term's procedure."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما الأنظمة الإلكترونية التي أحتاج استخدامها خلال دراستي؟",
                "Which electronic systems will I need during my studies?",
                "تحتاج إلى متابعة البريد الجامعي، ونظام إدارة التعلم الإلكتروني (Blackboard)، والنظام الأكاديمي، وأي روابط أو أنظمة تحددها الجامعة أو العمادة لاستقبال التسجيل ومتابعة الإجراءات الأكاديمية.",
                "You will need to follow your university email, the Blackboard learning management system, the academic system, and any links or systems the university or the deanship specifies for registration and for following academic procedures."));

            seed.Add(Faq(PgPhases.Applicants,
                "أين أتابع حالة طلب القبول؟",
                "Where do I track the admission application status?",
                "تُتابَع حالة طلب القبول عبر بوابة القبول الإلكترونية أو القناة الرسمية المعتمدة عند التقديم، مع أهمية متابعة البريد الإلكتروني والجوال المسجَّلين في الطلب.",
                "The admission application status is tracked through the electronic admission portal or the official channel announced at the time of application, and it is important to watch the email address and mobile number registered on the application."));

            seed.Add(Faq(PgPhases.Applicants,
                "ماذا تعني أوضاع طلب القبول؟",
                "What do the admission application statuses mean?",
                "توضّح أوضاع طلب القبول المرحلة التي بلغها طلبك، مثل: مكتمل، غير مكتمل، مرشّح للمقابلة، مقبولة مبدئياً، أو غير مقبولة؛ ويُرجى متابعة الحالة عبر بوابة القبول والقنوات الرسمية.",
                "The application statuses show the stage your application has reached, such as complete, incomplete, shortlisted for interview, provisionally accepted, or not accepted. Please follow the status through the admission portal and the official channels."));

            seed.Add(Faq(PgPhases.Applicants,
                "كيف أعرف أني مرشح للمقابلة الشخصية؟",
                "How will I know that I have been shortlisted for an interview?",
                "تُشعَر المرشّحات للمقابلة الشخصية عبر بوابة القبول أو القنوات الرسمية المعتمدة، وقد يتضمّن الإشعار موعد المقابلة وطريقتها والجهة المنظِّمة لها.",
                "Shortlisted applicants are notified through the admission portal or the approved official channels, and the notice may include the date of the interview, its format and the organising body."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل أعرف نتيجة المقابلة الشخصية؟",
                "Will I be told the result of the interview?",
                "تُعلَن نتيجة المقابلة الشخصية وفق الجدول الزمني المعتمد للقبول، وتظهر عادةً عبر بوابة القبول أو القنوات الرسمية المعتمدة.",
                "The interview result is announced according to the approved admission timetable and normally appears through the admission portal or the approved official channels."));

            seed.Add(Faq(PgPhases.Applicants,
                "إذا فاتني المقابلة الشخصية، ماذا أفعل؟",
                "What if I miss the interview?",
                "يُرجى التواصل فوراً مع الجهة الموضَّحة في إعلان المقابلات أو مع البرنامج الأكاديمي المختص لإيضاح سبب عدم الحضور؛ وإعادة الجدولة ليست مضمونة وتخضع لتقدير الجهة المختصة.",
                "Contact the body named in the interview announcement, or the relevant academic program, immediately to explain why you were absent. Rescheduling is not guaranteed and is at the discretion of the relevant body."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل يمكن إعادة جدولة المقابلة الشخصية؟",
                "Can the interview be rescheduled?",
                "يُنظر في إعادة جدولة المقابلة إذا وُجد مبرر مقبول وسمحت المدة الزمنية بذلك، وهذا يخضع لتقدير الجهة الأكاديمية المختصة.",
                "Rescheduling is considered where there is an acceptable reason and the timetable allows it; this is at the discretion of the relevant academic body."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل أستطيع القبول إذا لم أحقق شرط اللغة الإنجليزية؟",
                "Can I be admitted if I have not met the English language requirement?",
                "لا يُستكمل القبول دون تحقيق شرط اللغة الإنجليزية المعتمد للبرنامج، ويكون تحقيقه إما بالحصول على الدرجة المطلوبة في اختبار لغة معتمد مثل IELTS أو TOEFL، أو باجتياز الدورة التأهيلية للغة الإنجليزية.",
                "Admission cannot be completed without meeting the program's approved English language requirement, which is met either by achieving the required score in an approved language test such as IELTS or TOEFL, or by passing the English language preparatory course."));

            seed.Add(Faq(PgPhases.Applicants,
                "ماذا أفعل إذا لم أحقق شرط اللغة الإنجليزية؟",
                "What should I do if I have not met the English language requirement?",
                "تختار أحد المسارين: اجتياز اختبار لغة إنجليزية معتمد وتحقيق الدرجة المطلوبة، أو التسجيل في الدورة التأهيلية للغة الإنجليزية واجتيازها وفق الضوابط المعتمدة.",
                "Choose one of two routes: sit an approved English language test and achieve the required score, or enrol in the English language preparatory course and pass it under the approved rules."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا؟",
                "What is the English language preparatory course for postgraduate students?",
                "يقدّم معهد اللغة الإنجليزية للملتحقات ببرامج الدراسات العليا بالجامعة برنامجاً لغوياً يؤهّلهنّ لتحقيق متطلبات القبول في برامج الدراسات العليا.",
                "The English Language Institute offers students joining the university's postgraduate programs a language program that prepares them to meet the admission requirements of those programs."));

            seed.Add(Faq(PgPhases.Applicants,
                "كيف أسجل في الدورة التأهيلية للغة الإنجليزية؟",
                "How do I register for the English language preparatory course?",
                "تسجَّلين في الدورة عبر رابط التسجيل المعلن من معهد اللغة الإنجليزية، مع الاطّلاع على الإعلان الرسمي لمعرفة آخر موعد للتسجيل ومكان الدورة وتاريخ بدايتها.",
                "You register through the registration link announced by the English Language Institute; check the official announcement for the registration deadline, the location of the course and its start date."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل التسجيل في الدورة يعني أني حققت شرط اللغة؟",
                "Does registering for the course mean I have met the language requirement?",
                "التسجيل في الدورة لا يعني تحقيق شرط اللغة، وإنما يعني استيفاء متطلبات الدورة واجتيازها وفق الآلية المحددة.",
                "Registering for the course does not by itself meet the language requirement; the requirement is met by fulfilling the course requirements and passing it under the specified mechanism."));

            seed.Add(Faq(PgPhases.Applicants,
                "ماذا أفعل بعد اجتياز الدورة التأهيلية للغة الإنجليزية؟",
                "What should I do after passing the English language preparatory course?",
                "بعد الاجتياز، يُحتسب ما يثبت تحقيق شرط اللغة لديك عبر الآلية الرسمية المعتمدة، ويظهر ذلك ضمن إعلان القبول أو تحديثات حالة الطلب.",
                "Once you pass, the evidence that you have met the language requirement is recorded through the approved official mechanism and appears in the admission announcement or in the updates to your application status."));

            seed.Add(Faq(PgPhases.Applicants,
                "إذا حققت الدرجة المطلوبة في اختبار لغة معتمد، هل يلزمني الالتحاق بالدورة؟",
                "If I achieved the required score in an approved language test, must I still take the course?",
                "إذا حقّقتِ الدرجة المطلوبة في اختبار لغة إنجليزية معتمد وفق ضوابط البرنامج، فلا حاجة للالتحاق بالدورة التأهيلية، ويكفي رفع ما يثبت تحقيق شرط اللغة عبر الجهة المعتمدة.",
                "If you achieved the required score in an approved English language test under the program's rules, you do not need to take the preparatory course; it is enough to submit the evidence that you have met the requirement through the approved channel."));

            seed.Add(Faq(PgPhases.Applicants,
                "إذا لم أجتز الدورة التأهيلية للغة الإنجليزية، ماذا يحدث؟",
                "What happens if I do not pass the English language preparatory course?",
                "عدم اجتياز شرط اللغة الإنجليزية خلال المدة المحددة قد يؤثر على استكمال القبول أو الانتظام في البرنامج؛ ويُرجى التواصل مع الجهة المختصة لمعرفة إجراء استكمال القبول وفق التعليمات المعتمدة.",
                "Not meeting the English language requirement within the set period may affect the completion of your admission or your enrolment in the program; contact the relevant office to find out the procedure for completing admission under the approved instructions."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل أستطيع إكمال الدكتوراه بعد الماجستير؟",
                "Can I go on to a doctorate after the master's degree?",
                "يمكن إكمال الدكتوراه بعد الماجستير مع استيفاء شروط القبول في البرنامج، ويختلف المسار بحسب نوع الماجستير: فإن كان أكاديمياً يُتيح التقديم على الدكتوراه الأكاديمية مباشرة، وإن كان مهنياً فالمسار المناسب غالباً الدكتوراه المهنية.",
                "You may go on to a doctorate after the master's degree provided you meet the program's admission requirements. The route depends on the type of master's: an academic master's allows a direct application to an academic doctorate, while a professional master's usually leads to a professional doctorate."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما الفرق بين الماجستير الأكاديمي والماجستير المهني؟",
                "What is the difference between the academic and the professional master's?",
                "الماجستير الأكاديمي يركّز على التعمّق العلمي والبحث في التخصص، ويُتيح المتابعة نحو الدكتوراه الأكاديمية، أمّا الماجستير المهني فيركّز على التطبيق العملي للمعارف والمهارات في مجال مهني محدد، ويُتيح دخول سوق العمل أو الدكتوراه المهنية المناسبة.",
                "The academic master's focuses on scholarly depth and research within the discipline and allows progression to an academic doctorate. The professional master's focuses on the practical application of knowledge and skills in a defined professional field and leads to the labour market or to a suitable professional doctorate."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل الماجستير المهني يؤهل للدكتوراه الأكاديمية؟",
                "Does a professional master's qualify me for an academic doctorate?",
                "الماجستير المهني ليس المسار المباشر للدكتوراه الأكاديمية، وإنما يكون مساره غالباً نحو الدكتوراه المهنية أو المسارات المعتمدة الأخرى وفق شروط القبول لكل برنامج.",
                "A professional master's is not the direct route to an academic doctorate; it usually leads to a professional doctorate or to other approved routes, subject to the admission requirements of each program."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل الماجستير الأكاديمي يؤهل للدكتوراه الأكاديمية؟",
                "Does an academic master's qualify me for an academic doctorate?",
                "الماجستير الأكاديمي هو المسار المناسب للتقديم على الدكتوراه الأكاديمية، مع استيفاء شروط القبول الأخرى التي يحددها البرنامج، كالتخصص والمعدل ومتطلبات اللغة.",
                "The academic master's is the appropriate route for applying to an academic doctorate, provided you meet the other admission requirements set by the program, such as the discipline, the grade point average and the language requirements."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما المقصود بالدكتوراه المهنية؟",
                "What is meant by a professional doctorate?",
                "مؤهّل متقدّم يركّز على توظيف المعرفة والمهارات العلمية والتطبيقية لمعالجة قضايا مهنية وتطوير الممارسة في مجال محدد، ويختلف عن الدكتوراه الأكاديمية التي تركّز غالباً على البحث العلمي وإنتاج المعرفة.",
                "It is an advanced qualification focused on applying scholarly and practical knowledge and skills to professional problems and to developing practice in a defined field. It differs from the academic doctorate, which focuses mainly on research and the production of knowledge."));

            seed.Add(Faq(PgPhases.Applicants,
                "إذا كنت أرغب في الدكتوراه الأكاديمية، أي مسار ماجستير أختار؟",
                "If I want an academic doctorate, which master's route should I choose?",
                "إذا كان هدفك إكمال الدكتوراه الأكاديمية، فالمسار الأنسب هو الماجستير الأكاديمي لارتباطه بالبحث العلمي والتعمّق النظري في التخصص، بينما يناسب الماجستير المهني غالباً من يرغب في تطوير ممارسته المهنية أو التوجّه إلى الدكتوراه المهنية.",
                "If your goal is an academic doctorate, the academic master's is the more suitable route because of its link to research and theoretical depth in the discipline, while the professional master's usually suits those who want to develop their professional practice or move towards a professional doctorate."));

            seed.Add(Faq(PgPhases.Applicants,
                "كيف أنضم للسكن الجامعي؟",
                "How do I apply for student housing?",
                "تُستخدَم خدمة التسجيل الإلكتروني في سكن الطالبات بعمادة شؤون الطالبات من خلال الرابط الرسمي للخدمة، ويُسجَّل الطلب إلكترونياً مع استيفاء الشروط والمستندات المطلوبة.",
                "Use the electronic student housing registration service of the Deanship of Student Affairs through the official service link, and submit the request online while meeting the required conditions and documents."));

            seed.Add(Faq(PgPhases.Applicants,
                "هل يحق لطلبة الدراسات العليا السكن في السكن الجامعي؟",
                "Are postgraduate students entitled to student housing?",
                "يُتاح السكن الجامعي لطالبات الجامعة وفق الشروط والضوابط المعتمدة، وتُدرَج طالبة الدراسات العليا ضمن الفئات المستفيدة وفق الشروط المعلنة في صفحة الخدمة الرسمية.",
                "Student housing is available to the university's students under the approved conditions and rules, and postgraduate students are among the eligible categories under the conditions published on the official service page."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما رسوم السكن لطلبة الدراسات العليا؟",
                "What are the housing fees for postgraduate students?",
                "تُعتمد رسوم السكن لطالبة الدراسات العليا وفق ما تعلنه عمادة شؤون الطالبات في صفحة الخدمة الرسمية؛ فيُرجى الاطّلاع على الرسوم المحدَّثة وآلية سدادها.",
                "Housing fees for a postgraduate student are set as announced by the Deanship of Student Affairs on the official service page; please check the current fees and how they are paid."));

            seed.Add(Faq(PgPhases.Applicants,
                "ما المستندات المطلوبة للتقديم على السكن؟",
                "Which documents are required to apply for housing?",
                "تُعلَن المستندات المطلوبة في صفحة خدمة التسجيل الإلكتروني لسكن الطالبات، وتشمل غالباً إثبات القيد والهوية الوطنية والصورة الشخصية وأي متطلبات إضافية تحددها العمادة.",
                "The required documents are published on the electronic student housing registration service page and usually include proof of registration, the national identity document, a personal photograph and any additional requirements set by the deanship."));

            seed.Add(Faq(PgPhases.Applicants,
                "متى أُسجّل في السكن الجامعي؟",
                "When do I register for student housing?",
                "يُفتح التسجيل في السكن وفق المواعيد التي تعلنها عمادة شؤون الطالبات في صفحة الخدمة الرسمية وقنوات العمادة؛ فيُرجى متابعة مواعيد فتح التسجيل والتعليمات المصاحبة.",
                "Housing registration opens on the dates announced by the Deanship of Student Affairs on the official service page and the deanship's channels; please follow the registration opening dates and the accompanying instructions."));

            // ===================== المقيدون / Enrolled =====================
            _faqOrder = 0;

            seed.Add(Faq(PgPhases.Enrolled,
                "كم مرّةً يمكنني تأجيل القبول؟ ومتى؟",
                "How many times may I defer admission, and when?",
                "تأجيل القبول يكون لمرّةٍ واحدةٍ فقط، بعد صدور الرقم الجامعي وقبل تسجيل أي مقرر، ولا يُقبل بعد بدء الدراسة، بموافقة مجلس القسم وعميد الكلية (المادة العشرون).",
                "Admission may be deferred once only, after the university number is issued and before any course is registered. It is not accepted after study has begun, and it requires the approval of the department council and the dean of the college (Article 20)."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الفرق بين تأجيل القبول وتأجيل الدراسة؟",
                "What is the difference between deferring admission and deferring study?",
                "تأجيل القبول يكون قبل بدء الدراسة وقبل تسجيل أي مقرر (المادة العشرون)؛ أمّا تأجيل الدراسة فيكون بعد بدء الدراسة بإيقاف سريان المدة النظامية (المادة الثانية والعشرون)، بموافقة مجلس الكلية.",
                "Deferring admission takes place before study begins and before any course is registered (Article 20). Deferring study takes place after study has begun and suspends the statutory period (Article 22), with the approval of the college council."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل تُحتسب مدة التأجيل ضمن المدة النظامية للحصول على الدرجة؟",
                "Does the deferral period count towards the statutory period for obtaining the degree?",
                "لا تُحتسب مدة تأجيل الدراسة ضمن المدة المقررة للحصول على الدرجة، على ألا يتجاوز مجموع التأجيل نصف المدة المقررة للبرنامج.",
                "The study deferral period does not count towards the period set for obtaining the degree, provided the total deferral does not exceed half the period set for the program."));

            seed.Add(Faq(PgPhases.Enrolled,
                "متى يُلغى قيد الطالب/ـة؟",
                "When is a student's registration cancelled?",
                "يُلغى القيد عند انطباق إحدى الحالات النظامية، ومنها: الانقطاع عن الدراسة، أو انخفاض المعدل التراكمي عن تقدير «جيد جداً» في فصلين متتاليين، أو عدم اجتياز المقررات التكميلية، أو تجاوز المدة المحددة للبرنامج، أو الإخلال بالأمانة العلمية (المادة السادسة والعشرون).",
                "Registration is cancelled when one of the statutory cases applies, including: interruption of study, a grade point average below \"very good\" for two consecutive terms, failing the supplementary courses, exceeding the period set for the program, or a breach of academic integrity (Article 26)."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما شروط التقدّم بتسجيل خطة البحث للرسالة؟",
                "What are the requirements for registering the thesis research plan?",
                "يُشترط إنجاز ما لا يقل عن 50% من المقررات الدراسية، وألا يقلّ المعدل التراكمي عن تقدير «جيد جداً»، وتتقدّم المرشدة الأكاديمية بالطلب للقسم المختص (المادة السادسة والثلاثون).",
                "At least 50% of the coursework must be completed, the grade point average must not be below \"very good\", and the academic advisor submits the request to the relevant department (Article 36)."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الحدّ الأدنى للمعدل التراكمي للتخرّج ومنح الدرجة؟",
                "What is the minimum grade point average for graduation and the award of the degree?",
                "ألا يقلّ المعدل التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، وفي كل الأحوال لا يقلّ عن تقدير «جيد جداً» (المادة الرابعة والخمسون).",
                "The grade point average must not be below the average set by the university council for each program, and in all cases not below \"very good\" (Article 54)."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الفرق بين الفرصة الاستثنائية لرفع المعدل والفرصة للحصول على الدرجة؟",
                "What is the difference between the exceptional opportunity to raise the average and the one to obtain the degree?",
                "الأولى (المادة الثامنة والعشرون) تُمنَح بقرار مجلس الكلية لمعالجة انخفاض المعدل؛ والثانية (المادة التاسعة والعشرون) تُمنَح بقرار اللجنة الدائمة بناءً على تقرير المشرف عند انتهاء المدة دون الحصول على الدرجة، وكلتاهما لا تزيد على عامٍ دراسي.",
                "The first (Article 28) is granted by a decision of the college council to remedy a fall in the grade point average. The second (Article 29) is granted by a decision of the standing committee on the basis of the supervisor's report when the period ends without the degree being obtained. Neither exceeds one academic year."));

            seed.Add(Faq(PgPhases.Enrolled,
                "متى أحتاج إخلاء الطرف؟",
                "When do I need the clearance form?",
                "يُستكمل إخلاء الطرف عند الانسحاب أو التخرج أو إلغاء القيد، بوصفه خطوةً ختاميةً لبراءة الذمّة تجاه وحدات الجامعة.",
                "Clearance is completed on withdrawal, graduation or cancellation of registration, as the closing step in settling your obligations towards the university's units."));

            seed.Add(Faq(PgPhases.Enrolled,
                "كيف أعرف الإجراء الأكاديمي المناسب لحالتي؟",
                "How do I find the academic procedure that fits my situation?",
                "يعرض محور «الإجراءات الأكاديمية التفاعلية» الإجراءات كاملةً؛ وبالضغط على أيّ إجراءٍ تظهر شروطه ونصّ مادته ونماذجه المعتمدة ومساره، لتختاري ما يناسب حالتكِ.",
                "The interactive academic procedures axis lists every procedure. Selecting a procedure shows its requirements, the text of its article, its approved forms and its path, so you can choose the one that fits your situation."));

            seed.Add(Faq(PgPhases.Enrolled,
                "أين أجد النماذج المعتمدة للإجراءات؟",
                "Where do I find the approved forms for the procedures?",
                "ترتبط نماذج كل إجراءٍ مباشرةً بموقع العمادة داخل بطاقة الإجراء نفسه، وتُحدَّث تلقائياً بتحديث المصدر الرسمي.",
                "The forms for each procedure are linked directly to the deanship website from within the procedure card itself, and they update automatically when the official source is updated."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما المقصود بحافز النشر العلمي؟",
                "What is the research publication incentive?",
                "هو حافزٌ يشجّع طالبات الدراسات العليا على النشر العلمي المحكّم؛ وتجدين تفاصيله وضوابطه في المحور المخصّص له ضمن هذه المحطّة.",
                "It is an incentive encouraging postgraduate students to publish peer-reviewed research; its details and rules are in the axis dedicated to it on this page."));

            seed.Add(Faq(PgPhases.Enrolled,
                "أين أجد جدولي الدراسي؟",
                "Where do I find my study schedule?",
                "تجدين جدولكِ الدراسي في نظام إدارة التعلم الإلكتروني «Blackboard» بعد الدخول إلى صفحتكِ باسم المستخدم وكلمة المرور الجامعية. وعند عدم ظهور الجدول أو وجود تعارض، تواصلي مع القسم أو البرنامج الأكاديمي للتحقق من حالة التسجيل.",
                "You will find your study schedule in the Blackboard learning management system after signing in to your page with your university username and password. If the schedule does not appear or there is a clash, contact the department or the academic program to check your registration status."));

            seed.Add(Faq(PgPhases.Enrolled,
                "كيف أدخل على البلاك بورد؟",
                "How do I sign in to Blackboard?",
                "تدخلين إلى نظام إدارة التعلم الإلكتروني «Blackboard» باستخدام حسابكِ الجامعي، عبر اسم المستخدم وكلمة المرور المعتمدة للأنظمة الجامعية. وعند وجود مشكلةٍ في الدخول أو كلمة المرور، تواصلي مع الدعم الفني المختص.",
                "Sign in to the Blackboard learning management system with your university account, using the username and password approved for the university systems. If you have a problem signing in or with your password, contact the relevant technical support team."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ماذا أفعل إذا لم يظهر جدولي الدراسي؟",
                "What should I do if my study schedule does not appear?",
                "راجعي القسم أو البرنامج الأكاديمي أولاً للتأكد من حالة تسجيلكِ والمقررات المطروحة، ثم تابعي مع الجهة المختصة بحسب الإجراء المعتمد.",
                "Check first with the department or the academic program to confirm your registration status and the courses on offer, then follow up with the relevant office under the approved procedure."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الأنظمة الإلكترونية التي أحتاج استخدامها خلال دراستي؟",
                "Which electronic systems will I need during my studies?",
                "تحتاجين إلى متابعة البريد الجامعي، ونظام إدارة التعلم الإلكتروني «Blackboard»، والنظام الأكاديمي، وأي روابط أو أنظمةٍ تعلنها الجامعة أو العمادة لاستكمال التسجيل ومتابعة الإجراءات الأكاديمية. وتُدرِج جامعة الأميرة نورة نظام إدارة التعلم الإلكتروني ضمن أنظمتها بوصفه منصة تعلّمٍ إلكترونيٍّ متكاملة، كما تُدرِج النظام الأكاديمي للطالبات لإدارة عملياتهنّ الأكاديمية.",
                "You will need to follow your university email, the Blackboard learning management system, the academic system, and any links or systems the university or the deanship announces for completing registration and following academic procedures. Princess Nourah University lists the learning management system among its systems as an integrated e-learning platform, and likewise the students' academic system for managing their academic transactions."));

            seed.Add(Faq(PgPhases.Enrolled,
                "أين أتابع حالة طلب القبول؟",
                "Where do I track the admission application status?",
                "تتابعين حالة طلب القبول عبر بوابة القبول الإلكترونية أو القناة الرسمية المعلنة عند التقديم، مع أهمية متابعة البريد الإلكتروني والجوال المسجّلين في الطلب؛ فهما من قنوات إشعار المتقدمة بأي تحديثاتٍ أو مواعيدَ مهمة.",
                "You track the admission application status through the electronic admission portal or the official channel announced at the time of application, and it is important to watch the email address and mobile number registered on the application, since these are the channels used to notify applicants of updates and important dates."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ماذا تعني أوضاع طلب القبول؟",
                "What do the admission application statuses mean?",
                "توضّح أوضاع طلب القبول المرحلة التي بلغها طلبكِ، مثل: مكتمل، غير مكتمل، مرشحة للمقابلة، مقبولة مبدئياً، مقبولة قبولاً مشروطاً، مقبولة نهائياً، أو غير مقبولة. وتُتابَع الحالة باستمرارٍ عبر بوابة القبول والقنوات الرسمية المعلنة.",
                "The application statuses show the stage your application has reached, such as complete, incomplete, shortlisted for interview, provisionally accepted, conditionally accepted, finally accepted, or not accepted. The status should be followed continuously through the admission portal and the announced official channels."));

            seed.Add(Faq(PgPhases.Enrolled,
                "كيف أعرف أنني مرشحة للمقابلة الشخصية؟",
                "How will I know that I have been shortlisted for an interview?",
                "تُشعَر المرشحات للمقابلة الشخصية عبر بوابة القبول أو القنوات الرسمية المعلنة، وقد يتضمّن الإشعار موعد المقابلة، وطريقتها، والجهة المنفّذة لها، وأي تعليماتٍ يجب الالتزام بها.",
                "Shortlisted applicants are notified through the admission portal or the announced official channels, and the notice may include the date of the interview, its format, the body conducting it and any instructions to be followed."));

            seed.Add(Faq(PgPhases.Enrolled,
                "متى أعرف نتيجة المقابلة الشخصية؟",
                "When will I know the result of the interview?",
                "تُعلَن نتيجة المقابلة الشخصية وفق الجدول الزمني المعتمد للقبول، وتظهر عادةً عبر بوابة القبول أو القنوات الرسمية المعلنة؛ فاحرصي على متابعة حالة طلبكِ والبريد الإلكتروني والجوال المسجّلين فيه.",
                "The interview result is announced according to the approved admission timetable and normally appears through the admission portal or the announced official channels, so follow your application status and the email address and mobile number registered on it."));

            seed.Add(Faq(PgPhases.Enrolled,
                "إذا فاتتني المقابلة الشخصية، ماذا أفعل؟",
                "What if I miss the interview?",
                "بادري فوراً بالتواصل مع الجهة الموضّحة في إعلان المقابلات أو مع البرنامج الأكاديمي المختص، مع إرفاق بياناتكِ وسبب عدم الحضور. وإعادة جدولة المقابلة ليست مضمونة، وتخضع لتقدير الجهة المختصة والمدة الزمنية المتاحة.",
                "Contact the body named in the interview announcement, or the relevant academic program, immediately, giving your details and the reason for your absence. Rescheduling is not guaranteed and depends on the discretion of the relevant body and on the time available."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل يمكن إعادة جدولة المقابلة الشخصية؟",
                "Can the interview be rescheduled?",
                "يُنظَر في إعادة جدولة المقابلة إذا وُجد مبرّرٌ مقبولٌ وسمحت المدة الزمنية والإجراءات بذلك، غير أن الموافقة ليست تلقائية، وتخضع لما تقرّره الجهة الأكاديمية المختصة.",
                "Rescheduling is considered where there is an acceptable reason and the timetable and procedures allow it, but approval is not automatic and rests with the relevant academic body."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل أستطيع القبول إذا لم أحقق شرط اللغة الإنجليزية؟",
                "Can I be admitted if I have not met the English language requirement?",
                "لا يُستكمل القبول دون تحقيق شرط اللغة الإنجليزية المعتمد للبرنامج. ويكون تحقيقه إمّا بالحصول على الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ مثل IELTS أو TOEFL، أو باجتياز الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا متى كانت متاحةً ومعتمدةً ضمن شروط القبول.",
                "Admission cannot be completed without meeting the program's approved English language requirement. It is met either by achieving the required score in an approved English language test such as IELTS or TOEFL, or by passing the English language preparatory course for postgraduate students where it is available and approved within the admission requirements."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ماذا أفعل إذا لم أحقق شرط اللغة الإنجليزية؟",
                "What should I do if I have not met the English language requirement?",
                "اختاري أحد المسارين: التقدّم لاختبار لغةٍ إنجليزيةٍ معتمدٍ وتحقيق الدرجة المطلوبة، أو التسجيل في الدورة التأهيلية للغة الإنجليزية واجتيازها وفق الضوابط المعلنة. ولا يُعدّ الطلب مستوفياً لشرط اللغة إلا بعد تحقيق الدرجة المطلوبة أو اجتياز الدورة.",
                "Choose one of two routes: sit an approved English language test and achieve the required score, or enrol in the English language preparatory course and pass it under the announced rules. The application is not treated as meeting the language requirement until the required score is achieved or the course is passed."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الدورة التأهيلية للغة الإنجليزية لطلبة الدراسات العليا؟",
                "What is the English language preparatory course for postgraduate students?",
                "برنامجٌ مخصّصٌ للمقبلات على برامج الدراسات العليا بجامعة الأميرة نورة بنت عبد الرحمن، يهدف إلى تأهيلهنّ لغوياً وأكاديمياً لاستيفاء متطلبات القبول في برامج الدراسات العليا.",
                "It is a program for students about to join the postgraduate programs of Princess Nourah bint Abdulrahman University, designed to prepare them linguistically and academically to meet the admission requirements of those programs."));

            seed.Add(Faq(PgPhases.Enrolled,
                "كيف أسجّل في الدورة التأهيلية للغة الإنجليزية؟",
                "How do I register for the English language preparatory course?",
                "تسجّلين في الدورة عبر رابط التسجيل المعلن من معهد اللغة الإنجليزية. وتابعي الإعلان الرسمي لمعرفة آخر موعدٍ للتسجيل، ومقر الدورة، وتاريخ بداية البرنامج.",
                "You register through the registration link announced by the English Language Institute. Follow the official announcement for the registration deadline, the venue of the course and the start date of the program."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل التسجيل في الدورة يعني أنني حققت شرط اللغة؟",
                "Does registering for the course mean I have met the language requirement?",
                "التسجيل في الدورة لا يعني تحقيق شرط اللغة. التزمي بمتطلبات الدورة واجتيازها وفق الآلية المعتمدة، ثم زوّدي الجهة المختصة بما يثبت الاجتياز لاستكمال إجراءات القبول.",
                "Registering for the course does not mean the language requirement has been met. Meet the course requirements and pass it under the approved mechanism, then provide the relevant office with the evidence of passing so that the admission procedures can be completed."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ماذا أفعل بعد اجتياز الدورة التأهيلية للغة الإنجليزية؟",
                "What should I do after passing the English language preparatory course?",
                "بعد الاجتياز، احتفظي بما يثبت تحقيق شرط اللغة، ثم ارفعيه أو أرسليه عبر القناة الرسمية المحددة في إعلان القبول أو تعليمات العمادة، حتى تُحدَّث حالة طلبكِ وتُستكمل إجراءات القبول.",
                "Once you pass, keep the evidence that you have met the language requirement, then upload or send it through the official channel specified in the admission announcement or the deanship's instructions, so that your application status is updated and admission is completed."));

            seed.Add(Faq(PgPhases.Enrolled,
                "إذا اختبرت IELTS وحققت الدرجة المطلوبة، هل أحتاج الدورة؟",
                "If I sat IELTS and achieved the required score, do I still need the course?",
                "إذا حققتِ الدرجة المطلوبة في اختبار لغةٍ إنجليزيةٍ معتمدٍ وفق شروط البرنامج، فلا تحتاجين غالباً إلى الالتحاق بالدورة التأهيلية؛ والمهمّ رفع ما يثبت تحقيق شرط اللغة عبر القناة الرسمية المحددة.",
                "If you achieved the required score in an approved English language test under the program's requirements, you will generally not need to take the preparatory course; what matters is submitting the evidence that you have met the requirement through the specified official channel."));

            seed.Add(Faq(PgPhases.Enrolled,
                "إذا لم أجتز الدورة التأهيلية للغة الإنجليزية، ماذا يحدث؟",
                "What happens if I do not pass the English language preparatory course?",
                "عدم اجتياز شرط اللغة الإنجليزية خلال المدة المحددة قد يؤثّر على استكمال القبول أو الانتظام في البرنامج؛ فتواصلي مع الجهة المختصة لمعرفة الإجراء النظامي المتاح وفق تعليمات القبول.",
                "Not meeting the English language requirement within the set period may affect the completion of your admission or your enrolment in the program, so contact the relevant office to find out the statutory options available under the admission instructions."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل أستطيع إكمال الدكتوراه بعد الماجستير؟",
                "Can I go on to a doctorate after the master's degree?",
                "يمكن إكمال الدكتوراه بعد الماجستير متى استوفيتِ شروط القبول في البرنامج. ويختلف المسار بحسب نوع الماجستير؛ فإن كان أكاديمياً أمكن التقديم على الدكتوراه الأكاديمية، وإن كان مهنياً فالمسار المناسب بعده الدكتوراه المهنية، ولا يؤهّل عادةً للتقديم على الدكتوراه الأكاديمية.",
                "You may go on to a doctorate after the master's degree once you meet the program's admission requirements. The route depends on the type of master's: an academic master's allows an application to an academic doctorate, while a professional master's leads to a professional doctorate and does not usually qualify you to apply for an academic doctorate."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما الفرق بين الماجستير الأكاديمي والماجستير المهني؟",
                "What is the difference between the academic and the professional master's?",
                "الماجستير الأكاديمي يركّز على التعمّق العلمي والبحثي في التخصص، ويهيّئ الطالبة لمسارات البحث العلمي والدكتوراه الأكاديمية. والماجستير المهني يركّز على التطبيق المتقدّم للمعرفة والمهارات في مجالٍ مهنيٍّ محدد، ويهيّئ الطالبة للممارسة المهنية المتقدمة أو الدكتوراه المهنية.",
                "The academic master's focuses on scholarly and research depth in the discipline and prepares the student for research paths and an academic doctorate. The professional master's focuses on the advanced application of knowledge and skills in a defined professional field and prepares the student for advanced professional practice or a professional doctorate."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل الماجستير المهني يؤهل للدكتوراه الأكاديمية؟",
                "Does a professional master's qualify me for an academic doctorate?",
                "الماجستير المهني ليس المسار المناسب للدكتوراه الأكاديمية، وإنما يكون مساره غالباً نحو الدكتوراه المهنية أو المسارات التطبيقية المتقدمة، وفق شروط القبول المعتمدة لكل برنامج.",
                "A professional master's is not the appropriate route to an academic doctorate; it usually leads to a professional doctorate or to advanced applied routes, subject to the approved admission requirements of each program."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل الماجستير الأكاديمي يؤهل للدكتوراه الأكاديمية؟",
                "Does an academic master's qualify me for an academic doctorate?",
                "الماجستير الأكاديمي هو المسار المناسب للتقديم على الدكتوراه الأكاديمية، مع استيفاء شروط القبول الأخرى التي يحددها البرنامج، كالتخصص، والمعدل، ومتطلبات اللغة، وأي شروطٍ إضافية.",
                "The academic master's is the appropriate route for applying to an academic doctorate, provided you meet the other admission requirements set by the program, such as the discipline, the grade point average, the language requirements and any additional conditions."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما المقصود بالدكتوراه المهنية؟",
                "What is meant by a professional doctorate?",
                "مؤهّلٌ متقدّمٌ يركّز على توظيف المعرفة والمهارات البحثية والتطبيقية لمعالجة قضايا مهنية وتطوير الممارسة في مجالٍ محدد، ويختلف عن الدكتوراه الأكاديمية التي تركّز غالباً على البحث العلمي وإنتاج المعرفة.",
                "It is an advanced qualification focused on applying research and practical knowledge and skills to professional problems and to developing practice in a defined field. It differs from the academic doctorate, which focuses mainly on research and the production of knowledge."));

            seed.Add(Faq(PgPhases.Enrolled,
                "إذا كنت أرغب في الدكتوراه الأكاديمية، أي مسار ماجستير أختار؟",
                "If I want an academic doctorate, which master's route should I choose?",
                "إذا كان هدفكِ إكمال الدكتوراه الأكاديمية، فالمسار الأنسب الماجستير الأكاديمي؛ لارتباطه بالبحث العلمي والتعمّق النظري والمنهجي في التخصص. والماجستير المهني يناسب غالباً من ترغب في تطوير ممارستها المهنية أو التوجّه إلى الدكتوراه المهنية.",
                "If your goal is an academic doctorate, the academic master's is the more suitable route because of its link to research and to theoretical and methodological depth in the discipline. The professional master's usually suits those who want to develop their professional practice or move towards a professional doctorate."));

            seed.Add(Faq(PgPhases.Enrolled,
                "كيف أتقدّم للسكن الجامعي؟",
                "How do I apply for student housing?",
                "تتقدّمين عبر خدمة التسجيل الإلكتروني في سكن الطالبات بعمادة شؤون الطالبات، من خلال الرابط الرسمي للخدمة، مع استيفاء الشروط والمستندات المطلوبة.",
                "You apply through the electronic student housing registration service of the Deanship of Student Affairs, using the official service link, while meeting the required conditions and documents."));

            seed.Add(Faq(PgPhases.Enrolled,
                "هل يحقّ لطالبة الدراسات العليا السكن في السكن الجامعي؟",
                "Is a postgraduate student entitled to live in student housing?",
                "يتيح السكن الجامعي خدماته لطالبات الجامعة وفق الشروط والفئات المعتمدة، وتُدرَج طالبة الدراسات العليا ضمن الفئات المستفيدة وفق الضوابط المعلنة في صفحة الخدمة الرسمية.",
                "Student housing offers its services to the university's students under the approved conditions and categories, and a postgraduate student is among the eligible categories under the rules published on the official service page."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما رسوم السكن لطالبة الدراسات العليا؟",
                "What are the housing fees for a postgraduate student?",
                "تُحدَّد رسوم السكن لطالبة الدراسات العليا وفق ما تعلنه عمادة شؤون الطالبات في صفحة الخدمة الرسمية؛ فراجعي الصفحة للاطّلاع على الرسوم المحدّثة وآلية سدادها.",
                "Housing fees for a postgraduate student are set as announced by the Deanship of Student Affairs on the official service page; check the page for the current fees and how they are paid."));

            seed.Add(Faq(PgPhases.Enrolled,
                "ما المستندات المطلوبة للتقديم على السكن؟",
                "Which documents are required to apply for housing?",
                "تُحدَّد المستندات في صفحة خدمة التسجيل الإلكتروني في سكن الطالبات، وتشمل غالباً إثبات القيد والهوية والبيانات التي تطلبها الخدمة؛ فاتّبعي ما هو مبيَّنٌ في الصفحة الرسمية عند التقديم.",
                "The documents are set out on the electronic student housing registration service page and usually include proof of registration, an identity document and the details the service requires; follow what the official page states when you apply."));

            seed.Add(Faq(PgPhases.Enrolled,
                "متى أسجّل في السكن الجامعي؟",
                "When do I register for student housing?",
                "يُفتح التسجيل في السكن وفق المواعيد التي تعلنها عمادة شؤون الطالبات؛ فتابعي صفحة الخدمة الرسمية وقنوات العمادة لمعرفة مواعيد فتح التسجيل والتعليمات المصاحبة.",
                "Housing registration opens on the dates announced by the Deanship of Student Affairs; follow the official service page and the deanship's channels for the registration opening dates and the accompanying instructions."));

            seed.Add(Faq(PgPhases.Enrolled,
                "بمن أتواصل بشأن السكن الجامعي؟",
                "Whom do I contact about student housing?",
                "تُدار خدمات السكن عبر عمادة شؤون الطالبات، ويمكنكِ التواصل معها ومتابعة مستجدّات الخدمة من خلال صفحة خدمة التسجيل الإلكتروني في سكن الطالبات وقنوات العمادة الرسمية.",
                "Housing services are run by the Deanship of Student Affairs; you can contact it and follow service updates through the electronic student housing registration service page and the deanship's official channels."));

            // ===================== الخريجون / Graduates =====================
            _faqOrder = 0;

            seed.Add(Faq(PgPhases.Graduates,
                "ما المتطلّبات اللازمة لإتمام تخرّجي؟",
                "What is required in order to complete my graduation?",
                "تسليم الرسالة العلمية بصورتها النهائية إلى القسم والكلية لرفعها إلى العمادة، وتسليم نسختين ورقيتين من الرسالة لعمادة الدراسات العليا، واستكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه.",
                "Submitting the final version of the thesis to the department and the college so they may forward it to the deanship, delivering two printed copies of the thesis to the Deanship of Graduate Studies, and completing the clearance form with the approval of the entities listed on it."));

            seed.Add(Faq(PgPhases.Graduates,
                "كيف تُرفع التوصية بمنح الدرجة العلمية؟",
                "How is the recommendation to award the degree submitted?",
                "يرفع القسم تقرير لجنة المناقشة إلى الكلية، ثم تضمّ وحدة الخريجين بالعمادة اسمكِ إلى مذكرة الخريجين، وتُرفع إلى مجلس الجامعة، وتُعتمد من رئيس الجامعة المكلّفة وفق الصلاحيات النظامية.",
                "The department forwards the examination committee report to the college, then the graduates unit at the deanship adds your name to the graduates memorandum, which goes to the university council and is approved by the acting university president under the statutory powers."));

            seed.Add(Faq(PgPhases.Graduates,
                "كيف أستلم وثيقة التخرّج؟",
                "How do I receive my graduation document?",
                "بعد اعتماد منح الدرجة، تطبعين نموذج إخلاء الطرف وتستكملينه، ثم تراجعين عمادة القبول والتسجيل لاستلام وثيقة التخرّج.",
                "Once the award of the degree is approved, print and complete the clearance form, then go to the Deanship of Admission and Registration to receive your graduation document."));

            seed.Add(Faq(PgPhases.Graduates,
                "ما المقصود بإخلاء الطرف؟",
                "What is meant by clearance?",
                "هو استكمال إنهاء التزاماتكِ تجاه وحدات الجامعة عبر النموذج المعتمد، وهو خطوةٌ ختاميةٌ لازمةٌ قبل استلام وثيقة التخرّج.",
                "It is the completion of your obligations towards the university's units through the approved form, and it is a necessary closing step before receiving the graduation document."));

            seed.Add(Faq(PgPhases.Graduates,
                "كيف أتابع تغيّر حالتي إلى متخرّجة؟",
                "How do I follow the change of my status to graduate?",
                "تُحدَّث حالتكِ بعد اعتماد منح الدرجة العلمية ورفع خطاب إصدار الوثيقة إلى إدارة القبول والتسجيل؛ ويمكنكِ متابعة ذلك مع وحدة الخريجين بعمادة الدراسات العليا.",
                "Your status is updated once the award of the degree is approved and the letter requesting the issue of the document is sent to the Admission and Registration Department; you can follow this up with the graduates unit at the Deanship of Graduate Studies."));

            seed.Add(Faq(PgPhases.Graduates,
                "كيف أسلّم رسالتي العلمية للعمادة؟",
                "How do I submit my thesis to the deanship?",
                "تُسلَّم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة، مع تسليم نسختين ورقيتين للعمادة.",
                "The thesis is submitted electronically in its final form to the department and the college, which forward the approved version to the Deanship of Graduate Studies, together with two printed copies delivered to the deanship."));

            seed.Add(Faq(PgPhases.Graduates,
                "ما الجهات التي يلزم اعتمادها في نموذج إخلاء الطرف؟",
                "Which entities must sign off on the clearance form?",
                "يلزم اعتماد النموذج من: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا.",
                "The form must be approved by the university library, student housing, the financial affairs unit at the deanship, and the vice-deanship for postgraduate academic affairs."));

            seed.Add(Faq(PgPhases.Graduates,
                "ما دور رئيسة الجامعة المكلّفة في منح الدرجة؟",
                "What is the role of the acting university president in awarding the degree?",
                "بعد رفع مذكرة الخريجين إلى مجلس الجامعة، تُعتمد الدرجة العلمية وفق الصلاحيات النظامية المقرّرة، تمهيداً لإصدار وثيقة التخرّج.",
                "After the graduates memorandum goes to the university council, the degree is approved under the established statutory powers, in preparation for the issue of the graduation document."));

            seed.Add(Faq(PgPhases.Graduates,
                "من الجهة التي تُصدر وثيقة التخرّج؟",
                "Which body issues the graduation document?",
                "بعد اعتماد منح الدرجة، يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج، وتراجعينها لاستلامها بعد استكمال إخلاء الطرف.",
                "Once the award of the degree is approved, a letter is sent to the Admission and Registration Department to issue the graduation document, and you collect it from them after completing your clearance."));

            return seed;
        }
    }
}
