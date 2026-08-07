# PNU.Internet.Search — Custom search service for SharePoint 2019

A drop-in custom search service for the PNU SharePoint 2019 site. It
indexes pages, list items, menus, and faculties into a custom SQL
database and exposes search through two user controls
(`ucSearchInput`, `ucSearchResults`) plus an admin page. Designed for
bilingual Arabic / English content and the PNU site IA (Faculties /
Agencies / Deanship / Departments / Centers / MediaCenter / etc.).

---

## What's in the box

```
PNU.Internet.Search/
├── Sql/PNU_SearchIndex.sql              ← run once on SQL Server
├── Logging/
│   └── SearchLogger.cs                   ← writes to ULS (replaces Publics.WriteToLog)
├── DAL/
│   ├── SearchIndexItem.cs
│   └── SearchIndexDal.cs                 ← elevated SQL calls (fixes ANONYMOUS LOGON)
├── Provisioning/
│   └── SearchIndexingProvisioner.cs      ← creates /SearchIndexing/ + 7 lists
├── Config/
│   └── SearchConfigLoader.cs             ← reads config lists, 5-min cache
├── Indexer/
│   ├── SiteRules.cs                       ← IA classification
│   ├── SearchIndexer.cs                   ← orchestrator + helpers
│   ├── ContentListIndexer.cs              ← list-backed pages
│   ├── MenuPageCrawler.cs                 ← recursive menu walk + boilerplate strip
│   └── FacultiesIndexer.cs                ← AllFaculties under /Admin/
├── Receivers/
│   ├── SearchListEventReceiver.cs         ← real-time index update
│   └── SearchEventReceiverInstaller.cs
├── Tasks/
│   └── IndexingTasks.cs                   ← 7 tasks for the admin UI
├── TimerJobs/
│   └── SearchCrawlerTimerJob.cs           ← nightly 02:30 full crawl
├── Features/
│   └── PNUSearchInfrastructure/
│       ├── Feature.xml
│       └── PNUSearchInfrastructureEventReceiver.cs
├── Layouts/PNU.Internet.Search/
│   ├── SearchAdmin.aspx                   ← /_layouts/15/PNU.Internet.Search/SearchAdmin.aspx
│   └── SearchAdmin.aspx.cs
├── ControlTemplates/PNU.Internet.Search/
│   ├── ucSearchInput.ascx                 ← search box
│   ├── ucSearchInput.ascx.cs
│   ├── ucSearchResults.ascx               ← results page
│   └── ucSearchResults.ascx.cs
├── web.config.snippet.xml
└── README.md
```

---

## Key architectural choices

1. **Custom DB instead of SP Search.** Cross-language ranking
   (Arabic/English fields side by side), custom URL builders per
   list (`/ar/MediaCenter/News/Pages/NewsDetails.aspx?RequestID={ID}`),
   sub-second results, no full-text index required.
2. **List-driven config.** All "what to crawl" knobs live in seven
   SharePoint lists under `{rootSite}/SearchIndexing/`. Operations
   add/disable a content source without redeploying the WSP.
3. **Recursive site walk.** Many PNU webs have their own
   `TopMenuLevel*` lists. The crawler descends into every web that
   menus point at, plus every direct child SPWeb.
4. **IA-aware rules.** Encoded in `SiteRules.cs`:
   - `/ar/Agencies`, `/Deanship`, `/Departments`, `/Centers` — also
     index their `AllItems` list.
   - `/ar/Faculties` — indexed via `AllFaculties` only (no recursion).
   - any web with a list whose title starts with `About` / `عن` /
     `نبذة` — also index `home.aspx` / `default.aspx`.
5. **Skip-on-rerun.** Each task records the webs it processed in
   `IndexedWebs`. Clicking the same task twice in a row is cheap.
   Use **Reset** to force a full rerun.
6. **Boilerplate stripping.** The DGA government banner ("موقع
   حكومي رسمي…", login link, registration number `20250417424`,
   `محتوى الصفحة`, etc.) and header/nav/footer markup are removed
   before indexing.
7. **Elevated SQL calls.** Every DAL method runs inside
   `SPSecurity.RunWithElevatedPrivileges` so SQL connections are made
   under the application pool account — fixing the
   `NT AUTHORITY\ANONYMOUS LOGON` Kerberos double-hop error.
8. **ULS logging.** `SearchLogger` writes to the SharePoint ULS log
   under area `PNU Custom Search`, replacing the legacy
   `Publics.WriteToLog` calls.

---

## Step 1 — Create the database

On the SQL Server instance:

```sql
:r Sql\PNU_SearchIndex.sql
```

Or open the file in SSMS and hit Execute. This creates database
`PNU_SearchIndex`, two tables, and four stored procedures. **No
Full-Text Search component is required.**

