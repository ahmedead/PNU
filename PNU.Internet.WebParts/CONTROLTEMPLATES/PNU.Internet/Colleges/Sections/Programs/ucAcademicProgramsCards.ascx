<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAcademicProgramsCards.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucAcademicProgramsCards" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="bg-primary-25 py-5" data-aos="fade-up">
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2">
                <li class="breadcrumb-item small"><a href="<%= SPFactory.GetSiteURL() %>"><%= SPFactory.IsArabic ? "الرئيسية" : "Home" %></a></li>
                <li class="breadcrumb-item small"><a href="<%= SPFactory.GetSiteURL() %>RegAdm/Pages/home.aspx"><%= SPFactory.IsArabic ? "القبول والتسجيل" : "Admission & Registration" %></a></li>
                <li class="breadcrumb-item small active" aria-current="page"><span><%= SPFactory.IsArabic ? "البرامج الأكاديمية" : "Academic Programs" %></span></li>
            </ol>
        </nav>
        <div class="content">
            <h1 class="h2 mb-0 fw-semibold"><%= SPFactory.IsArabic ? "البرامج الأكاديمية" : "Academic Programs" %></h1>
            <div class="text mt-3 text-muted">
                <%= SPFactory.IsArabic ? "استعرض نماذج من برامج الجامعة في كروت توضّح طبيعة البرنامج، ومدة الدراسة، والكلية، ومسارات الثانوية المؤهلة." : "Explore the university programs with details on nature, duration, college, and qualifying secondary tracks." %>
            </div>
        </div>
    </div>
