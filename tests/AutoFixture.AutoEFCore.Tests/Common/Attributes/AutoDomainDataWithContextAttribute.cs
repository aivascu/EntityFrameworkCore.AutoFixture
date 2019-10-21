using AutoFixture.AutoEFCore.Tests.Common.Customizations;
using AutoFixture.Xunit2;

namespace AutoFixture.AutoEFCore.Tests.Common.Attributes
{
    public class AutoDomainDataWithContextAttribute : AutoDataAttribute
    {
        public AutoDomainDataWithContextAttribute()
            : base(() => new Fixture()
                .Customize(new DomainDataWithContextCustomization()))
        {
        }
    }
}