using System;
using System.Data.Common;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class OpenDatabaseConnection : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen is null) throw new ArgumentNullException(nameof(specimen));
        if (specimen is not DbConnection connection)
            throw new ArgumentException(
                $"Expected request of type {typeof(DbConnection)}. Actual {specimen.GetType()}.");

        connection.Open();
    }
}
