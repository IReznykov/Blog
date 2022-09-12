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
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false)]
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
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false)]
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
        Write-Host "Getting web ACL associated with the resource failed, check the Resource ARN" -ForegroundColor Red;
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
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$AwsProfile = "default"
    )

    #region Initialization
    $functionName = $($myInvocation.MyCommand.Name);
    Write-Host "$($functionName)(LogGroup=$LogGroupName, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

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

Function New-LogGroup {
    <#
    .SYNOPSIS
    New-LogGroup Function creats CloudWatch log group.
    .DESCRIPTION
    New-LogGroup Function creats CloudWatch log group.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # log group name
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [ValidateNotNullOrEmpty()]
        [string]$LogGroupName,

        # log retention in days
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [ValidateRange(1, 360)]
        [string]$RetentionDays = 180,

        # Tag name prefix
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [array]$Tags = $null,

        # region name
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]$AwsProfile = "default"
    )

    #region Initialization
    $functionName = $($myInvocation.MyCommand.Name);
    Write-Host "$($functionName)(LogGroup=$LogGroupName, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;
    #endregion

    #region Create CloudWatch log group
    $logGroupARN = Get-LogGroupARN `
        $logGroupName `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$Verbose;

    if (-not $?) {
        Write-Host "Getting log group failed" -ForegroundColor Red;
        return $null;
    }
    if (-not $logGroupARN) {
        Write-Verbose "Log group '$logGroupName' doesn't exist, let's create it";

        # no output
        aws --output json --profile $AwsProfile --region $RegionName --color on `
            logs create-log-group `
            --log-group-name $logGroupName `
            --tags $Tags;
        
        if (-not $?) {
            Write-Host "Creating CloudWatch log group failed" -ForegroundColor Red;
            return $null;
        }
    }
    # no output
    aws --output json --profile $AwsProfile --region $RegionName --color on `
        logs put-retention-policy `
        --log-group-name $logGroupName `
        --retention-in-days $RetentionDays;
        
    if (-not $?) {
        Write-Host "Updating CloudWatch log group failed" -ForegroundColor Red;
        return $null;
    }

    $logGroupARN = Get-LogGroupARN `
        $logGroupName `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$Verbose;

    if (-not $?) {
        Write-Host "Getting log group failed" -ForegroundColor Red;
        return $null;
    }
    else {
        return $logGroupARN;
    }

    #endregion
}

