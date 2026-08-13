# PNU.Internet.SearchIndex — v2

Custom search service for the PNU SharePoint 2019 portal. Replaces the
out-of-the-box SP search with a SQL-backed index that the crawler keeps
in sync via real-time event receivers, a nightly timer job, and an
admin page with task buttons.

This document covers what changed in **v2** vs v1 and how to deploy.

## What changed in v2

| Area | v1 | v2 |
|------|----|----|
| Categories | single `Category` column | **`CategoryAr` + `CategoryEn`** on every row |
| Category dropdown filter | failed in EN when row was indexed in AR | works in both languages |
| Snippets | plain text | **`<mark class='dga-hl'>` keyword highlighting** |
| Pagination | `LinkButton` (lost query string) | **`HyperLink` with full URL preserved** |
| Menu crawler crash recovery | restart from scratch | **resumes per sub-web** via `IndexedWebs` |
| Page indexing | one row per language | **one row holds AR + EN content**; URL is canonical, swapped at render time |
| Pages found in only one language | not tracked | **logged in `MissedPages` list** |
| Page indexing errors | swallowed | **logged in `IndexErrors` list with retry workflow** |
| Admin tasks | 7 buttons | **8 buttons** (added "Retry Failed") |

## Architecture overview

```
PNU.Internet.SearchIndex/
├─ Sql/               PNU_SearchIndex.sql  (re-runnable schema)
├─ DAL/               SearchIndexItem, SearchIndexDal
├─ Logging/           SearchLogger, MissedPagesLogger, IndexErrorLogger
├─ Provisioning/      SearchIndexingProvisioner   (creates 9 config lists)
├─ Config/            SearchConfigLoader, SearchConfig DTOs
├─ Indexer/           SiteRules, ContentListIndexer, FacultiesIndexer,
│                     MenuPageCrawler, SearchIndexer
├─ Tasks/             IndexingTasks  (8 button-driven jobs)
├─ Receivers/         SearchListEventReceiver, SearchEventReceiverInstaller
├─ TimerJobs/         SearchCrawlerTimerJob  (nightly 02:30)
├─ Features/          PNUSearchInfrastructure  (web-app scoped)
├─ Layouts/           SearchAdmin.aspx[.cs]
└─ ControlTemplates/  ucSearchInput, ucSearchResults
```

## The 9 provisioned lists (under `/SearchIndexing/`)

| List | Purpose |
|------|---------|
| `ContentListsConfig` | Maps SP lists to crawler. Each row carries `CategoryAr`, `CategoryEn`, content fields per language, and an optional `FilterField/FilterValue/FilterMode` for splitting one list into two categories. |
| `MenuListsConfig` | Menu lists whose items are URLs to crawl. |
| `ExcludedWebSites` | SPWeb URLs to skip. |
| `ExcludedPages` | Specific page URLs to skip. |
| `ExcludedLists` | List titles to skip wherever they appear. |
| `IndexedWebs` | Per-task progress checkpoint — drives resume-after-crash. |
| `ManualPagesToIndex` | One-off URLs to add to the index. |
| **`MissedPages`** | Pages found in one language but missing in the other. |
| **`IndexErrors`** | Pages or items that failed to index. Tick `Retry = Yes` after fixing the source, then run **Retry Failed** to re-attempt. Successful retries flip `Resolved = Yes` automatically. |

## Bilingual indexing

When the crawler indexes `https://newportal-uat.pnu.edu.sa/ar/Foo/Pages/x.aspx`,
it also tries `https://newportal-uat.pnu.edu.sa/en/Foo/Pages/x.aspx`. Both
results are merged into a single `SearchItems` row with `TitleAr/ContentAr`
and `TitleEn/ContentEn` populated. The stored `Url` is the AR canonical
form; the results page swaps `/ar/` ↔ `/en/` at render time based on
the visitor's culture (LCID 1025 = Arabic).

If only one of the two pages exists, the missing URL is recorded in the
`MissedPages` list with the URL that *was* found and the language pair.

## Resumable menu crawl

Each child SPWeb visited successfully gets a row in `IndexedWebs` keyed
by `(TaskName, WebUrl)`. On rerun the crawler skips any web with a
matching row. So:

> **You crashed at 2,800 records — clicking "Run Menus" again resumes
> from the next unprocessed web instead of restarting.**

If you actually want to start over, click the per-task **Reset** button
(or **Clear ALL task logs** for a global reset).

