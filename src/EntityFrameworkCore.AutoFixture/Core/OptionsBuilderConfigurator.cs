using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// 
/// </summary>
public class OptionsBuilderConfigurator : ISpecimenBuilder
{
    public OptionsBuilderConfigurator(
        ISpecimenBuilder builder, Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? configure = default)
    {
        Check.NotNull(builder, nameof(builder));

        this.Builder = builder;
        this.Configure = configure;
    }

    public ISpecimenBuilder Builder { get; }
    public Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? Configure { get; }

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
