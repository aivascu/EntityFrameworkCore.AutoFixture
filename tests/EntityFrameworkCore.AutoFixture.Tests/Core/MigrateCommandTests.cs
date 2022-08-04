using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class MigrateCommandTests
{
    [Fact]
    public void IsCommand()
    {
        typeof(MigrateCommand)
            .Should().BeAssignableTo<ISpecimenCommand>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        _ = new MigrateCommand();
    }

    [Fact]
    public void ThrowsWhenRequestNull()
    {
        var command = new MigrateCommand();

        var act = () => command.Execute(default!, new DelegatingSpecimenContext());

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsWhenRequestNotContext()
    {
        var command = new MigrateCommand();

        var act = () => command.Execute(new object(), new DelegatingSpecimenContext());

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void RunsMigrate()
    {
        var command = new MigrateCommand();
        var connection = new SqliteConnection("Data Source=:memory:");
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new TestDbContext(options);
        connection.Open();

        command.Execute(context, default!);

        context.Items.Add(new Item("potato", 1));
        context.SaveChanges();
    }
}
