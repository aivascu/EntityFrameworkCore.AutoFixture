using System;
using System.Data;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class OpenDatabaseConnectionTests
{
    [Fact]
    public void CanCreateInstance()
    {
        _ = new OpenDatabaseConnection();
    }

    [Fact]
    public void OpensDatabaseConnection()
    {
        var command = new OpenDatabaseConnection();
        var connection = new SqliteConnection("Data Source=:memory:");

        command.Execute(connection, default!);

        connection.State.Should().Be(ConnectionState.Open);
    }

    [Fact]
    public void ThrowsWhenSpecimenNull()
    {
        var command = new OpenDatabaseConnection();

        var act = () => command.Execute(null!, default!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsWhenSpecimenNotConnection()
    {
        var command = new OpenDatabaseConnection();

        var act = () => command.Execute(new object(), default!);

        act.Should().ThrowExactly<ArgumentException>();
    }
}
