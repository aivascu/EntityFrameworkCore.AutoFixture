using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Checks whether a parameter's member satisifies the given specification.
/// </summary>
public class ParameterMemberSpecification : IRequestSpecification
{
    /// <summary>
    /// Creates an instance of type <see cref="ParameterMemberSpecification"/>.
    /// </summary>
    /// <param name="specification">The parameter member specification.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="specification"/> is <see langword="null"/>.</exception>
    public ParameterMemberSpecification(IRequestSpecification specification)
    {
        Check.NotNull(specification, nameof(specification));

        this.Specification = specification;
    }

    /// <summary>
    /// Gets the parameter member specification.
    /// </summary>
    public IRequestSpecification Specification { get; }

    /// <inheritdoc />
    public bool IsSatisfiedBy(object request)
    {
        return request is ParameterInfo parameterInfo
               && this.Specification.IsSatisfiedBy(parameterInfo.Member);
    }
}
