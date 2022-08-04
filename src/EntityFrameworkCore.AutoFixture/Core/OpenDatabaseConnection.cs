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
    /// Exectues <see cref="M:DbConnection.Open()"/> on the specimen if it is a <see cref="DbConnection"/> instance.
    /// </summary>
    /// <param name="specimen">The specimen.</param>
    /// <param name="context">The specimen context.</param>
    /// <exception cref="ArgumentNullException">Thrown when specimen is null.</exception>
    /// <exception cref="ArgumentException">Thrown when specimen is not <see cref="DbConnection"/>.</exception>
    public void Execute(object specimen, ISpecimenContext context)
    {
        var connection = Check.IsOfType<DbConnection>(specimen, nameof(specimen));

        connection.Open();
    }
}
