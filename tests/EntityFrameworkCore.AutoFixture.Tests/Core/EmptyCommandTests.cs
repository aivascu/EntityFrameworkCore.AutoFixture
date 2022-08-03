using System;
using System.Linq;
using System.Reflection;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class EmptyCommandTests
{
    [Fact]
    public void HasSingleConstructor()
    {
        typeof(EmptyCommand).GetConstructors().Should().HaveCount(1);
    }

    [Fact]
    public void ConstructorIsParameterless()
    {
        typeof(EmptyCommand).GetConstructor(Array.Empty<Type>()).Should().NotBeNull();
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
        
        command.Execute(new object(), new DelegatingSpecimenContext());
    }
}
