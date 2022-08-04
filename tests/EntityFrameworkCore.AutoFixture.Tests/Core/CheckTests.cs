using System;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class CheckTests
{
    [Fact]
    public void NotNullThrowsWhenArgumentNull()
    {
        var act = () => Check.NotNull(null!, "param");

        act.Should().ThrowExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("param");
    }

    [Fact]
    public void NotNullDoesNotThrow()
    {
        Check.NotNull(new object(), "param");
    }

    [Fact]
    public void NotEmptyThrowsWhenArgumentNull()
    {
        var act = () => Check.NotEmpty(null!, "param");

        act.Should().ThrowExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("param");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" \r\n\t")]
    public void NotEmptyThrowsWhenArgumentEmpty(string value)
    {
        var act = () => Check.NotEmpty(value, "param");

        act.Should().ThrowExactly<ArgumentException>()
            .And.ParamName.Should().Be("param");
    }

    [Theory]
    [InlineData(".")]
    [InlineData("hello")]
    [InlineData("\tGeneral Kenobi\r\n")]
    public void NotEmptyDoesNotThrow(string value)
    {
        Check.NotEmpty(value, "param");
    }

    [Fact]
    public void IsOfTypeThrowsWhenArgumentNull()
    {
        var act = () => _ = Check.IsOfType<decimal?>(null!, "param");

        act.Should().ThrowExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("param");
    }
    
    [Fact]
    public void IsOfTypeThrowsWhenArgumentNotMatchingType()
    {
        var act = () => _ = Check.IsOfType<decimal?>(292, "param");

        act.Should().ThrowExactly<ArgumentException>()
            .And.ParamName.Should().Be("param");
    }

    [Fact]
    public void IsOfTypeDoesNotThrowForSameType()
    {
        object value = 292;
        var actual = Check.IsOfType<int>(value, "param");

        actual.Should().Be(292);
    }
}
