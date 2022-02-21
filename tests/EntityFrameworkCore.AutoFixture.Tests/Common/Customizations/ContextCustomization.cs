using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class ContextCustomization : CompositeCustomization
    {
        public ContextCustomization()
            : base(
                VirtualPropertyOmitterCustomization
                  .ForTypesInNamespaces(typeof(Customer)),
                new DbContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
