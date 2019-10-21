using AutoFixture.AutoMoq;

namespace AutoFixture.AutoEFCore.Tests.Common.Customizations
{
    public class DomainDataCustomization : CompositeCustomization
    {
        public DomainDataCustomization()
            : base(new AutoMoqCustomization())
        {
        }
    }
}