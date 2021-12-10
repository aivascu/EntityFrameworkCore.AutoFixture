using AutoFixture;
using AutoFixture.AutoMoq;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class DomainDataCustomization : CompositeCustomization
    {
        public DomainDataCustomization()
            : base(new AutoMoqCustomization())
        {
        }
    }
}
