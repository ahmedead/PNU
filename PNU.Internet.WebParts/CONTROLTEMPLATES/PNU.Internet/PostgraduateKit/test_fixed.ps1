Add-Type -AssemblyName System.Drawing

$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"

$htmlFile = "academic-kit-applicants.html"
$filePath = Join-Path $baseDir $htmlFile
$content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

# Convert all preloaded styles to direct stylesheets with absolute paths
$baseUri = "file:///D:/MyWorkSpaces/AI/Antigravity/PNU/PNU.Internet.SP/DGA_NewDesign/v4.0/"

# Replace preload with stylesheet
$fixedContent = $content -replace 'rel="preload"\s+href="([^"]+)"\s+as="style"[^>]*>', 'rel="stylesheet" href="$1">'
$fixedContent = $fixedContent -replace '<noscript>\s*<link rel="stylesheet"\s+href="([^"]+)"\s*/>\s*</noscript>', '<link rel="stylesheet" href="$1" />'

# Add base tag and fix AOS animation
$injectHead = @"
<base href="$baseUri" />
<link rel="stylesheet" href="public/css/style.min.css" />
<link rel="stylesheet" href="public/vendor/hugeicons/hgi-stroke-rounded.css" />
<link rel="stylesheet" href="public/css/academic-kit-pages.css" />
<style>
  * { animation: none !important; transition: none !important; }
  [data-aos] { opacity: 1 !important; transform: none !important; }
</style>
"@
$fixedContent = $fixedContent.Replace("<head>", "<head>`n$injectHead")

# Let's test isolating applicant-intro and applicant-section-1 in a clean container
function Capture-Direct-Section($secId, $outName, $height) {
    # Isolate the section inside the article container
    $isolateStyle = @"
<style>
  dga-header, aside, dga-footer, .bg-primary-25 { display: none !important; }
  article section { display: none !important; }
  article section#$secId { display: block !important; }
  body { background: #fff !important; padding: 20px !important; margin: 0 !important; }
  .container { max-width: 950px !important; width: 100% !important; margin: 0 auto !important; }
  .col-lg-8, .pnu-detail-article { width: 100% !important; max-width: 100% !important; flex: 0 0 100% !important; }
</style>
"@
    $pageHtml = $fixedContent.Replace("</head>", "$isolateStyle</head>")
    $tempFile = Join-Path $baseDir "temp_$outName.html"
    [System.IO.File]::WriteAllText($tempFile, $pageHtml, [System.Text.Encoding]::UTF8)

    $outFile = Join-Path $outDir "$outName.png"
    Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1000,$height --hide-scrollbars --screenshot=`"$outFile`" `"file:///$($tempFile.Replace('\', '/'))`"" -Wait
    Remove-Item -Force $tempFile
    Write-Host "Generated $outName -> Size: $((Get-Item $outFile).Length) bytes"
}

Capture-Direct-Section "applicant-intro" "test_fixed_intro" 450
Capture-Direct-Section "applicant-section-1" "test_fixed_section1" 400
Capture-Direct-Section "applicant-section-2" "test_fixed_section2" 350
