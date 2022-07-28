#nullable enable

using System;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    /// <summary>
    /// Creates a <see cref="DbContextOptionsBuilder{TContext}"/> instance using SQLite.
    /// </summary>
    public class SqliteOptionsBuilder : ISpecimenBuilder
    {
        public SqliteOptionsBuilder(ISpecimenBuilder builder, Action<SqliteDbContextOptionsBuilder>? configureProvider = null)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.ConfigureProvider = configureProvider;
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets or sets an optional action to allow additional provider specific configuration.
        /// </summary>
        public Action<SqliteDbContextOptionsBuilder>? ConfigureProvider { get; set; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            var result = this.Builder.Create(request, context);

            var connection = context.Create<SqliteConnection>();

            if (result is not DbContextOptionsBuilder builder)
                return new NoSpecimen();

            return builder.UseSqlite(connection, this.ConfigureProvider);
        }
    }
}
