using System;
using System.Data.Common;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Opens database connections.
/// </summary>
public class OpenDatabaseConnection : ISpecimenCommand
{
    /// <summary>
    /// Executes <see cref="M:DbConnection.Open()" /> on the specimen if it is a <see cref="DbConnection" /> instance.
    /// </summary>
    /// <param name="specimen">The specimen.</param>
    /// <param name="context">The specimen context.</param>
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen is DbConnection connection)
            connection.Open();
    }
}
