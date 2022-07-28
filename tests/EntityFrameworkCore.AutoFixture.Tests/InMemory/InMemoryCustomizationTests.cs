using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
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
        [Fact]
        public void CanCreateContext()
        {
            var fixture = new Fixture().Customize(
                new InMemoryCustomization
                {
                    DatabaseName = "MyDb",
                    UseUniqueNames = true,
                    Configure = x => x.ConfigureWarnings(y => y.Ignore()),
                    OmitDbSets = true,
                    OnCreate = OnCreateAction.Migrate // By default it is OnCreateAction.EnsureCreated
                });

            var context = fixture.Create<TestDbContext>();
        }

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
            var actual = new[]
            {
                typeof(Omitter),
                typeof(TypeRelay),
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryCustomization();

            customization.Customize(fixture);

            fixture.Customizations.Select(x => x.GetType())
                .Should().BeEquivalentTo<Type>(actual);
        }

        [Fact]
        public void AddsExpectedBehaviors()
        {
            var actual = new[]
            {
                typeof(DatabaseInitializingBehavior)
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryCustomization();

            customization.Customize(fixture);

            fixture.Behaviors.Select(x => x.GetType())
                .Should().BeEquivalentTo<Type>(actual);
        }

        [Fact]
        public void DoesNotAddBehaviorsWhenFlagsAreOff()
        {
            var fixture = new DelegatingFixture();
            var customization = new InMemoryCustomization();

            customization.Customize(fixture);

            fixture.Behaviors.Should().BeEmpty();
        }

        [Fact]
        public void DoesNotAddDbSetOmitterWhenFlagOff()
        {
            var actual = new[]
            {
                // typeof(DbContextOptionsSpecimenBuilder),
                typeof(TypeRelay),
            };
            var fixture = new DelegatingFixture();
            var customization = new InMemoryCustomization
            {
                OmitDbSets = false,
            };

            customization.Customize(fixture);

            fixture.Customizations.Select(x => x.GetType())
                .Should().BeEquivalentTo<Type>(actual);
        }

        [Theory, AutoData]
        public void ImplementsGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(InMemoryCustomization));
        }
    }
}
