using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
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
