<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaAcademicKitEnrolled.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaAcademicKitEnrolled" %>
<%@ Import Namespace="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure" %>

<%-- المقيدون — DGA v4 Postgraduate Academic Kit, Enrolled students station.
     Content is driven by the DgaPg* lists; the code-behind supplies the v4
     content as fallback whenever a list is missing or empty. Section anchor ids
     come from SectionAnchor() because academic-kit-pages.css styles
     #enrolled-section-1..10, #enrolled-procedure-1 and #enrolled-procedure-collapse-1/-2
     individually. --%>

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
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Intro) %>" aria-labelledby="enrolled-intro-title">
          <div class="card mb-4 bg-primary-25 border-0">
            <div class="card-body p-4 p-lg-5">
              <div class="d-flex flex-column gap-3">
                <span class="icon-container"><i class="hgi hgi-stroke <%= Enc(IconOr(IntroModel.IconClass, "hgi-book-open-01")) %> fs-4"
                    aria-hidden="true"></i></span>
                <h2 id="enrolled-intro-title" class="mb-0"><%= Enc(IntroModel.Title) %></h2>
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

        <%-- المحور الأول: خريطة الرحلة الأكاديمية --%>
        <% if (JourneySteps.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Journey))
           {
               string journeyLead = GetSectionLead(DgaPgSectionKeys.Journey, GetResource("JourneyLead", "خط سير طالبة الدراسات العليا المقيَّدة في خمس مراحلَ متتابعة، من تسجيل الجدول الدراسي حتى منح الدرجة العلمية.", "The enrolled postgraduate student's path across five consecutive stages, from schedule registration to degree conferral.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Journey) %>" aria-labelledby="enrolled-section-1-title">
          <h2 id="enrolled-section-1-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Journey)) %></h2>
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
                      <h3 class="card-title"><% if (step.StepNumber > 0)
                                                 { %><span class="kit-phase-badge"><%= Enc(GetResource("Stage", "المرحلة ", "Stage ") + step.StepNumber) %></span> <% } %><%= Enc(step.Title) %></h3>
                      <p class="card-text"><%= Enc(step.Description) %></p>
                    </div>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
            <% string journeyNote = GetSectionNote(DgaPgSectionKeys.Journey);
               if (!string.IsNullOrEmpty(journeyNote))
               { %>
            <p class="small text-muted mb-0"><%= Enc(journeyNote) %></p>
            <% } %>
          </div>
        </section>
        <% } %>

        <%-- المحور الثاني: الحقوق والواجبات / اللوائح النظامية --%>
        <% if ((Regulations.Count > 0 || GraduateIdentity != null || FinancialRules != null) && IsSectionVisible(DgaPgSectionKeys.Regulations))
           {
               string regLead = GetSectionLead(DgaPgSectionKeys.Regulations, GetResource("RegLead", "حزمة اللوائح الرسمية المعتمدة بالجامعة التي تبيّن حقوق طالبة الدراسات العليا وواجباتها — بالضغط على اللائحة تُفتح مباشرةً.", "The university's approved regulations setting out postgraduate students' rights and duties — select a regulation to open it.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Regulations) %>" aria-labelledby="enrolled-section-2-title">
          <h2 id="enrolled-section-2-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Regulations)) %></h2>
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

            <%-- The two charter accordions. collapse-1 / collapse-2 are styled individually
                 by academic-kit-pages.css, so each block keeps its own fixed id. --%>
            <div class="accordion accordion-flush" id="enrolled-procedure-1">
              <% if (GraduateIdentity != null)
                 { %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="enrolled-procedure-heading-1">
                  <button aria-controls="enrolled-procedure-collapse-1" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#enrolled-procedure-collapse-1"
                    data-bs-toggle="collapse" type="button"><span class="kit-procedure-btn-title"><%= Enc(GraduateIdentity.Title) %></span><% if (!string.IsNullOrEmpty(GraduateIdentity.BadgeText))
                                                                                                                                                 { %><span class="kit-procedure-badge"><%= Enc(GraduateIdentity.BadgeText) %></span><% } %></button>
                </p>
                <div class="accordion-collapse collapse" id="enrolled-procedure-collapse-1"
                  aria-labelledby="enrolled-procedure-heading-1" data-bs-parent="#enrolled-procedure-1">
                  <div class="accordion-body">
                    <div class="kit-graduate-identity-content">
                      <p class="kit-graduate-identity-intro"><%= Enc(GraduateIdentity.Description) %></p>
                      <div class="kit-graduate-identity-grid">
                        <% foreach (DgaPgArticleBlock trait in GraduateIdentity.Groups)
                           { %>
                        <section class="kit-graduate-identity-trait">
                          <h3><%= Enc(trait.Heading) %>:</h3>
                          <div class="kit-graduate-identity-tags">
                            <% foreach (string tag in trait.Body.Split(';'))
                               {
                                   string t = tag.Trim();
                                   if (t.Length == 0) continue; %>
                            <span><%= Enc(t) %></span>
                            <% } %>
                          </div>
                        </section>
                        <% } %>
                      </div>
                    </div>
                    <p class="bg-primary-25 rounded-2 p-3"><%= Enc(GraduateIdentity.Description) %></p>
                    <div class="row g-4">
                      <% foreach (DgaPgArticleBlock trait in GraduateIdentity.Groups)
                         { %>
                      <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                          <div class="card-body d-flex flex-column gap-4">
                            <div class="icon-container"><span class="d-inline-flex fs-3"><i
                                  class="hgi hgi-stroke <%= Enc(IconOr(GraduateIdentity.IconClass, "hgi-checkmark-badge-01")) %> fs-3" aria-hidden="true"></i></span>
                            </div>
                            <div>
                              <h3 class="card-title"><%= Enc(trait.Heading) %></h3>
                              <p class="card-text"><%= Enc(trait.Body.Replace(";", "،")) %></p>
                            </div>
                          </div>
                        </article>
                      </div>
                      <% } %>
                    </div>
                  </div>
                </div>
              </div>
              <% } %>
              <% if (FinancialRules != null)
                 { %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="enrolled-procedure-heading-2">
                  <button aria-controls="enrolled-procedure-collapse-2" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#enrolled-procedure-collapse-2"
                    data-bs-toggle="collapse" type="button"><span class="kit-procedure-btn-title"><%= Enc(FinancialRules.Title) %></span><% if (!string.IsNullOrEmpty(FinancialRules.BadgeText))
                                                                                                                                               { %><span class="kit-procedure-badge"><%= Enc(FinancialRules.BadgeText) %></span><% } %></button>
                </p>
                <div class="accordion-collapse collapse" id="enrolled-procedure-collapse-2"
                  aria-labelledby="enrolled-procedure-heading-2" data-bs-parent="#enrolled-procedure-1">
                  <div class="accordion-body">
                    <div class="kit-financial-regulations-content">
                      <header class="kit-financial-regulations-intro">
                        <h3><%= Enc(FinancialRules.Title) %></h3>
                        <p><%= Enc(FinancialRules.Description) %></p>
                      </header>
                      <div class="kit-financial-regulations-list">
                        <% foreach (DgaPgArticleBlock rule in FinancialRules.Groups)
                           { %>
                        <article>
                          <h4><%= Enc(rule.Heading) %></h4>
                          <p><%= Enc(rule.Body) %></p>
                        </article>
                        <% } %>
                      </div>
                      <% if (!string.IsNullOrEmpty(FinancialRules.NoteText))
                         { %>
                      <p class="kit-financial-regulations-note"><%= Enc(FinancialRules.NoteText) %></p>
                      <% } %>
                    </div>
                    <p class="bg-primary-25 rounded-2 p-3"><%= Enc(FinancialRules.Description) %></p>
                    <div class="row g-4">
                      <% int ri = 0;
                         foreach (DgaPgArticleBlock rule in FinancialRules.Groups)
                         {
                             ri++; %>
                      <div class="col-12 col-lg-4 col-md-6">
                        <article class="card h-100">
                          <div class="card-body d-flex flex-column gap-4">
                            <div class="icon-container"><span class="d-inline-flex fs-3"><i
                                  class="hgi hgi-stroke <%= Enc(IconOr(FinancialRules.IconClass, "hgi-chart")) %> fs-3" aria-hidden="true"></i></span></div>
                            <div>
                              <h3 class="card-title"><%= Enc(rule.Heading) %></h3>
                              <p class="card-text"><%= Enc(rule.Body) %></p>
                            </div>
                            <div class="d-flex flex-wrap mt-auto gap-2"><span class="badge badge-info"><%= ri %></span></div>
                          </div>
                        </article>
                      </div>
                      <% } %>
                    </div>
                  </div>
                </div>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الثالث: الإجراءات الأكاديمية --%>
        <% if (Procedures.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Procedures))
           {
               string procLead = GetSectionLead(DgaPgSectionKeys.Procedures, GetResource("ProcLead", "إجراءاتٌ مرتّبةٌ تصاعدياً وفق رقم المادة. بالضغط على الإجراء يتوسّع ليعرض شروطه ونصّ مادته والنماذج المرتبطة من موقع العمادة.", "Procedures ordered by article number. Selecting one expands its conditions, article text and related forms."));
               string[] subGroups = new string[] { "College", "Student" };
               int subIndex = 0; %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Procedures) %>" aria-labelledby="enrolled-section-3-title">
          <h2 id="enrolled-section-3-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Procedures)) %></h2>
          <% if (!string.IsNullOrEmpty(procLead))
             { %>
          <p><%= Enc(procLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-3">
            <% foreach (string subGroup in subGroups)
               {
                   List<DgaPgProcedureModel> group = ProceduresIn(subGroup);
                   if (group.Count == 0) continue;
                   subIndex++;
                   string accordionId = "enrolled-procedures-3-" + subIndex; %>
            <h3 class="kit-procedure-subgroup-title<%= subIndex > 1 ? " mt-4" : "" %>"><%= Enc(SubGroupTitle(subGroup)) %></h3>
            <div class="accordion accordion-flush kit-procedures-accordion" id="<%= accordionId %>">
              <div class="row g-3 kit-procedures-grid">
                <% foreach (DgaPgProcedureModel proc in group)
                   {
                       string pid = "enrolled-procedure-" + proc.Id; %>
                <div class="col-12 col-lg-6">
                  <div class="accordion-item">
                    <p class="accordion-header mb-0" id="<%= pid %>-heading">
                      <button aria-controls="<%= pid %>-collapse" aria-expanded="false"
                        class="accordion-button collapsed" data-bs-target="#<%= pid %>-collapse"
                        data-bs-toggle="collapse" type="button"><% if (!string.IsNullOrEmpty(proc.IconUrl))
                                                                    { %><span class="kit-procedure-icon"><img
                            src="<%= EncUrl(proc.IconUrl) %>" alt="" aria-hidden="true"></span><% } %><span
                          class="kit-procedure-btn-title"><%= Enc(proc.Title) %></span><% if (!string.IsNullOrEmpty(proc.BadgeText))
                                                                                           { %><span class="kit-procedure-badge"><%= Enc(proc.BadgeText) %></span><% } %></button>
                    </p>
                    <div class="accordion-collapse collapse" id="<%= pid %>-collapse"
                      aria-labelledby="<%= pid %>-heading" data-bs-parent="#<%= accordionId %>">
                      <div class="accordion-body">
                        <% if (!string.IsNullOrEmpty(proc.ShortDescription))
                           { %>
                        <p class="bg-primary-25 rounded-2 p-3"><%= Enc(proc.ShortDescription) %></p>
                        <% } %>
                        <% if (proc.Requirements.Count > 0)
                           { %>
                        <h4 class="h6 text-primary mt-4 mb-2"><%= Enc(GetResource("ProcConditions", "الشروط والمتطلبات", "Conditions and Requirements")) %></h4>
                        <ul class="mb-0 ps-3">
                          <% foreach (string requirement in proc.Requirements)
                             { %>
                          <li><%= Enc(requirement) %></li>
                          <% } %>
                        </ul>
                        <% } %>
                        <% if (proc.Articles.Count > 0)
                           { %>
                        <h4 class="h6 text-primary mt-4 mb-2"><%= Enc(GetResource("ProcArticle", "نصّ المادة من اللائحة", "Article Text from the Regulation")) %></h4>
                        <% foreach (DgaPgArticleBlock article in proc.Articles)
                           { %>
                        <div class="border rounded-2 p-3 mb-3">
                          <h4 class="h6 text-primary"><%= Enc(article.Heading) %></h4>
                          <p class="mb-0"><%= Enc(article.Body) %></p>
                        </div>
                        <% } %>
                        <% } %>
                        <% if (proc.Forms.Count > 0)
                           { %>
                        <h4 class="h6 text-primary mt-4 mb-2"><%= Enc(GetResource("ProcForms", "النماذج المرتبطة", "Related Forms")) %></h4>
                        <div class="d-flex flex-wrap gap-2">
                          <% foreach (DgaPgLinkItem form in proc.Forms)
                             { %><a class="btn btn-secondary external-link" href="<%= EncUrl(form.Url) %>" target="_blank"
                            rel="noopener"><%= Enc(form.Title) %></a><% } %>
                        </div>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(proc.DirectServiceUrl))
                           { %>
                        <h4 class="h6 text-primary mt-4 mb-2"><%= Enc(GetResource("ApplyForProcedure", "للتقديم على الإجراء", "To apply for this procedure")) %></h4>
                        <div class="d-flex flex-wrap gap-2"><a class="btn btn-secondary external-link"
                            href="<%= EncUrl(proc.DirectServiceUrl) %>" target="_blank"
                            rel="noopener"><%= Enc(GetResource("ServiceLink", "رابط الخدمة", "Service link")) %></a></div>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(proc.FlowText))
                           { %>
                        <p class="border rounded-2 p-3 mb-0"><%= Enc(proc.FlowText) %></p>
                        <% } %>
                      </div>
                    </div>
                  </div>
                </div>
                <% } %>
              </div>
            </div>
            <% } %>
          </div>
        </section>
        <% } %>

        <%-- المحور الرابع: الأسئلة الأكثر شيوعاً --%>
        <% if (FaqItems.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Faq))
           {
               string faqLead = GetSectionLead(DgaPgSectionKeys.Faq, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Faq) %>" aria-labelledby="enrolled-section-4-title">
          <h2 id="enrolled-section-4-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Faq)) %></h2>
          <% if (!string.IsNullOrEmpty(faqLead))
             { %>
          <p><%= Enc(faqLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="accordion accordion-flush" id="enrolled-faq-3">
              <% int fi = 0;
                 foreach (DgaPgFaqItemModel faq in FaqItems)
                 {
                     fi++; %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="enrolled-faq-heading-<%= fi %>">
                  <button aria-controls="enrolled-faq-collapse-<%= fi %>" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#enrolled-faq-collapse-<%= fi %>"
                    data-bs-toggle="collapse" type="button"><%= Enc(faq.Question) %></button>
                </p>
                <div class="accordion-collapse collapse" id="enrolled-faq-collapse-<%= fi %>"
                  aria-labelledby="enrolled-faq-heading-<%= fi %>" data-bs-parent="#enrolled-faq-3">
                  <div class="accordion-body">
                    <p class="mb-0"><%= Enc(faq.Answer) %></p>
                  </div>
                </div>
              </div>
              <% } %>
            </div>
            <% string faqNote = GetSectionNote(DgaPgSectionKeys.Faq);
               if (!string.IsNullOrEmpty(faqNote))
               {
                   string[] noteParts = faqNote.Split('|'); %>
            <div class="kit-contact-banner">
              <p class="mb-0"><%= Enc(noteParts[0]) %><% if (noteParts.Length >= 3)
                                                          { %> <a href="<%= EncUrl(noteParts[1]) %>"><%= Enc(noteParts[2]) %></a><% } %></p>
            </div>
            <% } %>
          </div>
        </section>
        <% } %>

        <%-- المحور الخامس: حافز النشر العلمي --%>
        <% if (PublicationIncentive != null && IsSectionVisible(DgaPgSectionKeys.PublishIncentive))
           {
               string pubLead = GetSectionLead(DgaPgSectionKeys.PublishIncentive, GetResource("PubLead", "مبادرةٌ تحفيزيةٌ لدعم المسيرة البحثية لطلبة الدراسات العليا.", "An initiative supporting the research path of postgraduate students.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.PublishIncentive) %>" aria-labelledby="enrolled-section-5-title">
          <h2 id="enrolled-section-5-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.PublishIncentive)) %></h2>
          <% if (!string.IsNullOrEmpty(pubLead))
             { %>
          <p><%= Enc(pubLead) %></p>
          <% } %>
          <div class="kit-publication-card">
            <div class="kit-pub-header">
              <div>
                <p class="kit-pub-eyebrow"><%= Enc(PublicationIncentive.Description) %></p>
                <h3 class="kit-pub-title"><%= Enc(PublicationIncentive.Title) %></h3>
              </div>
              <% if (!string.IsNullOrEmpty(PublicationIncentive.BadgeText))
                 { %>
              <div class="kit-pub-badge" dir="ltr">
                <% if (!string.IsNullOrEmpty(PublicationIncentive.IconUrl))
                   { %><img class="kit-pub-badge-icon" src="<%= EncUrl(PublicationIncentive.IconUrl) %>" alt="" aria-hidden="true"><% } %>
                <span><%= Enc(PublicationIncentive.BadgeText) %></span>
              </div>
              <% } %>
            </div>
            <% if (PublicationIncentive.Paragraphs.Count > 0)
               { %>
            <div class="kit-pub-body">
              <% foreach (string para in PublicationIncentive.Paragraphs)
                 { %>
              <p><%= Enc(para) %></p>
              <% } %>
            </div>
            <% } %>
            <% if (PublicationIncentive.Bullets.Count > 0)
               { %>
            <div class="kit-pub-callouts">
              <% int bi = 0;
                 foreach (string callout in PublicationIncentive.Bullets)
                 {
                     bi++; %>
              <div class="kit-pub-callout">
                <i class="hgi hgi-stroke <%= bi == 1 ? "hgi-user-group" : "hgi-award-02" %> fs-4" aria-hidden="true"></i>
                <span><%= Enc(callout) %></span>
              </div>
              <% } %>
            </div>
            <% } %>
            <% foreach (DgaPgArticleBlock extra in PublicationIncentive.Groups)
               { %>
            <div class="kit-pub-conferences">
              <h4><%= Enc(extra.Heading) %></h4>
              <p><%= Enc(extra.Body) %></p>
            </div>
            <% } %>
          </div>
        </section>
        <% } %>

        <%-- المحور السادس: إعفاء وتخفيض الرسوم الدراسية --%>
        <% if (DiscountRows.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Discounts))
           {
               string discountLead = GetSectionLead(DgaPgSectionKeys.Discounts, GetResource("DiscountLead", "الفئات التي يشملها الإعفاء والتخفيض:", "Categories covered by the exemption and reduction:")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Discounts) %>" aria-labelledby="enrolled-section-6-title">
          <h2 id="enrolled-section-6-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Discounts)) %></h2>
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

        <%-- المحور السابع: الأدلة الإرشادية --%>
        <% if (Guides.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Guides))
           {
               string guidesLead = GetSectionLead(DgaPgSectionKeys.Guides, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Guides) %>" aria-labelledby="enrolled-section-7-title">
          <h2 id="enrolled-section-7-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Guides)) %></h2>
          <% if (!string.IsNullOrEmpty(guidesLead))
             { %>
          <p><%= Enc(guidesLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <% foreach (DgaPgResourceCardModel guide in Guides)
                 { %>
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container">
                      <span class="d-inline-flex fs-3"><i class="hgi hgi-stroke <%= Enc(IconOr(guide.IconClass, "hgi-book-open-01")) %> fs-3"
                          aria-hidden="true"></i></span>
                    </div>
                    <div>
                      <h3 class="card-title"><%= Enc(guide.Title) %></h3>
                      <p class="card-text"><%= Enc(guide.Description) %></p>
                    </div>
                    <% if (!string.IsNullOrEmpty(guide.LinkUrl))
                       { %>
                    <div class="d-flex gap-3 flex-wrap"><a class="btn btn-primary external-link"
                        href="<%= EncUrl(guide.LinkUrl) %>" target="_blank"
                        rel="noopener"><%= Enc(string.IsNullOrEmpty(guide.LinkText) ? GetResource("View", "استعراض", "View") : guide.LinkText) %></a></div>
                    <% } %>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الثامن: الخدمات / المراكز --%>
        <% if (ServiceGroups.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Services))
           {
               string servicesLead = GetSectionLead(DgaPgSectionKeys.Services, GetResource("ServicesLead", "جمعنا لكِ أبرز ما تحتاجينه من خدماتٍ ومراكزَ في الجامعة، موصولةً مباشرةً بصفحاتها الرسمية؛ لتصلي إليها بيسرٍ ووضوح.", "The university services and centers you are most likely to need, linked straight to their official pages.")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Services) %>" aria-labelledby="enrolled-section-8-title">
          <h2 id="enrolled-section-8-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Services)) %></h2>
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

        <%-- المحور التاسع: السكن الجامعي --%>
        <% if (HousingInfo != null && IsSectionVisible(DgaPgSectionKeys.Housing))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Housing) %>" aria-labelledby="enrolled-section-9-title">
          <h2 id="enrolled-section-9-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Housing)) %></h2>
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
                    <div>
                      <h3 class="card-title"><%= Enc(HousingInfo.Title) %></h3>
                      <p class="card-text"><%= Enc(SectionTitle(DgaPgSectionKeys.Housing)) %></p>
                    </div>
                    <% if (!string.IsNullOrEmpty(HousingInfo.LinkUrl))
                       { %>
                    <div class="d-flex gap-3 flex-wrap"><a class="btn btn-primary external-link"
                        href="<%= EncUrl(HousingInfo.LinkUrl) %>" target="_blank"
                        rel="noopener"><%= Enc(GetResource("Go", "انتقال", "Go")) %></a></div>
                    <% } %>
                  </div>
                </article>
              </div>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور العاشر: تواصلي معنا --%>
        <% if (ContactInfo != null && IsSectionVisible(DgaPgSectionKeys.Contact))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Contact) %>" aria-labelledby="enrolled-section-10-title">
          <h2 id="enrolled-section-10-title" class="mt-5 mb-3"><%= Enc(SectionTitle(DgaPgSectionKeys.Contact)) %></h2>
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
                    <div>
                      <h3 class="card-title"><%= Enc(ContactInfo.Title) %></h3>
                      <p class="card-text"><%= Enc(SectionTitle(DgaPgSectionKeys.Contact)) %></p>
                    </div>
                    <% if (!string.IsNullOrEmpty(ContactInfo.LinkUrl))
                       { %>
                    <div class="d-flex gap-3 flex-wrap"><a class="btn btn-primary"
                        href="<%= EncUrl(ContactInfo.LinkUrl) %>"><%= Enc(GetResource("Go", "انتقال", "Go")) %></a></div>
                    <% } %>
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
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Reference) %>" aria-labelledby="enrolled-reference-title"
          class="kit-reference-section">
          <div class="kit-reference-text">
            <h2 id="enrolled-reference-title"><%= Enc(ReferenceTitle) %></h2>
            <p><%= Enc(ReferenceLead) %></p>
          </div>
          <a href="<%= EncUrl(ResolveKitHubUrl()) %>" class="kit-btn-prev"><%= Enc(GetResource("Previous", "السابق", "Previous")) %></a>
        </section>
        <% } %>

      </article>
    </div>
  </div>
</main>
