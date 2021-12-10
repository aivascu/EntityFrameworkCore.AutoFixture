using System;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    /// <summary>
    /// Creates database context options.
    /// </summary>
    public class InMemoryOptionsBuilder : OptionsBuilder
    {
        /// <summary>
        /// Creates a <see cref="InMemoryOptionsBuilder"/> instance.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="databaseName"/> is null.</exception>
        public InMemoryOptionsBuilder(string databaseName)
        {
            this.DatabaseName = databaseName
                ?? throw new ArgumentNullException(nameof(databaseName));
        }

        /// <summary>
        /// Creates a <see cref="InMemoryOptionsBuilder"/> instance.
        /// </summary>
        public InMemoryOptionsBuilder()
            : this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; }

        /// <inheritdoc />
        public override DbContextOptions<TContext> Build<TContext>()
            => new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(this.DatabaseName)
            .Options;
    }
}
