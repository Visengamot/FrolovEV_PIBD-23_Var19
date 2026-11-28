[CmdletBinding()]
param(
    [ValidateSet('Initialize','Start','Stop')][string]$Mode = 'Initialize',
    [string]$PostgresBin = 'D:\PostgreSQL\bin'
)
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:LOCALAPPDATA 'ShoeStoreLab2'
$data = Join-Path $root 'data'
$credentialsFile = Join-Path $root 'credentials.clixml'
$marker = Join-Path $root 'shoestore-lab2.instance'
$port = 55432
foreach ($tool in @('initdb','pg_ctl','psql')) {
    if (-not (Test-Path (Join-Path $PostgresBin "$tool.exe"))) { throw "PostgreSQL tool missing: $tool.exe; specify -PostgresBin." }
}
function Invoke-Pg([string]$Tool, [string[]]$Arguments) {
    & (Join-Path $PostgresBin "$Tool.exe") @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Tool failed (exit $LASTEXITCODE)." }
}
function Invoke-Sql([string]$Sql, [string]$User, [string]$Password, [string]$Database) {
    $previous = $env:PGPASSWORD
    try {
        $env:PGPASSWORD = $Password
        $output = $Sql | & (Join-Path $PostgresBin 'psql.exe') -X -w -h 127.0.0.1 -p $port -U $User -d $Database -v ON_ERROR_STOP=1 -f - 2>&1
        if ($LASTEXITCODE -ne 0) { throw 'SQL operation failed; details suppressed to protect credentials. Inspect the private local PostgreSQL log.' }
        $output
    } finally { $env:PGPASSWORD = $previous }
}
function New-RandomPassword {
    $bytes = New-Object byte[] 32
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    try { $rng.GetBytes($bytes) } finally { $rng.Dispose() }
    return ([BitConverter]::ToString($bytes)).Replace('-', '')
}
function Get-PlainPassword($Value) {
    return [System.Net.NetworkCredential]::new('', $Value).Password
}
function Confirm-ManagedCluster {
    if (-not (Test-Path $marker) -or (Get-Content $marker -Raw).Trim() -ne 'ShoeStoreLab2:55432') { throw 'Missing instance marker; refusing to manage an unknown cluster.' }
    if (-not (Test-Path (Join-Path $data 'PG_VERSION'))) { throw 'Cluster is incomplete; inspect local data manually. Nothing is deleted automatically.' }
    $conf = Get-Content (Join-Path $data 'postgresql.conf') -Raw
    if ($conf -notmatch "(?m)^port = 55432\s*$" -or $conf -notmatch "(?m)^listen_addresses = '127\.0\.0\.1'\s*$") { throw 'Unexpected cluster endpoint configuration; refusing to start.' }
}
function Start-ManagedCluster {
    Confirm-ManagedCluster
    & (Join-Path $PostgresBin 'pg_ctl.exe') status -D $data *> $null
    if ($LASTEXITCODE -eq 0) { Write-Host 'Lab cluster already running.'; return }
    if (Get-NetTCPConnection -State Listen -LocalPort $port -ErrorAction SilentlyContinue) { throw 'Port 55432 is occupied; refusing to connect to or stop another server.' }
    Invoke-Pg 'pg_ctl' @('start','-D',$data,'-l',(Join-Path $root 'postgresql.log'),'-w','-t','30')
}
if ($Mode -eq 'Stop') {
    Confirm-ManagedCluster
    & (Join-Path $PostgresBin 'pg_ctl.exe') status -D $data *> $null
    if ($LASTEXITCODE -ne 0) { Write-Host 'Lab cluster is already stopped.'; return }
    Invoke-Pg 'pg_ctl' @('stop','-D',$data,'-m','fast','-w','-t','30')
    return
}
if ($Mode -eq 'Start') { Start-ManagedCluster; return }
if (-not (Test-Path $root)) { New-Item -ItemType Directory -Path $root | Out-Null }
# Restrict local cluster, log and DPAPI credential access to this Windows user.
$sid = [System.Security.Principal.WindowsIdentity]::GetCurrent().User.Value
& icacls.exe $root '/inheritance:r' '/grant:r' "*${sid}:(OI)(CI)F" | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'Could not restrict the local data directory ACL.' }
if (-not (Test-Path $marker)) {
    if (Test-Path $data) { throw 'Unmarked data directory exists; refusing to overwrite it.' }
    Set-Content $marker 'ShoeStoreLab2:55432' -Encoding ASCII
}
if (-not (Test-Path $credentialsFile)) {
    if (Test-Path $data) { throw 'Existing cluster has no credential file; refusing to reset credentials.' }
    $adminPassword = New-RandomPassword
    $appPassword = New-RandomPassword
    [pscustomobject]@{
        Admin = (ConvertTo-SecureString $adminPassword -AsPlainText -Force)
        Application = (ConvertTo-SecureString $appPassword -AsPlainText -Force)
    } | Export-Clixml $credentialsFile
}
$credentials = Import-Clixml $credentialsFile
$adminPassword = Get-PlainPassword $credentials.Admin
$appPassword = Get-PlainPassword $credentials.Application
if (-not (Test-Path (Join-Path $data 'PG_VERSION'))) {
    if (Test-Path $data) { throw 'Incomplete cluster exists; inspect it manually rather than overwriting it.' }
    $passwordFile = Join-Path $root 'initdb-password.tmp'
    try {
        Set-Content $passwordFile $adminPassword -Encoding ASCII
        Invoke-Pg 'initdb' @('-D',$data,'-U','shoestore_admin','--encoding=UTF8','--locale=C','--auth-host=scram-sha-256','--auth-local=scram-sha-256',"--pwfile=$passwordFile")
    } finally { if (Test-Path $passwordFile) { Remove-Item -LiteralPath $passwordFile } }
    Add-Content (Join-Path $data 'postgresql.conf') "`n# ShoeStore Lab 2: separate loopback-only development instance`nlisten_addresses = '127.0.0.1'`nport = 55432`npassword_encryption = 'scram-sha-256'`n" -Encoding ASCII
}
Start-ManagedCluster
# Passwords are random hex strings: SQL literals cannot contain quotes or metacharacters.
$sql = @'
DO $block$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'shoestore_app') THEN
        CREATE ROLE shoestore_app LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE PASSWORD '__APP_PASSWORD__';
    END IF;
