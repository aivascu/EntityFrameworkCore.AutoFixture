using System.Data;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite;

public class SqliteConnectionCreationTests
{
    [Theory]
    [InlineData(true, ConnectionState.Open)]
    [InlineData(false, ConnectionState.Closed)]
    public void ConfigurationControlsConnectionState(bool autoOpen, ConnectionState expected)
    {
        var customization = new SqliteCustomization { AutoOpenConnection = autoOpen };
        var fixture = new Fixture().Customize(customization);

        var connection = fixture.Create<SqliteConnection>();

        connection.State.Should().Be(expected);
    }

    [Fact]
    public void UsesConfiguredConnectionString()
    {
        const string ConnectionString = "Data Source=:memory:;Mode=Memory;Cache=Shared;";
        var customization = new SqliteCustomization { ConnectionString = ConnectionString };
        var fixture = new Fixture().Customize(customization);
        var context = fixture.Create<TestDbContext>();

        var actual = context.Database.GetDbConnection();
        actual.ConnectionString.Should().Be(ConnectionString);
    }
}
