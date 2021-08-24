# script runs dotnet command
$Command = {
    $inputFile = """$($Env:ProgramFiles)\dotnet\dotnet.exe""";
    Write-Host "Run $inputFile";
    Start-Process $inputFile -ArgumentList `
        'dev-certs', 'https', '--trust' -Wait | Out-Host;
}
$WindowName = 'Security Warning';

.\close-window.ps1 `
    -Command $Command `
    -WindowName $WindowName `
    -Verbose;
