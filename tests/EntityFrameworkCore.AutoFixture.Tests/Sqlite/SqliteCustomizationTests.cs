using System.Data;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite;

public class SqliteCustomizationTests
{
    [Fact]
    public void IsCustomization()
    {
        typeof(SqliteCustomization).Should().BeAssignableTo<ICustomization>();
    }

    [Fact]
    public void IsContextCustomization()
    {
        typeof(SqliteCustomization).Should().BeAssignableTo<DbContextCustomization>();
    }

    [Theory]
    [AutoData]
    public void PropertiesSetValues(WritablePropertyAssertion assertion)
    {
        assertion.Verify(typeof(SqliteCustomization));
    }

    [Theory]
    [AutoData]
    public void GuardsMethods(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SqliteCustomization)
            .GetMethods().Where(x => !x.IsSpecialName));
    }

    [Fact]
    public void CanInstantiateCustomization()
    {
        var act = () => _ = new SqliteCustomization();

        act.Should().NotThrow();
    }

    [Fact]
    public void CanInstantiateCustomizationWithCustomConfiguration()
    {
        var act = () => _ = new SqliteCustomization
        {
            OnCreate = OnCreateAction.Migrate,
            OmitDbSets = true,
            Configure = x => x.EnableSensitiveDataLogging()
        };

        act.Should().NotThrow();
    }

    [Fact]
    public void CanCustomizeFixture()
    {
        var act = () => _ = new Fixture().Customize(new SqliteCustomization());

        act.Should().NotThrow();
    }

    [Fact]
    public void CanCreateContextOptionsBuilder()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());

        var builder = fixture.Create<DbContextOptionsBuilder<TestDbContext>>();

        builder.Should().NotBeNull();
    }

    [Fact]
    public void OptionsBuilderUsesProvider()
    {
        var extensionType = typeof(SqliteDbContextOptionsBuilderExtensions).Assembly
            .FindTypesByName("SqliteOptionsExtension")
            .FirstOrDefault();
        var fixture = new Fixture().Customize(new SqliteCustomization());
        var builder = fixture.Create<DbContextOptionsBuilder<TestDbContext>>();

        builder.Options.Extensions.Should().Contain(x => x.GetType() == extensionType);
    }

    [Fact]
    public void CanCreateOptions()
    {
        var extensionType = typeof(SqliteDbContextOptionsBuilderExtensions).Assembly
            .FindTypesByName("SqliteOptionsExtension")
            .FirstOrDefault();
        var fixture = new Fixture().Customize(new SqliteCustomization());
        var options = fixture.Create<DbContextOptions<TestDbContext>>();

        options.Extensions.Should().Contain(x => x.GetType() == extensionType);
    }

    [Fact]
    public void CreatesDifferentConnections()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());
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
        var fixture = new Fixture().Customize(new SqliteCustomization());
        var context1 = fixture.Freeze<TestDbContext>();
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
