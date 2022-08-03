using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class VirtualPropertySpecificationTests
{
    [Fact]
    public void IsSpecification()
    {
        typeof(VirtualPropertySpecification)
            .Should().BeAssignableTo<IRequestSpecification>();
    }

    [Fact]
    public void ReturnsTrueForVirtualProperty()
    {
        var sut = new VirtualPropertySpecification();
        var member = typeof(VirtualPropertyHolder<string>)
            .GetProperty(nameof(VirtualPropertyHolder<string>.Property));

        var actual = sut.IsSatisfiedBy(member!);

        actual.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseForNonVirtualProperty()
    {
        var sut = new VirtualPropertySpecification();
        var member = typeof(PropertyHolder<string>)
            .GetProperty(nameof(PropertyHolder<string>.Property));

        var actual = sut.IsSatisfiedBy(member!);

        actual.Should().BeFalse();
    }
}
