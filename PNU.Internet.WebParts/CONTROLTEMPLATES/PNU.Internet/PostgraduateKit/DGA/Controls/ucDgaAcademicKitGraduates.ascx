<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaAcademicKitGraduates.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls.ucDgaAcademicKitGraduates" %>
<%@ Import Namespace="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure" %>

<%-- الخريجون — DGA v4 Postgraduate Academic Kit, Graduates station.
     Content is driven by the DgaPg* lists; the code-behind supplies the v4
     content as fallback whenever a list is missing or empty. Section anchor ids
     come from SectionAnchor() because academic-kit-pages.css styles
     #graduate-section-1..4 individually. --%>

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
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Intro) %>" aria-labelledby="graduate-intro-title">
          <div class="card mb-4 bg-primary-25 border-0">
            <div class="card-body p-4 p-lg-5">
              <div class="d-flex flex-column gap-3">
                <span class="icon-container"><i class="hgi hgi-stroke <%= Enc(IconOr(IntroModel.IconClass, "hgi-book-open-01")) %> fs-4"
                    aria-hidden="true"></i></span>
                <h2 id="graduate-intro-title" class="mb-0"><%= Enc(IntroModel.Title) %></h2>
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

        <%-- المحور الأول: رحلة التخرّج --%>
        <% if (JourneySteps.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Journey))
           {
               string journeyLead = GetSectionLead(DgaPgSectionKeys.Journey, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Journey) %>" aria-labelledby="graduate-section-1-title">
          <h2 id="graduate-section-1-title" class="mt-5 mb-3"><%= Enc(GetSectionTitle(DgaPgSectionKeys.Journey, GetResource("GradAxis1", "المحور الأول: رحلة التخرّج", "First Axis: Graduation Journey"))) %></h2>
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

        <%-- المحور الثاني: متطلّبات التخرّج --%>
        <% if (GraduationRequirements.Count > 0 && IsSectionVisible(DgaPgSectionKeys.GradRequirements))
           {
               string reqLead = GetSectionLead(DgaPgSectionKeys.GradRequirements, GetResource("GradReqLead", "تستكمل الخرِّيجة المتطلّبات التالية لإتمام تخرّجها واستلام وثيقتها:", "The graduate completes the following requirements to finish graduation and obtain the diploma:")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.GradRequirements) %>" aria-labelledby="graduate-section-2-title">
          <h2 id="graduate-section-2-title" class="mt-5 mb-3"><%= Enc(GetSectionTitle(DgaPgSectionKeys.GradRequirements, GetResource("GradAxis2", "المحور الثاني: متطلّبات التخرّج", "Second Axis: Graduation Requirements"))) %></h2>
          <% if (!string.IsNullOrEmpty(reqLead))
             { %>
          <p><%= Enc(reqLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="row g-4">
              <% foreach (DgaPgResourceCardModel req in GraduationRequirements)
                 { %>
              <div class="col-12 col-lg-4 col-md-6">
                <article class="card h-100">
                  <div class="card-body d-flex flex-column gap-4">
                    <div class="icon-container"><span class="d-inline-flex fs-3"><i
                          class="hgi hgi-stroke <%= Enc(IconOr(req.IconClass, "hgi-check-list")) %> fs-3" aria-hidden="true"></i></span></div>
                    <div>
                      <h3 class="card-title"><%= Enc(req.Title) %></h3>
                      <p class="card-text"><%= Enc(req.Description) %></p>
                    </div>
                    <% if (!string.IsNullOrEmpty(req.BadgeText))
                       { %>
                    <div class="d-flex flex-wrap mt-auto gap-2"><span class="badge badge-info"><%= Enc(req.BadgeText) %></span></div>
                    <% } %>
                  </div>
                </article>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الثالث: إجراءات التخرّج --%>
        <% if (Procedures.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Procedures))
           {
               string procLead = GetSectionLead(DgaPgSectionKeys.Procedures, GetResource("GradProcLead", "الإجراءات الأكاديمية الخاصة بمرحلة التخرّج — بالضغط على كلٍّ منها تظهر شروطه ونماذجه ومساره:", "Academic procedures for the graduation stage — click each to view its conditions, forms and workflow:")); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Procedures) %>" aria-labelledby="graduate-section-3-title">
          <h2 id="graduate-section-3-title" class="mt-5 mb-3"><%= Enc(GetSectionTitle(DgaPgSectionKeys.Procedures, GetResource("GradAxis3", "المحور الثالث: إجراءات التخرّج", "Third Axis: Graduation Procedures"))) %></h2>
          <% if (!string.IsNullOrEmpty(procLead))
             { %>
          <p><%= Enc(procLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="accordion accordion-flush" id="graduate-procedure-1">
              <% int pi = 0;
                 foreach (DgaPgProcedureModel proc in Procedures)
                 {
                     pi++; %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="graduate-procedure-heading-<%= pi %>">
                  <button aria-controls="graduate-procedure-collapse-<%= pi %>" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#graduate-procedure-collapse-<%= pi %>"
                    data-bs-toggle="collapse" type="button"><% if (!string.IsNullOrEmpty(proc.IconUrl))
                                                                { %><span class="kit-procedure-icon"><img
                        src="<%= EncUrl(proc.IconUrl) %>" alt="" aria-hidden="true"></span><% } %><span
                      class="kit-procedure-btn-title"><%= Enc(proc.Title) %></span><% if (!string.IsNullOrEmpty(proc.BadgeText))
                                                                                       { %>
                    <span class="kit-procedure-badge"><%= Enc(proc.BadgeText) %></span><% } %></button>
                </p>
                <div class="accordion-collapse collapse" id="graduate-procedure-collapse-<%= pi %>"
                  aria-labelledby="graduate-procedure-heading-<%= pi %>" data-bs-parent="#graduate-procedure-1">
                  <div class="accordion-body">
                    <% if (!string.IsNullOrEmpty(proc.ShortDescription))
                       { %>
                    <p class="kit-procedure-summary"><%= Enc(proc.ShortDescription) %></p>
                    <% } %>
                    <% if (proc.Requirements.Count > 0)
                       { %>
                    <section class="kit-procedure-content-section" aria-labelledby="graduate-procedure-requirements-<%= pi %>">
                      <h4 class="kit-procedure-section-title" id="graduate-procedure-requirements-<%= pi %>"><i
                          class="hgi hgi-stroke hgi-file-edit" aria-hidden="true"></i><span><%= Enc(GetResource("ProcConditions", "الشروط والمتطلبات", "Conditions and Requirements")) %></span>
                      </h4>
                      <ul class="kit-procedure-list">
                        <% foreach (string requirement in proc.Requirements)
                           { %>
                        <li><%= Enc(requirement) %></li>
                        <% } %>
                      </ul>
                    </section>
                    <% } %>
                    <% if (proc.Articles.Count > 0)
                       { %>
                    <section class="kit-procedure-content-section" aria-labelledby="graduate-procedure-regulations-<%= pi %>">
                      <h4 class="kit-procedure-section-title" id="graduate-procedure-regulations-<%= pi %>"><i
                          class="hgi hgi-stroke hgi-legal-document-01" aria-hidden="true"></i><span><%= Enc(GetResource("ProcArticle", "نص المادة من اللائحة", "Article Text from the Regulation")) %></span></h4>
                      <% foreach (DgaPgArticleBlock article in proc.Articles)
                         { %>
                      <div class="kit-procedure-regulation">
                        <h5><%= Enc(article.Heading) %></h5>
                        <p><%= Enc(article.Body) %></p>
                      </div>
                      <% } %>
                    </section>
                    <% } %>
                    <% if (proc.Forms.Count > 0)
                       { %>
                    <section class="kit-procedure-content-section" aria-labelledby="graduate-procedure-forms-<%= pi %>">
                      <h4 class="kit-procedure-section-title" id="graduate-procedure-forms-<%= pi %>"><i
                          class="hgi hgi-stroke hgi-pin" aria-hidden="true"></i><span><%= Enc(GetResource("ProcForms", "النماذج المرتبطة", "Related Forms")) %></span></h4>
                      <div class="kit-procedure-downloads">
                        <% foreach (DgaPgLinkItem form in proc.Forms)
                           { %>
                        <a class="kit-procedure-download" href="<%= EncUrl(form.Url) %>" target="_blank"
                          rel="noopener"><span><%= Enc(form.Title) %></span><i class="hgi hgi-stroke hgi-download-02"
                            aria-hidden="true"></i></a>
                        <% } %>
                      </div>
                    </section>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(proc.FlowText))
                       { %>
                    <p class="kit-procedure-path"><%= Enc(proc.FlowText) %></p>
                    <% } %>
                  </div>
                </div>
              </div>
              <% } %>
            </div>
          </div>
        </section>
        <% } %>

        <%-- المحور الرابع: الأسئلة الأكثر شيوعاً --%>
        <% if (FaqItems.Count > 0 && IsSectionVisible(DgaPgSectionKeys.Faq))
           {
               string faqLead = GetSectionLead(DgaPgSectionKeys.Faq, string.Empty); %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Faq) %>" aria-labelledby="graduate-section-4-title">
          <h2 id="graduate-section-4-title" class="mt-5 mb-3"><%= Enc(GetSectionTitle(DgaPgSectionKeys.Faq, GetResource("GradAxis4", "المحور الرابع: الأسئلة الأكثر شيوعاً", "Fourth Axis: Frequently Asked Questions"))) %></h2>
          <% if (!string.IsNullOrEmpty(faqLead))
             { %>
          <p><%= Enc(faqLead) %></p>
          <% } %>
          <div class="d-flex flex-column gap-4">
            <div class="accordion accordion-flush" id="graduate-faq-2">
              <% int fi = 0;
                 foreach (DgaPgFaqItemModel faq in FaqItems)
                 {
                     fi++; %>
              <div class="accordion-item">
                <p class="accordion-header mb-0" id="graduate-faq-heading-<%= fi %>">
                  <button aria-controls="graduate-faq-collapse-<%= fi %>" aria-expanded="false"
                    class="accordion-button collapsed" data-bs-target="#graduate-faq-collapse-<%= fi %>"
                    data-bs-toggle="collapse" type="button"><%= Enc(faq.Question) %></button>
                </p>
                <div class="accordion-collapse collapse" id="graduate-faq-collapse-<%= fi %>"
                  aria-labelledby="graduate-faq-heading-<%= fi %>" data-bs-parent="#graduate-faq-2">
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

        <%-- الأساس النظامي والمرجعي --%>
        <% if (!string.IsNullOrEmpty(ReferenceLead) && IsSectionVisible(DgaPgSectionKeys.Reference))
           { %>
        <section id="<%= SectionAnchor(DgaPgSectionKeys.Reference) %>" aria-labelledby="graduate-reference-title"
          class="kit-reference-section">
          <div class="kit-reference-text">
            <h2 id="graduate-reference-title"><%= Enc(ReferenceTitle) %></h2>
            <p><%= Enc(ReferenceLead) %></p>
          </div>
          <a href="<%= EncUrl(ResolveKitHubUrl()) %>" class="kit-btn-prev"><%= Enc(GetResource("Previous", "السابق", "Previous")) %></a>
        </section>
        <% } %>

      </article>
    </div>
  </div>
</main>
