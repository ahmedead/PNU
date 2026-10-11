$chrome = "C:\Program Files\Google\Chrome\Application\chrome.exe"
$url = "file:///D:/MyWorkSpaces/AI/Antigravity/PNU/PNU.Internet.SP/DGA_NewDesign/v4.0/academic-kit-applicants.html"
$out = "D:\MyWorkSpaces\AI\Antigravity\PNU\PNU.Internet.SP\PNU\PNU.Internet.WebParts\CONTROLTEMPLATES\PNU.Internet\PostgraduateKit\screenshots\full_applicants.png"

Start-Process -FilePath $chrome -ArgumentList "--headless=new --disable-gpu --window-size=1440,3000 --hide-scrollbars --screenshot=`"$out`" `"$url`"" -Wait
Write-Host "Full screenshot generated. Size: $((Get-Item $out).Length) bytes"
