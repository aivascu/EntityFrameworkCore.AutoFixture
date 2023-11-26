using System;
using System.Linq;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class OptionsBuilderConfiguratorTests
{
    [Fact]
    public void IsBuilder()
    {
        typeof(OptionsBuilderConfigurator)
            .Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        var act = () => _ = new OptionsBuilderConfigurator(new DelegatingBuilder(), x => x);

        act.Should().NotThrow();
    }

    [Fact]
    public void ThrowsWhenBuilderNull()
    {
        var act = () => _ = new OptionsBuilderConfigurator(null!, x => x);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void PropertiesSetInConstructor()
    {
        var next = new DelegatingBuilder();
        var configure = (DbContextOptionsBuilder x) => x;
        var builder = new OptionsBuilderConfigurator(next, configure);

        using (new AssertionScope())
        {
            builder.Builder.Should().BeSameAs(next);
            builder.Configure.Should().BeSameAs(configure);
        }
    }

    [Fact]
    public void ForwardsResultIfNotBuilder()
    {
        var expected = new object();
        var next = new DelegatingBuilder { OnCreate = (_, _) => expected };
        var builder = new OptionsBuilderConfigurator(next, builder => builder);

        var actual = builder.Create(new object(), null!);

        actual.Should().BeSameAs(expected);
    }

    [Fact]
    public void ForwardsResultWhenConfigureNull()
    {
        var expected = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDb");
        var next = new DelegatingBuilder { OnCreate = (_, _) => expected };
        var builder = new OptionsBuilderConfigurator(next);

        var actual = builder.Create(new object(), null!);

        actual.Should().BeSameAs(expected);
    }

    [Fact]
    public void ReturnsConfiguredBuilder()
    {
        var extensionType = typeof(InMemoryDbContextOptionsExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension")
            .FirstOrDefault();
        var configure = (DbContextOptionsBuilder x) => x
            .UseInMemoryDatabase("TestDb");
        var next = new DelegatingBuilder { OnCreate = (_, _) => new DbContextOptionsBuilder<TestDbContext>() };
        var builder = new OptionsBuilderConfigurator(next, configure);

        var actual = (DbContextOptionsBuilder<TestDbContext>)builder.Create(new object(), null!);

        actual.Options.Extensions.Should().Contain(x => x.GetType() == extensionType);
    }
}
