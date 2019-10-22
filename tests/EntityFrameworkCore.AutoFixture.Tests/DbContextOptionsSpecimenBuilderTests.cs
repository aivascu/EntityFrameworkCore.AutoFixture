using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Common;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests
{
    public class DbContextOptionsSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrowArgumentException_WhenSpecimenContextIsNull(DbContextOptionsSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(DbContextOptions<TestDbContext>), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotDbContextOptions(DbContextOptionsSpecimenBuilder builder, Mock<ISpecimenContext> contextMock)
        {
            var actual = builder.Create(typeof(string), contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestIsPropertyInfo(
            DbContextOptionsSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            var property = typeof(string).GetProperty(nameof(string.Length));
            var actual = builder.Create(property, contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenContextCanNotResolveOptionsBuilder(
            DbContextOptionsSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(IOptionsBuilder)))
                       .Returns(new NoSpecimen());

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenContextResolvesOptionsBuilderAsDifferentType(
            DbContextOptionsSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(IOptionsBuilder)))
                       .Returns("Hello World");

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnOmitSpecimen_WhenContextSkipsOptionsBuilderResolution(
            DbContextOptionsSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(IOptionsBuilder)))
                       .Returns(new OmitSpecimen());

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().BeOfType<OmitSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNull_WhenContextResolvesOptionsBuilderAsNull(
            DbContextOptionsSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(IOptionsBuilder)))
                       .Returns(null);

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().BeNull();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldBeOfRequestedType_WhenContextResolvesOptionsBuilder(
           DbContextOptionsSpecimenBuilder builder,
           Mock<ISpecimenContext> contextMock,
           Mock<IOptionsBuilder> optionsBuilderMock)
        {
            optionsBuilderMock.Setup(x => x.Build(typeof(TestDbContext)))
                              .Returns(new DbContextOptions<TestDbContext>());

            contextMock.Setup(x => x.Resolve(typeof(IOptionsBuilder)))
                       .Returns(optionsBuilderMock.Object);

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().BeOfType<DbContextOptions<TestDbContext>>();
        }
    }
}
