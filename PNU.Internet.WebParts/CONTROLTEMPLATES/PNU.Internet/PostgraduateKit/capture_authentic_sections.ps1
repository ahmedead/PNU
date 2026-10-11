$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"

function Capture-Page-Sections($htmlFile, $prefix, $sections) {
    $filePath = Join-Path $baseDir $htmlFile
    $content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

    foreach ($sec in $sections) {
        $secId = $sec.Id

        $js = @"
<script>
window.addEventListener('load', function() {
    var el = document.getElementById('$secId');
    if (el) {
        el.scrollIntoView({ block: 'start', inline: 'nearest' });
    }
});
</script>
<style>
html, body { scroll-behavior: auto !important; }
[data-aos] { opacity: 1 !important; transform: none !important; transition: none !important; }
</style>
"@
        $modifiedHtml = $content.Replace("</head>", "$js</head>")
        $tempFile = Join-Path $baseDir "temp_$prefix`_$secId.html"
        [System.IO.File]::WriteAllText($tempFile, $modifiedHtml, [System.Text.Encoding]::UTF8)

        $outFile = Join-Path $outDir "$prefix-$secId.png"
        $windowHeight = if ($sec.Height) { $sec.Height } else { 750 }
        
        Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1400,$windowHeight --hide-scrollbars --screenshot=`"$outFile`" `"file:///$($tempFile.Replace('\', '/'))`"" -Wait
        
        Remove-Item -Force $tempFile
        Write-Host "Captured $prefix-$secId -> $outFile"
    }
}

# 1. Applicants (academic-kit-applicants.html)
$applicantSections = @(
    @{ Id = "applicant-intro"; Height = 650 },
    @{ Id = "applicant-section-1"; Height = 550 },
    @{ Id = "applicant-section-2"; Height = 550 },
    @{ Id = "applicant-section-3"; Height = 550 },
    @{ Id = "applicant-section-4"; Height = 750 },
    @{ Id = "applicant-section-5"; Height = 550 },
    @{ Id = "applicant-section-6"; Height = 800 },
    @{ Id = "applicant-section-7"; Height = 600 },
    @{ Id = "applicant-section-8"; Height = 450 },
    @{ Id = "applicant-section-9"; Height = 450 },
    @{ Id = "applicant-reference"; Height = 400 }
)
Capture-Page-Sections "academic-kit-applicants.html" "applicants" $applicantSections

# 2. Enrolled Students (academic-kit-enrolled-students.html)
$enrolledSections = @(
    @{ Id = "enrolled-intro"; Height = 650 },
    @{ Id = "enrolled-section-1"; Height = 550 },
    @{ Id = "enrolled-section-2"; Height = 650 },
    @{ Id = "enrolled-section-3"; Height = 800 },
    @{ Id = "enrolled-section-4"; Height = 750 },
    @{ Id = "enrolled-section-5"; Height = 650 },
    @{ Id = "enrolled-section-6"; Height = 800 },
    @{ Id = "enrolled-section-7"; Height = 550 },
    @{ Id = "enrolled-section-8"; Height = 600 },
    @{ Id = "enrolled-section-9"; Height = 450 },
    @{ Id = "enrolled-section-10"; Height = 450 },
    @{ Id = "enrolled-reference"; Height = 400 }
)
Capture-Page-Sections "academic-kit-enrolled-students.html" "enrolled" $enrolledSections

# 3. Graduates (academic-kit-graduates.html)
$graduateSections = @(
    @{ Id = "graduate-intro"; Height = 650 },
    @{ Id = "graduate-section-1"; Height = 550 },
    @{ Id = "graduate-section-2"; Height = 550 },
    @{ Id = "graduate-section-3"; Height = 550 },
    @{ Id = "graduate-section-4"; Height = 750 },
    @{ Id = "graduate-reference"; Height = 400 }
)
Capture-Page-Sections "academic-kit-graduates.html" "graduates" $graduateSections

Write-Host "All authentic page screenshots captured!"
