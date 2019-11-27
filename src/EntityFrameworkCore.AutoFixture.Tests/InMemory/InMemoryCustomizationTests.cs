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
        [Theory]
        [AutoData]
        public void SaveChanges_ShouldCreateCustomerRecord(Fixture fixture, InMemoryContextCustomization customization)
        {
            fixture.Customize(customization);
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
        public void Customize_ShouldAddOptionsBuilderToFixture(Fixture fixture, InMemoryContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should().ContainSingle(x => x.GetType() == typeof(InMemoryOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(InMemoryContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
