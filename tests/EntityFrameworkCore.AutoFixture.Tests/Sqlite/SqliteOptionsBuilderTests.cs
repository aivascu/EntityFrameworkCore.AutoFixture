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

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite
{
    public class SqliteOptionsBuilderTests
    {
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

            context.Extensions.Should().Contain(x => x.GetType() == GetOptionsType());
        }

        [Fact]
        public void GenericBuild_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            dynamic extension = new SqliteOptionsBuilder(connection)
                .Build<TestDbContext>()
                .Extensions
                .Single(x => x.GetType() == GetOptionsType());

            string actual = extension.Connection.ConnectionString;
            actual.Should().Be(connectionString);
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var context = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>();

            context.Extensions.Should().Contain(x => x.GetType() == GetOptionsType());
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            var connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            dynamic extension = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>()
                .Extensions
                .Single(x => x.GetType() == GetOptionsType());

            string actual = extension.Connection.ConnectionString;
            actual.Should().Be(connectionString);
        }

        private abstract class AbstractDbContext : DbContext
        { }

        private static Type GetOptionsType()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "Microsoft.EntityFrameworkCore.Sqlite");

            var internalExtensionType = assembly.GetType("Microsoft.EntityFrameworkCore.Infrastructure.Internal.SqliteOptionsExtension");
            var extensionType = assembly.GetType("Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal.SqliteOptionsExtension");

            if (extensionType is not null)
                return extensionType;

            if (internalExtensionType is not null)
                return internalExtensionType;

            throw new InvalidOperationException("Unable to find type \"SqliteOptionsExtension\" in the EF Core Sqlite provider assembly");
        }
    }
}
