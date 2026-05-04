# PachaFit BDD Testing & Coverage Script

# 0. Limpieza de resultados anteriores
if (Test-Path "./TestResults") {
    Write-Host "Cleaning old test results..." -ForegroundColor Yellow
    Remove-Item "./TestResults" -Recurse -Force
}

Write-Host "1. Building project..." -ForegroundColor Cyan
dotnet build PACHA_FIT.BddTests\PACHA_FIT.BddTests.csproj -c Debug

Write-Host "2. Running BDD Tests with Coverage (using coverlet.runsettings)..." -ForegroundColor Cyan
# Al usar --settings, evitamos errores de tokens inválidos en la terminal
dotnet test PACHA_FIT.BddTests\PACHA_FIT.BddTests.csproj -c Debug --no-build `
    --collect:"XPlat Code Coverage" `
    --settings coverlet.runsettings `
    --results-directory "./TestResults"

Write-Host "3. Generating Coverage Report..." -ForegroundColor Cyan
$CoverageFile = Get-ChildItem -Path "./TestResults" -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1

if ($CoverageFile) {
    dotnet tool run reportgenerator `
        -reports:$($CoverageFile.FullName) `
        -targetdir:"./TestResults/CoverageReport" `
        -reporttypes:Html `
        -filters:"-PACHA_FIT.Api*;-PACHA_FIT.Infrastructure*;-PACHA_FIT.Migrations*;-*Generated*;-PACHA_FIT.BddTests*"
    
    Write-Host "Done! Test execution complete." -ForegroundColor Green
    Write-Host "Report: ./TestResults/CoverageReport/index.html" -ForegroundColor White
} else {
    Write-Host "Error: Coverage file was not generated. Check if coverlet.runsettings is valid." -ForegroundColor Red
}
