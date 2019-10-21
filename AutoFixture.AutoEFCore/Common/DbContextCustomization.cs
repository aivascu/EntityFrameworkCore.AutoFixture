namespace AutoFixture.AutoEFCore.Common
{
    public class DbContextCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new DbContextSpecimenBuilder());
            fixture.Customizations.Add(new DbContextOptionsSpecimenBuilder());
        }
    }
}