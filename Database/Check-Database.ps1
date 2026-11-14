[CmdletBinding()]
param([string]$PostgresBin = 'D:\PostgreSQL\bin')
$ErrorActionPreference = 'Stop'
$credentialFile = Join-Path $env:LOCALAPPDATA 'ShoeStoreLab2\credentials.clixml'
if (-not (Test-Path $credentialFile)) { throw 'Initialize the local lab database first.' }
$credentials = Import-Clixml $credentialFile
$previous = $env:PGPASSWORD
try {
    $env:PGPASSWORD = [System.Net.NetworkCredential]::new('', $credentials.Application).Password
    & (Join-Path $PostgresBin 'psql.exe') -X -w -h 127.0.0.1 -p 55432 -U shoestore_app -d shoestore_lab2 -v ON_ERROR_STOP=1 -f (Join-Path $PSScriptRoot 'verify.sql')
    if ($LASTEXITCODE -ne 0) { throw 'Database verification failed.' }
} finally { $env:PGPASSWORD = $previous }
