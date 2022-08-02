using System;
using System.Linq;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class DeclaringTypeSpecificationTests
{
    [Fact]
    public void ThrowsWhenTypeSpecificationNull()
    {
        Action act = () => _ = new DeclaringTypeSpecification(default(IRequestSpecification)!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ReturnsTrueForMatchingType()
    {
        var sut = new DeclaringTypeSpecification(
            new TrueRequestSpecification());
        var property = typeof(PropertyHolder<string>)
            .GetProperties().Single();

        var actual = sut.IsSatisfiedBy(property);

        actual.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseForNonMatchingType()
    {
        var sut = new DeclaringTypeSpecification(
            new FalseRequestSpecification());
        var property = typeof(PropertyHolder<string>)
            .GetProperties().Single();

        var actual = sut.IsSatisfiedBy(property);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData("some value")]
    [InlineData(50)]
    [InlineData(2333.23d)]
    [InlineData(true)]
    public void ReturnsFalseForNonMemberRequests(object request)
    {
        var sut = new DeclaringTypeSpecification(
            new TrueRequestSpecification());

        var actual = sut.IsSatisfiedBy(request);

        actual.Should().BeFalse();
    }
}
