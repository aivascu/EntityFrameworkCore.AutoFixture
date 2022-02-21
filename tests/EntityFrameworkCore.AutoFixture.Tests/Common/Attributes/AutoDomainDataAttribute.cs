using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
