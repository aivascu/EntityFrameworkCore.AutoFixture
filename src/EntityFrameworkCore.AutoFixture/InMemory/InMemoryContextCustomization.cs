using System;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    /// <summary>
    /// Customizes AutoFixture to create in memory database contexts.
    /// </summary>
    public class InMemoryContextCustomization : DbContextCustomization
    {
        /// <summary>
        /// Automatically run <see cref="DatabaseFacade.EnsureCreated()"/>, on
        /// <see cref="DbContext.Database"/>, upon creating a database context instance. Default
        /// value is <see langword="false"/>.
        /// </summary>
        public bool AutoCreateDatabase { get; set; }

        /// <inheritdoc />
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);

            fixture.Customizations.Add(new TypeRelay(typeof(IOptionsBuilder), typeof(InMemoryOptionsBuilder)));

            if (this.AutoCreateDatabase)
            {
                fixture.Behaviors.Add(new DatabaseInitializingBehavior(
                    new BaseTypeSpecification(typeof(DbContext))));
            }
        }
    }
}
