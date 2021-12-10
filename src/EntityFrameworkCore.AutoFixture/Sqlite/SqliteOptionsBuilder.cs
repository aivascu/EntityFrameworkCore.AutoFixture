using System;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    /// <summary>
    /// Creates <see cref="DbContextOptions{TContext}"/> using SQLite.
    /// </summary>
    public class SqliteOptionsBuilder : OptionsBuilder
    {
        public SqliteOptionsBuilder(SqliteConnection connection)
        {
            this.Connection = connection
                ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public SqliteConnection Connection { get; }

        /// <summary>
        /// Builds default <see cref="DbContextOptions{TContext}"/>,
        /// using the SQLite connection.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns>Returns a <see cref="DbContextOptions{TContext}" /> instance.</returns>
        public override DbContextOptions<TContext> Build<TContext>()
            => new DbContextOptionsBuilder<TContext>()
            .UseSqlite(this.Connection)
            .Options;
    }
}
