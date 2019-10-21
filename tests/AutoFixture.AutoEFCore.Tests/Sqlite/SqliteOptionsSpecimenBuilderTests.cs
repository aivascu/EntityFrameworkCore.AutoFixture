using System;
using AutoFixture.AutoEFCore.Common;
using AutoFixture.AutoEFCore.Sqlite;
using AutoFixture.AutoEFCore.Tests.Common.Attributes;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests.Sqlite
{
    public class SqliteOptionsSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrowArgumentException_WhenSpecimenContextNull(SqliteOptionsSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(IOptionsBuilder), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotDbContextOptions(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            var obj = builder.Create(typeof(string), contextMock.Object);

            obj.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenSpecimenContextCannotResolveConnection(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns(new NoSpecimen());

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnOmitSpecimen_WhenSpecimenContextSkipsConnectionResolve(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns(new OmitSpecimen());

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.Should().BeOfType<OmitSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNull_WhenSpecimenContextResolvesConnectionAsNull(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns(null);

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.Should().BeNull();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenSpecimenContextResolvesConnectionToUnexpectedType(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns("Hello World");

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnSqliteOptionsBuilder_WhenSpecimenContextResolvesConnection(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns(new SqliteConnection("DataSource=:memory:"));

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.Should().BeOfType<SqliteOptionsBuilder>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnSqliteOptionsBuilder_WithInMemoryConnectionString_WhenSpecimenContextResolvesConnection(SqliteOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            contextMock
                .Setup(x => x.Resolve(typeof(SqliteConnection)))
                .Returns(new SqliteConnection("DataSource=:memory:"));

            var obj = builder.Create(typeof(IOptionsBuilder), contextMock.Object);

            obj.As<SqliteOptionsBuilder>().Connection.ConnectionString.Should().Be("DataSource=:memory:");
        }
    }
}