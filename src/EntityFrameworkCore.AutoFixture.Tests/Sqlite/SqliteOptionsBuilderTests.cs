using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

#if NETCOREAPP2_1
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
#endif

#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
#endif

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite
{
    public class SqliteOptionsBuilderTests
    {
        [Theory]
        [AutoData]
        public void Build_ShouldBuildDbContextOptionsInstance(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                var options = builder.Build(typeof(TestDbContext));

                options.Should().BeOfType<DbContextOptions<TestDbContext>>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNull(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(null);

                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNotDbContext(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(typeof(string));

                action.Should().Throw<ArgumentException>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsAbstract(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(typeof(AbstractDbContext));

                action.Should().Throw<ArgumentException>();
            }
        }

        [Theory]
        [AutoData]
        public void Ctors_ShouldInitializeProperties(ConstructorInitializedMemberAssertion assertion)
        {
            var members = typeof(SqliteOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        [Theory]
        [AutoData]
        public void Ctors_ShouldReceiveInitializedParameters(Fixture fixture, GuardClauseAssertion assertion)
        {
            fixture.Inject(new SqliteConnection("Data Source=:memory:"));
            var members = typeof(SqliteOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        [Fact]
        public void GenericBuild_ShouldCreateDbContextOptions_WithSqliteExtension()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var context = new SqliteOptionsBuilder(connection)
                .Build<TestDbContext>();

            context.Extensions.Should().Contain(x => x.GetType() == typeof(SqliteOptionsExtension));
        }

        [Fact]
        public void GenericBuild_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var extension = new SqliteOptionsBuilder(connection)
                .Build<TestDbContext>()
                .Extensions
                .Single(x => x.GetType() == typeof(SqliteOptionsExtension))
                .As<SqliteOptionsExtension>();

            extension.Connection.ConnectionString.Should().Be(connectionString);
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var context = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>();

            context.Extensions.Should().Contain(x => x.GetType() == typeof(SqliteOptionsExtension));
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var extension = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>()
                .Extensions
                .Single(x => x.GetType() == typeof(SqliteOptionsExtension))
                .As<SqliteOptionsExtension>();

            extension.Connection.ConnectionString.Should().Be(connectionString);
        }

        private abstract class AbstractDbContext : DbContext { }
    }
}