Grant the application pool identity these rights:

```sql
USE PNU_SearchIndex;
CREATE USER [DOMAIN\AppPoolAccount] FOR LOGIN [DOMAIN\AppPoolAccount];
ALTER ROLE db_datareader ADD MEMBER [DOMAIN\AppPoolAccount];
ALTER ROLE db_datawriter ADD MEMBER [DOMAIN\AppPoolAccount];
GRANT EXECUTE ON SCHEMA::dbo TO [DOMAIN\AppPoolAccount];
```

---

## Step 2 — Add the connection string

Edit `web.config` of every WFE in your web application:

`C:\inetpub\wwwroot\wss\VirtualDirectories\<port>\web.config`

Insert:

```xml
<connectionStrings>
    <add name="PNU_SearchIndex"
         connectionString="Server=SQLSERVER\INSTANCE;Database=PNU_SearchIndex;Integrated Security=true;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

## Step 3 — Build and deploy the WSP

In Visual Studio 2017+ with the SharePoint 2019 dev tools:

1. Open the project. Set **Site URL** to your PNU dev site collection.
2. **Properties → Signing → Sign the assembly** with `PNU.Internet.Search.snk`.
3. **Properties → SharePoint → Assembly Deployment Target = `GlobalAssemblyCache`**.
4. Solution Explorer → right-click project → **Package**.
5. Copy the resulting `.wsp` to a SharePoint server. Then in
   SharePoint Management Shell as farm admin:

   ```powershell
   Add-SPSolution -LiteralPath "C:\Deploy\PNU.Internet.Search.wsp"
   Install-SPSolution -Identity PNU.Internet.Search.wsp `
                      -GACDeployment `
                      -WebApplication https://your.pnu.web.app
   ```

6. Activate the feature on your web application:

   ```powershell
   Enable-SPFeature -Identity "PNUSearchInfrastructure" `
                    -Url https://your.pnu.web.app
   ```

   Activation does three things:
   - Creates `/SearchIndexing/` subsite + 7 config lists, seeds defaults.
   - Installs the nightly timer job (02:30).
   - Attaches `SearchListEventReceiver` to every existing non-system list.

---

## Step 4 — First crawl from the admin UI

Navigate to:

```
https://your.pnu.web.app/_layouts/15/PNU.Internet.Search/SearchAdmin.aspx
```

You will see seven task buttons. Click them in this order:

1. **Run Menus** — recursive site walk, indexes pages + AllItems + About-home.
2. **Run News** — `RequestsList`, `MainCategory = الأخبار الرئيسية`.
3. **Run Digital Media** — same list, opposite filter.
4. **Run Events** — `AdvertisementsRequests` under `/ar/MediaCenter/MediaCenterAdmin`.
5. **Run E-Services** — `EservicesList` under root.
6. **Run Faculties** — `AllFaculties` under `/Admin`.
7. **Run Manual Pages** — anything you've added to `ManualPagesToIndex`.

Each task shows how many items it indexed and how many webs it
skipped. Use the small **Reset** button next to each task to force a
full re-run.

---

## Step 5 — Add the user controls to a page

### Search box (anywhere)

```html
<%@ Register TagPrefix="pnu" TagName="SearchInput"
    Src="~/_controltemplates/15/PNU.Internet.Search/ucSearchInput.ascx" %>
…
<pnu:SearchInput runat="server" id="ucSearchInput1"
                 ResultsPageUrl="/ar/Pages/search.aspx" />
```

### Results page (a publishing page named `search.aspx`)

```html
<%@ Register TagPrefix="pnu" TagName="SearchResults"
    Src="~/_controltemplates/15/PNU.Internet.Search/ucSearchResults.ascx" %>
…
<pnu:SearchResults runat="server" id="ucSearchResults1" />
```

Or wrap each control inside a tiny WebPart (subclass of
`UserControlWebPart`) and place it on the page using the SharePoint
ribbon — same approach as the other PNU custom controls.

You can also store the results URL in the root web's property bag so
every search input picks it up automatically:

```powershell
$root = (Get-SPWeb https://your.pnu.web.app)
$root.AllProperties["PNU_SearchResultsUrl"] = "/ar/Pages/search.aspx"
$root.Update()
```

---

## The seven configuration lists

All under `{rootSite}/SearchIndexing/`.

| List                  | Purpose                                                 |
|-----------------------|---------------------------------------------------------|
| **ContentListsConfig**| Lists the indexer crawls as content. Supports a secondary `FilterField`/`FilterValue`/`FilterMode` to split one SP list into multiple search categories (e.g. RequestsList → News + Digital Media). |
| **MenuListsConfig**   | Menu lists whose URL field will be walked. Default seeded with TopMenuLevel1/2/3. |
| **ExcludedWebSites**  | SPWeb URLs to skip. |
| **ExcludedPages**     | Specific page URLs to skip. |
| **ExcludedLists**     | List titles to skip wherever they appear. |
| **IndexedWebs**       | Skip-on-rerun log. Each task writes one row per processed web. |
| **ManualPagesToIndex**| Page URLs to index by hand when auto-discovery misses them. |

