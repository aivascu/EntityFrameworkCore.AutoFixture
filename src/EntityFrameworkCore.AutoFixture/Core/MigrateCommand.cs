using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Executes the <see cref="M:DatabaseFacade.Migrate" /> method.
/// </summary>
public class MigrateCommand : ISpecimenCommand
{
    /// <summary>
    /// Executes <see cref="M:DatabaseFacade.Migrate" /> if the specimen is a <see cref="DbContext" />.
    /// </summary>
    /// <param name="specimen">The specimen.</param>
    /// <param name="context">The specimen context.</param>
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen is DbContext dbContext)
            dbContext.Database.Migrate();
    }
}
