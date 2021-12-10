using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(Framework = "netcoreapp3.0")] readonly GitVersion GitVersion;

    [Parameter] readonly bool Deterministic;

    [Parameter, Secret] readonly string NuGetApiKey;
    [Parameter] readonly string NuGetSource = "https://api.nuget.org/v3/index.json";

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "testresults";
    AbsolutePath CoverageDirectory => ArtifactsDirectory / "coverage";
    AbsolutePath ReportsDirectory => ArtifactsDirectory / "reports";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    IEnumerable<Project> TestProjects => Solution.GetProjects("*Tests");
    IReadOnlyCollection<AbsolutePath> Packages => PackagesDirectory.GlobFiles("*.nupkg");

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetNoRestore(FinishedTargets.Contains(Restore))
                .SetContinuousIntegrationBuild(IsServerBuild || Deterministic)
                .SetDeterministic(IsServerBuild || Deterministic));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Produces(TestResultsDirectory / "*.trx")
        .Produces(CoverageDirectory / "*.xml")
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetNoBuild(FinishedTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultsDirectory)
                .SetLogger("trx")
                .SetUseSourceLink(IsServerBuild)
                .SetProcessArgumentConfigurator(a => a
                    .Add("-- RunConfiguration.DisableAppDomain=true")
                    .Add("-- RunConfiguration.NoAutoReporters=true"))
                .When(InvokedTargets.Contains(Cover), _ => _
                    .SetDataCollector("XPlat Code Coverage"))
                .CombineWith(TestProjects, (_, p) => _
                    .SetProjectFile(p)
                    .When(InvokedTargets.Contains(Cover), _ => _
                        .SetResultsDirectory(TestResultsDirectory / p.Name))));

            Debug.Assert(
                TestResultsDirectory.GlobFiles("**\\*.trx").Count > 0,
                "No trx files were generated.");

            if (!Directory.Exists(CoverageDirectory))
            {
                Directory.CreateDirectory(CoverageDirectory);
            }

            TestResultsDirectory.GlobFiles("**\\*.xml")
                .Where(x => Guid.TryParse(x.GetParentDirectoryName(), out var _))
                .ForEach(x => File.Copy(x, CoverageDirectory / $"{x.Parent.GetParentDirectoryName()}.xml", true));

            if (InvokedTargets.Contains(Cover))
            {
                Debug.Assert(
                    CoverageDirectory.GlobFiles("**\\*.xml").Count > 0,
                    "No xml coverage files were generated.");
            }
        });

    Target Cover => _ => _
        .DependsOn(Test)
        .Consumes(Test)
        .Produces(ReportsDirectory / "*.html")
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetFramework("net5.0")
                .SetReports(CoverageDirectory / "*.xml")
                .SetTargetDirectory(ReportsDirectory)
                .SetReportTypes("lcov", ReportTypes.HtmlInline));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Consumes(Compile)
        .After(Test)
        .Produces(PackagesDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagesDirectory)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetNoBuild(FinishedTargets.Contains(Compile))
                .SetIncludeSymbols(IsServerBuild || Deterministic)
                .SetDeterministic(IsServerBuild || Deterministic)
                .SetVersion(GitVersion.NuGetVersion)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Consumes(Pack)
        .OnlyWhenDynamic(() => IsServerBuild)
        .OnlyWhenDynamic(() => !NuGetApiKey.IsNullOrEmpty())
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .SetSource(NuGetSource)
                .SetApiKey(NuGetApiKey)
                .EnableSkipDuplicate()
                .CombineWith(Packages, (_, file) => _
                    .SetTargetPath(file)));
        });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);
}
