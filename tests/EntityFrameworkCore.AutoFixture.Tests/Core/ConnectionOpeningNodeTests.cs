using System.Collections;
using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class ConnectionOpeningNodeTests
    {
        [Theory, MockData]
        public void ImplementsGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConnectionOpeningNode).GetConstructors());
        }

        [Fact]
        public void InvokesDatabaseCreation()
        {
            var openCalled = false;
            var connection = new FakeConnection { OnOpen = () => openCalled = true };
            var builder = new DelegatingBuilder { OnCreate = (_, _) => connection };
            var node = new ConnectionOpeningNode(builder, new TrueRequestSpecification());

            var context = node.Create(typeof(FakeConnection), new DelegatingSpecimenContext());

            Assert.True(openCalled);
        }

        [Fact]
        public void DoesNotInvokeConnectionWhenInvalidRequest()
        {
            var openCalled = false;
            var connection = new FakeConnection { OnOpen = () => openCalled = true };
            var builder = new DelegatingBuilder { OnCreate = (_, _) => connection };
            var node = new ConnectionOpeningNode(builder, new FalseRequestSpecification());

            var context = node.Create(typeof(FakeConnection), new DelegatingSpecimenContext());

            Assert.False(openCalled);
        }

        [Fact]
        public void ReturnsCorrectNodesAsEnumerable()
        {
            var connection = new FakeConnection();
            var builder = new DelegatingBuilder { OnCreate = (_, _) => connection };
            var node = new ConnectionOpeningNode(builder, new TrueRequestSpecification());

            var builders = ((IEnumerable)node).OfType<ISpecimenBuilder>().ToList();

            builders.Should().HaveCount(1).And.Contain(builder);
        }

        [Fact]
        public void ReturnsCorrectNodes()
        {
            var connection = new FakeConnection();
            var builder = new DelegatingBuilder { OnCreate = (_, _) => connection };
            var node = new ConnectionOpeningNode(builder, new TrueRequestSpecification());

            var builders = node.ToList();

            builders.Should().HaveCount(1).And.Contain(builder);
        }
    }
}