END
$block$;
SELECT 'CREATE DATABASE shoestore_lab2 OWNER shoestore_app'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'shoestore_lab2')
\gexec
DO $block$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_database d JOIN pg_roles r ON r.oid = d.datdba WHERE d.datname = 'shoestore_lab2' AND r.rolname = 'shoestore_app') THEN
        RAISE EXCEPTION 'Unexpected database owner; refusing to modify it';
    END IF;
END
$block$;
'@
Invoke-Sql ($sql.Replace('__APP_PASSWORD__', $appPassword)) 'shoestore_admin' $adminPassword 'postgres'
Invoke-Sql 'SELECT current_database(), current_user, inet_server_addr(), inet_server_port();' 'shoestore_app' $appPassword 'shoestore_lab2'
$project = Split-Path $PSScriptRoot -Parent
$settingsDirectory = Join-Path $project 'src\ShoeStore.WinForms'
if (-not (Test-Path $settingsDirectory)) { throw 'WinForms source directory does not exist; database is ready, but appsettings was not written.' }
$settings = Join-Path $settingsDirectory 'appsettings.Local.json'
$builder = [System.Data.Common.DbConnectionStringBuilder]::new()
$builder['Host'] = '127.0.0.1'
$builder['Port'] = $port
$builder['Database'] = 'shoestore_lab2'
$builder['Username'] = 'shoestore_app'
$builder['Password'] = $appPassword
[pscustomobject]@{ConnectionStrings = [pscustomobject]@{ShoeStore = $builder.ConnectionString}} | ConvertTo-Json -Depth 3 | Set-Content $settings -Encoding UTF8
& icacls.exe $settings '/inheritance:r' '/grant:r' "*${sid}:F" | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'Could not restrict the application settings ACL.' }
Write-Host 'Lab database ready on 127.0.0.1:55432. Local settings saved; passwords are never printed.'
