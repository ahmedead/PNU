# Page Creator Module — PNU.Internet.WebParts

Admin tool hosted on **/ar/ITAdmin/** that creates publishing pages across the portal from templates, optionally on both `/ar/` and `/en/` webs.

Page creation uses the **same `EnsurePage` pattern as `SideMenuListProvisioner` / `PageGenerator.CreatePublishingPage`** and the **existing `ControlLoaderWebPart`** (resolved via reflection — no new web part, no compile-time reference).

## Files → Visual Studio project locations

| File | Project location |
|---|---|
| `ITAdmin/ucPageCreator.ascx` (+ `.cs`, `.designer.cs`) | `CONTROLTEMPLATES/PNU.Internet/ITAdmin/` |
| `Classes/PageCreatorModels.cs` | `CONTROLTEMPLATES/Classes/` |
| `Classes/PageCreatorProvisioner.cs` | `CONTROLTEMPLATES/Classes/` |
| `Classes/busclsPageCreator.cs` | `CONTROLTEMPLATES/Classes/` |

References: `Microsoft.SharePoint.dll`, `Microsoft.SharePoint.Publishing.dll`.

## Lists (auto-provisioned on the ITAdmin web at first authenticated visit)

**PageTemplates** — `Title` (TemplateName), `PageLayoutURL` (Note/plain), `UserControlPath` (Note/plain), `DefaultProperties` (Note/plain), `IsActive` (Yes/No).
Example row: `College Programs` | `/_catalogs/masterpage/NewSideMenu.aspx` | `PNU.Internet/Colleges/DGA/ucCollegeProgramsDga.ascx` | `RowsCount=6`
(`UserControlPath` uses the same relative form as `SideMenuListProvisioner`'s `CTRL_*` constants — whatever `ControlLoaderWebPart.UserControlPath` expects.)

**PageLayoutsCatalog** — `Title` (Name), `PageLayoutURL` (Note/plain), `IsActive` (Yes/No).
(Named `PageLayoutsCatalog` internally to avoid confusion with the built-in layouts gallery; the requirement's "PageLayouts" list is this one.)

**PageCreatorAdmins** — `Title` (display name), `UserLogin` (`domain\user`, claims prefix handled).
The deploying user is seeded automatically on first creation so you are never locked out. Site collection admins always pass.

## How a page is created (`busclsPageCreator`)

1. `ucPageCreator` validates input (URL, page name sanitized to `name.aspx`, at least one title, template or layout chosen) and builds a `clsPageCreateRequest`. Selecting a template auto-fills the layout, user control path, and default properties.
2. `CreatePages` normalizes the URL, derives the paired web (`/ar/` ↔ `/en/` swap) when "both sites" is checked, and calls `CreateOnWeb` per target under `RunWithElevatedPrivileges` (logs skip/OK/error per web for the results panel; `CheckPageExists` guards duplicates).
3. **`EnsurePage(web, pageName, titleAr, titleEn, userControlPath, userControlProperties, pageLayoutUrl)`** — identical sequence to `SideMenuListProvisioner.EnsurePage`:
   - no-op if the page file already exists;
   - resolves the layout from the root web file, preferring the instance in `GetAvailablePageLayouts()`;
   - creates the `PublishingPage`; `Title` = Arabic when `web.Language == 1025`, else English;
   - `AddUserControlToPage` adds `ControlLoaderWebPart` (type `PNU.Internet.WebParts.ControlLoaderWebPart.ControlLoaderWebPart`, loaded via `Assembly.Load`) into zone **`TopZone`**, setting `UserControlPath` and, when supplied, `UserControlProperties`;
   - CheckIn → Approve (if moderation) → Publish (if minor versions).

Default layout when none supplied: `/_catalogs/masterpage/NewSideMenu.aspx` (`PAGE_LAYOUT_URL` constant — adjust if needed, as with `WP_ZONE_ID`).

## Deployment checklist

- [ ] Layouts used must contain the `TopZone` web part zone (same as `NewSideMenu.aspx` / `DGANewBlankWithSideMenu.aspx`).
- [ ] `ControlLoaderWebPart` already deployed (assembly `PNU.Internet.WebParts, Version=1.0.0.0, ... PublicKeyToken=bbc777e63fab09a3`) — nothing new to deploy for it.
- [ ] Create the tool page on `/ar/ITAdmin/` and add `ucPageCreator`.
- [ ] Retract → Delete → Deploy, then `iisreset`.
- [ ] Verify as: site admin, a `PageCreatorAdmins` user, and an unauthorized user (should see the denied alert).

## Conventions honored

- No `<%= %>` in markup; labels via `asp:Label`/`asp:Literal` set in `SetTitles()`; results repeater bound with `Eval()` on a DTO.
- Dropdowns rebound on every `Page_Load` (ViewState unreliable in SP zones); posted selections restored from `Request.Form`.
- `SPSecurity.RunWithElevatedPrivileges` with site/web reopened inside delegates; `AllowUnsafeUpdates` saved/restored exactly as in `EnsurePage`.
- URLs stored in plain-text Note fields; `Publics.WriteToLog(url, context, message)` everywhere.
- Bilingual labels via `T(key, ar, en)` with `PNUres` lookup and hardcoded fallbacks.
