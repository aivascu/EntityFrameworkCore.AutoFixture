using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class DbContextDataAttribute : AutoDataAttribute
    {
        public DbContextDataAttribute()
            : base(() => new Fixture()
                .Customize(new ContextCustomization()))
        {
        }
    }
}
