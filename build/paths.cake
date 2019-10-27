public static class Paths
{
    public static FilePath SolutionFile => "EntityFrameworkCore.AutoFixture.sln";
    public static string ObjPattern => "src/**/[Oo]bj";
    public static string BinPattern => "src/**/[Bb]in";
    public static DirectoryPath CoverageDir => "coverage";
    public static string CoveragePattern => "**/[Cc]overage";
    public static string TestProjectDirectory => "src/EntityFrameworkCore.AutoFixture.Tests";
    public static FilePath CoverageFile => "coverage.xml";
}