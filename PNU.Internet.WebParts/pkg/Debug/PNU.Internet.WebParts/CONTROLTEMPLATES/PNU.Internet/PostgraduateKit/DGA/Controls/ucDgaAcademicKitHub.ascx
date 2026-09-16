<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaAcademicKitHub.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaAcademicKitHub" %>

<style>
  .kit-home-main .kit-hero,
  .academic-kit-home .kit-hero,
  .kit-hero {
    position: relative;
    min-height: 447px;
    padding: 48px 0 68px;
    overflow: hidden;
    background: linear-gradient(108deg, #07553a 0%, #1b8354 100%) !important;
    color: #fff !important;
  }
  .kit-hero h1 {
    max-width: none;
    margin: 0 0 24px;
    color: #fff !important;
    font-size: clamp(32px, 4vw, 56px);
    font-weight: 700;
    line-height: 1.25;
  }
  .kit-hero-copy {
    max-width: 760px;
    margin-right: 0;
    margin-left: auto;
    text-align: right;
    font-size: 20px;
    line-height: 1.75;
    color: #fff !important;
  }
  .kit-hero-copy p {
    margin: 0 0 4px;
    color: #fff !important;
  }
  .kit-hero-dots {
    position: absolute;
    right: 50%;
    bottom: 16px;
    display: flex;
    gap: 10px;
    transform: translateX(50%);
  }
  .kit-hero-dots span {
    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: #d7dce1;
  }
  .kit-hero-dots span:first-child {
    background: #13a36f;
  }
  .kit-home-about {
    min-height: 450px;
    padding: 60px 0 80px;
    background: #f7fcf8;
  }
  .kit-section-heading {
    margin: 0 0 24px;
    color: #1a202c;
    font-size: 28px;
    font-weight: 700;
  }
  .kit-about-card {
    padding: 40px 50px;
    border-radius: 16px;
    background: #fff;
    box-shadow: 0 10px 30px rgba(0,0,0,0.05);
    font-size: 18px;
    line-height: 1.75;
    color: #2d3748;
  }
  .kit-categories {
    padding: 48px 0 96px;
    background: #ffffff;
  }
  .kit-categories-header {
    margin-bottom: 48px;
    text-align: center;
  }
  .kit-categories-header h2 {
    margin: 0 0 12px;
    font-size: 28px;
    font-weight: 700;
    color: #1a202c;
  }
  .kit-category-grid {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 24px;
  }
  @media (max-width: 991px) {
    .kit-category-grid {
      grid-template-columns: 1fr;
    }
    .kit-about-card {
      padding: 24px;
    }
  }
  .kit-category-card {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
    padding: 40px 28px;
    border-radius: 16px;
    background: #ffffff;
    border: 1px solid #e2e8f0;
    box-shadow: 0 4px 20px rgba(0,0,0,0.04);
    text-decoration: none;
    color: inherit;
    transition: all 0.3s ease;
  }
  .kit-category-card:hover {
    transform: translateY(-6px);
    box-shadow: 0 12px 30px rgba(7, 85, 58, 0.12);
    border-color: #07553a;
    color: inherit;
  }
  .kit-category-icon {
    margin-bottom: 20px;
  }
  .kit-category-card h3 {
    font-size: 24px;
    font-weight: 700;
    color: #07553a;
    margin-bottom: 8px;
  }
  .kit-stage {
    display: inline-block;
    padding: 4px 14px;
    border-radius: 20px;
    background: #e6f4ea;
    color: #07553a;
    font-size: 14px;
    font-weight: 600;
    margin-bottom: 16px;
  }
  .kit-category-card p {
    font-size: 15px;
    color: #4a5568;
    line-height: 1.6;
    margin: 0;
  }
</style>

<main id="main-content" class="kit-home-main academic-kit-home" tabindex="-1">
  <%-- Hero Section --%>
  <section class="kit-hero" aria-labelledby="kit-home-title">
    <div class="container">
      <h1 id="kit-home-title"><%= GetResource("HubTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students") %></h1>
      <div class="kit-hero-copy">
        <p><%= GetResource("HeroSub", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبـ/ـة من القبول حتى منح الدرجة العلمية", "A unified digital companion accompanying students from admission to graduation") %></p>
        <p><%= GetResource("HeroYear", "١٤٤٧ - ١٤٤٨هـ", "1447 - 1448 AH") %></p>
        <p><%= GetResource("HeroDeanship", "عمادة الدراسات العليا", "Deanship of Graduate Studies") %></p>
        <p><strong><%= GetResource("HeroDesignBy", "تصميم وإعداد وتنفيذ:", "Designed & Executed by:") %></strong></p>
        <p><%= GetResource("HeroAuthor", "د. ساره بنت عبدالعزيز العمار", "Dr. Sarah bint Abdulaziz Al-Ammar") %></p>
        <p><%= GetResource("HeroRole", "وكيلة عمادة الدراسات العليا للشؤون الأكاديمية", "Vice Dean of Graduate Studies for Academic Affairs") %></p>
      </div>
      <div class="kit-hero-dots" aria-hidden="true"><span></span><span></span><span></span><span></span></div>
    </div>
  </section>

  <%-- About Section --%>
  <section class="kit-home-about" aria-labelledby="kit-about-title">
    <div class="container">
      <div class="kit-about-card">
        <h2 id="kit-about-title" class="kit-section-heading"><%= GetResource("AboutHeading", "نبذة تعريفية", "Overview") %></h2>
        <p><%= GetResource("AboutP1", "تُعدّ الحقيبة الأكاديمية الذكية لطلبة الدراسات العليا إحدى مبادرات وكالة الشؤون الأكاديمية بعمادة الدراسات العليا في مسارها نحو التحوّل الرقمي وتجويد الخدمة المقدَّمة لطلبتها؛ إذ تجمع في مرجعيةٍ رقميةٍ واحدةٍ منظّمةٍ كل ما يحتاجه الطلبة من إجراءاتٍ أكاديميةٍ ولوائحَ منظّمةٍ وأدلّةٍ إرشاديةٍ ونماذجَ معتمدة، وتربطهم مباشرةً بالمصادر الرسمية في موقع العمادة لتظلّ محدّثةً على الدوام.", "The Smart Academic Kit for Postgraduate Students is an initiative of the Academic Affairs Agency at the Deanship of Graduate Studies towards digital transformation and service excellence; organizing all academic procedures, bylaws, manuals and approved forms into one single digital reference.") %></p>
        <p><%= GetResource("AboutP2", "وتنطلق هذه الحقيبة من إيمان العمادة بأنّ تيسير وصول الطلبة إلى المعلومة الصحيحة في حينها ركيزةٌ من ركائز جودة الحياة الأكاديمية؛ فجاءت لتختصر المسافة بين الطلبة وما يحتاجونه، وتوحّد مرجعيتهم، وترسّخ ثقافة الوضوح والشفافية في الإجراءات، مواكبةً لمستهدفات الجامعة ورؤية المملكة 2030 في تمكين بيئةٍ تعليميةٍ رقميةٍ متكاملة.", "This kit stems from the Deanship's belief that facilitating timely access to accurate information is essential to academic quality of life, bridging the distance between students and their requirements.") %></p>
        <p class="mb-0"><%= GetResource("AboutP3", "وتنتظم الحقيبة في ثلاث محطّاتٍ تمثّل مراحل الرحلة الأكاديمية، تنتقل إليها الطلبة بحسب موضعها من مسيرتها:", "The kit is organized into three stations representing the stages of the academic journey:") %></p>
      </div>
    </div>
  </section>

  <%-- Categories / Station Cards --%>
  <section class="kit-categories" aria-labelledby="kit-category-title">
    <div class="container">
      <header class="kit-categories-header">
        <h2 id="kit-category-title"><%= GetResource("CategoryTitle", "اختر فئتك للوصول إلى ما يخصّك", "Choose your category to access relevant resources") %></h2>
        <p><%= GetResource("CategoryLead", "تواكبك هذه الحقيبة في كل مرحلةٍ من مراحل رحلتك الأكاديمية", "This kit accompanies you through every stage of your academic journey") %></p>
      </header>
      <div class="kit-category-grid">
        <a class="kit-category-card" href="<%= ResolveStationUrl("/ar/Deanship/postgraduate/Pages/applicants.aspx", "/en/Deanship/postgraduate/Pages/applicants.aspx") %>">
          <span class="kit-category-icon"><img src="/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/applicants.png" width="122" height="122" alt="<%= GetResource("Applicants", "المتقدمون", "Applicants") %>"></span>
          <h3><%= GetResource("Applicants", "المتقدمون", "Applicants") %></h3>
          <span class="kit-stage"><%= GetResource("Stage1", "المرحلة الأولى", "Stage 1") %></span>
          <p><%= GetResource("ApplicantsDesc", "شروط القبول لكل برنامج، والمستندات المطلوبة، والدورة التأهيلية للغة الإنجليزية", "Admission requirements for each program, required documents, and English qualifying course") %></p>
        </a>
        <a class="kit-category-card" href="<%= ResolveStationUrl("/ar/Deanship/postgraduate/Pages/enrolled.aspx", "/en/Deanship/postgraduate/Pages/enrolled.aspx") %>">
          <span class="kit-category-icon"><img src="/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/enrolled.png" width="122" height="122" alt="<%= GetResource("Enrolled", "المقيدون", "Enrolled Students") %>"></span>
          <h3><%= GetResource("Enrolled", "المقيدون", "Enrolled Students") %></h3>
          <span class="kit-stage"><%= GetResource("Stage2", "المرحلة الثانية", "Stage 2") %></span>
          <p><%= GetResource("EnrolledDesc", "رحلتك الأكاديمية، حقوقك ولوائحك، السجل الإجرائي، وحافز النشر العلمي", "Your academic journey, rights, regulations, procedure directory, and publication incentive") %></p>
        </a>
        <a class="kit-category-card" href="<%= ResolveStationUrl("/ar/Deanship/postgraduate/Pages/graduates.aspx", "/en/Deanship/postgraduate/Pages/graduates.aspx") %>">
          <span class="kit-category-icon"><img src="/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/graduates.png" width="122" height="122" alt="<%= GetResource("Graduates", "الخريجون", "Graduates") %>"></span>
          <h3><%= GetResource("Graduates", "الخريجون", "Graduates") %></h3>
          <span class="kit-stage"><%= GetResource("Stage3", "المرحلة الثالثة", "Stage 3") %></span>
          <p><%= GetResource("GraduatesDesc", "إجراءات منح الدرجة وإخلاء الطرف، والوثائق، وهوية الخرّيج", "Degree conferral, university clearance, graduation documents, and graduate identity") %></p>
        </a>
      </div>
    </div>
  </section>
</main>
