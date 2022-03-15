using System;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class BaseTypeSpecificationTests
    {
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
            Action act = () => _ = new BaseTypeSpecification(null);

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
}
