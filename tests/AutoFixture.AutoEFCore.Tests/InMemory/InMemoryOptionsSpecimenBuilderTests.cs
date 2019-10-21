using System;
using AutoFixture.AutoEFCore.Common;
using AutoFixture.AutoEFCore.InMemory;
using AutoFixture.AutoEFCore.Tests.Common.Attributes;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests.InMemory
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
    }
}