Default seeded `ContentListsConfig` rows:

| ListTitle | ListWebUrl | Category | URL pattern | Filter |
|-----------|------------|----------|-------------|--------|
| RequestsList | /ar/MediaCenter/News | الأخبار / News | …NewsDetails.aspx?RequestID={ID} | MainCategory `Equals` الأخبار الرئيسية |
| RequestsList | /ar/MediaCenter/News | الوسائط الرقمية / Digital Media | …DigitalMediaDetails.aspx?RequestID={ID} | MainCategory `NotEquals` الأخبار الرئيسية |
| AdvertisementsRequests | /ar/MediaCenter/MediaCenterAdmin | الفعاليات والإعلانات | …AdvertisementDetails.aspx?RequestID={ID} | (none) |
| EservicesList | / | الخدمات الإلكترونية | …service-details.aspx?eti={ID} | (none) |
| AllFaculties | /Admin | الكليات / Faculties | …FacultyDetails.aspx?ID={ID} | (none) |

---

## Operations

### Force a full re-crawl

Either click **Clear ALL task logs** on the admin page and run each
task, or call `SearchIndexer.FullCrawl(siteId)` from a PowerShell
script:

```powershell
Add-Type -AssemblyName "PNU.Internet.Search,
    Version=1.0.0.0, Culture=neutral,
    PublicKeyToken=<your-token>"
[PNU.Internet.Search.Indexer.SearchIndexer]::FullCrawl((Get-SPSite https://your.pnu.web.app).ID)
```

### Add a new content list

1. Add a row to `ContentListsConfig` (Active = Yes).
2. Click **Reload configuration** on the admin page (clears the cache).
3. Click the corresponding task button (or run a Full Crawl).

### Exclude a noisy page

Add its full URL to `ExcludedPages`. Active = Yes. Reload config.

---

## Troubleshooting

### "Login failed for user 'NT AUTHORITY\ANONYMOUS LOGON'"

This is the Kerberos double-hop error. The DAL in this package wraps
every SQL call in `SPSecurity.RunWithElevatedPrivileges`, which runs
the call under the app pool account, so this error should never
appear in v1+. If it does:

1. Confirm `PNU.Internet.Search.dll` is the version actually loaded:
   `[System.Reflection.Assembly]::LoadFrom("C:\Windows\Assembly\GAC_MSIL\PNU.Internet.Search\1.0.0.0__<token>\PNU.Internet.Search.dll").GetName().FullName`
2. Confirm the app pool account has DB rights (Step 1).
3. Look for a stack trace in ULS:
   `Get-SPLogEvent -StartTime (Get-Date).AddMinutes(-10) | Where-Object { $_.Area -eq "PNU Custom Search" }`

### "Failed to load receiver assembly"

Strong-name the assembly and deploy to GAC. Verify with:

```powershell
[System.Reflection.Assembly]::LoadFrom(
    "C:\Windows\Assembly\GAC_MSIL\PNU.Internet.Search\1.0.0.0__<token>\PNU.Internet.Search.dll"
).GetName().FullName
```

The `Feature.xml` `ReceiverAssembly` must match this string exactly.

### "language-neutral solution package not found" when retracting

Build → Clean, close VS, then in SP Management Shell:

```powershell
Get-SPSolution | Where-Object { $_.Name -like "PNU*" } | ForEach-Object {
    Uninstall-SPSolution $_ -WebApplication https://your.pnu.web.app -Confirm:$false
    Remove-SPSolution    $_ -Confirm:$false
}
```

### "ContainsFieldWithInternalName not found"

That API doesn't exist on SP 2019 server-side. The package uses
`SPFieldCollection.ContainsField` and a custom `HasField` helper —
no further action needed.

### Full-Text Search not installed

`PNU_SearchIndex.sql` does **not** use full-text. LIKE-based search
is fine for ~50k items; PNU's expected volume is well below that.

---

## Where to find logs

```powershell
# Last 30 minutes of search-related ULS entries
Get-SPLogEvent -StartTime (Get-Date).AddMinutes(-30) `
  | Where-Object { $_.Area -eq "PNU Custom Search" } `
  | Select-Object Timestamp, Category, Message `
  | Format-Table -AutoSize
```

The DB also keeps a per-crawl summary in `dbo.SearchCrawlLog`:

```sql
SELECT TOP 20 * FROM dbo.SearchCrawlLog ORDER BY StartedAt DESC;
```
