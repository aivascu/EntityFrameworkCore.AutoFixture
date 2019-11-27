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
            this.Connection = connection
                ?? throw new ArgumentNullException(nameof(connection));
        }

        public SqliteConnection Connection { get; }

        public override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseSqlite(this.Connection)
            .Options;
    }
}
