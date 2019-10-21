using AutoFixture.AutoEFCore.Common;
using AutoFixture.AutoMoq;

namespace AutoFixture.AutoEFCore.Tests.Common.Customizations
{
    public class DomainDataWithContextCustomization : CompositeCustomization
    {
        public DomainDataWithContextCustomization()
            : base(
                new IgnoredVirtualMembersCustomization(),
                new DbContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}