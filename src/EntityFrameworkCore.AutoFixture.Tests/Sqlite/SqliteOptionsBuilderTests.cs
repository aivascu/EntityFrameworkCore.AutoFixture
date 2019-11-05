using System;
using AutoFixture;
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
        public void Build_ShouldBuildDbContextOptionsInstance(Fixture fixture)
        {
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                var options = builder.Build(typeof(TestDbContext));

                options.Should().BeOfType<DbContextOptions<TestDbContext>>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNull(Fixture fixture)
        {
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(null);

                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNotDbContext(Fixture fixture)
        {
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(typeof(string));

                action.Should().Throw<ArgumentException>();
            }
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsAbstract(Fixture fixture)
        {
            fixture.Customizations.Add(new SqliteConnectionSpecimenBuilder());

            using (var connection = fixture.Freeze<SqliteConnection>())
            {
                var builder = fixture.Create<SqliteOptionsBuilder>();

                Action action = () => builder.Build(typeof(AbstractDbContext));

                action.Should().Throw<ArgumentException>();
            }
        }

        private abstract class AbstractDbContext : DbContext { }
    }
}
