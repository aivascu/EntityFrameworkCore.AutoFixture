using System;
using AutoFixture;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public class DbContextCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new DbContextSpecimenBuilder());
            fixture.Customizations.Add(new DbContextOptionsSpecimenBuilder());
        }
    }
}
