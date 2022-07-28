#nullable enable
using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.InMemory;

/// <summary>
/// Configures the DbContextOptionsBuilder to use the in-memory database provider.
/// </summary>
public class InMemoryOptionsBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Creates an instance of type <see cref="InMemoryOptionsBuilder"/>.
    /// </summary>
    /// <param name="builder">The decorated builder.</param>
    /// <param name="options">The configuration options.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public InMemoryOptionsBuilder(ISpecimenBuilder builder, InMemoryOptions options)
    {
        this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        this.Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the decorated builder.
    /// </summary>
    public ISpecimenBuilder Builder { get; }

    /// <summary>
    /// Gets the configuration options.
    /// </summary>
    public InMemoryOptions Options { get; }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var result = this.Builder.Create(request, context);

        var nameSeed = this.Options.DatabaseName ?? Guid.NewGuid().ToString();
        var databaseName = this.Options.UseUniqueNames ? context.Create(nameSeed) : nameSeed;

        if (result is not DbContextOptionsBuilder builder)
            return new NoSpecimen();

        return builder.UseInMemoryDatabase(databaseName, this.Options.ConfigureProvider);
    }
}
