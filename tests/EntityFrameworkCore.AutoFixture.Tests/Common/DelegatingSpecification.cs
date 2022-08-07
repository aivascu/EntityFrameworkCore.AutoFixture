using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class DelegatingSpecification : IRequestSpecification
{
    public Func<object, bool> OnIsSatisfiedBy { get; set; }

    public bool IsSatisfiedBy(object request)
    {
        return this.OnIsSatisfiedBy?.Invoke(request) ?? false;
    }
}
