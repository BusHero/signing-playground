$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject 'CN=Revit Addin Dev' -CertStoreLocation Cert:\CurrentUser\My
$cert | Export-Certificate -FilePath dev-root.cer
Import-Certificate -FilePath .\dev-root.cer -CertStoreLocation Cert:\CurrentUser\Root | Format-Table
Set-AuthenticodeSignature -FilePath .\bin\Release\net10.0\signing-playground.dll -Certificate $cert | Format-Table
