# Noura Students (طالبات نورة) — DGA user controls

Splits the Noura Students page into **11 list-driven user controls**, a **main container**
that hosts them all, and a **generic CRUD control** for editors.
SharePoint 2019 on-premise, farm solution, C# + ASCX, no PowerShell.

Namespace: `PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls`
Deploy path: `CONTROLTEMPLATES\PNU.Internet\NouraStudents\...`

## Files

```
Infrastructure/
  NsListNames.cs         list-name constants
  NsListSchema.cs        single source of truth: lists, fields, labels, seed data
  NsListProvisioner.cs   idempotent C# provisioner
  NsTargetWeb.cs         resolves /ar/NouraStudents (the web that holds the lists)
  NsHelper.cs            language, Safe* readers, Arabic month names, GetRes
  NsLog.cs               the ONLY file that calls Publics.WriteToLog
  NsModels.cs            NsCard DTO + NsDateGroup
  NsSectionBase.cs       shared base class for all section controls
Controls/
  ucNouraStudents.ascx      main container (hosts the 11 sections)
  ucNsNumbers.ascx          طالبات نورة في أرقام
  ucNsAwards.ascx           لوحة الجوائز الطلابية
  ucNsAcademicServices.ascx الخدمات الأكاديمية  (swiper)
  ucNsQuickLinks.ascx       روابط سريعة
  ucNsStudentServices.ascx  الخدمات الطلابية  (image + icon list)
  ucNsImportantDates.ascx   تواريخ تهمك  (3 grouped columns)
  ucNsCampusLife.ascx       الحياة الجامعية
  ucNsCareer.ascx           التطوير المهني والوظيفي
  ucNsExperiences.ascx      تجارب ملهمة من طالباتنا
  ucNsFinancialSupport.ascx المنح والدعم المالي
  ucNsContact.ascx          التواصل والدعم
Admin/
  ucNsAdmin.ascx            generic add / edit / delete for all 11 lists
Features/
  NouraStudentsListsFeatureReceiver.cs   optional activation-time provisioning
```

## Lists provisioned (on **/ar/NouraStudents**)

| List | Fields beyond `Title` (Arabic title) |
|---|---|
| `NsNumbers` | Title_EN, StatValue, IconClass, ItemOrder |
| `NsAwards` | Title_EN, Description, Description_EN, IconClass, ItemOrder |
| `NsAcademicServices` | + BadgeText(_EN), LinkUrl, ButtonText(_EN) |
| `NsQuickLinks` | Title_EN, IconClass, LinkUrl, ItemOrder |
| `NsStudentServices` | Title_EN, IconClass, LinkUrl, ItemOrder |
| `NsImportantDates` | Title_EN, **EventDate**, **DateCategory** (choice), SubTitle(_EN), IconClass, ItemOrder |
| `NsCampusLife` | Title_EN, IconClass, LinkUrl, ItemOrder |
| `NsCareer` | Title_EN, Description(_EN), IconClass, LinkUrl, ItemOrder |
| `NsExperiences` | Title_EN, RoleText(_EN), Description(_EN), ImageUrl, IconClass, ItemOrder |
| `NsFinancialSupport` | Title_EN, Description(_EN), IconClass, LinkUrl, ItemOrder |
| `NsContact` | Title_EN, ContactValue(_EN), IconClass, LinkUrl, ItemOrder |

### Where the lists live

All eleven lists are provisioned on a single web — **`/ar/NouraStudents`** — no matter
which web renders the page, so the Arabic and English pages read the same bilingual items
and editors have one place to manage them. `NsTargetWeb` resolves that path against the
site collection root (so a site at a managed path such as `/sites/portal` still works) and
falls back to the current web, with a log entry, if the subweb is missing. Change
`NsTargetWeb.DefaultPath` if the lists ever move.

### Provisioning behaviour

Creating a list is **one transaction**: create → add every column → **insert the default
data** (the exact content of the static page, Arabic + English). Existing lists are never
re-seeded, so editors' changes are safe.

Every page load calls `EnsureAllListsExist()`, which opens the target web once and
runs `TryGetList` for all eleven lists. Missing lists are created, filled with columns and seeded on that request; a
per-request flag in `HttpContext.Items` stops the eleven controls from re-checking a list
that was just created in the same request.

### تواريخ تهمك

`DateCategory` is a Choice column with `مواعيد التسجيل`, `الفصول الدراسية`,
`جدول الاختبارات`. The control groups items by that column, keeping the schema order, so
the three designed columns always appear in sequence. Any extra category an editor adds
gets its own column at the end. The day/month badge is formatted with
`CultureInfo.InvariantCulture` and a hardcoded Arabic month table — never Hijri.

## Usage

```aspx
<%@ Register TagPrefix="pnu" TagName="NouraStudents"
             Src="~/_controltemplates/15/PNU.Internet/NouraStudents/Controls/ucNouraStudents.ascx" %>

<pnu:NouraStudents runat="server" ID="ucNs"
                   AcademicGuideUrl="/ar/Pages/academic-services-guide.aspx"
                   StudentServicesImageUrl="/Style Library/DGA/images/students/services.png"
                   CampusLifeImageUrl="/Style Library/DGA/images/hero/hero-campus.webp" />
```

Hide sections: `ShowNumbers`, `ShowAwards`, `ShowAcademicServices`, `ShowQuickLinks`,
`ShowStudentServices`, `ShowImportantDates`, `ShowCampusLife`, `ShowCareer`,
`ShowExperiences`, `ShowFinancialSupport`, `ShowContact` — set any to `"False"`.

Each section also works standalone:

```aspx
<pnu:NsNumbers runat="server" MaxItems="4" ShowTitle="True" />
```

Editor screen:

```aspx
<pnu:NsAdmin runat="server" />                             <%-- picker for all 11 lists --%>
<pnu:NsAdmin runat="server" FixedListName="NsNumbers" />   <%-- one list only --%>
```

## Logging

Every catch routes through one place:

```csharp
NsLog.Write("ucNsNumbers.Bind", ex);
// -> Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), userControlName, ex.Message);
```

`NsLog.cs` is the only file that references `Publics`. If it does not resolve, add the
correct `using` at the top of that one file and every control is fixed at once.

## Conventions followed

- Errors logged through `Publics.WriteToLog(url, control, message)` via `NsLog`
- Language via `web.Language == 1025`; labels via `PNUres` + hardcoded bilingual fallbacks
- No `<main>` wrapper — the master page supplies it; controls emit `<section>`
- No `<%= %>`; only `<%# %>` inside templates
- No `runat="server"` HTML controls inside `ItemTemplate` — conditional markup is
  pre-rendered on the DTO (`DescriptionHtml`, `BadgeHtml`, `ButtonHtml`, `ArrowLinkHtml`,
  `PortraitHtml`, `ContactValueHtml`) and text is HTML-encoded at projection time
- Data re-bound on **every** `Page_Load`; admin state in `HiddenField`s
- Manual `Safe*` readers and `SPFieldUrlValue` — no `[SharePointField]` mapping
- Src-registered child controls are cast before custom properties are set (avoids CS1061)

## Deployment checklist

1. Add all files to `PNU.Internet.WebParts`, mapped folder `ControlTemplates\PNU.Internet\NouraStudents`.
2. Build → Retract → Delete → Deploy the WSP.
3. `iisreset` if the GAC assembly changed.
4. Browse the page once — lists are created, columns added and default data inserted.
5. The Academic Services section needs the DGA swiper JS already present on the page.
