using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.PowerShell;
using Nuke.Common.Tools.Pwsh;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

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
        .Executes(() =>
        {
            PwshTasks.Pwsh(_ => _
                .EnableNoProfile()
                .SetCommand("./sign.ps1"));
        });
}