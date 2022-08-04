using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Executes the <see cref="M:DatabaseFacade.EnsureCreated"/> method.
/// </summary>
public class EnsureCreatedCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        var dbContext = Check.IsOfType<DbContext>(specimen, nameof(specimen));

        dbContext.Database.EnsureCreated();
    }
}
