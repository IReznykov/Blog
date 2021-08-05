# script runs dotnet command
$trustJob = {
    $inputFile = """$($Env:ProgramFiles)\dotnet\dotnet.exe""";
    Write-Host "Run $inputFile";
    Start-Process $inputFile -ArgumentList `
        'dev-certs', 'https', '--trust' -Wait | Out-Host;
}
# script seeks for the windows and send keys
$closeWindowJob = {
    Write-Host 'Seeking for a window';
    $wshell = New-Object -ComObject wscript.shell;
    $result = $wshell.AppActivate('Security Warning');
    Start-Sleep 5;
    if($result) {
        Write-Host 'Send keys'
        $wshell.SendKeys('{TAB}');
        $wshell.SendKeys('~');
    }
}
Write-Verbose 'start jobs';
Start-Job $trustJob;
Start-Sleep 5;
Start-Job $closeWindowJob;

Get-Job;
 
# Wait for all jobs to complete
While (Get-Job -State "Running")
{
    Write-Verbose 'Sleep 10';
    Start-Sleep 10;
}
 
Write-Host 'Getting the information back from the jobs';
Get-Job | Receive-Job;
