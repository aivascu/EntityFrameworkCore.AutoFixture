using AutoFixture.AutoEFCore.InMemory;
using AutoFixture.AutoMoq;

namespace AutoFixture.AutoEFCore.Tests.Common.Customizations
{
    public class DomainDataWithInMemoryContextCustomization : CompositeCustomization
    {
        public DomainDataWithInMemoryContextCustomization()
            : base(
                new IgnoredVirtualMembersCustomization(),
                new InMemoryContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}