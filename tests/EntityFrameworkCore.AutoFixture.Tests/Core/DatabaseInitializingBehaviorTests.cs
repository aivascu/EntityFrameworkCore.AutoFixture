using System.Data.Common;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class DatabaseInitializingBehaviorTests
    {
        [Theory, MockData]
        public void ImplementsGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DatabaseInitializingBehavior));
        }

        [Fact]
        public void UsesCorrectNodeToTransform()
        {
            var builder = new DelegatingBuilder();
            var specification = new TrueRequestSpecification();
            var behavior = new DatabaseInitializingBehavior(specification);

            var node = behavior.Transform(builder);

            node.Should().BeOfType(typeof(DatabaseInitializingNode));
        }

        [Fact]
        public void NodeWrapsCorrectBuilder()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>().UseSqlite("Data Source=:memory:").Options;
            var dbcontext = new TestDbContext(options);
            var context = new DelegatingSpecimenContext();
            var builder = new DelegatingBuilder { OnCreate = (_, _) => dbcontext };
            var specification = new TrueRequestSpecification();
            var behavior = new DatabaseInitializingBehavior(specification);

            var node = behavior.Transform(builder);
            var actual = node.Create(typeof(DbConnection), context);

            Assert.Same(dbcontext, actual);
        }
    }
}
