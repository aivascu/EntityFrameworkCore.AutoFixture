using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoFixture.AutoEFCore.Tests
{
    public class InMemoryConfigurationStrategyTests
    {
        [Fact]
        public void Test1()
        {
            var sut = new InMemoryOptionsBuilder();

            var options = sut.Build<TestDbContext>();
        }
    }

    public class TestDbContext : DbContext
    {
    }
}
