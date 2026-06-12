# PachaFit BDD Testing & Coverage Script

# 0. Limpieza de resultados anteriores
if (Test-Path "./TestResults") {
    Write-Host "Cleaning old test results..." -ForegroundColor Yellow
    Remove-Item "./TestResults" -Recurse -Force
}

Write-Host "1. Building project..." -ForegroundColor Cyan
dotnet build PACHA_FIT.BddTests\PACHA_FIT.BddTests.csproj -c Debug

Write-Host "2. Running BDD Tests with Coverage..." -ForegroundColor Cyan
# Recolectamos todo el ensamblado PACHA_FIT para evitar errores de vinculación
dotnet test PACHA_FIT.BddTests\PACHA_FIT.BddTests.csproj -c Debug --no-build `
    --collect:"XPlat Code Coverage" `
    --settings coverlet.runsettings `
    --results-directory "./TestResults"

Write-Host "3. Generating Focused Coverage Report (Filtering by physical folder)..." -ForegroundColor Cyan
$CoverageFile = Get-ChildItem -Path "./TestResults" -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1

if ($CoverageFile) {
    # Filtro por carpeta física: solo permitimos archivos dentro de src/Core
    # Esto elimina Program.cs, Api, Infrastructure y código generado automáticamente
    dotnet tool run reportgenerator `
        -reports:$($CoverageFile.FullName) `
        -targetdir:"./TestResults/CoverageReport" `
        -reporttypes:Html `
        -filefilters:"+*src\Core\*"
    
    Write-Host "Done! Test execution complete." -ForegroundColor Green
    Write-Host "Report (Core folder only): ./TestResults/CoverageReport/index.html" -ForegroundColor White
} else {
    Write-Host "Error: Coverage file was not generated." -ForegroundColor Red
}
