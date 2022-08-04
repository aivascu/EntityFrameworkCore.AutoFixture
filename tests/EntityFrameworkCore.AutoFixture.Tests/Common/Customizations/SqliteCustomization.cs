using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;

public class SqliteDataCustomization : CompositeCustomization
{
    public SqliteDataCustomization()
        : base(
            VirtualPropertyOmitterCustomization
                .ForTypesInNamespaces(typeof(Customer)),
            new SqliteCustomization(),
            new AutoMoqCustomization())
    {
    }
}
