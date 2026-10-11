$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$tempDir = Join-Path $baseDir "temp_screenshots"

if (-not (Test-Path $tempDir)) {
    New-Item -ItemType Directory -Force -Path $tempDir | Out-Null
}
if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Force -Path $outDir | Out-Null
}

$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"

function Capture-Sections($htmlFile, $prefix, $sectionIds) {
    $filePath = Join-Path $baseDir $htmlFile
    $content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)
    
    # Extract head content and fix paths to be absolute
    $headMatch = [regex]::Match($content, '(?s)<head>(.*?)</head>')
    $headContent = $headMatch.Groups[1].Value
    $baseUri = "file:///D:/MyWorkSpaces/AI/Antigravity/PNU/PNU.Internet.SP/DGA_NewDesign/v4.0/"
    $headContent = "<base href='$baseUri' />`n" + $headContent

    foreach ($secId in $sectionIds) {
        $pattern = "(?s)(<section\s+id=""$secId"".*?</section>)"
        if ($secId -eq 'kit-hero' -or $secId -eq 'kit-home-about' -or $secId -eq 'kit-categories') {
            $pattern = "(?s)(<section\s+class=""$secId"".*?</section>)"
        }
        $match = [regex]::Match($content, $pattern)
        if ($match.Success) {
            $secHtml = $match.Groups[1].Value
            $pageHtml = @"
<!DOCTYPE html>
<html lang="ar-SA" dir="rtl">
<head>
$headContent
<style>
body { background: #fff !important; margin: 0; padding: 25px; font-family: 'IBM Plex Sans Arabic', 'Cairo', sans-serif !important; }
.container { max-width: 1200px !important; margin: 0 auto !important; }
section { margin-bottom: 0 !important; }
</style>
</head>
<body class="dga-main-body">
  <div class="container">
    $secHtml
  </div>
</body>
</html>
"@
            $tempFile = Join-Path $tempDir "$prefix-$secId.html"
            [System.IO.File]::WriteAllText($tempFile, $pageHtml, [System.Text.Encoding]::UTF8)
            $outFile = Join-Path $outDir "$prefix-$secId.png"
            $p = Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1280,750 --hide-scrollbars --screenshot=`"$outFile`" `"$tempFile`"" -Wait -PassThru
            Write-Host "Captured $prefix-$secId -> $outFile (ExitCode: $($p.ExitCode))"
        } else {
            Write-Warning "Could not find section $secId in $htmlFile"
        }
    }
}

# Hub
Capture-Sections "academic-kit.html" "hub" @("kit-hero", "kit-home-about", "kit-categories")

# Applicants
Capture-Sections "academic-kit-applicants.html" "applicants" @("applicant-intro", "applicant-section-1", "applicant-section-2", "applicant-section-3", "applicant-section-4", "applicant-section-5", "applicant-section-6", "applicant-section-7", "applicant-section-8", "applicant-section-9", "applicant-reference")

# Enrolled
Capture-Sections "academic-kit-enrolled-students.html" "enrolled" @("enrolled-intro", "enrolled-section-1", "enrolled-section-2", "enrolled-section-3", "enrolled-section-4", "enrolled-section-5", "enrolled-section-6", "enrolled-section-7", "enrolled-section-8", "enrolled-section-9", "enrolled-section-10", "enrolled-reference")

# Graduates
Capture-Sections "academic-kit-graduates.html" "graduates" @("graduate-intro", "graduate-section-1", "graduate-section-2", "graduate-section-3", "graduate-section-4", "graduate-reference")

Remove-Item -Recurse -Force $tempDir
Write-Host "Done!"
