using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class DbContextCustomizationTests
{
    [Fact]
    public void IsCustomization()
    {
        typeof(DbContextCustomization)
            .Should().BeAssignableTo<ICustomization>();
    }

    [Theory]
    [AutoData]
    public void ThrowsWhenFixtureNull(DbContextCustomization customization)
    {
        var act = () => customization.Customize(default!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [AutoData]
    public void PropertiesStoreValues(WritablePropertyAssertion assertion)
    {
        assertion.Verify(typeof(DbContextCustomization));
    }

    [Fact]
    public void AddsDbSetOmitterWhenConfigured()
    {
        var customization = new DbContextCustomization();
        var fixture = new DelegatingFixture();
        var expected = new Omitter(
            new AndRequestSpecification(
                new PropertyTypeSpecification(typeof(DbSet<>)),
                new DeclaringTypeSpecification(
                    new BaseTypeSpecification(typeof(DbContext)))));

        customization.Customize(fixture);

        fixture.Customizations.Should().ContainEquivalentOf(expected);
    }

    [Fact]
    public void DoesNotAddDbSetOmitterWhenNotConfigured()
    {
        var customization = new DbContextCustomization { OmitDbSets = false };
        var fixture = new DelegatingFixture();

        customization.Customize(fixture);

        fixture.Customizations
            .Should().NotContain(x => x.GetType() == typeof(Omitter));
    }

    [Theory]
    [InlineData(OnCreateAction.None, typeof(EmptyCommand))]
    [InlineData(OnCreateAction.EnsureCreated, typeof(EnsureCreatedCommand))]
    [InlineData(OnCreateAction.Migrate, typeof(MigrateCommand))]
    public void ConfiguresPostprocessorWithCommand(OnCreateAction action, Type commandType)
    {
        var customization = new DbContextCustomization { OnCreate = action };
        var fixture = new DelegatingFixture();

        customization.Customize(fixture);

        fixture.Customizations
            .Where(x => x is FilteringSpecimenBuilder { Builder: Postprocessor postprocessor }
                        && postprocessor.Command.GetType() == commandType)
            .Should().HaveCount(1);
    }
}
