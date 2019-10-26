using AutoFixture.Xunit2;
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