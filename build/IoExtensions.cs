using System.IO;
using Nuke.Common.IO;

public static class IoExtensions
{
    public static string GetParentDirectoryName(this AbsolutePath path)
        => Directory.GetParent(path)?.Name;
}
