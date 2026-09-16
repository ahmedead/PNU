# Professional Certifications (دليل الشهادات الاحترافية) — DGA user controls

A **sibling module** to Noura Students, built on the same conventions: one list-driven
user control for the public guide, a **generic CRUD control** for editors, and an
idempotent C# provisioner that creates the list **and seeds it with the 44 certificates
delivered in the source workbook**.

SharePoint 2019 on-premise, farm solution, C# + ASCX, no PowerShell.

Namespace: `PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls`
Deploy path: `CONTROLTEMPLATES\PNU.Internet\Certifications\...`

## Files

```
Infrastructure/
  CertListNames.cs        list-name constants
  CertListSchema.cs       single source of truth: list, fields, labels, JSON keys
  CertSeedData.cs         the 44 default certificates (generated from data.xlsx)
  CertListProvisioner.cs  idempotent C# provisioner
  CertTargetWeb.cs        resolves /ar/Agencies/AcademicAffairs (the web that holds the list)
  CertHelper.cs           language, Safe* readers, GetRes, Clean, ordered query
  CertLog.cs              the ONLY file that calls Publics.WriteToLog
  CertModels.cs           CertJson (script-safe payload) + CertAdminRow
Controls/
  ucCertifications.ascx   the guide: header, search, college filter, cards,
                          details modal, list print view, single-certificate print view
Admin/
  ucCertAdmin.ascx        add / edit / delete + "restore default data"
Features/
  CertificationsListsFeatureReceiver.cs   optional activation-time provisioning
```

## List provisioned (on **/ar/Agencies/AcademicAffairs**)

`Certifications` — `Title` holds the Arabic certificate name (built-in field, re-labelled).

| Column | Type | JSON key |
|---|---|---|
| `Title` | Text | `title_ar` |
| `Title_EN` | Text | `title_en` |
| `College` | Text | `college` |
| `Department` | Text | `department` |
| `ProgramName` | Text | `Program` |
| `Description` | Note | `description` |
| `CertLevel` | Text | `level` |
| `CertLanguage` | Text | `language` |
| `Cost` | Text | `cost` |
| `DeliveryMode` | Text | `mode` |
| `IsSupported` | Text | `is_supported` |
| `Topics` | Note | `topics` |
| `Requirements` | Note | `requirements` |
| `RegistrationSteps` | Note | `registration_steps` |
| `Provider` | Note | `provider` |
| `LearningResources` | Note | `resources` |
| `Validity` | Text | `validity` |
| `ExamDuration` | Text | `exam_duration` |
| `QuestionsCount` | Text | `questions_count` |
| `QuestionsType` | Note | `questions_type` |
| `ContactOfficer` | Text | `contact_officer` |
| `DataApproved` | Text | `data_approved` |
| `CoursesCount` | Text | `courses_count` |
| `Course1` … `Course4` | Text | `course_1` … `course_4` |
| `OfficialLink` | Note | `link` |
| `ItemOrder` | Number | — |

Two naming decisions worth knowing:

- `level`, `language` and `mode` are stored as **`CertLevel`, `CertLanguage`,
  `DeliveryMode`** — `Language` in particular collides with a built-in publishing site
  column. The JSON keys stay `level` / `language` / `mode`, so the page markup and the
  client script are the ones from the static page, unchanged.
- `OfficialLink` is a **Note**, not a URL column: several official links in the workbook
  are longer than the 255 characters a SharePoint URL column accepts.

### Where the list lives

The list is provisioned on a single web — **`/ar/Agencies/AcademicAffairs`** — no matter which web
renders the page, so the Arabic and English pages read the same bilingual items and
editors have one place to manage them. `CertTargetWeb` resolves that path against the
site collection root (so a site at a managed path such as `/sites/portal` still works)
and falls back to the current web, with a log entry, if the subweb is missing. Change
`CertTargetWeb.DefaultPath` if the list ever moves.

The shared **`AdminUsers`** list on `/ar/ContentAdmin` is the single source of truth for
who may edit — the same list the Noura Students admin screen uses.

### Provisioning behaviour

Creating the list is **one transaction**: create → add every column → **insert the 44
default certificates**. Existing lists are never re-seeded, so editors' changes are safe.
`ucCertifications` calls `EnsureListExists()` on every page load, so a deleted list is
recreated, re-columned and re-seeded on the next request.

