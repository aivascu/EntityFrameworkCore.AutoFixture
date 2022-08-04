using System;
using System.Linq;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite;

public class DbContextCreationTests
{
    [Fact]
    public void CanCreateContext()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());

        Action act = () => _ = fixture.Create<TestDbContext>();

        act.Should().NotThrow();
    }

    [Fact]
    public void CanCreateContextWithCustomArguments()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());

        Action act = () => _ = fixture.Create<TestCustomDbContext>();

        act.Should().NotThrow();
    }

    [Fact]
    public void CanCreateContextWithExpectedProvider()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());
        var context = fixture.Create<TestDbContext>();

        context.Database.IsSqlite().Should().BeTrue();
    }

    [Fact]
    public void DoesNotSetDbSets()
    {
        var fixture = new Fixture().Customize(
            new SqliteDataCustomization());

        var context = fixture.Create<TestDbContext>();

        using (new AssertionScope())
        {
            context.Items.Should().BeOfType<InternalDbSet<Item>>();
            context.Customers.Should().BeOfType<InternalDbSet<Customer>>();
            context.Orders.Should().BeOfType<InternalDbSet<Order>>();
        }
    }

    [Fact]
    public void CreatesDifferentConnections()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());
        var context1 = fixture.Create<TestDbContext>();
        context1.Customers.Add(fixture.Create<Customer>());
        context1.SaveChanges();

        var context2 = fixture.Create<TestDbContext>();

        context2.Customers.Count().Should().Be(0);
    }

    [Fact]
    public void CreatesSameConnections()
    {
        var fixture = new Fixture().Customize(new SqliteCustomization());
        fixture.Freeze<SqliteConnection>();
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
