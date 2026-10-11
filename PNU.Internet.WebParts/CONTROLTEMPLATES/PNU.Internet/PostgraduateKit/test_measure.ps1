Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Web.Extensions

$baseDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\DGA_NewDesign\v4.0"
$outDir = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots"
$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"

function Capture-And-Crop-Page($htmlFile, $prefix, $sectionIds) {
    $filePath = Join-Path $baseDir $htmlFile
    $content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

    # Inject script that computes bounding boxes and stores them in a hidden element
    $idsJson = ($sectionIds | ConvertTo-Json -Compress)
    $inject = @"
<script>
window.addEventListener('load', function() {
    // Disable animations
    var style = document.createElement('style');
    style.innerHTML = '* { animation: none !important; transition: none !important; } [data-aos] { opacity: 1 !important; transform: none !important; }';
    document.head.appendChild(style);

    setTimeout(function() {
        var ids = $idsJson;
        var data = {};
        ids.forEach(function(id) {
            var el = document.getElementById(id);
            if (el) {
                var r = el.getBoundingClientRect();
                data[id] = {
                    x: Math.round(r.left + window.scrollX),
                    y: Math.round(r.top + window.scrollY),
                    w: Math.round(r.width),
                    h: Math.round(r.height)
                };
            }
        });
        var div = document.createElement('div');
        div.id = 'bounding-boxes-data';
        div.setAttribute('data-json', JSON.stringify(data));
        document.body.appendChild(div);
        document.title = 'READY_FOR_CAPTURE:::' + JSON.stringify(data);
    }, 500);
});
</script>
"@
    $modHtml = $content.Replace("</head>", "$inject</head>")
    $tempHtml = Join-Path $baseDir "temp_measure_$prefix.html"
    [System.IO.File]::WriteAllText($tempHtml, $modHtml, [System.Text.Encoding]::UTF8)

    # 1. Take full page screenshot
    $fullScreenshot = Join-Path $outDir "full_$prefix.png"
    # To get full page height, use 1400x12000
    Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1400,10000 --hide-scrollbars --screenshot=`"$fullScreenshot`" `"file:///$($tempHtml.Replace('\', '/'))`"" -Wait

    # 2. Get the bounding boxes from Chrome console or run Node/script
    # We can also compute positions directly in .NET or measure using Chrome
    Write-Host "Full screenshot captured: $fullScreenshot"
}

Capture-And-Crop-Page "academic-kit-applicants.html" "applicants" @("applicant-intro", "applicant-section-1", "applicant-section-2")
