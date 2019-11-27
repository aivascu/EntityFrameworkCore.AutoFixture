using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.InMemory;
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
        public void SaveChanges_ShouldCreateCustomerRecord()
        {
            var fixture = new Fixture().Customize(new InMemoryContextCustomization());
            using var context = fixture.Create<TestDbContext>();
            context.Database.EnsureCreated();

            context.Customers.Add(new Customer("John Doe"));
            context.SaveChanges();

            context.Customers.Should().Contain(x => x.Name == "John Doe");
        }

        [Theory]
        [AutoDomainDataWithInMemoryContext]
        public async Task SaveChangesAsync_ShouldCreateCustomerRecord(TestDbContext context)
        {
            using (context)
            {
                await context.Database.EnsureCreatedAsync();

                context.Customers.Add(new Customer("Jane Smith"));
                await context.SaveChangesAsync();

                context.Customers.Should().Contain(x => x.Name == "Jane Smith");
            }
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(Fixture fixture)
        {
            fixture.Customize(new InMemoryContextCustomization());

            fixture.Customizations.Should().ContainSingle(x => x.GetType() == typeof(InMemoryOptionsSpecimenBuilder));
        }

        [Fact]
        public void Customize_ForNullFixture_ShouldThrow()
        {
            var customization = new InMemoryContextCustomization();

            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
