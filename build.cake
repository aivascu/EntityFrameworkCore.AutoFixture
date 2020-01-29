#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=ReportGenerator&version=4.3.3"
#tool "nuget:?package=GitVersion.CommandLine&version=5.1.1"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"

#load "build/BuildData.cake"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

// SETUP
Setup<BuildData>((context) => {
   return new BuildData(context);
});

// TASKS

Task("Clean")
   .Does<BuildData>((data) => {
      data.CoverageData.DeleteDirectories();
      data.SolutionData.DeleteBuildDirectories();
      data.TestData.DeleteDirectories();
      data.PackageData.DeleteDirectories();
   });

Task("Restore")
   .IsDependentOn("Clean")
   .IsDependentOn("GitHub:Output:Version")
   .IsDependentOn("DotNet:Telemetry:Optout")
   .Does<BuildData>((data) =>{
      var settings = new DotNetCoreRestoreSettings {
         WorkingDirectory = data.SolutionData.SolutionPath.GetDirectory()
      };
      DotNetCoreRestore(settings);
   } );

Task("Build")
   .IsDependentOn("Restore")
   .Does<BuildData>((data) => {
      DotNetCoreBuild(
         data.SolutionData.SolutionPath.ToString(),
         new DotNetCoreBuildSettings
         { 
            Configuration = data.Configuration,
            ArgumentCustomization =
               args => args.Append($"/p:Version={data.VersionData.Version.NuGetVersionV2}")
         });
   });

Task("Test")
   .IsDependentOn("Build")
   .Does<BuildData>((data) => {
      data.CoverageData.CreateDirectories();
      var testSettings = new DotNetCoreTestSettings {
         NoBuild = true,
         Configuration = data.Configuration,
         ResultsDirectory = data.TestData.TestResultsDirectory,
         ArgumentCustomization = args => args.Append($"--logger trx")
      };
      var coverletSettings = new CoverletSettings {
         CollectCoverage = true,
         CoverletOutputDirectory = data.CoverageData.CoverageDirectory,
         CoverletOutputFormat = CoverletOutputFormat.opencover,
      };

      foreach(var project in data.SolutionData.GetTestProjects())
      {
         coverletSettings.CoverletOutputName = $"{project.Name}.opencover.xml";
         DotNetCoreTest(project.Path.FullPath, testSettings, coverletSettings);
      }
   });

Task("Test:Coverage")
   .IsDependentOn("Test")
   .Does<BuildData>((data) => {
      var reportSettings = new ReportGeneratorSettings {
         ArgumentCustomization = args => args.Append($"-reportTypes:{data.CoverageData.ReportTypes}")
      };
      var reports = data.CoverageData.GetReportFiles();
      ReportGenerator(reports, data.CoverageData.ReportsDirectory, reportSettings);
   });

Task("NuGet:Package")
   .IsDependentOn("Build")
   .Does<BuildData>((data) => {
      data.PackageData.CreateDirectories();

      var packSettings = new DotNetCorePackSettings 
      {
         Configuration = data.Configuration,
         OutputDirectory = data.PackageData.PackagesDirectory,
         NoBuild = true,
         IncludeSymbols = true,
         ArgumentCustomization =
            args => args.Append($"/p:Version={data.VersionData.Version.NuGetVersionV2} /p:SymbolPackageFormat=snupkg")
      };

      foreach(var project in data.SolutionData.GetAppProjects())
      {
         Information("Packaging: {0}...", project.Name);
         DotNetCorePack(project.Path.ToString(), packSettings);
      }
   });

Task("NuGet:Publish")
   .IsDependentOn("NuGet:Package")
   .Does<BuildData>((data) => {
      var settings = new NuGetPushSettings {
         ApiKey = data.PackageData.ApiKey,
         Source = "https://api.nuget.org/v3/index.json",
         SkipDuplicate = true
      };
      var packageFiles = data.PackageData.GetPackageFiles();
      NuGetPush(packageFiles, settings);
   });

Task("GitHub:Output:Version")
   .Does<BuildData>(data => {
      Information("::set-output name={0}::{1}", "version", data.VersionData.Version.MajorMinorPatch);
   });

Task("DotNet:Telemetry:Optout")
   .Does<BuildData>((data) => {
      data.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "true");
   });

RunTarget(target);