using AutoFixture.AutoEFCore.Tests.Common.Customizations;
using AutoFixture.Xunit2;

namespace AutoFixture.AutoEFCore.Tests.Common.Attributes
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