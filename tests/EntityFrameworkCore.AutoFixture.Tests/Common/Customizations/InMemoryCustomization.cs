using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class InMemoryCustomization : CompositeCustomization
    {
        public InMemoryCustomization()
            : base(
                VirtualPropertyOmitterCustomization
                  .ForTypesInNamespaces(typeof(Customer)),
                new InMemoryContextCustomization
                {
                    AutoCreateDatabase = true,
                    OmitDbSets = true
                },
                new AutoMoqCustomization())
        {
        }
    }
}
