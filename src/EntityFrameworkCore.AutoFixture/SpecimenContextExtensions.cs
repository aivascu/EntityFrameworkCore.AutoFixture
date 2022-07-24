#nullable enable
using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture;

internal static class SpecimenContextExtensions
{
    public static T Create<T>(this ISpecimenContext source, T seed)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return (T)source.Resolve(new SeededRequest(typeof(T), seed));
    }
}
