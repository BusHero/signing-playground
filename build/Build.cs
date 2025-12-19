using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.SignTool;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Secret] 
    [Parameter("Password used to protect the pfx file")] 
    readonly string Password;
    
    [Parameter("Password used to protect the pfx file")] 
    readonly string Certificate;

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
            SignToolTasks.SignTool(_ => _
                .SetFileDigestAlgorithm("SHA256")
                .SetFile(Certificate)
                .SetPassword(Password)
                .AddFiles(".\\Sample\\bin\\Release\\net10.0\\Sample.dll")
            );
        });
}