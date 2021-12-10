using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes
{
    public class AutoDomainDataWithSqliteContextAttribute : AutoDataAttribute
    {
        public AutoDomainDataWithSqliteContextAttribute()
            : base(() => new Fixture()
                .Customize(new DomainDataWithSqliteContextCustomization()))
        {
        }
    }
}
