<#
1. Check does a web ACL exist for the resource
2. Create a web ACL
3. Create a log group
4. Create a logging configuration
5. Associate the resource and the web ACL

Features:
* Script is idempotent, it checks existent resources.
* Script returns true/false
* Script doesn't catch exceptions
#>

<#
Script creates web ACL and associates it with a regional resource.
#>
Param (
    # Resource ARN
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateNotNullOrEmpty()]
    [string]$ResourceARN,

    # rules file name
    [Parameter(Mandatory = $true, Position = 1)]
    [ValidateNotNullOrEmpty()]
    [string]$RulesFilename,

    # Tag name
    [Parameter(Mandatory = $true, Position = 2)]
    [ValidateNotNullOrEmpty()]
    [string]$TagName,

    # Tag name prefix
    [Parameter(Mandatory = $true, Position = 3)]
    [ValidateNotNullOrEmpty()]
    [string]$TagNamePrefix,

    # AWS Region, could be set in user's credentials.
    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [string]$RegionName = "us-west-1",

    # AWS profile, default value is 'default'
    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [string]$AwsProfile = "default"
)

$startDateTime = $([DateTime]::Now);
$fileName = $(Split-Path -Path $PSCommandPath -Leaf);
Write-Host "Script $fileName start time = $([DateTime]::Now)" -ForegroundColor Blue;

$Verbose = $PSCmdlet.MyInvocation.BoundParameters["Verbose"].IsPresent;
. "$(Split-Path -Path $PSCommandPath)\functions.ps1"

