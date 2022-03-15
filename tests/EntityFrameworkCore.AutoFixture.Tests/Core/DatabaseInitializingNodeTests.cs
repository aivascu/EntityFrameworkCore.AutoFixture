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
    public class DatabaseInitializingNodeTests
    {
        [Theory, MockData]
        public void ImplementsGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DatabaseInitializingNode).GetConstructors());
        }

        [Fact]
        public void InvokesDatabaseCreation()
        {
            var delegateCalled = false;
            var dbcontext = new FakeDbContext();
            dbcontext.ActualFacade = new DelegatingDatabaseFacade(dbcontext)
            {
                OnEnusreCreated = () =>
                {
                    delegateCalled = true;
                    return true;
                }
            };
            var builder = new DelegatingBuilder { OnCreate = (_, _) => dbcontext };
            var node = new DatabaseInitializingNode(builder, new TrueRequestSpecification());

            var context = node.Create(typeof(FakeDbContext), new DelegatingSpecimenContext());

            Assert.True(delegateCalled);
        }

        [Fact]
        public void DoesNotInitializeDatabaseWhenInvalidRequest()
        {
            var delegateCalled = false;
            var dbcontext = new FakeDbContext();
            dbcontext.ActualFacade = new DelegatingDatabaseFacade(dbcontext)
            {
                OnEnusreCreated = () =>
                {
                    delegateCalled = true;
                    return true;
                }
            };
            var builder = new DelegatingBuilder { OnCreate = (_, _) => dbcontext };
            var node = new DatabaseInitializingNode(builder, new FalseRequestSpecification());

            var context = node.Create(typeof(FakeDbContext), new DelegatingSpecimenContext());

            Assert.False(delegateCalled);
        }

        [Fact]
        public void ReturnsCorrectNodesAsEnumerable()
        {
            var dbcontext = new FakeDbContext();
            dbcontext.ActualFacade = new DelegatingDatabaseFacade(dbcontext);
            var builder = new DelegatingBuilder { OnCreate = (_, _) => dbcontext };
            var node = new DatabaseInitializingNode(builder, new TrueRequestSpecification());

            var builders = ((IEnumerable)node).OfType<ISpecimenBuilder>().ToList();

            builders.Should().HaveCount(1).And.Contain(builder);
        }

        [Fact]
        public void ReturnsCorrectNodes()
        {
            var dbcontext = new FakeDbContext();
            dbcontext.ActualFacade = new DelegatingDatabaseFacade(dbcontext);
            var builder = new DelegatingBuilder { OnCreate = (_, _) => dbcontext };
            var node = new DatabaseInitializingNode(builder, new TrueRequestSpecification());

            var builders = node.ToList();

            builders.Should().HaveCount(1).And.Contain(builder);
        }
    }
}