</div>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <div class="container page-padding">
        <!-- Search and Filter Bar -->
        <dga-search-input>
            <div class="d-flex gap-3 mb-4 flex-wrap flex-md-nowrap" role="search" aria-label="ابحث عن برنامج أكاديمي">
                <div class="form-control-container has-icon flex-grow-1">
                    <span class="d-inline-flex fs-6 form-input-icon text-body-secondary">
                        <i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i>
                    </span>
                    <input id="programSearchInput" type="search" autocomplete="off" class="form-control" placeholder="<%= SPFactory.IsArabic ? "ابحث باسم شهادة التخرج أو التخصص" : "Search by degree name or major" %>" value="">
                </div>
                <button id="programSearchBtn" type="button" class="btn btn-primary" aria-label="بحث"><%= SPFactory.IsArabic ? "بحث" : "Search" %></button>
                <dga-filter-dropdown class="flex-shrink-0">
                    <div class="dropdown flex-shrink-0">
                        <button type="button" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside" class="btn btn-dark gap-1">
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-filter" aria-hidden="true"></i></span>
                            <span><%= SPFactory.IsArabic ? "تصفية" : "Filter" %></span>
                            <span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-down-01" aria-hidden="true"></i></span>
                        </button>
                        <div class="dropdown-menu px-2" style="width: 20rem;">
                            <p class="fw-semibold px-2 mb-2"><%= SPFactory.IsArabic ? "الكلية" : "College" %></p>
                            <div class="form-control-container has-icon px-2">
                                <span class="d-inline-flex fs-6 form-input-icon text-body-secondary"><i class="hgi hgi-stroke hgi-search-01" aria-hidden="true"></i></span>
                                <input id="collegeFilterSearchInput" type="text" autocomplete="off" class="form-control" placeholder="<%= SPFactory.IsArabic ? "ابحث عن كلية" : "Search for college" %>">
                            </div>
                            <div id="collegeFilterList" class="d-flex flex-column gap-2 mt-3 px-2 py-2 overflow-auto" style="max-height: 12.5rem;">
                                <asp:Repeater ID="rptCollegeFilter" runat="server">
                                    <ItemTemplate>
                                        <div class="form-check college-filter-item">
                                            <input type="checkbox" class="form-check-input college-checkbox" id='<%# "college-filter-" + Eval("Code") %>' value='<%# Eval("Code") %>' data-college-name='<%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>'>
                                            <label class="form-check-label" for='<%# "college-filter-" + Eval("Code") %>'><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <hr class="my-2">
                            <div class="d-flex justify-content-between gap-2 pb-2 px-2">
                                <button id="applyFilterBtn" type="button" class="btn btn-primary btn-sm"><%= SPFactory.IsArabic ? "تطبيق الاختيارات" : "Apply" %></button>
                                <button id="resetFilterBtn" type="button" class="btn btn-secondary btn-sm"><%= SPFactory.IsArabic ? "إعادة تعيين" : "Reset" %></button>
                            </div>
                        </div>
                    </div>
                </dga-filter-dropdown>
            </div>
        </dga-search-input>
        
        <div class="d-flex align-items-center justify-content-between gap-3 mb-4">
            <h2 class="mb-0 h4 fw-semibold"><%= SPFactory.IsArabic ? "البرامج الأكاديمية" : "Academic Programs" %></h2>
            <span id="programsCount" class="text-body-secondary small"></span>
        </div>

        <div class="row g-4" id="programsGrid">
            <asp:Repeater ID="rptPrograms" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6 col-lg-4 program-card-col" 
                         data-college-code='<%# Eval("CollegeCode") %>' 
                         data-college-name='<%# SPFactory.GetLocalizedTitle(Eval("CollegeName"), Eval("CollegeName_EN")) %>'
                         data-program-name='<%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %>'
                         data-major='<%# SPFactory.GetLocalizedTitle(Eval("Major"), Eval("Major_EN")) %>'
                         data-dept='<%# SPFactory.GetLocalizedTitle(Eval("DepartmentName"), Eval("DepartmentName_EN")) %>'
                         data-desc='<%# SPFactory.GetLocalizedTitle(Eval("ProgramDesc"), Eval("ProgramDesc_EN")) %>'>
                        <article class="card h-100" id='<%# "academic-program-card-" + Eval("ProgramCode") %>'>
                            <div class="card-body d-flex flex-column gap-4">
                                <div>
                                    <h3 class="card-title h5 mb-2"><%# SPFactory.GetLocalizedTitle(Eval("ProgramName"), Eval("ProgramName_EN")) %></h3>
                                    <p class="card-text line-clamp max-clamp-line-4 text-muted mb-0" id='<%# "prog_desc_" + Container.ItemIndex %>'>
                                        <%# SPFactory.GetLocalizedTitle(Eval("ProgramDesc"), Eval("ProgramDesc_EN")) %>
                                    </p>
                                </div>
                                <div class="d-flex flex-column gap-2 small text-body-secondary">
                                    <div class="d-flex gap-2 align-items-start" style='<%# String.IsNullOrEmpty(Eval("ProgramYears") as string) ? "display:none !important;" : "" %>'>
                                        <i class="hgi hgi-stroke hgi-clock-01 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><strong><%# SPFactory.IsArabic ? "مدة الدراسة" : "Duration" %>:</strong> <%# Eval("ProgramYears") %> <%# SPFactory.IsArabic ? "سنوات" : "Years" %></span>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <i class="hgi hgi-stroke hgi-building-01 fs-5 text-primary flex-shrink-0" aria-hidden="true"></i>
                                        <span><%# SPFactory.GetLocalizedTitle(Eval("CollegeName"), Eval("CollegeName_EN")) %><%# !String.IsNullOrEmpty(Eval("DepartmentName") as string) ? " - " + SPFactory.GetLocalizedTitle(Eval("DepartmentName"), Eval("DepartmentName_EN")) : "" %></span>
                                    </div>
                                </div>
                                <div class="d-flex flex-wrap mt-auto gap-2">
                                    <%# Eval("SecondaryTypeBadgesHtml") %>
                                </div>
                                <div class="d-flex gap-3 flex-wrap">
                                    <a class="btn btn-primary" href='<%# String.Format("ProgramDetails.aspx?ProgramCode={0}", Eval("ProgramCode")) %>'>
                                        <%# SPFactory.IsArabic ? "عرض التفاصيل" : "View Details" %>
                                    </a>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div id="noResultsMessage" class="text-center py-5 d-none">
            <i class="hgi hgi-stroke hgi-search-not-found fs-1 text-muted d-block mb-3"></i>
            <h3 class="h5 text-muted"><%= SPFactory.IsArabic ? "لا توجد برامج مطابقة لخيارات البحث أو التصفية" : "No programs matching your search or filter criteria" %></h3>
        </div>

        <dga-paginator>
            <div class="mt-5 p-2 d-flex justify-content-center">
                <nav aria-label="قائمة التنقل في الصفحات">
                    <ul class="pagination" id="programs-pagination">
                    </ul>
                </nav>
            </div>
        </dga-paginator>
    </div>
</main>

