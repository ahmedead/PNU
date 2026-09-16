# Postgraduate Academic Kit (الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا)

SharePoint 2019 modular ASCX user controls, an automatic list provisioner with bilingual seed
data, and an admin CRUD control for the four pages of the interactive postgraduate academic kit
of the Deanship of Graduate Studies, based on `DGA_NewDesign/v3.0 3/`.

| Design file    | Page                        | `Phase` value |
| -------------- | --------------------------- | ------------- |
| `index.html`   | الصفحة الرئيسة للحقيبة      | `Hub`         |
| `phase01.html` | المتقدمون                   | `Applicants`  |
| `phase02.html` | المقيدون                    | `Enrolled`    |
| `phase03.html` | الخريجون                    | `Graduates`   |

`index.html` is the landing page: its `<title>` and canonical tag are copied from the
university home page template, but its body is the kit hub — a hero carousel, the overview,
and three cards linking to the stations.

## Target web

All lists live on **`/ar/Deanship/postgraduate`** (configured in `PgTargetWeb.cs`). Both the
Arabic and the English pages read from that single web: every list carries an Arabic column and
its `_EN` twin, and `PgHelper.Pick` chooses by the current web's language (1025 = Arabic),
falling back to the other language when a value is empty. Nothing has to be duplicated per
language, and the English pages stay in sync with the Arabic ones.

The web is **not** created by this feature — it already exists. If it cannot be opened, the
controls log the problem and fall back to the current web rather than failing the page.

## File structure

```
CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\
├── Infrastructure/
│   ├── PgListNames.cs            list names, PgPhases, PgSectionKeys
│   ├── PgListSchema.cs           field/list definition types + shared field builders
│   ├── PgListSchema.Core.cs      PgSections, PgHeader, PgIntro, PgJourney + seed
│   ├── PgListSchema.Content.cs   PgResources, PgContentBlocks, PgDiscounts,
│   │                             PgDiscountNotes, PgServices, PgQuickLinks + seed
│   ├── PgListSchema.Procedures.cs PgProcedures + seed (17 procedures)
│   ├── PgListSchema.Faq.cs       PgFaq + seed (88 question/answer pairs)
│   ├── PgListSchema.Chrome.cs    PgTopBar, PgFooterLinks + seed
│   ├── PgListSchema.Hub.cs       PgHub + seed (landing page)
│   ├── PgPageBase.cs             page layout base, forces PostgraduateKit.master
│   ├── PgListProvisioner.cs      idempotent create / add columns / seed
│   ├── PgTargetWeb.cs            resolves /ar/Deanship/postgraduate
│   ├── PgHelper.cs               Safe* readers, language pick, multi-line parsing
│   ├── PgLog.cs                  error logging via Publics.WriteToLog
│   ├── PgModels.cs               view models
│   └── PgSectionBase.cs          base class: phase, heading lookup, item loading
├── Controls/
│   ├── ucPostgraduateKit.ascx    composite: one whole station page
│   ├── ucPgHub.ascx              composite: the landing page
│   ├── ucPgHubHero.ascx          landing page hero carousel
│   ├── ucPgHubCards.ascx         landing page station cards
│   ├── ucPgStyles.ascx           the CSS the base DGA stylesheet does not carry
│   ├── ucPgHeader.ascx           hero title + breadcrumb
│   ├── ucPgIntro.ascx            النبذة التعريفية
│   ├── ucPgJourney.ascx          خريطة الرحلة / رحلة التخرّج
│   ├── ucPgResources.ascx        link + document cards (4 groups)
│   ├── ucPgContentBlock.ascx     free-form cards (5 kinds)
│   ├── ucPgProcedures.ascx       الإجراءات الأكاديمية / إجراءات التخرّج
│   ├── ucPgFaq.ascx              الأسئلة الأكثر شيوعاً
│   ├── ucPgDiscounts.ascx        جدول الإعفاء والتخفيض + ملاحظاته
│   ├── ucPgServices.ascx         الخدمات / المراكز
│   ├── ucPgQuickLinks.ascx       السكن الجامعي / تواصلي معنا
│   ├── ucPgSecondBar.ascx        the strip above the navigation (master page)
│   └── ucPgSiteFooter.ascx       the kit footer (master page)
├── Admin/
│   └── ucPgAdmin.ascx            CRUD over every list + re-seed buttons
├── Features/
│   └── PostgraduateKitListsFeatureReceiver.cs
└── README.md
```

