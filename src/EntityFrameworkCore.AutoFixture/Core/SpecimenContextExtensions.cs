using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

internal static class SpecimenContextExtensions
{
    public static T Create<T>(this ISpecimenContext source, T seed)
    {
        return (T)source.Resolve(new SeededRequest(typeof(T), seed));
    }
}
