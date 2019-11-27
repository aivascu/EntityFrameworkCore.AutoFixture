using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Common;
using EntityFrameworkCore.AutoFixture.InMemory;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using FluentAssertions;
using Moq;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.InMemory
{
    public class InMemoryOptionsSpecimenBuilderTests
    {
        [Theory(DisplayName = "Create should create InMemoryOptionsBuilder instance when request type is IOptionsBuilder")]
        [AutoDomainData]
        public void Create_ShouldCreateInMemoryOptionsBuilder_WhenRequestTypeIsOptionsBuilderInterface(
            Mock<ISpecimenContext> context,
            InMemoryOptionsSpecimenBuilder builder)
        {
            var builderObj = builder.Create(typeof(IOptionsBuilder), context.Object);

            builderObj.Should().BeOfType<InMemoryOptionsBuilder>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrow_WhenSpecimenContextIsNull(InMemoryOptionsSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(IOptionsBuilder), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotOptionsBuilderInterface(
            Mock<ISpecimenContext> context,
            InMemoryOptionsSpecimenBuilder builder)
        {
            var obj = builder.Create(typeof(string), context.Object);

            obj.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Ctors_ShouldReceiveInitializedParameters(Fixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            var members = typeof(InMemoryOptionsSpecimenBuilder).GetConstructors();

            assertion.Verify(members);
        }
    }
}
