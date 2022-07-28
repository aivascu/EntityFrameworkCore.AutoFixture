using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class InMemoryDataCustomization : CompositeCustomization
    {
        public InMemoryDataCustomization()
            : base(
                VirtualPropertyOmitterCustomization
                  .ForTypesInNamespaces(typeof(Customer)),
                new InMemoryCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
