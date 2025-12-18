Describe 'Check Signature' {
    It 'Check Signatures' {
        $path = "C:\Users\Petru\projects\csharp\signing-playground\bin\Release\net10.0\signing-playground.dll"
        $signature = Get-AuthenticodeSignature $path
        $signature.Status | Should -Be 'Valid'
        $signature.SignerCertificate | Should -Not -BeNullOrEmpty
    }
}
