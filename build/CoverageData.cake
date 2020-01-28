public class CoverageData
{
    private readonly ISetupContext context;

    public const string CoverageDirectoryArgument = "coverageDir";
    public const string CoverageReportsDirectoryArgument = "reportDir";
    public const string CoverageReportTypesArgument = "reportTypes";

    public CoverageData(ISetupContext context)
    {
        this.context = context;
        this.CoverageDirectory = context.Argument<string>(CoverageDirectoryArgument, "./coverage");
        this.ReportsDirectory = context.Argument<string>(CoverageReportsDirectoryArgument, "./coverage/reports");
        this.ReportTypes = context.Argument<string>(CoverageReportTypesArgument, "Html");
    }

    public DirectoryPath CoverageDirectory { get; }
    public DirectoryPath ReportsDirectory { get; }
    public string ReportTypes { get; }

    public FilePathCollection GetReportFiles()
    {
        return context.GetFiles($"{this.CoverageDirectory}/*.xml");
    }

    public void CreateDirectories()
    {
        this.context.EnsureDirectoryExists(this.CoverageDirectory);
        this.context.EnsureDirectoryExists(this.ReportsDirectory);
    }

    public void DeleteDirectories()
    {
        if (!this.context.DirectoryExists(this.CoverageDirectory))
        {
            return;
        }

        this.context.Information($"Removing: {this.CoverageDirectory}...");

        this.context.DeleteDirectories(
            new [] { this.CoverageDirectory },
            new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }
}
