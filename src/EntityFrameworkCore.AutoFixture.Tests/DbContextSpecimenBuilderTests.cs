using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests
{
    public class DbContextSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrowArgumentException_WhenSpecimenContextNull(
            DbContextSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(TestDbContext), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotDbContext(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            var actual = builder.Create(typeof(string), contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestIsPropertyInfo(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            var property = typeof(string).GetProperty(nameof(string.Length));
            var actual = builder.Create(property, contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenContextCanNotResolveOptions(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(DbContextOptions<TestDbContext>)))
                       .Returns(new NoSpecimen());

            var actual = builder.Create(typeof(TestDbContext), contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnOmitSpecimen_WhenContextSkipsOptionsResolution(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(DbContextOptions<TestDbContext>)))
                       .Returns(new OmitSpecimen());

            var actual = builder.Create(typeof(TestDbContext), contextMock.Object);

            actual.Should().BeOfType<OmitSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNull_WhenContextResolvesOptionsAsNull(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(DbContextOptions<TestDbContext>)))
                       .Returns(null);

            var actual = builder.Create(typeof(TestDbContext), contextMock.Object);

            actual.Should().BeNull();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldBeOfRequestedType_WhenContextResolvesOptions(
            DbContextSpecimenBuilder builder,
            Mock<ISpecimenContext> contextMock)
        {
            contextMock.Setup(x => x.Resolve(typeof(DbContextOptions<TestDbContext>)))
                       .Returns(new DbContextOptionsBuilder<TestDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options);

            var actual = builder.Create(typeof(TestDbContext), contextMock.Object);

            actual.Should().BeOfType<TestDbContext>();
        }
    }
}
