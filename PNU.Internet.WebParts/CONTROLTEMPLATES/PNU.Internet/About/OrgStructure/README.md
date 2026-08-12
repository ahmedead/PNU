# Org Structure user control (ucOrgStructure) — bilingual, list-driven

List-driven, fully bilingual version of `org-structure.html`.

**Key idea (Option B):** the original SVG had every label baked in as Arabic vector
`<path>` outlines — not text — so it could never show English. This control uses a
**label-stripped SVG skeleton** (boxes, connector lines and dots only, *no text*) and
renders every label as an **HTML overlay** positioned over its box from the
`OrgStructureUnits` list. Result: one chart serves Arabic *and* English, labels flip
language instantly, and content editors change any label without a redeploy.

## How it works

1. The `.ascx` embeds the skeleton SVG (`viewBox 0 0 1237 1650`) — pure decoration.
2. Code-behind reads active units, and for each one parses the box geometry from the
   `Selector` column (`rect[x="..."][y="..."][width="..."][height="..."]`), converts it
   to percentage `left/top/width/height`, and emits an absolutely-positioned
   `.org-box-label` div inside `.org-overlay`.
3. Labels scale with the chart via CSS container-query units (`cqw`) and wrap with a
   3-line clamp. Text color per box comes from the `OrgTextColor` column (the original
   glyph color, e.g. white on dark boxes, teal on white boxes).
4. The label divs *are* the interactive units — hover/focus shows the tooltip card,
   click follows the details link. The SVG needs no JS.

## Files

| File | Purpose |
|------|---------|
| `OrgStructureUnit.cs` | DTO. Bilingual fields + `LocalizedX(isArabic)`, geometry parsed from `Selector` (`BoxX/Y/W/H`), and `OverlayStyle(isArabic)` producing the label's position+color CSS. |
| `OrgStructureProvisioner.cs` | Creates the `OrgStructureUnits` list + fields, grants anonymous read, seeds all **84** units (every labelled box) **only on first creation**. |
| `OrgStructureRepository.cs` | Elevated read for anonymous visitors + admin CRUD, manual `Safe*` mapping. |
| `ucOrgStructure.ascx / .cs / .designer.cs` | Skeleton SVG + list-bound overlay `Repeater` + tooltip. Sets `dir=rtl/ltr` per language. |
| `ucOrgStructureAdmin.ascx / .cs / .designer.cs` | In-page editor gated by `PortalAdmins` (create/edit/delete, incl. selector, theme, text color, link). |
| `Elements.xml` | `SafeControl` registration. |

## List: `OrgStructureUnits`

| Column | Internal name | Type |
|--------|---------------|------|
| Title (AR) | `Title` | Text |
| Title (EN) | `TitleEn` | Text |
| Order | `OrgOrder` | Number |
| Selector (box geometry) | `OrgSelector` | Note (plain) |
| Description AR/EN | `OrgDesc` / `OrgDescEn` | Note (plain) |
| Badge AR/EN | `OrgBadge` / `OrgBadgeEn` | Text |
| Meta AR/EN | `OrgMeta` / `OrgMetaEn` | Text |
| Icon | `OrgIcon` | Text |
| Theme | `OrgTheme` | Text |
| Text color | `OrgTextColor` | Text (CSS color / `var(--dga-primary-*)`) |
| Link URL | `OrgLinkUrl` | Note (plain) |
| Active | `OrgActive` | Boolean |

## Bilingual content

All 84 units are seeded with both Arabic and English titles/badges/descriptions.
Language is chosen at render time via `PortalHelper.IsArabic` (fallback
`Web.Language == 1025`); an empty English value falls back to Arabic.

## Control settings (properties)

Both controls expose:

- **`ListWebUrl`** — server-relative URL of the web that holds the list.
  Default `"/ar/AboutUniversity/"`. The list is provisioned and read on that web
  (opened by URL under elevated privileges for anonymous read).
- **`IconCssUrl`** (display control only) — URL of the Hugeicons stylesheet to inject
  so box icons render as glyphs. Default
  `"/_layouts/15/PNU.Internet/vendor/hugeicons/hgi-stroke-rounded.css"`.

Set them in the tag if your paths differ, e.g.:

```aspx
<uc:ucOrgStructure runat="server" ListWebUrl="/ar/AboutUniversity/"
    IconCssUrl="/Style Library/PNU/vendor/hugeicons/hgi-stroke-rounded.css" />
```

## Icons & legend

- Each box shows its Hugeicons glyph (`hgi hgi-stroke <Icon>`) next to the title.
  If you saw the raw class text (`hgi-user-group`, …) on the page, the Hugeicons
  **font/CSS was not loaded** on that site — deploy the `hugeicons` vendor folder to
  the `IconCssUrl` path (CSS **and** its font files together), or point `IconCssUrl`
  at wherever it already lives.
- The legend (color key) labels are re-rendered as a bilingual overlay, so the key
  reads correctly in Arabic and English.

## Deploy

1. Add all files to `PNU.Internet.WebParts` under
   `ControlTemplates\PNU.Internet\About\OrgStructure\` (SharePoint mapped folder).
2. Include `Elements.xml` in the feature that provisions control templates.
3. Deploy the Hugeicons vendor folder (CSS + fonts) to the `IconCssUrl` location.
4. Place `ucOrgStructure` on the org-structure page under `/ar/AboutUniversity/`;
   put `ucOrgStructureAdmin` on an editor page (or the same page for admins).
5. Retract → Delete → Deploy, then `iisreset` if the assembly changed.
6. First authenticated page load auto-creates the list on `/ar/AboutUniversity/` and
   seeds the 84 units. (The editor account needs no "Manage Lists" — provisioning runs
   elevated.)

## Conventions honored

- No `<%= %>` / `<%# %>` outside repeater `Eval()`; overlay values HTML-encoded in code-behind.
- No server controls referenced as fields inside the Repeater template.
- Anonymous read via `SPSecurity.RunWithElevatedPrivileges` (site/web reopened by ID).
- Provisioning gated to authenticated users in `OnInit`; ViewState-safe rebinding.
- Bilingual AR/EN; `Note` fields plain text; `TextMode` set in markup only.
- Modern CSS only (container queries, `color-mix`) — matches the DGA design system the
  original page already relies on.
