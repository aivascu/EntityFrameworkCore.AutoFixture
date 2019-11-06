using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory
{
    public class InMemoryOptionsBuilderTests
    {
        [Theory]
        [AutoData]
        public void Build_ShouldBuildDbContextOptionsInstance(InMemoryOptionsBuilder builder)
        {
            var options = builder.Build(typeof(TestDbContext));

            options.Should().BeOfType<DbContextOptions<TestDbContext>>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNull(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNotDbContext(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(typeof(string));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsAbstract(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(typeof(AbstractDbContext));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Ctors_ShouldReceiveInitializedParameters(Fixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            var members = typeof(InMemoryOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        private abstract class AbstractDbContext : DbContext { }
    }
}
