using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class BaseTypeSpecification : IRequestSpecification
    {
        public BaseTypeSpecification(Type baseType)
        {
            this.BaseType = baseType
                ?? throw new ArgumentNullException(nameof(baseType));
        }

        public Type BaseType { get; }

        public bool IsSatisfiedBy(object request)
        {
            return request is Type type
                && this.BaseType.IsAssignableFrom(type);
        }
    }
}
