<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaAcademicKitApplicants.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaAcademicKitApplicants" %>
<%@ Import Namespace="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure" %>

<%-- المتقدمون — DGA v4 Postgraduate Academic Kit, Applicants station.
     Content is driven by the DgaPg* lists; the code-behind supplies the v4
     content as fallback whenever a list is missing or empty. Section anchor ids
     come from SectionAnchor() because academic-kit-pages.css styles
     #applicant-section-1..9 individually. --%>

<div class="bg-primary-25 py-5">
  <div class="container">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb mb-2">
        <% for (int ci = 0; ci < HeaderModel.Breadcrumbs.Count; ci++)
           {
               DgaPgLinkItem crumb = HeaderModel.Breadcrumbs[ci];
               bool isCurrent = (ci == HeaderModel.Breadcrumbs.Count - 1) || string.IsNullOrEmpty(crumb.Url); %>
          <% if (isCurrent)
             { %>
        <li class="breadcrumb-item small active" aria-current="page"><span><%= Enc(crumb.Title) %></span></li>
          <% }
             else
             { %>
        <li class="breadcrumb-item small"><a href="<%= EncUrl(crumb.Url) %>"><%= Enc(crumb.Title) %></a></li>
          <% } %>
        <% } %>
      </ol>
    </nav>
    <div class="content">
      <h2 class="mb-0"><%= Enc(HeaderModel.Title) %></h2>
      <div class="text mt-4"><%= Enc(HeaderModel.Description) %></div>
    </div>
  </div>
</div>

