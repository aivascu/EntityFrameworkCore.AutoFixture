using System;
using System.Linq;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory;

public class InMemoryCustomizationTests
{
    [Fact]
    public void IsCustomization()
    {
        typeof(InMemoryCustomization).Should().BeAssignableTo<ICustomization>();
    }

    [Fact]
    public void IsContextCustomization()
    {
        typeof(InMemoryCustomization).Should().BeAssignableTo<DbContextCustomization>();
    }

    [Fact]
    public void CanInstantiateCustomization()
    {
        _ = new InMemoryCustomization();
    }

    [Fact]
    public void CanInstantiateCustomizationWithCustomConfiguration()
    {
        _ = new InMemoryCustomization
        {
            OnCreate = OnCreateAction.Migrate,
            DatabaseName = "Frank",
            UseUniqueNames = true,
            OmitDbSets = true,
            Configure = x => x.EnableSensitiveDataLogging()
        };
    }

    [Fact]
    public void ThrowsWhenFixtureNull()
    {
        var act = () => new InMemoryCustomization().Customize(default!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void CanCustomizeFixture()
    {
        _ = new Fixture().Customize(new InMemoryCustomization());
    }

    [Fact]
    public void CanCreateContextOptionsBuilder()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization());

        var builder = fixture.Create<DbContextOptionsBuilder<TestDbContext>>();

        builder.Should().NotBeNull();
    }

    [Fact]
    public void OptionsBuilderUsesProvider()
    {
        var extensionType = typeof(InMemoryDbContextOptionsExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension")
            .FirstOrDefault();
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var builder = fixture.Create<DbContextOptionsBuilder<TestDbContext>>();

        builder.Options.Extensions.Should().Contain(x => x.GetType() == extensionType);
    }

    [Fact]
    public void CanCreateOptions()
    {
        var extensionType = typeof(InMemoryDbContextOptionsExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension")
            .FirstOrDefault();
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var options = fixture.Create<DbContextOptions<TestDbContext>>();

        options.Extensions.Should().Contain(x => x.GetType() == extensionType);
    }

    [Fact]
    public void CanCreateContext()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var context = fixture.Create<TestDbContext>();

        context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");
    }

    [Fact]
    public void CreatesDifferentConnections()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var context1 = fixture.Create<TestDbContext>();
        context1.Customers.Add(fixture.Create<Customer>());
        context1.SaveChanges();

        var context2 = fixture.Create<TestDbContext>();

        context2.Customers.Count().Should().Be(0);
    }

    [Fact]
    public void CreatesSameConnections()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization { UseUniqueNames = false });
        var context1 = fixture.Create<TestDbContext>();
        var customer = fixture.Create<Customer>();
        context1.Customers.Add(customer);
        context1.SaveChanges();

        var context2 = fixture.Create<TestDbContext>();

        using (new AssertionScope())
        {
            context2.Customers.Should().HaveCount(1);
            var actual = context2.Customers.Include(x => x.Orders).Single();
            actual.Should().BeEquivalentTo(customer);
        }
    }
}
