using System.Data.Common;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture;

public class OpenDatabaseConnection : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen is DbConnection connection)
        {
            connection.Open();
        }
    }
}
