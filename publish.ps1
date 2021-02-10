sc.exe \\epweb1 stop azurelogsviewer3
Invoke-Command -ComputerName epweb1 -ScriptBlock { Stop-WebAppPool -Name "AzureLogsViewer2" }

Push-Location
cd src
dotnet publish  ./LogAnalyticsViewer.Web/LogAnalyticsViewer.Web.csproj -o \\epweb1\wwwroot\AzureLogsViewer2\
dotnet publish  ./LogAnalyticsViewer.Worker/LogAnalyticsViewer.Worker.csproj  -o \\epweb1\wwwroot\AzureLogsViewer2\Worker\

Pop-Location

Invoke-Command -ComputerName epweb1 -ScriptBlock { Start-WebAppPool -Name "AzureLogsViewer2" }
sc.exe \\epweb1 start azurelogsviewer3