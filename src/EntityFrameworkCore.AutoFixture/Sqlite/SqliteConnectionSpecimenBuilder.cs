using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

/// <summary>
/// Creates <see cref="SqliteConnection" /> instances.
/// </summary>
public class SqliteConnectionSpecimenBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Creates an instance of type <see cref="SqliteConnectionSpecimenBuilder"/>.
    /// </summary>
    /// <param name="connectionSpecification">
    /// The specification on which to return the <see cref="SqliteConnection"/> instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="connectionSpecification"/> is null.
    /// </exception>
    public SqliteConnectionSpecimenBuilder(IRequestSpecification connectionSpecification)
    {
        this.ConnectionSpecification = connectionSpecification
            ?? throw new ArgumentNullException(nameof(connectionSpecification));
    }

    /// <summary>
    /// Creates an instance of type <see cref="SqliteConnectionSpecimenBuilder"/>.
    /// </summary>
    public SqliteConnectionSpecimenBuilder()
        : this(new ExactTypeSpecification(typeof(SqliteConnection)))
    {
    }

    /// <summary>
    /// Gets the specification on which to return the <see cref="SqliteConnection"/> instance.
    /// </summary>
    public IRequestSpecification ConnectionSpecification { get; }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (!this.ConnectionSpecification.IsSatisfiedBy(request)) return new NoSpecimen();

        return new SqliteConnection("DataSource=:memory:");
    }
}
