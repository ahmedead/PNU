$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"

$htmlFile = "academic-kit-applicants.html"
$filePath = Join-Path $baseDir $htmlFile
$content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

# Inject custom CSS to hide other sections and isolate applicant-intro
$customStyle = @"
<style>
  dga-header, aside, footer, dga-footer, .bg-primary-25 { display: none !important; }
  article section:not(#applicant-intro) { display: none !important; }
  body { background: #fff !important; padding: 20px !important; }
  .container { max-width: 1000px !important; margin: 0 auto !important; }
  .col-lg-8 { width: 100% !important; max-width: 100% !important; flex: 0 0 100% !important; }
</style>
"@

$testHtml = $content.Replace("</head>", "$customStyle</head>")
$tempFile = Join-Path $baseDir "temp_test_intro.html"
[System.IO.File]::WriteAllText($tempFile, $testHtml, [System.Text.Encoding]::UTF8)

$outFile = Join-Path $outDir "test_real_applicant_intro.png"
Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1200,600 --hide-scrollbars --screenshot=`"$outFile`" `"$tempFile`"" -Wait

Remove-Item -Force $tempFile
Write-Host "Created $outFile, size: $((Get-Item $outFile).Length)"
