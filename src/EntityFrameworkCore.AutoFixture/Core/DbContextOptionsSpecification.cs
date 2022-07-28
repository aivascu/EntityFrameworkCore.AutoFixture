using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

internal class DbContextOptionsSpecification : IRequestSpecification
{
    public bool IsSatisfiedBy(object request)
    {
        return request is Type { IsAbstract: false, IsGenericType: true } type
            && typeof(DbContextOptions<>) == type.GetGenericTypeDefinition();
    }
}
