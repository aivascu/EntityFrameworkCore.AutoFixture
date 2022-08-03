using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;

public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        return new TestDbContext(options);
    }
}
