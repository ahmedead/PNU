# Central Library (المكتبة المركزية) — DGA User Controls

Splits the Central Library page into **8 list-driven user controls**, a **main container control** that hosts them all, and a **generic CRUD admin control** for content editors.
SharePoint 2019 on-premise, farm solution, C# + ASCX.

Namespace: `PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls`  
Deploy path: `CONTROLTEMPLATES\PNU.Internet\CentralLibrary\...`  
Target web: `/ar/UniversityLife`

## Directory Structure

```
Infrastructure/
  ClListNames.cs         List-name constants (9 lists)
  ClListSchema.cs        Single source of truth: lists, fields, labels, seed data
  ClListProvisioner.cs   Idempotent C# provisioner with manual & auto seeding
  ClTargetWeb.cs         Resolves /ar/UniversityLife (the web that holds the lists)
  ClHelper.cs            Language, Safe* readers, HtmlEncode, GetRes
  ClLog.cs               Log wrapper delegating to Publics.WriteToLog
  ClModels.cs            ClHeaderModel, ClAboutModel, ClCard DTOs
  ClSectionBase.cs       Shared base class for section controls
Controls/
  ucCentralLibrary.ascx  Main container control (hosts all sections)
  ucClHeader.ascx         Breadcrumb & Hero Header
  ucClAbout.ascx         عن المكتبة المركزية + المكتبة في أرقام
  ucClAwards.ascx        جوائز المكتبة المركزية
  ucClServices.ascx      خدمات المكتبة المركزية (Swiper slider)
  ucClCollections.ascx   المجموعات والمصادر
  ucClFacilities.ascx    المرافق والخدمات
  ucClFaq.ascx           الأسئلة الشائعة (Accordion)
  ucClContact.ascx       معلومات الزيارة والتواصل
Admin/
  ucClAdmin.ascx         Generic add / edit / delete editor with Seed Default Data button
Features/
  CentralLibraryListsFeatureReceiver.cs  Optional activation-time provisioning
```

## Provisioned Lists (on `/ar/UniversityLife`)

| List Internal Name | Description |
|---|---|
| `ClHeader` | Breadcrumb, hero title, subtitle, search & database buttons |
| `ClAbout` | About section text, paragraphs, banner images, numbers title |
| `ClNumbers` | Stat counters (StatValue, IconClass, ItemOrder) |
| `ClAwards` | Library awards (Description, SubTitle [Year], LogoUrl, ItemOrder) |
| `ClServices` | Slider services (Description, BadgeText, IconClass, LinkUrl, ButtonText, ItemOrder) |
| `ClCollections` | Collections & shelf numbers (Description, SubTitle [Location], LocationLabel, IconClass, ItemOrder) |
| `ClFacilities` | Facility cards (Description, ImageUrl, BadgeText [Comma separated tags], ItemOrder) |
| `ClFaq` | Frequently Asked Questions (Description [Answer], ItemOrder) |
| `ClContact` | Working hours & contact details (Working hours rows, Phone numbers, Email, Location note) |

## Usage Examples

### Main Container
```aspx
<%@ Register TagPrefix="pnu" TagName="CentralLibrary"
             Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucCentralLibrary.ascx" %>

<pnu:CentralLibrary runat="server" ID="ucLibrary" />
```

### Standalone Section Control
```aspx
<%@ Register TagPrefix="pnu" TagName="ClAbout"
             Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Controls/ucClAbout.ascx" %>

<pnu:ClAbout runat="server" ID="ucAbout" ShowTitle="True" />
```

### Admin Control
```aspx
<%@ Register TagPrefix="pnu" TagName="ClAdmin"
             Src="~/_controltemplates/15/PNU.Internet/CentralLibrary/Admin/ucClAdmin.ascx" %>

<pnu:ClAdmin runat="server" ID="ucAdmin" />
```
