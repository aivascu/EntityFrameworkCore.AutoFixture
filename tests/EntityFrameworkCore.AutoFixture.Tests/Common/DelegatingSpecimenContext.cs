using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Tests.Common;

public class DelegatingSpecimenContext : ISpecimenContext
{
    public Func<object, object> OnResolve { get; set; }

    public object Resolve(object request) => this.OnResolve?.Invoke(request);
}
