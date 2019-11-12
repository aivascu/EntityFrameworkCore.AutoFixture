#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=ReportGenerator&version=4.3.3"
#tool "nuget:?package=GitVersion.CommandLine&version=5.1.1"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"

#load "build/paths.cake"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var packageVersion = "0.1.0";
var reportTypes = "HtmlInline_AzurePipelines";
var solutionPath = "EntityFrameworkCore.AutoFixture.sln";
var cleanupSettings = new DeleteDirectorySettings {
   Recursive = true,
   Force = true
};

// TASKS

Task("Clean")
   .Does(() => {
      if (DirectoryExists(Paths.CoverageDir))
      {
         DeleteDirectory(Paths.CoverageDir, cleanupSettings);
         Verbose("Removed coverage folder");
      }

      var binDirs = GetDirectories(Paths.BinPattern);
      if (binDirs.Count > 0)
      {
         DeleteDirectories(binDirs, cleanupSettings);
         Verbose("Removed {0} \"bin\" directories", binDirs.Count);
      }

      var objDirs = GetDirectories(Paths.ObjPattern);
      if (objDirs.Count > 0)
      {
         DeleteDirectories(objDirs, cleanupSettings);
         Verbose("Removed {0} \"obj\" directories", objDirs.Count);
      }

      var testResultsDirs = GetDirectories(Paths.TestResultsPattern);
      if (testResultsDirs.Count > 0)
      {
         DeleteDirectories(testResultsDirs, cleanupSettings);
         Verbose("Removed {0} \"TestResults\" directories", testResultsDirs.Count);
      }

      var artifactDir = GetDirectories(Paths.ArtifactsPattern);
      if (artifactDir.Count > 0)
      {
         DeleteDirectories(artifactDir, cleanupSettings);
         Verbose("Removed {0} artifact directories", artifactDir.Count);
      }
   });

Task("Restore")
   .IsDependentOn("Clean")
   .Does(() => DotNetCoreRestore());

Task("Build")
   .IsDependentOn("Restore")
   .IsDependentOn("Version")
   .Does(() => {
      DotNetCoreBuild(
         Paths.SolutionFile.ToString(),
         new DotNetCoreBuildSettings
         { 
            Configuration = configuration,
            ArgumentCustomization = args => args.Append($"/p:Version={packageVersion}")
         });
   });

Task("Test")
   .IsDependentOn("Build")
   .Does(() => {
      EnsureDirectoryExists(Paths.CoverageDir);
      var testSettings = new DotNetCoreTestSettings {
         NoBuild = true,
         Configuration = configuration,
         ResultsDirectory = Directory("TestResults"),
         ArgumentCustomization = args => args.Append($"--logger trx")
      };
      var coverletSettings = new CoverletSettings {
         CollectCoverage = true,
         CoverletOutputDirectory = Paths.CoverageDir,
         CoverletOutputFormat = CoverletOutputFormat.cobertura,
         CoverletOutputName = $"coverage.cobertura.xml"
      };
      DotNetCoreTest(Paths.TestProjectDirectory, testSettings, coverletSettings);
   });

Task("Report")
   .IsDependentOn("Test")
   .Does(() => {
      var reportSettings = new ReportGeneratorSettings {
         ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
      };

      ReportGenerator("./coverage/coverage.cobertura.xml", Paths.CoverageDir, reportSettings);
   });

Task("Version")
   .Does(() => {
      var version = GitVersion();
      Information($"Calculated semantic version: {version.SemVer}");

      packageVersion = version.NuGetVersionV2;
      Information($"Corresponding package version: {packageVersion}");
   });

Task("Package")
   .IsDependentOn("Build")
   .Does(() => {
      EnsureDirectoryExists("./artifacts");

      var projects = ParseSolution(solutionPath).Projects
      .Where(p => p.GetType() != typeof(SolutionFolder) && !p.Name.EndsWith("Tests"));

      foreach(var project in projects)
      {
         Information($"Packaging project {project.Name}...");
         DotNetCorePack(project.Path.ToString(), new DotNetCorePackSettings {
            Configuration = configuration,
            OutputDirectory = Directory("./artifacts/"),
            IncludeSymbols = true,
            ArgumentCustomization = args => args.Append($"/p:Version={packageVersion} /p:SymbolPackageFormat=snupkg")
         });
      }
   });

Task("Publish")
   .IsDependentOn("Package")
   .Does(() => {
      var settings = new NuGetPushSettings {
         ApiKey = EnvironmentVariable("NuGetApiKey"),
         SkipDuplicate = true
      };

      foreach(var file in GetFiles("./artifacts/*.nupkg"))
      {
         NuGetPush(file, settings);
      }
   });

RunTarget(target);