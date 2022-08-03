using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class ParameterMemberSpecification : IRequestSpecification
{
    public ParameterMemberSpecification(IRequestSpecification specification)
    {
        this.Specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    public IRequestSpecification Specification { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is ParameterInfo parameterInfo
               && this.Specification.IsSatisfiedBy(parameterInfo.Member);
    }
}
