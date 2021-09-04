$CertificateName = 'ASP.NET Core HTTPS development certificate';

.\copy-certificate.ps1 `
    $CertificateName `
    -SourceStoreLocation 'CurrentUser' `
    -SourceStoreName 'My' `
    -TargetStoreLocation 'LocalMachine' `
    -TargetStoreName 'Root' `
    -Verbose;
