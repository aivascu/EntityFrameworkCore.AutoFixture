using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

public class MigrateCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        var dbContext = Check.IsOfType<DbContext>(specimen, nameof(specimen));

        dbContext.Database.Migrate();
    }
}
