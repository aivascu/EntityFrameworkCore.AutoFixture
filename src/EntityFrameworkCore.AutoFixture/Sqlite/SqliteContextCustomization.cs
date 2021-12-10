using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    /// <summary>
    /// Customizes AutoFixture to create sqlite Entity Framework database ontexts.
    /// </summary>
    public class SqliteContextCustomization : DbContextCustomization
    {
        /// <inheritdoc />
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);

            fixture.Customizations.Add(new SqliteOptionsSpecimenBuilder());
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());
        }
    }
}
