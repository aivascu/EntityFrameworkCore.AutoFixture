using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Creates <see cref="DbContextOptions{TContext}" /> instances.
/// </summary>
public class ContextOptionsBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Creates a <see cref="DbContextOptions{TContext}" /> instance matching requested type
    /// in <paramref name="request" />.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="context">The specimen context.</param>
    /// <returns>
    /// Returns <see cref="DbContextOptions{TContext}" /> instance when activation succeeds.<br />
    /// Returns <see cref="NoSpecimen" /> when received invalid request or unable to get a
    /// <see cref="DbContextOptionsBuilder{TContext}" /> instance.<br />
    /// Returns <see cref="OmitSpecimen" /> when <see cref="DbContextOptionsBuilder{TContext}" /> is omitted.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(request, nameof(request));

        var contextType = GetContextType(request);
        if (contextType is null)
            return new NoSpecimen();

        var builderType = typeof(DbContextOptionsBuilder<>).MakeGenericType(contextType);
        var result = context.Resolve(builderType);

        if (result is not DbContextOptionsBuilder builder)
            return result is NoSpecimen or OmitSpecimen ? result : new NoSpecimen();

        return builder.Options;
    }

    private static Type? GetContextType(object request)
    {
        if (request is not Type type)
            return null;

        if (!type.IsGenericType || type.IsGenericTypeDefinition)
            return null;

        if (type.GetGenericTypeDefinition() != typeof(DbContextOptions<>))
            return null;

        return type.GenericTypeArguments[0];
    }
}
