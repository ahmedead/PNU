# Postgraduate Category Programs Controls (برامج الدراسات العليا حسب الفئات)

Modular SharePoint 2019 ASCX user controls, an automatic list provisioner with complete bilingual seed data (45+ programs), and an administrative CRUD control for the 5 postgraduate program category pages of Princess Nourah bint Abdulrahman University:

| Category Key | Arabic Category Name | Example Live URL |
| :--- | :--- | :--- |
| `NouraStaff` | **كوادر نورة** | `/ar/RegAdm/PGD/Pages/pnur.aspx` |
| `Humanitarian` | **البرامج الإنسانية** | `/ar/RegAdm/PGD/Pages/HumanitarianPror.aspx` |
| `Interdisciplinary` | **البرامج البينية** | `/ar/RegAdm/PGD/Pages/Interfaceprogr.aspx` |
| `Scientific` | **البرامج العلمية** | `/ar/RegAdm/PGD/Pages/SciencesProgr.aspx` |
| `Health` | **البرامج الصحية** | `/ar/RegAdm/PGD/Pages/healthprogr1.aspx` |

---

## No Server-Side PowerShell Required

Provisioning and data seeding are **100% automated and self-contained within C# / SharePoint**:
1. **Auto-Provisioning on First Load**: `ucPgCategoryPrograms.ascx` and `ucPgCategoryAdmin.ascx` automatically provision and seed the lists in the background (using `SPSecurity.RunWithElevatedPrivileges`) on the first page load if they do not already exist.
2. **SharePoint Feature Activation**: `PgCategoryProgramsFeatureReceiver.cs` automatically provisions and seeds the lists whenever the feature is activated via SharePoint Solution / Site Features UI.
3. **Web-Based Admin Control**: `ucPgCategoryAdmin.ascx` provides one-click buttons to initialize, refresh, or re-seed data directly from the web browser.
4. *(Optional)* `Provision-PgCategoryPrograms.ps1` is provided only as an optional offline reference.

```
CONTROLTEMPLATES/PNU.Internet/RegAdm/PGD/
├── Infrastructure/
│   ├── PgCategoryModels.cs            // Data models & category enums
│   ├── PgCategorySeedData.cs          // Complete live seed data for 45+ programs
│   ├── PgCategoryProvisioner.cs       // Idempotent list creation & reseeding logic
│   └── PgCategoryRepository.cs        // Data access, bilingual picking & CRUD operations
├── Controls/
│   ├── ucPgCategoryPrograms.ascx      // Display User Control (DGA UI)
│   ├── ucPgCategoryPrograms.ascx.cs
│   └── ucPgCategoryPrograms.ascx.designer.cs
├── Admin/
│   ├── ucPgCategoryAdmin.ascx         // Admin CRUD User Control
│   ├── ucPgCategoryAdmin.ascx.cs
│   └── ucPgCategoryAdmin.ascx.designer.cs
└── Provision-PgCategoryPrograms.ps1   // PowerShell provisioning script
```

---

## 1. Using the Display User Control (`ucPgCategoryPrograms.ascx`)

Register the control on your `.aspx` page, Page Layout, or embed via SharePoint Web Part:

```aspx
<%@ Register TagPrefix="pnu" TagName="PgCategoryPrograms" 
    Src="~/_controltemplates/15/PNU.Internet/RegAdm/PGD/Controls/ucPgCategoryPrograms.ascx" %>

<!-- 1. Explicit Category in Markup -->
<pnu:PgCategoryPrograms runat="server" ProgramCategory="NouraStaff" />

<!-- 2. Humanitarian Category -->
<pnu:PgCategoryPrograms runat="server" ProgramCategory="Humanitarian" />

<!-- 3. Interdisciplinary Category -->
<pnu:PgCategoryPrograms runat="server" ProgramCategory="Interdisciplinary" />

<!-- 4. Scientific Category -->
<pnu:PgCategoryPrograms runat="server" ProgramCategory="Scientific" />

<!-- 5. Health Category -->
<pnu:PgCategoryPrograms runat="server" ProgramCategory="Health" />

<!-- 6. Auto-detect from URL or query string ?Category=... -->
<pnu:PgCategoryPrograms runat="server" />
```

### Available Properties:
- `ProgramCategory`: Category key or name (`NouraStaff`, `Humanitarian`, `Interdisciplinary`, `Scientific`, `Health` or Arabic names e.g. `كوادر نورة`).
- `ShowHero`: Show/hide top hero overview card (default `true`).
- `ShowBanner`: Show/hide electronic admission portal banner (default `true`).
- `ShowQuickCards`: Show/hide bottom quick links cards (default `true`).

---

## 2. Using the Admin CRUD User Control (`ucPgCategoryAdmin.ascx`)

Place the admin control on an administrative page:

```aspx
<%@ Register TagPrefix="pnu" TagName="PgCategoryAdmin" 
    Src="~/_controltemplates/15/PNU.Internet/RegAdm/PGD/Admin/ucPgCategoryAdmin.ascx" %>

<pnu:PgCategoryAdmin runat="server" />
```

### Admin Features:
- Filter programs by category or view all.
- Real-time search across program names, colleges, departments, and degrees.
- Add / Edit / Delete programs with validation.
- Edit category hero overview titles, descriptions, Banner SIS links, and bottom cards.
- **إعادة تعبئة هذه الفئة**: Reseeds the selected category with fresh default data.
- **إعادة تعبئة جميع الفئات**: Reseeds all categories and overview headers without leaving duplicate records.

---

## 3. SharePoint Lists Provisioned

1. **`PgCategoryOverview`**:
   - `CategoryKey`, `HeroTagAr`, `HeroTagEn`, `HeroSubTagAr`, `HeroSubTagEn`, `HeroTitleAr`, `HeroTitleEn`, `HeroDescriptionAr`, `HeroDescriptionEn`
   - `BannerTitleAr`, `BannerTitleEn`, `BannerDescriptionAr`, `BannerDescriptionEn`, `BannerButtonTextAr`, `BannerButtonTextEn`, `BannerUrl`
   - `Card1TitleAr`, `Card1DescAr`, `Card1Url`, `Card2TitleAr`, `Card2DescAr`, `Card2Url`, `IsActive`

2. **`PgCategoryPrograms`**:
   - `Title` (Arabic Title), `TitleEn` (English Title)
   - `CategoryKey` (`NouraStaff`, `Humanitarian`, `Interdisciplinary`, `Scientific`, `Health`)
   - `DegreeGroupKey`, `DegreeGroupTitleAr`, `DegreeGroupTitleEn`
   - `CollegeAr`, `CollegeEn`, `DepartmentAr`, `DepartmentEn`
   - `ScopeAr`, `ScopeEn` ("محلي", "عالمي", "عالمي (شراكة دولية)")
   - `DetailsUrl`, `ApplyUrl`, `ItemOrder`, `IsActive`
