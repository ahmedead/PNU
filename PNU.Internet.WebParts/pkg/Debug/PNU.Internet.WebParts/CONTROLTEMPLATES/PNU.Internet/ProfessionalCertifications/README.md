# Professional Certifications Guide (دليل الشهادات الاحترافية)

SharePoint 2019 modular ASCX user controls and automatic list provisioner for the **Professional Certifications Guide** feature based on `(dga)professional-certifications-guide.html`.

## Target Web Location
All SharePoint lists are hosted under **`/ar/Agencies/AcademicAffairs/`** (configured via `PcTargetWeb.cs`).

## Architecture & File Structure

```
CONTROLTEMPLATES\PNU.Internet\ProfessionalCertifications\
├── Infrastructure/
│   ├── PcListNames.cs
│   ├── PcListSchema.cs
│   ├── PcListProvisioner.cs
│   ├── PcTargetWeb.cs
│   ├── PcHelper.cs
│   ├── PcLog.cs
│   ├── PcModels.cs
│   └── PcSectionBase.cs
├── Controls/
│   ├── ucPcHeader.ascx (.cs, .designer.cs)
│   ├── ucPcGuide.ascx (.cs, .designer.cs)
│   └── ucProfessionalCertifications.ascx (.cs, .designer.cs)
├── Admin/
│   └── ucPcAdmin.ascx (.cs, .designer.cs)
├── Features/
│   └── ProfessionalCertificationsListsFeatureReceiver.cs
└── README.md
```

## SharePoint Lists Provisioned
1. **`PcHeader`**: Hero title, descriptions, export button, and breadcrumb config.
2. **`PcCertifications`**: Professional certification cards with colleges, departments, providers, requirements, topics, and validity.

## Admin Features
- `ucPcAdmin.ascx`: Generic CRUD editor for all lists with an **"إدراج البيانات الافتراضية" (Seed Default Data)** button.
