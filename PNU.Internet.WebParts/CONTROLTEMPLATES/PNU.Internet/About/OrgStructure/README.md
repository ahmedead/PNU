# Org Structure user control (ucOrgStructure)

List-driven version of `org-structure.html`. The SVG chart geometry stays fixed in
markup; each box's content (title, description, badge, meta, icon, theme, details
link) is loaded from a SharePoint list so editors can maintain it without a redeploy.

## Files

| File | Purpose |
|------|---------|
| `OrgStructureUnit.cs` | DTO for one unit (bilingual fields + `LocalizedX` helpers). |
| `OrgStructureProvisioner.cs` | Creates the `OrgStructureUnits` list + fields, grants anonymous read, seeds all **84** default units (every labelled box on the chart) **only on first creation** (`out bool created` pattern — never re-seeds). Titles were read from the chart's rendered labels. |
| `OrgStructureRepository.cs` | Reads (elevated, for anonymous visitors) + admin CRUD. Manual `Safe*` field mapping. |
| `ucOrgStructure.ascx / .cs / .designer.cs` | Display control. Emits `window.__pnuOrgUnits` (list data) + `window.__pnuOrgConfig` (bilingual fallbacks); client script binds them to the SVG rects by selector. |
| `ucOrgStructureAdmin.ascx / .cs / .designer.cs` | In-page editor gated by the `PortalAdmins` list (create / edit / delete units). |
| `Elements.xml` | `SafeControl` registration. |

## How the join works

Every `<rect>` in the SVG is matched to a list item through the **`OrgSelector`**
column, which holds the exact CSS selector for that rect, e.g.
`rect[x="482.5"][y="79.5"][width="280"][height="56"]`. Boxes with no list item fall
back to a generic descriptor derived from their fill/stroke color (client-side).

## List: `OrgStructureUnits`

| Column | Internal name | Type |
|--------|---------------|------|
| Title (AR) | `Title` | Text |
| Title (EN) | `TitleEn` | Text |
| Order | `OrgOrder` | Number |
| Selector | `OrgSelector` | Note (plain) |
| Description AR/EN | `OrgDesc` / `OrgDescEn` | Note (plain) |
| Badge AR/EN | `OrgBadge` / `OrgBadgeEn` | Text |
| Meta AR/EN | `OrgMeta` / `OrgMetaEn` | Text |
| Icon | `OrgIcon` | Text |
| Theme | `OrgTheme` | Text (`primary`/`sa`/`saSoft`/`saMuted`) |
| Link URL | `OrgLinkUrl` | Note (plain) |
| Active | `OrgActive` | Boolean |

## Deploy

1. Add all files to `PNU.Internet.WebParts` under
   `ControlTemplates\PNU.Internet\About\OrgStructure\` (SharePoint mapped folder).
2. Include `Elements.xml` in the feature that provisions control templates.
3. Drop `ucOrgStructure` on the org-structure publishing page (and
   `ucOrgStructureAdmin` on an editor-only page or the same page for admins).
4. Retract → Delete → Deploy, then `iisreset` if the assembly changed.
5. First authenticated page load auto-creates the list and seeds the 27 units.

## Conventions honored

- No `<%= %>` / `<%# %>` outside repeater `Eval()`; labels set from code-behind.
- No server controls referenced as fields inside the Repeater template.
- Anonymous read via `SPSecurity.RunWithElevatedPrivileges` (site/web reopened by ID).
- Provisioning gated to authenticated users in `OnInit`.
- ViewState-safe: grid rebinds every load; identity via `HiddenField` / command args.
- Bilingual AR/EN via `PortalHelper.IsArabic` with `Web.Language == 1025` fallback.
- `TextMode` configured in markup only; `Note` fields set to plain text.
