using System;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class BaseTypeSpecificationTests
{
    [Fact]
    public void IsSpecification()
    {
        typeof(BaseTypeSpecification)
            .Should().BeAssignableTo<IRequestSpecification>();
    }

    [Theory]
    [AutoData]
    public void GuardsConstructors(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(BaseTypeSpecification).GetConstructors());
    }

    [Theory]
    [AutoData]
    public void InitializesMembers(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify(typeof(BaseTypeSpecification));
    }

    [Fact]
    public void ReturnsTrueForSameType()
    {
        var sut = new BaseTypeSpecification(typeof(BaseType));

        var actual = sut.IsSatisfiedBy(typeof(BaseType));

        Assert.True(actual);
    }

    [Fact]
    public void ReturnsTrueForChildType()
    {
        var sut = new BaseTypeSpecification(typeof(BaseType));

        var actual = sut.IsSatisfiedBy(typeof(ChildType));

        Assert.True(actual);
    }

    [Fact]
    public void ThrowsForNullType()
    {
        Action act = () => _ = new BaseTypeSpecification(null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ReturnsFalseWhenRequestNotType()
    {
        var sut = new BaseTypeSpecification(typeof(BaseType));

        var actual = sut.IsSatisfiedBy("string value");

        Assert.False(actual);
    }

    [Fact]
    public void ReturnsFalseWhenRequestNotBaseType()
    {
        var sut = new BaseTypeSpecification(typeof(BaseType));

        var actual = sut.IsSatisfiedBy(typeof(string));

        Assert.False(actual);
    }

    public class BaseType
    {
    }

    public class ChildType : BaseType
    {
    }
}
