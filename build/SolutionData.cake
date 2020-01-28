public class SolutionData
{
    private readonly ISetupContext context;
    public const string SolutionPathArgument = "SolutionPath";
    public const string TestProjectsSuffixesArgument = "TestProjectsSuffixes";
    public const string BenchmarkProjectsSuffixesArgument = "BenchmarkProjectsSuffixes";

    public SolutionData(ISetupContext context)
    {
        this.context = context;
        this.SolutionPath = context.Argument<string>(SolutionPathArgument, context.GetFiles("*.sln").First().FullPath);
        this.TestProjectSuffixes = context.Argument(TestProjectsSuffixesArgument, "Tests").Split(';');
        this.BenchmarkProjectsSuffixes = context.Argument(BenchmarkProjectsSuffixesArgument, "Benchmarks").Split(';');
    }

    public FilePath SolutionPath { get; }
    public IEnumerable<SolutionProject> SolutionProjects { get; }
    public IEnumerable<string> TestProjectSuffixes { get; set; }
    public IEnumerable<string> BenchmarkProjectsSuffixes { get; set; }

    public IEnumerable<SolutionProject> GetSolutionProjects()
    {
        return this.context
            .ParseSolution(SolutionPath).Projects
            .Where(p => p.GetType() != typeof(SolutionFolder));
    }

    public IEnumerable<SolutionProject> GetTestProjects()
    {
        return GetSolutionProjects()
            .Where(p => this.TestProjectSuffixes.Any(p.Name.EndsWith));
    }

    public IEnumerable<SolutionProject> GetBenchmakrProjects()
    {
        return GetSolutionProjects()
            .Where(p => this.BenchmarkProjectsSuffixes.Any(p.Name.EndsWith));
    }

    public IEnumerable<SolutionProject> GetAppProjects()
    {
        return GetSolutionProjects()
            .Where(p => !this.TestProjectSuffixes.Any(p.Name.EndsWith)
                        && !this.BenchmarkProjectsSuffixes.Any(p.Name.EndsWith));
    }

    public void DeleteBuildDirectories()
    {
        var cleanupSettings = new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        };

        var binDirectories = this.context.GetDirectories("[Bb]in/");
        this.context.Information($"Removing:\r\n{string.Join("\r\n", binDirectories.Select(d => d.FullPath))}");
        this.context.DeleteDirectories(binDirectories, cleanupSettings);

        var objDirectories = this.context.GetDirectories("[Oo]bj/");
        this.context.Information($"Removing:\r\n{string.Join("\r\n", objDirectories.Select(d => d.FullPath))}");
        this.context.DeleteDirectories(objDirectories, cleanupSettings);
    }
}