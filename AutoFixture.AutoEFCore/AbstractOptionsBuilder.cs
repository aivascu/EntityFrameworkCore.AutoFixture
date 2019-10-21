using System;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore
{
    public abstract class AbstractOptionsBuilder : IOptionsBuilder
    {
        public abstract DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;

        public virtual object Build(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!typeof(DbContext).IsAssignableFrom(type) || type.IsAbstract)
                throw new ArgumentException($"The context type should be a non-abstract class inherited from {nameof(DbContext)}");

            var genericConfigureMethod = GetType().GetMethod(nameof(Build)).MakeGenericMethod(type);

            return genericConfigureMethod.Invoke(this, Array.Empty<object>());
        }
    }
}
