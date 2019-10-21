using AutoFixture.AutoEFCore.Tests.Common.Customizations;
using AutoFixture.Xunit2;

namespace AutoFixture.AutoEFCore.Tests.Common.Attributes
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new DomainDataCustomization()))
        {
        }
    }
}