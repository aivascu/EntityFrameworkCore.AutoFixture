using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class AutoDomainDataWithInMemoryContextAttribute : AutoDataAttribute
    {
        public AutoDomainDataWithInMemoryContextAttribute()
            : base(() => new Fixture()
                .Customize(new DomainDataWithInMemoryContextCustomization()))
        {
        }
    }
}