using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using static System.FormattableString;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public abstract class OptionsBuilder : IOptionsBuilder
    {
        public abstract DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;

        public virtual object Build(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(DbContext).IsAssignableFrom(type) || type.IsAbstract)
            {
                throw new ArgumentException(Invariant($"The context type should be a non-abstract class inherited from {typeof(DbContext)}"), nameof(type));
            }

            var methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var genericConfigureMethod = Array
                .Find(methods, m => m.Name == nameof(Build) && m.IsGenericMethodDefinition)
                .MakeGenericMethod(type);

            return genericConfigureMethod.Invoke(this, Array.Empty<object>());
        }
    }
}
