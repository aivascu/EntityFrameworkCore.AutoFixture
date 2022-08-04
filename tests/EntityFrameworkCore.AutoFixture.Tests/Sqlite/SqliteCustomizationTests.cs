using System.Data;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
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
        _ = new SqliteCustomization();
    }

    [Fact]
    public void CanInstantiateCustomizationWithCustomConfiguration()
    {
        _ = new SqliteCustomization
        {
            OnCreate = OnCreateAction.Migrate,
            OmitDbSets = true,
            Configure = x => x.EnableSensitiveDataLogging()
        };
    }

    [Fact]
    public void CanCustomizeFixture()
    {
        _ = new Fixture().Customize(new SqliteCustomization());
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
}
