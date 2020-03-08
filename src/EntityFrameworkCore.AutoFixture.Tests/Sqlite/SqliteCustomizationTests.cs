using System;
using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite
{
    public class SqliteCustomizationTests
    {
        [Theory]
        [AutoDomainDataWithSqliteContext]
        public void Customize_ShouldProvideSqliteContext(
            [Frozen] SqliteConnection connection,
            [Greedy] TestDbContext context,
            Item item, Customer customer)
        {
            using (connection)
            using (context)
            {
                connection.Open();
                context.Database.EnsureCreated();
                context.Items.Add(item);

                context.Customers.Add(customer);
                context.SaveChanges();

                customer.Order(item, 5);
                context.SaveChanges();

                context.Orders.Should().Contain(x => x.CustomerId == customer.Id && x.ItemId == item.Id);
            }
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(Fixture fixture, SqliteContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should().ContainSingle(x => x.GetType() == typeof(SqliteOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddConnectionBuilderToFixture(Fixture fixture, SqliteContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should().ContainSingle(x => x.GetType() == typeof(SqliteConnectionSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(SqliteContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