<main id="main-content" class="dga-main-body" tabindex="-1">
  <div class="container py-5">
    <div class="row g-4">

      <aside class="col-12 col-lg-4" data-aos="fade-up">
        <div class="toc d-flex flex-column gap-2">
          <div class="d-flex flex-column gap-2">
            <p class="toc-title"><%= Enc(GetResource("OnThisPage", "في هذه الصفحة", "On this page")) %></p>
            <p class="toc-page" id="pageTitle"><%= Enc(HeaderModel.Title) %></p>
          </div>
          <nav id="toc" class="d-flex flex-column" aria-label="<%= Enc(GetResource("ContentsOf", "محتويات ", "Contents of ") + HeaderModel.Title) %>">
            <% foreach (DgaPgLinkItem toc in TocItems)
               { %><button type="button" class="toc-item text-start border-0 bg-transparent" data-target="<%= Enc(toc.Url) %>"><span><%= Enc(toc.Title) %></span></button><% } %>
          </nav>
        </div>
      </aside>

      <article class="col-12 col-lg-8 pnu-detail-article" data-aos="fade-up">

        <%-- نبذة تعريفية --%>
        <% if (IsSectionVisible(DgaPgSectionKeys.Intro))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Intro) %>" aria-labelledby="applicant-intro-title">
          <div class="card mb-4 bg-primary-25 border-0">
            <div class="card-body p-4 p-lg-5">
              <div class="d-flex flex-column gap-3">
                <span class="icon-container"><i class="hgi hgi-stroke <%= Enc(IconOr(IntroModel.IconClass, "hgi-book-open-01")) %> fs-4"
                    aria-hidden="true"></i></span>
                <h2 id="applicant-intro-title" class="mb-0"><%= Enc(IntroModel.Title) %></h2>
                <p class="lead mb-0"><%= Enc(IntroModel.LeadText) %></p>
              </div>
            </div>
          </div>
          <% if (!string.IsNullOrEmpty(IntroModel.SubTitle))
             { %>
          <h3 class="h5"><%= Enc(IntroModel.SubTitle) %></h3>
          <% } %>
          <% foreach (string para in IntroModel.Paragraphs)
             { %>
          <p><%= Enc(para) %></p>
          <% } %>
          <% if (IntroModel.Badges.Count > 0)
             { %>
          <div class="d-flex flex-wrap gap-2"><% foreach (string badge in IntroModel.Badges)
                                                  { %><span class="badge badge-info"><%= Enc(badge) %></span><% } %>
          </div>
          <% } %>
          <% if (!string.IsNullOrEmpty(IntroModel.ReturnText))
             { %>
          <div class="d-flex gap-3 flex-wrap mt-3"><a class="btn btn-secondary"
              href="<%= EncUrl(string.IsNullOrEmpty(IntroModel.ReturnUrl) ? ResolveKitHubUrl() : IntroModel.ReturnUrl) %>"><%= Enc(IntroModel.ReturnText) %></a>
          </div>
          <% } %>
        </section>
        <% } %>

        <%-- المحور الأول: خريطة رحلة القبول --%>
        <% if (JourneySteps.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Journey))
           {
               string journeyLead = GetSectionLead(DgaPgSectionKeys.Journey, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Journey) %>" aria-labelledby="applicant-section-1-title">
          <h2 id="applicant-section-1-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Journey)) %></h2>
          <% if (!string.IsNullOrEmpty(journeyLead))
             { %>
          <p><%= Enc(journeyLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <% foreach (DgaPgJourneyStepModel step in JourneySteps)
                 { %>
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(step.IconClass, "hgi-route-03")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <div>
                      <h3 class="card-title"><%= Enc(step.Title) %></h3>
                      <p class="card-text"><%= Enc(step.Description) %></p>
                    </div>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الثاني: البرامج وشروط القبول والرسوم --%>
        <% if (ProgramCards.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Programs))
           {
               string programsLead = GetSectionLead(DgaPgSectionKeys.Programs, GetResource("ProgramsLead", "شروط القبول والرسوم تختلف بحسب البرنامج وتُحدَّث كل فصل؛ ولأن الرسوم مدرجةٌ في صفحة كل برنامج، ندلّكِ على المصادر الرسمية المحدّثة مباشرةً:", "Admission requirements and fees vary by program and are updated each term; the official, always-current sources are linked below:")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Programs) %>" aria-labelledby="applicant-section-2-title">
          <h2 id="applicant-section-2-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Programs)) %></h2>
          <% if (!string.IsNullOrEmpty(programsLead))
             { %>
          <p><%= Enc(programsLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <% foreach (DgaPgResourceCardModel card in ProgramCards)
                 { %>
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(card.IconClass, "hgi-link-square-02")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <a class="academic-kit-card-text-link" href="<%= EncUrl(card.LinkUrl) %>" target="_blank" rel="noopener">
                      <h3 class="card-title"><%= Enc(card.Title) %></h3>
                      <p class="card-text"><%= Enc(card.Description) %></p>
                    </a>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الثالث: الدورة التأهيلية للغة الإنجليزية --%>
        <% if (EnglishCourse != null && IsSectionVisible(DgaPgSectionKeys.EnglishCourse))
           {
               string englishLead = GetSectionLead(DgaPgSectionKeys.EnglishCourse, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.EnglishCourse) %>" aria-labelledby="applicant-section-3-title">
          <h2 id="applicant-section-3-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.EnglishCourse)) %></h2>
          <% if (!string.IsNullOrEmpty(englishLead))
             { %>
          <p><%= Enc(englishLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container"><span class="d-inline-flex fs-3"><i
                          class="hgi hgi-stroke <%= Enc(IconOr(EnglishCourse.IconClass, "hgi-book-open-01")) %> fs-3" aria-hidden="true"></i></span></div>
                    <div>
                      <h3 class="card-title"><%= Enc(EnglishCourse.Title) %></h3>
                      <p class="card-text"><%= Enc(EnglishCourse.Description) %></p>
                      <% foreach (string para in EnglishCourse.Paragraphs)
                         { %>
                      <p class="card-text"><%= Enc(para) %></p>
                      <% } %>
                      <% if (EnglishCourse.Bullets.Count > 0)
                         { %>
                      <ul class="mb-0 ps-3">
                        <% foreach (string bullet in EnglishCourse.Bullets)
                           { %>
                        <li><%= Enc(bullet) %></li>
                        <% } %>
                      </ul>
                      <% } %>
                    </div>
                    <% if (!string.IsNullOrEmpty(EnglishCourse.LinkUrl))
                       { %>
                    <div class="d-flex gap-3 flex-wrap"><a class="btn btn-secondary external-link"
                        href="<%= EncUrl(EnglishCourse.LinkUrl) %>" target="_blank" rel="noopener"><%= Enc(EnglishCourse.LinkText) %></a></div>
                    <% } %>
                  </div>
                </article>
              </div>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الرابع: الأسئلة الأكثر شيوعاً --%>
        <% if (FaqItems.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Faq))
           {
               string faqLead = GetSectionLead(DgaPgSectionKeys.Faq, GetResource("FaqLead", "إجاباتٌ موجزةٌ عن أبرز الاستفسارات المتكرّرة لدى طلبة الدراسات العليا.", "Brief answers to the questions postgraduate students ask most.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Faq) %>" aria-labelledby="applicant-section-4-title">
          <h2 id="applicant-section-4-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Faq)) %></h2>
          <% if (!string.IsNullOrEmpty(faqLead))
             { %>
          <p><%= Enc(faqLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="accordion accordion-flush" id="applicant-faq-1">
              <% int fi = 0;
                 foreach (DgaPgFaqItemModel faq in FaqItems)
                 {
                     fi++; %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="applicant-faq-heading-<%= fi %>">
                  <button aria-controls="applicant-faq-collapse-<%= fi %>" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#applicant-faq-collapse-<%= fi %>"
                    data-bs-toggle="collapse" type="button"><%= Enc(faq.Question) %></button>
                </p>
                <div class="accordion-collapse collapse" id="applicant-faq-collapse-<%= fi %>"
                  aria-labelledby="applicant-faq-heading-<%= fi %>" data-bs-parent="#applicant-faq-1">
                  <div class="accordion-body">
                    <p class="mb-0"><%= Enc(faq.Answer) %></p>
                  </div>
                </div>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الخامس: الحقوق والواجبات / اللوائح النظامية --%>
        <% if (Regulations.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Regulations))
           {
               string regLead = GetSectionLead(DgaPgSectionKeys.Regulations, GetResource("RegLead", "حزمة اللوائح الرسمية المعتمدة بالجامعة التي تبيّن حقوق طالبة الدراسات العليا وواجباتها — بالضغط على اللائحة تُفتح مباشرةً.", "The university's approved regulations setting out postgraduate students' rights and duties — select a regulation to open it.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Regulations) %>" aria-labelledby="applicant-section-5-title">
          <h2 id="applicant-section-5-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Regulations)) %></h2>
          <% if (!string.IsNullOrEmpty(regLead))
             { %>
          <p><%= Enc(regLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <% foreach (DgaPgResourceCardModel card in Regulations)
                 { %>
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(card.IconClass, "hgi-book-open-01")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <div>
                      <h3 class="card-title"><%= Enc(card.Title) %></h3>
                      <p class="card-text"><%= Enc(card.Description) %></p>
                    </div>
                    <% if (!string.IsNullOrEmpty(card.LinkUrl))
                       { %>
                    <div class="d-flex gap-3 flex-wrap"><a class="btn btn-primary external-link"
                        href="<%= EncUrl(card.LinkUrl) %>" target="_blank"
                        rel="noopener"><%= Enc(string.IsNullOrEmpty(card.LinkText) ? GetResource("View", "استعراض", "View") : card.LinkText) %></a></div>
                    <% } %>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور السادس: إعفاء وتخفيض الرسوم الدراسية --%>
        <% if (DiscountRows.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Discounts))
           {
               string discountLead = GetSectionLead(DgaPgSectionKeys.Discounts, GetResource("DiscountLead", "الفئات التي يشملها الإعفاء والتخفيض:", "Categories covered by the exemption and reduction:")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Discounts) %>" aria-labelledby="applicant-section-6-title">
          <h2 id="applicant-section-6-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Discounts)) %></h2>
          <% if (!string.IsNullOrEmpty(discountLead))
             { %>
          <p><%= Enc(discountLead) %></p>
          <% } %>
          <div class="kit-fees-table-wrap">
            <table class="kit-fees-table">
              <thead>
                <tr>
                  <th><%= Enc(GetResource("DiscountCol1", "نوع الخصم / الفئة المستهدفة", "Discount type / target category")) %></th>
                  <th><%= Enc(GetResource("DiscountCol2", "نسبة الخصم على إجمالي الرسوم", "Discount on total fees")) %></th>
                </tr>
              </thead>
              <tbody>
                <% foreach (DgaPgDiscountRowModel row in DiscountRows)
                   { %>
                <tr>
                  <td><%= Enc(row.Category) %></td>
                  <td><%= MultiLine(row.Percentage) %></td>
                </tr>
                <% } %>
              </tbody>
            </table>
          </div>
          <% if (DiscountNotes.Count > 0)
             { %>
          <h3 class="kit-fees-note-title"><%= Enc(GetResource("DiscountNotes", "معلومات هامة ذات علاقة بالإعفاء:", "Important information about the exemption:")) %></h3>
          <div class="kit-fees-notes">
            <% foreach (string note in DiscountNotes)
               { %>
            <p><%= Enc(note) %></p>
            <% } %>
          </div>
          <% } %>
        </section>
        <% } %>

        <%-- المحور السابع: الخدمات / المراكز --%>
        <% if (ServiceGroups.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Services))
           {
               string servicesLead = GetSectionLead(DgaPgSectionKeys.Services, GetResource("ServicesLead", "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح.", "The university services and centers you are most likely to need, linked straight to their official pages.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Services) %>" aria-labelledby="applicant-section-7-title">
          <h2 id="applicant-section-7-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Services)) %></h2>
          <% if (!string.IsNullOrEmpty(servicesLead))
             { %>
          <p><%= Enc(servicesLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <% foreach (DgaPgServiceGroupModel group in ServiceGroups)
               { %>
            <div>
              <h3 class="h5 mb-3"><%= Enc(group.GroupName) %></h3>
              <div class="row g-4">
                <% foreach (DgaPgServiceModel service in group.Services)
                   { %>
                <div class="col-12 col-lg-4 col-md-6">
                  <article class="card h-100">
                    <div class="card-body d-flex flex-column gap-4">
                      <div class="icon-container">
                        <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(service.IconClass, "hgi-building-03")) %> fs-3"
                            aria-hidden="true"></i></span>
                      </div>
                      <div>
                        <h3 class="card-title"><%= Enc(service.Title) %></h3>
                        <p class="card-text"><%= Enc(string.IsNullOrEmpty(service.Description) ? group.GroupName : service.Description) %></p>
                      </div>
                      <% if (!string.IsNullOrEmpty(service.LinkUrl))
                         { %>
                      <div class="d-flex gap-3 flex-wrap"><a class="btn btn-primary external-link"
                          href="<%= EncUrl(service.LinkUrl) %>" target="_blank"
                          rel="noopener"><%= Enc(GetResource("Go", "انتقال", "Go")) %></a></div>
                      <% } %>
                    </div>
                  </article>
                </div>
                <% } %>
              </div>
            </div>
            <% } %>
          </div>
        </section>
        <% } %>

        <%-- المحور الثامن: السكن الجامعي --%>
        <% if (HousingInfo != null && IsSectionVisible(DgaPgSectionKeys.Housing))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Housing) %>" aria-labelledby="applicant-section-8-title">
          <h2 id="applicant-section-8-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Housing)) %></h2>
          <% string housingLead = GetSectionLead(DgaPgSectionKeys.Housing, HousingInfo.Description);
             if (!string.IsNullOrEmpty(housingLead))
             { %>
          <p><%= Enc(housingLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(HousingInfo.IconClass, "hgi-building-03")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <a class="academic-kit-card-text-link" href="<%= EncUrl(HousingInfo.LinkUrl) %>" target="_blank" rel="noopener">
                      <h3 class="card-title"><%= Enc(HousingInfo.Title) %></h3>
                      <p class="card-text"><%= Enc(SectionTitle(DgaPgSectionKeys.Housing)) %></p>
                    </a>
                  </div>
                </article>
              </div>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور التاسع: تواصلي معنا --%>
        <% if (ContactInfo != null && IsSectionVisible(DgaPgSectionKeys.Contact))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Contact) %>" aria-labelledby="applicant-section-9-title">
          <h2 id="applicant-section-9-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Contact)) %></h2>
          <% string contactLead = GetSectionLead(DgaPgSectionKeys.Contact, ContactInfo.Description);
             if (!string.IsNullOrEmpty(contactLead))
             { %>
          <p><%= Enc(contactLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(ContactInfo.IconClass, "hgi-building-03")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <a class="academic-kit-card-text-link" href="<%= EncUrl(ContactInfo.LinkUrl) %>">
                      <h3 class="card-title"><%= Enc(ContactInfo.Title) %></h3>
                      <p class="card-text"><%= Enc(SectionTitle(DgaPgSectionKeys.Contact)) %></p>
                    </a>
                  </div>
                </article>
              </div>
            </div>
          </div>
        </section>
        <% } %>

        <%-- الأساس النظامي والمرجعي --%>
        <% if (!string.IsNullOrEmpty(ReferenceLead) && IsSectionVisible(DgaPgSectionKeys.Reference))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Reference) %>" aria-labelledby="applicant-reference-title"
          class="kit-reference-section">
          <div class="kit-reference-text">
            <h2 id="applicant-reference-title"><%= Enc(ReferenceTitle) %></h2>
            <p><%= Enc(ReferenceLead) %></p>
          </div>
          <a href="<%= EncUrl(ResolveKitHubUrl()) %>" class="kit-btn-prev"><%= Enc(GetResource("Previous", "السابق", "Previous")) %></a>
        </section>
        <% } %>

      </article>
    </div>
  </div>
</main>
