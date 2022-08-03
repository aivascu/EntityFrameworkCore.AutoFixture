using AutoFixture.Kernel;
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
        var result = builder.Create(typeof(DbContextOptionsBuilder<TestDbContext>), context);

        result.Should().BeSameAs(optionsBuilder.Options);
    }
    
    [Fact]
    public void ForwardsReceivedResponse()
    {
        var context = new DelegatingSpecimenContext { OnResolve = _ => new NoSpecimen() };
        var builder = new ContextOptionsBuilder();
        var result = builder.Create(typeof(DbContextOptionsBuilder<TestDbContext>), context);

        result.Should().BeOfType<NoSpecimen>();
    }
}
