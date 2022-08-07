using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite;

[Obsolete]
public class SqliteContextCustomizationTests
{
    [Fact]
    public void IsCustomization()
    {
        typeof(SqliteContextCustomization)
            .Should().BeAssignableTo<ICustomization>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        _ = new SqliteContextCustomization();
    }

    [Fact]
    public void CanCreateInstanceWithOptions()
    {
        _ = new SqliteContextCustomization { AutoCreateDatabase = true, OmitDbSets = true };
    }

    [Theory]
    [AutoData]
    public void PropertiesSetValues(WritablePropertyAssertion assertion)
    {
        assertion.Verify(typeof(SqliteContextCustomization));
    }

    [Theory]
    [AutoData]
    public void GuardsMethods(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SqliteContextCustomization)
            .GetMethods().Where(x => !x.IsSpecialName));
    }

    [Fact]
    public void CustomizesFixture()
    {
        ICustomization actual = default;
        var fixture = new DelegatingFixture();
        fixture.OnCustomize = x =>
        {
            actual = x;
            return fixture;
        };
        var customization = new SqliteContextCustomization();

        customization.Customize(fixture);

        actual.Should().BeOfType<SqliteCustomization>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConfiguresOmitDbSets(bool omitDbSets)
    {
        ICustomization actual = default;
        var fixture = new DelegatingFixture();
        fixture.OnCustomize = x =>
        {
            actual = x;
            return fixture;
        };
        var customization = new SqliteContextCustomization { OmitDbSets = omitDbSets };

        customization.Customize(fixture);

        actual.As<SqliteCustomization>()
            .OmitDbSets.Should().Be(omitDbSets);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConfiguresAutoOpenConnection(bool autoOpenConnection)
    {
        ICustomization actual = default;
        var fixture = new DelegatingFixture();
        fixture.OnCustomize = x =>
        {
            actual = x;
            return fixture;
        };
        var customization = new SqliteContextCustomization { AutoOpenConnection = autoOpenConnection };

        customization.Customize(fixture);

        actual.As<SqliteCustomization>()
            .AutoOpenConnection.Should().Be(autoOpenConnection);
    }

    [Theory]
    [InlineData(true, OnCreateAction.EnsureCreated)]
    [InlineData(false, OnCreateAction.None)]
    public void ConfiguresOnCreateAction(bool autoCreateDatabase, OnCreateAction action)
    {
        ICustomization actual = default;
        var fixture = new DelegatingFixture();
        fixture.OnCustomize = x =>
        {
            actual = x;
            return fixture;
        };
        var customization = new SqliteContextCustomization { AutoCreateDatabase = autoCreateDatabase };

        customization.Customize(fixture);

        actual.As<SqliteCustomization>()
            .OnCreate.Should().Be(action);
    }
}
