using System;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public interface IOptionsBuilder
    {
        /// <summary>
        /// Builds a database context options instance for a <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The database context type.</param>
        /// <returns>Returns a <see cref="Microsoft.EntityFrameworkCore.DbContextOptions{TContext}"/> instance,
        /// casted to <see cref="object" />.</returns>
        object Build(Type type);
    }
}
