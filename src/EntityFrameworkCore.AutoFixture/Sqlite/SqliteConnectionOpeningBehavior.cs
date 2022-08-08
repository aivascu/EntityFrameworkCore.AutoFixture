using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

public class SqliteConnectionOpeningBehavior : ConnectionOpeningBehavior
{
    public SqliteConnectionOpeningBehavior()
        : base(new ExactTypeSpecification(typeof(SqliteConnection)))
    {
    }
}