The `SearchAdmin` page also sets `Server.ScriptTimeout = 1800`
(30 minutes) so the IIS request timeout is no longer the bottleneck.
For very large crawls, prefer the nightly timer job — it runs under
`OWSTIMER.exe` with no HTTP timeout.

## Error logging and retry

When `MenuPageCrawler` fails on a page (HTTP 401, 404, 500, parse error,
etc.) it calls `IndexErrorLogger.Log(site, pageUrl, context, message)`.
That writes one row per page to `IndexErrors` with:

- `Title` = page URL
- `ErrorMessage` (truncated to 4000 chars)
- `ErrorContext` = where it failed (`"Fetch"`, `"Page"`, `"Faculty#42"`)
- `FirstSeenAt` / `LastSeenAt` / `Attempts`
- `Retry` (Boolean) — you set this
- `Resolved` (Boolean) — the crawler sets this on success

**Workflow when a page fails:**

1. Open `/SearchIndexing/Lists/IndexErrors` and read the error.
2. Fix the root cause (grant the app pool read permission, repair the
   broken page, update the URL, etc.).
3. Tick **Retry = Yes** on the row and save.
4. Go to the search admin page and click **Run Retry Failed**.
5. The crawler re-indexes that URL. If it succeeds, the row's
   `Resolved` flips to **Yes** automatically (history is kept).
6. If it fails again, `Attempts` increments and `Retry` is reset to No
   so you can investigate again.

The same logger is used by `ContentListIndexer` and `FacultiesIndexer`,
so list-item failures show up in the same place.

## Deployment

### 1. Database

Run `Sql/PNU_SearchIndex.sql` against your SQL instance. The script is
**re-runnable** — on a v1 database it adds the new `CategoryAr` /
`CategoryEn` columns and copies the old `Category` value into
`CategoryAr` so existing data still searches.

Grant the SharePoint app-pool account `db_datareader`, `db_datawriter`
and `EXECUTE` on `PNU_SearchIndex`.

### 2. web.config

Add to **every** front-end's web.config (under the web-application
virtual directory, NOT the central admin one):

```xml
<connectionStrings>
  <add name="PNU_SearchIndex"
       connectionString="Server=YOUR\SQLINSTANCE;Database=PNU_SearchIndex;Integrated Security=true;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

A working snippet is included as `web.config.snippet.xml`.

### 3. Build and deploy the WSP

1. Compile `PNU.Internet.SearchIndex` against .NET 4.7.2.
2. Strong-name the assembly with `PNU.Internet.SearchIndex.snk`.
3. Package as a WSP and deploy to the GAC + the web app.
4. Activate the **PNUSearchInfrastructure** feature on the web
   application:

```powershell
Enable-SPFeature -Identity "PNUSearchInfrastructure" `
                 -Url https://newportal-uat.pnu.edu.sa
```

The feature creates `/SearchIndexing/` under the root web of every
site collection in the web application and provisions all 9 config
lists. You can also re-trigger this from the admin page via the
**Re-provision config lists** button.

### 4. ANONYMOUS LOGON ("Login failed for user 'NT AUTHORITY\ANONYMOUS LOGON'")

Every DAL method runs inside `SPSecurity.RunWithElevatedPrivileges`,
which forces the SQL connection to use the application-pool identity
and avoids the Kerberos double-hop issue. No special SQL or AD config
is needed beyond granting the app-pool account access (step 1).

### 5. Page setup

Add the user controls to the AR and EN search pages:

```xml
<%@ Register TagPrefix="pnu" TagName="SearchInput"
    Src="~/_controltemplates/15/PNU.Internet.SearchIndex/ucSearchInput.ascx" %>
<%@ Register TagPrefix="pnu" TagName="SearchResults"
    Src="~/_controltemplates/15/PNU.Internet.SearchIndex/ucSearchResults.ascx" %>

<pnu:SearchInput   runat="server" id="SearchInput1"
                   ResultsPageUrl="/ar/Pages/SearchResults.aspx" />
<pnu:SearchResults runat="server" id="SearchResults1" />
```

The results control reads `q`, `cat`, `sort`, `p` from the query
string. Internal pagination links preserve all four so the highlight
and category filter survive every click.

## Custom search admin page

Navigate to `/_layouts/15/PNU.Internet.SearchIndex/SearchAdmin.aspx` (Site
Collection administrator only). Each task button:

