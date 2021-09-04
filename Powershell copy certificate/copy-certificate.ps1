param (
    # certificate name
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$CertificateName,

    # source store location, could be local or remote computer
    [Parameter(Mandatory = $false)]
    [string]$SourceStoreLocation = 'CurrentUser',

    # source store name
    [Parameter(Mandatory = $false)]
    [string]$SourceStoreName = 'My',

    # target store location, could be local or remote computer
    [Parameter(Mandatory = $false)]
    [string]$TargetStoreLocation = 'LocalMachine',

    # target store name
    [Parameter(Mandatory = $false)]
    [string]$TargetStoreName = 'Root'
)

# get certificate from user storage
$Path = "cert:\$($SourceStoreLocation)\$($SourceStoreName)";
$Certificate = `
    Get-ChildItem -Path $Path -Recurse | `
    Where-Object { $_.FriendlyName -like $CertificateName };
if ((-not $?) -or ($null -eq $Certificate)) {
    Write-Error "Certificate is not found '$CertificateName'";
    exit;
}
else {
    Write-Verbose "Get certificate, thumbrint=$($Certificate.Thumbprint)";
}

# open the target ceritficate store
$CertStore = New-Object System.Security.Cryptography.X509Certificates.X509Store `
    -ArgumentList $TargetStoreName, $TargetStoreLocation;
$CertStore.Open('ReadWrite');
# another way to get the same certificate store
# $StoreName = "cert:\$($TargetStoreLocation)\$($TargetStoreName)";
# $CertStore = Get-Item $StoreName
# $CertStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
if ((-not $?) -or ($null -eq $CertStore)) {
    Write-Error 'Certificate store is not opened';
    exit;
}
else {
    Write-Verbose 'Certificate store is opened';
}
$CertStore.Add($Certificate);
$CertStore.Close();
if (-not $?) {
    Write-Error 'Certificate was not added';
    exit;
}
else {    
    Write-Host "Certificate '$CertificateName' is added to the store 'cert:\$($TargetStoreLocation)\$($TargetStoreName)'" -ForegroundColor Blue;
}
