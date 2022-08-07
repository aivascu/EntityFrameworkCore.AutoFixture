using System;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

/// <summary>
/// Creates a <see cref="DbContextOptionsBuilder{TContext}" /> instance using SQLite.
/// </summary>
public class SqliteOptionsBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Creates an instance of type <see cref="SqliteOptionsBuilder" />.
    /// </summary>
    /// <param name="builder">The decorated builder.</param>
    /// <param name="configureProvider">Optional action allowing additional provider specific configuration.</param>
    public SqliteOptionsBuilder(
        ISpecimenBuilder builder, Action<SqliteDbContextOptionsBuilder>? configureProvider = default)
    {
        Check.NotNull(builder, nameof(builder));

        this.Builder = builder;
        this.ConfigureProvider = configureProvider;
    }

    /// <summary>
    /// Gets the decorated builder.
    /// </summary>
    public ISpecimenBuilder Builder { get; }

    /// <summary>
    /// Gets or sets an optional action allowing additional provider specific configuration.
    /// </summary>
    public Action<SqliteDbContextOptionsBuilder>? ConfigureProvider { get; set; }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        Check.NotNull(context, nameof(context));

        var result = this.Builder.Create(request, context);

        var connection = context.Create<SqliteConnection>();

        if (result is not DbContextOptionsBuilder builder)
            return new NoSpecimen();

        return builder.UseSqlite(connection, this.ConfigureProvider);
    }
}
