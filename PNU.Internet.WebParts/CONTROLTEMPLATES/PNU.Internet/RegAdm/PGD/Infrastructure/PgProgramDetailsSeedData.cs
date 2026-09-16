using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public static class PgProgramDetailsSeedData
    {
        public static List<PgProgramDetailsModel> GetSeedDetails()
        {
            var list = new List<PgProgramDetailsModel>();

            #region 1. الماجستير المهني في الإرشاد المدرسي (pro55)
            list.Add(new PgProgramDetailsModel
            {
                ProgramCode = "pro55",
                TitleAr = "الماجستير المهني في الإرشاد المدرسي",
                TitleEn = "Professional Master in School Counseling",
                CategoryKey = "NouraStaff",
                DegreeAr = "ماجستير مهني",
                DegreeEn = "Professional Master",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DurationAr = "سنتان (4 فصول دراسية)",
                DurationEn = "2 Years (4 Semesters)",
                CreditHoursAr = "42 ساعة معتمدة",
                CreditHoursEn = "42 Credit Hours",
                LanguageAr = "اللغة العربية",
                LanguageEn = "Arabic",
                StudySystemAr = "مقررات دراسية وتدريب ميداني ومشروع تطبيقي",
                StudySystemEn = "Coursework, Field Training, and Applied Project",
                TuitionFeesAr = "18,000 ريال / الفصل الدراسي",
                TuitionFeesEn = "18,000 SAR / Semester",
                OverviewAr = "يهدف برنامج الماجستير المهني في الإرشاد المدرسي إلى إعداد كفاءات وطنية متخصصة ومؤهلة مهنياً وعلمياً في مجال التوجيه والإرشاد النفسي والتربوي بالمدارس والبيئات التعليمية، بما يواكب مستهدفات رؤية المملكة 2030 وبرنامج تنمية القدرات البشرية.",
                OverviewEn = "The Professional Master in School Counseling prepares specialized national competencies professionally and academically qualified in educational and psychological counseling in schools.",
                MissionAr = "تقديم تعليم وتدريب مهني نوعي في الإرشاد المدرسي من خلال بيئة تعليمية محفزة وشراكات مجتمعية فاعلة لدعم التوافق النفسي والتربوي للطلبة.",
                MissionEn = "Providing quality professional education and training in school counseling through a stimulating learning environment and active community partnerships.",
                ObjectivesAr = @"<ul>
                    <li>إكساب الدارسات المعارف والمهارات المتقدمة في التقييم والتشخيص والإرشاد النفسي والتربوي.</li>
                    <li>تأهيل كوادر متميزة قادرة على تصميم وتنفيذ البرامج الوقائية والعلاجية في المؤسسات التعليمية.</li>
                    <li>تطبيق المعايير الأخلاقية والمهنية المعتمدة في الممارسة الإرشادية.</li>
                    <li>توظيف التقنيات الحديثة والبحوث الإجرائية لتطوير بيئات التعلم المدرسية.</li>
                </ul>",
                ObjectivesEn = @"<ul>
                    <li>Provide students with advanced knowledge and skills in psychological and educational assessment and counseling.</li>
                    <li>Prepare distinguished cadres capable of designing and implementing preventive programs in educational institutions.</li>
                    <li>Apply approved ethical and professional standards in counseling practice.</li>
                </ul>",
                AdmissionRequirementsAr = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'><i class='hgi hgi-stroke hgi-diploma text-success me-2'></i>المؤهل الأكاديمي</h4>
                        <p class='small text-muted mb-0'>أن تكون المتقدمة حاصلة على درجة البكالوريوس في تخصص (علم النفس، التربية الخاصة، الخدمة الاجتماعية، أو أحد التخصصات التربوية) من جامعة معترف بها بتقدير لا يقل عن جيد جداً.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'><i class='hgi hgi-stroke hgi-user-check text-success me-2'></i>المقابلة الشخصية والاختبارات</h4>
                        <p class='small text-muted mb-0'>اجتياز المقابلة الشخصية وأي اختبارات تحريرية أو معيارية تقرها الكلية واللجنة المشتركة للبرنامج.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'><i class='hgi hgi-stroke hgi-file-check text-success me-2'></i>الشروط العامة</h4>
                        <p class='small text-muted mb-0'>استيفاء الشروط العامة للقبول في الدراسات العليا بجامعة الأميرة نورة بنت عبدالرحمن واللائحة الموحدة للدراسات العليا في الجامعات السعودية.</p>
                    </div>
                </div>",
                AdmissionRequirementsEn = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>Academic Qualification</h4>
                        <p class='small text-muted mb-0'>Bachelor's degree in Psychology, Special Education, Social Work, or an Educational discipline with at least 'Very Good' grade.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>Personal Interview</h4>
                        <p class='small text-muted mb-0'>Passing personal interview and departmental admission exams.</p>
                    </div>
                </div>",
                StudyPlanAr = @"<div class='table-responsive'>
                    <table class='table table-bordered table-hover align-middle small mb-0'>
                        <thead class='table-light'>
                            <tr><th>الفصل الدراسي</th><th>رمز المقرر</th><th>اسم المقرر</th><th>عدد الساعات</th></tr>
                        </thead>
                        <tbody>
                            <tr><td rowspan='3' class='fw-bold text-center bg-light'>الفصل الأول</td><td>EDG 501</td><td>نظريات الإرشاد النفسي والتربوي</td><td>3</td></tr>
                            <tr><td>EDG 502</td><td>التقييم والتشخيص في الإرشاد المدرسي</td><td>3</td></tr>
                            <tr><td>EDG 503</td><td>مهارات المقابلة الإرشادية وتطبيقاتها</td><td>3</td></tr>
                            <tr><td rowspan='3' class='fw-bold text-center bg-light'>الفصل الثاني</td><td>EDG 504</td><td>الإرشاد الفردي والجمعي للطلبة</td><td>3</td></tr>
                            <tr><td>EDG 505</td><td>النمو النفسي ومشكلات الطفولة والمراهقة</td><td>3</td></tr>
                            <tr><td>EDG 506</td><td>تصميم البرامج الإرشادية والوقائية</td><td>3</td></tr>
                            <tr><td rowspan='3' class='fw-bold text-center bg-light'>الفصل الثالث</td><td>EDG 601</td><td>التدريب الميداني الإرشادي (1)</td><td>4</td></tr>
                            <tr><td>EDG 602</td><td>إرشاد ذوي الاحتياجات الخاصة والموهوبين</td><td>3</td></tr>
                            <tr><td>EDG 603</td><td>أخلاقيات وضوابط مهنة الإرشاد</td><td>2</td></tr>
                            <tr><td rowspan='2' class='fw-bold text-center bg-light'>الفصل الرابع</td><td>EDG 604</td><td>التدريب الميداني المكثف (2)</td><td>6</td></tr>
                            <tr><td>EDG 605</td><td>مشروع التخرج التطبيقي في الإرشاد المدرسي</td><td>3</td></tr>
                        </tbody>
                    </table>
                </div>",
                StudyPlanEn = "<p>Complete 4-semester curriculum including theoretical foundations, advanced assessment, field internships, and a capstone applied project.</p>",
                StudyPlanPdfUrl = "/ar/RegAdm/PGD/Documents/pro55-plan.pdf",
                CareerOpportunitiesAr = @"<div class='row g-3'>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مرشدة طلابية وموجهة مدرسية</h5><p class='small text-muted mb-0'>في مدارس التعليم العام والخاص ومدارس الموهوبين والدولية.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>أخصائية إرشاد نفسي وتربوي</h5><p class='small text-muted mb-0'>في مراكز التوجيه والإرشاد وإدارات التعليم والجامعات.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مستشارة تطوير سلوكي وتربوي</h5><p class='small text-muted mb-0'>في المنظمات غير الربحية ومراكز الاستشارات الأسرية والتربوية.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مصممة برامج وقائية وتوعوية</h5><p class='small text-muted mb-0'>في الهيئات والمؤسسات المعنية بالطفولة والشباب وجودة الحياة.</p></div></div>
                </div>",
                CareerOpportunitiesEn = @"<ul><li>School Counselor in Public, Private, and International Schools</li><li>Educational and Psychological Specialist in Guidance Centers</li><li>Behavioral and Educational Consultant</li></ul>",
                ApplyUrl = PgCategorySeedData.DefaultBannerUrl,
                ContactEmail = "dps-admissions@pnu.edu.sa",
                IsActive = true
            });
            #endregion

            #region 2. دكتوراه الفلسفة في الترجمة (progdf)
            list.Add(new PgProgramDetailsModel
            {
                ProgramCode = "progdf",
                TitleAr = "دكتوراه الفلسفة في الترجمة",
                TitleEn = "PhD in Translation Studies",
                CategoryKey = "Humanitarian",
                DegreeAr = "دكتوراه الفلسفة (PhD)",
                DegreeEn = "Doctor of Philosophy (PhD)",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم الترجمة",
                DepartmentEn = "Department of Translation",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DurationAr = "3 - 4 سنوات (6 - 8 فصول دراسية)",
                DurationEn = "3 - 4 Years (6 - 8 Semesters)",
                CreditHoursAr = "48 ساعة معتمدة + أطروحة الدكتوراه",
                CreditHoursEn = "48 Credit Hours + Doctoral Dissertation",
                LanguageAr = "اللغة العربية والإنجليزية",
                LanguageEn = "Arabic and English",
                StudySystemAr = "مقررات دراسية متقدمة واختبار شامل وأطروحة دكتوراه",
                StudySystemEn = "Advanced Coursework, Comprehensive Exam, and PhD Dissertation",
                TuitionFeesAr = "22,000 ريال / الفصل الدراسي",
                TuitionFeesEn = "22,000 SAR / Semester",
                OverviewAr = "برنامج أكاديمي بحثي متقدم يهدف إلى إعداد باحثات وأكاديميات متخصصات في دراسات الترجمة والنظريات اللغوية التطبيقية، وإثراء الإنتاج العلمي في مجالات الترجمة التخصصية والآلية وتعزيز الحوار الحضاري.",
                OverviewEn = "An advanced research doctoral program aiming to prepare specialized scholars in translation studies, applied linguistic theories, and AI-assisted translation.",
                MissionAr = "الريادة في دراسات الترجمة والبحث العلمي المتقدم لخدمة الهوية الوطنية واللغة العربية وإثراء التواصل الثقافي والمعرفي العالمي.",
                MissionEn = "Leadership in translation studies and advanced scientific research to serve national identity, the Arabic language, and intercultural communication.",
                ObjectivesAr = @"<ul>
                    <li>تأهيل باحثات متميزات قادرات على إجراء أبحاث أصيلة تسهم في تطوير نظريات الترجمة وتطبيقاتها.</li>
                    <li>دعم حركة التعريب ونقل المعارف المتقدمة إلى اللغة العربية في كافة العلوم.</li>
                    <li>تطوير استراتيجيات الترجمة التقنية والآلية والذكاء الاصطناعي في الترجمة.</li>
                    <li>إعداد كفاءات أكاديمية للعمل في الجامعات والمراكز البحثية والهيئات الدولية.</li>
                </ul>",
                ObjectivesEn = @"<ul>
                    <li>Prepare scholars capable of conducting original research that advances translation theories and applications.</li>
                    <li>Support knowledge transfer and localization into Arabic across all disciplines.</li>
                    <li>Develop AI and machine translation strategies.</li>
                </ul>",
                AdmissionRequirementsAr = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>المؤهل الأكاديمي السابق</h4>
                        <p class='small text-muted mb-0'>الحصول على درجة الماجستير في الترجمة أو اللغويات التطبيقية أو الأدب الإنجليزي بتقدير لا يقل عن جيد جداً.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>الكفاءة في اللغة الإنجليزية</h4>
                        <p class='small text-muted mb-0'>الحصول على درجة لا تقل عن (6.5) في اختبار IELTS الأكاديمي أو ما يعادلها في اختبار TOEFL iBT.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>مقترح البحث العلمي (Research Proposal)</h4>
                        <p class='small text-muted mb-0'>تقديم مقترح بحثي أصيل في أحد مجالات دراسات الترجمة واجتياز المقابلة العلمية أمام لجنة الدراسات العليا بالقسم.</p>
                    </div>
                </div>",
                AdmissionRequirementsEn = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>Academic Degree</h4>
                        <p class='small text-muted mb-0'>Master's degree in Translation, Applied Linguistics, or English Studies with at least 'Very Good' grade.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>English Language Proficiency</h4>
                        <p class='small text-muted mb-0'>IELTS Academic score of at least 6.5 or equivalent in TOEFL iBT.</p>
                    </div>
                </div>",
                StudyPlanAr = @"<p class='small text-muted mb-3'>يتضمن البرنامج مقررات دراسية متقدمة في النظريات النقدية لدراسات الترجمة، مناهج البحث النوعي والكمي، علم مصطلحات الترجمة، الترجمة الآلية وحوسبة اللغة، يليها الاختبار الشامل وإعداد أطروحة الدكتوراه ومناقشتها علنياً.</p>",
                StudyPlanEn = "<p>Advanced doctoral coursework followed by comprehensive qualifying exam and PhD dissertation defense.</p>",
                StudyPlanPdfUrl = "/ar/RegAdm/PGD/Documents/progdf-plan.pdf",
                CareerOpportunitiesAr = @"<div class='row g-3'>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>أستاذة جامعية وباحثة أكاديمية</h5><p class='small text-muted mb-0'>في كليات اللغات والترجمة والأقسام الأكاديمية بالجامعات السعودية والدولية.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>خبيرة ومستشارة ترجمة</h5><p class='small text-muted mb-0'>في المنظمات الدولية والسفارات والوزارات والهيئات الدبلوماسية والحكومية.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مديرة مراكز التعريب والترجمة</h5><p class='small text-muted mb-0'>في دور النشر الكبرى والمجامع اللغوية ومراكز الأبحاث الاستراتيجية.</p></div></div>
                </div>",
                CareerOpportunitiesEn = @"<ul><li>University Faculty & Academic Researcher</li><li>Senior Translation Consultant in International Organizations</li><li>Director of Translation & Localization Centers</li></ul>",
                ApplyUrl = PgCategorySeedData.DefaultBannerUrl,
                ContactEmail = "lang-pg@pnu.edu.sa",
                IsActive = true
            });
            #endregion

            #region 3. دكتوراه الفلسفة في إدارة الأعمال بالشراكة مع DCU (SciencesProg12)
            list.Add(new PgProgramDetailsModel
            {
                ProgramCode = "SciencesProg12",
                TitleAr = "دكتوراه الفلسفة في إدارة الأعمال بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "PhD in Business Administration in Partnership with Dublin City University (DCU)",
                CategoryKey = "Scientific",
                DegreeAr = "دكتوراه الفلسفة (PhD)",
                DegreeEn = "Doctor of Philosophy (PhD)",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "برنامج بيني (الإدارة - المحاسبة - الاقتصاد)",
                DepartmentEn = "Interdisciplinary (Management - Accounting - Economics)",
                ScopeAr = "عالمي (شراكة دولية)",
                ScopeEn = "International (Global Partnership)",
                DurationAr = "4 سنوات (8 فصول دراسية)",
                DurationEn = "4 Years (8 Semesters)",
                CreditHoursAr = "برنامج بحثي متقدم معتمد دولياً",
                CreditHoursEn = "Internationally Accredited Research Doctorate",
                LanguageAr = "اللغة الإنجليزية",
                LanguageEn = "English",
                StudySystemAr = "مقررات بحثية مشتركة وإشراف أكاديمي مزدوج وأطروحة دكتوراه",
                StudySystemEn = "Joint Research Seminars, Dual Academic Supervision & Dissertation",
                TuitionFeesAr = "برنامج دولي مخصص",
                TuitionFeesEn = "Specialized International Partnership Program",
                OverviewAr = "برنامج دكتوراه دولي نوعي يُقدّم بالشراكة الأكاديمية الاستراتيجية بين جامعة الأميرة نورة بنت عبدالرحمن وجامعة مدينة دبلن (Dublin City University - DCU) في أيرلندا، لتأهيل قائدات وباحثات في علوم الإدارة والأعمال وفق أعلى المعايير الدولية.",
                OverviewEn = "A premium international doctoral program delivered in academic partnership with Dublin City University (DCU), Ireland, preparing research leaders in business administration.",
                MissionAr = "توفير بيئة بحثية عالمية تمكّن الباحثات من إنتاج معرفة إدارية واقتصادية متقدمة ومبتكرة تُعزز تنافسية الأعمال وتدعم التحول الاقتصادي.",
                MissionEn = "Providing a world-class research environment empowering scholars to produce cutting-edge business knowledge supporting economic transformation.",
                ObjectivesAr = @"<ul>
                    <li>إعداد جيل من الباحثات الرائدات القادرات على إجراء أبحاث إدارية واقتصادية متقدمة منشورة في المجلات العلمية العالمية المحكمة (Q1/Q2).</li>
                    <li>تطوير حلول إدارية وريادية مبتكرة للتحديات المؤسسية والاقتصادية المعاصرة.</li>
                    <li>تعزيز التعاون البحثي الدولي والتبادل الأكاديمي مع الجامعات والمراكز العالمية المرموقة.</li>
                </ul>",
                ObjectivesEn = @"<ul>
                    <li>Prepare leading researchers capable of publishing high-impact management research in top peer-reviewed journals.</li>
                    <li>Develop innovative entrepreneurial solutions for contemporary organizational challenges.</li>
                    <li>Promote international research collaboration with Dublin City University.</li>
                </ul>",
                AdmissionRequirementsAr = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>1. المؤهل الأكاديمي السابق</h4>
                        <p class='small text-muted mb-0'>الحصول على درجة الماجستير في إدارة الأعمال (MBA) أو ماجستير العلوم في إدارة الأعمال، المحاسبة، المالية، أو الاقتصاد بتقدير لا يقل عن جيد جداً.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>2. الكفاءة في اللغة الإنجليزية</h4>
                        <p class='small text-muted mb-0'>الحصول على درجة لا تقل عن (6.5) في اختبار IELTS (مع ما لا يقل عن 6.0 في جميع المهارات) أو ما يعادلها في TOEFL iBT.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>3. مقترح البحث والمقابلة المشتركة</h4>
                        <p class='small text-muted mb-0'>تقديم مقترح بحثي متكامل باللغة الإنجليزية واجتياز المقابلة العلمية المشتركة مع لجنة القبول بجامعة مدينة دبلن (DCU).</p>
                    </div>
                </div>",
                AdmissionRequirementsEn = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>1. Academic Degree</h4>
                        <p class='small text-muted mb-0'>Master's degree in Business (MBA, MSc in Business/Accounting/Finance/Economics) with at least 'Very Good'.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>2. English Proficiency</h4>
                        <p class='small text-muted mb-0'>IELTS score of at least 6.5 (min 6.0 in each band) or TOEFL iBT equivalent.</p>
                    </div>
                </div>",
                StudyPlanAr = "<p class='small text-muted mb-2'>خطة دراسية وبحثية مشتركة تتضمن حلقات سيمينار بحثية متقدمة، الإشراف الأكاديمي المشترك من كبار الأساتذة في جامعة الأميرة نورة وجامعة مدينة دبلن، ونشر أوراق علمية محكمة وإتمام أطروحة الدكتوراه.</p>",
                StudyPlanEn = "<p>Joint doctoral research curriculum with DCU supervision and international publication milestones.</p>",
                StudyPlanPdfUrl = "/ar/RegAdm/PGD/Documents/SciencesProg12-plan.pdf",
                CareerOpportunitiesAr = @"<div class='row g-3'>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>عضو هيئة تدريس وباحثة قيادية</h5><p class='small text-muted mb-0'>في كليات الأعمال والجامعات السعودية والإقليمية والدولية.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مستشارة استراتيجية واقتصادية</h5><p class='small text-muted mb-0'>في كبرى بيوت الخبرة والاستشارات العالمية والشركات متعددة الجنسيات.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>رئيسة قطاع التخطيط والتطوير المؤسسي</h5><p class='small text-muted mb-0'>في الهيئات الحكومية والصناديق السيادية والمنظمات التنموية.</p></div></div>
                </div>",
                CareerOpportunitiesEn = @"<ul><li>University Faculty & Academic Leadership</li><li>Strategic & Economic Consultant in Global Advisory Firms</li><li>Executive Director of Corporate Planning & Development</li></ul>",
                ApplyUrl = PgCategorySeedData.DefaultBannerUrl,
                PartnerUniversityAr = "جامعة مدينة دبلن (Dublin City University - DCU)",
                PartnerUniversityEn = "Dublin City University (DCU)",
                PartnerUniversityUrl = "https://www.dcu.ie",
                ContactEmail = "cba-dcu@pnu.edu.sa",
                IsActive = true
            });
            #endregion

            #region 4. ماجستير العلوم في علوم التمريض بالشراكة مع DCU (healthProg1)
            list.Add(new PgProgramDetailsModel
            {
                ProgramCode = "healthProg1",
                TitleAr = "ماجستير العلوم في علوم التمريض (تخصص: ممارسة العلوم التمريضية المتقدمة) بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "MSc in Nursing (Advanced Nursing Practice) in Partnership with Dublin City University (DCU)",
                CategoryKey = "Health",
                DegreeAr = "ماجستير العلوم (MSc)",
                DegreeEn = "Master of Science (MSc)",
                CollegeAr = "كلية التمريض",
                CollegeEn = "College of Nursing",
                DepartmentAr = "لجنة البرنامج المشترك",
                DepartmentEn = "Joint Program Committee",
                ScopeAr = "عالمي (شراكة دولية)",
                ScopeEn = "International (Global Partnership)",
                DurationAr = "سنتان (4 فصول دراسية)",
                DurationEn = "2 Years (4 Semesters)",
                CreditHoursAr = "38 ساعة معتمدة + تدريب إكلينيكي متقدم",
                CreditHoursEn = "38 Credit Hours + Advanced Clinical Practicum",
                LanguageAr = "اللغة الإنجليزية",
                LanguageEn = "English",
                StudySystemAr = "مقررات دراسية وتدريب سريري متقدم ومشروع بحثي",
                StudySystemEn = "Coursework, Advanced Clinical Practicum, and Research Project",
                TuitionFeesAr = "برنامج دولي مخصص",
                TuitionFeesEn = "Specialized International Partnership Program",
                OverviewAr = "برنامج ماجستير دولي مرموق يُقدّم بالشراكة مع كلية التمريض والعلاج الطبيعي والصحة المجتمعية بجامعة مدينة دبلن (DCU)، لإعداد ممارسات تمريض متقدمات يتمتعن بمهارات قيادية وسريرية وبحثية استثنائية.",
                OverviewEn = "A prestigious international Master's program delivered in partnership with Dublin City University (DCU) to prepare advanced nursing practitioners with outstanding leadership and clinical skills.",
                MissionAr = "الارتقاء بالرعاية الصحية ومهنة التمريض من خلال إعداد ممارسات تمريض متقدمات يقدمن رعاية سريرية مبنية على البراهين ويسهمن في تطوير النظم الصحية.",
                MissionEn = "Advancing healthcare by preparing advanced practice nurses delivering evidence-based clinical care and transforming health systems.",
                ObjectivesAr = @"<ul>
                    <li>تطوير مهارات التقييم الإكلينيكي الشامل واتخاذ القرارات السريرية المتقدمة.</li>
                    <li>إعداد قائدات في التمريض قادرات على إدارة الفرق الصحية المعقدة وتطبيق مبادرات الجودة وسلامة المرضى.</li>
                    <li>إجراء أبحاث سريرية تطبيقية لتطوير الممارسة التمريضية المبنية على الأدلة والبراهين.</li>
                </ul>",
                ObjectivesEn = @"<ul>
                    <li>Develop advanced comprehensive clinical assessment and clinical decision-making skills.</li>
                    <li>Prepare nursing leaders capable of managing complex healthcare teams.</li>
                    <li>Conduct applied clinical research to advance evidence-based nursing practice.</li>
                </ul>",
                AdmissionRequirementsAr = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>1. المؤهل الأكاديمي</h4>
                        <p class='small text-muted mb-0'>بكالوريوس في علوم التمريض من جامعة معترف بها بتقدير لا يقل عن جيد جداً.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>2. الكفاءة في اللغة الإنجليزية</h4>
                        <p class='small text-muted mb-0'>حصول المتقدمة على درجة (6.5) في اختبار IELTS أو ما يعادلها في TOEFL iBT.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>3. التصنيف والترخيص المهني</h4>
                        <p class='small text-muted mb-0'>تسجيل وتصنيف مهني ساري المفعول من الهيئة السعودية للتخصصات الصحية (SCFHS) مع خبرة إكلينيكية لا تقل عن سنتين.</p>
                    </div>
                </div>",
                AdmissionRequirementsEn = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>1. Academic Degree</h4>
                        <p class='small text-muted mb-0'>Bachelor of Science in Nursing (BSN) with at least 'Very Good'.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>2. Professional Licensure</h4>
                        <p class='small text-muted mb-0'>Valid registration with Saudi Commission for Health Specialties (SCFHS) with at least 2 years clinical experience.</p>
                    </div>
                </div>",
                StudyPlanAr = "<p class='small text-muted mb-2'>مقررات متقدمة في الفيزيولوجيا المرضية السريرية، علم الأدوية المتقدم، القيادة والسياسات الصحية، الممارسة التمريضية المتقدمة والتدريب السريري المكثف في المستشفيات المرجعية.</p>",
                StudyPlanEn = "<p>Advanced curriculum in pathophysiology, advanced pharmacology, healthcare leadership, and clinical rotations.</p>",
                StudyPlanPdfUrl = "/ar/RegAdm/PGD/Documents/healthProg1-plan.pdf",
                CareerOpportunitiesAr = @"<div class='row g-3'>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>أخصائية تمريض إكلينيكي متقدم (APN)</h5><p class='small text-muted mb-0'>في المستشفيات التخصصية والمدن الطبية ومراكز الرعاية المتقدمة.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>مديرة خدمات تمريضية وجودة صحية</h5><p class='small text-muted mb-0'>في التجمعات الصحية والمستشفيات والقطاعات الصحية الحكومية والخاصة.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>محاضرة وباحثة إكلينيكية</h5><p class='small text-muted mb-0'>في كليات التمريض والعلوم الصحية ومراكز الأبحاث الطبية.</p></div></div>
                </div>",
                CareerOpportunitiesEn = @"<ul><li>Advanced Practice Nurse (APN) in Specialized Medical Centers</li><li>Director of Nursing Services & Healthcare Quality</li><li>Clinical Educator & Faculty Member</li></ul>",
                ApplyUrl = PgCategorySeedData.DefaultBannerUrl,
                PartnerUniversityAr = "جامعة مدينة دبلن (Dublin City University - DCU)",
                PartnerUniversityEn = "Dublin City University (DCU)",
                PartnerUniversityUrl = "https://www.dcu.ie",
                ContactEmail = "nursing-dcu@pnu.edu.sa",
                IsActive = true
            });
            #endregion

            // Populate generic detailed fallback templates for all remaining 40+ programs from PgCategorySeedData
            var allPrograms = PgCategorySeedData.GetPrograms();
            foreach (var prog in allPrograms)
            {
                string code = ExtractCodeFromUrl(prog.DetailsUrl);
                if (!string.IsNullOrEmpty(code) && !list.Any(x => x.ProgramCode.Equals(code, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(GenerateDefaultDetailsForProgram(prog, code));
                }
            }

            return list;
        }

        public static string ExtractCodeFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            string clean = url.Trim();
            int qIdx = clean.IndexOf('?');
            if (qIdx >= 0) clean = clean.Substring(0, qIdx);
            int hIdx = clean.IndexOf('#');
            if (hIdx >= 0) clean = clean.Substring(0, hIdx);
            try
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(clean);
                return !string.IsNullOrEmpty(filename) ? filename.Trim() : clean;
            }
            catch
            {
                return clean;
            }
        }

        private static PgProgramDetailsModel GenerateDefaultDetailsForProgram(PgProgramItemModel p, string code)
        {
            string degreeAr = p.DegreeGroupTitleAr ?? "ماجستير";
            string degreeEn = p.DegreeGroupTitleEn ?? "Master";

            return new PgProgramDetailsModel
            {
                ProgramCode = code,
                TitleAr = p.TitleAr,
                TitleEn = p.TitleEn,
                CategoryKey = p.CategoryKey,
                DegreeAr = degreeAr,
                DegreeEn = degreeEn,
                CollegeAr = p.CollegeAr,
                CollegeEn = p.CollegeEn,
                DepartmentAr = p.DepartmentAr,
                DepartmentEn = p.DepartmentEn,
                ScopeAr = p.ScopeAr,
                ScopeEn = p.ScopeEn,
                DurationAr = "سنتان (4 فصول دراسية)",
                DurationEn = "2 Years (4 Semesters)",
                CreditHoursAr = "36 - 42 ساعة معتمدة",
                CreditHoursEn = "36 - 42 Credit Hours",
                LanguageAr = p.CategoryKey == "Health" || p.IsInternational ? "اللغة الإنجليزية" : "اللغة العربية",
                LanguageEn = p.CategoryKey == "Health" || p.IsInternational ? "English" : "Arabic",
                StudySystemAr = "مقررات دراسية وتدريب / مشروع بحثي",
                StudySystemEn = "Coursework and Research Project",
                TuitionFeesAr = "18,000 ريال / الفصل الدراسي",
                TuitionFeesEn = "18,000 SAR / Semester",
                OverviewAr = string.Format("يهدف {0} المطروح في {1} ({2}) إلى إعداد كفاءات وطنية متخصصة ومؤهلة علمياً ومهنياً تلبي احتياجات سوق العمل والتنمية الوطنية المعاصرة وفق أعلى المعايير الأكاديمية.", p.TitleAr, p.CollegeAr, p.DepartmentAr),
                OverviewEn = string.Format("The {0} offered by {1} prepares specialized competencies academically and professionally qualified to meet labor market needs.", p.TitleEn, p.CollegeEn),
                MissionAr = string.Format("تقديم تعليم أكاديمي وبحثي متميز في مجال {0} يسهم في إثراء المعرفة وبناء مجتمع معرفي منافس عالمياً.", p.TitleAr),
                MissionEn = string.Format("Delivering quality education and research in {0} to foster knowledge and global competitiveness.", p.TitleEn),
                ObjectivesAr = @"<ul>
                    <li>تزويد الدارسات بالمعارف والمهارات التخصصية المتقدمة وفق أحدث المعايير العلمية.</li>
                    <li>تنمية مهارات التفكير النقدي والبحث العلمي التطبيقي وحل المشكلات المؤسسية.</li>
                    <li>تأهيل كفاءات قيادية تسهم بفاعلية في خطط التطوير ومستهدفات رؤية 2030.</li>
                </ul>",
                ObjectivesEn = @"<ul>
                    <li>Equip students with advanced specialized knowledge and skills.</li>
                    <li>Develop critical thinking and applied research capabilities.</li>
                    <li>Prepare leadership competencies contributing to Vision 2030 goals.</li>
                </ul>",
                AdmissionRequirementsAr = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>المؤهل الأكاديمي المطلوب</h4>
                        <p class='small text-muted mb-0'>أن تكون المتقدمة حاصلة على درجة البكالوريوس في تخصص ذي صلة من جامعة معترف بها بتقدير لا يقل عن جيد جداً.</p>
                    </div>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>الشروط العامة واللائحة الموحدة</h4>
                        <p class='small text-muted mb-0'>استيفاء الشروط العامة للقبول في الدراسات العليا بجامعة الأميرة نورة بنت عبدالرحمن واجتياز المقابلة الشخصية إن وجدت.</p>
                    </div>
                </div>",
                AdmissionRequirementsEn = @"<div class='d-flex flex-column gap-3'>
                    <div class='p-3 bg-light rounded-3 border'>
                        <h4 class='h6 fw-bold text-dark mb-2'>Academic Qualification</h4>
                        <p class='small text-muted mb-0'>Bachelor's degree in a relevant field with at least 'Very Good' grade.</p>
                    </div>
                </div>",
                StudyPlanAr = "<p class='small text-muted mb-2'>يتضمن البرنامج مقررات دراسية تخصصية وأنشطة تدريبية وتطبيقية ومشروع تخرج أو رسالة علمية موزعة على الفصول الدراسية المعتمدة.</p>",
                StudyPlanEn = "<p>Specialized coursework, training activities, and capstone project/thesis distributed across semesters.</p>",
                StudyPlanPdfUrl = p.DetailsUrl,
                CareerOpportunitiesAr = @"<div class='row g-3'>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>أخصائية ومستشارة متخصصة</h5><p class='small text-muted mb-0'>في الوزارات والهيئات الحكومية والشركات الرائدة.</p></div></div>
                    <div class='col-12 col-md-6'><div class='card h-100 p-3 border bg-white rounded-3 shadow-none'><h5 class='h6 fw-bold text-success mb-1'>باحثة وأكاديمية</h5><p class='small text-muted mb-0'>في الجامعات والمراكز البحثية المتخصصة.</p></div></div>
                </div>",
                CareerOpportunitiesEn = @"<ul><li>Specialist & Consultant in Government and Private Sectors</li><li>Researcher & Educator in Academic Institutions</li></ul>",
                ApplyUrl = p.ApplyUrl ?? PgCategorySeedData.DefaultBannerUrl,
                PartnerUniversityAr = p.IsInternational ? "شراكة دولية مع جامعة عالمية مرموقة" : "",
                PartnerUniversityEn = p.IsInternational ? "International University Partnership" : "",
                ContactEmail = "dps-admissions@pnu.edu.sa",
                IsActive = true
            };
        }
    }
}
