using System.IO;
using System.Linq;

using Microsoft.Build.Utilities;

using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.SignTool;

using Serilog;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Secret] [Parameter("Password used to protect the pfx file")] readonly string Password;

    [Parameter("Path to the certificate to sign the file")] readonly string Certificate;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(x => x
                .SetConfiguration(Configuration));
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(x => x);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Triggers(Sign)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(x => x
                .SetConfiguration(Configuration));
        });

    Target Sign => _ => _
        .Requires(() => Password)
        .Requires(() => Certificate)
        .Executes(() =>
        {
            var dlls = Directory
                .EnumerateFiles(RootDirectory / "Sample" / "bin" / Configuration / "net10.0", "*.dll")
                .ToList();

            SignToolTasks.SignTool(_ => _
                .SetFileDigestAlgorithm("SHA256")
                .SetFile(Certificate)
                .SetPassword(Password)
                .AddFiles(dlls)
            );
        });
}