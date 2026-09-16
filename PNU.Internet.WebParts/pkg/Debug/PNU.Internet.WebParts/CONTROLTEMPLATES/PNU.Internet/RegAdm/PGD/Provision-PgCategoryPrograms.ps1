<#
.SYNOPSIS
    Provisions and seeds the Postgraduate Category Programs lists in SharePoint.
.DESCRIPTION
    Creates 'PgCategoryOverview' and 'PgCategoryPrograms' on the target web (/ar/RegAdm/PGD or custom web),
    adds all necessary fields, and seeds the 45+ programs and overview headers across the 5 PNU categories:
    1. NouraStaff (كوادر نورة)
    2. Humanitarian (البرامج الإنسانية)
    3. Interdisciplinary (البرامج البينية)
    4. Scientific (البرامج العلمية)
    5. Health (البرامج الصحية)
.EXAMPLE
    .\Provision-PgCategoryPrograms.ps1 -SiteUrl "https://pnu.edu.sa" -WebRelativeUrl "/ar/RegAdm/PGD"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]$SiteUrl = "https://pnu.edu.sa",

    [Parameter(Mandatory=$false)]
    [string]$WebRelativeUrl = "/ar/RegAdm/PGD",

    [Parameter(Mandatory=$false)]
    [switch]$ForceReseed = $false
)

# Load SharePoint Snapin if available
if ((Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue) -eq $null) {
    Add-PSSnapin Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue
}

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "  PNU Postgraduate Category Programs - Provisioning & Seed Script" -ForegroundColor Cyan
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "Site URL: $SiteUrl" -ForegroundColor Gray
Write-Host "Web URL : $WebRelativeUrl" -ForegroundColor Gray

try {
    $site = Get-SPSite -Identity $SiteUrl -ErrorAction Stop
    $web = $site.OpenWeb($WebRelativeUrl)

    if ($web -eq $null -or !$web.Exists) {
        Write-Warning "Could not open web at $WebRelativeUrl, falling back to root web..."
        $web = $site.RootWeb
    }

    Write-Host "Connected to Web: $($web.Title) ($($web.Url))" -ForegroundColor Green

    # Execute C# Assembly method if deployed, or create via CSOM/SSOM
    Write-Host "Invoking PgCategoryProvisioner..." -ForegroundColor Yellow
    [PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgCategoryProvisioner]::EnsureLists($web)

    if ($ForceReseed) {
        Write-Host "ForceReseed specified. Reseeding all categories..." -ForegroundColor Yellow
        [PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgCategoryProvisioner]::ReseedCategory($web, [PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure.PgProgramCategory]::All)
    }

    Write-Host "Successfully provisioned and seeded lists!" -ForegroundColor Green
}
catch {
    Write-Error "Error during provisioning: $_"
}
finally {
    if ($web -ne $null) { $web.Dispose() }
    if ($site -ne $null) { $site.Dispose() }
}
