# Postgraduate Academic Kit (الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا - DGA v4)

Modular SharePoint 2019 ASCX user controls, master page, and automatic bilingual list provisioner with complete seed data for the 4 pages of the DGA v4 Postgraduate Academic Kit:

1. **الصفحة الرئيسة للحقيبة** (`academic-kit.html` / `(dga)academic-kit.html`)
2. **المتقدمون** (`academic-kit-applicants.html` / `(dga)academic-kit-applicants.html`)
3. **المقيدون** (`academic-kit-enrolled-students.html` / `(dga)academic-kit-enrolled-students.html`)
4. **الخريجون** (`academic-kit-graduates.html` / `(dga)academic-kit-graduates.html`)

---

## File Structure

```
CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\DGA\
├── Infrastructure/
│   ├── DgaPgListNames.cs              List names, phases, section keys
│   ├── DgaPgListSchema.cs             Field & list definitions registry
│   ├── DgaPgListSchema.Core.cs        Sections, Header, Intro, Journey schemas & seed
│   ├── DgaPgListSchema.Content.cs     Resources, ContentBlocks, Discounts, Services, QuickLinks
│   ├── DgaPgListSchema.Procedures.cs  Interactive academic & graduation procedures
│   ├── DgaPgListSchema.Faq.cs         FAQ questions and answers
│   ├── DgaPgListSchema.Hub.cs         Landing page hub cards
│   ├── DgaPgListProvisioner.cs        Idempotent provisioner & re-seeder
│   ├── DgaPgTargetWeb.cs              Resolves /ar/Deanship/postgraduate target web
│   ├── DgaPgHelper.cs                 Bilingual readers, Pick, safe parsers
│   ├── DgaPgModels.cs                 Strongly typed view models
│   ├── DgaPgLog.cs                    Isolated error logging
│   ├── DgaPgSectionBase.cs            Base user control class
│   └── DgaPostgraduateKitPageBase.cs  Base layout page forcing dga-PostgraduateKit.master
├── Controls/
│   ├── ucDgaKitHeader.ascx            New DGA header for kit master page
│   ├── ucDgaKitFooter.ascx            New DGA footer for kit master page
│   ├── ucDgaAcademicKitHub.ascx       Landing page hub control
│   ├── ucDgaAcademicKitApplicants.ascx Applicants station control
│   ├── ucDgaAcademicKitEnrolled.ascx  Enrolled students station control
│   ├── ucDgaAcademicKitGraduates.ascx Graduates station control
│   └── ucDgaPostgraduateKit.ascx      Composite / router control
├── Admin/
│   └── ucDgaPgAdmin.ascx              Editor CRUD & re-seed tool
└── README.md
```

---

## Master Page

- **Master Page File**: `MasterPages/dga-PostgraduateKit.master`
- **Assets Included**:
  - `IBM Plex Sans Arabic` fonts
  - `/Style Library/DGA/css/style.min.css`
  - `/Style Library/DGA/css/academic-kit-pages.css`
  - `/Style Library/DGA/vendor/hugeicons/hgi-stroke-rounded.css`
  - `/Style Library/DGA/vendor/css/swiper-bundle.min.css`
  - `/Style Library/DGA/vendor/js/bootstrap.bundle.min.js`
  - `/Style Library/DGA/vendor/js/swiper-bundle.min.js`
  - `/Style Library/DGA/js/app.js`
  - `SEO.ascx` header injection
  - `ucDgaKitHeader.ascx` custom header
  - `ucDgaKitFooter.ascx` custom footer

---

## Provisioned Lists (on `/ar/Deanship/postgraduate`)

All lists are prefixed with `DgaPg` and automatically created with full bilingual schema (`Title` / `Title_EN`, `Body` / `Body_EN`, etc.) and seeded on first page visit:

| List Name | Purpose |
|---|---|
| `DgaPgSections` | Section titles, leads, badges, and ordering per audience |
| `DgaPgHeader` | Page header titles, descriptions, and breadcrumbs |
| `DgaPgIntro` | Overview cards and badge pills |
| `DgaPgHub` | Landing page cards for the three audience tracks |
| `DgaPgJourney` | Sequential milestone steps (5 for applicants, 5 for enrolled, 6 for graduates) |
| `DgaPgResources` | Programs, Regulations, Guides, and Graduation Requirements |
| `DgaPgContentBlocks` | English Qualifying Course, Publication Incentive, Graduate Identity |
| `DgaPgProcedures` | Interactive academic and graduation procedures (conditions, articles, forms, workflow) |
| `DgaPgFaq` | FAQ accordion questions and answers |
| `DgaPgDiscounts` | Fee reduction / exemption tiers |
| `DgaPgDiscountNotes` | Conditions and notes governing fee exemptions |
| `DgaPgServices` | University centers and support services |
| `DgaPgQuickLinks` | Student housing and official contact channels |

---

## Page Layouts

Located in `PageLayouts/` to inherit `dga-PostgraduateKit.master` via `DgaPostgraduateKitPageBase`:

| Page Layout File | Target Audience & View |
|---|---|
| `DgaPostgraduateKitHub.aspx` | الصفحة الرئيسة للحقيبة (Hub Landing Page) |
| `DgaPostgraduateKitApplicants.aspx` | المتقدمون (Applicants Station) |
| `DgaPostgraduateKitEnrolled.aspx` | الطالبات المقيدات (Enrolled Students Station) |
| `DgaPostgraduateKitGraduates.aspx` | الخريجات (Graduates Station) |

---

## Usage in SharePoint Pages

```aspx
<%-- Hub / Landing Page --%>
<%@ Register TagPrefix="pnu" TagName="KitHub" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitHub.ascx" %>
<pnu:KitHub runat="server" />

<%-- Applicants Page --%>
<%@ Register TagPrefix="pnu" TagName="KitApplicants" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitApplicants.ascx" %>
<pnu:KitApplicants runat="server" />

<%-- Enrolled Students Page --%>
<%@ Register TagPrefix="pnu" TagName="KitEnrolled" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitEnrolled.ascx" %>
<pnu:KitEnrolled runat="server" />

<%-- Graduates Page --%>
<%@ Register TagPrefix="pnu" TagName="KitGraduates" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaAcademicKitGraduates.ascx" %>
<pnu:KitGraduates runat="server" />

<%-- Composite / Auto-routing Control --%>
<%@ Register TagPrefix="pnu" TagName="PostgraduateKit" Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/DGA/Controls/ucDgaPostgraduateKit.ascx" %>
<pnu:PostgraduateKit runat="server" Phase="Applicants" />
```

---

## Body CSS Classes (design parity — important)

`academic-kit-pages.css` scopes ~415 of its ~427 page rules under the body classes that the
static v4 pages carry:

| Page | Body classes in the static design |
|---|---|
| `academic-kit.html` | `dga-body academic-kit-page academic-kit-home` |
| `academic-kit-applicants.html` | `dga-body academic-kit-page academic-kit-detail academic-kit-applicants` |
| `academic-kit-enrolled-students.html` | `dga-body academic-kit-page academic-kit-detail academic-kit-enrolled` |
| `academic-kit-graduates.html` | `dga-body academic-kit-page academic-kit-detail academic-kit-graduates` |

`dga-PostgraduateKit.master` therefore declares `<body id="bodyTag" runat="server" …>` and
`DgaPgSectionBase.OnPreRender` rewrites the kit classes from the control's `Phase`
(`BodyCssClass`). Any new station control must set `Phase` in `Page_Load` or it will render
with hub styling.

The same classes are also stamped on `MSO_ContentDiv`, because a few design rules are written
as `.academic-kit-detail > .bg-primary-25` (the title band is a direct child of `<body>` in the
static pages, but sits inside `MSO_ContentDiv` under SharePoint).

---

## List-driven rendering (v4 binding)

The three station controls render entirely from the `DgaPg*` lists. Each control's
`EnsureDefaults()` still holds a reduced copy of the content, but that only applies when a
list is **absent** — once the provisioner has seeded a list, the list is what renders.

The seed therefore carries the complete v4 content, transcribed from the design files:

| List | Rows | Source |
|---|---|---|
| `DgaPgFaq` | 39 / 41 / 9 (applicants / enrolled / graduates) | design FAQ accordions |
| `DgaPgProcedures` | 15 enrolled + 2 graduates | design procedure accordions |
| `DgaPgServices` | 18, grouped by `Category` | design axis "الخدمات / المراكز" |
| `DgaPgDiscounts` / `DgaPgDiscountNotes` | 7 / 10 | design fees table |
| `DgaPgContentBlocks` | 4 (EnglishCourse, PublishIncentive, GraduateIdentity, FinancialRules) | design cards and charters |
| `DgaPgResources` | 13 (3 Programs, 4 Regulations, 3 Guides, 3 GradRequirements) | design resource cards |

Field conventions used by the seed and read back by `DgaPgHelper`:

- `Requirements`, `Bullets`, `Paragraphs`, `Badges` — one item per line.
- `Forms`, `ArticleText`, `Groups` — one item per line as `heading|value`.
- `BreadcrumbText` — `text|url;text|url;text|` (the last crumb is the current page).
- `Groups` for `GraduateIdentity` — `trait|tag; tag; tag`.
- `DgaPgProcedures.SubGroupKey` — `College` (design group 3.1) or `Student` (3.2).

### Section anchors

`academic-kit-pages.css` styles `#applicant-section-1..9`, `#enrolled-section-1..10`,
`#graduate-section-1..4`, `#enrolled-procedure-1` and `#enrolled-procedure-collapse-1/-2`
individually. Anchors are therefore produced by each control's `SectionAnchor(sectionKey)`
override, never from list position — reordering or hiding a section in SharePoint must not
move the styling onto different content. `TocItems` builds the sidebar from the sections that
actually rendered, and clearing `IsVisible` on a `DgaPgSections` row hides an axis and its TOC
entry together.