`ucCertAdmin` also offers **"استعادة البيانات الافتراضية"** (`SeedIfEmpty`), which
re-inserts the delivered rows **only into an empty list** — the way back after someone
cleared the content, without dropping the list and its columns.

### How the page gets its data

The control reads the list, projects each item onto the JSON keys above and writes the
payload into two `<script type="application/json">` blocks (`certificationsData`,
`certificationsLabels`). The client script then does search, college filtering, the
details modal and both print views **without a postback** — the exact UX of the static
page, with the content now coming from SharePoint.

The payload is escaped by `CertJson.Encode` (`<`, `>`, `&` → `\uXXXX`), so no value can
break out of the `<script>` element, and every value reaches the DOM through
`textContent`, never `innerHTML`.

## Usage

```aspx
<%@ Register TagPrefix="pnu" TagName="Certifications"
             Src="~/_controltemplates/15/PNU.Internet/Certifications/Controls/ucCertifications.ascx" %>

<pnu:Certifications runat="server" ID="ucCerts"
                    HomeUrl="/ar/Pages/default.aspx"
                    EmptyImageUrl="/Style Library/DGA/images/empty.svg" />
```

Properties: `ShowHeader`, `ShowBreadcrumb` (`"False"` hides), `HomeUrl`, `PageTitleText`,
`IntroText1`, `IntroText2`, `EmptyImageUrl`, `MaxItems` (0 = all), `ListNameOverride`.

Editor screen:

```aspx
<%@ Register TagPrefix="pnu" TagName="CertAdmin"
             Src="~/_controltemplates/15/PNU.Internet/Certifications/Admin/ucCertAdmin.ascx" %>

<pnu:CertAdmin runat="server" />
<pnu:CertAdmin runat="server" ShowSeedButton="False" />
```

## Requirements on the page

- Bootstrap 5 JS (`bootstrap.Modal`, `bootstrap.Dropdown`) — the DGA master page supplies it.
- HugeIcons (`hgi hgi-stroke ...`) and the DGA CSS (`card`, `badge-info`, `form-control-container`,
  `line-clamp`, `dga-search-input`, `dga-filter-dropdown`).
- The control degrades safely without Bootstrap: cards, search and filtering still work;
  only the modal and its dropdown auto-close do not.

## Logging

Every catch routes through one place:

```csharp
CertLog.Write("ucCertifications.Bind", ex);
// -> Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), userControlName, ex.Message);
```

`CertLog.cs` is the only file that references `Publics`. If it does not resolve, add the
correct `using` at the top of that one file and every control is fixed at once.

## Conventions followed

- Errors logged through `Publics.WriteToLog(url, control, message)` via `CertLog`
- Language via `web.Language == 1025`; labels via `PNUres` + hardcoded bilingual fallbacks
- No `<main>` wrapper — the master page supplies it; the control emits `<section>`
- No `<%= %>`; `<%# %>` expressions resolved by a single `this.DataBind()`
- No `runat="server"` HTML controls inside `ItemTemplate`
- Data re-read on **every** `Page_Load`; admin state in `HiddenField`s
- Manual `Safe*` readers — no `[SharePointField]` mapping
- C# 5 syntax only (no interpolated strings, no `?.`, no expression-bodied members)

## Deployment checklist

1. Add all files to `PNU.Internet.WebParts`, mapped folder `ControlTemplates\PNU.Internet\Certifications`.
2. Make sure the subweb **`/ar/Agencies/AcademicAffairs`** exists (or point `CertTargetWeb.DefaultPath` elsewhere).
3. Build → Retract → Delete → Deploy the WSP.
4. `iisreset` if the GAC assembly changed.
5. Browse the page once — the list is created, columns added and the 44 certificates inserted.
6. Add yourself to `AdminUsers` on `/ar/ContentAdmin` before opening the editor screen.

## Regenerating the seed

`CertSeedData.cs` is generated from the delivered workbook. To refresh it, re-export the
sheet with the same 28 headers and map them to the internal names in the table above —
one `Row("Field", "value", ...)` call per certificate, `ItemOrder` = the row number.
Values keep `\r\n` line breaks; `_x000D_` artefacts are stripped at generation time and,
as a safety net, again in `CertHelper.Clean`.
