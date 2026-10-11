<#
    Diagnose-PageContent.ps1

    Reports exactly where a publishing page keeps its content, so the content editor
    can be pointed at the right place.

    Run on the SharePoint server in the SharePoint Management Shell:

        .\Diagnose-PageContent.ps1 -SiteUrl "https://<portal>" -PageUrl "/ar/RegAdm/PGD/Pages/HumanitarianPro1.aspx"

    It is read-only: it opens nothing for editing and changes nothing.
#>

param(
    [Parameter(Mandatory = $true)] [string] $SiteUrl,
    [Parameter(Mandatory = $true)] [string] $PageUrl,
    [int] $PreviewChars = 200
)

if (-not (Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue)) {
    Add-PSSnapin Microsoft.SharePoint.PowerShell -ErrorAction Stop
}

function Write-Section([string] $title) {
    Write-Host ""
    Write-Host ("=" * 78) -ForegroundColor Cyan
    Write-Host $title -ForegroundColor Cyan
    Write-Host ("=" * 78) -ForegroundColor Cyan
}

function Get-Preview([string] $text, [int] $max) {
    if ([string]::IsNullOrEmpty($text)) { return "" }
    $flat = ($text -replace '\s+', ' ').Trim()
    if ($flat.Length -le $max) { return $flat }
    return $flat.Substring(0, $max) + " ..."
}

$site = $null
$web = $null

