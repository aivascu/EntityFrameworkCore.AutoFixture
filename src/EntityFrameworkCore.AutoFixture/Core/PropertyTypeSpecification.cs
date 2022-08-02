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
        this.Specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    public IRequestSpecification Specification { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is PropertyInfo propertyInfo
               && this.Specification.IsSatisfiedBy(propertyInfo.PropertyType);
    }
}
