Describe 'Check Signature' {
    It 'Check Signatures' {
        $root = Split-Path -Path $PSCommandPath -Parent
        $path = Join-Path -Path $root -ChildPath "\Sample\bin\Release\net10.0\Sample.dll"

        $signature = Get-AuthenticodeSignature $path
        $signature.Status | Should -Be 'Valid'
        $signature.SignerCertificate | Should -Not -BeNullOrEmpty
    }
}
