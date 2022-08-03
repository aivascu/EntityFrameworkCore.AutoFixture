using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using EntityFrameworkCore.AutoFixture.Tests.Core;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory;

public class InMemoryOptionsBuilderTests
{
    [Fact]
    public void IsBuilder()
    {
        typeof(InMemoryOptionsBuilder)
            .Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Theory]
    [AutoData]
    public void GuardsConstructor(Fixture fixture, GuardClauseAssertion assertion)
    {
        fixture.Inject<ISpecimenBuilder>(new DelegatingBuilder());

        assertion.Verify(typeof(InMemoryOptionsBuilder).GetConstructors());
    }

    [Theory]
    [AutoData]
    public void InitializesMembers(Fixture fixture, ConstructorInitializedMemberAssertion assertion)
    {
        fixture.Inject<ISpecimenBuilder>(new DelegatingBuilder());

        assertion.Verify(typeof(InMemoryOptionsBuilder));
    }

    [Fact]
    public void ThrowsWhenContextNull()
    {
        var builder = new InMemoryOptionsBuilder(new DelegatingBuilder(), new InMemoryOptions());

        var act = () => _ = builder.Create(new object(), default!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData(typeof(NoSpecimen), typeof(NoSpecimen))]
    [InlineData(typeof(PropertyHolder<string>), typeof(NoSpecimen))]
    [InlineData(typeof(OmitSpecimen), typeof(OmitSpecimen))]
    public void ForwardsResultWhenNotOptionsBuilder(Type type, Type expected)
    {
        var next = new DelegatingBuilder { OnCreate = (_, _) => Activator.CreateInstance(type) };
        var builder = new InMemoryOptionsBuilder(next, new InMemoryOptions());

        var actual = builder.Create(new object(), new DelegatingSpecimenContext());

        actual.Should().BeOfType(expected);
    }

    [Fact]
    public void AddsInMemoryExtension()
    {
        var extensionType = typeof(InMemoryDatabaseFacadeExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension").Single();
        var next = new DelegatingBuilder { OnCreate = (_, _) => new DbContextOptionsBuilder<TestDbContext>() };
        var builder = new InMemoryOptionsBuilder(next, new InMemoryOptions());

        var result = builder.Create(new object(), new DelegatingSpecimenContext());

        var extension = result.As<DbContextOptionsBuilder<TestDbContext>>().Options.Extensions
            .FirstOrDefault(x => ReferenceEquals(x.GetType(), extensionType));
        extension.Should().NotBeNull();
    }

    [Fact]
    public void SetsStoreName()
    {
        var extensionType = typeof(InMemoryDatabaseFacadeExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension").Single();
        var next = new DelegatingBuilder { OnCreate = (_, _) => new DbContextOptionsBuilder<TestDbContext>() };
        var builder = new InMemoryOptionsBuilder(next, new InMemoryOptions { DatabaseName = "TestDb" });

        var result = builder.Create(new object(), new DelegatingSpecimenContext());

        dynamic extension = result.As<DbContextOptionsBuilder<TestDbContext>>().Options.Extensions
            .FirstOrDefault(x => ReferenceEquals(x.GetType(), extensionType));
        string storeName = extension?.StoreName;
        storeName.Should().Be("TestDb");
    }

    [Fact]
    public void SetsStoreNameToUniqueName()
    {
        var extensionType = typeof(InMemoryDatabaseFacadeExtensions).Assembly
            .FindTypesByName("InMemoryOptionsExtension").Single();
        var next = new DelegatingBuilder { OnCreate = (_, _) => new DbContextOptionsBuilder<TestDbContext>() };
        var options = new InMemoryOptions { DatabaseName = "TestDb", UseUniqueNames = true };
        var builder = new InMemoryOptionsBuilder(next, options);

        var result = builder.Create(new object(), new DelegatingSpecimenContext { OnResolve = _ => "UniqueDbName" });

        dynamic extension = result.As<DbContextOptionsBuilder<TestDbContext>>().Options.Extensions
            .FirstOrDefault(x => ReferenceEquals(x.GetType(), extensionType));
        string storeName = extension?.StoreName;
        storeName.Should().Be("UniqueDbName");
    }
}