try {
    $site = New-Object Microsoft.SharePoint.SPSite($SiteUrl)
    $web = $site.OpenWeb()

    $file = $web.GetFile($PageUrl)
    if (-not $file.Exists) {
        # The page may live in a subweb; walk the URL from longest prefix down.
        $segments = $PageUrl.Trim('/').Split('/')
        for ($i = $segments.Length - 1; $i -ge 1; $i--) {
            $candidate = "/" + ($segments[0..($i - 1)] -join "/")
            try {
                $w = $site.OpenWeb($candidate)
                if ($w -and $w.Exists) {
                    $f = $w.GetFile($PageUrl)
                    if ($f.Exists) { $web = $w; $file = $f; break }
                    $w.Dispose()
                }
            } catch { }
        }
    }

    if (-not $file.Exists) {
        Write-Host "Page not found: $PageUrl" -ForegroundColor Red
        return
    }

    Write-Section "PAGE"
    Write-Host ("Web           : {0}" -f $web.Url)
    Write-Host ("File          : {0}" -f $file.ServerRelativeUrl)
    Write-Host ("UIVersionLabel: {0}" -f $file.UIVersionLabel)
    Write-Host ("CheckOutType  : {0}" -f $file.CheckOutType)
    Write-Host ("CheckedOutBy  : {0}" -f $(if ($file.CheckedOutByUser) { $file.CheckedOutByUser.Name } else { "-" }))
    Write-Host ("Level         : {0}" -f $file.Level)

    $item = $file.Item

    # ---- 1. Every field that holds HTML-ish content, with its size -------------
    Write-Section "FIELDS WITH CONTENT (current version)"
    $candidates = @(
        "PublishingPageContent", "PublishingPageContent_EN", "PublishingPageContentHtml",
        "PageContent", "Content", "Content_EN", "Body", "ArticleBody",
        "Description", "Description_EN", "Comments"
    )

    $anyField = $false
    foreach ($fieldName in $candidates) {
        if (-not $item.Fields.ContainsField($fieldName)) { continue }
        $val = [string] $item[$fieldName]
        if ([string]::IsNullOrWhiteSpace($val)) {
            Write-Host ("  {0,-30} (empty)" -f $fieldName) -ForegroundColor DarkGray
        }
        else {
            $anyField = $true
            Write-Host ("  {0,-30} {1,8} chars" -f $fieldName, $val.Length) -ForegroundColor Green
            Write-Host ("      {0}" -f (Get-Preview $val $PreviewChars)) -ForegroundColor Gray
        }
    }
    if (-not $anyField) { Write-Host "  None of the known content fields hold anything." -ForegroundColor Yellow }

    # ---- 2. Any OTHER note field that has content (catches custom fields) ------
    Write-Section "OTHER NON-EMPTY NOTE/HTML FIELDS (not in the known list)"
    $found = $false
    foreach ($f in $item.Fields) {
        if ($candidates -contains $f.InternalName) { continue }
        if ($f.Type -ne "Note" -and $f.TypeAsString -ne "HTML" -and $f.TypeAsString -ne "FullHtmlField") { continue }
        try {
            $val = [string] $item[$f.InternalName]
            if (-not [string]::IsNullOrWhiteSpace($val) -and $val.Length -gt 40) {
                $found = $true
                Write-Host ("  {0,-30} {1,8} chars  [{2}]" -f $f.InternalName, $val.Length, $f.TypeAsString) -ForegroundColor Yellow
                Write-Host ("      {0}" -f (Get-Preview $val $PreviewChars)) -ForegroundColor Gray
            }
        } catch { }
    }
    if (-not $found) { Write-Host "  (none)" -ForegroundColor DarkGray }

    # ---- 3. Web parts on the page ---------------------------------------------
    Write-Section "WEB PARTS ON THE PAGE"
    $mgr = $file.GetLimitedWebPartManager([System.Web.UI.WebControls.WebParts.PersonalizationScope]::Shared)
    try {
        if ($mgr.WebParts.Count -eq 0) { Write-Host "  (no web parts)" -ForegroundColor DarkGray }
        foreach ($wp in $mgr.WebParts) {
            Write-Host ("  {0}" -f $wp.GetType().FullName) -ForegroundColor Green
            Write-Host ("      Title : {0}" -f $wp.Title)
            Write-Host ("      ID    : {0}" -f $wp.ID)
            Write-Host ("      Zone  : {0}" -f $wp.ZoneID)

            # ContentEditorWebPart: show the HTML it carries
            $contentProp = $wp.GetType().GetProperty("Content")
            if ($contentProp) {
                $xml = $contentProp.GetValue($wp, $null)
                if ($xml) {
                    $html = $xml.InnerXml
                    if ([string]::IsNullOrWhiteSpace($html)) { $html = $xml.InnerText }
                    Write-Host ("      CONTENT: {0} chars" -f $html.Length) -ForegroundColor Magenta
                    Write-Host ("      {0}" -f (Get-Preview $html $PreviewChars)) -ForegroundColor Gray
                }
            }
            $linkProp = $wp.GetType().GetProperty("ContentLink")
            if ($linkProp) {
                $link = [string] $linkProp.GetValue($wp, $null)
                if ($link) { Write-Host ("      ContentLink: {0}" -f $link) -ForegroundColor Magenta }
            }
            # ControlLoaderWebPart: show which user control it loads
            $ucProp = $wp.GetType().GetProperty("UserControlPath")
            if ($ucProp) {
                Write-Host ("      UserControlPath: {0}" -f [string] $ucProp.GetValue($wp, $null)) -ForegroundColor Magenta
            }
        }
    }
    finally { $mgr.Dispose() }

    # ---- 4. Page layout --------------------------------------------------------
    Write-Section "PAGE LAYOUT"
    if ($item.Fields.ContainsField("PublishingPageLayout")) {
        Write-Host ("  {0}" -f [string] $item["PublishingPageLayout"])
        Write-Host "  (If the visible content is not in any field or web part above, it is"
        Write-Host "   rendered by this layout - edit the layout, not the page.)"
    }

    # ---- 5. Version history, with the size of the content field in each --------
    Write-Section "VERSION HISTORY (PublishingPageContent size per version)"
    foreach ($v in $item.Versions) {
        $len = 0
        try {
            $vv = [string] $v["PublishingPageContent"]
            if ($vv) { $len = $vv.Length }
        } catch { }
        Write-Host ("  {0,-8} {1,-20} {2,8} chars  {3}" -f `
            $v.VersionLabel, `
            $(if ($v.CreatedBy -and $v.CreatedBy.User) { $v.CreatedBy.User.Name } else { "-" }), `
            $len, `
            $v.Created)
    }

    Write-Host ""
    Write-Host "Done. The source with the largest, most recent content is what the page shows." -ForegroundColor Cyan
}
finally {
    if ($web) { $web.Dispose() }
    if ($site) { $site.Dispose() }
}
