[CmdletBinding()]
param([string]$PostgresBin = 'D:\PostgreSQL\bin')
$ErrorActionPreference = 'Stop'
$project = Split-Path $PSScriptRoot -Parent
$settingsPath = Join-Path $project 'src\ShoeStore.WinForms\appsettings.Local.json'
if (-not (Test-Path $settingsPath)) {
    throw 'Local application connection settings are missing.'
}
$settings = Get-Content -LiteralPath $settingsPath -Raw -Encoding UTF8 | ConvertFrom-Json
$connection = [System.Data.Common.DbConnectionStringBuilder]::new()
$connection.set_ConnectionString([string]$settings.ConnectionStrings.ShoeStore)
$previous = $env:PGPASSWORD
try {
    $env:PGPASSWORD = [string]$connection['Password']
    $server = [string]$connection['Host']
    $port = [string]$connection['Port']
    $user = [string]$connection['Username']
    $database = [string]$connection['Database']
    & (Join-Path $PostgresBin 'psql.exe') -X -w -h $server -p $port -U $user -d $database `
        -v ON_ERROR_STOP=1 -f (Join-Path $PSScriptRoot 'verify.sql')
    if ($LASTEXITCODE -ne 0) { throw 'Database verification failed.' }
} finally {
    $env:PGPASSWORD = $previous
}
