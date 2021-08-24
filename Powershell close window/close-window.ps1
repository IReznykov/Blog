param (
    # Command to run
    [Parameter(Mandatory = $true)]
    [ScriptBlock]
    $Command = { Write-Host 'Run command'; },

    # name of the window to close
    [Parameter(Mandatory = $true)]
    [string]$WindowName,

    # number of attempts
    [Parameter(Mandatory = $false)]
    [Int16]$MaxAttempts = 10,

    # delay in seconds
    [Parameter(Mandatory = $false)]
    [Int16]$Delay = 5
)

# script seeks for the windows and send keys
$closeWindowJob = {
    param (
        # name of the window to close
        [Parameter(Mandatory = $true)]
        [string]$WindowName,

        # number of attempts
        [Parameter(Mandatory = $false)]
        [Int16]$MaxAttempts = 10,

        # delay in seconds
        [Parameter(Mandatory = $false)]
        [Int16]$Delay = 5
    )
    Write-Host 'Creating a shell object';
    $wshell = New-Object -ComObject wscript.shell;

    for ($index = 0; $index -lt $MaxAttempts; $index++) {
        Write-Host "#$($index). Seeking for a window";
        $result = $wshell.AppActivate($WindowName);
        if ($result) {
            Write-Host 'Send keys';
            $wshell.SendKeys('{TAB}');
            $wshell.SendKeys('~');
            break;
        }
        else {
            Write-Host "Window '$WindowName' is not found";
            Start-Sleep $Delay;
        }
    }
}

Write-Verbose 'start jobs';
$jobs = New-Object "System.Collections.ArrayList";
$job = Start-Job -Name 'command-job' -ScriptBlock $Command;
$jobs.Add($job) | Out-Null;
$job = Start-Job -Name 'close-window-job' -ScriptBlock $closeWindowJob -ArgumentList $WindowName, $MaxAttempts, $Delay;
$jobs.Add($job) | Out-Null;

Write-Host "Command job Id: $($jobs[0].Id)";
Write-Host "Close window job Id: $($jobs[1].Id)";

# get all jobs in the current session
Get-Job;
$attempt = 0;
# Wait for all jobs to complete
While ((Get-Job -State "Running") -and ($attempt -lt $MaxAttempts)) {
    Write-Verbose "#$($attempt). Sleep $Delay seconds";
    Start-Sleep $Delay;
    ++$attempt;
}

# use job Id as the current session could contain more than 1 job with the name
$job = $null;
foreach ($job in $jobs) {
    $jobState = Get-Job -Id $job.Id;
    if ($jobState.State -eq "Running") {
        Stop-Job -Id $job.Id;
    }
}

Write-Host 'Getting the information back from the jobs';
foreach ($job in $jobs) {
    Get-Job -Id $job.Id; # | Receive-Job;
}