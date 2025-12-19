$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject 'CN=Revit Addin Dev' -CertStoreLocation Cert:\CurrentUser\My
# $cert | Export-Certificate -FilePath dev-root.cer
# Import-Certificate -FilePath .\dev-root.cer -CertStoreLocation Cert:\CurrentUser\Root | Format-Table
Set-AuthenticodeSignature -FilePath .\Sample\bin\Release\net10.0\Sample.dll -Certificate $cert | Format-Table
