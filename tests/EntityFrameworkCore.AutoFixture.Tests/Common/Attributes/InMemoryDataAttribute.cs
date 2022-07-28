using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class InMemoryDataAttribute : AutoDataAttribute
    {
        public InMemoryDataAttribute()
            : base(() => new Fixture()
                .Customize(new InMemoryDataCustomization()))
        {
        }
    }
}
