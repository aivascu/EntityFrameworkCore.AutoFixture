using System.Threading.Tasks;
using AutoFixture.AutoEFCore.InMemory;
using AutoFixture.AutoEFCore.Tests.Common.Attributes;
using AutoFixture.AutoEFCore.Tests.Common.Persistence;
using AutoFixture.AutoEFCore.Tests.Common.Persistence.Entities;
using FluentAssertions;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests.InMemory
{
    public class InMemoryCustomizationTests
    {
        [Fact]
        public void SaveChanges_ShouldCreateCustomerRecord()
        {
            var fixture = new Fixture().Customize(new InMemoryContextCustomization());
            using var context = fixture.Create<TestDbContext>();
            context.Database.EnsureCreated();

            context.Customers.Add(new Customer("John Doe"));
            context.SaveChanges();

            context.Customers.Should().Contain(x => x.Name == "John Doe");

            context.Database.EnsureDeleted();
        }

        [Theory]
        [AutoDomainDataWithInMemoryContext]
        public async Task SaveChangesAsync_ShouldCreateCustomerRecord(TestDbContext context)
        {
            await using (context)
            {
                await context.Database.EnsureCreatedAsync();

                context.Customers.Add(new Customer("Jane Smith"));
                await context.SaveChangesAsync();

                context.Customers.Should().Contain(x => x.Name == "Jane Smith");

                await context.Database.EnsureDeletedAsync();
            }
        }
    }
}