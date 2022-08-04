using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Checks whether the request is a member, and member's declaring type satisfies <see cref="Specification"/>.
/// </summary>
public class DeclaringTypeSpecification : IRequestSpecification
{
    /// <summary>
    /// Creates an instance of type <see cref="DeclaringTypeSpecification"/>.
    /// The <see cref="Specification"/> is set to <see cref="ExactTypeSpecification"/>.
    /// </summary>
    /// <param name="type">The type to match the member declaring type.</param>
    public DeclaringTypeSpecification(Type type)
        : this(new ExactTypeSpecification(type))
    {
    }

    /// <summary>
    /// Creates an instance of type <see cref="DeclaringTypeSpecification"/>.
    /// </summary>
    /// <param name="specification">The type to match the member declaring type.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="specification"/> is <see langword="null"/>.
    /// </exception>
    public DeclaringTypeSpecification(IRequestSpecification specification)
    {
        Check.NotNull(specification, nameof(specification));

        this.Specification = specification;
    }

    /// <summary>
    /// Gets the specification matching the member declaring type.
    /// </summary>
    public IRequestSpecification Specification { get; }

    /// <inheritdoc />
    public bool IsSatisfiedBy(object request)
    {
        if (request is not MemberInfo memberInfo)
            return false;

        return this.Specification.IsSatisfiedBy(memberInfo.DeclaringType);
    }
}
