using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public class PageMigrationItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Language { get; set; } // "ar" or "en"
        public string PageLayoutUrl { get; set; }
        public string ContentHtml { get; set; }

        public PageMigrationItem()
        {
            PageLayoutUrl = "/_catalogs/masterpage/DGANewBlankWebPartPage.aspx";
            Language = "ar";
        }
    }

    public static class ProgramPagesDgaData
    {
        public const string DefaultLayout = "/_catalogs/masterpage/DGANewBlankWebPartPage.aspx";

        public static List<PageMigrationItem> GetPredefinedPgdPrograms()
        {
            var list = new List<PageMigrationItem>();

            // 1. HumanitarianPro3 (Arabic & English) - E-Learning in person
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro3.aspx",
                Title = "الدبلوم العالي في التعلم الإلكتروني (حضوري)",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd3Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro3.aspx",
                Title = "Postgraduate Diploma in E-Learning (in person)",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd3En()
            });

            // 2. HumanitarianPro8 (Arabic & English) - E-Learning Virtual
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro8.aspx",
                Title = "الدبلوم العالي في التعلم الإلكتروني (عن بعد)",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd8Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro8.aspx",
                Title = "Postgraduate Diploma in E-Learning (Virtual)",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd8En()
            });

            // 3. HumanitarianPro4 (Arabic & English) - EMI
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro4.aspx",
                Title = "الدبلوم العالي في التدريس باللغة الإنجليزية (EMI)",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd4Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro4.aspx",
                Title = "Postgraduate Diploma in English as a Medium of Instruction (EMI)",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd4En()
            });

            // 4. HumanitarianPro5 (Arabic & English) - Applied Behavior Analysis
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro5.aspx",
                Title = "الدبلوم العالي في تحليل السلوك التطبيقي",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd5Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro5.aspx",
                Title = "Postgraduate Diploma in Applied Behavior Analysis",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd5En()
            });

            // 5. HumanitarianPro6 (Arabic & English) - French for Tourism
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro6.aspx",
                Title = "الدبلوم العالي باللغة الفرنسية للسياحة",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd6Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro6.aspx",
                Title = "Postgraduate Diploma in French for Tourism",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd6En()
            });

            // 6. HumanitarianPro7 (Arabic & English) - English for Tourism
            list.Add(new PageMigrationItem
            {
                Url = "/ar/RegAdm/PGD/Pages/HumanitarianPro7.aspx",
                Title = "الدبلوم العالي في اللغة الإنجليزية للسياحة",
                Language = "ar",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd7Ar()
            });
            list.Add(new PageMigrationItem
            {
                Url = "/en/RegAdm/PGD/Pages/HumanitarianPro7.aspx",
                Title = "Postgraduate Diploma in English for Tourism",
                Language = "en",
                PageLayoutUrl = DefaultLayout,
                ContentHtml = BuildPgd7En()
            });

            return list;
        }

        #region Program 1: E-Learning (in person)

        private static string BuildPgd3Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية التربية والتنمية البشرية</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">تعليم مدمج (حضوري)</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-mortarboard-02""></i><span>عمادة الدراسات العليا</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-computer text-primary""></i><span>الدبلوم العالي في التعلم الإلكتروني (حضوري)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         برنامج دبلوم عالٍ بنظام التعليم المدمج، يهدف إلى إعداد كوادر تعليمية تمتلك المعارف النظرية والمهارات التطبيقية في مجال التعلم الإلكتروني وتوظيف أحدث التقنيات الرقمية والمنصات التفاعلية لخدمة العملية التعليمية.
      </p>
   </div>

   <section aria-labelledby=""pgd3-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-user-star-01 fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">تنمية الكفاءات البشرية المؤهلة</h3>
               <p class=""small text-body-secondary mb-0"">تطوير كوادر تجمع بين التأهيل التربوي الأكاديمي والخبرة العملية في توظيف البرامج والخدمات الإلكترونية في الأنشطة التعليمية التفاعلية.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-flow-connection fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">التطبيق المهني وحل التحديات</h3>
               <p class=""small text-body-secondary mb-0"">الانتقال إلى التطبيق المهني المتقدم للتعلم الإلكتروني ومعالجة التحديات الميدانية والتقنية المرتبطة ببيئات التعلم الرقمية الحديثة.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات ومشروع بحثي</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة العربية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">14,400 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل العلمي:</strong> الحصول على درجة البكالوريوس في أي تخصص من جامعة معترف بها.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المعدل التراكمي:</strong> ألا يقل التقدير العام عن ""جيد"" في مرحلة البكالوريوس.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd3-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">مصمم تعليمي</strong>
               <span class=""text-muted small"">تصميم وهيكلة المحتوى والمقررات الإلكترونية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">مطور محتوى تعليمي</strong>
               <span class=""text-muted small"">إنتاج الوسائط والمواد الرقمية التفاعلية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">أخصائي تدريب وتطوير</strong>
               <span class=""text-muted small"">إدارة مشاريع ومنصات التعليم والتدريب الإلكتروني.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd3-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل وثيقة الخطة الدراسية المعتمدة وتوزيع المقررات (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التعلم%20الالكتروني%20-حضوري.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd3En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Education</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">Blended (In Person)</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-mortarboard-02""></i><span>Deanship of Postgraduate Studies</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-computer text-primary""></i><span>Postgraduate Diploma in E-Learning (in person)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         A Higher Diploma program offered in a blended learning system aimed at preparing educational professionals with both theoretical knowledge and practical skills in e-learning and modern interactive technologies.
      </p>
   </div>

   <section aria-labelledby=""pgd3-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-user-star-01 fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Human Resource Competency</h3>
               <p class=""small text-body-secondary mb-0"">Develop specialized human resources combining educational qualifications with hands-on expertise in interactive digital learning programs.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-flow-connection fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Professional Application & Challenges</h3>
               <p class=""small text-body-secondary mb-0"">Enable transition into professional applications of e-learning while effectively addressing implementation and technical challenges.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Courses & Research Project</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">Arabic</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">14,400 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's degree in any discipline from a recognized institution.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Minimum GPA:</strong> A minimum grade of ""Good"" (C) in the undergraduate degree.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd3-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd3-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Instructional Designer</strong>
               <span class=""text-muted small"">Design and structure digital educational courses.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Content Developer</strong>
               <span class=""text-muted small"">Author interactive learning media and assessments.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Training Specialist</strong>
               <span class=""text-muted small"">Manage corporate and academic e-learning programs.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd3-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd3-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved academic curriculum and course breakdown (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التعلم%20الالكتروني%20-حضوري.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion

        #region Program 2: E-Learning (Virtual)

        private static string BuildPgd8Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية التربية والتنمية البشرية</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">تعليم عن بعد (افتراضي)</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-award-02 text-primary""></i><span>الأول من نوعه في الجامعات العربية</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-laptop-programming text-primary""></i><span>الدبلوم العالي في التعلم الإلكتروني (عن بعد)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         يُصنف برنامج الدبلوم العالي في التعلم الإلكتروني بجامعة الأميرة نورة كأول برنامج من نوعه على مستوى الجامعات العربية، ويمنح الخريجين ميزة تنافسية كبرى لإعدادهم للممارسات المهنية واستكشاف أحدث المستجدات في تقنيات وتطبيقات التعليم الرقمي.
      </p>
   </div>

   <section aria-labelledby=""pgd8-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-user-star-01 fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">تأهيل الكفاءات في المنصات الافتراضية</h3>
               <p class=""small text-body-secondary mb-0"">تطوير كوادر تجمع بين التأهيل الأكاديمي والمهارات التقنية لإدارة الصفوف الافتراضية والمنصات التعليمية الرقمية بكفاءة.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-global-education fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">الريادة في حلول التعلم عن بعد</h3>
               <p class=""small text-body-secondary mb-0"">الريادة في ابتكار حلول مرنة للتعلم الذاتي والمتزامن وغير المتزامن بما يتماشى مع التوجهات العالمية للتحول الرقمي.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات دراسية فقط</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة العربية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطلاب والطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">12,960 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل العلمي:</strong> الحصول على درجة البكالوريوس في أي تخصص من جامعة معترف بها.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المعدل التراكمي:</strong> ألا يقل التقدير العام عن ""جيد"" في مرحلة البكالوريوس.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd8-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجين والخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">مصمم ومطور تعليمي</strong>
               <span class=""text-muted small"">بناء وتطوير البيئات الرقمية التفاعلية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">أخصائي تدريب عن بعد</strong>
               <span class=""text-muted small"">إدارة وتنسيق البرامج التدريبية الافتراضية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">مدير مشاريع تعليم إلكتروني</strong>
               <span class=""text-muted small"">إدارة المشاريع الرقمية في القطاعين العام والخاص.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd8-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل وثيقة الخطة الدراسية لبرنامج التعلم الإلكتروني عن بعد (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التعلم%20الالكتروني%20(عن%20بعد).pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd8En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Education</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">Online / Distance Learning</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-award-02 text-primary""></i><span>First in Arab Universities</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-laptop-programming text-primary""></i><span>Postgraduate Diploma in E-Learning (Virtual)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         Classified as the first online postgraduate diploma program in Arab universities, offering graduates a distinguished competitive advantage to master professional practices and explore cutting-edge e-learning technologies.
      </p>
   </div>

   <section aria-labelledby=""pgd8-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-user-star-01 fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Digital Learning Competence</h3>
               <p class=""small text-body-secondary mb-0"">Equip educational leaders with expertise in designing and managing scalable virtual classrooms and interactive digital platforms.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-global-education fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Leadership in Remote Education</h3>
               <p class=""small text-body-secondary mb-0"">Foster innovative solutions for synchronous and asynchronous distance education aligned with global digital transformation benchmarks.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Coursework Only</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">Arabic</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Male & Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">12,960 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's degree in any discipline from a recognized university.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Minimum GPA:</strong> A minimum grade of ""Good"" (C) in the undergraduate degree.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd8-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd8-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Instructional Designer</strong>
               <span class=""text-muted small"">Design comprehensive virtual courses and interactive paths.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">E-Learning Content Developer</strong>
               <span class=""text-muted small"">Develop interactive assessments and multimedia learning assets.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Online Project Manager</strong>
               <span class=""text-muted small"">Manage institutional and corporate digital learning projects.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd8-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd8-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved distance-learning curriculum and semester breakdown (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التعلم%20الالكتروني%20(عن%20بعد).pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion

        #region Program 3: EMI (English as a Medium of Instruction)

        private static string BuildPgd4Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية التربية والتنمية البشرية</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">تدريس باللغة الإنجليزية</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-global""></i><span>المدارس الدولية والأهلية</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-book-open-02 text-primary""></i><span>الدبلوم العالي في التدريس باللغة الإنجليزية (EMI)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         برنامج تخصصي موجه لخريجات تخصصات العلوم والرياضيات واللغة الإنجليزية، يهدف إلى إعدادهن وتأهيلهن لمهنة التدريس في المدارس العالمية والأهلية والبرامج الدولية من خلال دمج المقررات الأكاديمية التخصصية مع الخبرات والتطبيقات الميدانية.
      </p>
   </div>

   <section aria-labelledby=""pgd4-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-school fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">التأهيل للتعليم الدولي</h3>
               <p class=""small text-body-secondary mb-0"">إعداد المعلمات للتدريس بكفاءة في المدارس العالمية والأهلية والبرامج الأكاديمية الدولية المعتمدة.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-translate fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">التمكن التربوي ثنائي اللغة</h3>
               <p class=""small text-body-secondary mb-0"">إتقان طرق تدريس العلوم والرياضيات باللغة الإنجليزية وفق أحدث الاستراتيجيات التربوية العالمية المعاصرة.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات وتدريب ميداني</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة الإنجليزية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">18,600 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل العلمي:</strong> درجة البكالوريوس بتقدير عام لا يقل عن ""جيد"" (في العلوم، الرياضيات، أو اللغة الإنجليزية).</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">كفاءة اللغة الإنجليزية:</strong> الحصول على درجة (5.5) في اختبار IELTS Academic أو ما يعادلها في الاختبارات المعتمدة (مع إمكانية الإعفاء لمن لغتهم الأم الإنجليزية أو تخرجوا من دول ناطقة بها).</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المقابلة الشخصية:</strong> اجتياز المقابلة الشخصية المعتمدة من الكلية.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd4-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-6"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">معلمات في المدارس العالمية والأهلية</strong>
               <span class=""text-muted small"">تدريس المناهج الدولية باللغة الإنجليزية في مختلف المراحل التعليمية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">معلمات في المدارس ثنائية اللغة</strong>
               <span class=""text-muted small"">تدريس مواد العلوم والرياضيات واللغة الإنجليزية في البيئات التعليمية المزدوجة.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd4-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل وثيقة الخطة الدراسية المعتمدة لبرنامج التدريس بالإنجليزية (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/Deanship/postgraduate/PublishingImages/Pages/Plan/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التدريس%20باللغة%20الإنجليزية.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd4En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Education</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">English as a Medium of Instruction</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-global""></i><span>International & Private Schools</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-book-open-02 text-primary""></i><span>Postgraduate Diploma in English as a Medium of Instruction (EMI)</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         A specialized program designed for graduates in Science, Mathematics, and English Language fields, preparing them for teaching careers in international private schools and international diploma programs through intensive coursework combined with field practicum.
      </p>
   </div>

   <section aria-labelledby=""pgd4-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-school fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">International Teaching Preparedness</h3>
               <p class=""small text-body-secondary mb-0"">Prepare highly qualified educators capable of delivering international curricula in English across prestigious educational institutions.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-translate fs-4 text-primary""></i>
               </div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Bilingual Pedagogical Excellence</h3>
               <p class=""small text-body-secondary mb-0"">Master contemporary pedagogical methods for teaching Science and Mathematics through English with immersive field practice.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Coursework & Practicum</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">English</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">18,600 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's degree with a minimum grade of ""Good"" (C) in Science, Mathematics, or English.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">English Proficiency:</strong> Minimum score of (5.5) in IELTS Academic or equivalent (exemptions apply to native English graduates).</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Interview:</strong> Successfully passing the official admissions interview.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd4-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd4-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-6"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Teachers in International & Private Schools</strong>
               <span class=""text-muted small"">Teaching STEM and humanities in English for global diploma tracks.</span>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Teachers in Bilingual Schools</strong>
               <span class=""text-muted small"">Delivering bilingual curricula and international baccalaureate programs.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd4-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd4-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved academic plan for EMI program (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/Deanship/postgraduate/PublishingImages/Pages/Plan/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20التدريس%20باللغة%20الإنجليزية.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion

        #region Program 4: Applied Behavior Analysis

        private static string BuildPgd5Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية التربية والتنمية البشرية</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">رعاية طيف التوحد</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-heart-pulse text-primary""></i><span>ممارسات سلوكية معتمدة</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-user-check-01 text-primary""></i><span>الدبلوم العالي في تحليل السلوك التطبيقي</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         استُحدث البرنامج لتلبية الاحتياجات الوطنية والدولية لتخريج أخصائيات متخصصات لمساعدة الأطفال ذوي اضطراب طيف التوحد والإعاقات النمائية والسلوكية على تحقيق الاستقلالية والدمج المجتمعي وفق رؤية المملكة 2030 وأحدث الممارسات القائمة على الأدلة.
      </p>
   </div>

   <section aria-labelledby=""pgd5-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-brain fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">المفاهيم والخطط العلاجية</h3>
               <p class=""small text-body-secondary mb-0"">تزويد الخريجات بالنظريات والخطط القائمة على الأدلة في تحليل السلوك التطبيقي.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-analytics-01 fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">التفكير والتصميم التجريبي</h3>
               <p class=""small text-body-secondary mb-0"">تنمية مهارات التفكير الناقد لتصميم الخطط التدخلية والتجريبية السلوكية المتقدمة.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-user-group fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">الكفاءات والخدمة المجتمعية</h3>
               <p class=""small text-body-secondary mb-0"">المشاركة الفاعلة في تقديم الخدمات التعليمية والمجتمعية لذوي الإعاقات النمائية والتوحد.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات دراسية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة العربية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">16,200 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل العلمي:</strong> درجة البكالوريوس في أي تخصص بتقدير لا يقل عن ""جيد"" (مسار متخصص لخريجات التربية الخاصة وعلم النفس).</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">كفاءة اللغة:</strong> الحصول على درجة (4) كحد أدنى في اختبار IELTS أو ما يعادله.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المقابلة:</strong> اجتياز المقابلة الشخصية.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd5-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">محلل ومدرب سلوك</strong>
               <span class=""text-muted small"">تصميم وتطبيق البرامج السلوكية الفردية.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">أخصائي توحد</strong>
               <span class=""text-muted small"">مدارس التعليم العام والخاص والدولي.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">مراكز الرعاية والتأهيل</strong>
               <span class=""text-muted small"">مراكز الرعاية النهارية والتوحد بوزارة الموارد البشرية والقطاع الصحي.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd5-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل الخطة الدراسية المعتمدة لتحليل السلوك التطبيقي (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20تحليل%20السلوك%20التطبيقي.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd5En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Education</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">Applied Behavior Analysis</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-heart-pulse text-primary""></i><span>Evidence-Based Practices</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-user-check-01 text-primary""></i><span>Postgraduate Diploma in Applied Behavior Analysis</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         Established to address national and international demands by graduating specialized practitioners who support children with Autism Spectrum Disorder (ASD), developmental delays, and behavioral disorders toward daily independence and peer integration.
      </p>
   </div>

   <section aria-labelledby=""pgd5-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-brain fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Evidence-Based Theories</h3>
               <p class=""small text-body-secondary mb-0"">Provide in-depth knowledge of ABA principles, ethical guidelines, and proven intervention methodologies.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-analytics-01 fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Clinical & Behavioral Design</h3>
               <p class=""small text-body-secondary mb-0"">Develop competencies to design individualized single-subject experimental designs and behavioral plans.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6 col-lg-4"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-user-group fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Community Impact</h3>
               <p class=""small text-body-secondary mb-0"">Actively deliver therapeutic, educational, and family support services for individuals with autism.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Coursework</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">Arabic</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">16,200 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's degree with a minimum grade of ""Good"" (C), tailored for Special Education & Psychology majors.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Language Requirement:</strong> Minimum score of (4.0) in IELTS Academic or equivalent.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Interview:</strong> Successfully passing the admissions interview.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd5-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd5-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Behavior Analyst / Trainer</strong>
               <span class=""text-muted small"">Conduct functional behavior assessments and behavioral intervention.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Autism Specialist</strong>
               <span class=""text-muted small"">Schools under Ministry of Education (public, private, and international).</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Rehabilitation Centers</strong>
               <span class=""text-muted small"">Daycare centers and healthcare-affiliated autism treatment centers.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd5-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd5-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved academic curriculum for ABA program (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20تحليل%20السلوك%20التطبيقي.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion

        #region Program 5: French for Tourism

        private static string BuildPgd6Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية اللغات</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">شراكة مع وزارة السياحة</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-building-03""></i><span>الهيئة السعودية للسياحة</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-flag-02 text-primary""></i><span>الدبلوم العالي باللغة الفرنسية للسياحة</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         يُعد البرنامج الأول من نوعه في الجامعات السعودية الذي يطرح اللغة الفرنسية للأغراض السياحية تماشياً مع مستهدفات قطاع السياحة أحد ركائز الاستثمار في رؤية المملكة 2030، ويسهم في خلق فرص واعدة للخريجين ورفع تنافسيتهم بشراكة استراتيجية مع وزارة السياحة والهيئة السعودية للسياحة.
      </p>
   </div>

   <section aria-labelledby=""pgd6-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-translate fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">تأهيل الكوادر السياحية بالفرنسية</h3>
               <p class=""small text-body-secondary mb-0"">إعداد وتأهيل كوادر وطنية متخصصة تمتلك المهارات اللغوية والمهنية في الفرنسية لتلبية احتياجات قطاع السياحة وفق المعايير العالمية.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-compass fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">دعم منظومة السياحة والضيافة</h3>
               <p class=""small text-body-secondary mb-0"">تزويد القطاع السياحي بمرشدين ومسوقين قادرين على تقديم تجربة استثنائية لزوار المملكة من الناطقين باللغة الفرنسية.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات ومشروع</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة الفرنسية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطلاب والطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">22,400 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل:</strong> درجة البكالوريوس في (اللغة الفرنسية والترجمة أو اللغة الفرنسية) أو درجة البكالوريوس في أي تخصص لمن لغتهم الأم الفرنسية.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">الاختبارات:</strong> اجتياز اختبار DELF واختبار القدرات العامة للجامعيين.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المسوغات:</strong> اجتياز المقابلة الشخصية، تقديم سيرة ذاتية (CV)، وتوفر خبرة عملية ذات علاقة.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd6-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجين والخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">الإرشاد والضيافة</strong>
               <span class=""text-muted small"">الإرشاد السياحي المتخصص وإدارة الفنادق والمنتجعات.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">التسويق والفعاليات</strong>
               <span class=""text-muted small"">التسويق السياحي، تنظيم الفعاليات والمعارض ووكالات السفر.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">المتاحف والتراث</strong>
               <span class=""text-muted small"">إدارة المتاحف والمواقع السياحية والتراثية الوطنية.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd6-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل وثيقة الخطة الدراسية للدبلوم العالي بالفرنسية للسياحة (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/__الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20باللغة%20الفرنسية%20للسياحة.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd6En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Languages</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">Ministry of Tourism Partnership</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-building-03""></i><span>Saudi Tourism Authority</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-flag-02 text-primary""></i><span>Postgraduate Diploma in French for Tourism</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         The first program of its kind across Saudi universities offering French for tourism purposes. Tailored to the burgeoning tourism ecosystem under Saudi Vision 2030, enhancing graduate employability in partnership with the Ministry of Tourism and the Saudi Tourism Authority.
      </p>
   </div>

   <section aria-labelledby=""pgd6-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-translate fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">French Tourism Qualification</h3>
               <p class=""small text-body-secondary mb-0"">Prepare professionally certified specialists proficient in French language skills across diverse tourism and hospitality disciplines.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-compass fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Global Tourism Competitiveness</h3>
               <p class=""small text-body-secondary mb-0"">Empower the Kingdom's tourism ecosystem with high-caliber bilingual tour guides, destination marketers, and hospitality leaders.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Coursework & Project</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">French</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Male & Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">22,400 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's in French Language & Translation or French Language (or any major for native French speakers).</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Examinations:</strong> Passing the DELF (Diplôme d'Etudes en Langue Française) and General Aptitude Test for graduates.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Experience & Interview:</strong> Relevant work experience, Curriculum Vitae (CV), and passing the personal interview.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd6-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd6-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Tour Guiding & Hospitality</strong>
               <span class=""text-muted small"">Certified French tour guiding and hotel management.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Tourism Marketing & Events</strong>
               <span class=""text-muted small"">Travel agencies, exhibition planning, and destination marketing.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Museums & Heritage Sites</strong>
               <span class=""text-muted small"">Managing heritage attractions and cultural visitor centers.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd6-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd6-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved academic curriculum for French for Tourism (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/__الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20باللغة%20الفرنسية%20للسياحة.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion

        #region Program 6: English for Tourism

        private static string BuildPgd7Ar()
        {
            return @"<div dir=""rtl"" lang=""ar-SA"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">الدبلوم العالي</span>
            <span class=""badge bg-light text-primary border border-primary"">كلية اللغات</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">شراكة مع وزارة السياحة</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-building-03""></i><span>الهيئة السعودية للسياحة</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-translate text-primary""></i><span>الدبلوم العالي في اللغة الإنجليزية للسياحة</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         البرنامج الأول من نوعه في الجامعات السعودية الذي يقدم اللغة الإنجليزية المتخصصة في قطاع السياحة والضيافة، ملبيًا احتياجات سوق العمل السياحي المزدهر في المملكة، والمقدم بشراكة استراتيجية مع وزارة السياحة والهيئة السعودية للسياحة.
      </p>
   </div>

   <section aria-labelledby=""pgd7-goals-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-goals-title"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>أهداف البرنامج</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-global fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">الاحترافية اللغوية السياحية</h3>
               <p class=""small text-body-secondary mb-0"">إعداد كوادر مؤهلة مهنياً بمهارات اللغة الإنجليزية المتقدمة في مختلف مسارات السياحة والضيافة وفق المعايير الدولية.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-champion fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">تمكين التنافسية في سوق العمل</h3>
               <p class=""small text-body-secondary mb-0"">تزويد خريجي وخريجات اللغة الإنجليزية بمهارات الإدارة والتسويق السياحي والإرشاد لتعزيز فرصهم في القطاعات الواعدة.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-details-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-details-title"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>معلومات وبيانات البرنامج</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">نظام الدراسة</span>
               <strong class=""text-dark fs-6"">مقررات ومشروع بحثي</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">لغة البرنامج</span>
               <strong class=""text-dark fs-6"">اللغة الإنجليزية</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">الفئة المستهدفة</span>
               <strong class=""text-dark fs-6"">الطلاب والطالبات</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">التكلفة الإجمالية</span>
               <strong class=""text-primary fs-6 font-monospace"">22,400 ريال</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-req-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-req-title"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>شروط القبول</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المؤهل:</strong> درجة البكالوريوس في أقسام اللغة الإنجليزية (ترجمة، لغويات، أدب) أو إتقان تام/لغة أم لحملة البكالوريوس في التخصصات الأخرى.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">كفاءة اللغة:</strong> درجة لا تقل عن (4.5) في اختبار IELTS Academic أو ما يعادله.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">المسوغات:</strong> اجتياز المقابلة الشخصية، خبرة عملية ذات صلة، واجتياز اختبار القدرات العامة للجامعيين.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd7-career-title"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-career-title"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>الفرص الوظيفية للخريجين والخريجات</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">الإرشاد والضيافة</strong>
               <span class=""text-muted small"">الإرشاد السياحي الدولي وإدارة الفنادق والمنتجعات.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">التسويق وتنظيم الفعاليات</strong>
               <span class=""text-muted small"">التسويق السياحي الدولي وإدارة المعارض والفعاليات.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">وكالات السفر والمتاحف</strong>
               <span class=""text-muted small"">وكالات السفر والسياحة والمتاحف والمواقع التراثية الكبرى.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-plan-title"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd7-plan-title"">الخطة الدراسية للبرنامج</h3>
                  <p class=""small text-body-secondary mb-0"">تحميل الخطة الدراسية لبرنامج الإنجليزية للسياحة (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20اللغة%20الانجليزية%20للسياحة.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""تحميل الخطة الدراسية"">
               <span>تحميل الخطة (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        private static string BuildPgd7En()
        {
            return @"<div dir=""ltr"" lang=""en-US"" class=""d-flex flex-column gap-5 text-start"">
   <div class=""card border-0 bg-primary-25 rounded-3 p-4"">
      <div class=""d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-3"">
         <div class=""d-flex align-items-center gap-2"">
            <span class=""badge bg-primary text-white"">Postgraduate Diploma</span>
            <span class=""badge bg-light text-primary border border-primary"">College of Languages</span>
            <span class=""badge bg-success-subtle text-success-emphasis border border-success-subtle"">Ministry of Tourism Partnership</span>
         </div>
         <div class=""d-flex align-items-center gap-2 text-muted small"">
            <i class=""hgi hgi-stroke hgi-building-03""></i><span>Saudi Tourism Authority</span>
         </div>
      </div>
      <h1 class=""h3 fw-bold text-dark mb-3 d-flex align-items-center gap-2"">
         <i class=""hgi hgi-stroke hgi-translate text-primary""></i><span>Postgraduate Diploma in English for Tourism</span>
      </h1>
      <p class=""text-body-secondary leading-relaxed mb-0"">
         The first program of its kind in Saudi universities delivering specialized English for tourism and hospitality. Aligned with Saudi Vision 2030 investment goals and offered in direct partnership with the Ministry of Tourism and the Saudi Tourism Authority.
      </p>
   </div>

   <section aria-labelledby=""pgd7-goals-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-goals-title-en"">
         <i class=""hgi hgi-stroke hgi-target-02 text-primary fs-4"" aria-hidden=""true""></i><span>Program Goals</span>
      </h2>
      <div class=""row g-4"">
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-global fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Professional Linguistic Mastery</h3>
               <p class=""small text-body-secondary mb-0"">Prepare professionally qualified cadres with advanced English communication skills across all modern tourism and hospitality sectors.</p>
            </div>
         </div>
         <div class=""col-12 col-md-6"">
            <div class=""card h-100 p-4 border rounded-3 bg-light"">
               <div class=""icon-container bg-white mb-3 shadow-sm""><i class=""hgi hgi-stroke hgi-champion fs-4 text-primary""></i></div>
               <h3 class=""h6 fw-bold text-dark mb-2"">Job Market Competitiveness</h3>
               <p class=""small text-body-secondary mb-0"">Empower graduates with tourism management, international event coordination, and tour guiding competencies.</p>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-details-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-details-title-en"">
         <i class=""hgi hgi-stroke hgi-information-circle text-primary fs-4"" aria-hidden=""true""></i><span>Program Specifications</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Study System</span>
               <strong class=""text-dark fs-6"">Coursework & Research Project</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Language</span>
               <strong class=""text-dark fs-6"">English</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Target Audience</span>
               <strong class=""text-dark fs-6"">Male & Female Students</strong>
            </div>
         </div>
         <div class=""col-6 col-md-3"">
            <div class=""card p-3 border rounded-3 text-center bg-light h-100"">
               <span class=""text-muted small d-block mb-1"">Total Tuition Cost</span>
               <strong class=""text-primary fs-6 font-monospace"">22,400 SAR</strong>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-req-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-req-title-en"">
         <i class=""hgi hgi-stroke hgi-checklist text-primary fs-4"" aria-hidden=""true""></i><span>Admission Requirements</span>
      </h2>
      <div class=""card border rounded-3 p-4"">
         <ul class=""list-unstyled d-flex flex-column gap-3 mb-0"">
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Academic Degree:</strong> Bachelor's degree in English Language departments (Translation, Linguistics, Literature) or fluency for other majors.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">English Proficiency:</strong> Minimum score of (4.5) in IELTS Academic or equivalent.</div>
            </li>
            <li class=""d-flex align-items-start gap-3"">
               <i class=""hgi hgi-stroke hgi-checkmark-circle-02 text-success fs-5 mt-1""></i>
               <div><strong class=""text-dark"">Interview & Experience:</strong> Passing the personal interview, verified work experience, and General Aptitude Test.</div>
            </li>
         </ul>
      </div>
   </section>

   <section aria-labelledby=""pgd7-career-title-en"">
      <h2 class=""h4 fw-bold text-dark mb-3 d-flex align-items-center gap-2"" id=""pgd7-career-title-en"">
         <i class=""hgi hgi-stroke hgi-briefcase-02 text-primary fs-4"" aria-hidden=""true""></i><span>Career Opportunities</span>
      </h2>
      <div class=""row g-3"">
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Tour Guiding & Hospitality</strong>
               <span class=""text-muted small"">International tour guiding and resort management.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Tourism Marketing & Events</strong>
               <span class=""text-muted small"">Event organizing, travel agencies, and digital marketing.</span>
            </div>
         </div>
         <div class=""col-12 col-md-4"">
            <div class=""card p-3 border rounded-3 bg-light h-100 text-start"">
               <strong class=""text-dark d-block mb-1"">Museums & Tourist Sites</strong>
               <span class=""text-muted small"">Museum management, heritage sites, and visitor experience centers.</span>
            </div>
         </div>
      </div>
   </section>

   <section aria-labelledby=""pgd7-plan-title-en"">
      <div class=""card nav-card border p-4 text-start rounded-3 bg-primary-25"">
         <div class=""d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3"">
            <div class=""d-flex align-items-center gap-3"">
               <div class=""icon-container bg-white flex-shrink-0 shadow-sm"">
                  <i class=""hgi hgi-stroke hgi-pdf-02 fs-3 text-primary""></i>
               </div>
               <div>
                  <h3 class=""h5 fw-bold text-dark mb-1"" id=""pgd7-plan-title-en"">Program Study Plan</h3>
                  <p class=""small text-body-secondary mb-0"">Download approved academic curriculum for English for Tourism (PDF).</p>
               </div>
            </div>
            <a class=""btn btn-primary stretched-link flex-shrink-0"" href=""/ar/RegAdm/PGD/Documents/الخطة%20الدراسية%20لبرنامج%20الدبلوم%20العالي%20في%20اللغة%20الانجليزية%20للسياحة.pdf"" target=""_blank"" rel=""noopener noreferrer"" aria-label=""Download Study Plan"">
               <span>Download Plan (PDF)</span>
               <i class=""hgi hgi-stroke hgi-download-04 ms-1""></i>
            </a>
         </div>
      </div>
   </section>
</div>";
        }

        #endregion
    }
}
