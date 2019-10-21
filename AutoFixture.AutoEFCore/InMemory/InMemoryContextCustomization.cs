using AutoFixture.AutoEFCore.Common;

namespace AutoFixture.AutoEFCore.InMemory
{
    public class InMemoryContextCustomization : DbContextCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customizations.Add(new InMemoryOptionsSpecimenBuilder());
        }
    }
}