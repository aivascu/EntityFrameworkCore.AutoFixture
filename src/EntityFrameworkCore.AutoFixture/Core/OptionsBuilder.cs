using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using static System.FormattableString;

namespace EntityFrameworkCore.AutoFixture.Core
{
    /// <summary>
    /// Creates database options builders for a <see cref="DbContext"/> type.
    /// </summary>
    public abstract class OptionsBuilder : IOptionsBuilder
    {
        /// <summary>
        /// Builds a database context options instance for a <paramref name="type"/>.
        /// </summary>
        /// <typeparam name="TContext">the database context type.</typeparam>
        /// <returns>Returns a <see cref="DbContextOptions{TContext}"/> instance,
        /// casted to <see cref="object" />.</returns>
        public abstract DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;

        /// <inheritdoc />
        public virtual object Build(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!typeof(DbContext).IsAssignableFrom(type) || type.IsAbstract)
            {
                throw new ArgumentException(
                    Invariant($"The context type should be a non-abstract class inherited from {typeof(DbContext)}"),
                    nameof(type));
            }

            var methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var genericConfigureMethod = Array
                .Find(methods, m => m.Name == nameof(Build) && m.IsGenericMethodDefinition)
                .MakeGenericMethod(type);

            return genericConfigureMethod.Invoke(this, Array.Empty<object>());
        }
    }
}
