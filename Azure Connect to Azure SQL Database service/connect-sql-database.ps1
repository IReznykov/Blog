@('Az', 'SqlServer' ) |
ForEach-Object {
    $moduleName = $_;
    Write-Host "Check $moduleName module";
    if ( $(Get-InstalledModule | Where-Object { $_.Name -eq $moduleName }).Count -eq 0) {
        Write-Verbose "Import $moduleName module";
        Find-Module -Name $moduleName -Verbose:$false | `
            Install-Module -Scope CurrentUser -Repository PSGallery -Force -Verbose:$false;
    }
    else {
        Write-Verbose "$moduleName module is installed";
    }
}

# Azure subscription
$TenantID = 'ad488fc7-65a6-4a23-8ea1-08ae48e4e2f1';
$SubscriptionID = '998419f1-5d94-4627-814a-cb2bcd6eee42';
# Azure SQL Database
$AzureSqlServer = "tcp:test-server.database.windows.net,1433";
$AzureDatabaseName = "test-database";
# others
$ConnectionTimeout = 600;
$Timeout = 600;

$fileName = $(Split-Path -Path $PSCommandPath -Leaf);
$startDateTime = $([DateTime]::Now);
Write-Host "Script '$fileName' start time = $([DateTime]::Now)" -ForegroundColor Blue;

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path;
. "$ScriptDir\functions.ps1";

#region Run trivial query to database
$result = $true;
try {
    $accessToken = Get-SqlDatabaseAccessToken2 `
        -TenantID $TenantID `
        -SubscriptionID $SubscriptionID;
    if (-not $? -or ($null -eq $accessToken)) {
        Write-Error "AccessToken is empty, can't connect to Azure SQL Database";
        return $false;
    }
    
    try {
        $dbquery = "SELECT DB_NAME()";
        Write-Host $dbquery;
        Invoke-Sqlcmd `
            -ServerInstance $AzureSqlServer `
            -Database $AzureDatabaseName `
            -AccessToken $accessToken `
            -Query $dbquery `
            -QueryTimeout $Timeout `
            -ConnectionTimeout $ConnectionTimeout `
            -OutputSqlErrors $true `
            -ErrorAction Stop `
            -ErrorVariable scriptError | Out-Host;
        $result = $?;
    }
    catch {
        Write-Host $_;
        $result = $false;
    }
    
    if ($scriptError) { $scriptError | Write-Host; }
    Write-Host $(if ($result) { "Query run successfully" } else { "Query failed" });
    #endregion
}
finally {
    $scriptDuration = [DateTime]::Now - $startDateTime;
    Write-Host "Script '$fileName' ends, total duration $($scriptDuration.Days) day(s), " `
        "$($scriptDuration.Hours):$($scriptDuration.Minutes):$($scriptDuration.Seconds).$($scriptDuration.Milliseconds)" `
        -ForegroundColor Blue;
}
