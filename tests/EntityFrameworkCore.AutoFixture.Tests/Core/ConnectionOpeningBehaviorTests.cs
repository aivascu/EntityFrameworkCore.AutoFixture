using System.Data.Common;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class ConnectionOpeningBehaviorTests
    {
        [Theory, MockData]
        public void ImplementsGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConnectionOpeningBehavior));
        }

        [Fact]
        public void UsesCorrectNodeToTransform()
        {
            var behavior = new ConnectionOpeningBehavior(
                new TrueRequestSpecification());

            var node = behavior.Transform(new DelegatingBuilder());

            node.Should().BeOfType(typeof(ConnectionOpeningNode));
        }

        [Fact]
        public void NodeWrapsCorrectBuilder()
        {
            var connection = new FakeConnection();
            var builder = new DelegatingBuilder { OnCreate = (_, _) => connection };
            var behavior = new ConnectionOpeningBehavior(
                new TrueRequestSpecification());

            var node = behavior.Transform(builder);
            var actual = node.Create(typeof(DbConnection), new DelegatingSpecimenContext());

            Assert.Same(connection, actual);
        }
    }
}
