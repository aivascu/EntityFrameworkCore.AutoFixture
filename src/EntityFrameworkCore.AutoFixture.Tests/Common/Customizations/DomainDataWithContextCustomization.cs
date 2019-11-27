using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.Common;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
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
