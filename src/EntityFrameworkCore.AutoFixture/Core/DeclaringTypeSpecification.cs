using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class DeclaringTypeSpecification : IRequestSpecification
    {
        public DeclaringTypeSpecification(IRequestSpecification typeSpecification)
        {
            this.TypeSpecification = typeSpecification
                ?? throw new ArgumentNullException(nameof(typeSpecification));
        }

        public IRequestSpecification TypeSpecification { get; }

        public bool IsSatisfiedBy(object request)
        {
            return request switch
            {
                MemberInfo memberInfo => this.TypeSpecification.IsSatisfiedBy(memberInfo.DeclaringType),
                _ => false,
            };
        }
    }
}
