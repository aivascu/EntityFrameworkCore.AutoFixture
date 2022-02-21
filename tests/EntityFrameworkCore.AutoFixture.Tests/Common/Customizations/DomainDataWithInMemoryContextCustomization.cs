using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class DomainDataWithInMemoryContextCustomization : CompositeCustomization
    {
        public DomainDataWithInMemoryContextCustomization()
            : base(
                VirtualPropertyOmitterCustomization
                  .ForTypesInNamespaces(typeof(Customer)),
                new InMemoryContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
