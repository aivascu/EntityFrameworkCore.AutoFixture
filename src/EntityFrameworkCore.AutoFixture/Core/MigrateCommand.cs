using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Executes the <see cref="M:DatabaseFacade.Migrate"/> method.
/// </summary>
public class MigrateCommand : ISpecimenCommand
{
    /// <summary>
    /// Exectues <see cref="M:DatabaseFacade.Migrate"/> if the specimen is a <see cref="DbContext"/>.
    /// </summary>
    /// <param name="specimen">The specimen.</param>
    /// <param name="context">The specimen context.</param>
    /// <exception cref="ArgumentNullException">Thrown when specimen is null.</exception>
    /// <exception cref="ArgumentException">Thrown when specimen is not <see cref="DbContext"/>.</exception>
    public void Execute(object specimen, ISpecimenContext context)
    {
        var dbContext = Check.IsOfType<DbContext>(specimen, nameof(specimen));

        dbContext.Database.Migrate();
    }
}
