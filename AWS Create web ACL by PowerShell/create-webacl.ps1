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
    [string]$ResourceARN,

    # rules file name
    [Parameter(Mandatory = $true, Position = 1)]
    [string]$RulesFilename,

    # Tag name
    [Parameter(Mandatory = $true, Position = 2)]
    [string]$TagName,

    # Tag name prefix
    [Parameter(Mandatory = $true, Position = 3)]
    [string]$TagNamePrefix,

    # AWS Region, could be set in user's credentials.
    [Parameter(Mandatory = $false)]
    [string]$RegionName = "us-west-1",

    # AWS profile, default value is 'default'
    [Parameter(Mandatory = $false)]
    [string]$AwsProfile = "default"
)

$startDateTime = $([DateTime]::Now);
$fileName = $(Split-Path -Path $PSCommandPath -Leaf);
Write-Host "Script $fileName start time = $([DateTime]::Now)" -ForegroundColor Blue;

# ShowOutput == $true gives detailed output
$ShowOutput = $PSCmdlet.MyInvocation.BoundParameters["Verbose"].IsPresent;

. "$(Split-Path -Path $PSCommandPath)\WAF-functions.ps1"

# Actually these checks are false, as it checks by PS for mandatory parameters
if (-not $ResourceARN) { throw "-resourcearn is required."; }
if (-not $RulesFilename) { throw "-rulesfilename is required."; }
if (-not $TagName) { throw "-tagname is required."; }
if (-not $TagNamePrefix) { throw "-tagnameprefix is required."; }

try {
    $webAclName = "${tagNamePrefix}-web-owasp-2";
    $logsGroupName = "aws-waf-logs-$webAclName";

    # check for the existent web ACL and stop the script if a web ACL exists
    $webAclARN = Get-WAF-WebAclARN `
        $webAclName `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$ShowOutput;
    if (-not $?) {
        Write-Host "Getting web ACL failed" -ForegroundColor Red;
        return $false;
    }
    if ($webAclARN) {
        Write-Host "Web ACL '$webAclName' already exists, script is stopped";
        return $true;
    }
    # Write-Verbose "Web ACL '$webAclName' doesn't exist";

    # check the resource for the associated web ACL. also if ResourceARN is wrong, the script is stopped
    $webAclARN = Get-WAF-WebAclForResource `
        $ResourceARN `
        -regionname $RegionName -awsprofile $AwsProfile `
        -verbose:$ShowOutput;
    
    if (-not $?) {
        Write-Host "Getting web ACL associated with the resource failed" -ForegroundColor Red;
        return $false;
    }
    if ($webAclARN) {
        Write-Host "Web ACL for the resource is already associated, script is stopped";
        return $true;
    }
    # Write-Verbose "The resource doesn't have associated web ACL";

    # create web ACL with predefined set of rule sets
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

    # create or use existent log group
    $attempt = 0;
    do {
        $jsonObjects = $null;
        $strJsonObjects = $null;
        $awsObjects = $null;
        $existObject = $false;
    
        $queryRequest = "logGroups[?logGroupName==``$logsGroupName``]";
        $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
            logs describe-log-groups `
            --log-group-name-prefix $logsGroupName `
            --query $queryRequest;

        if (-not $?) {
            Write-Host "Listing CloudWatch log groups failed" -ForegroundColor Red;
            return $false;
        }
        else {
            if ($jsonObjects) {
                $strJsonObjects = [string]$jsonObjects;
                $awsObjects = ConvertFrom-Json -InputObject $strJsonObjects;
                $existObject = ($awsObjects.Count -gt 0);
            }
            if ($existObject) {
                Write-Verbose "Log group '$logsGroupName' exists, let's use it";
                $logsGroupARN = $awsObjects.arn;
                break;
            }
        }

        if (-not $existObject) {
            Write-Verbose "Log group '$logsGroupName' doesn't exists, let's create it";
            # no output
            aws --output json --profile $AwsProfile --region $RegionName --color on `
                logs create-log-group `
                --log-group-name $logsGroupName `
                --tags "Project=$tagNamePrefix";
        
            if (-not $?) {
                Write-Host "Creating CloudWatch log group failed" -ForegroundColor Red;
                return $false;
            }

            # no output
            aws --output json --profile $AwsProfile --region $RegionName --color on `
                logs put-retention-policy `
                --log-group-name $logsGroupName `
                --retention-in-days 180;
        
            if (-not $?) {
                Write-Host "Updating CloudWatch log group failed" -ForegroundColor Red;
                return $false;
            }
        }
        ++$attempt;
    } while ($attempt -lt 2);

    Write-Host "CloudWatch log group with ARN=$logsGroupARN is used";

    # add Web ACL logging
    $jsonObjects = $null;
    $strJsonObjects = $null;
    $awsObjects = $null;
    $existObject = $false;

    if ($logsGroupARN.EndsWith(":*")) {
        $logsGroupARN = $logsGroupARN.TrimEnd(":*");
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
      \"$logsGroupARN\"
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

    # add Web ACL association
    Write-Host "Pause the script until a web ACL completes initialization";
    Start-Sleep -Seconds 15;
    aws --output json --profile $AwsProfile --region $RegionName --color on `
        wafv2 associate-web-acl `
        --web-acl-arn $webAclARN `
        --resource-arn $ResourceARN;
    if (-not $?) {
        # Error is thrown, get details: $msg = $Error[0].Exception.Message
        Write-Host "Web ACL association is not created" -ForegroundColor Red;
        return $false;
    }        

    Write-Host "Web ACL is created and is associated with the resource:`nweb ACL ARN=$webAclARN`nResource ARN=$ResourceARN";
    return $true;
}
finally {
    $scriptDuration = [DateTime]::Now - $startDateTime;
    $fileName = $(Split-Path -Path $PSCommandPath -Leaf);
    Write-Host "**************************************************" -ForegroundColor Green;
    Write-Host "Script $fileName ends, total duration $($scriptDuration.Days) day(s), $($scriptDuration.Hours):$($scriptDuration.Minutes):$($scriptDuration.Seconds).$($scriptDuration.Milliseconds)" -ForegroundColor Blue;
}
