#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=ReportGenerator&version=4.3.3"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"

#load "build/paths.cake"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

var reportTypes = "HtmlInline_AzurePipelines";
var cleanupSettings = new DeleteDirectorySettings {
   Recursive = true,
   Force = true
};

Setup(context => {

   if(DirectoryExists(Paths.CoverageDir))
   {
      DeleteDirectory(Paths.CoverageDir, cleanupSettings);
      Verbose("Removed coverage folder");
   }

   var binDirs = GetDirectories(Paths.BinPattern);
   if(binDirs.Count > 0)
   {
      DeleteDirectories(binDirs, cleanupSettings);
      Verbose("Removed {0} \"bin\" directories", binDirs.Count);
   }

   var objDirs = GetDirectories(Paths.ObjPattern);
   if(objDirs.Count > 0)
   {
      DeleteDirectories(objDirs, cleanupSettings);
      Verbose("Removed {0} \"obj\" directories", objDirs.Count);
   }
});

// TASKS

Task("Clean")
   .Does(() => {
      DeleteDirectory(Paths.CoverageDir, cleanupSettings);
   });

Task("Restore")
   .Does(() => DotNetCoreRestore());

Task("Build")
   .IsDependentOn("Restore")
   .Does(() => {
      DotNetCoreBuild(
         Paths.SolutionFile.ToString(),
         new DotNetCoreBuildSettings
         { 
            Configuration = configuration
         });
   });

Task("Test")
   .IsDependentOn("Restore")
   .Does(() => {
      EnsureDirectoryExists(Paths.CoverageDir);
      var testSettings = new DotNetCoreTestSettings {
      };
      var coverletSettings = new CoverletSettings {
         CollectCoverage = true,
         CoverletOutputDirectory = Paths.CoverageDir,
         CoverletOutputFormat = CoverletOutputFormat.opencover,
         CoverletOutputName = $"results.xml"
      };
      DotNetCoreTest(Paths.TestProjectDirectory, testSettings, coverletSettings);
   });

Task("Report")
   .IsDependentOn("Test")
   .Does(() => {
      var reportSettings = new ReportGeneratorSettings {
         ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
      };

      ReportGenerator("./coverage/*.xml", Paths.CoverageDir, reportSettings);
   });

RunTarget(target);