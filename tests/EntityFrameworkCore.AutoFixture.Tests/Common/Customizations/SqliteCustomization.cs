using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class SqliteCustomization : CompositeCustomization
    {
        public SqliteCustomization()
            : base(
                VirtualPropertyOmitterCustomization
                  .ForTypesInNamespaces(typeof(Customer)),
                new SqliteContextCustomization() { OmitDbSets = true },
                new AutoMoqCustomization())
        {
        }
    }
}
