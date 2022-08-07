using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Tests.Common;

public class DelegatingBuilder : ISpecimenBuilder
{
    public Func<object, ISpecimenContext, object> OnCreate { get; set; }

    public object Create(object request, ISpecimenContext context)
    {
        return this.OnCreate?.Invoke(request, context);
    }
}
