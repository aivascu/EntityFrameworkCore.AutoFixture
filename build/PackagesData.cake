
public class PackagesData
{
    private readonly ISetupContext context;
    public const string PackageOutputArgument = "PacakgeDir";
    public const string NuGetApiKeyEnvironmentVariable = "NUGET_API_KEY";
    public PackagesData(ISetupContext context)
    {
        this.context = context;
        this.PackagesDirectory = context.Argument(PackageOutputArgument, "./artifacts");
        this.ApiKey = context.EnvironmentVariable(NuGetApiKeyEnvironmentVariable, string.Empty);
    }

    public DirectoryPath PackagesDirectory { get; }
    public string ApiKey { get; }

    public FilePathCollection GetPackageFiles()
    {
        return this.context.GetFiles(
            $"{this.PackagesDirectory}/*.nupkg", 
            new GlobberSettings {
                FilePredicate = (file) => !file.Path
                    .GetFilenameWithoutExtension()
                    .GetExtension()
                    .Equals(".symbols")
            }
        );
    }

    public FilePathCollection GetSymbolFiles()
    {
        return this.context.GetFiles(
            $"{this.PackagesDirectory}/*nupkg",
            new GlobberSettings {
                FilePredicate = (file) => {
                    var fileName = file.Path.GetFilename().ToString();
                    return fileName.EndsWith(".snupkg")
                        || fileName.EndsWith(".symbols.nupkg");
                }
            }
        );
    }

    public void CreateDirectories()
    {
        this.context.EnsureDirectoryExists(this.PackagesDirectory);
    }

    public void DeleteDirectories()
    {
        if (!this.context.DirectoryExists(this.PackagesDirectory))
        {
            return;
        }

        this.context.Information($"Removing: {this.PackagesDirectory}...");

        this.context.DeleteDirectories(
            new []{ this.PackagesDirectory },
            new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }
}