try {
    $webAclName = "${tagNamePrefix}-web-owasp-2";
    $logGroupName = "aws-waf-logs-$webAclName";

    #region Check for the existent web ACL and stop the script if a web ACL exists
    $webAclARN = Get-WebAclARN `
        $webAclName `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$Verbose;
    if (-not $?) {
        Write-Host "Getting web ACL failed" -ForegroundColor Red;
        return $false;
    }
    if ($webAclARN) {
        Write-Host "Web ACL '$webAclName' already exists, script is stopped";
        return $true;
    }
    # Write-Verbose "Web ACL '$webAclName' doesn't exist";
    #endregion

    #region Check the resource for the associated web ACL. also if ResourceARN is wrong, the script is stopped
    $webAclARN = Get-WebAclForResource `
        $ResourceARN `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$Verbose;
    
    if (-not $?) {
        Write-Host "Getting web ACL associated with the resource failed" -ForegroundColor Red;
        return $false;
    }
    if ($webAclARN) {
        Write-Host "Web ACL for the resource is already associated, script is stopped";
        return $true;
    }
    # Write-Verbose "The resource doesn't have associated web ACL";
    #endregion

    #region Create web ACL with predefined set of rule sets
    $rulesFilePath = "$(Split-Path -Path $PSCommandPath -Parent)\$($RulesFilename)";
    Write-Verbose "Rules file path: '$rulesFilePath'";
    if (-not(Test-Path -Path $rulesFilePath -PathType Leaf)) {
        Write-Host "File with rules for a web ACL is not found";
        return $false;
    }
    $rulesContent = (Get-Content $rulesFilePath -Raw) | `
        ForEach-Object { $_.replace('$tagNamePrefix', $tagNamePrefix).replace('"', '""') };

    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;
    
    $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 create-web-acl `
        --name $webAclName `
        --scope REGIONAL `
        --description "Web ACL for Application Load Balancer of Elastic Beanstalk" `
        --default-action "Allow={}" `
        --visibility-config "SampledRequestsEnabled=true, CloudWatchMetricsEnabled=true, MetricName=$tagNamePrefix-web-owasp-metric" `
        --rules $rulesContent `
        --tags "Key=Name,Value=$tagName OWASP Web ACL" "Key=Project,Value=$tagNamePrefix";
    if (-not $?) {
        Write-Host "Creating web ACL failed" -ForegroundColor Red;
        return $false;
    }
    if ($jsonObjects) {
        $strJsonObjects = [string]$jsonObjects;
        $awsObjects = ConvertFrom-Json -InputObject $strJsonObjects;
        $existObject = ($awsObjects.Count -gt 0);
    }
    if ($existObject) {
        $webAclARN = $awsObjects.Summary.ARN;
        $webAclId = $awsObjects.Summary.Id;
    }
    else {
        Write-Host "Creating a web ACL '$webAclName' failed" -ForegroundColor Red;
        return $false;
    }
    Write-Host "Web ACL is created succesfully, Id=$webAclId, ARN=$webAclARN";
    #endregion

    #region Create or use existent log group
    $logGroupTags = @( "Project=$tagNamePrefix" );
    $logGroupARN = New-CloudWatchLogGroupARN `
        -CloudWatchLogGroupname $logGroupName `
        -retentionindays 180 `
        -tags $logGroupTags `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$Verbose;
    
    if ((-not $?) -or (-not $logGroupARN)) {
        Write-Host "Getting log group failed" -ForegroundColor Red;
        return $false;
    }
    Write-Host "Log group '$logGroupName' is found, ARN=$logGroupARN";
    #endregion

    #region Add Web ACL logging
    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;

    if ($logGroupARN.EndsWith(":*")) {
        $logGroupARN = $logGroupARN.TrimEnd(":*");
    }
    $queryRequest = "LoggingConfigurations[?contains(ResourceArn, ``$webAclARN``) == ``true``]";
    $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 list-logging-configurations `
        --scope REGIONAL `
        --query $queryRequest;
    
    if (-not $?) {
        Write-Host "Getting logging configurations for web ACLs failed" -ForegroundColor Red;
        return $false;
    }
    else {
        if ($jsonObjects) {
            $strJsonObjects = [string]$jsonObjects;
            $loggingConfigurations = ConvertFrom-Json -InputObject $strJsonObjects;
            $existObject = ($loggingConfigurations.Count -gt 0);
        }
        if ($existObject) {
            Write-Verbose "Web ACL '$webAclARN' already has logging configuration";
        }
    }
    if (-not $existObject) {
        Write-Verbose "Adding logging configuration to Web ACL '$webAclARN'";
        $configuration = @"
{  
    \"ResourceArn\": \"$webAclARN\",
    \"LogDestinationConfigs\": [
      \"$logGroupARN\"
    ],
    \"RedactedFields\": [
      {
        \"SingleHeader\": {
          \"Name\": \"password\"
        }
      }
    ],
    \"ManagedByFirewallManager\": false,
    \"LoggingFilter\": {
        \"DefaultBehavior\": \"KEEP\",
        \"Filters\": [
            {
                \"Behavior\": \"KEEP\",
                \"Conditions\": [
                    {
                        \"ActionCondition\": {
                            \"Action\": \"BLOCK\"
                        }
                    }
                ],
                \"Requirement\": \"MEETS_ANY\"
            }
        ]
    }
}
"@
        $loggingConfiguration = aws --output json --profile $AwsProfile --region $RegionName --color on `
            wafv2 put-logging-configuration `
            --logging-configuration $configuration;
        if (-not $?) {
            Write-Host "Creating a web ACL logging configuration failed" -ForegroundColor Red;
            return $false;
        }
        else {
            Write-Verbose "Logging configuration to Web ACL '$webAclARN' is added";
        }
    }
    #endregion

    #region Add Web ACL association
    Write-Host "Pause the script until a web ACL completes initialization";
    Start-Sleep -Seconds 15;
    aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 associate-web-acl `
        --web-acl-arn $webAclARN `
        --resource-arn $ResourceARN;
    if (-not $?) {
        Write-Host "Web ACL association is not created" -ForegroundColor Red;
        return $false;
    }        

    Write-Host "Web ACL is created and is associated with the resource:`nweb ACL ARN=$webAclARN`nResource ARN=$ResourceARN";
    #endregion

    return $true;
}
finally {
    $scriptDuration = [DateTime]::Now - $startDateTime;
    $fileName = $(Split-Path -Path $PSCommandPath -Leaf);
    Write-Host "**************************************************" -ForegroundColor Green;
    Write-Host "Script $fileName ends, total duration $($scriptDuration.Days) day(s), $($scriptDuration.Hours):$($scriptDuration.Minutes):$($scriptDuration.Seconds).$($scriptDuration.Milliseconds)" -ForegroundColor Blue;
}
