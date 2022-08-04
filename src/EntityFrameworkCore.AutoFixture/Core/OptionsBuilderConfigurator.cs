using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Configures a <see cref="DbContextOptionsBuilder"/> instance provided by the decorated builer.
/// </summary>
public class OptionsBuilderConfigurator : ISpecimenBuilder
{
    /// <summary>
    /// Creates an instance of type <see cref="OptionsBuilderConfigurator"/>.
    /// </summary>
    /// <param name="builder">The decorated builder.</param>
    /// <param name="configure">The delegate to configure the <see cref="DbContextOptionsBuilder"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    public OptionsBuilderConfigurator(
        ISpecimenBuilder builder, Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? configure = default)
    {
        Check.NotNull(builder, nameof(builder));

        this.Builder = builder;
        this.Configure = configure;
    }

    /// <summary>
    /// Gets the decorated builder.
    /// </summary>
    public ISpecimenBuilder Builder { get; }

    /// <summary>
    /// Gets the delegate that configures the <see cref="DbContextOptionsBuilder"/> instance.
    /// </summary>
    public Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? Configure { get; }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        var result = this.Builder.Create(request, context);

        if (result is not DbContextOptionsBuilder optionsBuilder)
            return result;

        if (this.Configure is null)
            return optionsBuilder;

        return this.Configure(optionsBuilder);
    }
}
