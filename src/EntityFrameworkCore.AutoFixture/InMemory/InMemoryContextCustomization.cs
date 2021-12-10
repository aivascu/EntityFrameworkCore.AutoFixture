using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    /// <summary>
    /// Customizes AutoFixture to create in memory database contexts.
    /// </summary>
    public class InMemoryContextCustomization : DbContextCustomization
    {
        /// <inheritdoc />
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);
            fixture.Customizations.Add(new InMemoryOptionsSpecimenBuilder());
        }
    }
}
