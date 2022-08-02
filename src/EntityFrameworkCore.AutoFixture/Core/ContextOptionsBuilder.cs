using System;
using System.Linq;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

public class ContextOptionsBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request is not Type type) throw new ArgumentException("Request should be a type.", nameof(request));

        var dbContextType = type.GetGenericArguments().Single();
        var builderRequest = typeof(DbContextOptionsBuilder<>).MakeGenericType(dbContextType);
        var result = context.Resolve(builderRequest);

        return result is DbContextOptionsBuilder builder
            ? builder.Options
            : result;
    }
}