## Lists provisioned (15 lists, 240 seeded rows)

| List              | Rows | Purpose |
| ----------------- | ---- | ------- |
| `PgSections`      | 31   | Heading, lead paragraph and visibility of every axis, per audience |
| `PgHeader`        | 3    | Hero title and breadcrumb of each page |
| `PgIntro`         | 4    | النبذة التعريفية — three stations plus the landing page |
| `PgHub`           | 4    | Landing page: one carousel slide + three station cards |
| `PgJourney`       | 16   | Journey steps (5 / 5 / 6) |
| `PgResources`     | 17   | Programs, regulations, official guides, graduation requirements |
| `PgContentBlocks` | 7    | English course, graduate identity, financial rules, publication incentive, footer note |
| `PgProcedures`    | 17   | 15 academic procedures + 2 graduation procedures |
| `PgFaq`           | 88   | 38 applicants + 41 enrolled + 9 graduates |
| `PgDiscounts`     | 7    | Fee exemption / reduction table |
| `PgDiscountNotes` | 9    | The conditions printed under that table |
| `PgServices`      | 18   | Research, employability and student services |
| `PgQuickLinks`    | 2    | Student housing link, deanship email |
| `PgTopBar`        | 8    | The strip above the navigation: weather, date, time, place, 4 shortcuts |
| `PgFooterLinks`   | 8    | The three footer columns and the bottom bar links |

Every list has `Title` (Arabic), `Title_EN`, `ItemOrder` and a `Phase` choice column.
**A row with an empty `Phase` is shared and appears on every page** — that is how the fee
table, its notes, the services and the quick links are seeded once instead of three times.

## Placing the controls on a page

There are two composite controls: `ucPgHub` renders the landing page, and `ucPostgraduateKit`
renders one station page. Both are already wired into the page layouts below, so this section
only matters when placing them somewhere else — through the control loader web part, say.

```aspx
<%@ Register TagPrefix="pnu" TagName="PostgraduateKitHub"
    Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPgHub.ascx" %>
<%@ Register TagPrefix="pnu" TagName="PostgraduateKit"
    Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Controls/ucPostgraduateKit.ascx" %>

<pnu:PostgraduateKitHub runat="server" />
<pnu:PostgraduateKit runat="server" Phase="Applicants" />
```

`ucPgHub` takes `ShowHero`, `ShowIntro`, `ShowCards` and `ShowStyles`; it always reads the `Hub`
phase.

`Phase` accepts `Applicants`, `Enrolled` or `Graduates`; the sections that do not belong to that
audience are switched off automatically, and the rest render in the order of the design. Set
`ShowHeaderSection="False"` when the page layout already renders its own hero, and
`ShowStyles="False"` once the kit CSS has been merged into the site stylesheet.

Individual sections can also be placed on their own, which is useful when an editor wants a
single axis on a different page:

```aspx
<pnu:PgFaq runat="server" Phase="Enrolled" MaxItems="10" />
<pnu:PgResources runat="server" Phase="Applicants" GroupKey="Guides" />
<pnu:PgContentBlock runat="server" Phase="Enrolled" BlockKey="PublishIncentive" />
<pnu:PgQuickLinks runat="server" Phase="Applicants" SectionKey="Contact" />
```

Common section properties: `Phase`, `MaxItems`, `ShowTitle`, `SectionTitle` (hard override of the
heading), `ListNameOverride`, `SectionKeyOverride`.

## Admin control

`Admin/ucPgAdmin.ascx` is a generic editor over all fifteen lists:

- pick a list, and optionally filter the grid by audience;
- add, edit and delete items with a form generated from the schema (multi-line fields get a
  textarea, choice fields get a dropdown, and every field shows its hint);
- **إعادة تعبئة هذه القائمة** re-seeds the selected list, **إعادة تعبئة جميع القوائم** re-seeds all
  of them. Both **delete the current content first**, so seeding twice never produces duplicates;
  both ask for confirmation.

It is visible to site administrators and to users with `ManageWeb` or `EditListItems`.

```aspx
<%@ Register TagPrefix="pnu" TagName="PgAdmin"
    Src="~/_controltemplates/15/PNU.Internet/PostgraduateKit/Admin/ucPgAdmin.ascx" %>

<pnu:PgAdmin runat="server" />
<%-- or restricted to one list: --%>
<pnu:PgAdmin runat="server" FixedListName="PgFaq" />
```

