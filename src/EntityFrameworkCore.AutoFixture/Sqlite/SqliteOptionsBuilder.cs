using System;
using EntityFrameworkCore.AutoFixture.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    public class SqliteOptionsBuilder : OptionsBuilder
    {
        public SqliteOptionsBuilder(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public SqliteConnection Connection { get; }

        protected override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseSqlite(Connection)
            .Options;
    }
}
