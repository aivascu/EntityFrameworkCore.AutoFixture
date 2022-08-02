using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class BaseTypeSpecification : IRequestSpecification
{
    public BaseTypeSpecification(Type type)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public Type Type { get; }

    public bool IsSatisfiedBy(object request)
    {
        return request is Type type
               && this.Type.IsAssignableFrom(type);
    }
}
