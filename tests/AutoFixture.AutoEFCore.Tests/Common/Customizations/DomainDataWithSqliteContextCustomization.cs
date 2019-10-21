using AutoFixture.AutoEFCore.Sqlite;
using AutoFixture.AutoMoq;

namespace AutoFixture.AutoEFCore.Tests.Common.Customizations
{
    public class DomainDataWithSqliteContextCustomization : CompositeCustomization
    {
        public DomainDataWithSqliteContextCustomization()
            : base(
                new IgnoredVirtualMembersCustomization(),
                new SqliteContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}