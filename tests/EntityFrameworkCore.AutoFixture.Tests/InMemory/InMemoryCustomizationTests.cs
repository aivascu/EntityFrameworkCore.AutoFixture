using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory
{
    public class InMemoryCustomizationTests
    {
        [Theory, InMemoryData]
        public async Task CanAddItemToContext(TestDbContext context)
        {
            context.Customers.Add(new Customer("Jane Smith"));
            await context.SaveChangesAsync();

            context.Customers.Should().Contain(x => x.Name == "Jane Smith");
        }

        [Fact]
        public void AddsExpectedCustomizations()
        {
            var actual = new Type[]
            {
                typeof(Omitter),
                typeof(DbContextOptionsSpecimenBuilder),
                typeof(InMemoryOptionsSpecimenBuilder),
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryContextCustomization
            {
                AutoCreateDatabase = true,
                OmitDbSets = true,
            };

            customization.Customize(fixture);

            fixture.Customizations.Select(x => x.GetType())
                .Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void AddsExpectedBehaviors()
        {
            var actual = new Type[]
            {
                typeof(DatabaseInitializingBehavior)
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryContextCustomization
            {
                AutoCreateDatabase = true,
                OmitDbSets = true,
            };

            customization.Customize(fixture);

            fixture.Behaviors.Select(x => x.GetType())
                .Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void DoesNotAddBehaviorsWhenFlagsAreOff()
        {
            var fixture = new DelegatingFixture();
            var customization = new InMemoryContextCustomization
            {
                AutoCreateDatabase = false,
                OmitDbSets = true,
            };

            customization.Customize(fixture);

            fixture.Behaviors.Should().BeEmpty();
        }

        [Fact]
        public void DoesNotAddDbSetOmitterWhenFlagOff()
        {
            var actual = new Type[]
            {
                typeof(DbContextOptionsSpecimenBuilder),
                typeof(InMemoryOptionsSpecimenBuilder),
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryContextCustomization
            {
                OmitDbSets = false,
            };

            customization.Customize(fixture);

            fixture.Customizations.Select(x => x.GetType())
                .Should().BeEquivalentTo(actual);
        }

        [Theory, AutoData]
        public void ImplementsGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(InMemoryContextCustomization));
        }
    }
}
