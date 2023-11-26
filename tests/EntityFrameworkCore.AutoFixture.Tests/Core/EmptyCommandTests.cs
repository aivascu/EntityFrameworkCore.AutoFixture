using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class EmptyCommandTests
{
    [Fact]
    public void IsCommand()
    {
        typeof(EmptyCommand)
            .Should().BeAssignableTo<ISpecimenCommand>();
    }

    [Fact]
    public void HasSingleConstructor()
    {
        typeof(EmptyCommand).GetConstructors().Should().HaveCount(1);
    }

    [Fact]
    public void ConstructorIsParameterless()
    {
        typeof(EmptyCommand).GetConstructor([]).Should().NotBeNull();
    }

    [Fact]
    public void HasSingleMember()
    {
        typeof(EmptyCommand).GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x is not ConstructorInfo).Should().HaveCount(1);
    }

    [Fact]
    public void CanCreateInstance()
    {
        var command = new EmptyCommand();

        var act = () => command.Execute(new object(), new DelegatingSpecimenContext());

        act.Should().NotThrow();
    }
}
