public class VersionData
{
    private readonly ISetupContext context;

    public VersionData(ISetupContext context)
    {
        this.context = context;
        Version = context.GitVersion();
        this.context.Information("Version: {0}", Version.SemVer);
    }

    public GitVersion Version { get; }
}