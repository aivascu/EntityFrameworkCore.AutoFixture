using System;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class GenericPropertyTypeCriterionTests
    {
        [Fact]
        public void ReturnsTrueForSameGenericType()
        {
            var type = typeof(PropertyHolder<>);
            var sut = new GenericPropertyTypeCriterion(type);

            var property = typeof(PropertyHolder<PropertyHolder<string>>)
                .GetProperty(nameof(PropertyHolder<object>.Property));

            var actual = sut.Equals(property);

            Assert.True(actual);
        }

        [Fact]
        public void ReturnsFalseForDifferentGenericType()
        {
            var type = typeof(PropertyHolder<>);
            var sut = new GenericPropertyTypeCriterion(type);

            var property = typeof(PropertyHolder<GenericType<string>>)
                .GetProperty(nameof(PropertyHolder<object>.Property));

            var actual = sut.Equals(property);

            Assert.False(actual);
        }

        [Fact]
        public void ReturnsFalseForNonGenericType()
        {
            var type = typeof(PropertyHolder<>);
            var sut = new GenericPropertyTypeCriterion(type);

            var property = typeof(PropertyHolder<string>)
                .GetProperty(nameof(PropertyHolder<string>.Property));

            var actual = sut.Equals(property);

            Assert.False(actual);
        }

        [Fact]
        public void ReturnsFalseForNullInputType()
        {
            var type = typeof(PropertyHolder<>);
            var sut = new GenericPropertyTypeCriterion(type);

            var actual = sut.Equals(null);

            Assert.False(actual);
        }

        [Fact]
        public void ThrowsArgumentNullException()
        {
            Action act = () => _ = new GenericPropertyTypeCriterion(null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsArgumentException()
        {
            Action act = () => _ = new GenericPropertyTypeCriterion(typeof(string));

            act.Should().ThrowExactly<ArgumentException>();
        }

        public class GenericType<T>
        {
        }
    }
}
