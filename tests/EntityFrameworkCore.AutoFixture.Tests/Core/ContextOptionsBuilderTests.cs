using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class ContextOptionsBuilderTests
{
    [Fact]
    public void IsSpecification()
    {
        typeof(ContextOptionsBuilder)
            .Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        _ = new ContextOptionsBuilder();
    }

    [Fact]
    public void CreatesExpectedOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("hello");
        var context = new DelegatingSpecimenContext { OnResolve = _ => optionsBuilder };
        var builder = new ContextOptionsBuilder();
        var result = builder.Create(typeof(DbContextOptions<TestDbContext>), context);

        result.Should().BeSameAs(optionsBuilder.Options);
    }

    [Fact]
    public void ForwardsReceivedResponse()
    {
        var context = new DelegatingSpecimenContext { OnResolve = _ => new NoSpecimen() };
        var builder = new ContextOptionsBuilder();
        var result = builder.Create(typeof(DbContextOptions<TestDbContext>), context);

        result.Should().BeOfType<NoSpecimen>();
    }

    [Theory]
    [AutoData]
    public void GuardsMethods(Fixture fixture, GuardClauseAssertion assertion)
    {
        fixture.Inject<ISpecimenContext>(new DelegatingSpecimenContext());

        assertion.Verify(typeof(ContextOptionsBuilder));
    }

    [Fact]
    public void ReturnsNoSpecimenForInvalidRequest()
    {
        var builder = new ContextOptionsBuilder();

        var actual = builder.Create(new object(), new DelegatingSpecimenContext());

        actual.Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void RequestsCorrectBuilderType()
    {
        object request = null;
        var builder = new ContextOptionsBuilder();
        var context = new DelegatingSpecimenContext
        {
            OnResolve = r =>
            {
                request = r;
                return new DbContextOptionsBuilder<TestDbContext>();
            }
        };

        _ = builder.Create(typeof(DbContextOptions<TestDbContext>), context);

        request.Should().Be(typeof(DbContextOptionsBuilder<TestDbContext>));
    }

    [Fact]
    public void ReturnsExpectedOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("Batman");
        var builder = new ContextOptionsBuilder();
        var context = new DelegatingSpecimenContext { OnResolve = _ => optionsBuilder };

        var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), context);

        actual.Should().Be(optionsBuilder.Options);
    }

    [Theory]
    [InlineData(typeof(NoSpecimen), typeof(NoSpecimen))]
    [InlineData(typeof(PropertyHolder<string>), typeof(NoSpecimen))]
    [InlineData(typeof(OmitSpecimen), typeof(OmitSpecimen))]
    public void ReturnsNoResultForNonBuilderContextResult(Type resultType, Type expected)
    {
        var result = Activator.CreateInstance(resultType);
        var builder = new ContextOptionsBuilder();
        var context = new DelegatingSpecimenContext { OnResolve = _ => result };

        var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), context);

        actual.Should().BeOfType(expected);
    }

    [Theory]
    [InlineData(typeof(PropertyHolder<string>))]
    [InlineData(typeof(PropertyHolder<>))]
    [InlineData(typeof(DbContext))]
    [InlineData("hello")]
    public void ReturnsNoResultForInvalidRequest(object request)
    {
        var builder = new ContextOptionsBuilder();

        var actual = builder.Create(request, new DelegatingSpecimenContext());

        actual.Should().BeOfType<NoSpecimen>();
    }
}
