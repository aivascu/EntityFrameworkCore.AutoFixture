using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    public class InMemoryContextCustomization : DbContextCustomization
    {
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);
            fixture.Customizations.Add(new InMemoryOptionsSpecimenBuilder());
        }
    }
}
