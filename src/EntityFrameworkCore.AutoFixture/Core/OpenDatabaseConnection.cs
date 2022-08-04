using System.Data.Common;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class OpenDatabaseConnection : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        var connection = Check.IsOfType<DbConnection>(specimen, nameof(specimen));

        connection.Open();
    }
}
