using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory;

[Obsolete]
public class InMemoryContextCustomizationTests
{
    [Fact]
    public void IsCustomization()
    {
        typeof(InMemoryContextCustomization)
            .Should().BeAssignableTo<ICustomization>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        _ = new InMemoryContextCustomization();
    }

    [Fact]
    public void CanCreateInstanceWithOptions()
    {
        _ = new InMemoryContextCustomization { AutoCreateDatabase = true, OmitDbSets = true };
    }

    [Theory]
    [AutoData]
    public void PropertiesSetValues(WritablePropertyAssertion assertion)
    {
        assertion.Verify(typeof(InMemoryContextCustomization));
    }

    [Theory]
    [AutoData]
    public void GuardsMethods(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(InMemoryContextCustomization)
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
        var customization = new InMemoryContextCustomization();

        customization.Customize(fixture);

        actual.Should().BeOfType<InMemoryCustomization>();
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
        var customization = new InMemoryContextCustomization { OmitDbSets = omitDbSets };

        customization.Customize(fixture);

        actual.As<InMemoryCustomization>()
            .OmitDbSets.Should().Be(omitDbSets);
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
        var customization = new InMemoryContextCustomization { AutoCreateDatabase = autoCreateDatabase };

        customization.Customize(fixture);

        actual.As<InMemoryCustomization>()
            .OnCreate.Should().Be(action);
    }
}
