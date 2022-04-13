Function Get-WAF-WebAclARN {
    <#
    .SYNOPSIS
    Get-WAF-WebAclARN Function seek web ACL by its name and return ARN or $null if a web ACL is not found.
    .DESCRIPTION
    Get-WAF-WebAclARN Function seek web ACL by its name and return ARN or $null if a web ACL is not found.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # web ACL name
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [string]$WebAclName,

        # region name
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [string]$AwsProfile = "default"
    )
    Begin {
        $functionName = $($myInvocation.MyCommand.Name);
        Write-Host "$($functionName)(webACL=$WebAclName, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

        $jsonObjects = $null;
        $strJsonObjects = $null;
        $awsObjects = $null;
        $existObject = $false;
    }
    Process {
        # list web ACLs with the provided name
        $queryRequest = "WebACLs[?Name==``$webAclName``]";
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
            Write-Verbose "Web ACL '$webAclName' is found, ARN=$webAclARN";
            return $webAclARN;
        }
        else {
            Write-Verbose "Web ACL '$webAclName' doesn't exist";
            return $null;
        }
    }
    End {
        Write-Host "$($functionName) ends." -ForegroundColor Blue;
    }
}

Function Get-WAF-WebAclForResource {
    <#
    .SYNOPSIS
    Get-WAF-WebAclForResource Function return web ACL ARN if it is accosiated with the resource.
    .DESCRIPTION
    Get-WAF-WebAclForResource Function return web ACL ARN if it is accosiated with the resource. If a resource ARN is wrong or a web ACL is not accosiated with the resource, $null is returned.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Default')]
    Param (
        # resource ARN
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Default')]
        [string]$ResourceARN,

        # region name
        [Parameter(Mandatory = $false, Position = 1, ParameterSetName = 'Default')]
        [string]$RegionName = "us-west-1",

        # AWS profile name from User .aws config file
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [string]$AwsProfile = "default"
    )
    Begin {
        $functionName = $($myInvocation.MyCommand.Name);
        Write-Host "$($functionName)(Resource=$ResourceARN, region=$RegionName, profile=$AwsProfile) starts." -ForegroundColor Blue;

        $jsonObjects = $null;
        $strJsonObjects = $null;
        $awsObjects = $null;
        $existObject = $false;
    }
    Process {
        $jsonObjects = aws --output json --profile $AwsProfile --region $RegionName --color on `
            wafv2 get-web-acl-for-resource `
            --resource-arn $ResourceARN;
    
        if (-not $?) {
            Write-Verbose "Getting web ACL associated with the resource failed" -ForegroundColor Red;
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
    }
    End {
        Write-Host "$($functionName) ends." -ForegroundColor Blue;
    }
}

