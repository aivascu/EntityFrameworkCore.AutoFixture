using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

public class SqliteOptionsFactory : DbContextOptionsFactory
{
    protected override DbContextOptions<TContext> Create<TContext>(ISpecimenContext context)
    {
        var connection = context.Create<SqliteConnection>();

        return new DbContextOptionsBuilder<TContext>()
            .UseSqlite(connection)
            .Options;
    }
}
