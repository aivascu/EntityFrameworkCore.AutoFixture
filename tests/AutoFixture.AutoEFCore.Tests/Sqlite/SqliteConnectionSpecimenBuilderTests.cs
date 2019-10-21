using System;
using AutoFixture.AutoEFCore.Sqlite;
using AutoFixture.AutoEFCore.Tests.Common.Attributes;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests.Sqlite
{
    public class SqliteConnectionSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrowArgumentException_WhenSpecimenContextIsNull(
            SqliteConnectionSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(SqliteConnection), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var noSpecimen =  builder.Create(typeof(string), contextMock.Object);

            noSpecimen.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnSqliteConnectionInstance_WhenRequestTypeIsSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var noSpecimen = builder.Create(typeof(SqliteConnection), contextMock.Object);

            noSpecimen.Should().BeOfType<SqliteConnection>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldCreateSqliteConnection_WithConnectionString_WhenRequestTypeIsSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var connection = builder.Create(typeof(SqliteConnection), contextMock.Object);

            connection.As<SqliteConnection>().ConnectionString.Should().Be("DataSource=:memory:");
        }
    }
}