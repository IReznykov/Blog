$CertificateName = 'ASP.NET Core HTTPS development certificate';

.\trust-certificate.ps1 `
    $CertificateName `
    -SourceStoreLocation 'CurrentUser' `
    -SourceStoreName 'My' `
    -TargetStoreLocation 'LocalMachine' `
    -TargetStoreName 'Root' `
    -Verbose;
