using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
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

    [Theory]
    [AutoData]
    public void PropertiesSetValues(WritablePropertyAssertion assertion)
    {
        assertion.Verify(typeof(InMemoryCustomization));
    }

    [Fact]
    public void CanInstantiateCustomization()
    {
        var act = () => _ = new InMemoryCustomization();

        act.Should().NotThrow();
    }

    [Fact]
    public void CanInstantiateCustomizationWithCustomConfiguration()
    {
        var act = () => _ = new InMemoryCustomization
        {
            OnCreate = OnCreateAction.Migrate,
            DatabaseName = "Frank",
            UseUniqueNames = true,
            OmitDbSets = true,
            Configure = x => x.EnableSensitiveDataLogging()
        };

        act.Should().NotThrow();
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
        var act = () => _ = new Fixture().Customize(new InMemoryCustomization());

        act.Should().NotThrow();
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
    public void CanCreateContextUsingProvider()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var context = fixture.Create<TestDbContext>();

        using (new AssertionScope())
        {
            context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");
            context.Database.IsInMemory().Should().BeTrue();
        }
    }

    [Fact]
    public void CreatesDifferentConnections()
    {
        var fixture = new Fixture().Customize(new InMemoryCustomization());
        var context1 = fixture.Create<TestDbContext>();
        context1.Customers.Add(fixture.Create<Customer>());
        context1.SaveChanges();
        context1.ChangeTracker.Clear();

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
        context1.ChangeTracker.Clear();

        var context2 = fixture.Create<TestDbContext>();

        using (new AssertionScope())
        {
            context2.Customers.Should().HaveCount(1);
            var actual = context2.Customers.Include(x => x.Orders).Single();
            actual.Should().BeEquivalentTo(customer);
        }
    }
}
