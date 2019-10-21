using System;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore
{
    public class InMemoryOptionsBuilder : AbstractOptionsBuilder
    {
        public InMemoryOptionsBuilder(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public InMemoryOptionsBuilder()
            : this(Guid.NewGuid().ToString())
        {
        }

        public string DatabaseName { get; }

        public override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(DatabaseName)
            .Options;
    }
}
