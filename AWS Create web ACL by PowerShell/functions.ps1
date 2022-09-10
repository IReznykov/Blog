Function Get-WebAclARN {
    <#
    .SYNOPSIS
    Get-WebAclARN Function seek web ACL by its name and return ARN or $null if a web ACL is not found.
    .DESCRIPTION
    Get-WebAclARN Function seek web ACL by its name and return ARN or $null if a web ACL is not found.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # web ACL name
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$WebAclName,

        # region name
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$AwsProfile = "default"
    )

    #region Initialization
    $functionName = $($myInvocation.MyCommand.Name);
    Write-Host "$($functionName)(webACL=$WebAclName, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;
    #endregion

    #region List web ACLs with the provided name
    $queryRequest = "WebACLs[?Name==``$WebAclName``]";
    $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 list-web-acls `
        --scope REGIONAL `
        --query $queryRequest;
        
    if (-not $?) {
        Write-Host "Listing web ACLs failed" -ForegroundColor Red;
        return $null;
    }
    if ($jsonObjects) {
        $strJsonObjects = [string]$jsonObjects;
        $awsObjects = ConvertFrom-Json -InputObject $strJsonObjects;
        $existObject = ($awsObjects.Count -gt 0);
    }
    if ($existObject) {
        $webAclARN = $awsObjects.ARN;
        Write-Verbose "Web ACL '$WebAclName' is found, ARN=$webAclARN";
        return $webAclARN;
    }
    else {
        Write-Verbose "Web ACL '$WebAclName' doesn't exist";
        return $null;
    }
    #endregion
}

Function Get-WebAclForResource {
    <#
    .SYNOPSIS
    Get-WebAclForResource Function return web ACL ARN if it is accosiated with the resource.
    .DESCRIPTION
    Get-WebAclForResource Function return web ACL ARN if it is accosiated with the resource. If a resource ARN is wrong or a web ACL is not accosiated with the resource, $null is returned.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # resource ARN
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$ResourceARN,

        # region name
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$AwsProfile = "default"
    )
    #region Initialization
    $functionName = $($myInvocation.MyCommand.Name);
    Write-Host "$($functionName)(Resource=$ResourceARN, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;
    #endregion

    #region List accosiated Web ACL with resource
    $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 get-web-acl-for-resource `
        --resource-arn $ResourceARN;
    
    if (-not $?) {
        Write-Host "Getting web ACL associated with the resource failed" -ForegroundColor Red;
        return $null;
    }

    if ($jsonObjects) {
        $strJsonObjects = [string]$jsonObjects;
        $awsObjects = ConvertFrom-Json -InputObject $strJsonObjects;
        $existObject = ($awsObjects.Count -gt 0);
    }
    if ($existObject) {
        $webAclARN = $awsObjects.WebACL.ARN;
        Write-Verbose "Web ACL ARN=$webAclARN is associated with the resource";
        return $webAclARN;
    }
    else {
        Write-Verbose "The resource doesn't have associated web ACL";
        return $null;
    }
    #endregion
}

Function Get-LogGroupARN {
    <#
    .SYNOPSIS
    Get-LogGroupARN Function seek log group by its name and return ARN or $null if a log group is not found.
    .DESCRIPTION
    Get-LogGroupARN Function seek log group by its name and return ARN or $null if a log group is not found.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # log group name
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$LogGroupName,

        # region name
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$AwsProfile = "default"
    )

    #region Initialization
    $functionName = $($myInvocation.MyCommand.Name);
    Write-Host "$($functionName)(webACL=$WebAclName, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;
    #endregion

    #region List log groups with the provided name
    $queryRequest = "logGroups[?logGroupName==``$logGroupName``]";
    $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
        logs describe-log-groups `
        --log-group-name-prefix $logGroupName `
        --query $queryRequest;
    
    if (-not $?) {
        Write-Host "Listing CloudWatch log groups failed" -ForegroundColor Red;
        return $null;
    }
    if ($jsonObjects) {
        $strJsonObjects = [string]$jsonObjects;
        $awsObjects = ConvertFrom-Json -InputObject $strJsonObjects;
        $existObject = ($awsObjects.Count -gt 0);
    }
    if ($existObject) {
        $logGroupARN = $awsObjects.ARN;
        Write-Verbose "Log group '$LogGroupName' is found, ARN=$logGroupARN";
        return $logGroupARN;
    }
    else {
        Write-Verbose "Log group '$LogGroupName' doesn't exist";
        return $null;
    }
    #endregion
}

