[CmdletBinding()]
param([string]$PostgresBin = 'D:\PostgreSQL\bin')
& (Join-Path $PSScriptRoot 'Initialize-Database.ps1') -Mode Start -PostgresBin $PostgresBin
