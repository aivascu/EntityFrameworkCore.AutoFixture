using System;
using AutoFixture.AutoEFCore.Common;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore.InMemory
{
    public class InMemoryOptionsBuilder : OptionsBuilder
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

        protected override DbContextOptions<TContext> Build<TContext>() => new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(DatabaseName)
            .Options;
    }
}
