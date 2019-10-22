using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
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