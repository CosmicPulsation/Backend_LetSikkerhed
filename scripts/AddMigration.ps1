# CreateMigration.ps1

param (
    [Parameter(Mandatory = $true)]
    $MigrationName
)

$scriptFullPath = $MyInvocation.MyCommand.Path
$scriptDirectory = Split-Path -Parent $scriptFullPath

$startupProjectPath = Resolve-Path "$scriptDirectory\..\src\Backend\Backend.csproj"
$projectPath = Resolve-Path "$scriptDirectory\..\src\Backend.Database\Backend.Database.csproj"

Write-Host "Project path $ProjectPath"
Write-Host "Startup project path $startupProjectPath"

dotnet ef migrations add "$MigrationName" `
    --startup-project $startupProjectPath `
    --project $projectPath

Write-Host "Created migration '$MigrationName' using Backend.Database project."