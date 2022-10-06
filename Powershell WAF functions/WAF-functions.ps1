Function Get-WAF2WebAclARN {
    <#
    .SYNOPSIS
    Get-WAF2WebAclARN Function seek web ACL by its name and return ARN or $null if a web ACL is not found.
    .DESCRIPTION
    Get-WAF2WebAclARN Function seek regional web ACL by its name and return ARN or $null if a web ACL is not found.
    .PARAMETER WebAclName
    Name of web ACL which is searched
    .PARAMETER RegionName
    Name of AWS Region where web ACL is searched
    .PARAMETER AwsProfile
    Name of user AWS profile name from .aws config file
    .INPUTS
    None. You cannot pipe objects to Get-WAF2WebAclARN.
    .OUTPUTS
    Get-WAF2WebAclARN returns $null or ARN of found web ACL
    .EXAMPLE
    PS> Get-WAF2WebAclARN "blog-web-acl"
    Returns ARN of web ACL "blog-web-acl" in the us-west-1 region using default credentials
    .EXAMPLE
    PS> Get-WAF2WebAclARN "blog-web-acl" -RegionName "eu-west-1"
    Returns ARN of web ACL "blog-web-acl" in the eu-west-1 region using default credentials
    .EXAMPLE
    PS> Get-WAF2WebAclARN "blog-web-acl" -AWSProfile "BlogAuthor"
    Returns ARN of web ACL "blog-web-acl" in the us-west-1 region using credentials defined by BlogAuthor profile
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

Function Get-WAF2WebAclForResource {
    <#
    .SYNOPSIS
    Get-WAF2WebAclForResource Function return web ACL ARN if it is associated
    with the resource.
    .DESCRIPTION
    Get-WAF2WebAclForResource Function return web ACL ARN if it is associated with the resource. If a resource ARN is wrong or a web ACL is not associated with the resource, $null is returned.
    .PARAMETER ResourceARN
    ARN of the resource which is checked for associated web ACL
    .PARAMETER RegionName
    Name of AWS Region where resource is searched
    .PARAMETER AwsProfile
    Name of user AWS profile name from .aws config file
    .INPUTS
    None. You cannot pipe objects to Get-WAF2WebAclForResource.
    .OUTPUTS
    Get-WAF2WebAclForResource returns $null or ARN of associated web ACL
    .EXAMPLE
    PS> Get-WAF2WebAclForResource "arn:aws:elasticloadbalancing:us-west-1:123456789012:loadbalancer/app/load-balancer-EXAMPLE/0123456789abcdef"
    Returns ARN of web ACL if it associated with "load-balancer-EXAMPLE" Load Balancer in the us-west-1 region, otherwise return $null. To call AWS CLI the function uses default credentials.
    .EXAMPLE
    PS> Get-WAF2WebAclForResource "arn:aws:elasticloadbalancing:us-west-1:123456789012:loadbalancer/app/load-balancer-NONEXISTENT"
    Returns $null as resource ARN doesn't define existent resource in the us-west-1 region.
    .EXAMPLE
    PS> Get-WAF2WebAclForResource "arn:aws:elasticloadbalancing:us-west-1:123456789012:loadbalancer/app/load-balancer-EXAMPLE/0123456789abcdef" -AWSProfile "BlogAuthor"
    Returns ARN of web ACL if it associated with "load-balancer-EXAMPLE" Load Balancer in the us-west-1 region, otherwise return $null. To call AWS CLI the function uses credentials defined by BlogAuthor profile.
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

    #region List associated Web ACL with resource
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
