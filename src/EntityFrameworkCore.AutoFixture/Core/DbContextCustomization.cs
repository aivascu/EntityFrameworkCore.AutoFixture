using System;
using AutoFixture;

namespace EntityFrameworkCore.AutoFixture.Core
{
    /// <summary>
    /// Customizes AutoFixture to resolve database context options from fixture.
    /// </summary>
    public class DbContextCustomization : ICustomization
    {
        /// <inheritdoc />
        public virtual void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(new DbContextOptionsSpecimenBuilder());
        }
    }
}
