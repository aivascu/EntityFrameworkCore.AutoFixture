using AutoFixture.AutoEFCore.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore.Sqlite
{
    public class SqliteOptionsBuilder : OptionsBuilder
    {
        public SqliteOptionsBuilder(SqliteConnection connection)
        {
            Connection = connection;
        }

        public SqliteConnection Connection { get; }

        protected override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseSqlite(Connection)
            .Options;
    }
}
