Function Get-CloudWatchLogGroupARN {
    <#
    .SYNOPSIS
    Get-CloudWatchLogGroupARN Function seek log group by its name and return ARN or $null if a log group is not found.
    .DESCRIPTION
    Get-CloudWatchLogGroupARN Function seek log group by its name and return ARN or $null if a log group is not found.
    .PARAMETER LogGroupName
    Name of CloudWatch log group which is searched
    .PARAMETER RegionName
    Name of AWS Region where log group is searched
    .PARAMETER AwsProfile
    Name of user AWS profile name from .aws config file
    .INPUTS
    None. You cannot pipe objects to Get-CloudWatchLogGroupARN.
    .OUTPUTS
    Get-CloudWatchLogGroupARN returns $null or ARN of found CloudWatch log group
    .EXAMPLE
    PS> Get-CloudWatchLogGroupARN "blog-log-group"
    Returns ARN of log group "blog-log-group" in the us-west-1 region using default credentials
    .EXAMPLE
    PS> Get-CloudWatchLogGroupARN "blog-log-group" -RegionName "eu-west-1"
    Returns ARN of log group "blog-log-group" in the eu-west-1 region using default credentials
    .EXAMPLE
    PS> Get-CloudWatchLogGroupARN "blog-log-group" -AWSProfile "BlogAuthor"
    Returns ARN of log group "blog-log-group" in the us-west-1 region using credentials defined by BlogAuthor profile
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

Function New-CloudWatchLogGroup {
    <#
    .SYNOPSIS
    New-CloudWatchLogGroup Function create new CloudWatch log group and return its ARN.
    .DESCRIPTION
    New-CloudWatchLogGroup Function check for the existent log group. If it exists, its ARN is returned. If the log group doesn't exist, Function create new CloudWatch log group and return its ARN. If the creation of CloudWatch log group failed, $null is returned.
    .PARAMETER LogGroupName
    Name of CloudWatch log group which is searched
    .PARAMETER RetentionDays
    Retention in days of log group's streams
    .PARAMETER Tags
    Tags of log group. Could be $null.
    .PARAMETER RegionName
    Name of AWS Region where log group is searched
    .PARAMETER AwsProfile
    Name of user AWS profile name from .aws config file
    .INPUTS
    None. You cannot pipe objects to New-CloudWatchLogGroup.
    .OUTPUTS
    New-CloudWatchLogGroup returns $null or ARN of CloudWatch log group
    .EXAMPLE
    PS> New-CloudWatchLogGroup "blog-log-group"
    Returns ARN of log group "blog-log-group" in the us-west-1 region using default credentials
    .EXAMPLE
    PS> New-CloudWatchLogGroup "blog-log-group"
    Returns ARN of log group "blog-log-group" in the us-west-1 region using default credentials
    .EXAMPLE
    PS> New-CloudWatchLogGroup -LogGroupName "blog-log-group" -RetentionDays 90 -Tags "Key1=Value1,Key2=Value2" -RegionName "eu-west-1" -AwsProfile "BlogAuthor"
    Returns ARN of new or existent log group "blog-log-group" with retention period 6 months in the eu-west-1 region using credentials defined by BlogAuthor profile
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
        [int]$RetentionDays = 180,

        # tags of log group
        [Parameter(Mandatory = $false, Position = 2, ParameterSetName = 'Default')]
        [string]$Tags = $null,

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
    $logGroupARN = Get-CloudWatchLogGroupARN `
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

    $logGroupARN = Get-CloudWatchLogGroupARN `
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

