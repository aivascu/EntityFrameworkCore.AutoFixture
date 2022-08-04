using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class ParameterMemberSpecification : IRequestSpecification
{
    public ParameterMemberSpecification(IRequestSpecification specification)
    {
        Check.NotNull(specification, nameof(specification));

        this.Specification = specification;
    }

    public IRequestSpecification Specification { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is ParameterInfo parameterInfo
               && this.Specification.IsSatisfiedBy(parameterInfo.Member);
    }
}