<script>
document.addEventListener("DOMContentLoaded", function () {
    const isArabic = <%= SPFactory.IsArabic ? "true" : "false" %>;
    const itemsPerPage = 9;
    const allCards = Array.from(document.querySelectorAll(".program-card-col"));
    const searchInput = document.getElementById("programSearchInput");
    const searchBtn = document.getElementById("programSearchBtn");
    const collegeSearchInput = document.getElementById("collegeFilterSearchInput");
    const collegeCheckboxes = document.querySelectorAll(".college-checkbox");
    const applyFilterBtn = document.getElementById("applyFilterBtn");
    const resetFilterBtn = document.getElementById("resetFilterBtn");
    const countDisplay = document.getElementById("programsCount");
    const paginationContainer = document.getElementById("programs-pagination");
    const noResultsMsg = document.getElementById("noResultsMessage");

    let filteredCards = [...allCards];
    let currentPage = 1;

    function filterData() {
        const query = (searchInput ? searchInput.value : "").trim().toLowerCase();
        const selectedColleges = Array.from(collegeCheckboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value.trim().toLowerCase());

        filteredCards = allCards.filter(card => {
            const progName = (card.getAttribute("data-program-name") || "").toLowerCase();
            const progDesc = (card.getAttribute("data-desc") || "").toLowerCase();
            const major = (card.getAttribute("data-major") || "").toLowerCase();
            const dept = (card.getAttribute("data-dept") || "").toLowerCase();
            const collName = (card.getAttribute("data-college-name") || "").toLowerCase();
            const collCode = (card.getAttribute("data-college-code") || "").toLowerCase();

            // Text search match
            const matchesText = !query || 
                progName.includes(query) || 
                progDesc.includes(query) || 
                major.includes(query) || 
                dept.includes(query) || 
                collName.includes(query);

            // College filter match
            const matchesCollege = selectedColleges.length === 0 || selectedColleges.includes(collCode);

            return matchesText && matchesCollege;
        });

        currentPage = 1;
        updateView();
    }

    function updateView() {
        const totalItems = filteredCards.length;
        const totalPages = Math.ceil(totalItems / itemsPerPage) || 1;

        if (totalItems === 0) {
            allCards.forEach(c => c.style.display = "none");
            if (noResultsMsg) noResultsMsg.classList.remove("d-none");
            if (countDisplay) countDisplay.textContent = isArabic ? "0 برنامج معروض" : "0 programs displayed";
            if (paginationContainer) paginationContainer.innerHTML = "";
            return;
        } else {
            if (noResultsMsg) noResultsMsg.classList.add("d-none");
        }

        if (countDisplay) {
            countDisplay.textContent = isArabic 
                ? `${totalItems} برنامج معروض` 
                : `${totalItems} programs displayed`;
        }

        const start = (currentPage - 1) * itemsPerPage;
        const end = start + itemsPerPage;

        allCards.forEach(c => c.style.display = "none");
        filteredCards.slice(start, end).forEach(c => c.style.display = "");

        renderPagination(totalPages);
    }

    function renderPagination(totalPages) {
        if (!paginationContainer) return;
        paginationContainer.innerHTML = "";

        if (totalPages <= 1) return;

        // 1. First Page button (<<)
        const firstLi = document.createElement("li");
        firstLi.className = `page-item ${currentPage === 1 ? "disabled" : ""}`;
        const firstLink = document.createElement("a");
        firstLink.className = "page-link btn btn-secondary icon-btn navigation-link";
        firstLink.href = "#";
        firstLink.setAttribute("aria-label", isArabic ? "الصفحة الأولى" : "First Page");
        firstLink.innerHTML = `<span class="d-inline-flex fs-5 align-items-center"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" style="margin-inline-start: -8px;" aria-hidden="true"></i></span>`;
        firstLink.addEventListener("click", function (e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage = 1;
                updateView();
                const grid = document.getElementById("programsGrid");
                if (grid) window.scrollTo({ top: grid.offsetTop - 120, behavior: "smooth" });
            }
        });
        firstLi.appendChild(firstLink);
        paginationContainer.appendChild(firstLi);

        // 2. Previous button (<)
        const prevLi = document.createElement("li");
        prevLi.className = `page-item ${currentPage === 1 ? "disabled" : ""}`;
        const prevLink = document.createElement("a");
        prevLink.className = "page-link btn btn-secondary icon-btn navigation-link";
        prevLink.href = "#";
        prevLink.setAttribute("aria-label", isArabic ? "الصفحة السابقة" : "Previous");
        prevLink.innerHTML = `<span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-left-01 rtl-flip" aria-hidden="true"></i></span>`;
        prevLink.addEventListener("click", function (e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                updateView();
                const grid = document.getElementById("programsGrid");
                if (grid) window.scrollTo({ top: grid.offsetTop - 120, behavior: "smooth" });
            }
        });
        prevLi.appendChild(prevLink);
        paginationContainer.appendChild(prevLi);

        // 3. Numbered Pages (with smart windowing)
        let startPage = Math.max(1, currentPage - 2);
        let endPage = Math.min(totalPages, startPage + 4);
        if (endPage - startPage < 4) {
            startPage = Math.max(1, endPage - 4);
        }

        for (let i = startPage; i <= endPage; i++) {
            const li = document.createElement("li");
            li.className = "page-item";
            const a = document.createElement("a");
            a.className = `page-link btn btn-secondary ${i === currentPage ? "active" : ""}`;
            a.href = "#";
            a.textContent = i;
            a.addEventListener("click", function (e) {
                e.preventDefault();
                currentPage = i;
                updateView();
                const grid = document.getElementById("programsGrid");
                if (grid) window.scrollTo({ top: grid.offsetTop - 120, behavior: "smooth" });
            });
            li.appendChild(a);
            paginationContainer.appendChild(li);
        }

        // 4. Next button (>)
        const nextLi = document.createElement("li");
        nextLi.className = `page-item ${currentPage === totalPages ? "disabled" : ""}`;
        const nextLink = document.createElement("a");
        nextLink.className = "page-link btn btn-secondary icon-btn navigation-link";
        nextLink.href = "#";
        nextLink.setAttribute("aria-label", isArabic ? "الصفحة التالية" : "Next");
        nextLink.innerHTML = `<span class="d-inline-flex fs-5"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i></span>`;
        nextLink.addEventListener("click", function (e) {
            e.preventDefault();
            if (currentPage < totalPages) {
                currentPage++;
                updateView();
                const grid = document.getElementById("programsGrid");
                if (grid) window.scrollTo({ top: grid.offsetTop - 120, behavior: "smooth" });
            }
        });
        nextLi.appendChild(nextLink);
        paginationContainer.appendChild(nextLi);

        // 5. Last Page button (>>)
        const lastLi = document.createElement("li");
        lastLi.className = `page-item ${currentPage === totalPages ? "disabled" : ""}`;
        const lastLink = document.createElement("a");
        lastLink.className = "page-link btn btn-secondary icon-btn navigation-link";
        lastLink.href = "#";
        lastLink.setAttribute("aria-label", isArabic ? "الصفحة الأخيرة" : "Last Page");
        lastLink.innerHTML = `<span class="d-inline-flex fs-5 align-items-center"><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" aria-hidden="true"></i><i class="hgi hgi-stroke hgi-arrow-right-01 rtl-flip" style="margin-inline-start: -8px;" aria-hidden="true"></i></span>`;
        lastLink.addEventListener("click", function (e) {
            e.preventDefault();
            if (currentPage < totalPages) {
                currentPage = totalPages;
                updateView();
                const grid = document.getElementById("programsGrid");
                if (grid) window.scrollTo({ top: grid.offsetTop - 120, behavior: "smooth" });
            }
        });
        lastLi.appendChild(lastLink);
        paginationContainer.appendChild(lastLi);
    }

    // Event listeners
    if (searchInput) {
        searchInput.addEventListener("input", filterData);
        searchInput.addEventListener("keyup", function (e) {
            if (e.key === "Enter") filterData();
        });
    }
    if (searchBtn) {
        searchBtn.addEventListener("click", filterData);
    }

    if (collegeSearchInput) {
        collegeSearchInput.addEventListener("input", function () {
            const term = this.value.trim().toLowerCase();
            document.querySelectorAll(".college-filter-item").forEach(item => {
                const name = (item.querySelector("label").textContent || "").toLowerCase();
                item.style.display = (!term || name.includes(term)) ? "" : "none";
            });
        });
    }

    if (applyFilterBtn) {
        applyFilterBtn.addEventListener("click", function () {
            filterData();
            const ddToggle = document.querySelector('[data-bs-toggle="dropdown"][aria-expanded="true"]');
            if (ddToggle && window.bootstrap && window.bootstrap.Dropdown) {
                const instance = bootstrap.Dropdown.getInstance(ddToggle);
                if (instance) instance.hide();
            }
        });
    }

    if (resetFilterBtn) {
        resetFilterBtn.addEventListener("click", function () {
            collegeCheckboxes.forEach(cb => cb.checked = false);
            if (collegeSearchInput) collegeSearchInput.value = "";
            document.querySelectorAll(".college-filter-item").forEach(item => item.style.display = "");
            filterData();
        });
    }

    // Initial display
    updateView();
});
</script>
