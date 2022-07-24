using System;
using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite
{
    public class SqliteCustomizationTests
    {
        [Theory, SqliteData]
        public void CanUseResolvedContextInstance(
            TestDbContext context,
            Item item, Customer customer)
        {
            context.Items.Add(item);

            context.Customers.Add(customer);
            context.SaveChanges();

            customer.Order(item, 5);
            context.SaveChanges();

            context.Orders.Should()
                .Contain(x => x.CustomerId == customer.Id && x.ItemId == item.Id);
        }

        [Fact]
        public void AddsExpectedCustomizations()
        {
            var actual = new[]
            {
                typeof(Omitter),
                typeof(DbContextOptionsSpecimenBuilder),
                typeof(SqliteOptionsSpecimenBuilder),
                typeof(SqliteConnectionSpecimenBuilder)
            };
            var fixture = new DelegatingFixture();
            var customization = new SqliteContextCustomization
            {
                AutoCreateDatabase = true,
                AutoOpenConnection = true,
                OmitDbSets = true,
            };

            customization.Customize(fixture);

            fixture.Customizations.Select(x => x.GetType())
                .Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void AddsExpectedBehaviors()
        {
            var actual = new[]
            {
                typeof(ConnectionOpeningBehavior),
                typeof(DatabaseInitializingBehavior)
            };
            var fixture = new DelegatingFixture();
            var customization = new SqliteContextCustomization
            {
                AutoCreateDatabase = true,
                AutoOpenConnection = true,
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
            var customization = new SqliteContextCustomization
            {
                AutoCreateDatabase = false,
                AutoOpenConnection = false,
                OmitDbSets = true,
            };

            customization.Customize(fixture);

            fixture.Behaviors.Should().BeEmpty();
        }

        [Fact]
        public void DoesNotAddDbSetOmitterWhenFlagOff()
        {
            var actual = new[]
            {
                typeof(DbContextOptionsSpecimenBuilder),
                typeof(SqliteOptionsSpecimenBuilder),
                typeof(SqliteConnectionSpecimenBuilder)
            };
            var fixture = new DelegatingFixture();
            var customization = new SqliteContextCustomization
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
            assertion.Verify(typeof(SqliteContextCustomization));
        }
    }
}
