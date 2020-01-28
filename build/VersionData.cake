public class VersionData
{
    public VersionData(ISetupContext context)
    {
        Version = context.GitVersion();
    }

    public GitVersion Version { get; }
}