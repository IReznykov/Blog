Function Get-SqlDatabaseAccessToken {
    <#
    .SYNOPSIS
    Function Get-SqlDatabaseAccessToken returns access token to connect to Azure SQL Database service.
    .DESCRIPTION
    Function Get-SqlDatabaseAccessToken returns access token to connect to Azure SQL Database service.
    The script authenticate user via default browser. If any step fails, the function returns $null.
    .PARAMETER TenantID
    Tenant Id for Azure subscription
    .PARAMETER SubscriptionID
    Azure Subcription Id
    .INPUTS
    None. You cannot pipe objects to Get-SqlDatabaseAccessToken.
    .OUTPUTS
    Get-SqlDatabaseAccessToken returns $null or access token to Azure SQL Database service
    .EXAMPLE
    PS> Get-WAF2WebAclARN -TenantID ad488fc7-65a6-4a23-8ea1-08ae48e4e2f1 -SubscriptionId 998419f1-5d94-4627-814a-cb2bcd6eee42
    Returns acess token to connect to Azure SQL Database service in Azure subscription '998419f1-5d94-4627-814a-cb2bcd6eee42'
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param
    (
        # Azure TenantID
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[0-9A-F]{8}[-]{1}(?:[0-9A-F]{4}-){3}[0-9A-F]{12}$')]
        [string]$TenantID,

        # Azure SubscriptionID
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[0-9A-F]{8}[-]{1}(?:[0-9A-F]{4}-){3}[0-9A-F]{12}$')]
        [string]$SubscriptionID
    )

    #region Connect to Azure and get access token
    # NOTE: connection command will authenticate via default browser
    Connect-AzAccount -Tenant $TenantID | Out-Host;
    if (-not $?) {
        Write-Verbose "Can't connect to Azure Tenant";
        return $null;
    }
    Set-AzContext -Subscription $SubscriptionID | Out-Host;
    if (-not $?) {
        Write-Error "Can't connect to Azure Subscription";
        return $null;
    }
    $objectAccessToken = (Get-AzAccessToken -ResourceUrl "https://database.windows.net/");
    $accessToken = $objectAccessToken.Token;
    if ( -not $accessToken) {
        Write-Verbose "AccessToken is empty, can't connect to Azure SQL Database";
        return $null;
    }
    #endregion

    return $accessToken;
}

Function Get-SqlDatabaseAccessToken2 {
    <#
    .SYNOPSIS
    Function Get-SqlDatabaseAccessToken2 returns access token to Azure SQL database service.
    .DESCRIPTION
    Function Get-SqlDatabaseAccessToken2 returns access token to Azure SQL database service.
    At first step it requests the access token via IMDS;
    if exception is catch, request the access token by using tenant and subscription IDs.
    If any step fails, the function returns $null.
    .PARAMETER TenantID
    Tenant Id for Azure subscription
    .PARAMETER SubscriptionID
    Azure Subcription Id
    .INPUTS
    None. You cannot pipe objects to Get-SqlDatabaseAccessToken.
    .OUTPUTS
    Get-SqlDatabaseAccessToken returns $null or access token to Azure SQL Database service
    .EXAMPLE
    PS> Get-WAF2WebAclARN -TenantID ad488fc7-65a6-4a23-8ea1-08ae48e4e2f1 -SubscriptionId 998419f1-5d94-4627-814a-cb2bcd6eee42
    Returns acess token to connect to Azure SQL Database service in Azure subscription '998419f1-5d94-4627-814a-cb2bcd6eee42'
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param
    (
        # Azure TenantID
        [Parameter(Mandatory = $false)]
        [ValidatePattern('^|[0-9A-F]{8}[-]{1}(?:[0-9A-F]{4}-){3}[0-9A-F]{12}$')]
        [string]$TenantID,

        # Azure SubscriptionID
        [Parameter(Mandatory = $false)]
        [ValidatePattern('^|[0-9A-F]{8}[-]{1}(?:[0-9A-F]{4}-){3}[0-9A-F]{12}$')]
        [string]$SubscriptionID
    )

    #region Get an access token for managed identities for Azure resources
    $accessToken = $null;
    try {
        $response = Invoke-WebRequest `
            -Uri 'http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https%3A%2F%2Fdatabase.windows.net%2F' `
            -Headers @{Metadata = "true" };

        if (-not $? -or -not $response) {
            Write-Verbose "The command is run outside Azure VM";
            $accessToken = $null;
        }
        else {
            $content = $response.Content | ConvertFrom-Json;
            $accessToken = $content.access_token;
        }
    }
    catch {
        Write-Verbose $_.ErrorDetails;
        $accessToken = $null;
    }
    if ($accessToken) {
        return $accessToken;
    }
    #endregion

    #region Connect to Azure and get access token
    # NOTE: connection command will authenticate via default browser
    if (-not $TenantID -or -not $SubscriptionID) {
        return $null;
    }
    Connect-AzAccount -Tenant $TenantID | Out-Host;
    if (-not $?) {
        Write-Verbose "Can't connect to Azure Tenant";
        return $null;
    }
    Set-AzContext -Subscription $SubscriptionID | Out-Host;
    if (-not $?) {
        Write-Error "Can't connect to Azure Subscription";
        return $null;
    }
    $objectAccessToken = (Get-AzAccessToken `
            -ResourceUrl "https://database.windows.net/");
    $accessToken = $objectAccessToken.Token;
    if ( -not $accessToken) {
        Write-Verbose "AccessToken is empty, can't connect to Azure SQL Database";
        return $null;
    }
    #endregion

    return $accessToken;
}
