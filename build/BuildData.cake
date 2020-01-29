#load "./CoverageData.cake"
#load "./PackagesData.cake"
#load "./SolutionData.cake"
#load "./TestData.cake"
#load "./VersionData.cake"

public class BuildData
{
    private readonly ISetupContext context;
    public const string ConfigurationArgument = "configuration";

    public BuildData(ISetupContext context)
    {
        this.context = context;
        Configuration = context.Argument<string>(ConfigurationArgument, "Release");
        CoverageData = new CoverageData(context);
        PackageData = new PackagesData(context);
        SolutionData = new SolutionData(context);
        TestData = new TestData(context);
        VersionData = new VersionData(context);
    }

    public string Configuration { get; }
    public CoverageData CoverageData { get; }
    public PackagesData PackageData { get; }
    public SolutionData SolutionData { get; }
    public TestData TestData { get; }
    public VersionData VersionData { get; }

    public void SetEnvironmentVariable(string key, string value)
    {
        System.Environment.SetEnvironmentVariable(key, value);
        var actualValue = Environment.GetEnvironmentVariable(key);

        if(value != actualValue)
        {
            this.context.Warning("Environment variable {0} expected to be {1}, is actually {2}.", key, value, actualValue);
        }

        this.context.Information("Environment variable {0} set to {1}", key, value);
    }
}
