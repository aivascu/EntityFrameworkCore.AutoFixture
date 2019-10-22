using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public abstract class OptionsBuilder : IOptionsBuilder
    {
        protected abstract DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;

        public virtual object Build(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!typeof(DbContext).IsAssignableFrom(type) || type.IsAbstract)
                throw new ArgumentException($"The context type should be a non-abstract class inherited from {nameof(DbContext)}");

            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            var genericConfigureMethod = Array
                .Find(methods, m => m.Name == nameof(Build) && m.IsGenericMethodDefinition)?
                .MakeGenericMethod(type);

            return genericConfigureMethod?.Invoke(this, Array.Empty<object>());
        }
    }
}
