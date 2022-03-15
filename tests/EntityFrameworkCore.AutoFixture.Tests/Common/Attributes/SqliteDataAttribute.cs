using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class SqliteDataAttribute : AutoDataAttribute
    {
        public SqliteDataAttribute()
            : base(() => new Fixture()
                .Customize(new SqliteCustomization()))
        {
        }
    }
}
