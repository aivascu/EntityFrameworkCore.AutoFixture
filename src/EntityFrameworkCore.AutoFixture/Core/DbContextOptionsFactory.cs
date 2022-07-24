using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

public abstract class DbContextOptionsFactory : ISpecimenBuilder
{
    public virtual object Create(object request, ISpecimenContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request is not Type type) throw new ArgumentException("Argument should be a valid Type", nameof(request));

        return this.Create(type, context);
    }

    protected virtual object Create(Type type, ISpecimenContext context)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var contextType = type.GetGenericArguments().Single();
        if (!typeof(DbContext).IsAssignableFrom(contextType) || contextType.IsAbstract)
        {
            throw new ArgumentException(
                "The context type should be a non-abstract class inherited from {typeof(DbContext)}",
                nameof(type));
        }

        var methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var genericBuild = Array
            .Find(methods, m => m.Name == nameof(Create) && m.IsGenericMethodDefinition)
            .MakeGenericMethod(contextType);

        return genericBuild.Invoke(this, new object[] { context });
    }

    protected abstract DbContextOptions<TContext> Create<TContext>(ISpecimenContext context) where TContext : DbContext;
}