## Multi-line field formats

Several fields hold more than one value; one entry per line.

| Field                                    | Format                          |
| ---------------------------------------- | ------------------------------- |
| `Paragraphs`, `ArticleText`, `FooterParagraphs` | one paragraph per line   |
| `Bullets`, `Conditions`                  | one bullet per line             |
| `Forms`                                  | `Form name\|https://…`          |
| `Highlights`                             | `Text\|icon-file-name.png`      |
| `BreadcrumbText`                         | `Title\|Url;Title\|Url;Title\|` |

## Provisioning

Lists are created on first render (`PgListProvisioner.EnsureAllListsExist`, guarded per request
and per web) and by `PostgraduateKitListsFeatureReceiver` on feature activation. A list is seeded
when it is created or when it is empty; existing content is never overwritten except by the
explicit re-seed buttons. Add the receiver to the feature that already deploys the other
CONTROLTEMPLATES features, the way `CentralLibraryListsFeatureReceiver` is wired.

## Master page, page layouts and assets

The kit pages use their own master page and their own copy of the design assets, because the
v3.0 design ships a **different build of `style.min.css`** from the one deployed under
`/Style Library/DGA/` and adds a `custom.css`. Nothing outside these pages is touched.
(`app.js`, `bootstrap.bundle.min.js` and `swiper-bundle.min.js` are byte-identical to the
deployed ones, so only the stylesheets actually differ.)

### 1. Deploy the assets

Copy the whole `DGA_NewDesign/v3.0 3/public/` folder to
**`/Style Library/PostgraduateKit/public/`** in the site collection. That is the only asset
step: it brings the v3.0 stylesheets, the fonts, the vendor scripts and all 34 icons the
design references, at the paths the master page and the seed data already expect
(`PgListSchema.AssetRoot`).

To save space later, the `fonts/` and `images/` sub-folders can be pointed back at
`/Style Library/DGA/public/` — they are identical — by editing `AssetRoot`/`ImageRoot` in
`PgListSchema.cs` and the font and logo `<link>` tags in the master page.

### 2. Upload the master page

Upload `MasterPages/PostgraduateKit.master` to `_catalogs/masterpage` and publish it. It is
`DGA_Internal.master` with four differences:

- every stylesheet, font and script comes from `/Style Library/PostgraduateKit/public/`;
- it renders `ucPgSecondBar` (the strip the design adds above the navigation);
- it renders `ucPgSiteFooter` instead of `ucHomeFooter`;
- it has no title band, because the kit renders its own hero and breadcrumb through
  `ucPgHeader`, and the outer wrapper is a `<div class="pg-page">` rather than a second
  `<main>`, so the `<main id="main-content">` the kit emits is the page's only `<main>`.

It keeps the shared **`ucHomeHeader`**, so the mega-menu stays driven by SharePoint navigation
and the search box, the AR/EN switch, the canonical link and the meta-description injection all
keep working. The design's header is a static snapshot of that same menu; reproducing it
verbatim would have frozen the navigation and dropped those behaviours.

The design places its new strip *between* the government signature bar and the sticky
navigation, and both of those live inside `ucHomeHeader`. Instead of editing that shared
control, `ucPgSecondBar` moves itself in the control tree to sit directly after the control
named by its `AnchorControlId` property (`ucHomeDigitalStamp` by default). If the anchor is
ever renamed or removed, the strip simply renders where the master page puts it — above the
header — and logs a note.

### 3. Upload the page layouts

Upload these four from `PageLayouts/` to `_catalogs/masterpage`, associate them with the
publishing page content type, and publish:

| Layout                             | Page                   |
| ---------------------------------- | ---------------------- |
| `PostgraduateKitHub.aspx`          | الصفحة الرئيسة للحقيبة |
| `PostgraduateKitApplicants.aspx`   | المتقدمون              |
| `PostgraduateKitEnrolled.aspx`     | المقيدون               |
| `PostgraduateKitGraduates.aspx`    | الخريجون               |

Each one inherits `PostgraduateKitPageBase`, which forces
`PostgraduateKit.master` in `OnPreInit` exactly the way `DGAPageBase` forces
`DGA_Internal.master`. An editor creates a page, picks the matching layout, and is done —
there is no web part to configure. Each layout also leaves a web part zone below the kit for
anything extra.

