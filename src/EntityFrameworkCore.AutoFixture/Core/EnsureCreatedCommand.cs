using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

public class EnsureCreatedCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen is null) throw new ArgumentNullException(nameof(specimen));
        if (specimen is not DbContext dbContext)
            throw new ArgumentException("Argument should be a DbContext instance", nameof(specimen));

        dbContext.Database.EnsureCreated();
    }
}
