using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

#if NETCOREAPP2_1
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
#endif

#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
#endif

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

        [Theory]
        [AutoData]
        public void GenericBuild_ShouldCreateDbContextOptions_WithInMemoryExtension(InMemoryOptionsBuilder builder)
        {
            var actual = builder.Build<TestDbContext>();

            actual.Extensions.Should().Contain(x => x.GetType() == typeof(InMemoryOptionsExtension));
        }

        [Theory]
        [AutoData]
        public void GenericBuild_ShouldCreateDbContextOptions_WithInMemoryExtension_WithName(string expected)
        {
            var extension = new InMemoryOptionsBuilder(expected)
                .Build<TestDbContext>()
                .Extensions
                .Single(x => x.GetType() == typeof(InMemoryOptionsExtension))
                .As<InMemoryOptionsExtension>();

#pragma warning disable EF1001 // Internal EF Core API usage.
            extension.StoreName.Should().Be(expected);
#pragma warning restore EF1001 // Internal EF Core API usage.
        }

        [Theory]
        [AutoData]
        public void Build_ShouldCreateDbContextOptions_WithInMemoryExtension(InMemoryOptionsBuilder builder)
        {
            var actual = builder.Build(typeof(TestDbContext)).As<DbContextOptions<TestDbContext>>();

            actual.Extensions.Should().Contain(x => x.GetType() == typeof(InMemoryOptionsExtension));
        }

        [Theory]
        [AutoData]
        public void Build_ShouldCreateDbContextOptions_WithInMemoryExtension_WithName(string expected)
        {
            var extension = new InMemoryOptionsBuilder(expected)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>()
                .Extensions
                .Single(x => x.GetType() == typeof(InMemoryOptionsExtension))
                .As<InMemoryOptionsExtension>();

#pragma warning disable EF1001 // Internal EF Core API usage.
            extension.StoreName.Should().Be(expected);
#pragma warning restore EF1001 // Internal EF Core API usage.
        }

        private abstract class AbstractDbContext : DbContext { }
    }
}
