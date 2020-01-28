#load "./CoverageData.cake"
#load "./PackagesData.cake"
#load "./SolutionData.cake"
#load "./TestData.cake"
#load "./VersionData.cake"

public class BuildData
{
    public const string ConfigurationArgument = "configuration";

    public BuildData(ISetupContext context)
    {
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
}
