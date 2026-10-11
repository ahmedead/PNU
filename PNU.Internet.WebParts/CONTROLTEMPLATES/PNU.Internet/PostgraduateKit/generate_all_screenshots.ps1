Add-Type -AssemblyName System.Drawing

$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"
$baseUri = "file:///D:/MyWorkSpaces/AI/Antigravity/PNU/PNU.Internet.SP/DGA_NewDesign/v4.0/"

function Process-File-Sections($htmlFile, $prefix, $sections) {
    $filePath = Join-Path $baseDir $htmlFile
    $content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

    # Convert all preload styles to direct stylesheets
    $fixed = $content -replace 'rel="preload"\s+href="([^"]+)"\s+as="style"[^>]*>', 'rel="stylesheet" href="$1">'
    $fixed = $fixed -replace '<noscript>\s*<link rel="stylesheet"\s+href="([^"]+)"\s*/>\s*</noscript>', '<link rel="stylesheet" href="$1" />'

    $injectHead = @"
<base href="$baseUri" />
<link rel="stylesheet" href="public/css/style.min.css" />
<link rel="stylesheet" href="public/vendor/hugeicons/hgi-stroke-rounded.css" />
<link rel="stylesheet" href="public/vendor/css/swiper-bundle.min.css" />
<link rel="stylesheet" href="public/css/academic-kit-pages.css" />
<style>
  * { animation: none !important; transition: none !important; }
  [data-aos] { opacity: 1 !important; transform: none !important; }
  dga-header, aside, dga-footer, .bg-primary-25, #academic-kit-footer, .dga-footer-copyright { display: none !important; }
  body { background: #fff !important; padding: 15px !important; margin: 0 !important; }
  .container { max-width: 950px !important; width: 100% !important; margin: 0 auto !important; padding: 0 !important; }
  .col-lg-8, .pnu-detail-article { width: 100% !important; max-width: 100% !important; flex: 0 0 100% !important; }
  article section { display: none !important; margin: 0 !important; }
</style>
"@
    $fixed = $fixed.Replace("<head>", "<head>`n$injectHead")

    foreach ($sec in $sections) {
        $secId = $sec.Id
        $h = if ($sec.H) { $sec.H } else { 450 }

        $isolate = @"
<style>
  article section#$secId { display: block !important; }
</style>
"@
        $pageHtml = $fixed.Replace("</head>", "$isolate</head>")
        $tempFile = Join-Path $baseDir "temp_$prefix`_$secId.html"
        [System.IO.File]::WriteAllText($tempFile, $pageHtml, [System.Text.Encoding]::UTF8)

        $outFile = Join-Path $outDir "$prefix-$secId.png"
        Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=980,$h --hide-scrollbars --screenshot=`"$outFile`" `"file:///$($tempFile.Replace('\', '/'))`"" -Wait
        Remove-Item -Force $tempFile
        Write-Host "Generated $prefix-$secId -> $((Get-Item $outFile).Length) bytes"
    }
}

# 1. Applicants
$appSections = @(
    @{ Id = "applicant-intro"; H = 400 },
    @{ Id = "applicant-section-1"; H = 420 },
    @{ Id = "applicant-section-2"; H = 320 },
    @{ Id = "applicant-section-3"; H = 300 },
    @{ Id = "applicant-section-4"; H = 450 },
    @{ Id = "applicant-section-5"; H = 320 },
    @{ Id = "applicant-section-6"; H = 450 },
    @{ Id = "applicant-section-7"; H = 380 },
    @{ Id = "applicant-section-8"; H = 220 },
    @{ Id = "applicant-section-9"; H = 220 },
    @{ Id = "applicant-reference"; H = 180 }
)
Process-File-Sections "academic-kit-applicants.html" "applicants" $appSections

# 2. Enrolled Students
$enrolledSections = @(
    @{ Id = "enrolled-intro"; H = 400 },
    @{ Id = "enrolled-section-1"; H = 420 },
    @{ Id = "enrolled-section-2"; H = 480 },
    @{ Id = "enrolled-section-3"; H = 550 },
    @{ Id = "enrolled-section-4"; H = 450 },
    @{ Id = "enrolled-section-5"; H = 420 },
    @{ Id = "enrolled-section-6"; H = 450 },
    @{ Id = "enrolled-section-7"; H = 320 },
    @{ Id = "enrolled-section-8"; H = 380 },
    @{ Id = "enrolled-section-9"; H = 220 },
    @{ Id = "enrolled-section-10"; H = 220 },
    @{ Id = "enrolled-reference"; H = 180 }
)
Process-File-Sections "academic-kit-enrolled-students.html" "enrolled" $enrolledSections

# 3. Graduates
$gradSections = @(
    @{ Id = "graduate-intro"; H = 400 },
    @{ Id = "graduate-section-1"; H = 420 },
    @{ Id = "graduate-section-2"; H = 320 },
    @{ Id = "graduate-section-3"; H = 400 },
    @{ Id = "graduate-section-4"; H = 450 },
    @{ Id = "graduate-reference"; H = 180 }
)
Process-File-Sections "academic-kit-graduates.html" "graduates" $gradSections

Write-Host "All 29 section screenshots generated with 100% precision!"
