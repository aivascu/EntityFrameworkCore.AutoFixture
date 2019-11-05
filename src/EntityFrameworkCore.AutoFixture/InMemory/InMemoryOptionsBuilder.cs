using System;
using EntityFrameworkCore.AutoFixture.Common;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    public class InMemoryOptionsBuilder : OptionsBuilder
    {
        public InMemoryOptionsBuilder(string databaseName)
        {
            DatabaseName = databaseName
                ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public InMemoryOptionsBuilder()
            : this(Guid.NewGuid().ToString())
        {
        }

        public string DatabaseName { get; }

        protected override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(DatabaseName)
            .Options;
    }
}