| # | Task | What it does |
|---|------|--------------|
| 1 | Menus | Recursive site walk. Bilingual page indexing with sub-web checkpoints. |
| 2 | News | `RequestsList`, `MainCategory = الأخبار الرئيسية` |
| 3 | Digital Media | `RequestsList`, `MainCategory ≠ الأخبار الرئيسية` |
| 4 | Events | `AdvertisementsRequests` |
| 5 | E-Services | `EservicesList` |
| 6 | Faculties | `AllFaculties` (under `/Admin`) |
| 7 | Manual Pages | URLs from `ManualPagesToIndex` |
| **8** | **Retry Failed** | Re-runs every URL in `IndexErrors` where `Retry = Yes` |

Each task (except Manual Pages and Retry Failed) has its own **Reset**
button that wipes its `IndexedWebs` rows so you can force a full re-run.

## Nightly timer job

`SearchCrawlerTimerJob` runs at 02:30 daily under `OWSTIMER` and calls
`SearchIndexer.FullCrawl(siteId)` for the configured site. No HTTP
timeout applies. Provision via:

```powershell
$wa = Get-SPWebApplication https://newportal-uat.pnu.edu.sa
$wa.JobDefinitions["PNU Custom Search - nightly crawl"]
```

The feature receiver registers it automatically on activation.

## Common queries

| I want to… | Check / do |
|------------|-----------|
| See which pages failed | `/SearchIndexing/Lists/IndexErrors` |
| Find the EN counterparts I'm missing | `/SearchIndexing/Lists/MissedPages` |
| See the crawl progress | `/SearchIndexing/Lists/IndexedWebs` |
| Force a single task to start over | Click its **Reset** button on the admin page |
| Force EVERYTHING to start over | **Clear ALL task logs** on the admin page |
| Re-load config without recycling the app pool | **Reload configuration** button |
| Add a new content list | Add a row to `ContentListsConfig` and click **Reload configuration** |
| Add a one-off page | Add a row to `ManualPagesToIndex` and click **Run Manual Pages** |

## Boilerplate stripping

The crawler removes the DGA government banner, login link, registration
number `20250417424`, "محتوى الصفحة" / "Page content" headers, the
common `<header>`, `<footer>`, `<nav>` and `dga-*` HTML blocks, and a
short list of repeating phrases. See
`MenuPageCrawler.RemoveBoilerplate` for the full list — extend it
there if new boilerplate appears.

---

## Page catalog (audit tool)

The **Page Catalog** at
`/_layouts/15/PNU.Internet.SearchIndex/PageIndexAdmin.aspx` is a separate
audit tool that inventories every publishing page under a target web
and captures:

- **Page title / URL / layout**
- **User-control paths** — the `.ascx` behind each web part, or the
  full type name for framework parts (CQWP, XSLT, etc.)
- **Web-part properties** — a filtered reflection dump of every WP's
  public properties, skipping ASP.NET framework noise (`ChromeState`,
  `AllowClose`, styling, etc.) to keep the output readable

Data lives in `dbo.WebsitePages` (unique index on `PageURL`). The table
is bootstrapped automatically the first time you open the page —
`WebsitePagesDal.EnsureWebsitePagesTableExists()` creates it if
missing, so you don't need to re-run the `.sql` script just for this.

### Usage

1. Open **Site Settings → PNU Custom Search → Page Catalog** (or click
   **Page Catalog »** from the search admin page).
2. Enter a target web URL — server-relative (`/ar/Faculties/CBA`) or
   absolute. Leave blank to use the current site collection root.
3. Tick **Only index this web** to skip subsites; leave unticked to
   recurse into every descendant SPWeb.
4. **Load & Check Pages** — enumerates pages and shows a grid with
   `Indexed` / `Not Indexed` status against `dbo.WebsitePages`.
5. **Run Indexing** — upserts every loaded row into
   `dbo.WebsitePages`; the status column flips to `Indexed` on
   success.
6. **Export to Excel** — downloads a formatted `.xls` with green
   headers, striped rows, and colour-coded status cells. Opens
   natively in Excel; the file uses the HTML-as-XLS trick so no
   OpenXML dependency is required.

### Notes

- The tool ignores excluded webs (`ExcludedWebSites` config list is
  **not** consulted here — this is an audit, not an indexer, so all
  webs are inspected unless you tick the checkbox).
- Non-publishing webs fall back to inspecting the `SitePages` or
  `Pages` list if present.
- Property values longer than 800 chars are truncated with an
  ellipsis so the Excel export stays legible.
