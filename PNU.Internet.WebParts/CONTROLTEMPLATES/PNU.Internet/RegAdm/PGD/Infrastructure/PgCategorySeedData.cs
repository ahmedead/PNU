using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public static class PgCategorySeedData
    {
        public const string DefaultBannerUrl = "https://banssb.pnu.edu.sa/PROD_ar/bwykolad.P_ShowQuesAns?applno=MTQ0MzEwMDAwMDA3Nzgx";
        public const string DefaultReqUrl = "/ar/RegAdm/PGD/Pages/AdmissionRequirements.aspx";
        public const string DefaultScheduleUrl = "/ar/RegAdm/PGD/Pages/AdmissionSchedule.aspx";

        public static List<PgCategoryOverviewModel> GetOverviews()
        {
            return new List<PgCategoryOverviewModel>
            {
                new PgCategoryOverviewModel
                {
                    CategoryKey = "NouraStaff",
                    HeroTagAr = "برامج كوادر نورة",
                    HeroTagEn = "Nourah Cadres Programs",
                    HeroSubTagAr = "الدراسات العليا",
                    HeroSubTagEn = "Postgraduate Studies",
                    HeroTitleAr = "برامج كوادر نورة للدراسات العليا",
                    HeroTitleEn = "Nourah Cadres Postgraduate Programs",
                    HeroDescriptionAr = "تطرح جامعة الأميرة نورة بنت عبدالرحمن مجموعة متميزة من برامج الماجستير المهني والدبلوم العالي المصممة لتطوير وتأهيل الكوادر الوطنية، وتلبية الاحتياجات المتجددة لسوق العمل والقطاعين الحكومي وغير الربحي وفق أعلى المعايير الأكاديمية والمهنية.",
                    HeroDescriptionEn = "Princess Nourah bint Abdulrahman University offers a distinguished set of professional master's and higher diploma programs designed to develop and qualify national cadres and meet evolving labor market needs.",
                    BannerTitleAr = "بوابة القبول الإلكترونية الموحدة (Banner SIS)",
                    BannerTitleEn = "Unified Electronic Admission Portal (Banner SIS)",
                    BannerDescriptionAr = "يمكنك التقديم مباشرة على البرامج المتاحة والاطلاع على حالة الطلب عبر البوابة الإلكترونية.",
                    BannerDescriptionEn = "You can apply directly to available programs and track application status through the electronic portal.",
                    BannerButtonTextAr = "التقديم عبر بوابة القبول",
                    BannerButtonTextEn = "Apply via Admission Portal",
                    BannerUrl = DefaultBannerUrl,
                    Card1TitleAr = "دليل وشروط القبول العام",
                    Card1TitleEn = "General Admission Guide & Requirements",
                    Card1DescAr = "الاطلاع على اللائحة الموحدة والشروط العامة للقبول",
                    Card1DescEn = "View unified regulations and general admission criteria",
                    Card1Url = DefaultReqUrl,
                    Card2TitleAr = "التقويم الزمني للقبول",
                    Card2TitleEn = "Admission Timeline",
                    Card2DescAr = "مواعيد فتح وإغلاق بوابات القبول للدراسات العليا",
                    Card2DescEn = "Opening and closing dates for postgraduate admissions",
                    Card2Url = DefaultScheduleUrl,
                    IsActive = true
                },
                new PgCategoryOverviewModel
                {
                    CategoryKey = "Humanitarian",
                    HeroTagAr = "التخصصات الإنسانية",
                    HeroTagEn = "Humanitarian Disciplines",
                    HeroSubTagAr = "الدراسات العليا",
                    HeroSubTagEn = "Postgraduate Studies",
                    HeroTitleAr = "برامج الدراسات العليا في التخصصات الإنسانية",
                    HeroTitleEn = "Postgraduate Programs in Humanitarian Disciplines",
                    HeroDescriptionAr = "دليل برامج الدكتوراه، الماجستير الأكاديمي، الماجستير المهني، والدبلوم العالي في التخصصات الإنسانية والتربوية واللغوية والاجتماعية والقانونية بجامعة الأميرة نورة بنت عبدالرحمن.",
                    HeroDescriptionEn = "Guide to PhD, Academic Master's, Professional Master's, and Higher Diploma programs in humanities, education, languages, social sciences, and law at Princess Nourah University.",
                    BannerTitleAr = "بوابة القبول الإلكترونية الموحدة (Banner SIS)",
                    BannerTitleEn = "Unified Electronic Admission Portal (Banner SIS)",
                    BannerDescriptionAr = "يمكنك التقديم مباشرة على برامج الدراسات العليا والاطلاع على حالة الطلب عبر البوابة الإلكترونية.",
                    BannerDescriptionEn = "You can apply directly to postgraduate programs and check your application status online.",
                    BannerButtonTextAr = "التقديم عبر بوابة القبول",
                    BannerButtonTextEn = "Apply via Admission Portal",
                    BannerUrl = DefaultBannerUrl,
                    Card1TitleAr = "دليل وشروط القبول العام",
                    Card1TitleEn = "General Admission Guide & Requirements",
                    Card1DescAr = "الاطلاع على اللائحة الموحدة والشروط العامة للقبول",
                    Card1DescEn = "View unified regulations and general admission criteria",
                    Card1Url = DefaultReqUrl,
                    Card2TitleAr = "التقويم الزمني للقبول",
                    Card2TitleEn = "Admission Timeline",
                    Card2DescAr = "مواعيد فتح وإغلاق بوابات القبول للدراسات العليا",
                    Card2DescEn = "Opening and closing dates for postgraduate admissions",
                    Card2Url = DefaultScheduleUrl,
                    IsActive = true
                },
                new PgCategoryOverviewModel
                {
                    CategoryKey = "Interdisciplinary",
                    HeroTagAr = "البرامج البينية",
                    HeroTagEn = "Interdisciplinary Programs",
                    HeroSubTagAr = "الدراسات العليا",
                    HeroSubTagEn = "Postgraduate Studies",
                    HeroTitleAr = "البرامج البينية (Interdisciplinary Programs)",
                    HeroTitleEn = "Interdisciplinary Postgraduate Programs",
                    HeroDescriptionAr = "برامج دراسات عليا نوعية تجمع بين تخصصات وكليات متعددة لتقديم حلول تعليمية وبحثية ومهنية تكاملية تواكب التطورات المعرفية واحتياجات سوق العمل والتنمية الوطنية.",
                    HeroDescriptionEn = "High-quality postgraduate programs combining multiple disciplines and colleges to deliver integrative educational, research, and professional solutions aligned with national development goals.",
                    BannerTitleAr = "بوابة القبول الإلكترونية الموحدة (Banner SIS)",
                    BannerTitleEn = "Unified Electronic Admission Portal (Banner SIS)",
                    BannerDescriptionAr = "يمكنك التقديم مباشرة على البرامج البينية والاطلاع على حالة الطلب عبر البوابة الإلكترونية.",
                    BannerDescriptionEn = "You can apply directly to interdisciplinary programs and track your application status online.",
                    BannerButtonTextAr = "التقديم عبر بوابة القبول",
                    BannerButtonTextEn = "Apply via Admission Portal",
                    BannerUrl = DefaultBannerUrl,
                    Card1TitleAr = "شروط القبول العامة",
                    Card1TitleEn = "General Admission Requirements",
                    Card1DescAr = "شروط وضوابط القبول الموحدة للبرامج البينية",
                    Card1DescEn = "Unified admission terms and guidelines for interdisciplinary programs",
                    Card1Url = DefaultReqUrl,
                    Card2TitleAr = "التقويم الزمني للقبول",
                    Card2TitleEn = "Admission Timeline",
                    Card2DescAr = "مواعيد التقديم ومراحل الفرز والمفاضلة",
                    Card2DescEn = "Application schedules, screening, and selection stages",
                    Card2Url = DefaultScheduleUrl,
                    IsActive = true
                },
                new PgCategoryOverviewModel
                {
                    CategoryKey = "Scientific",
                    HeroTagAr = "التخصصات العلمية وإدارة الأعمال",
                    HeroTagEn = "Scientific & Business Disciplines",
                    HeroSubTagAr = "الدراسات العليا",
                    HeroSubTagEn = "Postgraduate Studies",
                    HeroTitleAr = "برامج الدراسات العليا في التخصصات العلمية وإدارة الأعمال",
                    HeroTitleEn = "Postgraduate Programs in Scientific & Business Disciplines",
                    HeroDescriptionAr = "دليل شامل لكافة برامج الدكتوراه، الماجستير الأكاديمي، والماجستير المهني في مجالات العلوم الأساسية، الحوسبة وتقنية المعلومات، وإدارة الأعمال، سواءً البرامج المحلية أو البرامج المقدمة بالشراكة مع الجامعات العالمية المرموقة.",
                    HeroDescriptionEn = "Comprehensive guide to all PhD, Academic Master's, and Professional Master's programs across pure sciences, computing & IT, and business administration, including global university partnerships.",
                    BannerTitleAr = "بوابة القبول الإلكترونية الموحدة (Banner SIS)",
                    BannerTitleEn = "Unified Electronic Admission Portal (Banner SIS)",
                    BannerDescriptionAr = "يمكنك التقديم مباشرة على كافة برامج الدراسات العليا العلمية عبر بوابة القبول الإلكترونية لجامعة الأميرة نورة.",
                    BannerDescriptionEn = "Apply directly to all scientific postgraduate programs via Princess Nourah University admission portal.",
                    BannerButtonTextAr = "التقديم عبر بوابة القبول",
                    BannerButtonTextEn = "Apply via Admission Portal",
                    BannerUrl = DefaultBannerUrl,
                    Card1TitleAr = "دليل شروط القبول",
                    Card1TitleEn = "Admission Requirements Guide",
                    Card1DescAr = "شروط وضوابط القبول في البرامج العلمية",
                    Card1DescEn = "Admission terms and criteria for scientific programs",
                    Card1Url = DefaultReqUrl,
                    Card2TitleAr = "التقويم الزمني للقبول",
                    Card2TitleEn = "Admission Timeline",
                    Card2DescAr = "مواعيد التقديم وإعلان نتائج القبول",
                    Card2DescEn = "Application schedules and admission results announcement",
                    Card2Url = DefaultScheduleUrl,
                    IsActive = true
                },
                new PgCategoryOverviewModel
                {
                    CategoryKey = "Health",
                    HeroTagAr = "التخصصات الصحية والطبية",
                    HeroTagEn = "Health & Medical Disciplines",
                    HeroSubTagAr = "الدراسات العليا",
                    HeroSubTagEn = "Postgraduate Studies",
                    HeroTitleAr = "برامج الدراسات العليا في التخصصات الصحية والطبية",
                    HeroTitleEn = "Postgraduate Programs in Health & Medical Disciplines",
                    HeroDescriptionAr = "دليل شامل لبرامج الماجستير في العلوم التمريضية، الصيدلة الإكلينيكية والصناعية، طب الأسنان، علم النفس الإكلينيكي، التغذية، والمحاكاة الصحية لتأهيل الكوادر الصحية المتخصصة وفق أعلى المعايير المهنية والسريرية العالمية.",
                    HeroDescriptionEn = "Comprehensive guide to Master's programs in nursing, clinical and industrial pharmacy, dentistry, clinical psychology, nutrition, and health simulation according to highest international clinical standards.",
                    BannerTitleAr = "بوابة القبول الإلكترونية الموحدة (Banner SIS)",
                    BannerTitleEn = "Unified Electronic Admission Portal (Banner SIS)",
                    BannerDescriptionAr = "يمكنك التقديم مباشرة على كافة البرامج الصحية والاطلاع على حالة الطلب عبر البوابة الإلكترونية.",
                    BannerDescriptionEn = "Apply directly to all health programs and monitor application status through the portal.",
                    BannerButtonTextAr = "التقديم عبر بوابة القبول",
                    BannerButtonTextEn = "Apply via Admission Portal",
                    BannerUrl = DefaultBannerUrl,
                    Card1TitleAr = "دليل وشروط القبول الصحي",
                    Card1TitleEn = "Health Admission Guide & Requirements",
                    Card1DescAr = "شروط التصنيف والتسجيل المهني ومتطلبات الكليات الصحية",
                    Card1DescEn = "Professional classification, registration, and health college admission prerequisites",
                    Card1Url = DefaultReqUrl,
                    Card2TitleAr = "التقويم الزمني للقبول",
                    Card2TitleEn = "Admission Timeline",
                    Card2DescAr = "مواعيد التقديم والاختبارات والمقابلات الشخصية",
                    Card2DescEn = "Application schedules, entrance exams, and personal interviews",
                    Card2Url = DefaultScheduleUrl,
                    IsActive = true
                }
            };
        }

        public static List<PgProgramItemModel> GetPrograms()
        {
            var list = new List<PgProgramItemModel>();
            int order = 1;

            #region 1. كوادر نورة (NouraStaff)
            // برامج الماجستير المهني 1
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في الإرشاد المدرسي",
                TitleEn = "Professional Master in School Counseling",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pro55.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في علم نفس العمل في المنظمات",
                TitleEn = "Professional Master in Organizational Work Psychology",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pro342.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في الخدمات الانتقالية للطلبة ذوي الإعاقة",
                TitleEn = "Professional Master in Transition Services for Students with Disabilities",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/promm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تنمية القدرات البشرية للقطاع الحكومي",
                TitleEn = "Professional Master in Human Capability Development for the Government Sector",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "مشترك بين قسم التعليم والتعلم وقسم القيادة التربوية والتطوير التنظيمي",
                DepartmentEn = "Joint between Teaching & Learning and Educational Leadership & Organizational Development",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pronmm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // برامج الماجستير المهني 2
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تعليم العلوم والتكنولوجيا والهندسة والرياضيات (ستيم)",
                TitleEn = "Professional Master in STEM Education",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/prommm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تصميم التعليم والتكنولوجيا",
                TitleEn = "Professional Master in Instructional Design and Technology",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro9.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في القيادة التعليمية وريادة الأعمال",
                TitleEn = "Professional Master in Educational Leadership and Entrepreneurship",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم القيادة التربوية والتطوير التنظيمي",
                DepartmentEn = "Department of Educational Leadership and Organizational Development",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/mmb.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // برامج الدبلوم العالي
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في قيادة المنظمات التعليمية غير الربحية",
                TitleEn = "Higher Diploma in Leading Non-Profit Educational Organizations",
                CategoryKey = "NouraStaff",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية / كلية العلوم الإنسانية والاجتماعية",
                CollegeEn = "College of Education & Human Development / College of Humanities & Social Sciences",
                DepartmentAr = "قسم القيادة التربوية والتطوير التنظيمي / الخدمة الاجتماعية",
                DepartmentEn = "Educational Leadership & Organizational Development / Social Work",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/progd.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            #endregion

            #region 2. البرامج الإنسانية (Humanitarian)
            // دكتوراه
            list.Add(new PgProgramItemModel
            {
                TitleAr = "دكتوراه الفلسفة في الترجمة",
                TitleEn = "PhD in Translation Studies",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "PhD",
                DegreeGroupTitleAr = "برامج الدكتوراه",
                DegreeGroupTitleEn = "Doctoral Programs (PhD)",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم الترجمة",
                DepartmentEn = "Department of Translation",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/progdf.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // ماجستير أكاديمي
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير في الترجمة التخصصية من اللغة العربية إلى اللغة الإنجليزية وبالعكس",
                TitleEn = "Master in Specialized Translation (Arabic - English / English - Arabic)",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم الترجمة",
                DepartmentEn = "Department of Translation",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro1.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير في تصميم الأزياء والنسيج",
                TitleEn = "Master in Fashion and Textile Design",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية التصاميم والفنون",
                CollegeEn = "College of Design and Arts",
                DepartmentAr = "قسم تصميم الأزياء والنسيج",
                DepartmentEn = "Department of Fashion and Textile Design",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro2.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير التدخل المبكر في الطفولة",
                TitleEn = "Master in Early Childhood Intervention",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "برنامج بيني بين قسمي (التعليم والتعلم - الطفولة المبكرة)",
                DepartmentEn = "Interdisciplinary between Teaching & Learning and Early Childhood",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro10.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الفنون البصرية",
                TitleEn = "Master of Visual Arts",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية التصاميم والفنون",
                CollegeEn = "College of Design and Arts",
                DepartmentAr = "قسم الفنون البصرية",
                DepartmentEn = "Department of Visual Arts",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro11.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الخدمة الاجتماعية الإكلينيكية",
                TitleEn = "Master of Clinical Social Work",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية العلوم الإنسانية والاجتماعية",
                CollegeEn = "College of Humanities and Social Sciences",
                DepartmentAr = "قسم الخدمة الاجتماعية",
                DepartmentEn = "Department of Social Work",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro12.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الطفولة المبكرة",
                TitleEn = "Master in Early Childhood",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم الطفولة المبكرة",
                DepartmentEn = "Department of Early Childhood",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/mmch.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الأمن الفكري",
                TitleEn = "Master in Intellectual Security",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "كلية العلوم الإنسانية والاجتماعية",
                CollegeEn = "College of Humanities and Social Sciences",
                DepartmentAr = "قسم الدراسات الإسلامية",
                DepartmentEn = "Department of Islamic Studies",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/bbro.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // ماجستير مهني 1
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في الإرشاد المدرسي",
                TitleEn = "Professional Master in School Counseling",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pro55.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في علم نفس العمل في المنظمات",
                TitleEn = "Professional Master in Organizational Work Psychology",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pro342.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في الخدمات الانتقالية للطلبة ذوي الإعاقة",
                TitleEn = "Professional Master in Transition Services for Students with Disabilities",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/promm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تنمية القدرات البشرية للقطاع الحكومي",
                TitleEn = "Professional Master in Human Capability Development for the Government Sector",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "مشترك بين قسم التعليم والتعلم وقسم القيادة التربوية والتطوير التنظيمي",
                DepartmentEn = "Joint between Teaching & Learning and Educational Leadership & Organizational Development",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/pronmm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في الإنجليزية المهنية والتواصل العالمي",
                TitleEn = "Professional Master in Professional English and Global Communication",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني 1",
                DegreeGroupTitleEn = "Professional Master's Programs 1",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم اللغويات التطبيقية",
                DepartmentEn = "Department of Applied Linguistics",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/MPEGC.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // ماجستير مهني 2
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تعليم العلوم والتكنولوجيا والهندسة والرياضيات (ستيم)",
                TitleEn = "Professional Master in STEM Education",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/prommm.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في تصميم التعليم والتكنولوجيا",
                TitleEn = "Professional Master in Instructional Design and Technology",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro9.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في القيادة التعليمية وريادة الأعمال",
                TitleEn = "Professional Master in Educational Leadership and Entrepreneurship",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "ProfessionalMaster2",
                DegreeGroupTitleAr = "برامج الماجستير المهني 2",
                DegreeGroupTitleEn = "Professional Master's Programs 2",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم القيادة التربوية والتطوير التنظيمي",
                DepartmentEn = "Department of Educational Leadership and Organizational Development",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/mmb.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // دبلوم عالي
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في التعلم الإلكتروني (حضوري)",
                TitleEn = "Higher Diploma in E-Learning (In-Person)",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro3.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في التعلم الإلكتروني (عن بعد)",
                TitleEn = "Higher Diploma in E-Learning (Distance Learning)",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro8.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في التدريس باللغة الإنجليزية",
                TitleEn = "Higher Diploma in English Language Teaching",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro4.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في تحليل السلوك التطبيقي",
                TitleEn = "Higher Diploma in Applied Behavior Analysis",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية",
                CollegeEn = "College of Education and Human Development",
                DepartmentAr = "قسم التعليم والتعلم",
                DepartmentEn = "Department of Teaching and Learning",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro5.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في اللغة الفرنسية للسياحة",
                TitleEn = "Higher Diploma in French for Tourism",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم الترجمة",
                DepartmentEn = "Department of Translation",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro6.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في اللغة الإنجليزية للسياحة",
                TitleEn = "Higher Diploma in English for Tourism",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية اللغات",
                CollegeEn = "College of Languages",
                DepartmentAr = "قسم اللغويات التطبيقية",
                DepartmentEn = "Department of Applied Linguistics",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro7.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في الوسائل البديلة لفض وتسوية المنازعات",
                TitleEn = "Higher Diploma in Alternative Dispute Resolution",
                CategoryKey = "Humanitarian",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية القانون",
                CollegeEn = "College of Law",
                DepartmentAr = "قسم القانون الخاص",
                DepartmentEn = "Department of Private Law",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/HumanitarianPro08.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            #endregion

            #region 3. البرامج البينية (Interdisciplinary)
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدبلوم العالي في قيادة المنظمات التعليمية غير الربحية",
                TitleEn = "Higher Diploma in Leading Non-Profit Educational Organizations",
                CategoryKey = "Interdisciplinary",
                DegreeGroupKey = "HigherDiploma",
                DegreeGroupTitleAr = "برامج الدبلوم العالي",
                DegreeGroupTitleEn = "Higher Diploma Programs",
                CollegeAr = "كلية التربية والتنمية البشرية / كلية العلوم الإنسانية والاجتماعية",
                CollegeEn = "College of Education & Human Development / College of Humanities & Social Sciences",
                DepartmentAr = "القيادة التربوية والتطوير التنظيمي / الخدمة الاجتماعية",
                DepartmentEn = "Educational Leadership & Organizational Development / Social Work",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/progd.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الآداب في دراسات المرأة",
                TitleEn = "Master of Arts in Women's Studies",
                CategoryKey = "Interdisciplinary",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي",
                DegreeGroupTitleEn = "Academic Master's Programs",
                CollegeAr = "عمادة الدراسات العليا",
                CollegeEn = "Deanship of Graduate Studies",
                DepartmentAr = "اللجنة الدائمة للبرنامج المشترك",
                DepartmentEn = "Standing Committee for the Joint Program",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/Interfaceprog1.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في إدارة المخاطر والطوارئ واستمرارية الأعمال",
                TitleEn = "Professional Master in Risk, Emergency and Business Continuity Management",
                CategoryKey = "Interdisciplinary",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني",
                DegreeGroupTitleEn = "Professional Master's Programs",
                CollegeAr = "كلية العلوم / كلية إدارة الأعمال",
                CollegeEn = "College of Science / College of Business Administration",
                DepartmentAr = "الأحياء / وقسم الإدارة",
                DepartmentEn = "Biology / Department of Management",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/RMEBC.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            #endregion

            #region 4. البرامج العلمية (Scientific)
            // دكتوراه
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الدكتوراه في الأحياء التطبيقية والتقنية الحيوية",
                TitleEn = "PhD in Applied Biology and Biotechnology",
                CategoryKey = "Scientific",
                DegreeGroupKey = "PhD",
                DegreeGroupTitleAr = "برامج الدكتوراه",
                DegreeGroupTitleEn = "Doctoral Programs (PhD)",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "قسم الأحياء",
                DepartmentEn = "Department of Biology",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg10.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "دكتوراه الفلسفة في إدارة الأعمال بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "PhD in Business Administration in Partnership with Dublin City University (DCU)",
                CategoryKey = "Scientific",
                DegreeGroupKey = "PhD",
                DegreeGroupTitleAr = "برامج الدكتوراه",
                DegreeGroupTitleEn = "Doctoral Programs (PhD)",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "برنامج بيني (الإدارة - المحاسبة - الاقتصاد)",
                DepartmentEn = "Interdisciplinary (Management - Accounting - Economics)",
                ScopeAr = "عالمي (شراكة دولية)",
                ScopeEn = "International (Global Partnership)",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg12.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });

            // ماجستير علمي / إدارة أعمال
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في إدارة الأعمال بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "MSc in Business Administration in Partnership with Dublin City University (DCU)",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم الإدارة",
                DepartmentEn = "Department of Management",
                ScopeAr = "عالمي",
                ScopeEn = "International",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg1.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في إدارة الأعمال (مسار القيادة في الطيران) بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "MSc in Business Administration (Aviation Leadership Track) with Dublin City University (DCU)",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم الإدارة",
                DepartmentEn = "Department of Management",
                ScopeAr = "عالمي",
                ScopeEn = "International",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg13.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في المحاسبة والضرائب",
                TitleEn = "MSc in Accounting and Taxation",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم المحاسبة",
                DepartmentEn = "Department of Accounting",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg2.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الاقتصاد التطبيقي",
                TitleEn = "Master of Applied Economics",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم الاقتصاد",
                DepartmentEn = "Department of Economics",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg3.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير إدارة الأعمال التنفيذي (EMBA) بالشراكة مع جامعة ستراثكلايد البريطانية",
                TitleEn = "Executive MBA (EMBA) in Partnership with University of Strathclyde",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم الإدارة",
                DepartmentEn = "Department of Management",
                ScopeAr = "عالمي",
                ScopeEn = "International",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg4.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علوم الحوسبة (علم البيانات)",
                TitleEn = "MSc in Computer Science (Data Science)",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية علوم الحاسب والمعلومات",
                CollegeEn = "College of Computer and Information Sciences",
                DepartmentAr = "برنامج بيني (علوم الحاسبات - نظم المعلومات - تقنية المعلومات)",
                DepartmentEn = "Interdisciplinary (Computer Science - Information Systems - Information Technology)",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg5.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في الأمن السيبراني",
                TitleEn = "MSc in Cybersecurity",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية علوم الحاسب والمعلومات",
                CollegeEn = "College of Computer and Information Sciences",
                DepartmentAr = "برنامج بيني (علوم الحاسبات - نظم المعلومات - تقنية المعلومات)",
                DepartmentEn = "Interdisciplinary (Computer Science - Information Systems - Information Technology)",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg6.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير في علوم الأحياء المتقدمة (المسارات: الأحياء الجزيئي والخلوي | علم المناعة | علم المفصليات والطفيليات)",
                TitleEn = "Master in Advanced Biological Sciences (Tracks: Molecular & Cellular Biology | Immunology | Arthropods & Parasitology)",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "قسم الأحياء",
                DepartmentEn = "Department of Biology",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg7.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير علم الأحياء الدقيقة البيئي والتقنية الحيوية",
                TitleEn = "Master in Environmental Microbiology and Biotechnology",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "قسم الأحياء",
                DepartmentEn = "Department of Biology",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg8.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في الطاقة والابتكار بالشراكة مع جامعة سوانزي البريطانية",
                TitleEn = "MSc in Energy and Innovation with Swansea University",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "برنامج بيني",
                DepartmentEn = "Interdisciplinary Program",
                ScopeAr = "عالمي",
                ScopeEn = "International",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg9.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير كيمياء وتقنيات المواد",
                TitleEn = "Master in Chemistry and Material Technologies",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "قسم الكيمياء",
                DepartmentEn = "Department of Chemistry",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/SciencesProg11.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الرياضيات الحيوية والتنبؤ بالأوبئة",
                TitleEn = "Master in Biomathematics and Epidemic Forecasting",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "برنامج بيني (العلوم الرياضية - الأحياء)",
                DepartmentEn = "Interdisciplinary (Mathematical Sciences - Biology)",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/mmath.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الفيزياء النووية والإشعاع",
                TitleEn = "Master in Nuclear Physics and Radiation",
                CategoryKey = "Scientific",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير الأكاديمي والمهني",
                DegreeGroupTitleEn = "Academic & Professional Master's Programs",
                CollegeAr = "كلية العلوم",
                CollegeEn = "College of Science",
                DepartmentAr = "قسم الفيزياء",
                DepartmentEn = "Department of Physics",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/prt.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "الماجستير المهني في المراجعة الداخلية والحوكمة",
                TitleEn = "Professional Master in Internal Auditing and Governance",
                CategoryKey = "Scientific",
                DegreeGroupKey = "ProfessionalMaster1",
                DegreeGroupTitleAr = "برامج الماجستير المهني",
                DegreeGroupTitleEn = "Professional Master's Programs",
                CollegeAr = "كلية إدارة الأعمال",
                CollegeEn = "College of Business Administration",
                DepartmentAr = "قسم المحاسبة",
                DepartmentEn = "Department of Accounting",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/MIAG.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            #endregion

            #region 5. البرامج الصحية (Health)
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علوم التمريض (تخصص: ممارسة العلوم التمريضية المتقدمة) بالشراكة مع جامعة مدينة دبلن (DCU)",
                TitleEn = "MSc in Nursing (Advanced Nursing Practice) in Partnership with Dublin City University (DCU)",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية التمريض",
                CollegeEn = "College of Nursing",
                DepartmentAr = "لجنة البرنامج المشترك",
                DepartmentEn = "Joint Program Committee",
                ScopeAr = "عالمي (شراكة دولية)",
                ScopeEn = "International (Global Partnership)",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg1.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علوم التمريض – تمريض رعاية كبار السن",
                TitleEn = "MSc in Nursing - Gerontological Nursing",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية التمريض",
                CollegeEn = "College of Nursing",
                DepartmentAr = "تمريض صحة المجتمع والصحة النفسية والعقلية",
                DepartmentEn = "Community, Mental and Psychiatric Health Nursing",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg2.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علوم التمريض - ممارسة العلوم التمريضية المتقدمة مسار تمريض الرعاية الحرجة للكبار",
                TitleEn = "MSc in Nursing - Advanced Nursing Practice (Adult Critical Care Track)",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية التمريض",
                CollegeEn = "College of Nursing",
                DepartmentAr = "التمريض الباطني والجراحي",
                DepartmentEn = "Medical-Surgical Nursing",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg3.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علوم التمريض - ممارسة العلوم التمريضية المتقدمة مسار تمريض العناية القلبية والأوعية الدموية",
                TitleEn = "MSc in Nursing - Advanced Nursing Practice (Cardiovascular Care Track)",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية التمريض",
                CollegeEn = "College of Nursing",
                DepartmentAr = "التمريض الباطني والجراحي",
                DepartmentEn = "Medical-Surgical Nursing",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg3.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في علم النفس الإكلينيكي",
                TitleEn = "MSc in Clinical Psychology",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية الصحة وعلوم التأهيل",
                CollegeEn = "College of Health and Rehabilitation Sciences",
                DepartmentAr = "قسم علوم الصحة",
                DepartmentEn = "Department of Health Sciences",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg4.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير الصيدلة الإكلينيكية المتقدمة",
                TitleEn = "Master in Advanced Clinical Pharmacy",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية الصيدلة",
                CollegeEn = "College of Pharmacy",
                DepartmentAr = "قسم الممارسة الصيدلانية",
                DepartmentEn = "Department of Pharmacy Practice",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg5.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في الصيدلة الصناعية",
                TitleEn = "MSc in Industrial Pharmacy",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية الصيدلة",
                CollegeEn = "College of Pharmacy",
                DepartmentAr = "قسم العلوم الصيدلانية",
                DepartmentEn = "Department of Pharmaceutical Sciences",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg6.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            list.Add(new PgProgramItemModel
            {
                TitleAr = "ماجستير العلوم في طب الأسنان الإصلاحي والتجميلي",
                TitleEn = "MSc in Restorative and Esthetic Dentistry",
                CategoryKey = "Health",
                DegreeGroupKey = "AcademicMaster",
                DegreeGroupTitleAr = "برامج الماجستير في التخصصات الصحية",
                DegreeGroupTitleEn = "Master's Programs in Health Disciplines",
                CollegeAr = "كلية طب الأسنان",
                CollegeEn = "College of Dentistry",
                DepartmentAr = "قسم علوم طب الأسنان السريرية",
                DepartmentEn = "Department of Clinical Dental Sciences",
                ScopeAr = "محلي",
                ScopeEn = "Local",
                DetailsUrl = "/ar/RegAdm/PGD/Pages/healthProg14.aspx",
                ApplyUrl = DefaultBannerUrl,
                ItemOrder = order++,
                IsActive = true
            });
            #endregion

            return list;
        }
    }
}
