using AutoFixture.AutoEFCore.Tests.Common.Attributes;
using AutoFixture.AutoEFCore.Tests.Common.Persistence;
using AutoFixture.AutoEFCore.Tests.Common.Persistence.Entities;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests.Sqlite
{
    public class SqliteCustomizationTests
    {
        [Theory]
        [AutoDomainDataWithSqliteContext]
        public void Customize_ShouldProvideSqliteContext([Frozen] SqliteConnection connection, TestDbContext context, Item item, Customer customer)
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

                context.Database.EnsureDeleted();
            }
        }
    }
}