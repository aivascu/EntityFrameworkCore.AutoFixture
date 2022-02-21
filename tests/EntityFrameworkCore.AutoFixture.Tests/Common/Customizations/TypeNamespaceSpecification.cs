using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class TypeNamespaceSpecification : IRequestSpecification
    {
        public TypeNamespaceSpecification(string @namespace)
        {
            this.Namespace = @namespace;
        }

        public string Namespace { get; }

        public bool IsSatisfiedBy(object request)
        {
            return request is Type type
                && this.Namespace == type.Namespace;
        }
    }
}
