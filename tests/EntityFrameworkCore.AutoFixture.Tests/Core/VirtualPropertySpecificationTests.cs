using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class VirtualPropertySpecificationTests
    {
        [Fact]
        public void ReurnsTrueForVirtualProperty()
        {
            var sut = new VirtualPropertySpecification();
            var virtualMember = typeof(VirtualPropertyHolder<string>)
                .GetProperty(nameof(VirtualPropertyHolder<string>.Property));

            var actual = sut.IsSatisfiedBy(virtualMember);

            actual.Should().BeTrue();
        }

        [Fact]
        public void ReurnsFalseForNonVirtualProperty()
        {
            var sut = new VirtualPropertySpecification();
            var virtualMember = typeof(PropertyHolder<string>)
                .GetProperty(nameof(PropertyHolder<string>.Property));

            var actual = sut.IsSatisfiedBy(virtualMember);

            actual.Should().BeFalse();
        }
    }
}
