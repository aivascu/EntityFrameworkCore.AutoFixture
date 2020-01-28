public class TestData
{
    private readonly ISetupContext context;
    public const string TestResultsDirectoryArgument = "TestResults";
    public TestData(ISetupContext context)
    {
        this.context = context;
        this.TestResultsDirectory = context.Argument(TestResultsDirectoryArgument, "TestResults");
    }

    public DirectoryPath TestResultsDirectory { get; }

    public void CreateDirectories()
    {
        this.context.EnsureDirectoryExists(this.TestResultsDirectory);
    }

    public void DeleteDirectories()
    {
        if (!this.context.DirectoryExists(this.TestResultsDirectory))
        {
            return;
        }

        this.context.Information($"Removing: {this.TestResultsDirectory}...");

        this.context.DeleteDirectories(
            new [] { this.TestResultsDirectory },
            new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }
}