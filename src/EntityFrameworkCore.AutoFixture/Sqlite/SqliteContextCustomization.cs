using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    public class SqliteContextCustomization : DbContextCustomization
    {
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);

            fixture.Customizations.Add(new SqliteOptionsSpecimenBuilder());
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());
        }
    }
}
