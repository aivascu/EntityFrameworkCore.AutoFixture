using AutoFixture.AutoEFCore.Common;

namespace AutoFixture.AutoEFCore.Sqlite
{
    public class SqliteContextCustomization : DbContextCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customizations.Add(new SqliteOptionsSpecimenBuilder());
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());
        }
    }
}