### The kit stylesheet

The master deliberately does **not** link the design's `custom.css`. Every rule in that file is
written as `.custom-skin .foo`, and the design applies `custom-skin` to `<body>`; adopting that
class would also pull in unrelated homepage tweaks (the 252px `.icon-container` circle, centred
`.custom-card`) onto the shared header and footer.

Instead `ucPgStyles.ascx` carries the same rules scoped under `.pg-kit` — `.custom-title`,
`.custom-paragraph`, `.fs-64`, `.fw-custom-bold`, `.fs-custom-4`, `.border-custom-10`, the accent
border colours, `.step-icon`, `.faq-icon`, `.custom-link-collapse`, `.list-group-item-primary`,
`.bg-custom-primary` — which is every rule the kit markup actually uses. `#secondBar` lives in
the master's own style block because it sits outside `.pg-kit`. So the kit styling has one
source, it works on the kit master page and equally on any ordinary DGA master, and it cannot
leak into the rest of the site. Leave `ShowStyles` at its default.

## Deviations from the static design

These are all editable afterwards from the admin control; they are listed so the changes are not
mistaken for transcription errors.

1. **Corrected headings copied from another axis.** In `phase02.html`, axis two is headed
   "المحور الثاني: البرامج وشروط القبول والرسوم" while its content and its lead paragraph are the
   regulations; it is seeded as "المحور الثاني: الحقوق والواجبات واللوائح النظامية". Likewise
   axis seven and axis eight are both headed "المحور السابع/الثامن: الخدمات / المراكز"; axis
   seven is seeded as "المحور السابع: الأدلة الرسمية", matching its own lead paragraph.
2. **Corrected spelling** in the fee axis heading: "إعفاء وتخفيض الرسول الدراسية … الدرسات العليا"
   → "إعفاء وتخفيض الرسوم الدراسية … الدراسات العليا".
3. **Duplicate note removed.** The fee notes in the design repeat "يطبق التخفيض الأعلى في حال
   استحقاق المستفيد لأكثر من تخفيض" twice; it is seeded once (9 notes rather than 10).
4. **Graduate identity label.** The ninth attribute group is labelled "القيادة:" twice in the
   design; the second group (communication skills) is seeded as "التواصل".
5. **Realigned graduate FAQ answers.** In `phase03.html` the answers from question five onwards
   are shifted by one (the answer under "كيف أتابع تغيّر حالتي إلى متخرّجة؟" describes submitting
   the thesis, and so on). The pairs are realigned, and the question that was left without an
   answer is answered from the documented steps of the same page.
6. **Corrected form links.** In `phase03.html` the "تقرير لجنة مناقشة الرسالة العلمية" link points
   at the amendment-acknowledgement document; it is seeded with the committee report URL used by
   the same procedure in `phase02.html`.
7. **Headings.** The design puts a visually hidden `<h1>` on the page and renders the audience
   name as `<h2>`; the control renders it as the page `<h1>`, and the sub-blocks inside a
   procedure card use `<h4>` instead of the `<h2>` in the static markup, so the outline is
   `h1 → h2 (axis) → h3 (procedure) → h4 (requirements / article / forms)`.
8. **Placeholder links kept.** Where the design has `href="#"` (لائحة إسكان الطالبات, and the
   four "رابط الخدمة" links of the student-initiated procedures) the seed keeps `#`; these need
   the real URLs before the pages go live.
9. **Landing page carousel trimmed.** `index.html` carries three carousel slides, but only the
   first is about the kit; the other two ("أخبار جامعة الأميرة نورة", "جامعة الأميرة نورة") are
   leftovers from the university home page template. Only the kit slide is seeded — add rows of
   kind `Slide` to `PgHub` for any more.
10. **Station card icons unscrambled.** The design puts `gratued.png` on المتقدمون and
   `apply.png` on الخريجون; they are seeded the other way round so each card matches its icon.
11. **Page URLs.** The breadcrumbs and the landing page cards are seeded with
   `/ar/Deanship/postgraduate/Pages/{academic-kit,applicants,enrolled,graduates}.aspx` and their
   `/en/` counterparts. Rename them in `PgHeader`, `PgHub` and `PgTopBar` if the pages end up at
   different URLs.
