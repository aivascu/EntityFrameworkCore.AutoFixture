using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Checks whether a property's return type satisifies the given specification.
/// </summary>
public class PropertyTypeSpecification : IRequestSpecification
{
    /// <summary>
    /// Creates an instance of type <see cref="PropertyTypeSpecification"/>.
    /// </summary>
    /// <param name="type">The exact type to match.</param>
    public PropertyTypeSpecification(Type type)
        : this(new ExactTypeSpecification(type))
    {
    }

    /// <summary>
    /// Creates an instance of type <see cref="PropertyTypeSpecification"/>.
    /// </summary>
    /// <param name="specification">The parameter member specification.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="specification"/> is <see langword="null"/>.</exception>
    public PropertyTypeSpecification(IRequestSpecification specification)
    {
        Check.NotNull(specification, nameof(specification));

        this.Specification = specification;
    }

    /// <summary>
    /// Gets the property's return type specification.
    /// </summary>
    public IRequestSpecification Specification { get; }

    /// <inheritdoc />
    public bool IsSatisfiedBy(object request)
    {
        return request is PropertyInfo propertyInfo
               && this.Specification.IsSatisfiedBy(propertyInfo.PropertyType);
    }
}
