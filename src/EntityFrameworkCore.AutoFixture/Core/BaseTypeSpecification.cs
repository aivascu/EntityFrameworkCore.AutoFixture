using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Checks whether a type is assignable to the base type.
/// </summary>
public class BaseTypeSpecification : IRequestSpecification
{
    /// <summary>
    /// Creates an instance of type <see cref="BaseTypeSpecification"/>.
    /// </summary>
    /// <param name="type">The base type.</param>
    /// <exception cref="ArgumentNullException">Thrown when <param name="type" /> is <see langword="null"/>.</exception>
    public BaseTypeSpecification(Type type)
    {
        Check.NotNull(type, nameof(type));

        this.Type = type;
    }

    /// <summary>
    /// Gets the base type.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Checks if the request is a <see cref="Type"/> and it is assignable to the base type.
    /// </summary>
    /// <param name="request">The request to be validated.</param>
    /// <returns>
    /// When the request is a type assignable to te base type, returns <see langword="true"/>.
    /// Otherwise returns <see langword="false"/>
    /// </returns>
    public bool IsSatisfiedBy(object request)
    {
        return request is Type type
               && this.Type.IsAssignableFrom(type);
    }
}
