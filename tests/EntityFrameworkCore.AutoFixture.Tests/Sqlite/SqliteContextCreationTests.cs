using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Tests.Common.Customizations;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite
{
    public class SqliteContextCreationTests
    {
        [Fact]
        public void CanCreateContext()
        {
            var fixture = new Fixture().Customize(
                new SqliteCustomization());

            Action act = () => _ = fixture.Create<TestDbContext>();

            act.Should().NotThrow();
        }

        [Fact]
        public void DoesNotSetDbSets()
        {
            var fixture = new Fixture().Customize(
                new SqliteCustomization());

            var context = fixture.Create<TestDbContext>();

            using (new AssertionScope())
            {
                context.Items.Should().BeOfType<InternalDbSet<Item>>();
                context.Customers.Should().BeOfType<InternalDbSet<Customer>>();
                context.Orders.Should().BeOfType<InternalDbSet<Order>>();
            }
        }
    }
}
