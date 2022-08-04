using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class PropertyTypeSpecification : IRequestSpecification
{
    public PropertyTypeSpecification(Type type)
        : this(new ExactTypeSpecification(type))
    {
    }

    public PropertyTypeSpecification(IRequestSpecification specification)
    {
        Check.NotNull(specification, nameof(specification));

        this.Specification = specification;
    }

    public IRequestSpecification Specification { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is PropertyInfo propertyInfo
               && this.Specification.IsSatisfiedBy(propertyInfo.PropertyType);
    }
}
