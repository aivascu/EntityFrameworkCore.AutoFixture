using AutoFixture.AutoEFCore.Tests.Common.Customizations;
using AutoFixture.Xunit2;

namespace AutoFixture.AutoEFCore.Tests.Common.Attributes
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