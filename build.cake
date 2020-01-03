#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=ReportGenerator&version=4.3.3"
#tool "nuget:?package=GitVersion.CommandLine&version=5.1.1"
#tool "nuget:?package=coveralls.net&version=1.0.0"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"
#addin "nuget:?package=Cake.Coveralls&version=0.10.1"

#load "build/paths.cake"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var reportTypes = Argument("reportTypes", "Html");
var packageVersion = "0.1.0";
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

Task("Version")
   .Does(() => {
      var version = GitVersion();
      Information($"Calculated semantic version: {version.SemVer}");

      packageVersion = version.NuGetVersionV2;
      Information($"Corresponding package version: {packageVersion}");
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
         CoverletOutputFormat = CoverletOutputFormat.opencover,
         CoverletOutputName = $"coverage.xml"
      };
      DotNetCoreTest(Paths.TestProjectDirectory, testSettings, coverletSettings);
   });

Task("Test:Coverage:Report")
   .IsDependentOn("Test")
   .Does(() => {
      var reportSettings = new ReportGeneratorSettings {
         ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
      };

      ReportGenerator("./coverage/coverage.xml", Paths.CoverageDir, reportSettings);
   });

Task("Test:Coverage:Publish")
   .Does(() => {
      var token = EnvironmentVariable("COVERALLS_REPO_TOKEN");

      if(string.IsNullOrWhiteSpace(token))
      {
         Error("Unable to find variable {0} on current environemtn", "COVERALLS_REPO_TOKEN");
      }

      CoverallsNet(
         "./coverage/coverage.xml",
         CoverallsNetReportType.OpenCover,
         new CoverallsNetSettings {
         RepoToken = token
      });
   });

Task("NuGet:Package")
   .IsDependentOn("Version")
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

Task("NuGet:Publish")
   .Does(() => {
      var apiKey = EnvironmentVariable("NUGET_API_KEY");

      if(string.IsNullOrWhiteSpace(apiKey))
      {
         Error("Unable to find variable {0} on current environemtn", "NUGET_API_KEY");
      }

      var settings = new NuGetPushSettings {
         ApiKey = apiKey,
         SkipDuplicate = true
      };

      var packagePath = GetFiles("./artifacts/*.nupkg").Single();
      var symbolPackagePath = GetFiles("./artifacts/*.snupkg").Single();

      NuGetPush(packagePath, settings);
      NuGetPush(symbolPackagePath, settings);
   });

RunTarget(target);